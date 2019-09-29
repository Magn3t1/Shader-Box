
#include "ShadersStorage.hpp"


ShadersStorage::ShadersStorage() : actualIndex_(0) {

}

void ShadersStorage::addShader(const char* vertexPath, const char* fragmentPath, const char* geometryPath){

	openedShaders_.emplace_back(vertexPath, fragmentPath, geometryPath);
	if(!openedShaders_.back().open_){
		openedShaders_.pop_back();
	}

}

///WIP
void ShadersStorage::findInAllFolder(std::string const& startingPath){}


Shader const& ShadersStorage::getActualShader(){

	return openedShaders_[actualIndex_];

}

void ShadersStorage::moveForward(){

	++actualIndex_;
	if(actualIndex_ >= openedShaders_.size()){
		actualIndex_ = 0;
	}

}


void ShadersStorage::moveBackward(){

	--actualIndex_;
	if(actualIndex_ < 0){
		actualIndex_ = openedShaders_.size() - 1;
	}

}