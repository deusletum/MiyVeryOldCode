
class CVar : public CFileOp
	{
	public:
		CVar() : CFileOp()
			{
			csFileName = "~bogus~.tmp";
			setCaseSensitive(FALSE);
			}
		CString resolveVar(LPCTSTR szData);
	};