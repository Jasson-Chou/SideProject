#include <Windows.h>
#include <iostream>

typedef struct
{
	UINT64 ulSrcAddr;
	UINT64 ulDstAddr;
	UINT32 uiControl;
}DMA_DESCRIPTOR;

int main()
{
	BOOL bIsToDevice = FALSE;


	DWORD dwPages = 16;
	DWORD dwBytes = 1024 * 4;
	DMA_DESCRIPTOR des[256];
	DWORD i;

	ZeroMemory(des, sizeof(DMA_DESCRIPTOR) * 256);

	for (i = 0; i < dwPages; i++)
	{
		if (bIsToDevice)
		{

		}
		else
		{

		}

		if (i < dwPages - 1)
		{

		}
		else
		{

		}

	}

	system("pause");
}

