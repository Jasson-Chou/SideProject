#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <tchar.h>

#define _FileName _T("ShareMemory")

#define _Status_None 0x00
#define _Status_AskConnectServer 0x01
#define _Status_ServerAck 0x02
#define _Status_ClientAck 0x03
#define _Status_Connecting 0x04
#define _Status_AskDisconnectServer 0x05

#define _ClientSending 0x01
#define _ServerSending 0x03
#define _UnSend 0x00
//volatile
struct MyDataStruct
{
	byte volatile Status : 3;
	byte volatile Sending : 2;
	byte Datas[256];
}*myDataStruct;