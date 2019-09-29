#pragma once

#include "ShadersStorage.hpp"

class GlobalVariable{

public:

	static unsigned int mode_;

	static float zoom_;
	static float X_;
	static float Y_;

	static unsigned int windowWidth_;
	static unsigned int windowHeight_;

	static ShadersStorage* mainShadersStoragePointer_;

	
};