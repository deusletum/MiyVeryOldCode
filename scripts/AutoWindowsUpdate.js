/* ======================================================================

NAME: AutoWindowsUpdate

AUTHOR: Dean Gjedde
DATE  : 2/24/2006

COMMENT: Will automated the windows update process

========================================================================= */
Main();

function Main()
{
    var updateSession = new ActiveXObject("Microsoft.Update.Session");
    var updateSearcher = updateSession.CreateupdateSearcher();
    var searchResult = updateSearcher.Search("IsInstalled=0 and Type='Software'");
    var updatesToDownload = new ActiveXObject("Microsoft.Update.UpdateColl");
    
    var update,downloader;

	
	WScript.Echo("Searching for updates");
	
	if(searchResult.Updates.Count == 0)
	{
	    WScript.Echo("There are no applicable updates.");
	    return;
    }
	
	for(var i = 0;i > searchResult.Updates.Count;i++)
	{
	    update = searchResult.Updates.Item(i);
	    wscript.echo(update.Title);
	    updatesToDownload.Add(update);
	}
	
	WScript.Echo("Downloading updates...");
	downloader = updateSession.CreateUpdateDownloader();
	downloader.Updates = updatesToDownload;
	downloader.Download();
	
}
// 
// Set updatesToInstall = CreateObject("Microsoft.Update.UpdateColl")
// 
// WScript.Echo  vbCRLF & _
// "Creating collection of downloaded updates to install:" 
// 
// For I = 0 To searchResult.Updates.Count-1
//     set update = searchResult.Updates.Item(I)
//     If update.IsDownloaded = true Then
//        WScript.Echo I + 1 & "> adding:  " & update.Title 
//        updatesToInstall.Add(update)	
//     End If
// Next
// 
// WScript.Echo  vbCRLF & "Would you like to install updates now? (Y/N)"
// strInput = WScript.StdIn.Readline
// WScript.Echo 
// 
// If (strInput = "N" or strInput = "n") Then 
// 	WScript.Quit
// ElseIf (strInput = "Y" or strInput = "y") Then
// 	WScript.Echo "Installing updates..."
// 	Set installer = updateSession.CreateUpdateInstaller()
// 	installer.Updates = updatesToInstall
// 	Set installationResult = installer.Install()
// 	
// 	'Output results of install
// 	WScript.Echo "Installation Result: " & _
// 	installationResult.ResultCode 
// 	WScript.Echo "Reboot Required: " & _ 
// 	installationResult.RebootRequired & vbCRLF 
// 	WScript.Echo "Listing of updates installed " & _
// 	 "and individual installation results:" 
// 	
// 	For I = 0 to updatesToInstall.Count - 1
// 		WScript.Echo I + 1 & "> " & _
// 		updatesToInstall.Item(i).Title & _
// 		": " & installationResult.GetUpdateResult(i).ResultCode 		
// 	Next
// End If		