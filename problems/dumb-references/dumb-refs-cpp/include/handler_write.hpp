#pragma once

#include <string>
class WriteHandler {
public:
	WriteHandler(std::string& buffer);

	void handle();
	void rewrite(std::string new_buffer);

private:
	std::string& buffer;
};
