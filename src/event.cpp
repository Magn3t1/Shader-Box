
#include "event.hpp"

#include "GlobalVariable.hpp"

void framebuffer_size_callback(GLFWwindow* window, int width, int height){
	GlobalVariable::windowWidth_ = width;
	GlobalVariable::windowHeight_ = height;
    glViewport(0, 0, width, height);
}


void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods){
    
    if(key == GLFW_KEY_UP && action == GLFW_PRESS){
    	GlobalVariable::mainShadersStoragePointer_->moveForward();
    }
    
    if (key == GLFW_KEY_DOWN && action == GLFW_PRESS){
    	GlobalVariable::mainShadersStoragePointer_->moveBackward();
    }

    if (key == GLFW_KEY_R && action == GLFW_PRESS){
    	GlobalVariable::zoom_ = 1;
    	GlobalVariable::X_ = 0;
    	GlobalVariable::Y_ = 0;
    	GlobalVariable::mode_ = 0;
    }

    //The key from 0 to 9 have an idea between 48 and 57.
	if(key >= 48 && key <= 57 && action == GLFW_PRESS){

		unsigned int value = key - 48;
		GlobalVariable::mode_ = value;

	}

}
