// ======================================================================
// Copyright . 2004
//
// Module : Inst_Rosetta.js
//
// Summary: Install Rosetta silently
//
// ErrorCodes: 1 - bad user input
//             2 - SQL Server Not installed
//             3 - WMI Error
//             4 - CDS Error
//             5 - MSI install failure
//             6 - MSI install pass requires reboot
//             7 - Unable to read Compatible_Rosetta_Build.ini
//
// History: (6/11/2003) Dean Gjedde - Initial coding
//          (6/19/2003) Dean Gjedde - Fixed 2926
//          (10/22/2003) Mark Yocom - Added "RSUSESSL=0" and swapped
//               VAPP & VDIRECTORY values
//          (10/24/2003) Dean Gjedde - combined Rosetta_Wrapper.vbs with
//              Inst_Rosetta.js, DCR 35106
//          (01/30/2004) John Heaton - Added multi-language support
//          (02/28/2004) Dean Gjedde - Now create Inst_Rosetta.log in %systemdrive%\logs
//          (03/02/2004) Dean Gjedde - DITS Pauses for errors other than 1
//          (04/22/2004) Dean Gjedde - Removed DITSPause
//          (05/28/2004) Dean Gjedde - Added Support for install Rosetta SP1 bug 3536
//          (06/18/2004) Dean Gjedde - Added support for reading build info file for sp1 bug 3725
//          (06/23/2004) Dean Gjedde - Changed sp1 file name and sp1 default path bug 3772
//          (06/25/2004) Dean Gjedde - Fixed miscommunication of when to install sp1 bug 3725
// ======================================================================
var g_sCommandLineParms;

Main();
// -------------------------------------------------------------------
// summary:
//    gets the build number for the product in strProduct given the build token
//
// returns:
//    returns a build number
//
// History: (10/23/2003) Dean Gjedde - Initial coding
// -------------------------------------------------------------------
function GetBuildNum(sProduct, sBuildToken, sLog)
{
    var iCDSError = 4;
    var oCDS, sBuild;

    try
    {
        oCDS = new ActiveXObject("CDSCom.Builds")
        sBuild = oCDS.GetLatestBuildByToken(sProduct, sBuildToken)
        if(sBuild == "")
        {
            WriteLog("VAR_ABORT - There is no " + strBuildToken + " build for product " + strProduct, sLog, iCDSError)
        }
        return sBuild;
    }
    catch(e)
    {
        WriteLog("VAR_ABORT - There was a problem with CDS", sLog, iCDSError)
    }
}

// ======================================================================
// summary:
//    the WriteLog function will write messages to a log file And
//    report errors to the user.
//
// returns:
//    Nothing
//
// History: (6/11/2003) Dean Gjedde - Initial coding
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
//    executes a program or command and returns the errorlevel after the
//       command is run
//
// returns:
//    returns the errorlevel of the command
//
// history:
//  History: (6/11/2003) Dean Gjedde - Initial coding
// ======================================================================
function RunCMD(sCMD)
{
    var oShell, iExitCode;
    var bWaitForExit = true;
    var iMinimize = 6;

    oShell = new ActiveXObject("WScript.Shell");
    WScript.Echo("Running:");
    WScript.Echo(sCMD);
    iExitCode = oShell.Run(sCMD, iMinimize, bWaitForExit);

    return iExitCode;
}

// ======================================================================
// summary:
//    Checks for SQL Server Service
//
//
//returns:
//    Returns true if SQL Server Service is on the box and false if not
//
// history:
//    History: (6/11/2003) Dean Gjedde - Initial coding
// ======================================================================
function CheckForSQL(sLog)
{
    var oLocator, oServices, aInfoSet, sInfo, bResult = false;
    var iSQLError = 3;

    try
    {
        oLocator = new ActiveXObject("Wbemscripting.SWbemlocator");
        oServices = oLocator.ConnectServer(".", "root\\CIMv2");
        aInfoSet = new Enumerator(oServices.InstancesOf("Win32_Service"));

        for (;!aInfoSet.atEnd();aInfoSet.moveNext())
        {
            sInfo = aInfoSet.item();
            if (sInfo.Name.match(/mssql/i) != null)
            {
                bResult = true;
                break;
            }
        }
    }
    catch(e)
    {
        WriteLog("VAR_ABORT - WMI Error:" + e.description + " " + e.number, sLog, iSQLError);
    }
    return bResult;
}

// ======================================================================
// summary:
//    Displays usage
//
// returns:
//    Nothing
//
// history:
//    06/12/03 deangj First Creation
// ======================================================================
function Usage()
{
    WScript.Echo("Usage:");
    WScript.Echo("cscript.exe Inst_Rosetta.js [/BUILD:] [/MOMXBUILD:] [/AUTOSTART:]"
        + " [/VDIRECTORY:] [/VAPP:] [/DBSERVER:] [/DBNAME:] [/PERPROCESSOR|PERSEAT:]"
        + " [/MSILOG:] [LANGUAGE:] [/?]");
    WScript.Echo("/BUILD: - The build number or build token of Rosetta to install"
        + " \n\tsupported build tokens are BLESSED and LATEST\n\tthe default value for"
        + " is BLESSED")
    WScript.Echo("/MOMXBUILD: - The build number or build token of MOMX"
        + " \n\tsupported build tokens are BLESSED and LATEST\n\tthe default value for"
        + " is LATEST")
    WScript.Echo("/AUTOSTART: - {1,0} - 1 enables autostart for the Rosetta service"
        + " and 0 disables autostart for the Rosetta service\n\tthe default value is 1");
    WScript.Echo("/VDIRECTORY: - Name of the virtual directory for IIS\n\t"
        + "the default value is ReportServer");
    WScript.Echo("/VAPP: - Name of the virtual directory application for IIS\n\t"
        + "the default value is Reports");
    WScript.Echo("/DBSERVER: - Specifies the SQL Server instance that hosts the"
        + " report server database\n\tthe default value is %COMPUTERNAME%");
    WScript.Echo("/DBNAME: - Specifies the name of the report server database"
        + " that the report server will use\n\tthe default value is ReportServer");
    WScript.Echo("/PERPROCESSOR:|/PERSEAT: - Specifies the number of per processor"
        + " or perseat licenses purchased\n\tthe default value is /PERPROCESSOR:1000");
    WScript.Echo("/SP1: - Will install Rosetta SP1 after Rosetta is installed");
    WScript.Echo("/I: - Specifies file name and path for the install log file"
        + "\n\tthe default value is %SYSTEMDRIVE%\\logs\\Rosetta_Install.log");
    WScript.Echo("\tSupported Languages are:");
    WScript.Echo("\t\tEnglish - EN\n\t\tChinese simplified - CHS\n\t\tChinese traditional - CHT")
    WScript.Echo("\t\tFrench - FR\n\t\tGerman - DE\n\t\tItalian - IT\n\t\tJapanese - JA")
    WScript.Echo("\t\tKorean  - KO\n\t\tSpanish  - ES")
    WScript.Echo("Example:");
    WScript.Echo("cscript.exe Inst_Rosetta.js /AUTOSTART:1 /VDIRECTORY:TestDir"
        + " /VAPP:TestServer /DBSERVER:TESTDB /DBNAME:ReportServer /PERPROCESSOR:100"
        + " /INSTLOGLOC:%SYSTEMDRIVE%\"\\Rosetta.log\"");
}

// ======================================================================
// summary:
//    Main body of code
//
// returns:
//    Nothing
//
// history:
//    06/12/03 deangj First Creation
// ======================================================================
function Main()
{
    var sTestDrop = "\\\\smx.net\\drop\\MOMX"
    var sProduct = "MOMX"
    var iForReading = 1
    var bCreateFile = false
    var sFile = "x86\\RosettaBuildInfo\\Compatible_Rosetta_Build.ini"
    var sMSILog = "ROSETTA_Install.log";
    var sGrant = "RSGRANTACCOUNT=1";
    var sServer = "\\\\smx.net\\products";
    var sShare = "\\Rosetta";
    var sSetupFile = "\\setup.exe";
    var iUserInputError = 1;
    var iUnableOpenFile = 7;
    var iSQLError = 2;
    var iMSIInstallFailure = 5;
    var iMSIReboot = 6;
    var sProduct = "Rosetta";
    var iRosettaNeedsReboot = 1641;
    //only allow a 1 or a 0
    var regexAutoStart = /^[01]$/;
    //not allow \/ and only 1 to 240 chars (IIS VDir char limit)
    var regexVDirectory = /^[^\\\/]{1,240}$/;
    //not allow \/ and only 1 to 256 chars (IIS VApp char limit)
    var regexVApp = /[^\\\/]{1,256}/;
    // allows 1 to 15 letters, numbers, undrescore, dash or 1 to 16 letters, numbers, undrescore, dash, \
    var regexServer = /^[\w-]{1,15}(\\[\w-]{1,16})?$/;
    //only allow numbers, letters, underscore, dash and only 1 to 127 chars (SQL database name limit)
    var regexDatabase = /^[\w-]{1,127}$/;
    //allow numbers from 1 to 9 chars
    var regexNumbers = /^[0-9]{1,9}$/;
    //allow numbers, lettesr, underscore, dash, space, \
    var regexMSILOG = /^[\w-\s\\]+$/;
    var sAUTOSTART, sDBSERVER, sDBNAME, sPERPROCESSOR;
    var sPERSEAT, sMSILOG, sVDIRECTORY, sVAPP;
    var sBuild, sMOMXBuild, bUseRosettaBuild, bUseMOMXBuild, sSP1Build;
    var oShell = new ActiveXObject("WScript.Shell")
    var sComputerName = oShell.ExpandEnvironmentStrings("%COMPUTERNAME%")
    var sSystemDrive = oShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    var sOSLang = oShell.ExpandEnvironmentStrings("%OSLANG%")
    var sLog = sSystemDrive + "\\logs\\Inst_Rosetta.log";
    var sMSIFileEn = "RSRun.msi"
    var sMSIFileJA = "RSRun_1041.msi"
    var sMSIFileCHS = "RSRun_2052.msi"
    var sMSIFileCHT = "RSRun_1028.msi"
    var sMSIFileFR = "RSRun_1036.msi"
    var sMSIFileDE = "RSRun_1031.msi"
    var sMSIFileIT = "RSRun_1040.msi"
    var sMSIFileKO = "RSRun_1042.msi"
    var sMSIFileES = "RSRun_3082.msi"
    var sMSIFile = "\\X86\\setup\\" + sMSIFileEn;
    var bSP1 = false;
    var bUseINISP1Settings = false;

    bUseRosettaBuild = false;
    bUseMOMXBuild = false;
    sBuild = "BLESSED";
    sSP1Build = "SP1_RTM";
    sMOMXBuild = GetBuildNum("MOMX", "LATEST", sLog);
    sAUTOSTART = 1;
    sVDIRECTORY = "ReportServer";
    sVAPP = "Reports";
    sDBSERVER = sComputerName;
    sDBNAME = "ReportServer";
    sMSILOG = sSystemDrive + "\\logs\\" + sMSILog;

    //Check if Inst_Rosetta is being run under CScript.exe
    if(WScript.FullName.match(/cscript/i) == null)
    {
        WriteLog("VAR_ABORT - This script must be run with CScript.exe", sLog, iUserInputError);
    }

    //Check for args and parse them
    if (WScript.Arguments.length != 0)
    {
        var sResult;
        var aArgs = new Enumerator(WScript.Arguments);
        for (;!aArgs.atEnd();aArgs.moveNext())
        {
            var sArg = aArgs.item();
            var aSplit = sArg.split(":");
            var sSplit = aSplit[1];
            switch (aSplit[0].toUpperCase())
            {
                case "/?":
                    Usage();
                    WriteLog("VAR_ABORT - Usage was reqested", sLog, iUserInputError);
                    break;
                case "/BUILD":
                    sBuild = sSplit;
                    bUseRosettaBuild = true;
                    break;
                case "/MOMXBUILD":
                    var sNewBuild = new String(sSplit);
                    bUseMOMXBuild = true;
                    if(sNewBuild.toUpperCase() == "BLESSED")
                    {
                        sMOMXBuild = GetBuildNum("MOMX", "BLESSED", sLog)
                    }
                    else if(sNewBuild.toUpperCase() == "LATEST")
                    {
                        sMOMXBuild = GetBuildNum("MOMX", "LATEST", sLog)
                    }
                    else
                    {
                        sMOMXBuild = sSplit;
                    }
                    break;
                case "/AUTOSTART":
                    sResult = sSplit.match(regexAutoStart);
                    if (sResult == sSplit)
                    {
                        sAUTOSTART = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /AUTOSTART:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/VDIRECTORY":
                    sResult = sSplit.match(regexVDirectory);
                    if (sResult == sSplit)
                    {
                        sVDIRECTORY = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /VDIRECTORY:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/VAPP":
                    sResult = sSplit.match(regexVApp);
                    if (sResult == sSplit)
                    {
                        sVAPP = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /VAPP:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/DBSERVER":
                    var oRegEx = new RegExp(regexServer);
                    if(oRegEx.test(sSplit))
                    {
                        sDBSERVER = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /DBSERVER:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/DBNAME":
                    sResult = sSplit.match(regexDatabase);
                    WScript.Echo(sSplit + " " + sResult);
                    if (sResult == sSplit)
                    {
                        sDBNAME = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /DBNAME:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/PERPROCESSOR":
                    sResult = sSplit.match(regexNumbers);
                    if (sResult == sSplit)
                    {
                        sPERPROCESSOR = sSplit; 
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /PERPROCESSOR:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/PERSEAT":
                    sResult = sSplit.match(regexNumbers);
                    if (sResult == sSplit)
                    {
                        sPERSEAT = sSplit;
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /PERSEAT:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/MSILOG":
                    sResult = sSplit.match(regexMSILOG);
                    if (sResult == sSplit)
                    {
                        if(sSplit.length >= 2)
                        {
                            sMSILOG = sSplit + ":" + aSplit[2];
                        }
                        else
                        {
                            sMSILOG = sSplit
                        }
                    }
                    else
                    {
                        WriteLog("VAR_ABORT - " + sSplit + " is not a valid value for /MSILOG:"
                        , sLog, iUserInputError);
                    }
                    break;
                case "/SP1":
                    bSP1 = true;
                    break;
                default:
                    WriteLog("VAR_ABORT - " + aSplit[0] + " is not a valid switch"
                    , sLog, iUserInputError);
                    break;
            }
        }
    }

    // Check if SQL Server is installed on the computer
    if (!CheckForSQL(sLog))
    {
        WriteLog("VAR_ABORT - SQL Server is not installed on the box", sLog, iSQLError);
    }

    if((bUseMOMXBuild) && (bUseRosettaBuild))
    {
        WriteLog("VAR_ABORT - /BUILD: and /MOMXBUILD: cannot be used together, you must use one or the other"
        , sLog, iUserInputError);
    }

    if((!bUseMOMXBuild) && (!bUseRosettaBuild) && (!bSP1))
    {
        bUseMOMXBuild = true;
        bSP1 = true;
        bUseINISP1Settings = true;
    }
    else if((!bSP1) && (bUseMOMXBuild))
    {
        bUseINISP1Settings = true;
        bSP1 = true;
    }

    if((!sPERPROCESSOR) && (!sPERSEAT))
    {
        sPERPROCESSOR = 1000;
    }

    if((sPERPROCESSOR) && (sPERSEAT))
    {
        WriteLog("VAR_ABORT - both /PERPROCESSOR: and /PERSEAT: switches where specified"
        , sLog, iUserInputError);
    }

    var sRosettaBuilds;
    if(bUseMOMXBuild)
    {
        var oFSO = new ActiveXObject("Scripting.FileSystemObject")
        var sRosettaINIFile = sTestDrop + "\\" + sMOMXBuild + "\\" + sFile
        if(oFSO.FileExists(sRosettaINIFile))
        {
            try
            {
                var oStream = oFSO.OpenTextFile(sRosettaINIFile, iForReading, bCreateFile);
                var sTempVal;
                sTempVal = oStream.ReadAll();
                oStream.Close();
                sRosettaBuilds = sTempVal.split(";");
                if((bSP1) && (bUseINISP1Settings))
                {
                    if(sRosettaBuilds.length == 1)
                    {
                        bSP1 = false;
                    }
                    else
                    {
                        sSP1Build = sRosettaBuilds[1];
                    }
                }
                sBuild = sRosettaBuilds[0]
            }
            catch(e)
            {
                WriteLog("VAR_ABORT - Inst_Rosetta.js was unable to open file " + sRosettaINIFile
                    , sLog, iUnableOpenFile);
            }
        }
        else
        {
            WriteLog("VAR_ABORT - Inst_Rosetta.js was unable to open file " + sRosettaINIFile
                + "\nMOMX Build number " + sMOMXBuild + " may not exist", sLog, iUnableOpenFile);
        }
    }

    //Detect OS Language
    var Lang = sOSLang;
    switch (Lang.toUpperCase())
    {
        case "EN":
            sMSIFile = "\\X86\\setup\\" + sMSIFileEn;
            break;
        case "CHS":
            sMSIFile = "\\X86\\setup\\" + sMSIFileCHS;
            break;
        case "CHT":
            sMSIFile = "\\X86\\setup\\" + sMSIFileCHT;
            break;
        case "FR":
            sMSIFile = "\\X86\\setup\\" + sMSIFileFR;
            break;
        case "DE":
            sMSIFile = "\\X86\\setup\\" + sMSIFileDE;
            break;
        case "IT":
            sMSIFile = "\\X86\\setup\\" + sMSIFileIT;
            break;
        case "JA":
            sMSIFile = "\\X86\\setup\\" + sMSIFileJA;
            break;
        case "KO":
            sMSIFile = "\\X86\\setup\\" + sMSIFileKO;
            break;
        case "ES":
            sMSIFile = "\\X86\\setup\\" + sMSIFileES;
            break;
        default:
            sMSIFile = "\\X86\\setup\\" + sMSIFileEn;
            break;
    }

    //Build the command string to execute
    var sCMD = sServer + sShare +  "\\" + sBuild + "\\X86\\setup.exe /i " + sServer + sShare + "\\"
                + sBuild + sMSIFile + " RSAUTOSTART=" + sAUTOSTART
                + " RSVIRTUALDIRECTORYSERVER=\"" + sVDIRECTORY + "\" RSVIRTUALDIRECTORYMANAGER=\""
                + sVAPP + "\" RSDATABASESERVER=\"" + sDBSERVER
                + "\" RSDATABASENAME=\"" + sDBNAME + "\"" + " RSUSESSL=0";

    if(sPERSEAT)
    {
        sCMD += " PERSEAT=" + sPERSEAT;
    }
    else
    {
        sCMD += " PERPROCESSOR=" + sPERPROCESSOR;
    }
    sCMD += " " + sGrant + " /qn /l*v \"" + sMSILOG + "\"";

    var iError = RunCMD(sCMD);
    if(iError == iRosettaNeedsReboot)
    {
        WriteLog("VAR_PASS - Rosetta install passed but required a reboot", sLog, iMSIReboot);
    }
    else if(iError != 0)
    {
        WriteLog("VAR_FAIL - Rosetta install failed with error number:" + iError, sLog, iMSIInstallFailure);
    }
    else
    {
        if(bSP1)
        {
            sCMD = sServer + sShare + "\\" + sSP1Build + "\\SQL2KRSSP1-ENG.EXE /q"
            var iSP1Error = RunCMD(sCMD);
            if (iSP1Error != 0)
            {
                WriteLog("VAR_FAIL - Rosetta SP1 install failed with error number:" + iError
                , sLog, iMSIInstallFailure);
            }
            else
            {
                WriteLog("VAR_PASS - Rosetta w/SP1 install passed", sLog, 0);
            }
        }
        else
        {
            WriteLog("VAR_PASS - Rosetta install passed", sLog, 0);
        }
    }
}