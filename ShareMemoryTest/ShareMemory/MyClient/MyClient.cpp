// MyClient.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <ShareMemoryHeader.h>
int main()
{
	printf_s("My Server Initializing ... \r\n");

	LPVOID  pBuf;
	HANDLE handleFile;
	TCHAR szFileName[] = _FileName;
	SIZE_T BuffSize = sizeof(MyDataStruct);

	handleFile = OpenFileMapping(
		FILE_MAP_ALL_ACCESS,
		FALSE,
		szFileName
	);

	if (handleFile == NULL)
	{
		printf_s("Open Share Memory Fail!\r\n");
		printf_s("Error Code : %d\r\n", GetLastError());

		return -1;
	}
	else
	{
		printf_s("Opend Share Memory Success!\r\n");
	}


	pBuf = MapViewOfFile(
		handleFile, FILE_MAP_ALL_ACCESS,
		0, 0, BuffSize );

	if (NULL == pBuf) {

		printf_s("Could not map view of file (%d)\r\n", GetLastError());

		CloseHandle(handleFile);

		return -2;
	}
	else
	{
		myDataStruct = (MyDataStruct*)pBuf;
	}

	myDataStruct->Status = _Status_AskConnectServer;
	do
	{
		Sleep(100);
	} while (myDataStruct->Status != _Status_ServerAck);// try Ack Connect with Server
	
	Sleep(100);
	
	myDataStruct->Status = _Status_ClientAck;

	printf_s("Connect with Server! \r\n");

	Sleep(200);

	printf_s("Start Send Datas ! \r\n");

	for (int i = 0; i < 1e4 && myDataStruct->Status == _Status_Connecting; i++)
	{
		char str[100];
		sprintf_s(str, sizeof(str), "Test Data Num : %d", i);
		printf_s(str);
		printf_s("\r\n");
		memcpy_s(myDataStruct->Datas, sizeof(myDataStruct->Datas), str, sizeof(str));
		
		myDataStruct->Sending = _ClientSending;

		while (myDataStruct->Sending == _ClientSending);
	}

	//Disconnect with Server
	myDataStruct->Status = _Status_AskDisconnectServer;
	CloseHandle(handleFile);

	printf_s("Close Client ! \r\n");
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
