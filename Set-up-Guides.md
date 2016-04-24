# Basic Set-up
Download and install the [latest version](https://github.com/EDDiscovery/EDDiscovery/wiki/Download-the-Application,-here!) - It's a typical 'next, next, finish' installer.

The first run of the application might take a little while as it will go off and download the maps that can later be used to overlay with your travels.  Just be a little a patient.  
![](http://i.imgur.com/D40THzU.png)  
  
**Recommendation:**  
Now quit EDDiscovery _and_ the game.  The reason for this is that it provides opportunity for EDD to set-up the verbose logging for you.

Re-open EDD and the Elite Dangerous game.

With luck, EDD will start with your current system showing in the travel log [if you previously had verbose logging enabled, then you will see a list of the systems yous have visited]:  
![](http://i.imgur.com/hLT21S5.png)

**Note:**  If your current system does not show it is likely that either the verbose logging is not enabled, or EDD was unable to automatically detect the location of the logs and will need to be set manually.

## Manually setting the log location  
Navigate to the "Settings" tab.  Select the 'Manual' radio button and then browse to the location of your log files.  
![](http://i.imgur.com/sYBdidl.png)  
  
A list of the possible log file locations may be found [on the Frontier Developments knowledgebase](https://support.frontier.co.uk/kb/faq.php?id=108)  

## Manually Enabling Verbose Logging
Verbose logging is enabled in the AppConfig.xml file - the location of this file may be found in the above link for log locations.  It should be noted that the AppConfig.xml file is replaced each time the game is updated and therefore any changes made are removed on each update.  

To avoid this problem, EDDiscovery will create AppConfigLocal.xml in the same folder as the AppConfig.xml.    The AppConfigLocal.xml remains untouched during the updates.  When the game starts, it checks for the existence of an AppConfigLocal.xml file and replaces any settings within the AppConfig.xml with the corresponding values in the AppConfigLocal.xml.  
  
Should you still wish to edit the AppConfig.xml, open the file in notepad and search for the network section.  It will look something like this:
> \<Network  
> Port="0"  
> upnpenabled="1"  
> LogFile="netLog"  
> DatestampLog="1"  
> \>  
> \</Network\>  

You need to add the _VerboseLogging="1"_ line so it looks like this:  
> \<Network  
> Port="0"  
> upnpenabled="1"  
> LogFile="netLog"  
> DatestampLog="1"  
> VerboseLogging="1"  
> \>  
> \</Network\> 
  
This set-up will have EDDiscovery tracking your flight logs.  Please note, that you will not be able to tri-laterate unknown system positions without an EDSM account (later in this guide).

# Renaming Screenshots  
EDDiscovery includes the ability to monitor the Elite Dangerous screenshot folder and rename those images to include the system name and/or convert the image to another format.  
Navigate to the 'Screenshots' tab  
Simply ticking the "Auto convert ED screenshots" box ought to be sufficient to enable the function  
![](http://i.imgur.com/nHzslpG.png)  
The folders should be self explanatory.  The 'Save As' drop down allows you to select a different format to convert the image to, and the "remove original image after conversion" will obviously delete the original file.  
  
Once enabled, taking screenshots will result in the images being renamed and/or converted as you have enabled.  E.g., instead of "Screenshot_0000.bmp" and "Screenshot_0001.bmp", one gets:  
![](http://i.imgur.com/NHitWNi.png)  
  
# Advanced Set-up  
## EDSM Integration
In order to save tri-lateration results back into EDSM, you need to have configured an API key.  
First head over to [http://www.edsm.net/](www.edsm.net) and create yourself a free account.  With that done, in EDSM go to the user drop down and select 'My API Key'  
![](http://i.imgur.com/6g8VGlZ.png)  
Now copy the numbers and and letters from the API Key  
![](http://i.imgur.com/VSwjmNh.png)  
  
And paste into the "EDSM api key" section of the Settings tab.  Also enter your Cmdr name.
![](http://i.imgur.com/9MTfxP1.png)  
That's it!  You should now be able to submit your tri-laterated distances back to EDSM as well and sync your travel logs back and forth.