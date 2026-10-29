
echo Extended Controls

cd ExtendedControls
rd /s  .vs
rd /s  ExtendedControls\obj
rd /s  ExtendedControls\bin
rd /s  ExtendedForms\obj
rd /s  ExtendedForms\bin
cd..
echo Finish Extended Controls

echo Action language
cd ActionLanguage
rd /s  ActionLanguage\obj
rd /s  ActionLanguages\bin
cd..
echo Finish Action Language

echo Base Utils
cd BaseUtilities
rd /s  .vs
rd /s  Audio\obj
rd /s  Audio\bin
rd /s  BaseUtilities\obj
rd /s  BaseUtilities\bin
rd /s  DirectInput\obj
rd /s  DirectInput\bin
rd /s  NetMQUtils\obj
rd /s  NetMQUtils\bin
rd /s  SQLLite\obj
rd /s  SQLLite\bin
cd..
echo Finish Base Utils

echo CAPI
cd CAPI
rd /s  CAPI\obj
rd /s  CAPI\bin
cd..
echo Finish Base Utils

echo Elite Dangerous core
cd EliteDangerousCore
rd /s  EliteDangerous\obj
rd /s  EliteDangerous\bin
cd..
echo Finish Elite Dangerous core

echo EDD
rd /s  .vs
cd EDDiscovery
rd /s  obj
rd /s  bin
cd..
echo Finish EDD

quit
