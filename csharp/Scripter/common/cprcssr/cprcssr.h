class CProcessor : public CObject
	{ 
	public:
		void open(LPCTSTR szFilename);
		virtual void exec();

		void setLog(CLog *pclNew)
			{
			if(!m_bLocalLog)
				m_pclOutput = pclNew;
			}
		
		void setVarList(CVar *pcvNew)
			{
			if(!m_bLocalVars)
				m_pcvStore = pcvNew;
			}
		
	protected:
		CFileOp m_cfoScript;
		
		BOOL m_bLocalVars;
		CVar *m_pcvStore;

		BOOL m_bLocalLog;
		CLog *m_pclOutput;

	};