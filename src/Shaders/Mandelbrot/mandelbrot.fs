/*Author Etienne PENAULT*/
/*		PARIS VIII		*/

/*TODO - MORE GENERIC CODE				*/
/*TODO - FIX THE ZOOM OFFSET			*/
/*TODO - INCREASE THE MAXIMUM RESOLUTION*/

#version 330 core

/*Define Julia for Julia fractal*/
//#define Julia
/*Define Move for a moving fractal, better to use with Julia's one which is symmetrical*/
//#define Move

out vec4 FragColor;

in vec2 UV;

uniform float time;
uniform float X;
uniform float Y;
uniform float Zoom;
uniform vec2 screenSize;
uniform float xRatio;
uniform int mode;

float iteration = 200;
float scale 	= 400;

	float realJulia 		= -0.7269/*-0.8*/;
	float imaginaryJulia	=  0.1889/*0.156*/;

void main(){

    vec2 zTmp, z, cTmp;
	float border;
	vec4 finalColor;
	int i;

	//We get the FragCoord and we divide it bit the viewport size to normalize the coord then we center them by removing 0.5
	vec2 c = UV - 0.5;

	//Go Back to screenCoord Space
	c.x *= xRatio;

	c *= 5;

	//Add The Zoom 
	//c /= (1000)/10.0;



	c *= Zoom;

	//Add the X and Y Offset
	c += vec2(X, Y);
	

    z 			= c;
	cTmp		= c;

	/*Replace our complex component by our Julia values*/
	if(mode == 2 || mode == 3){
		z.x 	   +=0.5;
	    cTmp.x 	   +=0.5;
		cTmp 		= vec2(realJulia,imaginaryJulia);
	}

	/*Multiply every part of our complex by a number between [-1:1] in function of the time*/
	if(mode == 1 || mode == 3){
		cTmp.x	   *= cos(time*0.02);
		cTmp.x	   += sin(time*0.05);
		cTmp.y	   *= sin(time*0.06);
		cTmp.y	   += cos(time*0.08);
	}


	for(i =0; i< iteration; i++){

		/*Infinity limit*/
		if((z.x*z.x) + (z.y*z.y) > 20){
			break;
		}

		/*Mandelbrot formula*/
		float x = z.x*z.x - z.y*z.y;
		float y = 2 * z.x * z.y;

		zTmp.x = x + cTmp.x;
		zTmp.y = y + cTmp.y;		

		z = zTmp;			
	}


	/*Color change by the time and the actual complexs*/
	finalColor = vec4(
						(i == iteration ? 0.0 : float(i*cos(time*0.5)/10)),
						(i == iteration ? 0.0 : float(i)) / 70.0,
						cos(sqrt(float(i)/iteration)*time)/2,
						1
					);

	FragColor = finalColor;
}