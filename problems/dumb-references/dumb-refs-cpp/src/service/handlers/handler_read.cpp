#include <iostream>
#include "handler_read.hpp"

ReadHandler::ReadHandler(const std::string& buffer)
	: buffer(buffer)
{}

void ReadHandler::handle() {
	std::cout << "ReadHandler: " << buffer << " (" << &buffer << ")" << std::endl;
}
