if "%1" == "" (
set /P vno=Version Number (10.1.2 etc) :
) else (
set vno=%1
)

REM you can do .. but it need the full path wmic datafile where Name="c:\\code\\EDDiscovery\\EDDiscovery\\bin\\Debug\\EDDiscovery.exe" get Version

find "%vno%" ..\eddiscovery\properties\AssemblyInfo.cs
if %ERRORLEVEL%==1 goto :error

echo Build %vno%
"\Program Files (x86)\Inno Setup 6\iscc.exe" /DMyAppVersion=%vno% innoscript.iss
copy ..\EDDiscovery\bin\Release\EDDiscovery.Portable.Zip installers\EDDiscovery.Portable.%vno%.zip
certutil -hashfile installers\EDDiscovery-%vno%.exe SHA256 >installers\checksums.%vno%.txt
certutil -hashfile installers\EDDiscovery.Portable.%vno%.zip SHA256 >>installers\checksums.%vno%.txt

exit /b

:error
echo Version %vno% does not correspond to AssemblyInfo.cs

