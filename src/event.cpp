
#include "event.hpp"

#include "GlobalVariable.hpp"

void framebuffer_size_callback(GLFWwindow* window, int width, int height){
    glViewport(0, 0, width, height);
}


void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods){
    
    if(key == GLFW_KEY_UP && action == GLFW_PRESS){
    	GlobalVariable::mainShadersStoragePointer_->moveForward();
    }
    
    if (key == GLFW_KEY_DOWN && action == GLFW_PRESS){
    	GlobalVariable::mainShadersStoragePointer_->moveBackward();
    }
}
