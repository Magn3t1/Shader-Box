#version 330 core

#define M_PI 3.1415926535897932384626433832795

#define NB_LAYERS 10.0

out vec4 FragColor;

in vec2 UV;

uniform float time;

uniform vec2 screenSize;
uniform float xRatio;

uniform int mode;

uniform float Zoom;
uniform float X;
uniform float Y;

//Hash function found on the internet
float Hash21(vec2 p) {
	p = fract(p*vec2(234.34, 435.345));
    p += dot(p, p+34.23);
    return fract(p.x*p.y);
}

//Same as Hash21, found on the shader book
float random (in vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,78.233))) * 43758.5453123);
}

vec2 Hash22(vec2 p){
	float n = Hash21(p);
	return vec2(n, Hash21(p + n));
}

//Link to this function http://paulbourke.net/miscellaneous/interpolation/
float cosineInterpolate(float y1, float y2, float mu){
   
   float mu2;

   mu2 = (1-cos(mu * M_PI))/2;
   
   return(y1*(1-mu2)+y2*mu2);
}

//2D noise using the random function
float noise (in vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + vec2(1.0, 0.0));
    float c = random(i + vec2(0.0, 1.0));
    float d = random(i + vec2(1.0, 1.0));

    // Smooth Interpolation

    // Cubic Hermine Curve.  Same as SmoothStep()
    vec2 u = f*f*(3.0-2.0*f);
    // u = smoothstep(0.,1.,f);

    // Mix 4 coorners percentages
    return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}


void main(){

	const float speed = 0.1;

	float t = time*0.1;

	vec2 coord = vec2(UV.x - 0.5, UV.y - 0.5);
	//vec2 coord = UV;
	coord.x *= xRatio;

	///Controles
	//Zoomer
	//coord *= cosineInterpolate(0, 10, cos(time/5)) + 5;
	coord *= Zoom;

	float s = sin(t);
	float c = cos(t);
	mat2 roation = mat2(c, -s, s, c);

	coord *= roation;
	
	//Avancer
	//coord.y += time*speed;
	//coord.x += cos(time/2)*speed;

	vec2 offset = vec2(X, Y) * roation;
	///Fin controles


	vec3 color = vec3(0);


	float value = 0;

	float test1 = smoothstep(0.30*abs(cos(t*3)), 0.4*abs(cos(t*3)), noise(coord * 10.0 + vec2(t*1.1234, -t*1.1347) + offset*0.8)); // éclaboussure noire
    float test2 = smoothstep(0.4*abs(cos(t*1.78)), 0.35*abs(cos(t*1.78)), noise(coord * 10.0 + vec2(t*1.24368, t*1.1234) + offset*0.9)); // 'trou' dans l'éclaboussure
    float test3 = smoothstep(0.8*abs(cos(t*2.78)), 0.9*abs(cos(t*2.78)), noise(coord * 5.0 + vec2(-t*1.3457, t*1.1243) + offset*1 )); // éclaboussure noire
    float test4 = smoothstep(0.7*abs(cos(t*2.38)), 0.8*abs(cos(t*2.38)), noise(coord * 8.0 + vec2(-t*1.1457, -t) + offset*1.1)); // éclaboussure noire

    value = test1 - test2 + test3 - test4;

    float tempo1 = smoothstep(0.35, 0.45, value) * smoothstep(0.65, 0.55, value);// * smoothstep(0.7, 0.8, value);
    //float tempo2 = smoothstep(0.6, 0.7, value) * smoothstep(0.9, 0.8, value);


    if(mode == 1){
    	value = tempo1;
    }

	vec3 sceneColor = sin(t*20 * vec3(0.339, 0.414, 0.532)) * 0.4 + 0.6;

	color += value * sceneColor;

	FragColor = vec4(color, 1);

}