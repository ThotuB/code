#include "service.hpp"
#include <iostream>

Service create_service(std::string buffer) {
	auto service = Service(buffer);

	delete &buffer;

	return service;
}

int main(){
	std::string buffer = "Hello, World!";

	std::cout << "Buffer: " << buffer << " (" << &buffer << ")" << std::endl;

	auto service = create_service(buffer);
	service.handle();
	service.write("Goodbye, World!");
	service.handle();

	std::cout << "Buffer: " << buffer << " (" << &buffer << ")" << std::endl;
}

