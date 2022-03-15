// ConsoleApplication.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include "pch.h"
#include <Windows.h>
#include <iostream>
enum class EConsoleTextForeground
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

enum class EConsoleTextBackground
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

HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

void SetConsoleTextColor(EConsoleTextBackground back, EConsoleTextForeground fore);
void MyPrint(const char* msg, EConsoleTextForeground fore);
void MyPrint(const char* msg);

int main()
{
	/*SetConsoleTextColor(EConsoleTextBackground::Black, EConsoleTextForeground::BrightWhite);
	printf_s("Testing...\n");*/
	MyPrint("Testing...\n", EConsoleTextForeground::BrightWhite);

	/*SetConsoleTextColor(EConsoleTextBackground::Black, EConsoleTextForeground::White);
	printf_s("Result:");*/
	MyPrint("Result:");

	/*SetConsoleTextColor(EConsoleTextBackground::Black, EConsoleTextForeground::Red);
	printf_s("Fail\n");*/
	MyPrint("Fail\n", EConsoleTextForeground::Red);

	/*SetConsoleTextColor(EConsoleTextBackground::Black, EConsoleTextForeground::White);*/
	system("pause");
}

void SetConsoleTextColor(EConsoleTextBackground back,EConsoleTextForeground fore)
{
	SetConsoleTextAttribute(hConsole, ((DWORD)back << 4) | (DWORD)fore);
}

void MyPrint(const char* msg, EConsoleTextForeground fore, EConsoleTextBackground back)
{
	SetConsoleTextAttribute(hConsole, ((DWORD)back << 4) | (DWORD)fore);
	printf_s(msg);
	SetConsoleTextAttribute(hConsole, ((DWORD)EConsoleTextBackground::Black << 4) | (DWORD)EConsoleTextForeground::White);
}

void MyPrint(const char* msg, EConsoleTextForeground fore)
{
	MyPrint(msg, fore, EConsoleTextBackground::Black);
}

void MyPrint(const char* msg)
{
	MyPrint(msg, EConsoleTextForeground::White, EConsoleTextBackground::Black);
}




