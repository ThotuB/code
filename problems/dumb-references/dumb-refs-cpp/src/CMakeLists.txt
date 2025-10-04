add_subdirectory(service)

add_library(dumb_src INTERFACE)
add_library(dumb::src ALIAS dumb_src)

target_link_libraries(dumb_src
	INTERFACE
		dumb::service
)
