// Project.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <ShareMemoryHeader.h>
#include <time.h>

#if _DEBUG
#define _ClientPath "start /d E:\\SideProject\\ShareMemoryTest\\ShareMemory\\Debug MyClient.exe"
#else
#define _ClientPath "start /d E:\\SideProject\\ShareMemoryTest\\ShareMemory\\Release MyClient.exe"
#endif



int main()
{
	printf_s("My Server Initializing ... \r\n");

	LPVOID  pBuf;
	HANDLE handleFile;
	TCHAR szFileName[] = _FileName;
	SIZE_T BuffSize = sizeof(MyDataStruct);

	handleFile = CreateFileMapping((HANDLE)0xFFFFFFFF, NULL, PAGE_READWRITE, 0, BuffSize, (LPCTSTR)szFileName);

	if (handleFile == NULL)
	{
		printf_s("Create Share Memory Fail!\r\n");
		printf_s("Error Code : %d\r\n", GetLastError());

		return -1;
	}
	else
	{
		printf_s("Created Share Memory Success!\r\n");
	}

	pBuf = MapViewOfFile(handleFile, FILE_MAP_ALL_ACCESS, 0, 0, BuffSize);

	if (NULL == pBuf) {

		printf_s("Could not map view of file (%d)\r\n", GetLastError());

		CloseHandle(handleFile);

		return -2;
	}
	else
	{
		myDataStruct = (MyDataStruct*)pBuf;
	}

	myDataStruct->Status = _Status_None;
	myDataStruct->Sending = _UnSend;

	printf_s("My Server Initialize Done! \r\n");

	printf_s("Waiting for Client Connected! \r\n");

	system(_ClientPath);
	
	do
	{
		Sleep(100);
	} while (myDataStruct->Status != _Status_AskConnectServer); // Wait Client Ask Connecting
	
	Sleep(100);
	myDataStruct->Status = _Status_ServerAck;
	
	do
	{
		Sleep(100);
	} while (myDataStruct->Status != _Status_ClientAck);// Wait Client Ack

	myDataStruct->Status = _Status_Connecting;

	printf_s("Connect with Client! \r\n");

	double START, END; START = clock();
	unsigned int dataCnt = 0;

	while (myDataStruct->Status == _Status_Connecting)
	{
		switch (myDataStruct->Sending)
		{
			case _ClientSending:// Has Data From Client
			{
				//To Do...
#if OutputConsole
				printf_s("Recieve Data : { %s }\r\n", (char*)myDataStruct->Datas);
#endif
				//Clear Buff
				memset(myDataStruct->Datas, 0, sizeof(myDataStruct->Datas));

				dataCnt++;

				myDataStruct->Sending = _UnSend;
			}
			break;
		}
	}

	END = clock();

	//Close Server
	if(UnmapViewOfFile(pBuf))
		printf_s("unmap view file success!\r\n");
	else
		printf_s("unmap view file fail!\r\n Error Code %d \r\n", GetLastError());

	if (CloseHandle(handleFile))
		printf_s("close handle success!\r\n");
	else
		printf_s("close handle fail!\r\n Error Code %d\r\n", GetLastError());


	printf_s("Server Closed !\r\n");

	double spendTime = (END - START) / CLOCKS_PER_SEC;
	printf_s("------------------------------------------------------------\r\n");
	printf_s("Total Spend Time : %.10f Second!\r\n" , spendTime);
	printf_s("Per Data Spend Time : %.10f Second!\r\n" , spendTime / dataCnt);

	getchar();

	return 0;
	

}

// 執行程式: Ctrl + F5 或 [偵錯] > [啟動但不偵錯] 功能表
// 偵錯程式: F5 或 [偵錯] > [啟動偵錯] 功能表

// 開始使用的秘訣: 
//   1. 使用 [方案總管] 視窗，新增/管理檔案
//   2. 使用 [Team Explorer] 視窗，連線到原始檔控制
//   3. 使用 [輸出] 視窗，參閱組建輸出與其他訊息
//   4. 使用 [錯誤清單] 視窗，檢視錯誤
//   5. 前往 [專案] > [新增項目]，建立新的程式碼檔案，或是前往 [專案] > [新增現有項目]，將現有程式碼檔案新增至專案
//   6. 之後要再次開啟此專案時，請前往 [檔案] > [開啟] > [專案]，然後選取 .sln 檔案
