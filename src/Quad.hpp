#pragma once

#include <glad/glad.h>

#include <vector>

class Quad{

public:
	Quad();

	void draw() const;

private:

	unsigned int VAO_;
	unsigned int VBO_;

	void loadVertices();


	std::vector<float> vertices_;

};