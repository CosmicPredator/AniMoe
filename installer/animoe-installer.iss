[Setup]
PrivilegesRequired=admin
AppPublisher=Cosmic Predator
AppPublisherURL=https://github.com/CosmicPredator/AniMoe
AppName=AniMoe
AppVersion=1.0
DefaultDirName={tmp}\AniMoe
DefaultGroupName=AniMoe
Uninstallable=no
DisableDirPage=yes

[Files]
Source: "{tmp}\cert.cer"; DestDir:{app}; Flags: external;
Source: "{tmp}\bundle.msix"; DestDir:{app}; Flags: external;

[Run]
Filename: "certutil.exe"; Parameters: "-addstore ""Root"" {tmp}\cert.cer"; Flags: runhidden; \
  StatusMsg: "Adding trusted root publisher..." 

Filename: "powershell.exe"; Parameters: "Add-AppPackage -path {tmp}\bundle.msix"; Flags: runhidden; \
  StatusMsg: "Installing AniMoe..."


[Code]
var
  DownloadPage: TDownloadWizardPage;

function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if Progress = ProgressMax then
    Log(Format('Successfully downloaded file to {tmp}: %s', [FileName]));
  Result := True;
end;

procedure InitializeWizard;
begin
  DownloadPage := CreateDownloadPage(SetupMessage(msgWizardPreparing), SetupMessage(msgPreparingDesc), @OnDownloadProgress);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  if CurPageID = wpReady then begin
    DownloadPage.Clear;
    // Use AddEx to specify a username and password
    DownloadPage.Add('https://github.com/CosmicPredator/AniMoe/releases/latest/download/AniMoe_Certificate.cer', 'cert.cer', '');
    DownloadPage.Add('https://github.com/CosmicPredator/AniMoe/releases/latest/download/AniMoe.App.msix', 'bundle.msix', '');
    DownloadPage.Show;
    try
      try
        DownloadPage.Download; // This downloads the files to {tmp}
        Result := True;
      except
        if DownloadPage.AbortedByUser then
          Log('Aborted by user.')
        else
          SuppressibleMsgBox(AddPeriod(GetExceptionMessage), mbCriticalError, MB_OK, IDOK);
        Result := False;
      end;
    finally
      DownloadPage.Hide;
    end;
  end else
    Result := True;
end;