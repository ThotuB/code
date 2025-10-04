cmake_minimum_required(VERSION 3.22..3.31)

set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

project(dumb-references VERSION 0.1 LANGUAGES CXX)

add_executable(dumb-references src/main.cpp)

include_directories(include)
target_link_libraries(dumb-references
	PUBLIC
		dumb::src
)

add_subdirectory(src)
