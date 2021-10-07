#include "..\common\common.h"
#include "..\common\cprcssr\cprcssr.h"
#include "..\common\crun\crun.h"
#include "..\common\cscript\cscript.h"

void main(int argc, char *argv[], char *envp[])
	{
	CLog clOutput;
	CScript cscrInput;
	CRun crCurrent;
	CVar cvStore;
	char *szParam;
	int nCount;
	CStringOp csoParam;

	clOutput.setDebug(FALSE);

	nCount = 0;
	while (envp[nCount] && *(envp[nCount]))
		cvStore.add(envp[nCount++]);
	
	crCurrent.setLog(&clOutput);
	crCurrent.setVarList(&cvStore);
	
	cscrInput.setProcessor(&crCurrent);
	cscrInput.setLog(&clOutput);

	for (nCount=1; nCount < argc; nCount++)
		{
		
		ASSERT(argv[nCount] && *argv[nCount]);
		
		csoParam = argv[nCount];
		csoParam.MakeLower();
		
		//clOutput.db(__FILE__,__LINE__,"main","param %d = |%s|", nCount, csoParam);

		if (csoParam[0] == '/' || csoParam[0] == '-')
			{
			csoParam = csoParam.Right(csoParam.GetLength()-1);
			
			szParam = csoParam.GetBuffer(1);
			switch(csoParam[0])
				{
				case 'd':
					clOutput.setProgram("Scripter");
					clOutput.setDebug(TRUE);
					clOutput.db(__FILE__,__LINE__,"main","Debug ON!");
					break;
				default:
					if (!strncmp(szParam, "list", 4) && *((argv[nCount])+6))
						{
						cscrInput.setFile((argv[nCount])+6);
						clOutput.db(__FILE__,__LINE__,"main","List:%s:", (argv[nCount])+6);
						}
					if (!strncmp(szParam, "sec", 3) && *((argv[nCount])+5))
						{
						cscrInput.setSec((argv[nCount])+5);
						clOutput.db(__FILE__,__LINE__,"main","Sec:%s:", (argv[nCount])+5);
						}
				}
			csoParam.ReleaseBuffer(-1);

			}
		else
			{
			//clOutput.db(__FILE__,__LINE__,"main","exec |%s|", csoParam);
			crCurrent.open(csoParam);
			crCurrent.exec();
			}
		}
	
	cscrInput.run();
	
	}