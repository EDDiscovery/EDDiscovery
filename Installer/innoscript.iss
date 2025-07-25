; EDD script

#define MyAppName "EDDiscovery"
#ifndef MyAppVersion
#define MyAppVersion "12.1.710"
#endif
#define MyAppPublisher "Robby & EDDiscovery Team"
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

[Files]
Source: "EDDiscovery.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "EDDiscovery.exe.config"; DestDir: "{app}"; Flags: ignoreversion

Source: "x64\*.*"; DestDir: "{app}\x64"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "x86\*.*"; DestDir: "{app}\x86"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "runtimes\win-x64\*.*"; DestDir: "{app}\runtimes\win-x64"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "runtimes\win-x86\*.*"; DestDir: "{app}\runtimes\win-x86"; Flags: ignoreversion recursesubdirs createallsubdirs replacesameversion
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion;
Source: "eddwebsite.zip"; DestDir: "{app}"; Flags: ignoreversion;
Source: "defaultactfiles.zip"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\Translations\*.tlf"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\UserControls\Translations\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\EliteDangerousCore\EliteDangerous\Translations\*.tlp"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\..\..\Installer\ExtraFiles\EUROCAPS.TTF"; DestDir: "{app}"; Flags: ignoreversion;
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
var DataDirIndex: Integer;
var InstallChangeAppLocation : TButton;
var SettoAppData: TButton;

// callbacks for the data dir page

procedure OnClickSetChangeDataFolder(Sender: TObject);
var installunderpf:Boolean;
begin
  if ( DataDirPage.Values[0] = 'c:\' ) then begin // if we have a c:\ default marker in there, we are installing to PFs, but they want a custom location, so pick a default one
      DataDirPage.Values[0] := 'c:\EDDiscoveryData';
  end;

  DataDirPage.SubCaptionLabel.Caption := 'Installing under ' + WizardDirValue + #13#10#13#10 + 'Select location:';

  SettoAppData.Visible := false;
  InstallChangeAppLocation.Visible := false;
  DataDirPage.Edits[DataDirIndex].Visible := true;
  DataDirPage.Buttons[DataDirIndex].Visible := true;
end;

procedure OnClickSettoAppData(Sender: TObject);
begin
  DataDirPage.Values[0] := 'c:\'

  DataDirPage.SubCaptionLabel.Caption := 'Installing under ' + WizardDirValue + #13#10#13#10 + 'User Data will be in c:\Users\<user>\AppData\Local\EDDiscovery';

end;



// On initialisation, we set up a new data dir page with two buttons

procedure InitializeWizard;
begin
  // make a new page
  DataDirPage := CreateInputDirPage(wpSelectDir,
    'Select Personal Data Directory', 'Where should personal data files be installed?',
    'L1' + #13#10#13#10 + 'L2' + #13#10,    // text here is generated changed by code
    False, '');

  DataDirIndex := DataDirPage.Add('');
  DataDirPage.Edits[DataDirIndex].Visible := false;
  DataDirPage.Buttons[DataDirIndex].Visible := false;

  DataDirPage.Values[0] := 'c:\';    // c:\ means default appdata

  InstallChangeAppLocation:=TButton.Create(DataDirPage);
  InstallChangeAppLocation.Parent := DataDirPage.Buttons[0].Parent;
  InstallChangeAppLocation.Top := DataDirPage.Edits[DataDirIndex].Top;
  InstallChangeAppLocation.Left := 0;
  InstallChangeAppLocation.Width := 200;
  InstallChangeAppLocation.Caption := 'Change Location';
  InstallChangeAppLocation.OnClick := @OnClickSetChangeDataFolder;

  SettoAppData:=TButton.Create(DataDirPage);
  SettoAppData.Parent := DataDirPage.Buttons[0].Parent;
  SettoAppData.Top := DataDirPage.Edits[DataDirIndex].Top;
  SettoAppData.Left := 250;
  SettoAppData.Width := 200;
  SettoAppData.Caption := 'Use Local App Data';
  SettoAppData.OnClick := @OnClickSettoAppData;

end;

// on next on any page, do work

function NextButtonClick(CurPageID: Integer): Boolean;
var installunderpf:Boolean;
var prevdatafolder:String;
begin
  Result:=True;
  installunderpf := WildCardMatch(WizardDirValue,'*Program Files*');

  if ( CurPageID = wpSelectDir ) Then begin

  // PFs install use c:\ marker to indicate %localappdata%\EDDiscovery

    if ( installunderpf ) then begin
        DataDirPage.Values[0] := 'c:\'; // signal default loc
        DataDirPage.SubCaptionLabel.Caption := 'Installing under ' + WizardDirValue + #13#10#13#10 + 'User Data will be in c:\Users\<user>\AppData\Local\EDDiscovery';
        end
    else begin
      prevdatafolder := GetPreviousData('DataDir','');
      Log('Prev Data Folder computed as ' + PrevDataFolder );

      if ( prevdatafolder <> '' ) And (prevdatafolder <> 'c:\' ) then begin   // if a good one, use it
        DataDirPage.Values[0] := prevdatafolder;
      end
      else begin
        DataDirPage.Values[0] := WizardDirValue + '\Data';
      end;

      DataDirPage.SubCaptionLabel.Caption := 'Installing under ' + WizardDirValue + #13#10#13#10 + 'User Data will be in ' + DataDirPage.Values[0];
    end;

  end

  // after data page, just check we do not have a silly situation
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
  // fix c:\ marker so it makes more sense to display
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
    // if user data location is not c:\, we need to write a redirect option file
    if ( DataDirPage.Values[0] <> 'c:\') Then begin
        Log('** Appdata is ' + DataDirPage.Values[0] + ' need to write options file to ' + WizardDirValue);
        SaveStringToFile(WizardDirValue+'\optionsappdata.txt', '-appfolder ' + DataDirPage.Values[0] + #13#10, false);
    end else  begin
      Log('** Appdata is ' + DataDirPage.Values[0] + ' write null option file to overwrite previous one if present');
        SaveStringToFile(WizardDirValue+'\optionsappdata.txt', '-null' + #13#10, false);
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

// Help:
// this thing is written in Dephi.
// To find class heirachy, best to look at say the CreateInputDirPage return class (TInputDirWizardPage)
// Then look it up, you can see its properties.
// and you can follow it up the class tree to find more properties and call backs
// TButtons for instance, has a TButtonControl parent class, TWinControl, to TControl which has left/top etc.


