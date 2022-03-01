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
; LicenseFile="{#SourcePath}\..\EDDiscovery\Resources\EDD License.rtf"
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

[Files]
Source: "EDDiscovery.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "EDDiscovery.exe.config"; DestDir: "{app}"; Flags: ignoreversion

Source: "x64\*.*"; DestDir: "{app}\x64"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "x86\*.*"; DestDir: "{app}\x86"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion;
Source: "eddwebsite.zip"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\Translations\*.tlf"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\UserControls\Translations\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;                                             
Source: "..\..\..\EliteDangerousCore\EliteDangerous\EliteDangerous\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\Installer\ExtraFiles\EUROCAPS.TTF"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\EliteDangerousCore\EliteDangerous\JournalEvents\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent 

[Messages]
SelectDirBrowseLabel=To continue, click Next.
ConfirmUninstall=Are you sure you want to completely remove %1 executable files

[UninstallDelete]
Type: files; Name: "{app}\*.txt"

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

var DataDirPage: TInputDirWizardPage;
var InstallUnderProgramButton : TButton;

// callbacks for the data dir page

procedure OnClickSetUnderInstall(Sender: TObject);
begin
    DataDirPage.Values[0] := WizardDirValue + '\Data';
end;

procedure OnClickSetDefAppLocation(Sender: TObject);
begin
    DataDirPage.Values[0] := 'c:\';
end;

// On initialisation, we set up a new data dir page with two buttons

procedure InitializeWizard;
var
  but1: TButton;
begin
  // make a new page
  DataDirPage := CreateInputDirPage(wpSelectDir,
    'Select Personal Data Directory', 'Where should personal data files be installed?',
    'Select the folder in which Setup should install personal data files, ' +
      'then click Next.' + #13#10 +#13#10
    '!!! LEAVE AT c:\ for the default location (c:\Users\<user>\AppData\Local\EDDiscovery) !!!',
    False, '');
  DataDirPage.Add('');
  DataDirPage.Values[0] := ''; 

  // Button to set data page value to local app
  but1:=TButton.Create(DataDirPage);
  but1.Parent := DataDirPage.Buttons[0].Parent;
  but1.Top := 100;
  but1.Left := 0;
  but1.Width := 200;
  but1.Caption := 'Local App Location';
  but1.OnClick := @OnClickSetDefAppLocation;

  // and to set data location to under the install
  InstallUnderProgramButton:=TButton.Create(DataDirPage);
  InstallUnderProgramButton.Parent := DataDirPage.Buttons[0].Parent;
  InstallUnderProgramButton.Top := 140;
  InstallUnderProgramButton.Left := 0;
  InstallUnderProgramButton.Width := 200;
  InstallUnderProgramButton.Caption := 'Under install location';
  InstallUnderProgramButton.OnClick := @OnClickSetUnderInstall;
end;

// on next on any page, do work

function NextButtonClick(CurPageID: Integer): Boolean;
var DefInstallPath:String;
var AppPath:String;
var installunderpf:Boolean;
var PrevDataFolder:String;
begin
  Result:=True;

  DefInstallPath := ExpandConstant('{autopf}\EDDiscovery');
  Log('Default install computed as ' + DefInstallPath );

  PrevDataFolder := GetPreviousData('DataDir','');
  Log('Prev Data Folder computed as ' + PrevDataFolder );

  installunderpf := WildCardMatch(WizardDirValue,'*Program Files*');

  // if next from select dir page, next is data page, set it up
  if ( CurPageID = wpSelectDir ) Then begin
      InstallUnderProgramButton.Visible := Not(installunderpf);   // only visible if not installing under program files

      // select the default data folder based on history, and program file location

      if ( PrevDataFolder <> '' ) Then begin
        DataDirPage.Values[0] := PrevDataFolder;
      end else if ( not(installunderpf) ) Then begin
        DataDirPage.Values[0] := WizardDirValue + '\Data';
      end else begin
        DataDirPage.Values[0] := 'c:\';
      end;

    end
    // if exiting data page, check conditions
    else if ( CurPageID = DataDirPage.ID) Then begin
      if ( installunderpf And WildCardMatch(DataDirPage.Values[0],'*Program Files*')) Then begin
          MsgBox('Cannot store data files in program files location', mbError, mb_Ok);
          Result:=False;
        end;
    end;

end;

// called at end, to store data for installer. Store the data page selected

procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
  { Store the selected folder for further reinstall/upgrade }
  Log('Store Data path ' + DataDirPage.Values[0] );
  SetPreviousData(PreviousDataKey, 'DataDir', DataDirPage.Values[0]);
end;


// called when ready to install is displayed
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  if ( DataDirPage.Values[0] = 'c:\') Then begin
    S := 'c:\Users\<user>\AppData\Local\EDDiscovery';
  end else begin
    S := DataDirPage.Values[0];
  end;

  S := 'EDDiscovery will be installed into ' + NewLine + MemoDirInfo + NewLine + NewLine + 'User Data will be saved into ' + NewLine + Space + S + NewLine;
  
  //S := S + ExpandConstant('{username}') + NewLine;
  //S := S + ExpandConstant('{localappdata}') + NewLine;
  //S := S + ExpandConstant('{userappdata}') + NewLine;
  Result := S;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if( CurStep = ssPostInstall ) then begin
    if ( DataDirPage.Values[0] <> 'c:\') Then begin
        Log('** Appdata is ' + DataDirPage.Values[0] + ' need to write options file to ' + WizardDirValue);
        SaveStringToFile(WizardDirValue+'\optionsappdata.txt', '-appfolder ' + DataDirPage.Values[0] + #13#10, false);
    end else  begin
      Log('** Appdata is ' + DataDirPage.Values[0] + ' no need to write option file ');
    end;
    
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var 
  appdata: String;
begin
  appdata := GetPreviousData('DataDir','');
                                      
  if ( CurUninstallStep = usUninstall) then begin
    if ( appdata='c:\') then begin
      MsgBox('User data is stored in your c:\Users\<user>\AppData\Local\EDDiscovery folder.' + #13#10#13#10 +'If you want to remove the data, you need to manually delete it', mbConfirmation, MB_OK);
    end else if ((appdata<>'')) Then begin   
      if MsgBox('Also, user data is stored in ' + appdata + #13#10 + 'Do you want to delete all data files in this location?' + #13#10#13#10 + '!!!WARNING your user settings will be lost!!!', mbConfirmation, MB_YESNO) = IDYES then begin
          DelTree( appdata, true, true, true);
      end
    end
  end;
end;
