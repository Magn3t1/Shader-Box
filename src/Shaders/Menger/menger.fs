/*Author Etienne PENAULT*/
/*TODO - REFLEXIONS*/
/*TODO - MORE GENERIC CODE*/

#version 330 core

out vec4 FragColor;
in vec2 UV;

uniform float time;
uniform float X;
uniform float Y;
uniform float Z;
uniform float Zoom;
uniform vec2 screenSize;
uniform float Yaw;
uniform float Pitch;
uniform float launch;
uniform float xRatio;


#define MAX_STEPS 100
#define MAX_DISTANCE 100.0
#define SURFACE_DISTANCE 0.01 //Quality of details
#define PI 3.1415926535897932384626433832795

#define BINDING_KEY

mat2 rotation(float f){
	float s = sin(f);
	float c = cos(f);
	return mat2(c,-s,s,c);
}

float sdSphere(vec3 point, vec3 size, float radius){
	return length(point - size) - radius;
}

float sdSphereHollow(vec3 point, vec3 size, float radius){
	return abs(length(point) - radius);
}

float sdCapsule(vec3 point, vec3 a, vec3 b, float radius){ //a & b are both center of two spheres taht make capsule
	vec3 a_b = b - a;
	vec3 a_point = point - a;

	float t = dot(a_b,a_point)/dot(a_b,a_b);
	t = clamp(t,0.0,1.0);

	vec3 c = a + t * a_b;
	return length(point - c)-radius;
}

float sdTorus(vec3 point, vec2 radius){//vec2 radius for the big and the small one(empty)
	float x = length(point.xz) - radius.x;
	return (length(vec2(x,point.y)) - radius.y);
}

 
float sdBox(vec3 point, vec3 size){
  vec3 distance = abs(point) - size;
  return min(max(distance.x,max(distance.y,distance.z)),0.0) + length(max(distance,0.0));
}

float sdBoxHollow(vec3 point, vec3 size){
  vec3 distance = abs(point) - size;
  return abs(min(max(distance.x,max(distance.y,distance.z)),0.0) + length(max(distance,0.0)));
}

float sdCylinder(vec3 p, vec3 a, vec3 b, float r) {
	vec3 ab = b-a;
    vec3 ap = p-a;
    
    float t = dot(ab, ap) / dot(ab, ab);
    
    vec3 c = a + t*ab;
    
    float x = length(p-c)-r;
    float y = (abs(t-.5)-.5)*length(ab);
    float e = length(max(vec2(x, y), 0.));
    //very cost performance but don't show buggy shadows
    float i = min(max(x, y), 0.);
    
    return e+i;
}

vec4 booleanSubtraction(vec4 pointA, vec4 pointB){
	return (-pointA.w >pointB.w) ? -pointA : pointB;
}

float booleanIntersection(float pointA, float pointB){
	return max(pointA,pointB);
}

vec4 booleanUnion(vec4 pointA, vec4 pointB){
	return (pointA.w <pointB.w) ? pointA : pointB;
}

/*smooth boolean union*/
vec4 smin(vec4 pointA, vec4 pointB, float k){
	float h = clamp(0.5+0.5*(pointB.w - pointA.w)/k,0.0,1.0);
	vec3 color = mix(pointB.xyz,pointA.xyz,h);
	float dist = mix(pointB.w,pointA.w,h) - k*h*(1.0-h);
	return vec4(color,dist);
}


float mengerSponge(vec3 p, int iteration){
  // float d = sdBox(p,vec3(1.0));
   float d = sdBox(p,vec3(1));

   float s = 1.0;
   for( int m=0; m<iteration; m++ )
   {
      vec3 a = mod( p*s, 2.0 )-1.0;
      s *= 3.0;
      vec3 r = abs(sin(time)*1.0 - 3.0*abs(a*sin(time)));



      float da = max(r.x,r.y);
      float db = max(r.y,r.z);
      float dc = max(r.z,r.x);
      float c = (min(da,min(db,dc))-1.0)/s;
      


      d = max(d,c);
   }
   return d;
}

vec4 getDist(vec3 point){
	//floor
	vec4 planeDist = vec4(0.855, 0.439, 0.839,point.y);


	vec3 m = point- vec3(0,2.25,6.25);
	m.yz*=rotation(time*0.27);
	m.xz*=rotation(time*0.24);
	m.xy*=rotation(time*0.124);
	vec4 menger = vec4(0.85,1.0,0,mengerSponge(m,3));


	//vec3 spherePoint2 = point - vec3(-1,1.25,launch-3);
	vec3 spherePoint = point - vec3(launch-18,1.25,5.25);
	vec4 sphereDistance = vec4(0,0,1,sdSphere(spherePoint,vec3(1),0.25));

	vec3 boxShere = point- vec3(0,cos(PI+time*2.09),6.25);
	boxShere.xz*=rotation(time*2);
	boxShere-= vec3(2,0,0);
	vec4 box = vec4(0.0,1,0.0,sdBox(boxShere,vec3(0.2)));

	vec3 boxShere2 = point- vec3(0,cos(time*2.09),6.25);
	boxShere2.xz*=rotation(time*2);
	boxShere2-= vec3(-2,0,0);
	vec4 box2 = vec4(0.0,1,0.0,sdBox(boxShere2,vec3(0.2)));


	vec4 fusion = smin(menger, sphereDistance,3);
	vec4 fusion2 = smin(planeDist, box ,1);
	fusion2 = smin(fusion2, box2 ,1);
	vec4 totalDist = booleanUnion(fusion,fusion2);

	return totalDist;
}

float rayMarch(vec3 rayOrigin, vec3 rayDirection, inout vec3 dColor){
	float distanceToTheOrigin = 0.0;
	
	for(int i = 0; i < MAX_STEPS; ++i){
		vec3 currentMarchingPoint = rayOrigin + rayDirection * distanceToTheOrigin;
		vec4 distanceToTheScene = getDist(currentMarchingPoint);
		distanceToTheOrigin += distanceToTheScene.w;
		dColor = distanceToTheScene.xyz;
		if( (distanceToTheOrigin > MAX_DISTANCE) || distanceToTheScene.w < SURFACE_DISTANCE ){
			break;
		}
	}
	return distanceToTheOrigin;
}

vec3 getNormal(vec3 point){
	float distance = getDist(point).w;
	vec2 e = vec2 (0.01,0);

	vec3 normal = distance - vec3(
			getDist(point-e.xyy).w,
			getDist(point-e.yxy).w,
			getDist(point-e.yyx).w
		);

	return normalize(normal);
}

float getLight(vec3 pointToShade, vec3 lightPosition, vec3 dColor){
	vec3 ligthVector = normalize(lightPosition - pointToShade);
	vec3 normal = getNormal(pointToShade);
	float diffuseLight = clamp(dot(normal,ligthVector),0.0,1.0);
	/*SHADOWS*/
		float distance = rayMarch(pointToShade + normal*SURFACE_DISTANCE*2.0,ligthVector, dColor);
		if(distance<length(lightPosition - pointToShade))
			diffuseLight *= 0.1;

	return diffuseLight;
}

void main(){
	#ifdef BINDING_KEY
		float X = X;
		float Y_ = Z;
		float Z_ = Y;
	#endif

	vec3 dColor;
		//uv to the middle
		//vec2 uv = (fragCoord.xy)*screenSize/screenSize.y;
		vec2 uv = vec2(UV.x, UV.y) - 0.5;
		uv *= Zoom;
		uv.x *= xRatio;
		vec3 rayOrigin = vec3(0+X,2+Y_,-1+Z_);
		vec3 rayDirection = normalize(vec3(uv.x, uv.y,1));

	float distance = rayMarch(rayOrigin,rayDirection,dColor);

	vec3 point = rayOrigin + rayDirection * distance;
	float diffuseLight = getLight(point,vec3(0,2.25,6.25),dColor);
	diffuseLight += getLight(point,vec3(0+X,2+Y_,-1+Z_),dColor) /** getLight(point,vec3(0,8,6.25),dColor)*/;
	vec3 color =vec3(diffuseLight) * dColor;

	if(distance > 100){
		color = vec3(0.3,0.4/point.y*2,0.8/point.y);
	}

	FragColor = vec4(color,1.0);
}