// ======================================================================
//
// NAME: Inst_VS.js
//
// AUTHOR: Dean Gjedde , Microsoft
// DATE  : 1/28/2005
//
// COMMENT: Installs VS 8 Whidbey
//
// ======================================================================
var g_sCommandLineParms = "";
WScript.Quit(Main())

// ======================================================================
// summary:
//    executes a program or command and returns the errorlevel after the
//       command is run
// ======================================================================
function RunCMD(sCMD)
{
    var oShell, iExitCode;
    var bWaitForExit = true;
    var iMinimize = 6;

	//var oFSO = new ActiveXObject("Scripting.FileSystemObject");
    oShell = new ActiveXObject("WScript.Shell");
    WScript.Echo("Running:");
    WScript.Echo(sCMD);
    try
    {
    	iExitCode = oShell.Run(sCMD, iMinimize, bWaitForExit);
    }
    catch(e)
    {
    	WScript.Echo("There was an error: " + e.description)
    }

    return iExitCode;
}

// ======================================================================
// summary:
//   Gets the path to the MSDN setup file
// ======================================================================
function GetMSDNPath(sSetupINIFile, sLog)
{
    var ForReading = 1;
    var iRetry = 3;
    var iSleepTime = 5000;
    var DontCreateFile = false;
    var iError = 4;
    var iTristateTrue = -1;
    var MSDNDocLine = "[Documentation]"
    var oStream, sContents;
    var oFSO = new ActiveXObject("Scripting.FileSystemObject");

	for(var i = 0;i < iRetry;i++)
	{
	    if(!oFSO.FileExists(sSetupINIFile))
	    {
	    	WScript.Sleep(iSleepTime)
	    }
	    else
	    {
	    	break;
	    }
	}
    try
    {
    	oStream = oFSO.OpenTextFile(sSetupINIFile, ForReading, DontCreateFile, iTristateTrue);
    	while(!oStream.AtEndOfStream)
    	{
    		sContents = oStream.ReadLine();
    		if(sContents == MSDNDocLine)
    		{
    			sContents = oStream.ReadLine();
    			var aSplit = sContents.split("=")
    			sContents = aSplit[1]
    			var aArgs = new Enumerator(sContents.split("\\"));
    			sContents = "";
        		for (;!aArgs.atEnd();aArgs.moveNext())
        		{
          			var sArg = aArgs.item();
        			if(sArg !="..")
        			{
        				sContents += "\\" + sArg;
        			}
        		}

    			break;
    		} 
    	}
    	oStream.Close();
    }
    catch(e)
    {
    	WriteLog("VAR_ABORT - Error reading file: " + sSetupINIFile + " - " + e.description + " " + e.number, sLog, iError);
    }
    return "\\\\cpvsbuild\\drops\\" + sContents;
}
 
// ======================================================================
// summary:
//    the WriteLog function will write messages to a log file And
//    report errors to the user.
// ======================================================================
function WriteLog(sMessage, sLog, iExitCode)
{
    var ForAppending = 8;
    var oEnv, oFSO, oStream, oDate, sDate, oShell, sWINDIR;
    var bOverWrite = true;

    try
    {
        oDate = new Date();
        sDate = (oDate.getMonth() + 1) + "/";
        sDate += oDate.getDate() + "/";
        sDate += oDate.getYear() + " ";
        sDate += oDate.getHours() + ":";
        if (oDate.getMinutes() <= 9)
        {
            sDate += "0" + oDate.getMinutes();
        }
        else
        {
            sDate += oDate.getMinutes();
        }

        oFSO = new ActiveXObject("Scripting.FileSystemObject");
        oShell = new ActiveXObject("WScript.Shell");
        sWINDIR = oShell.ExpandEnvironmentStrings("%WINDIR%");
        oStream = oFSO.OpenTextFile(sLog, ForAppending, bOverWrite);
        oStream.Write("\nInst_Rosetta.js command line parameter(s) are:\n");
        oStream.Write(g_sCommandLineParms);
        oStream.Write("\n*LOG_START*-Inst_Rosetta");
        if(iExitCode == 0)
        {
            oStream.Write("[" +  sDate + "] Inst_Rosetta " + sMessage);
        }
        else
        {
            oStream.Write("[" +  sDate + "] ERR Inst_Rosetta " + sMessage);
        }
        oStream.Write("\n*LOG_DONE*");
        oStream.Close();
        WScript.Echo(sMessage);
        WScript.Quit(iExitCode);
    }
    catch(e)
    {
        WScript.Echo("Inst_Rosetta was unable to write a log");
        WScript.Echo(e.description);
    }
}

// ======================================================================
// summary:
//    Displays usage
// ======================================================================
function Usage()
{
    WScript.Echo("Usage:");
    WScript.Echo("cscript.exe Inst_VS.js /BUILD: [/NOSQL] [/SKU:]");
    WScript.Echo("/BUILD: - The build number of VS Whidbey you would like to install")
    WScript.Echo("/NOSQL - Do not install SQL Express")
    WScript.Echo("/SKU: - The product sku to install - PRO|VSTS|STD")
    WScript.Echo("Example:");
    WScript.Echo("cscript.exe Inst_Rosetta.js /BUILD:50117.00 /NOSQL /SKU:PRO");
}

function Main()
{
	var ForReading = 1;
	var ForAppending = 8;
    var DontCreateFile = false;
    var iTristateTrue = -1;
	var sVSWhidbeyDrop = "\\\\cpvsbuild\\drops\\Whidbey"
	var sLab22DevPath = "\\\Lab22dev\\layouts\\x86ret"
	var sInstallPath = "\\enu\\vs\\pro\\cd\\NetSetup"
    var iForReading = 1
    var bCreateFile = false
    var sProFile = "vspro.ini"
    var sVSTSFile = "vsts.ini"
    var sSTDFile = "std.ini"
    var sSetupINI = "setup.ini"
    var sMSILog = "VS_Install.log";
    var sServer = "\\\\vspulabsrv\\simaddog\\misc";
    var sSetupFile = "\\setup.exe";
    var sComponentDir = "\\WCU";
    var sRunMSI = "\\runmsi.exe";
    var sSmartDeviceEMU = "\\ARM\\vs_emulator.exe";
    var sJSharp = "\\JSharpRedistCore\\vjredist.exe";
    var sSQLCE = "\\SQLCE\\sqlmobile30devtoolsenu.msi";
    var sNETCF = "\\NETCF\\NetCFSetupv2.msi";
    var sOfficeTools = "\\VS Tools for Office\\vstor.exe";
    var sMSDN = "\\msdn.msi";
    var sInstallParms = " /T:%TEMP% /C:\"%TEMP%\\install.exe /q\"";
    var sMSDNPreParms = "msiexec.exe /I ";
    var iUserInputError = 1;
    var iUnableOpenFile = 7;
    var iMSIInstallFailure = 5;
    var iMSIReboot = 6;
	var sBuild, bNoSQL, sResult, sSku, oFSO, oStream, sContents, sFile, iError;
	var sBranch;
	var regexBuildNumbers = /^[0-9]{5}\.[0-9]{2}$/;
    var oShell = new ActiveXObject("WScript.Shell")
    var sComputerName = oShell.ExpandEnvironmentStrings("%COMPUTERNAME%")
    var sSystemDrive = oShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    var sOSLang = oShell.ExpandEnvironmentStrings("%OSLANG%")
    var sTempDir = oShell.ExpandEnvironmentStrings("%TEMP%")
    var sMSDNPostParms = " SETUP_EXE=\"yes\" /qb"
    var sLog = sTempDir + "\\Inst_Rosetta.log";

    bNoSQL = false;
    sSku = "PRO"
    sFile = sProFile;
    sBranch = "lab22dev";
	
    //Check if Inst_Rosetta is being run under CScript.exe
    if(WScript.FullName.match(/cscript/i) == null)
    {
        WriteLog("VAR_ABORT - This script must be run with CScript.exe", sLog, iUserInputError);
    }
    
    //Check for args and parse them
    if (WScript.Arguments.length == 0)
    {
    	Usage();
    	WriteLog("VAR_ABORT - Usage was reqested", sLog, iUserInputError)
    }
    else
    {
    	var sResult;
        var aArgs = new Enumerator(WScript.Arguments);
        for (;!aArgs.atEnd();aArgs.moveNext())
        {
            var sArg = aArgs.item();
            g_sCommandLineParms += " " & sArg
            var aSplit = sArg.split(":");
            var sSplit = aSplit[1];
            switch (aSplit[0].toUpperCase())
            {
                case "/?":
                    Usage();
                    WriteLog("VAR_ABORT - Usage was reqested", sLog, iUserInputError);
                    break;
                case "/BUILD":
                    sResult = sSplit.match(regexBuildNumbers);
                    if (sResult == sSplit)
                    {
                        sBuild = sSplit;

                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /BUILD:"
                        	, sLog, iUserInputError);
                    }
                    break;
                case "/NOSQL":
                	bNoSQL = true;
                	break;
               	case "/SKU":
               		sSku = sSplit.toUpperCase();
               		switch(sSku)
               		{
               			case "PRO":
               				break;
               			case "VSTS":
               				break;
               			case "STD":
               				break;
               			default:
               				WriteLog("VAR_ABORT - " + sSku + " is not a valid value for /SKU:"
                        		, sLog, iUserInputError);
                        	break;
               		}
               		if(sSku == "VSTS")
               		{
               			sFile = sVSTSFile; 
               		}
               		else if(sSku == "STD")
               		{
               			sFile = sSTDFile;
               		}
               		break;
            }
        }
    }
    
    // VSTS support to be added later
    var sVSWhidbeyDrop = "\\\\cpvsbuild\\drops\\Whidbey\\" + sBranch + "\\layouts\\x86ret\\"
    	+ sBuild + "\\enu\\vs\\" + sSku + "\\cd\\NetSetup"
    
    try
    {	
	   	oFSO = new ActiveXObject("Scripting.FileSystemObject");
	   	oStream = oFSO.OpenTextFile(sServer + "\\" + sFile, ForReading, DontCreateFile, iTristateTrue);
	   	if(oFSO.FileExists(sTempDir + "\\" + sFile))
	   	{
	   		oFSO.DeleteFile(sTempDir + "\\" + sFile);
	   	}
	   	oNewStream = oFSO.OpenTextFile(sTempDir + "\\" + sFile, ForAppending, true, iTristateTrue);
	   	while(!oStream.AtEndOfStream)
	   	{
	   		sContents = oStream.Readline();
	   		var regex = "^InstallDirectory=[A-Z]{1}:";
	   		if(bNoSQL)
	   		{
	   			if(sContents  == "gfn_mid sse")
	   			{
	   				oNewStream.WriteLine("");
	   			}
	   			else if(sContents.match(regex))
	   			{
	   				oNewStream.WriteLine(sContents.replace(/[a-zA-Z]{1}:/,sSystemDrive));
	   			}
	   			else
	   			{
	   				oNewStream.WriteLine(sContents);
	   			}
	   		}
	   		else
	   		{
	   			if(sContents.match(regex))
	   			{
	   				oNewStream.WriteLine(sContents.replace(/[a-zA-Z]{1}:/,sSystemDrive));
	   			}
	   			else
	   			{
	   				oNewStream.WriteLine(sContents);
	   			}
	   		}
	   	}
	   	oStream.Close();
	   	oNewStream.Close();
	}
	catch(e)
	{
		WriteLog("There was an error creating VS unattend file", sLog, iUserInputError);
	}
   	
  	var MSDNPath = GetMSDNPath(sVSWhidbeyDrop + "\\" + sSetupINI, sLog);
   	//install VS
   	iError = RunCMD(sVSWhidbeyDrop + "\\setup\\setup.exe /unattendfile " + sTempDir + "\\" + sFile)
   	if(iError != 0)
   	{
   		WriteLog("There was an install failure installing VS", sLog, iMSIInstallFailure);
   	}
   	//Install JSharp
   	iError = RunCMD(sVSWhidbeyDrop + sComponentDir + sJSharp + sInstallParms)
	if(iError != 0)
   	{
   		WriteLog("There was an install failure installing JSharp Redist", sLog, iMSIInstallFailure);
   	}
   	//Install .NET Compact framework
   	iError = RunCMD(sVSWhidbeyDrop + sComponentDir + sRunMSI + " " + sVSWhidbeyDrop + sComponentDir + sNETCF)
   	if(iError != 0)
   	{
   		WriteLog("There was an install failure installing .NET Compact framework", sLog, iMSIInstallFailure);
   	}
   	//Install SQL for smart dev
   	iError = RunCMD(sVSWhidbeyDrop + sComponentDir + sRunMSI + " " + sVSWhidbeyDrop + sComponentDir + sSQLCE)
   	if(iError != 0)
   	{
   		WriteLog("There was an install failure installing SQL for Moblie Devices", sLog, iMSIInstallFailure);
   	}
   	//Install Smart Device Emulator
   	iError = RunCMD(sVSWhidbeyDrop + sComponentDir + sSmartDeviceEMU + sInstallParms)
   	if(iError != 0)
   	{
   		WriteLog("There was an install failure installing Smart Device Emulator", sLog, iMSIInstallFailure);
   	}
   	//Install Office Runtime tools
   	if(sSku == "VSTS")
   	{
   		iError = RunCMD("\"" + sVSWhidbeyDrop + sComponentDir + sOfficeTools + "\"" + sInstallParms)
   		if(iError != 0)
	   	{
	   		WriteLog("There was an install failure installing Office Runtime tools", sLog, iMSIInstallFailure);
	   		WScript.Echo("%ERRORLEVEL% = " + iError)
	   	}
   	}
	//Install MSDN
	if(sSku != "STD")
	{
	   	iError = RunCMD(sMSDNPreParms + MSDNPath + sMSDN + sMSDNPostParms)
	   	if(iError != 0)
	   	{
	   		WriteLog("There was an install failure installing MSDN", sLog, iMSIInstallFailure);
	   	}
	}   	
   	WriteLog("Inst_VS.js finished without error", sLog, 0);
}