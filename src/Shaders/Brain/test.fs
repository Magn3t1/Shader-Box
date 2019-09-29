#version 330 core

#define M_PI 3.1415926535897932384626433832795



#define NB_LAYERS 3.0

out vec4 FragColor;

in vec2 UV;

uniform float time;

uniform vec2 screenSize;
uniform float xRatio;

uniform float Zoom;
uniform float X;
uniform float Y;

//Hash function found on the internet
float Hash21(vec2 p) {
	p = fract(p*vec2(234.34, 435.345));
    p += dot(p, p+34.23);
    return fract(p.x*p.y);
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






float distLine(vec2 point, vec2 a, vec2 b){
	vec2 pa = point - a;
	vec2 ba = b - a;
	float t = clamp(dot(pa, ba)/dot(ba, ba), 0.0, 1.0);
	return length(pa - ba * t);
}

float drawLine(vec2 point, vec2 a, vec2 b){
	float d = distLine(point, a, b);
	float m = smoothstep(0.03, 0.01, d);

	float d2 = length(b - a);

	m *= smoothstep(1.2, 0.8, d2) * 0.5 + smoothstep(0.05, 0.03, abs(d2 - 0.75));
	return m;
}

vec2 getPos(vec2 id, vec2 offset, float t){

	//vec2 noise = Hash22(id + offset) * time;

	float n = Hash21(id+offset);
	float n1 = fract(n*10.0);
    float n2 = fract(n*100.0);
    float a = t + n;
    return offset + vec2(sin(a*n1), cos(a*n2)) * 0.4;

	// float x = sin(time * noise.x);
	// float y = cos(time * noise.y);

	//return offset + sin(noise) * 0.37;

}

float brain(vec2 coord, float index, float t){

	float m = 0;

	vec2 dividedCoord = fract(coord) - 0.5;
	
	vec2 originalDividedCoord = dividedCoord;
	vec2 id = floor(coord);
	

	vec2 neighbour[9];
	int i = 0;
	for(float y = -1.0; y <= 1.0; ++y){
		for(float x = -1.0; x <= 1.0; ++x){
			neighbour[i++] = getPos(id, vec2(x, y), t); 
		}		
	}


	float sparkle = 0;
	for(int i = 0; i < 9; ++i){
		m += drawLine(dividedCoord, neighbour[4], neighbour[i]);

		float dist_Coord_i = length(neighbour[i] - dividedCoord);

		//float tempoSparkle = 1.0/dot(dist_Coord_i, dist_Coord_i);
		float tempoSparkle = (0.005 / (dist_Coord_i * dist_Coord_i));
		tempoSparkle *= smoothstep(1.0, 0.7, dist_Coord_i);

		float pulse = sin((fract(neighbour[i].x) + fract(neighbour[i].y) + t) * 5.0) * 0.4 + 0.6;
		pulse = pow(pulse, 20.0);

		tempoSparkle *= pulse;
		sparkle += tempoSparkle;
	}

	m += drawLine(dividedCoord, neighbour[1], neighbour[3]);
	m += drawLine(dividedCoord, neighbour[1], neighbour[5]);
	m += drawLine(dividedCoord, neighbour[7], neighbour[3]);
	m += drawLine(dividedCoord, neighbour[7], neighbour[5]);

	float sPhase = (sin(t + index) + sin(t * 0.1)) * 0.25 + 0.5;
    sPhase += pow(sin(t * 0.1) * 0.5 + 0.5, 50.0) * 5.0;
    m += sparkle*sPhase;//(*.5+.5);

	return m;

}


void main(){

	vec2 coord = vec2(UV.x - 0.5, UV.y - 0.5);
	//vec2 coord = UV;
	coord.x *= xRatio;

	

	//Zoomer
	//coord *= cosineInterpolate(0, 10, cos(time/5)) + 5;
	
	coord *= Zoom;
	
	//Avancer
	//coord.y += time*speed;
	//coord.x += cos(time/2)*10;
	
	 coord.y += Y/5.0f;
	 coord.x += X/5.0f;

	float gradient = coord.y;

	vec3 color = vec3(0);

	//float gradient = UV.y;

	float m = 0;



	float t = time*0.1;

	float s = sin(t);
	float c = cos(t);
	mat2 roation = mat2(c, -s, s, c);
	vec2 offset = vec2(X, Y);

	coord *= roation;
	offset *= roation;

	for(float i = 0.0; i < 1; i += 1.0/NB_LAYERS){

		float depth = fract(i + t);
		float size = mix(15.0, 1.0, depth);

		float fade = smoothstep(0.0, 0.6, depth) * smoothstep(1.0, 0.8, depth);

		m += brain(coord * size + i * 20.0, i, time) * fade;

	}

	vec3 sceneColor = sin(t*20 * vec3(0.339, 0.414, 0.532)) * 0.4 + 0.6;

	color += sceneColor * m;

	color -= gradient * 0.2 * sceneColor;



	FragColor = vec4(color, 1);

}