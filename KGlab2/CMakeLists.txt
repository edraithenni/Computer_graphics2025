cmake_minimum_required(VERSION 3.16)

project(labkg VERSION 0.1 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

set(CMAKE_AUTOUIC ON)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

find_package(Qt6 COMPONENTS Widgets REQUIRED)

include("D:/vcpkg/vcpkg/scripts/buildsystems/vcpkg.cmake")
#set(Protobuf_DIR "D:/vcpkg/vcpkg/installed/x64-windows/share/protobuf")
#set(OpenCV_DIR "D:/vcpkg/vcpkg/installed/x64-windows/share/opencv4")
#set(quirc_DIR "D:/vcpkg/vcpkg/installed/x64-windows/share/quirc")
find_package(OpenCV REQUIRED)

message(STATUS "OpenCV include dirs: ${OpenCV_INCLUDE_DIRS}")
message(STATUS "OpenCV libraries: ${OpenCV_LIBS}")

set(PROJECT_SOURCES
    main.cpp
    mainwindow.cpp
    mainwindow.h
    mainwindow.ui
)

qt_add_executable(labkg MANUAL_FINALIZATION ${PROJECT_SOURCES})

target_link_libraries(labkg PRIVATE
    Qt6::Widgets
    ${OpenCV_LIBS}
)

target_include_directories(labkg PRIVATE
    ${OpenCV_INCLUDE_DIRS}
)

set_target_properties(labkg PROPERTIES WIN32_EXECUTABLE TRUE)
qt_finalize_executable(labkg)
