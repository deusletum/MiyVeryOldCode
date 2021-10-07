'==========================================================================
'
' VBScript Source File -- Created with SAPIEN Technologies PrimalSCRIPT(TM)
'
' NAME: PrivateFiles.vbs
'
' AUTHOR: Glenn LaVigne (glennlav)
' DATE  : 05/12/03
'
' COMMENT: System for replacing files on a machine. Designed for testing private
'   version of both product and O/S files in an efficent manner.
'
' See spec for complete details on operation, algorithm and usage.
'
'   Versions:
'       1.1: DCR to copy down symbols for any file replaced (if avail)
'       1.2: Added WMI code to find and stop all services / processes
'            that have the private files loaded. (MARKING)
'==========================================================================

'-------------------------------------------------------------------
'
' <summary>
'    Create global objects including log and error file references
' </summary>
'
' <remarks>
'     Create the objects Shell and FileSystemObjects that can be used globally.
'         Prevents having to recreate and destroy frequently
'
'     Create the LogFile and ErrFile - again can be used globally.
'         Prevents having to open and close file everytime you want to write.
'         Note - If program abnormally terminates, this can leave open process
'         lock on the files.
'
' </remarks>
'
'-------------------------------------------------------------------
Option Explicit

Const PROG_DESC = "PrivateFiles - V 1.2"
Const wbemFlagReturnImmediately = &h10
Const wbemFlagForwardOnly = &h20
Const sleepTimeMS = 10000
Const numLoops = 12


Dim g_WshShell
Set g_WshShell = WScript.CreateObject("WScript.Shell")

Dim g_refFSO
Set g_refFSO = CreateObject("Scripting.FileSystemObject")


' Make file handles to log and error file global to avoid lots of passing around
' Also declare global flag to determine If we need to clean up error file at End (e.g. any errors logged)
Dim g_LogFile, g_ErrFile
Dim g_ErrFileUsed
g_ErrFileUsed = False

' Global flag to determine if should print out verbose logging to screen
Dim g_bVerbose
g_bVerbose = False

WScript.Quit main()

'
' <summary>
'    the main Function gets everything started.
' </summary>
'
' <returns>
'    Error Code
'   0 = Success
'   1 = One or more private version of files not found on target (excluded known symbol files)
'   2 = Folder containing privates not at location specified
'   3 = Target path to replace files in does not exist
'   4 = Post-private command line returned non-zero value or post-private file does not exist
'   5 = User input error (bad input arguement combos, etc)
' </returns>
'
Function main()

    ' Default return code
    main=0

    If (InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) = 0) Then
        WScript.Echo "Only runs under CScript - Please run 'CScript PrivatesInstall.vbs [args...]'"
        Usage()
        main = 5
        Exit Function
    End If

    Dim argsNamed
    Set argsNamed = WScript.Arguments.Named

    If (argsNamed.Count = 0) Then
        Usage()
        main = 0
        Exit Function
    End If


    ' Variable to hold user arguement - fill in appropriate defaults
    Dim pErrorFile, pLogFile
    pLogFile = "PrivateFiles.log"
    pErrorFile = "PrivateFiles.err"

    Dim pPrivateRootPath, pPrivateFolder, pTarget, pPostPriv, pPrivateLoc
    pPrivateRootPath = "\\smxfiles\privates"
    pPrivateFolder = Empty
    pPrivateLoc = Empty
    pTarget = Empty
    pPostPriv = Empty

    Dim bOS, bReboot
    bOS = False
    bReboot = True

    ' Decode user input
    Dim arg
    For Each arg In argsNamed
        Select Case LCASE(arg)
            Case "?"
                Usage
                main = 0
                Exit Function
            Case "help"
                Usage
                main = 0
                Exit Function
            Case "privshare"
                pPrivateRootPath = argsNamed.item(arg)
            Case "privfolder"
                pPrivateFolder = argsNamed.item(arg)
            Case "postpriv"
                pPostPriv = argsNamed.item(arg)
            Case "os"
                bOS = True
            Case "verbose"
                g_bVerbose = True
            Case "logfile"
                pLogFile = argsNamed.item(arg)
            Case "errorfile"
                pErrorFile = argsNamed.item(arg)
            Case "target"
                pTarget = argsNamed.item(arg)
            Case "noreboot"
                bReboot = False
            Case Else
                WScript.Echo "Cannot find variable " & arg & ", please check spelling..."
                Usage
                main = 5
                Exit Function
        End Select
    Next

    ' Create log and error file if no argument errors found
    main = OpenLogFiles(pLogFile, pErrorFile)

    ' If argument errors found or problem opening log/error files, just exit now
    If (main <> 0) Then
        Exit Function
    End If

    ' If doing O/S replace, make sure user did not specify incompatible flags
    If (bOS) Then
        If (pTarget <> Empty) Then
            LogError "Invalid input combination - Can not specify both /os:True and a target path", True
            Main = 5
        End If
        If (bReboot = False) Then
            LogError "Invalid input combination - Can not specify both /os:True and /reboot:False", True
            Main = 5
        End If
        ' O/S replacement - put in systemroot/system32 as target folder
        ' Magic Number "1" is constant specifying systemfolder
        pTarget = g_refFSO.GetSpecialFolder(1)
    End If

    ' Create path to private file folder and verify it exists
    pPrivateLoc = g_refFSO.BuildPath(pPrivateRootPath,pPrivateFolder)
    If (g_refFSO.FolderExists(pPrivateLoc) = False) Then
        LogError "Can not find Privates folder at " & pPrivateLoc, True
        Main = 2
    End If

    ' Check to see target replacement location exists
    If ((bOS = False) and (g_refFSO.FolderExists(pTarget)= False)) Then
        LogError "Can not find target replacement folder:" & pTarget, True
        Main = 3
    End If

   ' Check to see post-replacement execution file exists
    If ((pPostPriv <> Empty) and (g_refFSO.FileExists(pPostPriv)= False)) Then
        LogError "Can not find post-replacement execution file:" & pPostPriv, True
        Main = 4
    End If

    ' If no errors, do the file replacement operations
    If (main = 0) Then

        ' Print out some feedback on operations to user
        LogInfo "",True
        LogInfo "Private Files located at: " & pPrivateLoc, True
        If (bOS) Then
            LogInfo "Replacing O/S files (" & pTarget & ")", True
        Else
            LogInfo "Replacing files at: " & pTarget, True
        End If
        If (pPostPriv <> Empty) Then
            LogInfo "After replacements done, will run: " & pPostPriv, True
        End If
        If (bReboot) Then
            LogInfo "Will reboot system on completion", True
        Else
            LogInfo "Will not reboot system on completion", True
        End If
        LogInfo "", True

        Dim objFiles, curFile, itemFile
         ' Go through all the files in this directory
        Set objFiles = g_refFSO.GetFolder(pPrivateLoc)
        For Each itemFile in objFiles.Files
            LogInfo "Private File: " & itemFile.Name, True
            curFile = itemFile.Name
            ' Don't worry about symbol files - they will be copied when corresponding binary copied
            If (LCASE(g_refFSO.GetExtensionName(curFile)) <> "pdb") And _
               (LCASE(g_refFSO.GetExtensionName(curFile)) <> "sym") And _
               (LCASE(g_refFSO.GetExtensionName(curFile)) <> "dbg") Then
                ' For every locally installed file, see if a private version exists and replace with it
                ' If replacement False and not a symbols file, set error code
                If (ReplacePrivate(pPrivateLoc, curFile, pTarget, bOS, False) = False) Then
                    LogError "   " & curFile & " - NOT FOUND ON TARGET (no replacement)", False
                    main = 1
                End If
            Else
                LogInfo "   <Symbol file> " & curFile, g_bVerbose
            End If
            LogInfo "", True
        Next

        ' Run the post private replacement command line
        If (pPostPriv <> Empty) Then
            LogInfo "", True
            LogInfo "Running: " & pPostPriv, True
            ' Magic Number "1" specifies run in displayed Window.
            ' "True" indicates what for process to finish
            If (g_WshShell.Run("cmd.exe /c " & pPostPriv,1,True) <> 0) Then
                main = 4
            End If
        End If

        ' Get rid of zero byte error file (no errors were logged)
        g_ErrFile.Close
        If (g_ErrFileUsed = False) Then
            g_refFSO.DeleteFile pErrorFile, True
        End If

        ' Reboot machine if necessary.
        If (bReboot) Then
            If (main <> 0) Then
                LogInfo "One or more errors detected - Reboot Aborted", True
                LogInfo "NOTE: To complete the file replacement, please REBOOT machine", True
            Else
                LogInfo "PrivateFiles Rebooting machine", True
                ' Magic Number "2" specifies run in normal Window.
                ' "True" indicates what for process to finish
                g_WshShell.Run "shutdown.exe -f -r -t 5",2,True
            End If
            g_LogFile.Close
        End If
    End If ' Main line operation
End Function

'
' <summary>
'   Recursive search - starts at current directory and goes looking for a specific file
'    If found, the file is replaced with private version . Will replace all instances of the private file
' </summary>
'
' <arguments>
'  sPrivateLoc - path to folder private file stored in
'  sPrivateName - name of private file
'  sSearchPath - folder we are currently searching for possible file replacement
'  bOSReplace - boolean to indicate if replacing Windows Files
'  bCacheReplaced - boolean to indicate if already replaced version in DLLCache
'</arguments>
'
'
' <returns>
'    True is found file and replaced it at least once on target
' </returns>
'
Function ReplacePrivate(sPrivateLoc, sPrivateName, sSearchPath, bOSReplace, bCacheReplaced)

    ' Should return True if any replacment works
    ReplacePrivate = False

    LogInfo "", g_bVerbose
    LogInfo "[ReplacePrivate] - Private Name: " & sPrivateName, g_bVerbose

    ' If doing an O/S replacement and cache not replaced yet, deal with DLLCache directory first
    If (bOSReplace = True) and (bCacheReplaced = False) Then
        ReplaceFileInDir sPrivateLoc, sPrivateName, g_refFSO.BuildPath(sSearchPath, "dllcache")
    End If

    ' Replace all instances in top-level folder
    ReplacePrivate = (ReplacePrivate) OR (ReplaceFileInDir(sPrivateLoc, sPrivateName, sSearchPath, bOSReplace))

    ' Once we are done with any project in this directory, do the same for all it's sub-dirs
    Dim objSubDirs, itemDir
    Set objSubDirs = g_refFSO.GetFolder(sSearchPath)
    For Each itemDir in objSubDirs.SubFolders
         ' Don't do dllcach again if doing o/s replace
        If not ((bOSReplace = True) and (itemDir.Name = "dllcache")) Then
            ReplacePrivate = ReplacePrivate OR _
                             (ReplacePrivate(sPrivateLoc, _
                              sPrivateName, _
                              g_refFSO.BuildPath(sSearchPath, itemDir.Name),_
                              bOSReplace,_
                              True))
        End If
    Next
End Function

'
' <summary>
'    Search for a given file in a directory - if found, replace with private version
' </summary>
'
' <arguments>
'  sPrivateLoc - path to folder private file stored in
'  sPrivateName - name of private file
'  sSearchPath - folder we are currently searching for possible file replacement
'</arguments>
'
' <returns>
'    Boolean - True if file replaced
' </returns>
'
Function ReplaceFileInDir(sPrivateLoc, sPrivateName, sSearchPath, bOSReplace)
    Dim objFiles, itemFile, sLocFile

    ReplaceFileInDir = False

    LogInfo "   [ReplaceFileInDir] - Searching Folder: " & sSearchPath, g_bVerbose

    ' Go through all the files the directory
    Set objFiles = g_refFSO.GetFolder(sSearchPath)
    For Each itemFile in objFiles.Files
        If (UCASE(itemFile.Name) = UCASE(sPrivateName)) Then
            ReplaceFileInDir = True
            sLocFile = g_refFSO.BuildPath(sSearchPath, sPrivateName)

            ' Delete any .old verison of file and Rename existing file to .old
            ' This gets around problems trying to replace in-use file
            Dim sRenCmd
            If (g_refFSO.FileExists(sLocFile & ".old") = True) Then
                On Error Resume Next
                g_refFSO.DeleteFile sLocFile & ".old",True
                On Error GoTo 0
            End If

            If (g_refFSO.FileExists(sLocFile)) Then
                'call Sub to stop services and kill processes using this file
                If (Not bOSReplace) Then
                    FindAndStopProcessesUsingDll(sLocFile)
                End If
                ' Using ren allows rename of in-use file (unlike FSO Rename) - see spec for details
                sRenCmd = "cmd /c ren " & Chr(34) & sLocFile & Chr(34) & " " & sPrivateName & ".old"
                g_WshShell.Run sRenCmd,2,True
            End If

            ' Now copy down the private
            g_refFSO.CopyFile g_refFSO.BuildPath(sPrivateLoc, sPrivateName), sLocFile, True
            LogInfo "  - Replaced at " & sLocFile, True

            ' Check to see if private symbol file exists
            CopyPrivateSymbols sPrivateName, sPrivateLoc, "sym",sSearchPath
            CopyPrivateSymbols sPrivateName, sPrivateLoc, "pdb",sSearchPath
            CopyPrivateSymbols sPrivateName, sPrivateLoc, "dbg",sSearchPath

        End If
    Next
End Function

' <summary>
'   copies down the symbol files for the private binary
' </summary>
'
' <arguments>
'  sFileName - Name of the private file
'  sFilePath - Path where to find symbol file
'  sSymbolExt - extension of the symbol file
'  sSearchPath - local directory where symbol file should be copied
'</arguments>
'
' <returns>
'   nothing.
' </returns>
Function CopyPrivateSymbols(sFileName, sFilePath, sSymbolExt, sSearchPath)
    Dim sPrivateSymbols, sLocSymbols

    sPrivateSymbols = g_refFSO.BuildPath(sFilePath, g_refFSO.GetBaseName(sFileName) & "." & sSymbolExt)
    sLocSymbols = g_refFSO.BuildPath(sSearchPath, g_refFSO.GetBaseName(sFileName) & "." & sSymbolExt)

    LogInfo "  - Searching for private symbols " & sPrivateSymbols, g_bVerbose

    If (g_refFSO.FileExists(sPrivateSymbols) = True) Then

        On Error Resume Next
        ' Delete any current version of symbols
        If (g_refFSO.FileExists(sLocSymbols) = True) Then
            g_refFSO.DeleteFile sLocSymbols,True
        End If

        ' In case deleting did not work, try renaming
        If (g_refFSO.FileExists(sLocSymbols)) Then
            ' Using ren allows rename of in-use file (unlike FSO Rename) - see spec for details
            sRenCmd = "cmd /c ren " & Chr(34) & sLocSymbols & Chr(34) & " " & sFileName & sSymbolExt & ".old"
            g_WshShell.Run sRenCmd,2,True
        End If

        ' Now copy down the private
        g_refFSO.CopyFile sPrivateSymbols, sLocSymbols, True
        LogInfo "  - Replaced Symbols with private version at " & sLocSymbols, True

        On Error GoTo 0

    End If

End Function

' <summary>
'    Uses WMI to find all of the processes that have dllName loaded into their address space
'    then, it determines if there are any services running in those processes and stops the services
'    if there are no services in the process, the process is simply terminated.
' </summary>
'
' <arguments>
'  dllName - name of the module to look for in the running processes
'</arguments>
'
' <returns>
'   nothing.
' </returns>
Sub FindAndStopProcessesUsingDll(ByVal dllName)
    Dim svc : Set svc = getObject("winmgmts:root\cimv2")
    svc.Security_.Privileges.AddAsString "SeDebugPrivilege", True

    Dim files
    Dim file
    Dim queryProcesses
    Dim queryServices
    Dim processes
    Dim process
    Dim services
    Dim service
    Dim sQuery
    Dim foundService : foundService = False

    'WMI path names need backslashitus
    dllName = replace(dllName, "\", "\\")

    sQuery = "select __relpath from cim_datafile where name = '" & dllName & "'"

    LogInfo sQuery, g_bVerbose

    Set files = svc.execQuery(sQuery, "WQL", 0)
    For Each file In files
        queryProcesses =  "ASSOCIATORS OF {" & file.path_.relpath & _
                          "} WHERE role = antecedent resultClass = win32_process assocclass = cim_processexecutable"
        LogInfo queryProcesses, g_bVerbose
        Set processes = svc.execQuery(queryProcesses)
        For Each process In processes
            queryServices = "SELECT * FROM Win32_Service WHERE ProcessId =" & process.ProcessId
            LogInfo process.caption & " " & process.path_, g_bVerbose
            Set services = svc.execQuery(queryServices)
            For Each service In services
                foundService = True
                LogInfo vbTab & "stopping " & service.name, g_bVerbose
                If StopService(service.name) Then
                    LogInfo "stopped " & service.name, g_bVerbose
                Else
                    LogError "could not stop " & service.name, True
                End If
            Next
            If (ShutdownCOMApplications(process.ProcessId)) Then
                foundServie = True
            End If
            If Not foundService Then
            'there are no services or COM+ apps to stop in the process, so we attempt to kill it
                process.Terminate(0)
            End If
        Next
    Next
End Sub

Function ShutdownCOMApplications (ByVal processId)
    'On Error Resume Next
    Dim objCatalog
    Dim instanceID
    
    Err.Clear
    Set objCatalog = CreateObject("COMAdmin.COMAdminCatalog")
    If Err.number <> 0 Then
        LogError "Could not create COMAdmin.COMAdminCatalog object -> (0x" & Hex(Err.number) & ") desc [" & Err.Description & "]", True
        ShutdownCOMApplications = False
        Exit Function
    End If
    Err.Clear
    instanceID = objCatalog.GetApplicationInstanceIDFromProcessID(processId)
    If Err.number <> 0 Then
        LogInfo "process id [" & processId & "] didn't have any COM+ applications", g_bVerbose
        ShutdownCOMApplications = False
        Exit Function
    End If
    LogInfo "found [" & instanceID  & "] COM+ application in process id [" & processId & "]", g_bVerbose
    Err.Clear
    objCatalog.ShutdownApplicationInstances(instanceID)
    If Err.number <> 0 Then
        LogError "Could not shutdown COM+ app -> (0x" & Hex(Err.number) & ") desc [" & Err.Description & "]", True
        ShutdownCOMApplications = False
        Exit Function
    End If
    ShutdownCOMApplications = True
End Function

' <summary>
'    Stops the service specified by strServiceName
' </summary>
'
' <arguments>
'  strServiceName - name of the service to stop
'</arguments>
'
' <returns>
'    True on Success (service is stopped)
'    False on all failures (service is not stopped)
' </returns>
Function StopService (strServiceName)
    'On Error Resume Next

    Dim objWMIService : Set objWMIService = GetObject("winmgmts:root\cimv2")
    Dim colItems : Set colItems = objWMIService.ExecQuery("SELECT * FROM Win32_Service WHERE name='" _
                                  & strServiceName & "'", "WQL", wbemFlagReturnImmediately + wbemFlagForwardOnly)
    Dim objItem
    Dim i

    Dim colItemsRefresh
    Dim objItemRefresh
    Dim fDone : fDone = False
    Dim fSuccess : fSuccess = True
    Dim stopResult

    For Each objItem In colItems
        If objItem.Started Then
            If objItem.AcceptStop Then
                LogInfo strServiceName & " is in the " & objItem.State & " state.", g_bVerbose
                stopResult = objItem.StopService
                If 0 <> stopResult Then
                    fSuccess = False
                    LogError "stopping " & strServiceName & " returned error: " & stopResult, True
                End If
                For i = 1 to numLoops
                    If fDone Then
                        Exit For
                    End If
                    WScript.Sleep(sleepTimeMS)
                    Set colItemsRefresh = objWMIService.ExecQuery("SELECT * FROM Win32_Service WHERE name='" _
                                        & strServiceName & "'", "WQL", _
                                        wbemFlagReturnImmediately + wbemFlagForwardOnly)
                    For Each objItemRefresh in colItemsRefresh
                        If UCase(objItemRefresh.State) = "STOPPED" Then
                            fDone = True
                        End If
                    Next
                    Set colItemsRefresh = Nothing
                    If numLoops = i Then
                        LogError "Timeout expired waiting for " & strServiceName & " to stop.", True
                        fSuccess = False
                    End If
                Next
            Else
                'service doesn't support stop control
                fSuccess = False
                LogError strServiceName & " does not support the stop control.", True
            End If 'AcceptStop
        End If 'Started
    Next  'objItem
    Set objWMIService = Nothing
    Set colItems = Nothing
    StopService = fSuccess
End Function

' <summary>
'    Starts the service specified by strServiceName
' </summary>
'
' <arguments>
'  strServiceName - name of the service to start
'</arguments>
'
' <returns>
'    True on Success (service is started)
'    False on all failures (service is not started)
' </returns>
Function StartService (strServiceName)
    'On Error Resume Next

    Dim objWMIService : Set objWMIService = GetObject("winmgmts:root\cimv2")
    Dim colItems : Set colItems = objWMIService.ExecQuery("SELECT * FROM Win32_Service WHERE name='" _
                                  & strServiceName & "'", "WQL", wbemFlagReturnImmediately + wbemFlagForwardOnly)
    Dim objItem
    Dim i

    Dim colItemsRefresh
    Dim objItemRefresh
    Dim fDone : fDone = False
    Dim fSuccess : fSuccess = True
    Dim startResult

    For Each objItem In colItems
        If Not objItem.Started Then
            LogInfo  strServiceName & " is in the " & objItem.State & " state.", g_bVerbose
            startResult = objItem.StartService
            If 0 <> startResult Then
                LogError "StartService for " & strServiceName & " returned error: " & startResult
                fSuccess = False
            End If
            For i = 1 to 10
                If fDone Then
                    Exit For
                End If
                WScript.Sleep(sleepTimeMS)
                Set colItemsRefresh = objWMIService.ExecQuery("SELECT * FROM Win32_Service WHERE name='" _
                                      & strServiceName & "'", "WQL", _
                                      wbemFlagReturnImmediately + wbemFlagForwardOnly)
                For Each objItemRefresh in colItemsRefresh
                    If UCase(objItemRefresh.State) = "RUNNING" Then
                        fDone = True
                    End If
                Next
                Set colItemsRefresh = Nothing
                If 10 = i Then
                    LogError "Timeout expired waiting for " & strServiceName & " to start.", True
                    fSuccess = False
                End If
            Next
        End If 'Not Started
    Next  'objItem
    Set objWMIService = Nothing
    Set colItems = Nothing
    StartService = fSuccess
End Function


' <summary>
'    Opens the log and error files
' </summary>
'
' <arguments>
'  pLogFileName - name of file to open for logging purposes
'  pErrorFileName - name of file to open for error logging purposes
'</arguments>
'
' <requires>
'    Global object g_LogFileName and g_ErrorFileName - handles set to open files
'    Global object g_refFSO - FileSystemObject
' </requires>

' <returns>
'    Error code - 0 if both logs opened, otherwise error code is returned
' </returns>

Function OpenLogFiles(pLogFileName, pErrorFileName)
    OpenLogFiles = 0
    On Error Resume Next
    ' Try to open up the LOG file
    Set g_LogFile = g_refFSO.CreateTextFile(pLogFileName, True)
    If (err) Then
        OpenLogFiles = err
        WScript.Echo "Error - could not Open log file: " & pLogFileName
        Exit Function
    End If
    ' Try to open up the ERROR file
    Set g_ErrFile = g_refFSO.CreateTextFile(pErrorFileName, True)
    If (err) Then
        OpenLogFiles = err
        WScript.Echo "Error - could not Open Error Log file: " & pErrorFileName
        Exit Function
    End If

    On Error GoTo 0
    Wscript.Echo
    LogInfo PROG_DESC, True        ' First thing in log file is PROG_DESC
End Function

'
' <summary>
'    Logs a message to the log file and optionally also to the screen
' </summary>
'
' <arguments>
'  mMsg - string to log out to file and optionally screen
'  bVerbose - if True, also puts out message to screen
'</arguments>
'
' <requires>
'    Global object g_LogFile points to a valid file we can write to
' </requires>
'
Sub LogInfo(sMsg, bVerbose)
    If (bVerbose) Then
        WScript.Echo sMsg
    End If
    g_LogFile.WriteLine(sMsg)
End Sub

'
' <summary>
'    Logs a message to the error file. Also put in log file and to screen
' </summary>
'
' <arguments>
'  mMsg - string to log out to error and log file
'  bHeader - should pre-append standard ERROR tag to front of message
'</arguments>
'
' <requires>
'    Global object g_ErrFile points to a valid file we can write to
' </requires>
'
Sub LogError(sMsg, bHeader)
    If (bHeader) Then
        sMsg =  "Error - " & sMsg
    End If
    LogInfo sMsg, True
    g_ErrFile.WriteLine(sMsg)
    ' Mark that we wrote at least one line to error file
    g_ErrFileUsed = True
End Sub


'
' <summary>
'   Display Usage
' </summary>
'
Sub Usage()
    WScript.Echo
    WScript.Echo "cscript PrivateFiles.vbs /privshar:<Path> /privfolder:<Folder Name> /target:<Path>"
    Wscript.Echo "       /logfile:<Path> /errorfile:<Path> /postpriv:<Path>"
    WScript.Echo "       /os /reboot /verbose"
    WScript.Echo ""
    WScript.Echo "/privshar:<Path> = Path to share with folder containing private files"
    WScript.Echo "  Default: \\smxfiles\privates"
    WScript.Echo ""
    WScript.Echo "/privfolder:<Folder Name> = Name of folder containing private files"
    WScript.Echo "  Required"
    WScript.Echo ""
    WScript.Echo "/target:<Path> = Path to folder to start replacmement. Any file that matches any"
    WScript.Echo "    private file in this directory and all it's sub-directories will be replaced."
    WScript.Echo "  Required if OS flag is False"
    WScript.Echo ""
    WScript.Echo "/postpriv:<Path> = Path to file to execute after replacement operation done but before any reboot"
    WScript.Echo "  Optional"
    WScript.Echo ""
    WScript.Echo "/logfile:<Path> = Path and name of file to log operation of this script"
    WScript.Echo "  Note: Folder structure must exist - will not be created by script."
    WScript.Echo "  Default: (Current Directory)/privatefiles.log"
    WScript.Echo ""
    WScript.Echo "/errorfile:<Path> = Path and name of file to log errors"
    WScript.Echo "  Note: Folder structure must exist - will not be created by script."
    WScript.Echo "  Default: (Current Directory)/privatefiles.err"
    WScript.Echo ""
    WScript.Echo "/os = Flag to indicate replacing O/S files"
    WScript.Echo "    If this flag specified, then script always reboots and no target can be specified"
    WScript.Echo "    NOTE: During O/S replacement, it is possible Windows will pop-up a dialog box"
    WScript.Echo "      Warning about replacing system files. Ignore it. The reboot at the end clears the problem"
    WScript.Echo ""
    WScript.Echo "/noreboot = Flag to indicate computer should not reboot after file replacement"
    WScript.Echo "    Can not use with /os flag above"
    WScript.Echo ""
    WScript.Echo "/verbose = Flag to turn on verbose logging to screen"
    WScript.Echo ""
    Wscript.Echo "Example Usage:"
    Wscript.Echo
    WScript.Echo "CScript privatefiles.vbs /privshare:\\smxfiles\scratch /privfolder:glennlav /os"
    Wscript.Echo
    Wscript.Echo "  Replace all O/S files that corresponding files found at \\smxfiles\scratch\glennlav"
    WScript.Echo "  After replacement, computer will be rebooted."
    WScript.Echo ""
    WScript.Echo "CScript privatefiles.vbs /privfolder:glennlav /target:c:/hold"
    WScript.Echo "  /noreboot /postpriv:\\smxfiles\glennlav\post.cmd"
    Wscript.Echo
    Wscript.Echo "  Replace all files in the folder c:\hold and its sub-folder with the corresponding files"
    WScript.Echo "  found at \\smxfiles\privates\glennlav. After all files replaced, execute"
    WScript.Echo "  \\smxfiles\glennlav\post.cmd.  After replacement, computer will not be rebooted."
    WScript.Echo ""
    WScript.Echo ""
End Sub
