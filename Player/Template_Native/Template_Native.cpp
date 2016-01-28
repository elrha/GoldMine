// Template_Native_Core.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#define EXPORT extern "C" __declspec(dllexport)

EXPORT void __stdcall InnerInitialize(int myNumber, int totalPlayerCount, int col, int row)
{
}

EXPORT int __stdcall InnerProcess(int myNumber, int* playerPosition, int* playerPower, int* playerStun, int* mapBlocks)
{
	return 0;
}