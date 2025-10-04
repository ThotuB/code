#include <iostream>
#include "handler_write.hpp"

WriteHandler::WriteHandler(std::string& buffer)
	: buffer(buffer)
{}

void WriteHandler::handle() {
	std::cout << "WriteHandler: " << buffer << " (" << &buffer << ")" << std::endl;
}

void WriteHandler::rewrite(std::string new_buffer) {
	buffer = new_buffer;
}
