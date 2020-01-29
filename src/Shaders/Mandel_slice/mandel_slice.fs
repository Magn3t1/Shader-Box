#version 330 core
out vec4 FragColor;

in vec2 UV;

uniform float time;
uniform float X;
uniform float Y;
uniform float Z;
uniform float Zoom;
uniform vec2 screenSize;
uniform float xRatio;
uniform int mode;

/*Nombre de fractale de Mandel*/
float power = 10;


mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

mat4 rotation3d(vec3 axis, float angle) {
  axis = normalize(axis);
  float s = sin(angle);
  float c = cos(angle);
  float oc = 1.0 - c;

  return mat4(
		oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
    oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
    oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
		0.0,                                0.0,                                0.0,                                1.0
	);
}

void main()
{	
	/*On ajuste la position de l'ecran pour pas que la fractale soit dans l'angle en bas a gaucvhe en fonction du zoom*/
	vec3 position = (vec3(UV.x,UV.y, 0.5)-0.5);
	
	position.x *= xRatio;

	//position *= 5;

	//Add The Zoom 
	//position /= (1000)/10.0;



	position.xy *= (Zoom);
	//position.xy -= 3;
	
	//Add the X and Y Offset
	position += vec3(X, Y,Z/100);
position.xy*=rotate2d(time* 0.035);
	vec3 z = position;

	float dr = 1.0;
	float r = 0.0;
    int iterations = 0;
	power+=1;

	for (int i = 0; i < 15 ; i++) {
        iterations = i;
		r = length(z);

		// Conversion en coordonnées polaires
		float theta = acos(z.z/r);
		float phi = atan(z.y,z.x);
		dr =  pow( r, power-1.0)*power*dr + 1.0;

		// Calcul du nouveau Z, du scale et de la rotation de la fractale en fonction du nombres de fractale de Mandelbrot
		float zr = pow(r,power);
		theta = theta*power;
		phi = phi*power;
		
		// Conversion en coordonnées cartesiennes
		z = zr*vec3(sin(theta)*cos(phi), sin(phi)*sin(theta), cos(theta));

		/*On fait varier notre complex avec le temps*/
		z+=position*cos(time*0.203);


		/*Definition de l'infini (la limite)*/
		if(r>3) {
			/*Effet visuel sympa*/
			FragColor = vec4(
				(atan(z.z - time*0.0103))/6,
				cos(z.y - time*0.403)/6,
				cos(z.x - time*0.243)/50,
				1.0
				);
            break;
        }

		
		/*Effet visuel sympa*/
		FragColor = vec4(
				(((cos(z.z - time*0.103)))*(dr)/atan(time))/abs(sin(phi)*theta),///((cos(z.z - time*0.103)))
				((cos(z.y - time*0.403)))/dr,
				(((cos(z.x - time*0.0103))*cos(dr))/*-(cos(z.x - time*0.0103))/abs(tan(theta))*abs(tan(phi))*/),
				1.0
				) * rotation3d(vec3(1,1,1), time*0.2);
	}
}