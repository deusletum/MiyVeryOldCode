#include "..\common.h"

CString CStringOp::trimLeft()
	{
	CStringOp csoTemp;
	int nCount;
			
	csoTemp = *this;
			
	if (csoTemp.IsEmpty())
		{
		csoTemp.Empty();
		return csoTemp;
		}
			
	nCount = 0;
	while(nCount < csoTemp.GetLength() && isspace(csoTemp[nCount++]));
			
	if(nCount)
		csoTemp = csoTemp.Right(csoTemp.GetLength()-(nCount-1));
			
	return csoTemp;
	}


CString CStringOp::trimRight()
	{
	CStringOp csoTemp;
	int nCount;
			
	csoTemp = *this;
			
	if (csoTemp.IsEmpty())
		{
		csoTemp.Empty();
		return csoTemp;
		}

	nCount = csoTemp.GetLength();
	while(nCount > 0 && isspace(csoTemp[--nCount]));
	
	csoTemp = csoTemp.Left(nCount+1);
			
	return csoTemp;
	}

int CStringOp::getNumParam(char cDelim)
	{
	CStringOp csTemp;
	int nCount, nIndex;
				
	nCount = 0;
	csTemp = *this;
	while(1)
		{
		if (csTemp.IsEmpty())
			break;
				
		nIndex = csTemp.Find(cDelim);

		if (nIndex < 0)
			break;
		else
			csTemp = csTemp.Right(csTemp.GetLength()-nIndex-1);
				
		nCount++;
		}

	return nCount + 1;
	}

CString CStringOp::getParam(int nParam, char cDelim)
	{
	CString csTemp;
	int nCount, nIndex;

	csTemp.Empty();
    
    if(IsEmpty())
    	return csTemp;
    
	nCount = nParam;
	csTemp = *this;
	while(1)
		{
		nIndex = csTemp.Find(cDelim);

		if (!nCount)
			break;

		if (nIndex < 0)
			{
			if (nCount)
				csTemp.Empty();
			break;
			}
		else
			if ((csTemp.GetLength()-nIndex-1) < GetLength())
				csTemp = csTemp.Right(csTemp.GetLength()-nIndex-1);
			else
				{
				csTemp.Empty();
				return csTemp;
				}
				
		nCount--;
		}
			
	if (csTemp != "" && nIndex >= 0)
		csTemp = csTemp.Left(nIndex);
			
	return csTemp;
	}
		
unsigned long CStringOp::asLong()
	{
	CStringOp csoTemp;
	char szHex[128];
	int nCount = 0;
	unsigned long lRet = 0L, lPlace = 1;
			
	csoTemp = *this;
	csoTemp.MakeLower();
	csoTemp = csoTemp.getParam(1, 'x');
            
	while (nCount < 128 && nCount < csoTemp.GetLength() && isxdigit(csoTemp[nCount]))
		{
		szHex[nCount] = csoTemp[nCount];
		nCount++;
		szHex[nCount] = 0;
		}
			
	while(nCount--)
		{
		if (isdigit(szHex[nCount]))
			{
			lRet += ((szHex[nCount]-48) * lPlace);
			}
		else
			{
			lRet += ((szHex[nCount]-87) * lPlace);
			}
		lPlace *= 16;
		}
			
	return lRet;
	}

