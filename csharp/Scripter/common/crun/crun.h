
class CRun : public CProcessor
	{
	public:
		CRun()
			{
			m_nRun=0;
			m_bLocalVars = FALSE;
			m_bLocalLog = FALSE;
			}

		~CRun()
			{
			m_cfoIniFile.save();
			m_cfoIniFile.close();
			}
		
		void exec();

	private:
		void callFunction(LPCTSTR szFunction, LPCTSTR szParams);
		
		BOOL add(LPCTSTR szParams);
		BOOL remove(LPCTSTR szParams);
		BOOL copy(LPCTSTR szParams);
		BOOL insert(LPCTSTR szParams);
		BOOL toLower(LPCTSTR szParams);
		BOOL toUpper(LPCTSTR szParams);
		BOOL fileAppend(LPCTSTR szParams);
		BOOL getParam(LPCTSTR szParams);
		BOOL makeName(LPCTSTR szParams);

	private:
		#define PARAMDELIM '}'
		
		int m_nRun;
		CStringOp m_csoFile;
		CStringOp m_csoDepend;
		CStringOp m_csoFunction;
		CStringOp m_csoParams;
		
		CFileOp m_cfoIniFile;

	};