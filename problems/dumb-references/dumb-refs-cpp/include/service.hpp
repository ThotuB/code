#pragma once

#include "handler_read.hpp"
#include "handler_write.hpp"
#include <string>

class Service {
public:
	Service(std::string& buffer);

	void handle();

	void write(std::string new_buffer);
private:
	ReadHandler read_handler;
	WriteHandler write_handler;

	// std::string buffer;
};
