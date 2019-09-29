#version 330 core

#define M_PI 3.1415926535897932384626433832795

#define MODE 0

#define NB_LAYERS 10.0

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


float circle(vec2 coord, float depth, int index, float t){

	float m = 0;

	vec2 originealCoord = coord;

	if(MODE == 0){
		coord.x += (cos(t * Hash21(vec2(index*326, index*219)))*0.8 +0.2) * 50;
		coord.y += (sin(t * Hash21(vec2(index, index)))*0.8 + 0.2) * 50;
	}
	else if(MODE == 1){
		coord.x += (cos(t * Hash21(vec2(index*326, index*219)))*0.8 +0.2) * 30;
		coord.y += (cos(t * Hash21(vec2(index, index)))*0.8 + 0.2) * 30;
	}
	else if(MODE == 2){
		coord.x += (tan(t * Hash21(vec2(index*326, index*219)))*0.8 +0.2) * 30;
		coord.y += (tan(t * Hash21(vec2(index, index)))*0.8 + 0.2) * 30;
	}

	

	coord.x = cosineInterpolate(coord.x, originealCoord.x, depth);
	coord.y = cosineInterpolate(coord.y, originealCoord.y, depth);

	float size = 0.12;
	//m += smoothstep(0.4, 0.39, length(coord)) * smoothstep(0.25, 0.26, length(coord) - size);


	float dist_Coord_i = length(coord - normalize(coord)*0.5);
	//dist_Coord_i += length(coord - normalize(coord + 10)*0.5);

	//float tempoSparkle = 1.0/dot(dist_Coord_i, dist_Coord_i);
	float sparkle = (0.005 / (dist_Coord_i * dist_Coord_i));
	sparkle *= smoothstep(1.0, 0.7, dist_Coord_i);

	// float pulse = sin((fract(neighbour[i].x) + fract(neighbour[i].y) + t) * 5.0) * 0.4 + 0.6;
	// pulse = pow(pulse, 20.0);

	// tempoSparkle *= pulse;
	//sparkle += tempoSparkle;

	float sPhase = (sin(t + index) + sin(t * 0.1)) * 0.25 + 0.5;
    sPhase += pow(sin(t * 0.1) * 0.5 + 0.5, 50.0) * 5.0;
    m += sparkle*sPhase;//(*.5+.5);


	return m;

}


void main(){

	vec2 coord = vec2(UV.x - 0.5, UV.y - 0.5);
	//vec2 coord = UV;
	coord.x *= xRatio;

	///Controles
	//Zoomer
	//coord *= cosineInterpolate(0, 10, cos(time/5)) + 5;
	coord *= Zoom;
	
	//Avancer
	//coord.y += time*speed;
	//coord.x += cos(time/2)*10;
	//coord.y += Y/5.0f;
	//coord.x += X/5.0f;
	///Fin controles

	float gradient = coord.y;

	vec3 color = vec3(0);

	float m = 0;
	float t = time * 0.1;

	float test = 0;

	
	//float i = 0.0;

	if(coord.x < 0){

		coord.x *= -1.0;

	}
	if(coord.y < 0){
		coord.y *= -1.0;
	}


	float nbLayer = 30.0;

	//float t = time * 10;

	for(int i = 0; i < nbLayer; ++i){

		float iFract = i/nbLayer;


		float depth = fract(iFract + t);
		float size = mix(80.0, 0.1, depth);

		float fade = smoothstep(0.0, 0.2, depth) * smoothstep(1.0, 0.8, depth);

		m += circle(coord * size + vec2(X, Y), depth, i, time) * fade;

	}


	vec3 sceneColor = sin(t*20 * vec3(0.339, 0.414, 0.532)) * 0.4 + 0.6;

	color += sceneColor * m;

	color -= gradient * 0.2 * sceneColor;


	color += m;

	FragColor = vec4(color, 1);

}