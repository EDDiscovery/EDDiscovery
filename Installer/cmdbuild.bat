echo off

rem cmd.exe is needed for this

SETLOCAL EnableExtensions DisableDelayedExpansion

rem you can choose Debug or Release now here
rem for Debug builds, you need to just tweak the EDDiscovery.csproj file to make it spit out the eddweb site, change Debug here to nonsense
rem   <Target Name="AfterBuild" Outputs="$(OutDir)EDDiscovery.Portable.zip" Condition=" '$(OS)' != 'Unix' And '$(Configuration)' != 'Debug' ">

set EXEROOT=Release
set exefolder=..\\EDDiscovery\\bin\\%EXEROOT%\\

rem trick to get absolute path

pushd %exefolder%
set absexefolder=%CD%
popd

rem using EnableExtensions, use the pattern replacer to double \\

set vno=%1

rem support having a prefix to the appname/guid so you can install two EDDs at the same time. Use cmd /c cmdbuild 19.1.200.0 DR

if "%2" == "" set altname=
if NOT "%2" == "" set altname=-%2

echo Want version `%vno`, Name `%EDDiscovery%altname%`
pause


if "%vno%"=="" goto :errorVER

if "%CAPIID%"=="" goto :errorCAPI

find "%vno%" ..\eddiscovery\properties\AssemblyInfo.cs
if %ERRORLEVEL%==1 goto :errorAI
echo Assembly passed

echo .
echo Building default act files into %exefolder%
del %exefolder%\defaultactfiles.zip >nul
powershell compress-archive -Path ..\..\EDDiscoveryData\ActionFiles\V1\*.* -DestinationPath %exefolder%\defaultactfiles.zip

echo.
echo Build %vno%%altname%

if "%altname%"==""      "\Program Files (x86)\Inno Setup 6\iscc.exe" /DMyAppVersion=%vno% /DReleaseFolder=%EXEROOT% innoscript.iss
if NOT "%altname%"==""  "\Program Files (x86)\Inno Setup 6\iscc.exe" /DMyAppVersion=%vno% /DMyAppName=EDDiscovery%altname% /DMyAppGUIDAux=%altname% innoscript.iss

rem program built

rem echo copy ..\EDDiscovery\bin\Release\EDDiscovery.Portable.Zip installers\EDDiscovery%altname%.Portable.%vno%.zip
copy ..\EDDiscovery\bin\%EXEROOT%\EDDiscovery.Portable.Zip installers\EDDiscovery%altname%.Portable.%vno%.zip

certutil -hashfile installers\EDDiscovery%altname%-%vno%.exe SHA256 >installers\checksums%altname%.%vno%.txt
certutil -hashfile installers\EDDiscovery%altname%.Portable.%vno%.zip SHA256 >>installers\checksums%altname%.%vno%.txt

explorer .\installers

exit /b

:errorAI
echo Version %vno% does not correspond to AssemblyInfo.cs or to the release exe
exit /b

:errorEXE
echo Version %vno% does not correspond to the release exe
type %TMP%\vno.txt
exit /b

:errorVER
echo Must give version on command line, example: build 16.1.1
exit /b


:errorCAPI
echo No CAPIID variable defined
exit /b

