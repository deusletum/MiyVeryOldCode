#include "..\common.h"
#include "..\cprcssr\cprcssr.h"
#include "crun.h"

void CRun::exec()
	{
	char szTemp[32];
	CString csRun;
	CStringOp csoTemp;
	
	m_nRun = 1;
	
	if (!m_pclOutput)
		{
		m_pclOutput = new CLog;
		m_pclOutput->setProgram("CScript");
		m_pclOutput->setDebug(FALSE);
		m_bLocalLog = TRUE;
		}

	if (!m_pcvStore)
		{
		m_pcvStore = new CVar;
		m_bLocalVars = TRUE;
		}

	m_cfoScript.setCaseSensitive(FALSE);
	
	while (1)
		{
		m_pclOutput->db(__FILE__,__LINE__, "EXEC", "Run #%d", m_nRun);
		
		itoa(m_nRun, szTemp, 10);
		ASSERT (szTemp && *szTemp);
		csoTemp = szTemp;
		csoTemp = csoTemp.trimLeft();
		csoTemp = csoTemp.trimRight();
		
		csRun = "[Run";
		csRun += csoTemp;
		csRun += "]";
		
		if (!m_cfoScript.findSection(csRun))
			break;
		
		csoTemp = m_cfoScript.getValue(csRun, "file");
		csoTemp = m_pcvStore->resolveVar(csoTemp);
		if (!csoTemp.IsEmpty())
			{
			csoTemp.MakeLower();
	
			m_pclOutput->db(__FILE__,__LINE__, "EXEC", "file=|%s|", csoTemp);
	
			if (m_csoFile != csoTemp)
				{
				m_cfoIniFile.save();
				m_cfoIniFile.close();
				
				if (csoTemp.GetLength())
					{
					m_cfoIniFile.open(csoTemp);
					}
				}
			m_csoFile = csoTemp;
			}
		
		m_csoDepend = m_pcvStore->resolveVar(m_cfoScript.getValue(csRun, "depend"));

		m_csoFunction = m_cfoScript.getValue(csRun, "function");

		m_csoParams = m_pcvStore->resolveVar(m_cfoScript.getValue(csRun, "params"));
		
		callFunction(m_csoFunction, m_csoParams);
		
		m_nRun++;
		}

	if(m_bLocalLog)
		{
		delete m_pclOutput;
		m_pclOutput = NULL;
		m_bLocalLog = FALSE;
		}
	
	if(m_bLocalVars)
		{
		delete m_pcvStore;
		m_pcvStore = NULL;
		m_bLocalVars = FALSE;
		}
		
	return;
	}

void CRun::callFunction(LPCTSTR szFunction, LPCTSTR szParams)
	{
	int nIndex=0;
	BOOL bRet = TRUE;
	CString csFunction;
	static char *szFunctions[] =	{
									"add",
									"remove",
									"copy",
									"insert",
									"tolower",
									"toupper",
									"fileappend",
									"getparam",
									"makename"
									};
	
	ASSERT(szFunction && szParams);

	csFunction = szFunction;
	csFunction.MakeLower();
	
	while(szFunctions[nIndex][0])
		{
		if ((CString)szFunctions[nIndex] == csFunction)
			break;
		nIndex++;
		}

	m_pclOutput->db(__FILE__,__LINE__, "CF", "Index = %d", nIndex);

	switch(nIndex)
		{
		case 0:
			bRet = add(szParams);
			break;
		case 1:
			bRet = remove(szParams);
			break;
		case 2:
			bRet = copy(szParams);
			break;
		case 3:
			bRet = insert(szParams);
			break;
		case 4:
			bRet = toLower(szParams);
			break;
		case 5:
			bRet = toUpper(szParams);
			break;
		case 6:
			bRet = fileAppend(szParams);
			break;
		case 7:
			bRet = getParam(szParams);
			break;
		case 8:
			bRet = makeName(szParams);
			break;
		default:
			m_pclOutput->db(__FILE__,__LINE__, "CF", "|%s| is an unknown function.", szFunction);
			return;
		};
	
	if (!bRet)
		{
		m_pclOutput->db(__FILE__,__LINE__, "CF", "%s function failed in RUN %d.", szFunction, m_nRun);
		}
	
	return;
	}


//szKey{PARAMDELIM}szValue
//szKey{PARAMDELIM}szValue{PARAMDELIM}szKey
//szKey{PARAMDELIM}szValue{PARAMDELIM}[szSection]
//szKey{PARAMDELIM}szValue{PARAMDELIM}[szSection]{PARAMDELIM}szKey
BOOL CRun::add(LPCTSTR szParams)
	{
	CStringOp csoParams;
	CString csTemp;
	
	csoParams = szParams;
	
	m_pclOutput->db(__FILE__,__LINE__, "ADD", "Params=|%s|", csoParams);
	
	if (csoParams.getParam(0,PARAMDELIM).IsEmpty())
		return FALSE;

	csTemp = csoParams.getParam(0,PARAMDELIM);
	csTemp += "=";
	csTemp += csoParams.getParam(1,PARAMDELIM);
	
	if (!csoParams.getParam(2,PARAMDELIM).IsEmpty())
		{
		if (csoParams.getParam(2,PARAMDELIM)[0] == '[')
			{
			if (!csoParams.getParam(3,PARAMDELIM).IsEmpty())
				{
				//add after ([section],Key) if they exist (will add at end of file if they do not exist)
				m_cfoIniFile.add(csTemp, m_cfoIniFile.getPosition(csoParams.getParam(2,PARAMDELIM), csoParams.getParam(3,PARAMDELIM)));
				}
			else
				{
				//reset value of (Key) if it already exists (will create if not found)
				m_cfoIniFile.setValue(csoParams.getParam(2,PARAMDELIM), csoParams.getParam(0,PARAMDELIM), csoParams.getParam(1,PARAMDELIM));
				}
			}
		else
			{
			//add after (Key) if it exists (will add at end of file if it does not exist)
			m_cfoIniFile.add(csTemp, m_cfoIniFile.getPosition(NULL, csoParams.getParam(2,PARAMDELIM)));
			}
		return TRUE;
		}
	else 
		{
		//add at end of file
		m_cfoIniFile.add(csTemp);
		return TRUE;
		}
		
	return FALSE;
	}

//szKey
//szKey{PARAMDELIM}szSection
BOOL CRun::remove(LPCTSTR szParams)
	{
	CStringOp csoParams;
	POSITION pLine;
	
	csoParams = szParams;
	
	m_pclOutput->db(__FILE__,__LINE__, "RMV", "Params=|%s|", csoParams);

	if (!csoParams.getParam(0,PARAMDELIM).GetLength())
		{
		return FALSE;
	    }
	    
	if (!((csoParams.getParam(1,PARAMDELIM)).IsEmpty()))
		pLine = m_cfoIniFile.getPosition(csoParams.getParam(1,PARAMDELIM), csoParams.getParam(0,PARAMDELIM));
	else
		pLine = m_cfoIniFile.getPosition(NULL, csoParams.getParam(0,PARAMDELIM));
		
	if (pLine)
		{
		m_cfoIniFile.remove(pLine);
		return TRUE;
		}

	return FALSE;
	}

//szDestVar
//szDestVar{PARAMDELIM}szValue
//szDestVar{PARAMDELIM}[szSection]{PARAMDELIM}szKey
BOOL CRun::copy(LPCTSTR szParams)
	{
	CStringOp csoParams, csoTemp;
	
	csoParams = szParams;
	m_pclOutput->db(__FILE__,__LINE__, "CPY", "Params=|%s|", csoParams);

	if (csoParams.getParam(0,PARAMDELIM).IsEmpty())
		{
		return FALSE;
	    }
	
	if (!csoParams.getParam(2,PARAMDELIM).IsEmpty())
		{
		csoTemp = m_cfoIniFile.getValue(csoParams.getParam(1,PARAMDELIM), csoParams.getParam(2,PARAMDELIM));
		m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoTemp);
		}
	else
		{
		if (csoParams.getParam(1,PARAMDELIM).IsEmpty())
			{
			m_pcvStore->remove(m_pcvStore->getPosition(NULL, csoParams.getParam(0,PARAMDELIM)));
		    }
		else
			{
			m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoParams.getParam(1,PARAMDELIM));
			}
		}
	
	return TRUE;
	}

//szKey{PARAMDELIM}szValue
//szKey{PARAMDELIM}szValue{PARAMDELIM}szKey
//szKey{PARAMDELIM}szValue{PARAMDELIM}[szSection]
//szKey{PARAMDELIM}szValue{PARAMDELIM}[szSection]{PARAMDELIM}szKey
BOOL CRun::insert(LPCTSTR szParams)
	{
	CStringOp csoParams;
	CString csTemp;
	
	csoParams = szParams;
	
	m_pclOutput->db(__FILE__,__LINE__, "INSRT", "Params=|%s|", csoParams);
	
	if (csoParams.getParam(0,PARAMDELIM).IsEmpty())
		return FALSE;

	csTemp = csoParams.getParam(0,PARAMDELIM);
	csTemp += "=";
	csTemp += csoParams.getParam(1,PARAMDELIM);
	
	if (!csoParams.getParam(2,PARAMDELIM).IsEmpty())
		{
		if (csoParams.getParam(2,PARAMDELIM)[0] == '[')
			{
			if (!csoParams.getParam(3,PARAMDELIM).IsEmpty())
				{
				//insert before ([section],Key) if they exist (will insert at begining of file if they do not exist)
				m_cfoIniFile.insert(csTemp, m_cfoIniFile.getPosition(csoParams.getParam(2,PARAMDELIM), csoParams.getParam(3,PARAMDELIM)));
				}
			else
				{
				//reset value of (Key) if it already exists (will create if not found)
				m_cfoIniFile.setValue(csoParams.getParam(2,PARAMDELIM), csoParams.getParam(0,PARAMDELIM), csoParams.getParam(1,PARAMDELIM));
				}
			}
		else
			{
			//insert before (Key) if it exists (will insert at begining of file if it does not exist)
			m_cfoIniFile.insert(csTemp, m_cfoIniFile.getPosition(NULL, csoParams.getParam(2,PARAMDELIM)));
			}
		return TRUE;
		}
	else 
		{
		//insert at begining of file
		m_cfoIniFile.insert(csTemp);
		return TRUE;
		}
		
	return FALSE;
	}

//szDestVar{PARAMDELIM}szData
BOOL CRun::toLower(LPCTSTR szParams)
	{
	CStringOp csoParams, csoTemp;
	
	csoParams = szParams;
    m_pclOutput->db(__FILE__,__LINE__, "TOLOW", "Params=|%s|", csoParams);
    
	if (!csoParams.getParam(0,PARAMDELIM).IsEmpty())
		if (!csoParams.getParam(1,PARAMDELIM).IsEmpty())
			{
			csoTemp = csoParams.getParam(1,PARAMDELIM);
			csoTemp.MakeLower();
			m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoTemp);
			return TRUE;
			}

	return FALSE;
	}

//szDestVar{PARAMDELIM}szData
BOOL CRun::toUpper(LPCTSTR szParams)
	{
	CStringOp csoParams, csoTemp;
	
	csoParams = szParams;
	m_pclOutput->db(__FILE__,__LINE__, "TOUP", "Params=|%s|", csoParams);
	
	if (!csoParams.getParam(0,PARAMDELIM).IsEmpty())
		if (!csoParams.getParam(1,PARAMDELIM).IsEmpty())
			{
			csoTemp = csoParams.getParam(1,PARAMDELIM);
			csoTemp.MakeUpper();
			m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoTemp);
			return TRUE;
			}

	return FALSE;
	}

//szDestFile{PARAMDELIM}szSourceFile
BOOL CRun::fileAppend(LPCTSTR szParams)
	{
	UINT uSize = 64000, uRead;
	CFile cfSource, cfDest;
	CFileException cfeSrcError, cfeDstError;
	CStringOp csoParams;
	char *pszBuffy;
	
	csoParams = szParams;
	m_pclOutput->db(__FILE__,__LINE__, "FAPPND", "Params=|%s|", csoParams);
	
	if (!csoParams.getParam(0,PARAMDELIM).IsEmpty()
		&& !csoParams.getParam(1,PARAMDELIM).IsEmpty())
		{
		cfSource.Open(csoParams.getParam(1,PARAMDELIM), CFile::shareDenyNone | CFile::modeRead | CFile::typeBinary, &cfeSrcError);
		if(cfeSrcError.m_cause != CFileException::none)
			{
			m_pclOutput->db(__FILE__,__LINE__, "FAPPND", "Failed to Open SRC |%s|", csoParams.getParam(1,PARAMDELIM));
			return FALSE;
			}

		cfDest.Open(csoParams.getParam(0,PARAMDELIM), CFile::shareExclusive | CFile::modeWrite | CFile::typeBinary, &cfeDstError);
		if(cfeDstError.m_cause != CFileException::none)
			{
			m_pclOutput->db(__FILE__,__LINE__, "FAPPND", "Failed to Open DST |%s|", csoParams.getParam(0,PARAMDELIM));
			return FALSE;
			}
		
		cfDest.SeekToEnd();
		
		while(uSize > 1000 && !(pszBuffy = new char[uSize]))
			uSize /= 2;
		
		if (!pszBuffy)
			{
			m_pclOutput->db(__FILE__,__LINE__, "FAPPND", "Failed to Allocate %d bytes", uSize*2);
			return FALSE;
			}
		
		while((uRead = cfSource.Read(pszBuffy, uSize-1)))
			{
			cfDest.Write(pszBuffy, uRead);
			if (uRead < uSize-1)
				break;
			}
		
		delete pszBuffy;
		return TRUE;
		}
	
	m_pclOutput->db(__FILE__,__LINE__, "FAPPND", "Invalid Parameters");
	
	return FALSE;
	}

//szDestVar{PARAMDELIM}szData{PARAMDELIM}nParam{PARAMDELIM}cDelim
BOOL CRun::getParam(LPCTSTR szParams)
	{
	CStringOp csoParams, csoData;
	int nParam;
	char cDelim;
	
	csoParams = szParams;
	m_pclOutput->db(__FILE__,__LINE__, "GTPRM", "Params=|%s|", csoParams);
	
	if (!csoParams.getParam(0,PARAMDELIM).IsEmpty()
		&& !csoParams.getParam(1,PARAMDELIM).IsEmpty()
		&& !csoParams.getParam(2,PARAMDELIM).IsEmpty()
		&& !csoParams.getParam(3,PARAMDELIM).IsEmpty())
		{
		nParam = atoi(csoParams.getParam(2,PARAMDELIM));
		csoData = csoParams.getParam(3,PARAMDELIM);
		cDelim = (csoData.trimLeft())[0];
		csoData = csoParams.getParam(1,PARAMDELIM);
		csoData = csoData.getParam(nParam, cDelim);
//		cout << csoData << ", " << cDelim << ", " << nParam << endl;
		m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoData);
		return TRUE;
		}
	
	return FALSE;
	}


BOOL CRun::makeName(LPCTSTR szParams)
	{
	unsigned long far * ulCurrentTick;
	unsigned long ulTick;
	char szReturn[32];
	CStringOp csoParams, csoTemp;
	
	csoParams = szParams;
	m_pclOutput->db(__FILE__,__LINE__, "MKNM", "Params=|%s|", csoParams);
	
	if (!csoParams.getParam(0,PARAMDELIM).IsEmpty())
		{
		ulCurrentTick=(unsigned long far *) 0x0000046C;
		ulTick=*ulCurrentTick;
		ltoa((unsigned long) ulTick, szReturn, 10);

		csoTemp = szReturn;

		m_pcvStore->setValue(NULL, csoParams.getParam(0,PARAMDELIM), csoTemp);
		return TRUE;
		}

	return FALSE;
	}
