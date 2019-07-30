
To debug, you can use two visual studio instances

In the first, run EDD normally in debug mode.
	Use the command line -wsf c:\<code>\eddiscovery\website\edd
	The option tells EDD not to search for the default eddwebsite.zip (located in the exe folder) to serve from,
	but to serve from the raw files in this folder.
	
	On deployment, the release has the .zip file with the website which is the normal way to serve. For debugging, we
	don't do that.
	
	The main code is in webserver.cs, serving the HTML pages from the zip, the ICONS from the icon packs, and
	hosts a websocket to provide and push journal and indicator status

Second, you can open the c:\<code>\eddiscovery\website\eddweb.sln file in the second VS instance. 

This sets up a project in VS to allow you to debug the website and its javascript.  

In the property page of the EDD website (right click to access), in start options, set the start URL to http://localhost:6502

Run the website in VS using the ISSExpress(Google Chrome) tool bar option - you can configure this button to google chrome
using the drop down arrow next to it. The browser must be google chrome (firefox does not support this) and the script debugging
must be on.

You can use breakpoints, see the console.log output, all in VS, see variables - full visibility.

Don't forget shift=ctrl-I in chrome to look at the CSS and elements directly in the webpage.

