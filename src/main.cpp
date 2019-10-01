
//OpegnGl includes

#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <filesystem>

#include <iostream>

//Functions Headers
#include "event.hpp"

//Classes Headers
#include "GlobalVariable.hpp"
#include "ShadersStorage.hpp"


//Test
#include "Quad.hpp"
//#include "Shader.hpp"

#define INITIAL_WIDTH 1200
#define INITIAL_HEIGHT 700



void processInput(GLFWwindow *window);

int main(){

    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    
    #ifdef __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
	#endif
  
	

    GLFWwindow* window = glfwCreateWindow(INITIAL_WIDTH, INITIAL_HEIGHT, "Shaders-Box", NULL, NULL);
	if (window == NULL)
	{
	    std::cout << "Failed to create GLFW window" << std::endl;
	    glfwTerminate();
	    return -1;
	}
	glfwMakeContextCurrent(window);

	
	//FPS 60
    glfwSwapInterval(1);

	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    glfwSetKeyCallback(window, key_callback);




	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
    	std::cout << "Failed to initialize GLAD" << std::endl;
    	return -1;
	}

	{

		GLint vp[4];
		glGetIntegerv( GL_VIEWPORT, vp);
		

		GlobalVariable::windowWidth_ = vp[2];
   		GlobalVariable::windowHeight_ = vp[3];

	}

	Quad monCarre;
	//Shader monShader("Shaders/default.vs", "Shaders/mandel.fs");
	ShadersStorage monStorage;

	GlobalVariable::mainShadersStoragePointer_ = &monStorage;

	monStorage.addShader("Shaders/default.vs", "Shaders/Noise/noise.fs");
	monStorage.addShader("Shaders/default.vs", "Shaders/CircleShine/circleShine.fs");
	monStorage.addShader("Shaders/default.vs", "Shaders/Brain/brain.fs");
	monStorage.addShader("Shaders/default.vs", "Shaders/Truchet/truchet.fs");
	monStorage.addShader("Shaders/default.vs", "Shaders/Mandelbrot/mandelbrot.fs");
	monStorage.addShader("Shaders/default.vs", "Shaders/Raymarching/raymarching.fs");


	float time;
	float deltaTime, lastFrame;

	while(!glfwWindowShouldClose(window)){

		time = glfwGetTime();

        deltaTime = time - lastFrame;
        lastFrame = time;

		processInput(window);

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);

		Shader const& actualShader(monStorage.getActualShader());

		actualShader.use();

		actualShader.setFloat("time", time);

		///Pour tester, ces uniforme sont à remplacé pour des uniformes plus générique
		actualShader.setInt("mode", GlobalVariable::mode_);
		actualShader.setFloat("X", GlobalVariable::X_);
		actualShader.setFloat("Y", GlobalVariable::Y_);
		actualShader.setFloat("Z", GlobalVariable::Z_);
		actualShader.setFloat("Zoom", GlobalVariable::zoom_);
		actualShader.setVec2("screenSize", glm::vec2(GlobalVariable::windowWidth_, GlobalVariable::windowHeight_));
		actualShader.setFloat("xRatio", (float)GlobalVariable::windowWidth_/(float)GlobalVariable::windowHeight_);

		/*MORE GENERIC*/
		if(GlobalVariable::launch_ == true){
        	GlobalVariable::launchValue_+=0.15;
        }
       
        if(time - GlobalVariable::launchTime_ > 5){
        	GlobalVariable::launchValue_ = 1.0;
        	GlobalVariable::launch_ = false;
        }

	    actualShader.setFloat("launch", GlobalVariable::launchValue_);

		monCarre.draw();

	    glfwSwapBuffers(window);
	    glfwPollEvents();    
	}

    glfwTerminate();
    
	return 0;

}

void processInput(GLFWwindow *window){
	
    if(glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS){
        glfwSetWindowShouldClose(window, true);
    }

    if(glfwGetKey(window, GLFW_KEY_T) == GLFW_PRESS){
        GlobalVariable::Z_ += 0.1 * GlobalVariable::zoom_;
    }

    if(glfwGetKey(window, GLFW_KEY_G) == GLFW_PRESS){
        GlobalVariable::Z_ -= 0.1 * GlobalVariable::zoom_;
    }

    if(glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS){
        GlobalVariable::Y_ += 0.1 * GlobalVariable::zoom_;
    }

	if(glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS){
		GlobalVariable::Y_ -= 0.1 * GlobalVariable::zoom_;
	}

  	if(glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS){
        GlobalVariable::X_ += 0.1 * GlobalVariable::zoom_;
  	}

    if(glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS){
        GlobalVariable::X_ -= 0.1 * GlobalVariable::zoom_;
    }

    if(glfwGetKey(window, GLFW_KEY_E) == GLFW_PRESS){
        GlobalVariable::zoom_ /= 1.05;
    }

    if(glfwGetKey(window, GLFW_KEY_Q) == GLFW_PRESS){
        GlobalVariable::zoom_ *= 1.05;
    }

    if(glfwGetKey(window, GLFW_KEY_SPACE) == GLFW_PRESS){
    	GlobalVariable::launchValue_ = 1.0;
		GlobalVariable::launchTime_ = glfwGetTime();
		GlobalVariable::launch_ = true;
    }

}