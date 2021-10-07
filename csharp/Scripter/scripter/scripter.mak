# Microsoft Visual C++ generated build script - Do not modify

PROJ = SCRIPTER
DEBUG = 0
PROGTYPE = 6
CALLER = 
ARGS = /d /list:D:\config.ini /sec:[NetSetup]
DLLS = 
D_RCDEFINES = -d_DEBUG
R_RCDEFINES = -dNDEBUG
ORIGIN = MSVC
ORIGIN_VER = 1.00
PROJPATH = D:\DOC\PROJECT\CORE\SCRIPTER\
USEMFC = 1
CC = cl
CPP = cl
CXX = cl
CCREATEPCHFLAG = 
CPPCREATEPCHFLAG = 
CUSEPCHFLAG = 
CPPUSEPCHFLAG = 
FIRSTC =             
FIRSTCPP = CFILEOP.CPP 
RC = rc
CFLAGS_D_DEXE = /nologo /G2 /W3 /Zi /AL /Od /D "_DEBUG" /D "_DOS" /FR /Fd"SCRIPTER.PDB"
CFLAGS_R_DEXE = /nologo /Gs /G2 /W3 /AL /Os /Ox /D "NDEBUG" /D "_DOS" /FR 
LFLAGS_D_DEXE = /NOLOGO /NOE /NOI /STACK:8192 /ONERROR:NOEXE /CO 
LFLAGS_R_DEXE = /NOLOGO /NOE /NOI /STACK:5120 /ONERROR:NOEXE 
LIBS_D_DEXE = lafxcrd oldnames llibce 
LIBS_R_DEXE = lafxcr oldnames llibce 
RCFLAGS = /nologo
RESFLAGS = /nologo
RUNFLAGS = 
OBJS_EXT = 
LIBS_EXT = 
!if "$(DEBUG)" == "1"
CFLAGS = $(CFLAGS_D_DEXE)
LFLAGS = $(LFLAGS_D_DEXE)
LIBS = $(LIBS_D_DEXE)
MAPFILE = nul
RCDEFINES = $(D_RCDEFINES)
!else
CFLAGS = $(CFLAGS_R_DEXE)
LFLAGS = $(LFLAGS_R_DEXE)
LIBS = $(LIBS_R_DEXE)
MAPFILE = nul
RCDEFINES = $(R_RCDEFINES)
!endif
!if [if exist MSVC.BND del MSVC.BND]
!endif
SBRS = CFILEOP.SBR \
		CLOG.SBR \
		CSCRIPT.SBR \
		SCRIPTER.SBR \
		CPRCSSR.SBR \
		CRUN.SBR \
		CVAR.SBR \
		CSTRNGOP.SBR


CFILEOP_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h


CLOG_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h


CSCRIPT_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h \
	d:\doc\project\core\common\cprcssr\cprcssr.h \
	d:\doc\project\core\common\cscript\cscript.h


SCRIPTER_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h \
	d:\doc\project\core\common\cprcssr\cprcssr.h \
	d:\doc\project\core\common\crun\crun.h \
	d:\doc\project\core\common\cscript\cscript.h


CPRCSSR_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h \
	d:\doc\project\core\common\cprcssr\cprcssr.h


CRUN_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h \
	d:\doc\project\core\common\cprcssr\cprcssr.h \
	d:\doc\project\core\common\crun\crun.h


CVAR_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h


CSTRNGOP_DEP = d:\doc\project\core\common\common.h \
	d:\doc\project\core\common\cstrngop\cstrngop.h \
	d:\doc\project\core\common\cfileop\cfileop.h \
	d:\doc\project\core\common\clog\clog.h \
	d:\doc\project\core\common\cvar\cvar.h


all:	$(PROJ).EXE $(PROJ).BSC

CFILEOP.OBJ:	..\COMMON\CFILEOP\CFILEOP.CPP $(CFILEOP_DEP)
	$(CPP) $(CFLAGS) $(CPPCREATEPCHFLAG) /c ..\COMMON\CFILEOP\CFILEOP.CPP

CLOG.OBJ:	..\COMMON\CLOG\CLOG.CPP $(CLOG_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CLOG\CLOG.CPP

CSCRIPT.OBJ:	..\COMMON\CSCRIPT\CSCRIPT.CPP $(CSCRIPT_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CSCRIPT\CSCRIPT.CPP

SCRIPTER.OBJ:	SCRIPTER.CPP $(SCRIPTER_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c SCRIPTER.CPP

CPRCSSR.OBJ:	..\COMMON\CPRCSSR\CPRCSSR.CPP $(CPRCSSR_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CPRCSSR\CPRCSSR.CPP

CRUN.OBJ:	..\COMMON\CRUN\CRUN.CPP $(CRUN_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CRUN\CRUN.CPP

CVAR.OBJ:	..\COMMON\CVAR\CVAR.CPP $(CVAR_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CVAR\CVAR.CPP

CSTRNGOP.OBJ:	..\COMMON\CSTRNGOP\CSTRNGOP.CPP $(CSTRNGOP_DEP)
	$(CPP) $(CFLAGS) $(CPPUSEPCHFLAG) /c ..\COMMON\CSTRNGOP\CSTRNGOP.CPP

$(PROJ).EXE::	CFILEOP.OBJ CLOG.OBJ CSCRIPT.OBJ SCRIPTER.OBJ CPRCSSR.OBJ CRUN.OBJ \
	CVAR.OBJ CSTRNGOP.OBJ $(OBJS_EXT) $(DEFFILE)
	echo >NUL @<<$(PROJ).CRF
CFILEOP.OBJ +
CLOG.OBJ +
CSCRIPT.OBJ +
SCRIPTER.OBJ +
CPRCSSR.OBJ +
CRUN.OBJ +
CVAR.OBJ +
CSTRNGOP.OBJ +
$(OBJS_EXT)
$(PROJ).EXE
$(MAPFILE)
c:\apps\win16\msvc\lib\+
c:\apps\win16\msvc\mfc\lib\+
$(LIBS)
$(DEFFILE);
<<
	link $(LFLAGS) @$(PROJ).CRF

run: $(PROJ).EXE
	$(PROJ) $(RUNFLAGS)


$(PROJ).BSC: $(SBRS)
	bscmake @<<
/o$@ $(SBRS)
<<
