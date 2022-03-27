// WinServiceProcessHandler.cpp : 定義 DLL 應用程式的匯出函式。
//

#include "stdafx.h"
#include <Windows.h>
#include <tlhelp32.h>
#include <comdef.h>
#include <tchar.h>
#include <UserEnv.h>
#include <string>
#include <lmcons.h>
#include <WtsApi32.h>

#pragma comment(lib, "WtsApi32.lib")
#pragma comment(lib, "advapi32.lib")
#pragma comment(lib, "userenv.lib")

#define WinlogonProcessIdsBuffSize 1024

typedef struct
{
	DWORD PID;
	DWORD SessionID;
}ProcessInfo;

void GetWinlogonProcessId(const UINT32 ui_buffSize, ProcessInfo* pproInfo_buff, UINT32* ui_GetSize)
{
	*ui_GetSize = 0;
	PROCESSENTRY32 entry;
	entry.dwSize = sizeof(PROCESSENTRY32);
	HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
	if (Process32First(snapshot, &entry) == TRUE)
	{
		while (Process32Next(snapshot, &entry) == TRUE)
		{
			if (lstrcmpiW(entry.szExeFile, L"winlogon.exe") == 0)
			{
				auto processInfo = (pproInfo_buff + (*ui_GetSize));
				processInfo->PID = entry.th32ProcessID;
				ProcessIdToSessionId(processInfo->PID, &(processInfo->SessionID));
				++(*ui_GetSize);
				if (*ui_GetSize == ui_buffSize)
					return;
			}
		}
	}
}

BOOL CreateEnvirnment(HANDLE hToken, LPVOID pEnv)
{
	STARTUPINFO startupInfo;
	ZeroMemory(&startupInfo, sizeof(STARTUPINFO));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.lpDesktop = LPWSTR("WinSta0\\Default");
	startupInfo.wShowWindow = SW_SHOW;
	startupInfo.dwFlags = STARTF_USESHOWWINDOW;

	LPVOID pEnv = NULL;
	DWORD dwCreateionFlag = NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT;

	if (!CreateEnvironmentBlock(&pEnv, hToken, FALSE))
	{
		return FALSE;
	}

	return TRUE;
}

BOOL FindMatchSessionIdProcessInfo(const DWORD dw_SessionID, ProcessInfo* pproInfo_buff, const UINT32 ui_buffSize, ProcessInfo* pproInfo_match)
{
	for (UINT32 idx = 0; idx < ui_buffSize; idx++)
	{
		auto processInfo = *(pproInfo_buff + idx);
		if (processInfo.SessionID == dw_SessionID)
		{
			memcpy_s(pproInfo_match, sizeof(ProcessInfo), &processInfo, sizeof(ProcessInfo));
			return TRUE;
		}
	}
	return FALSE;
}

bool OpenProcessByPassUAC(const char* sz_appFileName)
{
	DWORD d_conSessId;
	UINT32 uiGetSize;

	HANDLE h_Process = NULL;
	HANDLE h_ProcessToken = NULL;
	HANDLE h_DuplicateToken = NULL;

	DWORD d_DesiredAccess = TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY | 
		TOKEN_DUPLICATE | TOKEN_ASSIGN_PRIMARY | TOKEN_ADJUST_SESSIONID |
		TOKEN_READ | TOKEN_WRITE;

	ProcessInfo matchProcessInfo;
	ZeroMemory(&matchProcessInfo, sizeof(ProcessInfo));

	ProcessInfo winlogonProcessInfos[WinlogonProcessIdsBuffSize];
	ZeroMemory(winlogonProcessInfos, sizeof(ProcessInfo) * WinlogonProcessIdsBuffSize);
	GetWinlogonProcessId(WinlogonProcessIdsBuffSize, winlogonProcessInfos, &uiGetSize);

	d_conSessId = WTSGetActiveConsoleSessionId();
	if(!FindMatchSessionIdProcessInfo(d_conSessId, winlogonProcessInfos, uiGetSize, &matchProcessInfo))
	{
		return false;
	}

	h_Process = OpenProcess(PROCESS_ALL_ACCESS, FALSE, matchProcessInfo.PID);
	if(!OpenProcessToken(h_Process, d_DesiredAccess, &h_ProcessToken))
	{
		CloseHandle(h_Process);
		return false;
	}

	if (!DuplicateTokenEx(h_ProcessToken, MAXIMUM_ALLOWED, NULL, SecurityIdentification, TokenPrimary, &h_DuplicateToken))
	{
		CloseHandle(h_ProcessToken);
		return false;
	}

	if (!SetTokenInformation(h_DuplicateToken, TokenSessionId, (void*)&d_conSessId, sizeof(DWORD)))
	{
		CloseHandle(h_ProcessToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}


	STARTUPINFO startupInfo;
	PROCESS_INFORMATION processInfo;
	ZeroMemory(&startupInfo, sizeof(STARTUPINFO));
	ZeroMemory(&processInfo, sizeof(PROCESS_INFORMATION));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.lpDesktop = LPWSTR("WinSta0\\Default");
	startupInfo.wShowWindow = SW_SHOW;
	startupInfo.dwFlags = STARTF_USESHOWWINDOW;

	LPVOID pEnv = NULL;
	DWORD dwCreateionFlag = NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT;
	
	if (!CreateEnvironmentBlock(&pEnv, h_DuplicateToken, FALSE))
	{
		CloseHandle(h_ProcessToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}

	
	if (!CreateProcessAsUser(h_DuplicateToken, NULL, LPWSTR(sz_appFileName), NULL, NULL,
		FALSE, dwCreateionFlag, pEnv, NULL, &startupInfo, &processInfo))
	{
		CloseHandle(h_ProcessToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}

	if (pEnv)
	{
		DestroyEnvironmentBlock(pEnv);
	}

	CloseHandle(h_ProcessToken);
	CloseHandle(h_DuplicateToken);

	return true;
}

bool OpenProcess(const char* AppFileName, int SeesionID)
{
	DWORD d_sessionID = (DWORD)SeesionID;
	LUID luid_LookupPriVal;
	HANDLE h_UserToken = NULL;
	HANDLE h_DuplicateToken = NULL;

	TOKEN_PRIVILEGES tkp;
	tkp.PrivilegeCount = 1;
	tkp.Privileges[0].Luid = luid_LookupPriVal;
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

	STARTUPINFO startupInfo;
	ZeroMemory(&startupInfo, sizeof(STARTUPINFO));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.lpDesktop = LPWSTR("WinSta0\\Default");
	startupInfo.wShowWindow = SW_SHOW;
	startupInfo.dwFlags = STARTF_USESHOWWINDOW;

	LPVOID pEnv = NULL;

	DWORD dwCreateionFlag = NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT;

	PROCESS_INFORMATION processInfo;
	ZeroMemory(&processInfo, sizeof(PROCESS_INFORMATION));

	if (!WTSQueryUserToken(d_sessionID, &h_UserToken))
	{
		return false;
	}

	if (!LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luid_LookupPriVal))
	{
		CloseHandle(h_UserToken);
		return false;
	}

	if (!AdjustTokenPrivileges(h_UserToken, FALSE, &tkp, sizeof(tkp), NULL, NULL))
	{
		CloseHandle(h_UserToken);
		return false;
	}
	
	if (!DuplicateTokenEx(h_UserToken, MAXIMUM_ALLOWED, NULL, SecurityIdentification, TokenPrimary, &h_DuplicateToken))
	{
		CloseHandle(h_UserToken);
		return false;
	}

	if (!SetTokenInformation(h_DuplicateToken, TokenSessionId, (void*)&d_sessionID, sizeof(DWORD)))
	{
		CloseHandle(h_UserToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}
	
	
	if(!CreateEnvirnment(h_DuplicateToken, pEnv))
	{
		CloseHandle(h_UserToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}


	
	
	if (!CreateProcessAsUser(h_DuplicateToken, NULL, LPWSTR(AppFileName), NULL, NULL,
		FALSE, dwCreateionFlag, pEnv, NULL, &startupInfo, &processInfo))
	{
		CloseHandle(h_UserToken);
		CloseHandle(h_DuplicateToken);
		return false;
	}

	if (pEnv)
	{
		DestroyEnvironmentBlock(pEnv);
	}

	CloseHandle(h_UserToken);
	CloseHandle(h_DuplicateToken);

	return true;
}
