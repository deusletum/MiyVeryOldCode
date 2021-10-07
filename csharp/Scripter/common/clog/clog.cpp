#include "..\common.h"

void CLog::setProgram(LPCTSTR szProg)
	{
	CStringOp csoTemp;
	
	m_csProgram = szProg;
	
	csoTemp = "C:\\";
	csoTemp += m_csProgram.Left(8);
	csoTemp += ".dbg";
	
	open(csoTemp);

	return;
	}

void CLog::db(char *szFile, int nLine, char *szProc, LPCTSTR szString, ...)
	{
	POSITION pSection = NULL;
	CStringOp csoTemp, csoDebug;
	char **szParamList, *szTmp;
	
	if(!m_bDebug)
		return;
	
	csoDebug = "(";
	csoDebug += szProc;

	csoTemp = szFile;
	csoTemp = csoTemp.getParam(csoTemp.getNumParam('\\'), '\\');
	csoTemp += "@";  

    szTmp = new char[1024];
    if (!szTmp)
    	{
    	cout << "new(1024) Failed!" << endl;
    	return;
    	}

	*szTmp=0;
	itoa(nLine, szTmp, 10);
	csoTemp += szTmp;
	csoTemp += ")";

	szParamList = (char**)((&szString)+1);

	*szTmp=0;
	sprintf(szTmp, szString, szParamList[0], szParamList[1], szParamList[2], szParamList[3], szParamList[4], szParamList[5], szParamList[6]);
    if (strlen(szTmp) > 1024)
    	{
    	CStringOp csoDebug;
    	csoDebug = "Debug |";
    	csoDebug += csoTemp;
    	csoDebug += "| String Exceeded Limit.";
//    	blowupex(__FILE__, __LINE__,"DBO",ERROR,csoTemp);
		delete szTmp;
    	db(__FILE__,__LINE__,"DB",csoTemp);
    	return;
    	}
    csoDebug += csoTemp;
	csoDebug += "DEBUG: ";
	csoDebug += szTmp;

	delete szTmp;

	cout << csoDebug << endl;
	add(csoDebug);
	save();

	return;
	}
