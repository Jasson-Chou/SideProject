// ConsoleApplication.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include "pch.h"
#include <Windows.h>
#include <iostream>

//Define Attribute Color
enum class EConsoleAttributeColor
{
	Blue = 1,
	Green = 2,
	Aqua = 3,
	Red = 4,
	Purple = 5,
	Yellow = 6,
	White = 7,
	Gray = 8,
	LightBlue = 9,
	Black = 0,
	LightGreen = 10,
	LightAqua = 11,
	LightRed = 12,
	LightPurple = 13,
	LightYellow = 14,
	BrightWhite = 15,
};


HANDLE hConsole;
//Define Print Overload
void MyPrint(const char* msg, EConsoleAttributeColor fore, EConsoleAttributeColor back);
void MyPrint(const char* msg, EConsoleAttributeColor fore);
void MyPrint(const char* msg);

int main()
{
	//Get Console Handle
	hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	MyPrint("Testing...\n", EConsoleAttributeColor::BrightWhite);

	MyPrint("Result:");

	MyPrint("Fail\n", EConsoleAttributeColor::Red);

	system("pause");
}


void MyPrint(const char* msg, EConsoleAttributeColor fore, EConsoleAttributeColor back)
{
	SetConsoleTextAttribute(hConsole, ((DWORD)back << 4) | (DWORD)fore);
	printf_s(msg);
	//Default Attribute
	SetConsoleTextAttribute(hConsole, ((DWORD)EConsoleAttributeColor::Black << 4) | (DWORD)EConsoleAttributeColor::White);
}

void MyPrint(const char* msg, EConsoleAttributeColor fore)
{
	MyPrint(msg, fore, EConsoleAttributeColor::Black);
}

void MyPrint(const char* msg)
{
	MyPrint(msg, EConsoleAttributeColor::White, EConsoleAttributeColor::Black);
}




