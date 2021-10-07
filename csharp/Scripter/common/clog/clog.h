
class CLog : public CFileOp
	{
	public:
		BOOL isDebug()
			{return m_bDebug;}
		void setDebug(BOOL bState)
			{m_bDebug=bState;}
		
		void db(char *szFile, int nLine, char *szProc, LPCTSTR szString, ...);
		void setProgram(LPCTSTR szProg);
	private:
		BOOL m_bDebug;
		CString m_csProgram;
	};