Release log: 

2016-04-13 Release 3.0.5

klightspeed: 3dMaps

Beeing able to show multiple maps on the Y=0 plane, and being able to filter by expedition or by date.
Add support for map textures to 3D map
Order maps by order clicked
Add ability to filter 3DMap visited systems by date
Camera now centers correctly on requested system again.
Improves the performance of the 3D map significantly by using Vertex Buffer Objects. This along with Vsync should give smooth movement using the keyboard.
Reduce the amount of CPU and GPU time being used when no movement is occurring, and stops the map being updated when the map window is not active, as requested in #60. This should also fix #161.

Set active commander when saving netlogs

Iainross;

Prevent error centring 3D map on nonexistent system
Merge remote-tracking branch 'blessed/master'
Prevent error clicking on header row of wanted systems list
Finwen:

Added a custom toolstrip renderer. To make theming of menues and toolsstrip (still some work to do)
Solved some exceptions usesers reported.
Added a button to show all stars on 2D Map.
Probably solved a expeption in 3D graph some users get then using a XBox controller.


2016-04-05 Release 3.0.4

Iainross:

3D Map is now centred properly and references lines drawn correctly when launched from the trilateration tab if default centre is set to Home System.
Added 3D Map button to Route tab, plotted route is drawn onto the map when launched from there.
Added 'Add to trilateration' context menu to Nearest Systems grid on Travel History control.
Added context menu to Distances grid on trilateration to copy systems to wanted systems list - local wanted systems are shown in the wanted systems grid, can be added to the distances grid by clicking and are persisted between sessions.
Added context menu to Wanted Systems grid on trilateration for deletion of locally added Wanted Systems.
Corrected check that systems are known on EDSM so it works again (site has started returning [] rather than -1 for bad system names)
Systems can be added to wanted from travel history

klightspeed: 3DMap updates
This adds the option for perspective rendering as an alternative to the current orthogonal rendering.
This also maps Right-drag to Y-axis translation, and Left+Right-drag to panning on the X and Z axes.
This adds the ability to see what star is at a given position in the 3D map, and to center the map on that star.
A single-click on a star will select the star, but not set it as the center system.
Double-clicking on a star will select the star, set it as the center system, and center the map on it.
It was suggested in #160 that when the centered system is changed, the camera angle should not change. This implements this, and also slews the camera in order to reduce confusion as to where the camera moved to.
This currently uses a sinusoidal velocity curve with a fixed 1 second slew duration regardless of distance or zoom. Clicking the mouse or pressing a movement button cancels the slew.

Finwen:
Fix rare exception in GetSystemsCount function
After 3d map was opened the close button failed to exit EDDiscovery


2016-03-23  Beta 3.0.0

* First version of Theme support. (Robby)
* Perform log file parsing using a regular expression Fixes  bug then ED logs  position incorrectly (Myshka)
* Prevent runtime error on deleting distance in trilateration. (Ian666)
* Check for actual current system when clicking the Trilaterate button (Ian666)
* Update to routing (Robby)
* Reduced memory usage for distances. (Finwen)

2016-03-13 Version 2.7.6
* Solve a DirectoryInfo exception then path is not valid.
* Get pushed systems from EDSM in a thread so UI do not need to wait for request..
* Show a messagebox if another EDDiscovery is running

2016-03-11  Version 2.7.5

* Changed EDSM sync option from Full/Push only to 2 checkboxes with ToEDSM/FromEDSM
* Shows Wanted systems from EDSM instead of Closestsystems for last known positions in tril tab.
* Support for DW4 map in 2DMaps
* Check so we only start one EDDiscovery (mwerle).
* Add Help -> About Box.
* Allot of improvments in Routing (Robby)
* Bug fix: with duplicate entries in Trilateration View (mwerle)
* Bug fix:  add additional checks for NULL system (mwerle)

* Lots of improvment in imageconversion from Robby 
    Rewrite of the Image conversion to fix issues picking
    Problem with deleting files which have been cropped.
    Added more output file formats
    Fixed it so it renames them _1, _2 if multiples exist
    Can delete if you crop.
    Have checked it for cropping, deleting, both, all formats, BMP warning

* Trilateration UI improvements (mwerle)
* Allow sending distances to systems if current system has known coordinates without trilaterating (mwerle).
* Automatically copy "Last System" to clipboard when control is activated (mwerle)

2016-02-19 Version 2.7.4 

* Removed systems from Proving ground
* Multicommander bug fix:  Forst commander did read EDSM api key from commander 2....


2016-02-17 Version 2.7.3 

Robby: Can write note directly in the travelhistorylist now!
Mwerle : Trilateration: Tab skip over readonly fields So pressing tab in distance field moves you to next system name.
Iain: Option to only send travelhistory to EDSM.
Iain: Bugfix with logfiles passing over midnight.
Mwerle: Option to croop image on conversion. 
Mwerle: Menu to copy selected systems from Travelhistory to trilateration.
Finwen: Better switching between multicommandes.
Finwen: Trilateration Tab to systemname copes name to clipboard (no need to do CTRL+C)


2016-01-03 Version 2.6.2

3DMap 
* 3DMap now open maximized
* Created keyboard bindings (WASDG) to travel around in the galaxy.
* Can transform the camera in all directions and rotations
* Statusbar with information about position and more.
* Grid

2DMap
* Support for the new map of the whole galaxy made by Finwen and Marlon Blake


2016-12-28 Version 2.6.1

3dMap
*Show All known stars (on/off)
*Show all known stations (On/off)
EDDB:
*Bug: did not update system with population information from EDDB.
*Added in Admin menu a item to Force EDDB update.
EDSM
*Optional logging of EDSM communication


2015-11-04 Version 2.5.2
* Stores Travel log in local DB. 
* Can sync Travellog with EDSM and sync log with several computers. 
* Trilateration window enchanced. Copy systemname to clipboard on click. (No need to CTRL+C anymore.)
3D map in trilateration window shows lines to reference stars. 
Added icons to Remove unused references and to remove all. 


2015-09-04 Version 2.4.8
* Support for 2D maps made by Corbin Moran
* Autoconvert ED screenshots to png or jpeg and renames them to Systemname with time. 
* Better integration with EDSM
* Trilateration is now better on suggesting good reference stars



2015-08-04 Version 2.3.7

* Moved trilateration to an own tab. 
* Trilateration has a 3d star map button that show reference systems and suggested references- 
* Star map works again
* Travel history age selection will remember the selection to next usage. And we added last 20 systems as option. 
* EDStarCoordinator homepage has been down allot last days so we now have support for EDSM also. 
* Moved status login to bottom of window.
* Can select installation directory


2015-07-10 Version 2.3.5

* The search box now moves when the window is resized
* New alternate installer instead of the auto updating click once installer. Also checks if new installer is available. 
* 3d map did show government instead of normal plotting...


2015-07-10 Version 2.3.4

* Search in travel history. Search works on all fields. So you can search for a system name or something in your notes. 
* Downloaded json files from EDDB and EDSC is now stored in appdata instead of current directory. This is for the the future (next version) alternative installer that will a more regular installer instead of the current auto updating clickonce installer. Both installers will be available. 
* EDDiscovery should not xrach if netlogmain has invalid path

2015-06-26 Version 2.3.0 

Together with Cmdr Majkl we have created allot of new things in this version. 

* Trilateration routine to get coordinates for unknown stars. 
* Eddiscovery also creates an AppConfigLocal.xml file to automatically to add verbose logging in ED. 
* EDDiscovery starts faster with more work running in different threads. 
* Easy way to add distance to next system. 
* And a long list of other small bugfixes and small changes.






2016-06-18 Version 2.2.0

* Updates UI better during startuo.
* Travel history list is allot faster.
* EDDB integration. Get data about systems from http://EDDB.io Added 2 buttons to view a system in eddb or edit information i Ross.


2015-06-14 Version 2.1.27

* Allow regions with , as decimal separator to also use . as decimal separator
* Test button removed.. 
* Added more time span options (6 hrs, 12 hrs, 3 days, 2 weeks) - Thanks Majkl

2015-06-12 Version 2.1.26

* Much smaller installation package. Instead of bundling corrected TGC data from Redwizzard EDDiscovery now checks if a new version exists on my mirrored server and then download it on demand. 



2015-06-10 Version 2.1.25
* Shows how many visits you have done to a system. 
* Added some statistics Tolls menu and Statistics open up a form with some statistics. Will add more later.



2015-05-07 Version 2.1.22

* Refresh in travel history now gets new systems from EDSC.
* New corrected star database from Redwizzard.

2015-05-05 Version 2.1.21

* I had inverted Z axis in the 3d map. Should be correct now.
* Now shows coordinates for a system with full precision. 



2014-04-24 Version 2.1.18

Only some small changes in this update

* Getting distances from EDSC in a thread -> EDDiscovery stars allot faster
* Added a menu. With links to forum threads and quick ways to open Elite Dangerous directory and the log file directory.