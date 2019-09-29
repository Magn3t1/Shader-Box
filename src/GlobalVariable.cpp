
#include "GlobalVariable.hpp"

ShadersStorage* GlobalVariable::mainShadersStoragePointer_ = nullptr;

unsigned int GlobalVariable::windowWidth_ = 0;
unsigned int GlobalVariable::windowHeight_ = 0;


unsigned int GlobalVariable::mode_ = 0;

float GlobalVariable::zoom_ = 1;
float GlobalVariable::X_ = 0;
float GlobalVariable::Y_ = 0;