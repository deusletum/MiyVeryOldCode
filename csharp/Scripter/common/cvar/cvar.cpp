#include "..\common.h"

CString CVar::resolveVar(LPCTSTR szData)
	{
	int nStart, nEnd;
	CString csReturn, csData, csTemp;
	
	csReturn.Empty();
	
	if (!szData || !(*szData))
		{
		return csReturn;
		}
	
	ASSERT(szData && *szData);
	
	csData = szData;
	
	while(1)
		{
		nStart = csData.Find('%');
		
		if(nStart < 0)
			break;
		
		csReturn += csData.Left(nStart);
        
        if ((csData.GetLength()-nStart-1) >= 0)
        	csData = csData.Right(csData.GetLength()-nStart-1);
		
		nEnd = csData.Find('%');

		if(nEnd < 0)
			break;
		
		csTemp = csData.Left(nEnd);
		if ((csData.GetLength()-nEnd-1) >= 0)
			csData = csData.Right(csData.GetLength()-nEnd-1);
		
		if (!nEnd)
			{
			csReturn += "%";
			}
		else
			{
			csReturn += getValue(NULL, csTemp);
			}
		}
	
	csReturn += csData;
	return csReturn;
	}

