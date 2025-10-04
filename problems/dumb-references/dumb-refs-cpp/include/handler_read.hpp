#pragma once

#include <string>
class ReadHandler {
public:
	ReadHandler(const std::string& buffer);

	void handle();

private:
	const std::string& buffer;
};
