add_subdirectory(handlers)

set(SOURCES
	service.cpp
)

add_library(dumb_service STATIC ${SOURCES})
add_library(dumb::service ALIAS dumb_service)

target_link_libraries(dumb_service
	PUBLIC
		dumb::handlers
)
