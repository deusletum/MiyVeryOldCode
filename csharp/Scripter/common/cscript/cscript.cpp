#include "..\common.h"
#include "..\cprcssr\cprcssr.h"
#include "cscript.h"

void CScript::run()
	{
	CString csScript;
	POSITION pCurrent;
	
//	m_pclOutput->db(__FILE__,__LINE__,"run","Running...");
	
	if (!m_pcpEngine || !m_pTop)
		return;

	if (!m_pclOutput)
		{
		m_pclOutput = new CLog;
		m_pclOutput->setProgram("CScript");
		m_pclOutput->setDebug(FALSE);
		m_bLocalLog = TRUE;
		}
	
	pCurrent = m_pTop;
	
	while(pCurrent = m_cfoFile.getNext(pCurrent))
		{
		csScript = m_cfoFile.getKey(pCurrent);
		
//		m_pclOutput->db(__FILE__,__LINE__,"run","exec? |%s|", csScript);
		
		if (!csScript.GetLength())
			continue;
				
		if (csScript[0] == '[')
			break;
		
		m_pclOutput->db(__FILE__,__LINE__,"run","exec |%s|", csScript);
		m_pcpEngine->open(csScript);
		m_pcpEngine->exec();
		}
	
	if(m_bLocalLog)
		{
		delete m_pclOutput;
		m_pclOutput = NULL;
		m_bLocalLog = FALSE;
		}
	return;
	}

