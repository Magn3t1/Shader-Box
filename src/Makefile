SRC	= $(wildcard *.cpp) #glad.c
OBJS	= $(SRC:.cpp=.o) glad.o
HEADER	= $(wildcard *.hpp) $(wildcard *.h)
OUT		= executable
CC	 	= g++

CFLAGS	= -O3 -Wall -W -pedantic -std=c++17

macLFLAGS	= -framework openGL -lglfw
linuxLFLAGS	= -lGL -lGLU -lglfw -ldl

OS := $(shell uname)


all: $(OBJS)
ifeq ($(OS), Darwin)
	$(CC) $(OBJS) -o $(OUT) $(macLFLAGS)
else
	$(CC) $(OBJS) -o $(OUT) $(linuxLFLAGS)
endif


%.o:	%.cpp %.hpp
	$(CC) -c $< $(CFLAGS)

%.o:	%.cpp %.h
	$(CC) -c $< $(CFLAGS)

%.o:	%.cpp
	$(CC) -c $< $(CFLAGS)

glad.o: glad.c
	$(CC) -c glad.c $(CFLAGS)


clean:
	rm -f $(OBJS)

cleanall:
	rm -f $(OBJS) $(OUT)