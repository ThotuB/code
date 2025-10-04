#include "service.hpp"
#include "handler_read.hpp"
#include "handler_write.hpp"
#include <iostream>

Service::Service(std::string& buffer)
	: read_handler(buffer)
	, write_handler(buffer)
{}

void Service::handle(){
	// std::cout << "Service: " << buffer << " (" << &buffer << ")" << std::endl;
	read_handler.handle();
	write_handler.handle();
}

void Service::write(std::string data){
	// std::cout << "Service write: \"" << buffer << "\" to \"" << data << "\"" << std::endl;
	write_handler.rewrite(data);
}
