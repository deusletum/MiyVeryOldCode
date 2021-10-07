
class CStringOp : public CString
	{
	
	public:
		unsigned long asLong();
		CString getParam(int nParam, char cDelim);
		int getNumParam(char cDelim);
		CString trimRight();
		CString trimLeft();
		
		CStringOp& operator =(const CStringOp& stringSrc)
			{
			//AssignCopy(stringSrc.m_nDataLength, stringSrc.m_pchData);
			AssignCopy(stringSrc.GetLength(), stringSrc.m_pchData);
			return *this;
			}
		
		CStringOp& operator =(const char* psz)
			{
			AssignCopy(SafeStrlen(psz), psz);
			return *this;
			}
	
	
	};

