#version 330 core

out vec4 FragColor;

in vec2 UV;

uniform float time;
uniform float xRatio;

void main(){

	vec2 coord = vec2(UV.x * xRatio, UV.y);

	coord = normalize(coord);



	//coord *= abs(cos(time/5.0))*5;
	coord *= 5;

	vec2 divededCoord = fract(coord) - 0.5;

	FragColor = vec4(0, 0, 0, 1);
	if(divededCoord.y > 0.48) FragColor = vec4(1, 0, 0, 1);

}