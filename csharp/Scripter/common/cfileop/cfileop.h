
class CFileOp : public CObject
	{
	protected:
		CString		csFileName;
		CStringList	cslCurrent;
		BOOL m_fChanged;
		BOOL m_bCase;
		
	public:
		CFileOp()
			{
			m_bCase=TRUE;
			m_fChanged = FALSE;
			}
		~CFileOp()
			{
			close();
			}
		BOOL open(LPCTSTR szFile, UINT nFileFlags = NULL);
		BOOL save(LPCTSTR szFile = NULL);
		BOOL close();
		
		POSITION getRoot();
		POSITION getLast();
		POSITION getNext(POSITION pCurrent);
		POSITION getPosition(LPCTSTR szSection, LPCTSTR szKey = NULL);

		POSITION findSection(LPCTSTR szSection);
		POSITION findKey(LPCTSTR szKey, POSITION pCurrent);

		CString getValue(LPCTSTR szSection, LPCTSTR szKey);
		CString getValue(POSITION pCurrent);
		CString getKey(POSITION pCurrent);
		CString getSec(POSITION pCurrent);

		BOOL setValue(LPCTSTR szSection, LPCTSTR szKey, LPCTSTR szValue);
		BOOL setValue(LPCTSTR szValue, POSITION pCurrent);
		BOOL setKey(LPCTSTR szValue, POSITION pCurrent);
		
		BOOL add(LPCTSTR szLine, POSITION pCurrent = NULL);
		BOOL insert(LPCTSTR szLine, POSITION pCurrent = NULL);
		BOOL remove(POSITION pCurrent);
		
		BOOL isChanged();
		BOOL isCaseSensitive()
			{return m_bCase;}
		void setCaseSensitive(BOOL bNew)
			{m_bCase = bNew;}
			
	private:
		void changed();
	};