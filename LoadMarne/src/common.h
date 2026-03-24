#pragma once

#pragma execution_character_set("utf-8")

#pragma warning(disable:4996)

#define WIN32_LEAN_AND_MEAN

#include <filesystem>
#include <fstream>
#include <iostream>
#include <ShlObj_core.h>
#include <string>
#include <thread>
#include <windows.h>

#include "MinHook/MinHook.h"

#pragma comment(linker, "/export:DirectInput8Create=C:\\WINDOWS\\System32\\dinput8.DirectInput8Create,@1")
#pragma comment(linker, "/export:DllCanUnloadNow=C:\\WINDOWS\\System32\\dinput8.DllCanUnloadNow,@2")
#pragma comment(linker, "/export:DllGetClassObject=C:\\WINDOWS\\System32\\dinput8.DllGetClassObject,@3")
#pragma comment(linker, "/export:DllRegisterServer=C:\\WINDOWS\\System32\\dinput8.DllRegisterServer,@4")
#pragma comment(linker, "/export:DllUnregisterServer=C:\\WINDOWS\\System32\\dinput8.DllUnregisterServer,@5")
#pragma comment(linker, "/export:GetdfDIJoystick=C:\\WINDOWS\\System32\\dinput8.GetdfDIJoystick,@6")

// 线程休眠时间
#define ThreadSleep(ms) (std::this_thread::sleep_for(std::chrono::milliseconds(ms)))