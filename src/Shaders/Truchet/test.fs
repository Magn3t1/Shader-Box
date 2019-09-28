#version 330 core

#define M_PI 3.1415926535897932384626433832795

out vec4 FragColor;

in vec2 UV;

uniform float time;
uniform float xRatio;

//Hash function found on the internet
float Hash21(vec2 p) {
	p = fract(p*vec2(234.34, 435.345));
    p += dot(p, p+34.23);
    return fract(p.x*p.y);
}

//Link to this function http://paulbourke.net/miscellaneous/interpolation/
float cosineInterpolate(float y1, float y2, float mu){
   
   float mu2;

   mu2 = (1-cos(mu * M_PI))/2;
   
   return(y1*(1-mu2)+y2*mu2);
}

void main(){

	float speed = 2;
	vec2 coord = vec2(UV.x - 0.5, UV.y - 0.5);
	//vec2 coord = UV;
	
	coord.x *= xRatio;

	//coord -= 0.5;


	//coord = normalize(coord);

	//coord = vec2(coord.x / (1*xRatio), coord.y / 1);



	//Zoomer
	//coord *= abs(sin(time/5.0)) * 10 + 3;
	coord *= cosineInterpolate(0, 10, cos(time/5)) + 5;
	//coord *= 5;


	//Avancer
	coord.y += time*speed;
	coord.x += cos(time/2)*10;
	



	vec2 divededCoord = fract(coord) - 0.5;
	vec2 id = floor(coord);
	float randomValue = Hash21(id);


	if(randomValue < 0.5) divededCoord.x *= -1.0;


	vec3 color = vec3(0);

	///Select your size calculation methode :

	//float size = abs(cos(time/5.0))*0.5;
	float size = 0.65 * ( sqrt(2)/2.0 - length(UV - 0.5) );
	//float size = cosineInterpolate(0, 0.5, cos(time/3.678));
	//float size = 0.5 * UV.x;

	float dist = abs(abs(divededCoord.y + divededCoord.x) - 0.5);
	color += vec3(smoothstep(0.01, -0.01, dist - size));
	//vec3 color = vec3(clamp(-0.01, 0.01, abs(divededCoord.y + divededCoord.x) - size));

	color -= vec3(abs(cos(time/1.7834)), abs(cos(time/1.6234)), abs(cos(time/1.8973)));

	//if(abs(divededCoord.y + divededCoord.x) - size < 0.01) color = vec3(1, 1, 1);

	//if(color.x == 0) color = vec3(abs(cos(time/3.5)), abs(cos(time/3.8)), abs(cos(time/3.23)));

	//color = vec3();

	FragColor = vec4(color, 1);
	//if(divededCoord.x > 0.48 || divededCoord.y > 0.48) FragColor = vec4(1, 0, 0, 1);

}