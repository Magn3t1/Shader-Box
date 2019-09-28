
#pragma once

//Lets wait Macos 10.15....
//#include <filesystem>
#include <string>
#include <vector>

#include "Shader.hpp"

class ShadersStorage{

public:
	ShadersStorage();

	void addShader(const char* vertexPath, const char* fragmentPath, const char* geometryPath = nullptr);

	//Not working for the moment..
	void findInAllFolder(std::string const& startingPath);

	Shader const& getActualShader();

	void moveForward();
	void moveBackward();

private:

	ssize_t actualIndex_;

	std::vector<Shader> openedShaders_;


};