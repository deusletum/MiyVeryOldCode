#ifndef COMMON_H

#define COMMON_H

#include <iostream.h>
#include <stdio.h>
//#include <malloc.h>
#include <string.h>
#include <stdlib.h>
#include <stdarg.h>
#include <time.h>
#include <ctype.h>
#include <fcntl.h>
#include <io.h>
#pragma warning (disable:4142)
#include <conio.h>
#pragma warning (default:4142)
#include <sys\stat.h>

#include <afx.h>
#include <afxcoll.h>

#include ".\cstrngop\cstrngop.h"
#include ".\cFileOp\cFileOp.h"
#include ".\clog\clog.h"
#include ".\cVar\cVar.h"

/******* Common definitions and typedefs ***********************************/

#define VOID		    void

#ifndef _WINDEF_
#define FAR                 _far
#define NEAR		    _near
#define PASCAL		    _pascal
#define CDECL		    _cdecl
#endif

/****** Simple types & common helper macros *********************************/

typedef int		    BOOL;
#define FALSE		    0
//#define TRUE		    !FALSE

typedef unsigned char	    BYTE;
typedef unsigned short      WORD;
typedef unsigned long       DWORD;

typedef unsigned int	    UINT;

#define LONG long

/****** Common pointer types ************************************************/

typedef char NEAR*          PSTR;
typedef char NEAR*          NPSTR;

typedef char NEAR*          PCHAR;
typedef char NEAR*          NPCHAR;

typedef char FAR*           LPSTR;
typedef const char FAR*     LPCSTR;

typedef BYTE NEAR*	    PBYTE;
typedef BYTE FAR*	    LPBYTE;

typedef int NEAR*	    PINT;
typedef int FAR*	    LPINT;

typedef WORD NEAR*          PWORD;
typedef WORD FAR*           LPWORD;

typedef long NEAR*	    PLONG;
typedef long FAR*	    LPLONG;

typedef DWORD NEAR*         PDWORD;
typedef DWORD FAR*          LPDWORD;

typedef void FAR*           LPVOID;

#endif

#ifndef NULL
#define NULL		    0
#endif
