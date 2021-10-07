class CScript : public CObject
	{
	public:
		CScript()
			{
			m_bLocalLog = FALSE;
			m_pTop = NULL;
			m_pcpEngine=NULL;
			m_pclOutput=NULL;
			}
		
		void run();
		
		void setFile(LPCTSTR szFilename)
			{m_cfoFile.open(szFilename);}
			
		void setSec(LPCTSTR szSection)
			{m_pTop = m_cfoFile.findSection(szSection);}

		void setLog(CLog *pclNew)
			{
			if(!m_bLocalLog)
				m_pclOutput = pclNew;
			}
			
		void setProcessor(CProcessor *pcpEngine)
			{
			m_pcpEngine = pcpEngine;
			}
	
	private:
		BOOL m_bLocalLog;
		CLog *m_pclOutput;
		CFileOp m_cfoFile;
		POSITION m_pTop;
		CProcessor *m_pcpEngine;
	};