#include "stdafx.h"

#ifndef _ARP_HELPER_INC_
#define _ARP_HELPER_INC_

extern TCHAR ifNames[8][MAX_PATH];
extern TCHAR txtArp[8192];

TCHAR* getArpTable(TCHAR* wStr);

#endif