// inline.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include "pch.h"
#include <Windows.h>
#include <iostream>
void Log(std::string sz_func, double d_usec);
inline int inlineFunc(const int& a, const int& b);
int Func(const int& a, const int& b);
#define SIZE UINT32_MAX
int main()
{
	LARGE_INTEGER ull_Start, ull_Stop, ull_Freq;
	
	UINT32 i = SIZE;
	QueryPerformanceCounter(&ull_Start);
	while (--i)
	{
		inlineFunc(5, 2);
		/*fun1(5, 2);
		fun1(5, 2);*/
	}
	QueryPerformanceCounter(&ull_Stop);
	QueryPerformanceFrequency(&ull_Freq);
	// Sec * 1000 * 1000 ==> us
	Log("inlineFunc Spend: ", (ull_Stop.QuadPart - ull_Start.QuadPart) / (double)ull_Freq.QuadPart * 1000000 / SIZE);

	i = SIZE;
	QueryPerformanceCounter(&ull_Start);
	while (--i)
	{
		Func(5, 2);
		/*fun2(5, 2);
		fun2(5, 2);*/
	}
	QueryPerformanceCounter(&ull_Stop);
	QueryPerformanceFrequency(&ull_Freq);
	// Sec * 1000 * 1000 ==> us
	Log("Func       Spend: ", (ull_Stop.QuadPart - ull_Start.QuadPart) / (double)ull_Freq.QuadPart * 1000000 / SIZE);
	system("pause");
}

void Log(std::string sz_func, double d_usec)
{
	std::cout << sz_func.c_str() << d_usec << "us" << std::endl;
}

inline int inlineFunc(const int& ui_a, const int& ui_b)
{
	return ui_a / ui_b;
}
int Func(const int& ui_a, const int& ui_b)
{
	return ui_a / ui_b;
}
