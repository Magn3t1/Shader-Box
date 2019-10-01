
#include "GlobalVariable.hpp"

ShadersStorage* GlobalVariable::mainShadersStoragePointer_ = nullptr;

unsigned int GlobalVariable::windowWidth_ = 0;
unsigned int GlobalVariable::windowHeight_ = 0;


unsigned int GlobalVariable::mode_ = 0;

float GlobalVariable::zoom_ = 1;
float GlobalVariable::X_ = 0;
float GlobalVariable::Y_ = 0;
float GlobalVariable::Z_ = 0;
bool GlobalVariable::launch_ = false;
float GlobalVariable::launchTime_ = 0;
float GlobalVariable::launchValue_ = 0;