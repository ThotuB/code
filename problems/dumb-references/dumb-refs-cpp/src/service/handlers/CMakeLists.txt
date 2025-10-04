set(SOURCES
	handler_read.cpp
	handler_write.cpp
)

add_library(dumb_handlers STATIC ${SOURCES}) 
add_library(dumb::handlers ALIAS dumb_handlers)
