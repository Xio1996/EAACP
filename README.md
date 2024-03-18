# EAACP
<p>A small application to integrate AstroPlanner and Stellarium. The application provides a simple user interface to execute multiple functions via the scripting interfaces of AstroPlanner and Stellarium.</p>
<p>This is a work in progress!</p>

<b>Quick Observation</b>: Displays a dialog allowing an observation to be added to one or more selected objects in a plan or to all objects associated with an object in the plan.

<b>Stellarium Sync</b>: This moves the Stellarium view to the selected object in AstroPlanner. The size (or zoom) of Stellarium’s field of view is not altered. 

<b>SharpCapDSA</b>: Select an object in AstroPlanner and pressing this button will place that objects SharpCap DSA format onto the clipboard. If the AstroPlanner object is a comet or asteroid, then the exact coordinates will be retrieved from JPL. Paste the entries into SharpCap, using SharpCap’s Deep Sky Annotation dialog.

<b>Add to AP</b>: This adds the currently selected object in Stellarium into the AstroPlanner plan. I have noticed it can take time for the objects ID to appear in AstroPlanner. Usually, clicking onto another object and then clicking outside AstroPlanner forces a refresh. I’m afraid this is an AstroPlanner problem. 

<b>Configuration</b>: The IP and port of AstroPlanner's Web Services interface. The default is the localhost on port 8080. for increased security an Authentication string (max 31 characters) can be specified in AstroPlanner and in the EAACP. The string in both applications much match or any calls will fail.

The application runs on top of all other applications. So, it should be hard to lose 😊
I have assumed that this is running on your primary monitor. I hope that is OK. It will remember its location and size on the primary screen. It is also possible to resize the dialog just like any other window. There are some constraints on minimum and maximum size. Hopefully, this allows easy placement of the dialog on your screen and allows you to see the buttons you require.


<b>INSTALLATION</b>

In AstroPlanner please create a new script called “EAACP”. It must be that name. Then copy the new script from the EAACP.txt file and paste it over the top of the newly created script. You don’t need to assign a control key or display it in the script menu as it will not function that way.

The EAACP folder needs to be copied to a location on your system (Desktop, Documents etc). 
To run the application, double-click on the EAACP.exe. The following may happen.

Microsoft will almost certainly want to scan the application first to ensure that it is OK. 
If you do not have the .NET framework runtime installed, then I believe Microsoft will prompt you to download it from their website. 
The application is using the 4.8 .NET framework runtime, available to download here <a href="https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-web-installer">Download .NET Framework 4.8 Web Installer (microsoft.com)</a> 

In AstroPlanner, please click on the menu item Edit->Preferences. On the dialog that appears click on the Web tab.

Tick the two marked checkboxes. The Port should default to 8080. I would also check the ‘Log all errors’ checkbox and finally click on the ‘Start Listening’ button. An Authentication string can be specified in this dialog and in the EAACP Configuration dialog. If any changes are made to the AstroPlanner dialog then please Stop Listening and Start Listening for the changes to come into effect.

To test it is working click on the link ‘Default page: http://localhost:8080’. Your browser should display AstroPlanner’s default web page.

