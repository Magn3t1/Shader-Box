#include "Quad.hpp"

Quad::Quad(){

	//Initialize our quad's vertices
	loadVertices();


	glGenVertexArrays(1, &VAO_);
	glGenBuffers(1, &VBO_);

	glBindVertexArray(VAO_);

	glBindBuffer(GL_ARRAY_BUFFER, VBO_);
	glBufferData(GL_ARRAY_BUFFER, vertices_.size() * sizeof(float), vertices_.data(), GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*) 0);
	glEnableVertexAttribArray(0);

	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*) (3 * sizeof(float)) );
	glEnableVertexAttribArray(1);

	glBindVertexArray(0);


}

void Quad::draw() const{

	glBindVertexArray(VAO_);

	glDrawArrays(GL_TRIANGLES, 0, 6);

}

void Quad::loadVertices(){

	vertices_ = {
    // first triangle
     1.0f,  1.0f, 0.0f, 1.0f, 1.0f,  // top right
     1.0f, -1.0f, 0.0f, 1.0f, 0.0f,  // bottom right
    -1.0f,  1.0f, 0.0f, 0.0f, 1.0f, // top left 
    // second triangle
     1.0f, -1.0f, 0.0f, 1.0f, 0.0f,  // bottom right
    -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,  // bottom left
    -1.0f,  1.0f, 0.0f, 0.0f, 1.0f   // top left
	};

}