#include "stdafx.h"
#include "arp_helper.h"
#include "log2file.h"

TCHAR ifNames[8][MAX_PATH];
TCHAR txtArp[8192];
TCHAR* typeNames[]={L"n0ne", L"other", L"invalid", L"dynamic", L"static"};

void updateIFnames(){
	DWORD i;
	PMIB_IFTABLE ifTable;
	MIB_IFROW MibIfRow;
	DWORD dwSize = 0;
	DWORD dwRetVal = 0;
	TCHAR	tStr[MAX_PATH];

	for(int x=0; x<8; x++)
		wsprintf(ifNames[x], L"");;

	/*
	* GetIfTable() is used before using GetIfEntry().
	* This is to obtain number of network interfaces.
	*/

	/* Get size required for GetIfTable() */
	if (GetIfTable(NULL, &dwSize, 0) == ERROR_INSUFFICIENT_BUFFER) {
		ifTable = (MIB_IFTABLE *) malloc (dwSize);
	}

	/* Get actual data using GetIfTable() */
	if ((dwRetVal = GetIfTable(ifTable, &dwSize, 0)) == NO_ERROR) {
		if (ifTable->dwNumEntries > 0) {
			DEBUGMSG(1, (L"\r\n\r\nNumber of Adapters: %ld\r\n", ifTable->dwNumEntries));
			for (i=1; i<=ifTable->dwNumEntries; i++) {
				MibIfRow.dwIndex = i;
				if ((dwRetVal = GetIfEntry(&MibIfRow)) == NO_ERROR) {
					wsprintf(tStr, L"Adapter# %i, Description: %S\r\n", MibIfRow.dwIndex, MibIfRow.bDescr);
					//mbstowcs(tStr, aStr, strlen(aStr));
					DEBUGMSG(1, (tStr));
					wsprintf(ifNames[MibIfRow.dwIndex], L"%S", MibIfRow.bDescr);
				}
			}
			DEBUGMSG(1, (L"\r\n"));
		}
	} else {
		printf("GetInterfaceInfo failed.\r\n");
		LPVOID lpMsgBuf;

		if (FormatMessage( 
			FORMAT_MESSAGE_ALLOCATE_BUFFER | 
			FORMAT_MESSAGE_FROM_SYSTEM | 
			FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL,
			dwRetVal,
			MAKELANGID(LANG_NEUTRAL,
			SUBLANG_DEFAULT), // Default language
			(LPTSTR) &lpMsgBuf,
			0,
			NULL ))  {
				printf("Error: %s", lpMsgBuf);
		}

		LocalFree( lpMsgBuf );
	}

	return;
}
//####################################################################
TCHAR* getIfName(int index){
	if(wcslen(ifNames[index])==0)
		wsprintf(ifNames[index], L"%i", index);
	return ifNames[index];
}

//####################################################################
// see also http://www.geekpage.jp/en/programming/iphlpapi/interface-info.php
//####################################################################
TCHAR* getArpTable(TCHAR* wStr){
	memset(wStr, 0, wcslen(wStr)*sizeof(TCHAR));
	TCHAR tmpStr[MAX_PATH];
	DWORD dwRetVal=0;
	MIB_IPNETTABLE* pIpNetTable=NULL;
//	MIB_IPNETROW*  pArpEntries=NULL;
	ULONG dwSize=0;
	BOOLEAN bOrder=true;
	DWORD i=0;

	updateIFnames();

	//get needed size
	dwRetVal=GetIpNetTable(pIpNetTable, &dwSize, bOrder);
	if(dwRetVal==ERROR_INSUFFICIENT_BUFFER){
		//alloc buffer
		pIpNetTable=(MIB_IPNETTABLE*)malloc(dwSize);
		//fill the table
		 /* Now that we know the size, lets use GetIpNetTable() */
		if ((dwRetVal = GetIpNetTable(pIpNetTable, &dwSize, 0))== NO_ERROR) 
		{
		  if (pIpNetTable->dwNumEntries > 0) {
			  wsprintf(tmpStr, L"\r\n"); wcscat(wStr, tmpStr);
			for (i=0; i<pIpNetTable->dwNumEntries; i++) {
				wsprintf(tmpStr, L"Address: %S\r\n",inet_ntoa(*(struct in_addr *)&pIpNetTable->table[i].dwAddr));
				wcscat(wStr,tmpStr);

				wsprintf(tmpStr, L"Phys Addr Len: %d\r\n", pIpNetTable->table[i].dwPhysAddrLen);
				wcscat(wStr,tmpStr);

				if(pIpNetTable->table[i].dwPhysAddrLen==6){
					wsprintf(tmpStr, L"Phys Address: %.2x:%.2x:%.2x:%.2x:%.2x:%.2x\r\n",
						pIpNetTable->table[i].bPhysAddr[0],
						pIpNetTable->table[i].bPhysAddr[1],
						pIpNetTable->table[i].bPhysAddr[2],
						pIpNetTable->table[i].bPhysAddr[3],
						pIpNetTable->table[i].bPhysAddr[4],
						pIpNetTable->table[i].bPhysAddr[5]);
				}
				wcscat(wStr,tmpStr);
				wsprintf(tmpStr, L"Index:  %ld\r\n", pIpNetTable->table[i].dwIndex);
				wcscat(wStr,tmpStr);
				wsprintf(tmpStr, L"Name:  %s\r\n", getIfName(pIpNetTable->table[i].dwIndex));
				wcscat(wStr,tmpStr);
				wsprintf(tmpStr, L"Type:   %s\r\n", typeNames[pIpNetTable->table[i].dwType]);
				wcscat(wStr,tmpStr);
				wsprintf(tmpStr, L"\r\n");
				wcscat(wStr,tmpStr);

				//log to file: interface->ip->mac->type
				Add2LogWtime(L"'%s'\t'%S'\t%.2x:%.2x:%.2x:%.2x:%.2x:%.2x\t%s\n", 
					getIfName(pIpNetTable->table[i].dwIndex), 
					inet_ntoa(*(struct in_addr *)&pIpNetTable->table[i].dwAddr),
					pIpNetTable->table[i].bPhysAddr[0],
					pIpNetTable->table[i].bPhysAddr[1],
					pIpNetTable->table[i].bPhysAddr[2],
					pIpNetTable->table[i].bPhysAddr[3],
					pIpNetTable->table[i].bPhysAddr[4],
					pIpNetTable->table[i].bPhysAddr[5],
					typeNames[pIpNetTable->table[i].dwType]);

			}//for
		}//if dwNumEntries
	  }//if NO_ERROR
	}// ERROR_INSUFFICIENT_BUFFER
	else{
		wsprintf(wStr, L"ERROR_INSUFFICIENT_BUFFER\r\n");
	}
	DEBUGMSG(1,(wStr)); //output debug window
	return wStr;
}

/*
DWORD
WINAPI
CreateIpNetEntry(
    IN PMIB_IPNETROW    pArpEntry
    );

DWORD
WINAPI
SetIpNetEntry(
    IN PMIB_IPNETROW    pArpEntry
    );

DWORD
WINAPI
DeleteIpNetEntry(
    IN PMIB_IPNETROW    pArpEntry
    );

DWORD
WINAPI
FlushIpNetTable(
    IN DWORD   dwIfIndex
    );
*/

