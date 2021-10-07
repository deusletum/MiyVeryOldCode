#include "..\common.h"

BOOL CFileOp::open(LPCTSTR szFile, UINT nFileFlags)
	{
	char *szTemp;
	UINT uCount, uIndex, uStart;
	CFileException cfeError;
	CFile cfFile;
#define MAXSIZE 2048

	ASSERT(szFile && *szFile);
	
	close();
	
	szTemp = new char[MAXSIZE];
	
	ASSERT(szTemp);
	
	csFileName = szFile;
	strcpy(szTemp, szFile);

	ASSERT(szTemp && *szTemp);
	
	cfFile.Open(szTemp, nFileFlags | CFile::shareDenyNone | CFile::modeRead | CFile::typeBinary, &cfeError);
	
	m_fChanged = FALSE;
	
	if(cfeError.m_cause != CFileException::none)
		{
		return FALSE;
		}
	
	uStart = 0;
	while(1)
		{
		uCount = cfFile.Read(szTemp, MAXSIZE);
		
		ASSERT(uCount <= MAXSIZE);
		ASSERT(szTemp && *szTemp);
				
		for (uIndex=0 ;uIndex < uCount; uIndex++)
			{
			if (szTemp[uIndex] == 0x0d) //Discard
				szTemp[uIndex] = 0;
			if (szTemp[uIndex] == 0x0a) //End of Line. Add it.
				{
				szTemp[uIndex] = 0;
				
				if (*(char *)(szTemp+uStart))
					add((char *)(szTemp+uStart));
				
				uStart = uIndex+1;
				}
			}
			
		if (uCount < MAXSIZE) //End of File
			{
			if (uStart <= uIndex) //No 0x0a on the Last line. Add it anyway.
				{
				szTemp[uIndex] = 0;
				if (*(char *)(szTemp+uStart))
					add((char *)(szTemp+uStart));
				}

			break;
			}
		
		if (uStart <= uIndex) //incomplete line in buffer.  Refill the Buffer.
				{
				uStart = uCount-uStart;
				cfFile.Seek(-((long)uStart), CFile::current);
				uStart = 0;
				}
		}

	return FALSE;
	}
	
BOOL CFileOp::save(LPCTSTR szFile)
	{
	CFile cfSave;
	CFileException cfeError;
	CString csTemp;
	POSITION pCurrent;	
	
	if (isChanged())
		{

		if (szFile && *szFile)
			{
			// if file exists, rename.
			cfSave.Open(szFile,	CFile::modeCreate 
								| CFile::modeReadWrite 
								| CFile::shareDenyWrite 
								| CFile::typeBinary
								, &cfeError);
			}
		else
			{
			// if file exists, rename.
			if (csFileName.IsEmpty())
				return FALSE;
			cfSave.Open(csFileName,	CFile::modeCreate 
									| CFile::modeReadWrite 
									| CFile::shareDenyWrite 
									| CFile::typeBinary
									, &cfeError);
			}
		
		if(cfeError.m_cause != CFileException::none)
			{
			return FALSE;
			}

		pCurrent = getRoot();
		
		while(pCurrent)
			{
			csTemp = cslCurrent.GetAt(pCurrent);
			if (csTemp.GetLength())
				{
				if (csTemp[0] == '[')
					csTemp = "\x0d\x0a" + csTemp;
				if (csTemp[csTemp.GetLength()-1] != '\x0a')
					csTemp += "\x0d\x0a";
				cfSave.Write(csTemp, csTemp.GetLength());
				}
			cslCurrent.GetNext(pCurrent);
			}
		
		return TRUE;
		}
	return FALSE;
	}
	
BOOL CFileOp::close()
	{
	csFileName.Empty();

	if (!(cslCurrent.IsEmpty()))
		{
		cslCurrent.RemoveAll();
		return TRUE;
		}
	
	return FALSE;
	}
	
		
POSITION CFileOp::getRoot()
	{
	if (cslCurrent.IsEmpty())
		return NULL;
	return cslCurrent.GetHeadPosition();
	}
	
POSITION CFileOp::getLast()
	{
	if (cslCurrent.IsEmpty())
		return NULL;
	return cslCurrent.GetTailPosition();
	}
	
POSITION CFileOp::getNext(POSITION pCurrent)
	{
	POSITION pTemp;

	if (cslCurrent.IsEmpty())
		return NULL;
	
	pTemp = pCurrent;
	
	cslCurrent.GetNext(pTemp);
	
	return pTemp;
	}
	
POSITION CFileOp::getPosition(LPCTSTR szSection, LPCTSTR szKey)
	{
	POSITION pFound = NULL;

	if (cslCurrent.IsEmpty())
		return NULL;
	
	if (szSection && *szSection)
		pFound = findSection(szSection);
	
	if (szKey && *szKey)
		pFound = findKey(szKey, pFound);
	
	return pFound;
	}

CString CFileOp::getValue(LPCTSTR szSection, LPCTSTR szKey)
	{
	CStringOp csoTemp;
	POSITION pFound = NULL;

	csoTemp.Empty();
	
	if (cslCurrent.IsEmpty())
		return csoTemp;
    
    if (szSection && *szSection)
    	{
		pFound = findSection(szSection);
		if (!pFound) 
			return csoTemp; //We still need to return a Valid CString.
		}
	
	if(szKey && *szKey)
		pFound = findKey(szKey, pFound);
	
	return getValue(pFound);
	}

POSITION CFileOp::findSection(LPCTSTR szSection)
	{
	CStringOp csoTemp, csoSection;
	POSITION pCurrent;

	if (cslCurrent.IsEmpty())
		return NULL;
	
	ASSERT(szSection && *szSection);
	
	csoSection = szSection;
	
	pCurrent = getRoot();
	
	while(pCurrent)
		{
		csoTemp = cslCurrent.GetAt(pCurrent);
		if (!m_bCase)
			{
			csoTemp.MakeLower();
			csoSection.MakeLower();
			}
		if (csoTemp == csoSection)
			break;
		cslCurrent.GetNext(pCurrent);
		}
		
	return pCurrent;
	}

POSITION CFileOp::findKey(LPCTSTR szKey, POSITION pCurrent)
	{
	BOOL bSec = TRUE;
	CStringOp csoTemp, csoKey;

	if (cslCurrent.IsEmpty())
		return NULL;
	
	if (!pCurrent)
		{
		bSec = FALSE;
		if (!(pCurrent = getRoot()))
			return NULL;
		}
	
	ASSERT(szKey && *szKey);
	ASSERT(pCurrent);
	
	csoKey = szKey;
	
	csoTemp = cslCurrent.GetAt(pCurrent);
	if (csoTemp[0] == '[' && bSec)
		cslCurrent.GetNext(pCurrent);
		
	while(pCurrent)
		{
		csoTemp = getKey(pCurrent);
		if(csoTemp.GetLength())
			{
			if (csoTemp[0] == '[' && bSec)
				return NULL;

			if (!m_bCase)
				{
				csoTemp.MakeLower();
				csoKey.MakeLower();
				}
			
			if (csoTemp == csoKey)
				break;
			}
		cslCurrent.GetNext(pCurrent);
		}
		
	return pCurrent;
	}
	
CString CFileOp::getValue(POSITION pCurrent)
	{
	CStringOp csoTemp;

	csoTemp.Empty();

	if (cslCurrent.IsEmpty())
		return csoTemp;
	
	if (!pCurrent)
		return csoTemp;
	
	csoTemp = cslCurrent.GetAt(pCurrent);
	csoTemp = csoTemp.getParam(1, '=');
	
	csoTemp = csoTemp.trimLeft();
	csoTemp = csoTemp.trimRight();
	
	return csoTemp;
	}
	
CString CFileOp::getKey(POSITION pCurrent)
	{
	CStringOp csoTemp;
	
	csoTemp.Empty();

	if (cslCurrent.IsEmpty())
		return csoTemp;
	
	if (!pCurrent)
		return csoTemp;
	
	csoTemp = cslCurrent.GetAt(pCurrent);
	csoTemp = csoTemp.getParam(0, '=');

	csoTemp = csoTemp.trimLeft();
	csoTemp = csoTemp.trimRight();

	return csoTemp;
	}
	
CString CFileOp::getSec(POSITION pCurrent)
	{
	CString csRet;
	csRet.Empty();
	return csRet;
	}
	


BOOL CFileOp::setValue(LPCTSTR szSection, LPCTSTR szKey, LPCTSTR szValue)
	{
	CString csTemp;
	POSITION pFound;
	
	ASSERT(szValue);
//	ASSERT(szSection && *szSection);
//	ASSERT(szKey && *szKey);
	
	pFound = getPosition(szSection, szKey);
	
	if (pFound)
		{
		setValue(szValue, pFound);
		}
	else
		{
		csTemp.Empty();
		if(szKey && *szKey)
			csTemp = szKey;
		if(szValue && *szValue)
			{
			csTemp += "=";
			csTemp += szValue;
			}
		
		pFound = getPosition(szSection);
		if (pFound)
			{
			do
				{
				cslCurrent.GetNext(pFound);
				if (getKey(pFound)[0] == '[')
					{
					cslCurrent.GetPrev(pFound);
					break;
					}
				}while(pFound);
			
			if (!csTemp.IsEmpty())
				add(csTemp, pFound);
			}
		else
			{
			if(szSection && *szSection)
				add(szSection);
			if (!csTemp.IsEmpty())
				add(csTemp);
			}
		}

	return FALSE;
	}
	
BOOL CFileOp::setValue(LPCTSTR szValue, POSITION pCurrent)
	{
	CString csTemp;
	
	if (cslCurrent.IsEmpty() || !pCurrent)
		return FALSE;
	
	csTemp = getKey(pCurrent);
	if(szValue && *szValue)
		{
		csTemp += "=";
		csTemp += szValue;
		}
	
	cslCurrent.SetAt(pCurrent, csTemp);
	
	return TRUE;
	}
	
BOOL CFileOp::setKey(LPCTSTR szValue, POSITION pCurrent)
	{
	CString csTemp;

	if (cslCurrent.IsEmpty() || !pCurrent)
		return FALSE;
	
	csTemp.Empty();
	
	if(szValue && *szValue)
		{
		csTemp = szValue;
		csTemp += "=";
		}
	csTemp += getValue(pCurrent);
		
	cslCurrent.SetAt(pCurrent, csTemp);
	
	return TRUE;
	}
	
		
BOOL CFileOp::add(LPCTSTR szLine, POSITION pCurrent)
	{
	CString csLine;
	
	ASSERT(szLine && *szLine);
	
	csLine = szLine;
	
	if (pCurrent)
		{
		cslCurrent.InsertAfter(pCurrent, csLine);
		changed();
		}
	else
		{
		cslCurrent.AddTail(csLine);
		changed();
		}
	
	return TRUE;
	}
	
BOOL CFileOp::insert(LPCTSTR szLine, POSITION pCurrent)
	{
	CString csLine;
	
	ASSERT(szLine && *szLine);
	
	csLine = szLine;
	
	if (pCurrent)
		{
		cslCurrent.InsertBefore(pCurrent, csLine);
		changed();
		}
	else
		{
		cslCurrent.AddHead(csLine);
		changed();
		}
	
	return TRUE;
	}
	
BOOL CFileOp::remove(POSITION pCurrent)
	{
	if (cslCurrent.IsEmpty() || !pCurrent)
		return FALSE;
		
	cslCurrent.RemoveAt(pCurrent);
	changed();
	
	return TRUE;
	}
	
BOOL CFileOp::isChanged()
	{
	return m_fChanged;
	}
	
void CFileOp::changed()
	{
	m_fChanged = TRUE;
	}
