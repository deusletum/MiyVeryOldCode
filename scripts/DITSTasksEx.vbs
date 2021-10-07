' ---------------------------------------------------------------------------------------------------------
'
'   company: oration
'   copyright: Copyright (c) roration 2002-2003
'
'   file: DITSTasks.vbs 
'   summary:
'        A set of VB Script subroutines to make creating DITS Job XML easier.
'    remarks:
'        The functions contained here are maintained as a part of the DITS system and will
'        created valid DITS Job XML when used as indicated. Functions to create standard
'        tasks will be compatible with the current implementations of the Server Management
'        Test Infrastructure and Operations tools that are invoked.
'
'    history:
'        02-May-02    JERRYHEN    created
'        08-Jul-02    JERRYHEN    modified to meet coding standards                
'        11-Feb-03    JERRYHEN    error checking, retry, timeout, etc.
'        11-Mar-03    JERRYHEN    Glenn's changes. Formatting at jheat request.
'        02-Apr-03    JERRYHEN    DITS 2.5
'        10-Jul-03    JERRYHEN    Add old functions back in. Set sDITSServer default.
'        11-Jul-03    JERRYHEN    Allow zero length paramater values in MakeDITSStandardTask.
'        11-Aug-03    JERRYHEN    Define sProdLang parameter to MakeInstallOffice
'        08-Sep-03    JERRYHEN    Default DITS Server becomes BigHeart
'                                                                                                  
' ---------------------------------------------------------------------------------------------------------
Option Explicit

Public Const DOMDOCUMENT = "Msxml2.DOMDocument.3.0"
Public Const USEWEBSERVICE  = True

' This is global so the DITS Server name can be accessed for the web service
Dim sDITSServer ' DITS Server
sDITSServer = "BIGHEART"

Public Function MakeJobXML
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create an XML object for DITS Jobs setting all the nice things we want.
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       None
'                                                                                                       
'    Returns:
'        the DITS Job XML object.                                                                       
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML        ' the XML DOM Document

On Error Resume Next
Set objJobXML = CreateObject(DOMDOCUMENT)
If Err = 429 Then
    WScript.Echo "You do not have the proper level of MSXML installed."
    ' Set objJobXML = CreateObject(DOMDOCUMENT)
ElseIf Err <> 0 Then
    WScript.Echo "Error creating XML document. Error=" & Err & " " & Err.Description & "."
Else
    objJobXML.setProperty "SelectionLanguage","XPath"
    objJobXML.async = False
End If

Set MakeJobXML = objJobXML

End Function

Public Function MakeDITSJob(ByVal sDITSServer, ByVal sName,ByVal sNotify, _
    ByVal sOwner,ByVal sComments, ByVal iEstimated, _
    ByVal iPriority, ByVal iHold, ByVal sJobgroup, ByVal sRelease)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new JOB node in the XML.                                               
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       sName               The job name.
'       sNotify             A list of users to notify when significant job events occur.                
'       sOwner              The owner of the job.                                                       
'       sComments           Comments about the job.                                                     
'       iEstimated          Estimated time, in hours, for the job to complete.                          
'       iPriority           The job priority.
'       iHold               The hold count.
'       sJobgroup           The job group.
'       sRelease            The release parameter.
'                                                                                                       
'    Returns:
'        the JOB node object.                                                                       
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML        ' a temp XML DOM Document
Dim objJob           ' the created JOB node
Dim sNotifyUsers     ' the formatted string of users to notify

Dim sRequest
Dim sData
Dim sJobXml
    
sRequest    = "CreateDITSJob"
sData = "Name=" & sName & "&Comments=" & URLEncode(sComments) & "&Priority=" & iPriority _
            & "&Hold=" & iHold & "&Jobgroup=" & URLEncode(sJobgroup) _
            & "&Owner=" & URLEncode(sOwner) _
            & "&Notify=" & URLEncode(sNotify) _
            & "&Estimated=" & iEstimated & "&Release=" & URLEncode(sRelease) _
            & "&Domain=&Account=&Password=&OnLogon="
            
sJobXML = DITSJobCreationWebService(sDITSServer,sRequest,sData)
Set objJobXML = CreateObject(DOMDOCUMENT)
objJobXML.loadXML sJobXML
Set objJob = objJobXML.documentElement

Set MakeDITSJob = objJob

End Function

Public Function MakeDITSTrigger(ByVal sDITSServer, objJob,ByVal sTriggerType, _
    ByVal sTriggerStart,ByVal sTriggerStop, _
    ByVal sTriggerWeek,ByVal sTriggerDays)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a TRIGGER node in the job XML                                            
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objJob              The JOB node object returned by MakeNewJob                                     
'       sTriggerType        The type (Now,Once,Daily,Weekly,Monthly,Month,Monthlydayofweek              
'       sTriggerStart       The date and time the job is first eligible for execution.                  
'       sTriggerStop        The date and time after which the job is no longer eligible to run.         
'       sTriggerWeek        Specifies the week of the month for type = "monthlydayofweek".              
'       sTriggerDays        Specifies the days of the week for type = "weekly" or "monthlydayofweek".   
'                                                                                                       
'    Returns:
'        the TRIGGER node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTrigger

Set objJobXML = CreateObject(DOMDOCUMENT)

Dim sRequest
Dim sData
Dim sMachineXml
    
sRequest    = "CreateDITSTrigger"
sData = "Type=" & sTriggerType & "&Start=" & sTriggerStart _
            & "&Stop=" & sTriggerStop & "&Week=" & sTriggerWeek & "&Days=" & sTriggerDays
            
sMachineXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sMachineXml
Set objTrigger = objJobXML.documentElement    

Set MakeDITSTrigger = objTrigger

objJob.appendChild objTrigger

End Function

Public Function MakeDITSMachine(ByVal sDITSServer, objJob,ByVal sMachineid, _
    ByVal sMachine, ByVal sTransfer, ByVal sQuery, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a MACHINE node in the job XML                                            
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objJob              The JOB node object returned by MakeNewJob                                     
'       sMachineid          The machineid attribute for the machine. One is generated in this is empty. 
'       sMachine            The machine name or pool specification.                                     
'       sTransfer           Transfer machine on release.
'       sQuery              Machine Librarian query.
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the MACHINE node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objMachine
Dim objMachines
Dim iMachineNumber
Dim sName
Dim sPool

Set objJobXML = CreateObject(DOMDOCUMENT)

If IsNull(sMachineid) Or IsEmpty(sMachineid) Or Len(sMachineid) = 0 Then
    Set objMachines = objJob.selectNodes("MACHINE")
    iMachineNumber = objMachines.Length + 1
    sMachineid = "Machine" & String(3 - Len(iMachineNumber),"0") & iMachineNumber
End If

If UCase(Left(sMachine,5) ) = "POOL=" Then
    ' This can be of the format Pool=PoolName or Pool=PoolName,MachineName
    If Instr(sMachine,",") > 0 Then
        sPool = Mid(sMachine,6,Instr(sMachine,",") - 6)
        sName = Mid(sMachine,Instr(sMachine,",") + 1)
    Else
        sPool = Mid(sMachine,6)
    End If
Else
    sName = sMachine
End If

Dim sRequest
Dim sData
Dim sMachineXml
    
sRequest = "CreateDITSMachine"
sData = "Machineid=" & URLEncode(sMachineid) _
        & "&Name=" & URLEncode(sName) _
        & "&Pool=" & URLEncode(sPool) _
        & "&Transfer=" & URLEncode(sTransfer) _
        & "&Query=" & URLEncode(sQuery)  _
        & "&Domain=" & sDomain & "&Account=" & sAccount _
        & "&Password=" & URLEncode(sPassword) & "&OnLogon="
            
sMachineXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sMachineXml
Set objMachine = objJobXML.documentElement    

objJob.appendChild objMachine

Set MakeDITSMachine = objMachine

End Function

Public Function MakeDITSTasklist(ByVal sDITSServer, objMachine,ByVal sName, _
        ByVal sDomain,ByVal sAccount,ByVal sPassword, _
        ByVal sOnLogon, ByVal sCd, ByVal bInteractive)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new TASKLIST node in the job XML                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objMachine          The MACHINE node object returned by MakeDITSMachine                             
'       sName               A name for the task list.                                                   
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'       sCd                 Current directory for tasklist execution 
'       bInteractive        True to run in interactive user logon session                                                                                       
'                                                                                                       
'    Returns:
'        the TASKLIST node object.                                                                  
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTasklist
Dim sRequest
Dim sData
Dim sTasklistXml

Set objJobXML = CreateObject(DOMDOCUMENT)

If bInteractive Then
    bInteractive = True
Else
    bInteractive = False
End If
    
sRequest    = "CreateDITSTasklist"
sData = "Name=" & URLEncode(sName) & "&Cd=" & URLEncode(sCd) _
        & "&Domain=" & sDomain & "&Account=" & sAccount _
        & "&Password=" & URLEncode(sPassword) & "&OnLogon=" & URLEncode(sOnLogon) _
        & "&Interactive=" & bInteractive
            
sTasklistXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sTasklistXml
Set objTasklist = objJobXML.documentElement    
objMachine.AppendChild objTasklist
  
Set MakeDITSTasklist = objTasklist 

End Function

Public Function MakeDITSTask(ByVal sDITSServer, objTasklist,ByVal sName, _
    ByVal bEven,ByVal bOnly, ByVal bWait, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword, _
    ByVal sOnLogon, ByVal sCd, ByVal bInteractive)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new TASK node in the job XML                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objTasklist         The TASKLIST node object returned by MakeDITSTasklist                           
'       sName               A name for the task.                                                        
'       bEven               Set to True if the task should run even If the job is cancelled.            
'       bOnly               Set to True if the task should run only If the job is cancelled.            
'       bWait               Set to True if the task should wait for a starttask message.
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'       sCd                 Current directory for tasklist execution 
'       bInteractive        True to run in interactive user logon session                                                                                       
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTask

Set objJobXML = CreateObject(DOMDOCUMENT)

Dim sRequest
Dim sData
Dim sTaskXml
    
If bEven Then
    bEven = True
Else
    bEven = False
End If
If bOnly Then
    bOnly = True
Else
    bOnly = False
End If
If bWait Then
    bWait = True
Else
    bWait = False
End If
If bInteractive Then
    bInteractive = True
Else
    bInteractive = False
End If
    
sRequest    = "CreateDITSTask"
sData = "Name=" & URLEncode(sName) & "&Even=" & bEven & "&Only=" & bOnly _
        & "&Wait=" & bWait & "&Cd=" & URLEncode(sCd) & "&Interactive=" & bInteractive _
        & "&Domain=" & sDomain & "&Account=" & sAccount _
        & "&Password=" & URLEncode(sPassword) & "&OnLogon=" & sOnLogon
        
sTaskXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sTaskXml
Set objTask = objJobXML.documentElement    
objTasklist.AppendChild objTask  

Set MakeDITSTask = objTask 

End Function

Public Function MakeDITSCommand(ByVal sDITSServer,objTask,ByVal sText, _
    ByVal sName, ByVal bEnabled, ByVal bOutput, ByVal iTimeout, _
    ByVal iWaitforreboot, ByVal bRebootbyDITS,ByVal iRetry,ByVal iRetrydelay, _
    ByVal sSuccesscode, ByVal bRebootonfailure,ByVal iOnfailure, _
    ByVal sOnfailnotify, ByVal bInteractive, ByVal sCd, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword, ByVal sOnLogon)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new COMMAND node in the job XML                                        
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objTask             The TASK node object returned by MakeDITSTask.                                  
'       sText               A text of the command.                                                      
'       bEnabled            True if the command should be executed.
'       bOutput             True if the STDOUT/STDERR should be captured.
'       iTimeout            The time (in seconds) for the command to complete.                          
'                           -1 (INFINITE) or 0 means no timout processing.                              
'                           A positive value means the job will be paused If the command does not       
'                           complete in the time specified. A negative value means the job will be      
'                           cancelled If the command does not complete in the time specified.           
'       iWaitforreboot      The time (in seconds) for a reboot to occur.                              
'                           0 means no reboot is expected. The job will be paused If one occurs during  
'                           execution of this command. A positive value means the job will be paused If 
'                           If a reboot does not occur within the time specified. A negative value means
'                           that DITS will wait the specifiec time for a reboot to occur but the job    
'                           not be paused If one does not occur.                                        
'       bRebootbyDITS       True if DITS should reboot the system after the command completes.
'       iRetry              The number of times the command should be retried after a failure.
'       iRetrydelay         The number of seconds to wait before the command is retried.
'       sSuccesscode        A comma separated list of command exit codes that are considered successful.
'       bRebootonfailure    True if DITS should reboot the system after a command failure.
'       iOnfailure          Action to take on a failure:
'                           0 means continue.
'                           Positive means Pause the job.
'                           Negative means Cancel the job.
'       sOnfailnotify       A list of users to notify on command failure.
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'       sCd                 Current directory for tasklist execution 
'       bInteractive        True to run in interactive user logon session                                                                                       
'                                                                                                       
'    Returns:
'        the COMMAND node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objCommand
Dim sRequest
Dim sData
Dim sCommandXml

Set objJobXML = CreateObject(DOMDOCUMENT)

If bEnabled Then
    bEnabled = True
Else
    bEnabled = False
End If
If bOutput Then
    bOutput = True
Else
    bOutput = False
End If
If bRebootByDITS Then
    bRebootByDITS = True
Else
    bRebootByDITS = False
End If
If bRebootOnFailure Then
    bRebootOnFailure = True
Else
    bRebootOnFailure = False
End If
If bInteractive Then
    bInteractive = True
Else
    bInteractive = False
End If
iRetry = CInt(iRetry)
iRetryDelay = CInt(iRetryDelay)
iOnFailure = CInt(iOnFailure)

sRequest    = "CreateDITSCommand"
sData = "Name=" & URLEncode(sName) _
            & "&Enabled=" & bEnabled & "&Output=" & bOutput _
            & "&Timeout=" & iTimeout & "&WaitForReboot=" & iWaitForReboot _
            & "&RebootByDITS=" & bRebootByDITS _
            & "&Retry=" & iRetry & "&RetryDelay=" & iRetryDelay _
            & "&SuccessCode=" & URLEncode(sSuccessCode) _
            & "&RebootOnFailure=" & bRebootOnFailure _
            & "&OnFailure=" & iOnFailure & "&OnFailNotify=" & URLEncode(sOnFailNotify) _
            & "&Cd=" & URLEncode(sCd) & "&Interactive=" & bInteractive _
            & "&Domain=" & sDomain & "&Account=" & sAccount _
            & "&Password=" & URLEncode(sPassword) _
            & "&Onlogon=" & URLEncode(sOnLogon) _
            & "&Command=" & URLEncode(sText)
                       
sCommandXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sCommandXml
Set objCommand = objJobXML.documentElement    
objTask.AppendChild objCommand  

Set MakeDITSCommand = objCommand

End Function

Public Function MakeDITSEnvironment(ByVal sDITSServer, objNode,ByVal sName,ByVal sValue)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create an ENVIRONMENT node in the job XML                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.
'       objNode             The parent node object for this node.                                       
'       sName               The name of the environment variable to set.                                
'       sValue              The value for the environment variable.                                     
'                                                                                                       
'    Returns:
'        the ENVIRONMENT node object.                                                               
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objEnvironment

Set objJobXML = CreateObject(DOMDOCUMENT)

Dim sRequest
Dim sData
Dim sEnvironmentXml
    
sRequest    = "CreateDITSEnvironment"
sData = "Name=" & URLEncode(sName) & "&Value=" & URLEncode(sValue)             
sEnvironmentXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
objJobXML.loadXML sEnvironmentXml
Set objEnvironment = objJobXML.documentElement    

objNode.AppendChild objEnvironment 
    
Set MakeDITSEnvironment = objEnvironment 

End Function

Public Function MakeTrigger(objJob,ByVal sTriggerType,ByVal sTriggerStart,ByVal sTriggerStop, _
    ByVal sTriggerWeek,ByVal sTriggerDays)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a TRIGGER node in the job XML                                            
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJob              The JOB node object returned by MakeDITSJob                                     
'       sTriggerType        The type (Now,Once,Daily,Weekly,Monthly,Month,Monthlydayofweek              
'       sTriggerStart       The date and time the job is first eligible for execution.                  
'       sTriggerStop        The date and time after which the job is no longer eligible to run.         
'       sTriggerWeek        Specifies the week of the month for type = "monthlydayofweek".              
'       sTriggerDays        Specifies the days of the week for type = "weekly" or "monthlydayofweek".   
'                                                                                                       
'    Returns:
'        the TRIGGER node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTrigger

Set objJobXML = CreateObject(DOMDOCUMENT)

If USEWEBSERVICE Then
    Set objTrigger = MakeDITSTrigger(sDITSServer,objJob,sTriggerType,sTriggerStart, _
                        sTriggerStop,sTriggerWeek,sTriggerDays)
Else
    Set objTrigger = objJobXML.createElement("TRIGGER")
    objTrigger.setAttribute "type",sTriggerType
    objTrigger.setAttribute "start",sTriggerStart
    objTrigger.setAttribute "stop",sTriggerStop
    objTrigger.setAttribute "week",sTriggerWeek
    objTrigger.setAttribute "days",sTriggerDays
    objJob.appendChild objTrigger
End If

Set MakeTrigger = objTrigger

End Function

Public Sub SetSecurity(objNode,ByVal sDomain,ByVal sAccount,ByVal sPassword,ByVal sOnlogon)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This subroutine will add the security parameters to an XML node                                         
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objNode             The node object 
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'       sOnlogon            Command to execute when account logged on 
'       bInteractive        True to run in interactive user logon session 
'                                                                                                       
'    Returns:
'        nothing
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
If Len(sAccount) > 0 Then
    objNode.setAttribute "domain",sDomain
    objNode.setAttribute "account",sAccount
    objNode.setAttribute "password",sPassword
    objNode.setAttribute "onlogon",sOnlogon
End If
' Always set interactive
' objNode.setAttribute "interactive",bInteractive

End Sub

Public Sub SetCommandOptions(objCommand,ByVal iTimeout,ByVal iWaitforreboot, _
    ByVal bRebootbyDITS,ByVal iRetry,ByVal iRetrydelay,ByVal sSuccesscode, _
    ByVal bRebootonfailure,ByVal iOnfailure,ByVal sOnfailnotify)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This subroutine will add the retry parameters to a COMMAND node                                         
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objCommand          The COMMAND node object 
'       iTimeout            The time (in seconds) for the command to complete.                          
'                           -1 (INFINITE) or 0 means no timout processing.                              
'                           A positive value means the job will be paused If the command does not       
'                           complete in the time specified. A negative value means the job will be      
'                           cancelled If the command does not complete in the time specified.           
'       iWaitforreboot      The time (in seconds) for a reboot to occur.                              
'                           0 means no reboot is expected. The job will be paused If one occurs during  
'                           execution of this command. A positive value means the job will be paused If 
'                           If a reboot does not occur within the time specified. A negative value means
'                           that DITS will wait the specifiec time for a reboot to occur but the job    
'                           not be paused If one does not occur.                                        
'       bRebootbyDITS       True If DITS should reboot after the command completes
'       iRetry                Number of times to retry a failing command
'       iRetrydelay            Time in seconds before doing the retry
'       sSuccesscode        A comma-separated list of successful exit code
'       bRebootonfailure    True If DITS should reboot the machine before doing the retry
'       iOnfailure            Action to take when all retries have failed. 0 = continue,
'                           < 0 = cancel the job, > 0 - pause the job.
'       sOnfailnotify       List of email address to notify when all retries fail.
'                                                                                                       
'    Returns:
'        nothing
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
If IsNumeric(iTimeout) And CInt(iTimeout) <> -1 And CInt(iTimeout) <> 0 Then
    objCommand.setAttribute "timeout",iTimeout * 1000
End If
If iWaitforreboot <> 0 Then
    objCommand.setAttribute "waitforreboot",iWaitforreboot * 1000
End If
If bRebootbyDITS Then
    objCommand.setAttribute "rebootbydits",bRebootbyDITS
End If

objCommand.setAttribute "retry",iRetry
objCommand.setAttribute "retrydelay",iRetrydelay * 1000
objCommand.setAttribute "successcode",sSuccesscode
If bRebootonfailure Then
    objCommand.setAttribute "rebootonfailure",bRebootonfailure
End If
objCommand.setAttribute "onfailure",iOnfailure
objCommand.setAttribute "onfailnotify",sOnfailnotify

End Sub

Public Function MakeStandardTask(objTasklist,ByVal sName,ByVal aParms, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a standard library task
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist         The TASKLIST node object returned by MakeDITSTasklist                           
'       sName               A name of the standard task.                                                        
'       aParms              An array of parameters for the task
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
'
Dim objTask
Dim sRequest
Dim sData
Dim sTaskXml
Dim objTaskXML        ' a temp XML DOM Document
Dim objElement
Dim i

sRequest = "CreateDITSStandardTask"
Set objTaskXML = CreateObject(DOMDOCUMENT)
Set objElement = objTaskXML.createElement("TASK")
objElement.setAttribute "name",sName
objTaskXML.appendChild objElement
' Get the array of variables
If Not IsNull(aParms) Then
    For i = 0 To UBound(aParms,1)
        If Not IsNull(aParms(i,1)) And Not IsEmpty(aParms(i,1)) Then
            Set objElement = objTaskXML.createElement("VARIABLE")
            objElement.setAttribute "name",aParms(i,0)
            objElement.text = aParms(i,1)
            objTaskXML.documentElement.appendChild objElement
        End If
    Next
End If
                    
sData = "xml=" & URLEncode(objTaskXML.xml)
sTaskXml = DITSJobCreationWebService(sDITSServer,sRequest,sData)
Set objTaskXML = CreateObject(DOMDOCUMENT)
objTaskXML.loadXML sTaskXml
Set objTask = objTaskXML.documentElement
' Check to see if an error occurred
If objTask.nodeName = "ERROR" Then
    ' Build a TASK node with the error
    Set objTask = objTaskXMl.createElement("TASK")
    objTask.setAttribute "error","TASK """ & sName & """ not found in the library."
End If
objTasklist.AppendChild objTask  

Set MakeStandardTask = objTask 

End Function

Public Function MakeDITSInit(objJobXML,objTasklist,objTask)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to initialize DITS processing on the machine.                   
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Set objTask = MakeStandardTask(objTasklist,"DITSInit",Null,"","","")

Set MakeDITSInit = objTask 
    
End Function

Public Function MakeDITSTerm(objJobXML,objTasklist,objTask)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to terminate DITS processing on the machine.                    
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Set objTask = MakeStandardTask(objTasklist,"DITSTerm",Null,"","","")
    
Set MakeDITSTerm = objTask 

End Function

Public Function MakeInstallOS(objJobXML,objTasklist,objTask,ByVal sOS,ByVal sOSVer,ByVal sOSBuild, _
    ByVal sOSType,ByVal sOSLang,ByVal sOSDomain,ByVal sOSFileSys,ByVal sPOPSCmd)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install an operating system on the machine.                  
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sOS             The OS to install. (nt5, xp or other value supported by OASIS)              
'       sOSVer          The OS Version to install. (PRO, SRV, ENT, ADS, etc as supported by OASIS)  
'       sOSBuild        The OS build to install.                                                    
'       sOSType         The OS type to install. FRE or CHK.                                         
'       sOSLang         The OS language to install.                                                 
'       sOSDomain       The OS domain.
'       sOSFileSys      The file system to specify when formatting the OS partition.      
'       sPOPSCmd        The POPS command to run after the OS is installed.                          
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim sOASISCommand
Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeNewTask(objTasklist,"Setup OS",0,0,"","","")
End If
sOASISCommand = "/O " & sOS & _
                " /V " & sOSVer & " /B " & sOSBuild & " /T " & sOSType & _
                " /L " & sOSLang & " /F " & sOSFileSys & " /C " & sPOPSCmd
If Len(sOSDomain) > 0 Then
    sOASISCommand = sOASISCommand & " /M " & sOSDomain                
End If
Set objCommand = MakeNewCommand(objTask,"CScript.exe //nologo %DITS_Path%\DITSOASIS.vbs " & sOASISCommand,3600,120,"","","")
SetCommandOptions objCommand,3600,120,False,0,0,"0",False,1,""
Set objCommand = MakeNewCommand(objTask,"%DITS_Path%\Desktop.exe 255 128 0",180,0,"","","")
SetCommandOptions objCommand,180,0,False,5,15,"0",False,0,""

Set MakeInstallOS = objTask 

End Function

Public Function MakeNewInstallOS(objJobXML,objTasklist,objTask,ByVal sOS,ByVal sOSVer,ByVal sOSBuild, _
    ByVal sOSType,ByVal sOSLang,ByVal sOSDomain,ByVal sOSFileSys,ByVal sPOPSCmd)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install an operating system on the machine.                  
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sOS             The OS to install. (nt5, xp or other value supported by OASIS)              
'       sOSVer          The OS Version to install. (PRO, SRV, ENT, ADS, etc as supported by OASIS)  
'       sOSBuild        The OS build to install.                                                    
'       sOSType         The OS type to install. FRE or CHK.                                         
'       sOSLang         The OS language to install.                                                 
'       sOSDomain       The OS domain.
'       sOSFileSys      The file system to specify when formatting the OS partition.      
'       sPOPSCmd        The POPS command to run after the OS is installed.                          
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aParms()
Redim aParms(7,1)
aParms(0,0) = "%os"
aParms(0,1) = sOS
aParms(1,0) = "%version"
aParms(1,1) = sOSVer
aParms(2,0) = "%build"
aParms(2,1) = sOSBuild
aParms(3,0) = "%type"
aParms(3,1) = sOSType
aParms(4,0) = "%lang"
aParms(4,1) = sOSLang
aParms(5,0) = "%domain"
aParms(5,1) = sOSDomain
aParms(6,0) = "%format"
aParms(6,1) = sOSFileSys
aParms(7,0) = "%pops"
aParms(7,1) = sPOPSCmd

Set objTask = MakeStandardTask(objTasklist,"Setup OS",aParms,"","","")

Set MakeInstallOS = objTask 

End Function

Public Function MakeUpgradeOS(objJobXML,objTasklist,objTask,ByVal sOS,ByVal sOSVer, _
    ByVal sOSBuild,ByVal sOSType)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to upgrade an operating system on the machine.                  
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       sOS             The OS to install. (nt5, xp or other value supported by OASIS)              
'       sOSVer          The OS Version to install. (PRO, SRV, ENT, ADS, etc as supported by OASIS)  
'       sOSBuild        The OS build to install.                                                    
'       sOSType         The OS type to install. FRE or CHK.                                         
'       objTask         The task to add the commands to. If Null a new task is created.             
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Upgrade Test OS to .Net Server",False,False,False,"", _
                    "","","","",False)
End If

Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "\\acstools\drop\dotnetup.cmd " & sOSBuild & " " & sOSType & " " & _
    sOSVer,"",True,True, _
    3600,600,False,0,0,"0",False,1,"",False,"","","","","")

Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "%DITS_Path%\Desktop.exe 255 128 0","",True,True, _
    180,0,False,0,0,"0",False,0,"",False,"","","","","")

Set MakeUpgradeOS = objTask 

End Function

Public Function MakeRestoreOS(objJobXML,objTasklist,objTask)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to boot the machine to the OS booted before installing a new OS 
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
'    Note that If the new OS is installed on the same partition as the old one, all this really         
'    accomplishes is rebooting the machine.                                                             
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Restore OS",True,False,True,"", _
                    "","","","",False)
End If
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "cscript.exe //nologo %DITS_Path%\RestoreOS.vbs","",True,True, _
    600,120,False,0,0,"0",False,1,"",False,"","","","","")
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "%DITS_Path%\Desktop.exe 255 128 0","",True,True, _
    180,0,False,0,0,"0",False,0,"",False,"","","","","")

Set MakeRestoreOS = objTask 

End Function

Public Function MakeRefreshOS(objJobXML,objTasklist,objTask)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to refresh the currently installed OS                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Refresh OS",True,False,True,"", _
                    "","","","",False)
End If
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "cscript.exe //nologo %DITS_Path%\RefreshOS.vbs","",True,True, _
    1200,120,False,0,0,"0",False,1,"",False,"","","","","")
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
    "%DITS_Path%\Desktop.exe 255 128 0","",True,True, _
    180,0,False,0,0,"0",False,0,"",False,"","","","","")

Set MakeRefreshOS = objTask 

End Function

Public Function MakeInstallProduct(objJobXML,objTasklist,objTask,ByVal sProduct,ByVal sBuild,ByVal sType,ByVal sLang)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install a product.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        The product to install.                                                     
'       sBuild          The product build to install.                                               
'       sLang           The product language to install.                                               
'       sProdType       FRE, CHK or Patch 
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aTheProducts
Dim aTheBuilds
Dim aTheProdtypes
Dim sProductToInstall
Dim sBuildToInstall
Dim sProdTypeToInstall
Dim sPatch
Dim i                ' a counter
Dim objCommand

If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Install " & sProduct,False,False,False,"", _
                    "","","","",False)
End If

aTheProducts = Split(sProduct,",")
aTheBuilds = Split(sBuild,",")
aTheProdtypes = Split(sType,",")

' If installing AC Then get 2 static IPs
If UCase(Left(sProduct,2)) = "AC" And UCase(Left(sProduct,3)) <> "ACT" And UCase(aTheProdtypes(0)) <> "PATCH" Then
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%systemroot%\acstools\ipset.cmd get 2 ","",True,True, _
        600,0,False,0,0,"0",False,1,"",False,"","","","","")
End If

For i = 0 to UBound(aTheProducts) 
    sProductToInstall = aTheProducts(i)
    If i < UBound(aTheBuilds) Then
        sBuildToInstall = aTheBuilds(i)
    Else
        sBuildToInstall = aTheBuilds(UBound(aTheBuilds))
    End If
    If i < UBound(aTheProdtypes) Then
        sProdTypeToInstall = aTheProdtypes(i)
    Else
        sProdTypeToInstall = aTheProdtypes(UBound(aTheProdtypes))
    End If
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "\\acstools\drop\acinst.cmd " & sProductToInstall & " " & _
        sBuildToInstall & " " & sProdTypeToInstall,"",True,True, _
        1200,-120,False,0,0,"0",False,1,"",True,"","","","","")
    If UCase(sProductToInstall) = "AC10SP1" And UCase(sProdTypeToInstall) = "PATCH" Then
        SetCommandOptions objCommand,-1,-1000,False,0,0,"",False,0,""
    End If
    ' Language is specified through an environment variable
    If sLang <> "" Then
        MakeDITSEnvironment sDITSServer,objCommand,"LOC",sLang
    End If

    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%DITS_Path%\Desktop.exe 255 128 0","",True,True, _
        180,0,False,0,0,"0",False,0,"",False,"","","","","")
Next

Set MakeInstallProduct = objTask 

End Function

Public Function MakeRunBVT(objJobXML,objTasklist,objTask,ByVal sProduct,ByVal sBuild,ByVal sProdType, _
    ByVal Suite,ByVal iMachine)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to run the BVTs.                                                
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        The product to test.                                                        
'       sBuild          The test code build to use.                                                 
'       sProdType       FRE or CHK.                                                                 
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aTheProducts
Dim aTheBuilds
Dim aTheProdtypes
Dim sTheProdType
Dim sProductToTest
Dim sCommand
Dim sMachineSeparator
Dim objMachineList
Dim objMachine
Dim objCommand
Dim i                ' a counter

Rem Product MIGHT be a list. For the Product name and Build use the LAST entries
aTheProducts = Split(sProduct,",")
sProductToTest = aTheProducts(UBound(aTheProducts))

aTheBuilds = Split(sBuild,",")
sBuild = aTheBuilds(UBound(aTheBuilds))

Rem For the product type use the last value that wasn't "Patch"
sTheProdType = sProdType
aTheProdtypes = Split(sProdType,",")
For i = UBound(aTheProdtypes) To 0 Step -1
    If UCase(aTheProdtypes(i)) <> "PATCH" Then
        sTheProdType = aTheProdtypes(i)
        Exit For
    End If
Next

If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Run " & sProductToTest & " " & sTheProdType & _
                    " BVT",False,False,True,"", _
                    "","","","",False)
End If
If UCase(Left(sProduct,2)) = "AC" And UCase(Left(sProduct,3)) <> "ACT" Then
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%systemroot%\acstools\debug.cmd","",True,True, _
        180,0,False,5,30,"0",False,1,"",True,"","","","","")
    If iMachine = 0 Then
        Rem The first machine is where we run the Application Center Tests
        sCommand = "\\astdrop\drop\" & sProductToTest & "\x86\bvt\acsbvt.bat """
        sMachineSeparator = ""
        ' Get the list of machine for this job
        ' JOB is the parent of the parent of TASKLIST
        Set objMachineList = objTasklist.parentNode.parentNode.selectNodes("MACHINE")
        For Each objMachine In objMachineList
            sCommand = sCommand & sMachineSeparator & "%DITS_" & objMachine.getAttribute("machineid") & "%"
            sMachineSeparator = ","
        Next
        Rem Product and test builds should coincide
        sCommand = sCommand & """ " & sBuild & " " & Suite
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            sCommand,"",True,True, _
            -1,0,False,0,0,"0",False,1,"",True,"","","","","")
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            "CScript.exe //nologo %DITS_Path%\ScanLogs.vbs %SYSTEMDRIVE%\BVT """ & _
            sProductToTest & " " & sBuild & " " & sTheProdType & """","",True,True, _
            -1,0,False,0,0,"0",False,1,"",False,"","","","","")
    End If
Else
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "CMD.exe","",True,True, _
        -1,0,False,0,0,"0",False,0,"",False,"","","","","")
End If

Set MakeRunBVT = objTask 

End Function

Public Function MakeRunSmokes(objJobXML,objTasklist,objTask,ByVal sProduct,ByVal sBuild,ByVal sProdType, _
    ByVal Suite,ByVal iMachine)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to run smoke tests.                                             
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        The product to test.                                                        
'       sBuild          The test code build to use.                                                 
'       sProdType       FRE or CHK.                                                                 
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aTheProducts
Dim aTheBuilds
Dim aTheProdtypes
Dim sProductToTest
Dim sTheProdType
Dim sCommand
Dim sMachineSeparator
Dim objMachineList
Dim objMachine
Dim objCommand
Dim i                ' a counter

Rem Product MIGHT be a list. For the Product name and Build use the LAST entries
aTheProducts = Split(sProduct,",")
sProductToTest = aTheProducts(UBound(aTheProducts))

aTheBuilds = Split(sBuild,",")
sBuild = aTheBuilds(UBound(aTheBuilds))

Rem For the product type use the last values that wasn't "Patch"
sTheProdType = sProdType
aTheProdtypes = Split(sProdType,",")
For i = UBound(aTheProdtypes) To 0 Step -1
    If UCase(aTheProdtypes(i)) <> "PATCH" Then
        sTheProdType = aTheProdtypes(i)
        Exit For
    End If
Next

If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Run " & sProductToTest & " " & sTheProdType & _
                    " Smokes",False,False,True,"", _
                    "","","","",False)
End If
If UCase(Left(sProduct,2)) = "AC" And UCase(Left(sProduct,3)) <> "ACT" Then
    ' GLENNLAV - added line to merge into reg key to prevent event log overflow
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%SYSTEMROOT%\regedit -s \\acstools\drop\applogfull.reg","",True,True, _
        -1,0,False,0,0,"0",False,1,"",False,"","","","","")
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%systemroot%\acstools\debug.cmd","",True,True, _
        180,0,False,5,30,"0",False,1,"",False,"","","","","")
    If iMachine = 0 Then
        Rem The first machine is where we run the Application Center Tests
        sCommand = "\\astdrop\drop\" & sProductToTest & "\x86\Smoke\ACSSmoke.bat """
        sMachineSeparator = ""
        ' Get the list of machine for this job
        ' JOB is the parent of the parent of TASKLIST
        Set objMachineList = objTasklist.parentNode.parentNode.selectNodes("MACHINE")
        For Each objMachine In objMachineList
            sCommand = sCommand & sMachineSeparator & "%DITS_" & objMachine.getAttribute("machineid") & "%"
            sMachineSeparator = ","
        Next
        Rem Product and test builds should coincide
        sCommand = sCommand & """ " & sBuild & " " & Suite
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            sCommand,"",True,True, _
            -1,0,False,0,0,"0",False,1,"",False,"","","","","")
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            "CScript.exe //nologo %DITS_Path%\ScanLogs.vbs %SYSTEMDRIVE%\SMOKE """ & _
            sProductToTest & " " & sBuild & " " & sTheProdType & """","",True,True, _
            -1,0,False,0,0,"0",False,1,"",False,"","","","","")
    End If
Else
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "CMD.exe","",True,True, _
        -1,0,False,0,0,"0",False,0,"",False,"","","","","")
End If

Set MakeRunSmokes = objTask 

End Function

Public Function MakeRunFuncs(objTasklist,objTask,ByVal sProduct,ByVal sBuild,ByVal sProdType, _
    ByVal Suite,ByVal iMachine)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to run smoke tests.                                             
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        The product to test.                                                        
'       sBuild          The test code build to use.                                                 
'       sProdType       FRE or CHK.                                                                 
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aTheProducts
Dim aTheBuilds
Dim aTheProdtypes
Dim sProductToTest
Dim sTheProdType
Dim sCommand
Dim sMachineSeparator
Dim objMachineList
Dim objMachine
Dim objCommand
Dim i                ' a counter

Rem Product MIGHT be a list. For the Product name and Build use the LAST entries
aTheProducts = Split(sProduct,",")
sProductToTest = aTheProducts(UBound(aTheProducts))

aTheBuilds = Split(sBuild,",")
sBuild = aTheBuilds(UBound(aTheBuilds))

Rem For the product type use the last values that wasn't "Patch"
sTheProdType = sProdType
aTheProdtypes = Split(sProdType,",")
For i = UBound(aTheProdtypes) To 0 Step -1
    If UCase(aTheProdtypes(i)) <> "PATCH" Then
        sTheProdType = aTheProdtypes(i)
        Exit For
    End If
Next

If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Run " & sProductToTest & " " & sTheProdType & _
                    " Funcs",False,False,True,"", _
                    "","","","",False)
End If
If UCase(Left(sProduct,2)) = "AC" And UCase(Left(sProduct,3)) <> "ACT" Then
    ' GLENNLAV - added line to merge into reg key to prevent event log overflow
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%SYSTEMROOT%\regedit -s \\acstools\drop\applogfull.reg","",True,True, _
        -1,0,False,0,0,"0",False,1,"",False,"","","","","")
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "%systemroot%\acstools\debug.cmd","",True,True, _
        180,0,False,5,30,"0",False,1,"",True,"","","","","")
    If iMachine = 0 Then
        Rem The first machine is where we run the Application Center Tests
        sCommand = "\\astdrop\drop\" & sProductToTest & "\x86\Functionals\ACSFunc.bat """
        sMachineSeparator = ""
        ' Get the list of machine for this job
        ' JOB is the parent of the parent of TASKLIST
        Set objMachineList = objTasklist.parentNode.parentNode.selectNodes("MACHINE")
        For Each objMachine In objMachineList
            sCommand = sCommand & sMachineSeparator & "%DITS_" & objMachine.getAttribute("machineid") & "%"
            sMachineSeparator = ","
        Next
        Rem Product and test builds should coincide
        sCommand = sCommand & """ " & sBuild & " " & Suite
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            sCommand,"",True,True, _
            -1,0,False,0,0,"0",False,1,"",True,"","","","","")
        Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
            "CScript.exe //nologo %DITS_Path%\ScanLogs.vbs %SYSTEMDRIVE%\FUNC """ & _
            sProductToTest & " " & sBuild & " " & sTheProdType & """","",True,True, _
            -1,0,False,0,0,"0",False,1,"",False,"","","","","")
    End If
Else
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "CMD.exe","",True,True, _
        -1,0,False,0,0,"0",False,0,"",False,"","","","","")
End If

Set MakeRunFuncs = objTask 

End Function


'GLENNLAV
Public Function MakeInstallPrivateBinaries(objTasklist, ByVal sProdBuild, ByVal sPrivShare, ByVal sPrivFolder, ByVal sTargetPath)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    Replace one or more binaries at target path or sub-folders with equivalent stored on a remote share                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       sPrivShare      Share where private binaries are stored
'       sPrivFolder     Folder where private binaries are stored
'       sTargetPath     Path on machines to replace any found binaries
'
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------
    Const cPrivateInstallScript = "\\acstools\drop\PrivatesFiles.vbs"

    Dim objTask
    Dim sSetupCmd

	' Install Privates on all machines
	sSetupCmd = "cscript //nologo " & cPrivateInstallScript & " /privshare:" & sPrivShare& " /privfolder:" & sPrivFolder & " /target:" & sTargetPath

	Set objTaskNode = MakeNewTask(objMachineTasklistNode, "Put private binaries on box",False,False,"","","")
	Set objCommand = MakeNewCommand(objTaskNode, sSetupCmd, -2 * cFiveMinInSec, 0, "", "", "")
	SetCommandOptions objCommand,cFiveMinInSec,cFiveMinInSec,False,3,60,0,False,1,""
    
    MakeInstallPrivateBinaries = objTask
    
End Function 


' GLENNLAV
Public Function MakeInstallMOMXFull(objTasklist, ByVal sProdBuild, ByVal sProdType, ByVal sProdLang, ByVal sProdPath, ByVal bInstallMCF)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install Rosetta.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       sProdBuild      Build number of MOMX to install - default is blessed
'       sProdType       Type of MOMX to install - default is nonopt
'       sProdPath       Path to MSI to install - default is official build
'       sVersion        MOM component to install - default is complete (reporting
'       bInstallMCF     Install MOMX MCF
'
'    Returns:
'        the TASK node object.                                                                      
'
' Note: This function does no error checking for valid inputs - it just passes them along to the appropriate
'   standard install system. It assume that the Rosetta install script will return an appropriate error code If
'   Rosetta install fails.                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objTask
Dim aParms()

Const sMMPC = " /INSTALLMMPC"
Const sMOMAccounts = " /DAS_USER:smx\momdas /ACTION_USER:smx\momactionact /TASK_USER:smx\momdts /REP_USER:smx\momdw"

If (bInstallMCF) Then
    ReDim aParms(5,1)
    If (sProdPath <> Empty) Then
        aParms(4,0) = "%optional"
        aParms(4,1) = "/path:" & sProdPath
        If (bInstallMCF) Then
            aParms(4,1) = aParms(4,1) & sMMPC & sMOMAccounts
        End If
    Else
        aParms(4,0) = "%optional"
        aParms(4,1) = sMOMAccounts
        If (bInstallMCF) Then
            aParms(4,1) = aParms(4,1) & sMMPC
        End If
    End If
Else
    ReDim aParms(4,1)
    If (sProdPath <> Empty) Then
        ReDim aParms(5,1)
        aParms(4,0) = "%optional"
        aParms(4,1) = "/path:" & sProdPath & sMOMAccounts
    End If
End If

aParms(0,0) = "%build"
If (sProdBuild = Empty) Then
   aParms(0,1) = "blessed"
Else
    aParms(0,1) = sProdBuild
End If

aParms(1,0) = "%type"
If (sProdType = Empty) Then
   aParms(1,1) = "nonopt"
Else
    aParms(1,1) = sProdType
End If

aParms(2,0) = "%language"
If (sProdLang = Empty) Then
   aParms(2,1) = "EN"
Else
    aParms(2,1) = sProdLang
End If

aParms(3,0) = "%version"
aParms(3,1) = "complete"

Set objTask = MakeStandardTask(objTasklist,"Install MOMX",aParms,"","","")
objTask.setAttribute "name", "MOMX Install - Complete"

aParms(3,1) = "reporting"

Set objTask = MakeStandardTask(objTasklist,"Install MOMX",aParms,"","","")
objTask.setAttribute "name", "MOMX Install - Reporting"

Set MakeInstallMOMXFull = objTask 

End Function

' GLENNLAV
Function MakeInstallCoverageRuntime(objTasklist)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'   Install the Magellan Coverage Runtime                                   
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------
	dim sSetupCmd
	sSetupCmd = "cscript //nologo \\acstools\toolbox\magellan\Ver4.5\MagAutoInstall.vbs /Coverage Runtime"

	dim objTaskNode, objCommand
		
	set objTaskNode = MakeNewTask(objTasklist, "Install Coverage Runtime",False,False,"","","")
	set objCommand = MakeNewCommand(objTaskNode, sSetupCmd, -2 * cFiveMinInSec, 0, "", "", "")
	SetCommandOptions objCommand,(-2 * cFiveMinInSec),0,false,10,60,0,false,1,""
	
	Set MakeInstallCoverageRuntime = objTaskNode
	
End Function

' GLENNLAV
Function MakeInstallCoverageRuntimeAll(objJob)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'   Install the Magellan Coverage Runtime                                   
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------
	dim sSetupCmd
	sSetupCmd = "cscript //nologo \\acstools\toolbox\magellan\Ver4.5\MagAutoInstall.vbs /Coverage Runtime"

    Dim objMachineList, objMachine, objTasklist
	dim objTaskNode, objCommand

    Set objMachineList = objJob.selectNodes("MACHINE")
    For Each objMachine In objMachineList
        Set objTasklist = objMachine.selectSingleNode("TASKLIST")
    	set objTaskNode = MakeNewTask(objTasklist, "Install Coverage Runtime",False,False,"","","")
    	set objCommand = MakeNewCommand(objTaskNode, sSetupCmd, -2 * cFiveMinInSec, 0, "", "", "")
    	SetCommandOptions objCommand,(-2 * cFiveMinInSec),0,false,10,60,0,false,1,""
    Next

	Set MakeInstallCoverageRuntimeAll = nothing
	
End Function



' GLENNLAV
Function MakeImportCoverageInfo(objTasklist, sTraceName)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install MOMX.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       sTraceName      The trace name to save the coverage data under
'
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------

	' Install Privates on all machines
	dim sSetupCmd
	sSetupCmd = "\\acstools\toolbox\magellan\Ver4.5\coverageimport.bat " & sTraceName

	dim objTaskNode, objCommand
		
	set objTaskNode = MakeNewTask(objTasklist, "Import coverage data for " & sTraceName,False,False,"","","")
	set objCommand = MakeNewCommand(objTaskNode, sSetupCmd, -2 * cFiveMinInSec, 0, "", "", "")
	SetCommandOptions objCommand,(-2 * cFiveMinInSec),0,false,3,60,"0,1",false,1,""
	
	Set MakeImportCoverageInfo = objTaskNode
End Function




' GLENNLAV
Public Function MakeAVDetector(objTasklist, objTask, bTurnon)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will turn on or off the AV Detector                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       bTurnon         If true, turn on AV detection, otherwise turn off
'       
'
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------
Dim objCommand
    If Not IsObject(objTask) Then
        Set objTask = MakeNewTask(objTasklist,"AV Detector",0,0,"","","")
    End If
    
    Dim sCmd
    If (bTurnon) Then
        sCmd = "SetDebugOptions.wsf /Default"
    Else
        sCmd = "SetDebugOptions.wsf /Restore"
    End if
    
    set objCommand = MakeNewCommand(objTask,sCmd, -1,-120,"","","")
    SetCommandOptions objCommand,3600,-60,False,0,0,"0",False,0,""
    
    Set MakeAVDetector = objTask 
End Function

' GLENNLAV
Public Function MakeAVDetectorAll(objJob, bTurnon)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will turn on or off the AV Detector                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The Job node object                           
'       bTurnon         If true, turn on AV detection, otherwise turn off
'       
'    Returns:
'        the TASK node object.                                                                      
'
' ---------------------------------------------------------------------------------------------------------
    Dim sCmd
    If (bTurnon) Then
        sCmd = "SetDebugOptions.wsf /Default"
    Else
        sCmd = "SetDebugOptions.wsf /Restore"
    End If

    Dim objMachineList, objMachine, objTasklist
	dim objTaskNode, objCommand

    Set objMachineList = objJob.selectNodes("MACHINE")
    For Each objMachine In objMachineList
        Set objTasklist = objMachine.selectSingleNode("TASKLIST")
        Set objTaskNode = MakeNewTask(objTasklist,"AV Detector",0,0,"","","")
        set objCommand = MakeNewCommand(objTaskNode,sCmd, -1,-120,"","","")
        SetCommandOptions objCommand,3600,-60,False,0,0,"0",False,0,""
    Next

    Set MakeAVDetectorAll = Nothing 

End Function


' GLENNLAV
Public Function MakeRunExecEngineEx(objTasklist,objTask,ByVal sProduct, ByVal sTestType, ByVal sConfig, ByVal sBuild, ByVal sSuite, ByVal sLogs, ByVal sTestPath, ByVal sCover, ByVal bExecMachine)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to run tests using the execution engine.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        Product being tested 
'       sTestType       Level of tests to run (BVT, Smoke, Funcs)
'       sConfig         Configuration of tests to run - if left blank, Exec Engine uses defaults
'       sBuild          Build number to test On
'       sLogs           Where you want test logs save (blank is nowhere)
'       sTestPath       Where is the test drop located (blank is default, offical code)
'       sCover          Trace name to save coverage results as (blank is no coverage results saved)
'       bExecMachine    Is this the main 
'
'    Returns:
'        the TASK node object.                                                                      
'
'
' TO DO: Turn into standard task on Web Service - this becomes wrapper call
' ---------------------------------------------------------------------------------------------------------
Dim objCommand

    If Not IsObject(objTask) Then
        Set objTask = MakeNewTask(objTasklist,"Run Exec Engine: " & sProduct & "-" & sTestType,0,0,"","","")
    End If
    
    if (bExecMachine) Then
        ' Create the command line to run Exec Engine
        dim sCmdLine
        
        If (sTestPath = Empty) Then
            sCmdLine = "cscript \\astdrop\drop\" & sProduct & "\" & sBuild & "\x86\exec\exectest.vbs"
        Else
            sCmdLine = "cscript " & sTestPath & "\x86\exec\exectest.vbs"
        End If

        ' Put in mandatory parameters
        sCmdLine = sCmdLine & " " & sProduct & " " & sTestType & " "
        
        dim sMachineSeparator, objMachineList, objMachine
        sMachineSeparator = ""
        ' Extract list of machines from job definition - put into Exec Engine
        Dim sMachineList
        sMachineList = Chr(34)
        Set objMachineList = objTasklist.parentNode.parentNode.selectNodes("MACHINE")
        For Each objMachine In objMachineList
            sMachineList = sMachineList & sMachineSeparator & "%DITS_" & objMachine.getAttribute("machineid") & "%"
            sMachineSeparator = ","
        Next
        sMachineList = sMachineList & Chr(34)
        sCmdLine = sCmdLine & sMachineList
        
        ' Fill in all parameters 
        sCmdLine = sCmdLine & " /build:" & sBuild
        
        if (sSuite <> Empty) then
            sCmdLine = sCmdLine & " /suite:" & chr(34) & sSuite & chr(34)
		end if         
        
        if (sConfig <> Empty) then
            sCmdLine = sCmdLine & " /config:" & chr(34) & sConfig & chr(34)
        end If

        if (sLogs <> Empty) then
            sCmdLine = sCmdLine & " /log:" & chr(34) & sLogs & chr(34)
        end if
        
        If (sTestPath <> Empty) Then
            sCmdLine = sCmdLine & " /droppoint:" & chr(34) & sTestPath & chr(34)
        End If
        
        If (sCover <> Empty) Then
            sCmdLine = sCmdLine & " /cover:" & chr(34) & sCover & chr(34)
        End If
        
        set objCommand = MakeNewCommand(objTask,sCmdLine,-1,-120,"","","")
        SetCommandOptions objCommand,0,0,False,0,0,"0",False,1,""
        objCommand.setAttribute "interactive",true
 
        Set objCommand = MakeNewCommand(objTask,"CScript.exe //nologo %DITS_Path%\ScanLogsEx.vbs %SYSTEMDRIVE%\" & sTestType & " " & sMachineList & " " & sProduct & " DITS",0,0,"","","")
        SetCommandOptions objCommand,0,0,False,0,0,"0",False,1,""
    end if

    Set MakeRunExecEngineEx = objTask 

End Function


' GLENNLAV
Public Function MakeInstallRosetta(objTasklist,objTask, ByVal sProdBuild)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install Rosetta.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProdBuild      Build number of Rosetta to install - default is blessed
'
'    Returns:
'        the TASK node object.                                                                      
'
' Note: This function does no error checking for valid inputs - it just passes them along to the appropriate
'   standard install system. It assume that the Rosetta install script will return an appropriate error code If
'   Rosetta install fails.                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aParms()
Redim aParms(0,1)
aParms(0,0) = "%build"
If (sProdBuild = Empty) Then
    aParms(0,1) = "blessed"
Else
    aParms(0,1) = sProdBuild
End If

Set objTask = MakeStandardTask(objTasklist,"Install Rosetta",aParms,"","","")

Set MakeInstallRosetta = objTask 

End Function

Public Function MakeInstallSQL(objTasklist,objTask, ByVal sProdType, ByVal sProdLang, ByVal sInstanceName)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install SQL Server 2000 with SP 3.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProdType       Standard, Enterprise, Personal or Developer 
'       sProdLang       Code for language to install - default is english                                                                                                
'       sInstanceName   If you want to create a specific named instance - optional
'
'    Returns:
'        the TASK node object.                                                                      
'
' Note: This function does no error checking for valid inputs - it just passes them along to the appropriate
'   standard install system. It assume that the SQL Inst system will return an appropriate error code If
'   SQL install fails.                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aParms()
Redim aParms(2,1)
aParms(0,0) = "%version"
If (UCASE(sProdType) = "ENTERPRISE") Then
    sProdType = "ENT"
End If
aParms(0,1) = sProdType
aParms(1,0) = "%instance"
aParms(1,1) = sInstanceName
If (sProdLang <> "") Then
    aParms(2,0) = "%lang"
    aParms(2,1) = sProdLang
End If

Set objTask = MakeStandardTask(objTasklist,"Install SQL",aParms,"","","")

Set MakeInstallSQL = objTask 

End Function

Public Function MakeInstallOffice(objTasklist,objTask, ByVal sProduct, ByVal sVersion, ByVal sProdLang)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to install Office.                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sProduct        Part of Office to install - either Outlook, Access or Office                                               
'       sVersion        Version of Office to install - either XP or 2K
'       sProdLang       Language of Office to install - default in English
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'
' Note: This function does no error checking for valid inputs - it just passes them along to the appropriate
'    standard install system. It assume that the Office Inst system will return an appropriate error code If
'   Office install fails.                                                                                                       
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Install " & sProduct,False,False,False,"", _
                    "","","","",False)
End If

If (sProdLang = "") Then
    sProdLang = "en"
End If

If (LCASE(sProdLang) <> "en") Then
        MakeDITSEnvironment objTaskNode,"LOC",sProdLang
End If

' Command options: Timeout:60 minutes, Will accept reboot at End, will pause on failure code
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "\\acstools\drop\offinst.cmd " & sProduct & " " & _
        sVersion,"",True,True, _
        3600,-300,False,0,0,"0",False,1,"", _
        False,"","","","","")

Set MakeInstallOffice = objTask 

End Function

Public Function MakeUpgradeW2KwithSP(objTasklist,objTask,ByVal sSP)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create commands to upgrade an operating system on a W2K machine to a given service pack.                  
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sSP             Service Pack to install (only SP3 valid at this point)              
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
' 
'
' Note: This function does no error checking for valid inputs - it just passes input parameters along to the
'   appropriate underlaying install system.                                                                                  
'                                                                                                    
' ---------------------------------------------------------------------------------------------------------
Dim objCommand

If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Upgrade W2K OS to " & sSP,False,False,False,"", _
                    "","","","",False)
End If

' Command options: Timeout:60 minutes, Requires a reboot to happen at End, will pause on failure code
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        "\\acstools\drop\winspinst.cmd " & sSP,"",True,True, _
        3600,300,False,0,0,"0",False,1,"", _
        False,"","","","","")

Set MakeUpgradeW2KwithSP = objTask 

End Function

Public Function MakeUpgradeACforW2K3(objTasklist,objTask,ByVal sACInstallPath)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create command to upgrade an AC SP 2 installation to support Win 2003.                  
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist     The TASKLIST node object returned by MakeTasklist                           
'       objTask         The task to add the commands to. If Null a new task is created.             
'       sACIntallPath    Path to where AC was installed on local machine
'                        If blank, defaults to standard install point for AC              
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------

Dim objCommand
If Not IsObject(objTask) Then
    Set objTask = MakeDITSTask(sDITSServer,objTasklist, _
                    "Upgrade AC for Win 2003",False,False,False,"", _
                    "","","","",False)
End If

If (sACInstallPath = "") Then
    sACInstallPath = "%systemdrive%\Program Files\Microsoft Application Center"
End If

Dim sCmdLine
    sCmdLine = "CMD.EXE /C cd /d " & sACInstallPath & " & " & " W2003SrvPostUpgrade.bat ACSP2PostPatch.log"

' Command options: Timeout:20 minutes, No reboot expected, will pause on failure code
Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        sCmdLine,"",True,True, _
        1200,0,False,0,0,"0",False,1,"", _
        False,"","","","","")

Set MakeUpgradeACforW2K3 = objTask 

End Function

Public Function MakeCommands(objJobXML,objTask,ByVal Commands)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create several COMMAND nodes in the XML.                                        
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML       An IXMLDOMDocument object for the job XML. Ignored but retained for compatability
'       objTask         The TASK node object returned by MakeTask.                                  
'       Commands        Commands for the job.                                                       
'                                                                                                       
'    Returns:
'        the JOB node object.                                                                       
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim i                ' a counter
Dim aTheCommands
Dim objCommand

aTheCommands = Split(Commands,",")
For i = 0 to UBound(aTheCommands) Step 3
    Set objCommand = MakeDITSCommand(sDITSServer,objTask, _
        aTheCommands(i),"",True,True, _
        aTheCommands(i + 1),aTheCommands(i + 2),False,0,0,"0",False,0,"", _
        False,"","","","","")
Next

End Function

Public Function SubmitJob(objJobXML,sDITSServer,bDebugging,sSaveToFile)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                        
'    This function will create several COMMAND nodes in the XML.                                        
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJobXML           An IXMLDOMDocument object for the job XML.                                  
'       sDITSServer         The DITS Server to submit to.                                               
'       bDebugging          Running script in debug mode.                                               
'       sSaveToFile         Save the job XML to a file. This can be a null string,
'                           a file name, or a Boolean value.                                                 
'                                                                                                       
'    Returns:
'        True or False        False says an error occurred.
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim sFilename
Dim bSubmit
Dim objXMLHttp
Dim iJobNumber
Dim sJobName
Dim sJobComments
Dim sJobError
Dim objJobNode
Dim objErrorNode
Dim objShell    ' Windows Scripting Host Shell object
Dim bSaveToFile
Dim sXML

If VarType(sSaveToFile) = vbBoolean Or UCase(sSaveToFile) = "TRUE" Or UCase(sSaveToFile) ="FALSE" Then
    bSaveToFile = CBool(sSaveToFile)
    If bSaveToFile Then
        sFilename = InputBox("Enter the file name for the saved DITS Job XML.","Save the DITS Job XML","")
        If Not IsNull(sFilename) And Not IsEmpty(sFilename) And Len(sFilename) > 0 Then
            objJobXML.Save sFilename
        End If
    End If
ElseIf Len(sSaveToFile) > 0 Then
    objJobXML.Save sSaveToFile
End If

bSubmit = True
If bDebugging Then
    WScript.Echo objJobXML.xml
    Set objShell = WScript.CreateObject("WScript.Shell")
    If vbNo = objShell.Popup("Do you wish to submit the job(s)?",0,"DITS",vbYesNo + vbQuestion) Then
        bSubmit = False
        SubmitJob = True        ' this is not an error, just a user request.
    End If
End If

' Don't allow ASTTEST as the job owner
For Each objJobNode In objJobXML.selectNodes("//JOB")
    If UCase(objJobNode.getAttribute("owner")) = "ASTTEST" Then
        WScript.Echo "ERROR:ASTTEST must not be used as the job owner. Job(s) not submitted."
        WScript.Quit 42
    End If
Next

If bSubmit Then
    Set objXMLHttp = CreateObject("Microsoft.XMLHTTP")
    On Error Resume Next
    Dim sURL
    ' If the DITSServer name has more than 1 level, remove the last one.
    ' ie. LESSA/DITS becomes LESSA here.
    sURL = "http://"
    If InStrRev(sDITSServer,"/") = 0 Then
        sURL = sURL + sDITSServer
    Else
        sURL = sURL + Left(sDITSServer,InStrRev(sDITSServer,"/") - 1) 
    End If
    sURL = sURL +  "/DITSQueue/DITSQueue.asmx/ScheduleJob"
    objXMLHttp.Open "POST", sURL , false 
   
    If Err = 0 Then
        sXML = URLEncode(objJobXML.xml)
        objXMLHttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
        objXMLHttp.setRequestHeader "Content-Length", Len(sXML) + 8
        ' the 8 is the length of "xmldata="
        objXMLHttp.Send "xmldata=" + sXML

        If Err = 0 Then
            On Error Goto 0
            If objXMLHttp.status = 200 Then
                Set objJobXML = CreateObject("Msxml2.DOMDocument.3.0")
                objJobXML.loadXML(objXMLHttp.responseXML.documentElement.firstChild.text)

                For Each objJobNode In objJobXML.selectNodes("//JOB")
                    iJobNumber   = objJobNode.getAttribute("number")
                    sJobName     = objJobNode.getAttribute("name")
                    sJobComments = objJobNode.getAttribute("comments")
                    sJobError    = objJobNode.getAttribute("error")
                    If Err = 0 And Len(iJobNumber) <> 0 And IsNull(sJobError) Then
                        If iJobNumber > 0 Then
                            WScript.Echo "Job " & iJobNumber & " " & sJobName & "[" & sJobComments & "]" & " submitted."
                        Else
                            WScript.Echo "Job " & sJobName & " scheduled."
                        End If
                        SubmitJob = True
                    ElseIf Err = 0 And Len(iJobNumber) <> 0 And Len(sJobError) > 0 Then                        
                        WScript.Echo "Job " & iJobNumber & " " & sJobName & "[" & sJobComments & "]" & " " & sJobError & "."
                    Else
                        WScript.Echo "Error submitting " & sJobName & "[" & sJobComments & "]" & " job. XML nodes with errors:"
                        For Each objErrorNode In objJobXML.selectNodes("//*[@error!=""""]")
                            WScript.Echo objErrorNode.cloneNode(False).xml
                        Next
                        SubmitJob = False
                    End If
                Next
            Else
                WScript.Echo "Job could not be submitted. XMLHTTP status=" & objXMLHttp.statusText & "."
                SubmitJob = False
            End If
        Else
            WScript.Echo "Job could not be submitted. XMLHTTP.Send Err=" & Err & " " & Err.Description & "."
            SubmitJob = False
        End If
    Else
        WScript.Echo "Job could not be submitted. XMLHTTP.Open Err=" & Err & " " & Err.Description & "."
        SubmitJob = False
    End If
End If

End Function

Public Function DITSJobCreationWebService(sDITSServer,sRequest,sData)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                        
'    This function will call the DITS Job Creation Web Service.                                        
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sDITSServer         The DITS Server.                                               
'       sRequest            .                                                 
'       sData               .                                                 
'                                                                                                       
'    Returns:
'       The response string.
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objXMLHttp
Dim sURL

Set objXMLHttp = CreateObject("Microsoft.XMLHTTP")
On Error Resume Next
' If the DITSServer name has more than 1 level, remove the last one.
' ie. LESSA/DITS becomes LESSA here.
sURL = "http://"
If InStrRev(sDITSServer,"/") = 0 Then
    sURL = sURL + sDITSServer
Else
    sURL = sURL + Left(sDITSServer,InStrRev(sDITSServer,"/") - 1) 
End If
sURL = sURL +  "/DITSJobCreation/DITSJobCreation.asmx/" & sRequest
objXMLHttp.Open "POST", sURL , false 
   
If Err = 0 Then
    objXMLHttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
    objXMLHttp.setRequestHeader "Content-Length", Len(sData)
    objXMLHttp.Send sData

    If Err = 0 Then
        On Error Goto 0
        If objXMLHttp.status = 200 Then
            Dim sXML
            sXML = objXMLHttp.responseXML.xml
            DITSJobCreationWebService = objXMLHttp.responseXML.documentElement.firstChild.text
        Else
            WScript.Echo "DITS Job Creation Web Service " & sRequest & "?" & sData & " request failed. XMLHTTP status=" & objXMLHttp.statusText & "."
        End If
    Else
        WScript.Echo "DITS Job Creation Web Service " & sRequest & "?" & sData & " request failed. XMLHTTP.Send Err=" & Err & " " & Err.Description & "."
    End If
End If
If IsNull(DITSJobCreationWebService) Or IsEmpty(DITSJobCreationWebService) Or Len(DITSJobCreationWebService) = 0 Then
    DITSJobCreationWebService = "<ERROR/>"
End If

End Function

Public Function MakeNotify(ByVal sNotify)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will format the email notify list                                                    
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sNotify              A list of users to notify.                                                  
'                                                                                                       
'    Returns:
'        the formatted notify list.                                                                 
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim sNotifyUsers
Dim aTheUsers
Dim i                ' a counter

Rem Add microsoft.com to the user ids If required and put them in the right format for email
aTheUsers = Split(sNotify,",")
For i = 0 to UBound(aTheUsers) 
    If Instr(aTheUsers(i),"@") = 0 Then
        sNotifyUsers = sNotifyUsers & aTheUsers(i) & "@microsoft.com;"
    Else
        sNotifyUsers = sNotifyUsers & aTheUsers(i) & ";"
    End If
Next
MakeNotify = sNotifyUsers 

End Function

Public Function ProcessDITSJobParameters(ByVal objArguments)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will process the JOB and MACHINE specifications                                                    
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objArguments            The WSH Arguments object
'                                                                                                       
'    Returns:
'        The DITS JOB XML node with child MACHINE and TASKLIST nodes.                                                                 
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objArgument ' an argument
Dim objShell    ' Windows Scripting Host Shell object
Dim objEnv      ' Environment object

Dim objJobNode      ' the DITS JOB node
Dim objMachineNode  ' a DITS MACHINE node
Dim objTasklistNode ' a DITS TASKLIST node

Dim sJobname    ' the DITS job name
Dim sNotify     ' notify
Dim sOwner      ' owner
Dim sComments   ' job comments
Dim iPriority   ' job priority
Dim iEstimated  ' estimated job completion time
Dim sJobgroup   ' job group
Dim sRelease    ' job group release
Dim iHold       ' hold value
Dim bEcho       ' echo

Set objShell = WScript.CreateObject("WScript.Shell")
Set objEnv   = objShell.Environment("Process")

' Set some defaults
sJobname    = "MOMX Test"
sNotify     = objEnv("USERNAME")
sOwner      = objEnv("USERNAME")
iPriority   = 128
iEstimated  = 0
iHold       = 0

If objArguments.Named.Exists("J") Then
    sJobname = objArguments.Named("J")
End If
If objArguments.Named.Exists("N") Then
    sNotify = objArguments.Named("N")
End If
If objArguments.Named.Exists("O") Then
	sOwner = objArguments.Named("O")
End If
If objArguments.Named.Exists("C") Then
	sComments = objArguments.Named("C")
End If
If objArguments.Named.Exists("P") Then
	iPriority = CInt(objArguments.Named("P"))
End If
If objArguments.Named.Exists("E") Then
	iEstimated = CInt(objArguments.Named("E"))
End If
If objArguments.Named.Exists("G") Then
	sJobgroup = objArguments.Named("G")
End If
If objArguments.Named.Exists("H") Then
	iHold = CInt(objArguments.Named("H"))
End If
If objArguments.Named.Exists("R") Then
    sRelease = objArguments.Named("R")
End If

' Validate the notify parameter. It must NOT be asttest.
If UCase(sNotify) = "ASTTEST" Then
    WScript.Echo "ERROR:You can not specify " & sNotify & " for email notification." & vbCrLf _
        & "Please specify ""/N userid"" to override."
Else
    Set objJobNode = MakeDITSJob(sDITSServer,sJobname,sNotify, _
                        sOwner,sComments,iEstimated, _
                        iPriority,iHold,sJobgroup,sRelease)
                        
    ' Set ECHO environment variable?                        
    If objArguments.Named.Exists("ECHO") Then
        bEcho = objArguments.Named("ECHO")
        If IsNull(bEcho) Or IsEmpty(bEcho) Then
            bEcho = True
        End If
    Else
        bEcho = False
    End If
    If bEcho Then
        MakeDITSEnvironment sDITSServer,objJobNode,"ECHO","ON"
    End If

    For Each objArgument in objArguments.Unnamed
        Set objMachineNode  = MakeDITSMachine(sDITSServer,objJobNode,"",objArgument,"","","","","")
        Set objTasklistNode = MakeDITSTasklist(sDITSServer,objMachineNode, _
                                    "","","","","","",False)
    Next
End If    

Set ProcessDITSJobParameters = objJobNode

End Function

Public Function GetDITSServer(ByVal objArguments)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will get the DITS Server name from the arguments                                                    
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objArguments            The WSH Arguments object
'                                                                                                       
'    Returns:
'        The DITS Server Name
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim sDITSServer ' The DITS Server name

' Set the default
sDITSServer = "BIGHEART"

If objArguments.Named.Exists("S") Then
    sDITSServer    = objArguments.Named("S")
End If

GetDITSServer = sDITSServer

End Function

Public Function GetDebugMode(ByVal objArguments)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will get the debug mode specified
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objArguments            The WSH Arguments object
'                                                                                                       
'    Returns:
'        True of False
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
If objArguments.Named.Exists("D") Then
    GetDebugMode = objArguments.Named("D")
    If IsNull(GetDebugMode) Or IsEmpty(GetDebugMode) Then
        GetDebugMode = True
    End If
Else
    GetDebugMode = False
End If

End Function

Public Function GetSaveFile(ByVal objArguments)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will get the file name for the XML
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objArguments            The WSH Arguments object
'                                                                                                       
'    Returns:
'        A file name or a null string
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
If objArguments.Named.Exists("F") Then
    GetSaveFile = objArguments.Named("F")
Else
    GetSaveFile = ""
End If

End Function

Public Function ValidateArguments(ByVal objArguments)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will return an array of the named arguments that DITSTasks
'     does not support.
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objArguments            The WSH Arguments object
'                                                                                                       
'    Returns:
'        
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim aArguments()' array of arguments we don't understand
Dim objArgument ' an argument
Dim iArgument   ' a counter

ReDim aArguments(objArguments.Named.Count)
iArgument = 0
For Each objArgument In objArguments.Named
    Select Case UCase(objArgument)
	Case "S"
	Case "J"
	Case "N"
	Case "O"
	Case "C"
	Case "P"
	Case "E"
	Case "G"
	Case "H"
	Case "R"
	Case "D"
	Case "F"
	Case "ECHO"
    Case Else
    aArguments(iArgument) = objArgument
    iArgument = iArgument + 1
    End Select
Next

ReDim Preserve aArguments(iArgument - 1)
ValidateArguments = aArguments

End Function

REM
REM
REM All the old DITS 2.0 compatible functions
REM
REM

Public Function MakeNewJob(ByVal sName,ByVal sNotify,ByVal sOwner,ByVal sComments,byVal iEstimated, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new JOB node in the XML.                                               
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       wName               The job name.                                                               
'       sNotify             A list of users to notify when significant job events occur.                
'       sOwner              The owner of the job.                                                       
'       sComments           Comments about the job.                                                     
'       iEstimated          Estimated time, in hours, for the job to complete.                          
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the JOB node object.                                                                       
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Set MakeNewJob = MakeDITSJob(sDITSServer,sName,sNotify,sOwner,sComments,iEstimated,128,0,"","")

End Function

Public Function MakeNewMachine(objJob,ByVal sMachineid,ByVal sMachine,ByVal sNotes, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a MACHINE node in the job XML                                            
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objJob              The JOB node object returned by MakeNewJob                                     
'       sMachineid          The machineid attribute for the machine. One is generated in this is empty. 
'       sMachine            The machine name or pool specification.                                     
'       sNotes              Notes to include with the machine checkout. Displayed in Lab Explorer.      
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the MACHINE node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objMachine
Dim objMachines
Dim iMachineNumber

Set objJobXML = CreateObject(DOMDOCUMENT)
Set objMachine = objJobXML.createElement("MACHINE")
If IsNull(sMachineid) Or IsEmpty(sMachineid) Or Len(sMachineid) = 0 Then
    Set objMachines = objJob.selectNodes("MACHINE")
    iMachineNumber = objMachines.Length + 1
    sMachineid = "Machine" & String(3 - Len(iMachineNumber),"0") & iMachineNumber
End If
objMachine.setAttribute "machineid",sMachineid
If UCase(Left(sMachine,5) ) = "POOL=" Then
    objMachine.setAttribute "pool",Mid(sMachine,6)
Else
    objMachine.setAttribute "name",sMachine
End If
objMachine.setAttribute "notes",sNotes
SetSecurity objMachine,sDomain,sAccount,sPassword,""
objJob.appendChild objMachine

Set MakeNewMachine = objMachine

End Function

Public Function MakeNewTasklist(objMachine,ByVal sName,ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new TASKLIST node in the job XML                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objMachine          The MACHINE node object returned by MakeNewMachine                             
'       sName               A name for the task list.                                                   
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the TASKLIST node object.                                                                  
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTasklist

Set objJobXML = CreateObject(DOMDOCUMENT)
Set objTasklist = objJobXML.createElement("TASKLIST")
objTasklist.setAttribute "name",sName
SetSecurity objTasklist,sDomain,sAccount,sPassword,""
objMachine.AppendChild objTasklist  

Set MakeNewTasklist = objTasklist 

End Function

Public Function MakeNewTask(objTasklist,ByVal sName,ByVal bEven,ByVal bOnly, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new TASK node in the job XML                                           
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTasklist         The TASKLIST node object returned by MakeNewTasklist                           
'       sName               A name for the task.                                                        
'       bEven               Set to True If the task should run even If the job is cancelled.            
'       bOnly               Set to True If the task should run only If the job is cancelled.            
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the TASK node object.                                                                      
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objTask

Set objJobXML = CreateObject(DOMDOCUMENT)
Set objTask = objJobXML.createElement("TASK")
objTask.setAttribute "name",sName
If bEven Then
    objTask.setAttribute "even",bEven
End If
If bOnly Then
    objTask.setAttribute "only",bOnly
End If
SetSecurity objTask,sDomain,sAccount,sPassword,""
objTasklist.AppendChild objTask  

Set MakeNewTask = objTask 

End Function

Public Function MakeNewCommand(objTask,ByVal sText,ByVal iTimeout,ByVal iWaitforreboot, _
    ByVal sDomain,ByVal sAccount,ByVal sPassword)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create a new COMMAND node in the job XML                                        
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objTask             The TASK node object returned by MakeNewTask.                                  
'       sText               A text of the command.                                                      
'       iTimeout            The time (in seconds) for the command to complete.                          
'                           -1 (INFINITE) or 0 means no timout processing.                              
'                           A positive value means the job will be paused If the command does not       
'                           complete in the time specified. A negative value means the job will be      
'                           cancelled If the command does not complete in the time specified.           
'       iWaitforreboot      The time (in seconds) for a reboot to occur.                              
'                           0 means no reboot is expected. The job will be paused If one occurs during  
'                           execution of this command. A positive value means the job will be paused If 
'                           If a reboot does not occur within the time specified. A negative value means
'                           that DITS will wait the specifiec time for a reboot to occur but the job    
'                           not be paused If one does not occur.                                        
'       sDomain             Security credentials                                                        
'       sAccount                                                                                         
'       sPassword                                                                                        
'                                                                                                       
'    Returns:
'        the COMMAND node object.                                                                   
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objCommand

Set objJobXML = CreateObject(DOMDOCUMENT)
Set objCommand = objJobXML.createElement("COMMAND")
SetCommandOptions objCommand,iTimeout,iWaitforreboot,False,0,0,"",False,0,""
objCommand.text = sText
SetSecurity objCommand,sDomain,sAccount,sPassword,""
objTask.AppendChild objCommand  

Set MakeNewCommand = objCommand

End Function

Public Function MakeNewEnvironment(objNode,ByVal sName,ByVal sValue)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will create an ENVIRONMENT node in the job XML                                       
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       objNode             The parent node object for this node.                                       
'       sName               The name of the environment variable to set.                                
'       sValue              The value for the environment variable.                                     
'                                                                                                       
'    Returns:
'        the ENVIRONMENT node object.                                                               
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objJobXML
Dim objEnvironment

Set objJobXML = CreateObject(DOMDOCUMENT)
Set objEnvironment = objJobXML.createElement("ENVIRONMENT")
objEnvironment.setAttribute "name",sName
objEnvironment.setAttribute "value",sValue
objNode.AppendChild objEnvironment 
    
Set MakeNewEnvironment = objEnvironment 

End Function

Public Function URLEncode(ByVal sData)
' ---------------------------------------------------------------------------------------------------------
'                                                                                                       
'    This function will do URL encoding of unsafe characters    
'    Please see http://www.w3.org/Addressing/rfc1738.txt search 
'    this text for 'Unsafe:'. It covers the unsafe characters in urls.                                    
'                                                                                                       
'    Parameters:                                                                                        
'                                                                                                       
'       sData               The data to encode
'                                                                                                       
'    Returns:
'        The encoded data
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
' Unsafe characters for URLs
sData = Replace(sData,"%","%25")
sData = Replace(sData,"""","%22")
sData = Replace(sData,"<","%3C")
sData = Replace(sData,">","%3E")
sData = Replace(sData,"+","%2B")
sData = Replace(sData," ",Chr(43))
sData = Replace(sData,"#","%23")
sData = Replace(sData,"{","%7B")
sData = Replace(sData,"}","%7D")
sData = Replace(sData,"|","%7C")
sData = Replace(sData,"\","%5C")
sData = Replace(sData,"^","%5E")
sData = Replace(sData,"~","%7E")
sData = Replace(sData,"[","%5B")
sData = Replace(sData,"]","%5D")
sData = Replace(sData,"`","%60")
' Reserved for special meanings
sData = Replace(sData,";","%3B")
sData = Replace(sData,"/","%2F")
sData = Replace(sData,"?","%3F")
sData = Replace(sData,":","%3A")
sData = Replace(sData,"=","%3D")
sData = Replace(sData,"&","%26")
    
URLEncode = sData 

End Function

Public Sub ProcessModel(objTMT2DITS,objTasklistNode,objTest)
' ---------------------------------------------------------------------------------------------------------
'                                    
'    The ProcessModel function.
'                                                                                                       
'    Parameters:                                                                                        
'        objTMT2DITS        An object that can examine and modify the model                                                                                                    
'        objTasklistNode    The TASKLIST
'        objTest            The Test element of the model                                                                                                    
'                                                                                                       
'    Returns:
'        Nothing
'                                                                                                       
' ---------------------------------------------------------------------------------------------------------
Dim objTransitionNodes  ' TRANSITION elements in the XTC file
Dim objTransition       ' TRANSITION element in the XTC file
Dim objActionNodes  ' ACTION elements in the XTC file
Dim objAction       ' ACTION element in the XTC file
Dim sAction         ' The ACTION name
Dim objParamNodes   ' PARAM elements in the XTC file
Dim objParam        ' PARAM element in the XTC file
Dim sParam          ' a PARAM name
Dim iParam          ' a counter for PARAMs
Dim aParms()        ' array of PARAMs
Dim objTaskNode     ' a DITS TASK node

Set objTransitionNodes = objTest.selectNodes("STEP/TRANSITION")
For Each objTransition in objTransitionNodes
    Set objActionNodes = objTransition.selectNodes("ACTION")
    For Each objAction in objActionNodes
        ' Call the user object
        If IsObject(objTMT2DITS) Then
            objTMT2DITS.TransitionCreate objTransition
        End If
        ' Now create the DITS task
        sAction = objAction.getAttribute("Name")
        ' Remove the underscores from the action name
        Do While InStr(sAction,"_") > 0 
            sAction = Left(sAction,InStr(sAction,"_") - 1) & " " & Mid(sAction,InStr(sAction,"_") + 1)
        Loop
        If Not IsNull(sAction) And Not IsEmpty(sAction) And Len(sAction) > 0 Then
            ' Build the array or parameters
            REM Set objParamNodes = objAction.selectNodes("PARAM[@Type='In']")
            Set objParamNodes = objTransition.selectNodes("PARAM")
            Redim aParms(objParamNodes.length,1)
            iParam = 0
            For Each objParam in objParamNodes
                ' The name to use follows the first (if any) underscore
                sParam = objParam.getAttribute("Name")
                If InStr(sParam,"_") > 0 Then
                    sParam = Mid(sParam,InStr(sParam,"_") + 1)
                End If
                aParms(iParam,0) = "%" & sParam
                aParms(iParam,1) = objParam.getAttribute("Value")
                iParam = iParam + 1
            Next   
            Set objTaskNode = MakeStandardTask(objTasklistNode,sAction,aParms,"","","")
        End If
    Next
Next

End Sub