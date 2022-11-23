// LogicAndBitwiseOperations.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//
#include <Windows.h>
#include <iostream>
#include <math.h>
void Compare(const int Count, int (*func)(int, int));
int LogicOperation(int a, int b);
int BitwiseOperation(int a, int b);
int main()
{
    const int Count = INT32_MAX;
    LARGE_INTEGER Freq;
    LARGE_INTEGER sCounter;
    LARGE_INTEGER eCounter;
    LARGE_INTEGER diff1;
    LARGE_INTEGER diff2;
    LONGLONG time1;
    LONGLONG time2;
    LONGLONG diff;

    QueryPerformanceCounter(&sCounter);
    Compare(Count, LogicOperation);
    QueryPerformanceCounter(&eCounter);
    QueryPerformanceFrequency(&Freq);

    diff1.QuadPart = eCounter.QuadPart - sCounter.QuadPart;
    time1 = diff1.QuadPart * 1000 / Freq.QuadPart;

    QueryPerformanceCounter(&sCounter);
    Compare(Count, BitwiseOperation);
    QueryPerformanceCounter(&eCounter);
    QueryPerformanceFrequency(&Freq);

    diff2.QuadPart = eCounter.QuadPart - sCounter.QuadPart;
    time2 = diff2.QuadPart * 1000 / Freq.QuadPart;
    diff = abs(time2 - time1);

    std::cout << "Logic Operation(5) Spend Time: " << time1 << "ms" <<std::endl;
    std::cout << "Bitwise Operation(5) Spend Time: " << time2 << "ms" <<std::endl;
    std::cout << "Different Time: " << diff << "ms" << std::endl;

    system("pause");
}

void Compare(const int Count, int (* func)(int , int ))
{
    int index = 0;
    do
    {
        func(index, Count);
        ++index;
    } while (index < Count);
}

int LogicOperation(int a, int b)
{
    return (a / 2) + (b / 2) + (a * 2) + (b * 2);
}

int BitwiseOperation(int a, int b)
{
    return (a >> 1) + (b >> 1) + (a << 1) + (b << 1);
}

// 執行程式: Ctrl + F5 或 [偵錯] > [啟動但不偵錯] 功能表
// 偵錯程式: F5 或 [偵錯] > [啟動偵錯] 功能表

// 開始使用的提示: 
//   1. 使用 [方案總管] 視窗，新增/管理檔案
//   2. 使用 [Team Explorer] 視窗，連線到原始檔控制
//   3. 使用 [輸出] 視窗，參閱組建輸出與其他訊息
//   4. 使用 [錯誤清單] 視窗，檢視錯誤
//   5. 前往 [專案] > [新增項目]，建立新的程式碼檔案，或是前往 [專案] > [新增現有項目]，將現有程式碼檔案新增至專案
//   6. 之後要再次開啟此專案時，請前往 [檔案] > [開啟] > [專案]，然後選取 .sln 檔案
