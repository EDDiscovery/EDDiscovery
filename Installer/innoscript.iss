; EDD script

#define MyAppName "EDDiscovery"
#ifndef MyAppVersion
#define MyAppVersion "11.4.505"
#endif
#define MyAppPublisher "EDDiscovery Team (Robby)"
#define MyAppURL "https://github.com/EDDiscovery"
#define MyAppExeName "EDDiscovery.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AllowUNCPath=no
AppId={{66D786F5-B09D-F1B4-6910-DE98F4475083}
AppName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppVerName={#MyAppName} {#MyAppVersion}
AppVersion={#MyAppVersion}
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=no
DisableWelcomePage=no
DirExistsWarning=auto
LicenseFile="{#SourcePath}\..\EDDiscovery\Resources\EDD License.rtf"
OutputBaseFilename={#MyAppName}-{#MyAppVersion}
OutputDir="{#SourcePath}\installers"
SolidCompression=yes
SourceDir="{#SourcePath}\..\EDDiscovery\bin\Release"
UninstallDisplayIcon={app}\{#MyAppExeName}
UsePreviousTasks=no
UsePreviousAppDir=yes

WizardImageFile="{#SourcePath}\Logo.bmp"
WizardSmallImageFile="{#SourcePath}\Logosmall.bmp"
WizardImageStretch=no
WizardStyle=modern
WizardSizePercent=150

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "EDDiscovery.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "EDDiscovery.exe.config"; DestDir: "{app}"; Flags: ignoreversion

Source: "x64\*.*"; DestDir: "{app}\x64"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "x86\*.*"; DestDir: "{app}\x86"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion;
Source: "eddwebsite.zip"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\Translations\*.tlf"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\UserControls\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\EliteDangerous\EliteDangerous\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\Installer\ExtraFiles\EUROCAPS.TTF"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\EliteDangerous\JournalEvents\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Messages]
SelectDirBrowseLabel=To continue, click Next.
ConfirmUninstall=Are you sure you want to completely remove %1 and all of its components? Note that all your user data is not removed by this uninstall and is still stored in your local app data

[Code]

const
  QuitMessageReboot = 'Detected an older version of EDDiscovery'#13#10'Please manually remove this via add/remove programs before running setup again';

function InitializeSetup(): Boolean;
var Path:String;
var EDD:String;
begin
  Path := ExpandConstant('{autopf}');
  Path := Path + '\EDDiscovery\EDDiscovery';  // where Advanced installer stuffed it
  EDD := Path + '\EDDiscovery.exe';          // and the file
  Log('Path computed as ' + Path + ' file ' + EDD);   // use /Log="c:\code\setup.log" in Run | Parameters, also appears in log window below
  if ( DirExists(Path) And FileExists(EDD)) Then begin
    MsgBox(QuitMessageReboot, mbError, mb_Ok);
  end else
      Result := True;

end;


