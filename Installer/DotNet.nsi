; Usage
; 1 Call SetupDotNetSectionIfNeeded from .onInit function
;   This function will check if the required version 
;   or higher version of the .NETFramework is installed.
;   If .NET is NOT installed the section which installs dotnetfx is selected.
;   If .NET is installed the section which installs dotnetfx is unselected.
 
!define SF_USELECTED  0
#!define SF_SELECTED   1
#!define SF_SECGRP     2
#!define SF_BOLD       8
#!define SF_RO         16
#!define SF_EXPAND     32
###############################
 
!define DOT_MAJOR 4
!define DOT_MINOR 0
!include MUI2.nsh
!macro SecSelect SecId
  Push $0
  IntOp $0 ${SF_SELECTED} | ${SF_RO}
  SectionSetFlags ${SecId} $0
  SectionSetInstTypes ${SecId} 7
  Pop $0
!macroend
 
!define SelectSection '!insertmacro SecSelect'
#################################
 
!macro SecUnSelect SecId
  Push $0
  IntOp $0 ${SF_USELECTED} | ${SF_RO}
  SectionSetFlags ${SecId} $0
  SectionSetText  ${SecId} ""
  SectionSetInstTypes ${SecId} 0
  Pop $0
!macroend
 
!define UnSelectSection '!insertmacro SecUnSelect'
###################################
!macro SecUnSelect2 SecId
  Push $0
  IntOp $0 ${SF_USELECTED} | ${SF_RO}
  SectionSetFlags ${SecId} $0
  SectionSetInstTypes ${SecId} 0
  Pop $0
!macroend
 
!define UnSelectSection2 '!insertmacro SecUnSelect2'
###################################
 
!macro SecExtract SecId
  Push $0
  IntOp $0 ${SF_USELECTED} | ${SF_RO}
  SectionSetFlags ${SecId} $0
  SectionSetInstTypes ${SecId} 2
  Pop $0
!macroend
 
!define SetSectionExtract '!insertmacro SecExtract'
###################################
 
!macro Groups GroupId
  Push $0
  SectionGetFlags ${GroupId} $0
  IntOp $0 $0 | ${SF_RO}
  IntOp $0 $0 ^ ${SF_BOLD}
  IntOp $0 $0 ^ ${SF_EXPAND}
  SectionSetFlags ${GroupId} $0
  Pop $0
!macroend
 
!define SetSectionGroup "!insertmacro Groups"
####################################
 
!macro GroupRO GroupId
  Push $0
  IntOp $0 ${SF_SECGRP} | ${SF_RO}
  SectionSetFlags ${GroupId} $0
  Pop $0
!macroend
 
!define MakeGroupReadOnly '!insertmacro GroupRO'
 
Function SetupDotNetSectionIfNeeded
 
  StrCpy $0 "0"
  StrCpy $1 "SOFTWARE\Microsoft\.NETFramework" ;registry entry to look in.
  StrCpy $2 0
 
  StartEnum:
    ;Enumerate the versions installed.
    EnumRegKey $3 HKLM "$1\policy" $2
 
    ;If we don't find any versions installed, it's not here.
    StrCmp $3 "" noDotNet notEmpty
 
    ;We found something.
    notEmpty:
      ;Find out if the RegKey starts with 'v'.  
      ;If it doesn't, goto the next key.
      StrCpy $4 $3 1 0
      StrCmp $4 "v" +1 goNext
      StrCpy $4 $3 1 1
 
      ;It starts with 'v'.  Now check to see how the installed major version
      ;relates to our required major version.
      ;If it's equal check the minor version, if it's greater, 
      ;we found a good RegKey.
      IntCmp $4 ${DOT_MAJOR} +1 goNext yesDotNetReg
      ;Check the minor version.  If it's equal or greater to our requested 
      ;version then we're good.
      StrCpy $4 $3 1 3
      IntCmp $4 ${DOT_MINOR} yesDotNetReg goNext yesDotNetReg
 
    goNext:
      ;Go to the next RegKey.
      IntOp $2 $2 + 1
      goto StartEnum
 
  yesDotNetReg:
    ;Now that we've found a good RegKey, let's make sure it's actually
    ;installed by getting the install path and checking to see if the 
    ;mscorlib.dll exists.
    EnumRegValue $2 HKLM "$1\policy\$3" 0
    ;$2 should equal whatever comes after the major and minor versions 
    ;(ie, v1.1.4322)
    StrCmp $2 "" noDotNet
    ReadRegStr $4 HKLM $1 "InstallRoot"
    ;Hopefully the install root isn't empty.
    StrCmp $4 "" noDotNet
    ;build the actuall directory path to mscorlib.dll.
    StrCpy $4 "$4$3.$2\mscorlib.dll"
    IfFileExists $4 yesDotNet noDotNet
 
  noDotNet:
    ${SelectSection} ${SECDOTNET}
    goto done
 
  yesDotNet:
    ;Everything checks out.  Go on with the rest of the installation.
    ${UnSelectSection} ${SECDOTNET}
    goto done
 
  done:
    ;All done.
 
FunctionEnd
 
!define BASE_URL http://download.microsoft.com/download
; .NET Framework
; English
!define URL_DOTNET_1033 "${BASE_URL}/a/a/c/aac39226-8825-44ce-90e3-bf8203e74006/dotnetfx.exe"
;German
!define URL_DOTNET_1031 "${BASE_URL}/4/f/3/4f3ac857-e063-45d0-9835-83894f20e808/dotnetfx.exe"
;Spanish
!define URL_DOTNET_1034 "${BASE_URL}/8/f/0/8f023ff4-2dc1-4f10-9618-333f5b9f8040/dotnetfx.exe"
;French
!define URL_DOTNET_1036 "${BASE_URL}/e/d/a/eda9d4ea-8ec9-4431-8efa-75391fb91421/dotnetfx.exe"
;Portuguese (Brazil)
!define URL_DOTNET_1046 "${BASE_URL}/8/c/f/8cf55d0c-235e-4062-933c-64ffdf7e7043/dotnetfx.exe"
;Chinese (Simplified)
!define URL_DOTNET_2052 "${BASE_URL}/7/b/9/7b90644d-1af0-42b9-b76d-a2770319a568/dotnetfx.exe"
!define URL_DOTNET_4100 "${BASE_URL}/7/b/9/7b90644d-1af0-42b9-b76d-a2770319a568/dotnetfx.exe"
;Chinese (Traditional)
!define URL_DOTNET_1028 "${BASE_URL}/8/2/7/827bb1ef-f5e1-4464-9788-40ef682930fd/dotnetfx.exe"
!define URL_DOTNET_3076 "${BASE_URL}/8/2/7/827bb1ef-f5e1-4464-9788-40ef682930fd/dotnetfx.exe"
!define URL_DOTNET_5124 "${BASE_URL}/8/2/7/827bb1ef-f5e1-4464-9788-40ef682930fd/dotnetfx.exe"
;Czech
!define URL_DOTNET_1029 "${BASE_URL}/2/a/2/2a224db0-2e6d-4961-99ed-6f377555b1ef/dotnetfx.exe"
;Danish
!define URL_DOTNET_1030 "${BASE_URL}/e/7/5/e755a559-025d-4282-95ae-d14a8d0b1929/dotnetfx.exe"
;Dutch
!define URL_DOTNET_1043 "${BASE_URL}/4/6/b/46b519cb-bdd2-4701-b962-9ffaa323f40b/dotnetfx.exe"
!define URL_DOTNET_2067 "${BASE_URL}/4/6/b/46b519cb-bdd2-4701-b962-9ffaa323f40b/dotnetfx.exe"
;Finnish
!define URL_DOTNET_1035 "${BASE_URL}/d/a/6/da6b472c-157c-429a-98f6-6eb87fa36fd3/dotnetfx.exe"
;Greek
!define URL_DOTNET_1032 "${BASE_URL}/5/9/8/598fb686-cd09-45cd-8b13-2a0fd814e4cc/dotnetfx.exe"
;Hungarian
!define URL_DOTNET_1038 "${BASE_URL}/8/2/0/82093ba7-c9a4-457d-864d-bbeb1cd884d4/dotnetfx.exe"
;Italian
!define URL_DOTNET_1040 "${BASE_URL}/1/f/a/1fa816d7-a8d6-4f15-b682-b96239e68ab7/dotnetfx.exe"
!define URL_DOTNET_2064 "${BASE_URL}/1/f/a/1fa816d7-a8d6-4f15-b682-b96239e68ab7/dotnetfx.exe"
;Japanese
!define URL_DOTNET_1041 "${BASE_URL}/5/b/5/5b510096-5b68-4e3f-8f9e-56fb7a80ca81/dotnetfx.exe"
;Korean
!define URL_DOTNET_1042 "${BASE_URL}/d/2/d/d2db6a60-6fb1-4015-ae45-2fb84ec30faa/dotnetfx.exe"
;Norwegian
!define URL_DOTNET_1044 "${BASE_URL}/b/6/6/b663aaf1-ef27-4119-8cf1-fa23888cf6a7/dotnetfx.exe"
!define URL_DOTNET_2068 "${BASE_URL}/b/6/6/b663aaf1-ef27-4119-8cf1-fa23888cf6a7/dotnetfx.exe"
;Polish
!define URL_DOTNET_1045 "${BASE_URL}/c/9/f/c9f672f3-c14b-4cff-9671-d419842d792d/dotnetfx.exe"
;Portuguese (Portugal)
!define URL_DOTNET_2070 "${BASE_URL}/1/2/0/1206b231-b961-40ca-9ac2-e4ab7e92ca9b/dotnetfx.exe"
;Russian
!define URL_DOTNET_1049 "${BASE_URL}/0/8/6/086e7824-ddad-45c0-b765-721e5e28e4c5/dotnetfx.exe"
;Swedish
!define URL_DOTNET_1053 "${BASE_URL}/3/0/0/300b9c1c-9a26-4334-b273-8c0064ba5f2b/dotnetfx.exe"
;Turkish
!define URL_DOTNET_1055 "${BASE_URL}/a/f/7/af738ebf-dc15-4c61-a20d-1c29306cd9bc/dotnetfx.exe"
; ... If you need one not listed above you will have to visit the Microsoft Download site,
; select the language you are after and scan the page source to obtain the link.
 
 
Var "LANGUAGE_DLL_TITLE"
Var "LANGUAGE_DLL_INFO"
Var "URL_DOTNET"
Var "OSLANGUAGE"
Var "DOTNET_RETURN_CODE"
 
 
LangString DESC_REMAINING ${LANG_ENGLISH} " (%d %s%s remaining)"
LangString DESC_PROGRESS ${LANG_ENGLISH} "%d.%01dkB/s" ;"%dkB (%d%%) of %dkB @ %d.%01dkB/s"
LangString DESC_PLURAL ${LANG_ENGLISH} "s"
LangString DESC_HOUR ${LANG_ENGLISH} "hour"
LangString DESC_MINUTE ${LANG_ENGLISH} "minute"
LangString DESC_SECOND ${LANG_ENGLISH} "second"
LangString DESC_CONNECTING ${LANG_ENGLISH} "Connecting..."
LangString DESC_DOWNLOADING ${LANG_ENGLISH} "Downloading %s"
LangString DESC_SHORTDOTNET ${LANG_ENGLISH} "Microsoft .Net Framework 4.0"
LangString DESC_LONGDOTNET ${LANG_ENGLISH} "Microsoft .Net Framework 4.0"
LangString DESC_DOTNET_DECISION ${LANG_ENGLISH} "$(DESC_SHORTDOTNET) is required.$\nIt is strongly \
  advised that you install$\n$(DESC_SHORTDOTNET) before continuing.$\nIf you choose to continue, \
  you will need to connect$\nto the internet before proceeding.$\nWould you like to continue with \
  the installation?"
LangString SEC_DOTNET ${LANG_ENGLISH} "$(DESC_SHORTDOTNET) "
LangString DESC_INSTALLING ${LANG_ENGLISH} "Installing"
LangString DESC_DOWNLOADING1 ${LANG_ENGLISH} "Downloading"
LangString DESC_DOWNLOADFAILED ${LANG_ENGLISH} "Download Failed:"
LangString ERROR_DOTNET_DUPLICATE_INSTANCE ${LANG_ENGLISH} "The $(DESC_SHORTDOTNET) Installer is \
  already running."
LangString ERROR_NOT_ADMINISTRATOR ${LANG_ENGLISH} "$(DESC_000022)"
LangString ERROR_INVALID_PLATFORM ${LANG_ENGLISH} "$(DESC_000023)"
LangString DESC_DOTNET_TIMEOUT ${LANG_ENGLISH} "The installation of the $(DESC_SHORTDOTNET) \
  has timed out."
LangString ERROR_DOTNET_INVALID_PATH ${LANG_ENGLISH} "The $(DESC_SHORTDOTNET) Installation$\n\
  was not found in the following location:$\n"
LangString ERROR_DOTNET_FATAL ${LANG_ENGLISH} "A fatal error occurred during the installation$\n\
  of the $(DESC_SHORTDOTNET)."
LangString FAILED_DOTNET_INSTALL ${LANG_ENGLISH} "The installation of $(PRODUCT_NAME) will$\n\
  continue. However, it may not function properly$\nuntil $(DESC_SHORTDOTNET)$\nis installed."
 
 
Section $(SEC_DOTNET) SECDOTNET
    SectionIn RO
    
    IfSilent lbl_IsSilent
    !define DOTNETFILESDIR "Common\Files\MSNET"
    StrCpy $DOTNET_RETURN_CODE "0"
!ifdef DOTNET_ONCD_1033
    StrCmp "$OSLANGUAGE" "1033" 0 lbl_Not1033
    SetOutPath "$PLUGINSDIR"
    file /r "${DOTNETFILESDIR}\dotnetfx1033.exe"
    DetailPrint "$(DESC_INSTALLING) $(DESC_SHORTDOTNET)..."
    Banner::show /NOUNLOAD "$(DESC_INSTALLING) $(DESC_SHORTDOTNET)..."
    nsExec::ExecToStack '"$PLUGINSDIR\dotnetfx1033.exe" /q /c:"install.exe /q"'
    pop $DOTNET_RETURN_CODE
    Banner::destroy
    SetRebootFlag true
    Goto lbl_NoDownloadRequired
    lbl_Not1033:
!endif
; Insert Other language blocks here
 
    ; the following Goto and Label is for consistencey.
    Goto lbl_DownloadRequired
    lbl_DownloadRequired:
    DetailPrint "$(DESC_DOWNLOADING1) $(DESC_SHORTDOTNET)..."
    MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "$(DESC_DOTNET_DECISION)" /SD IDNO \
      IDYES +2 IDNO 0
    Abort
    ; "Downloading Microsoft .Net Framework"
    AddSize 153600
    STRCPY $URL_DOTNET "http://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f1%2fB%2fE%2f1BE39E79-7E39-46A3-96FF-047F95396215%2fdotNetFx40_Full_setup.exe"
    nsisdl::download /TRANSLATE "$(DESC_DOWNLOADING)" "$(DESC_CONNECTING)" \
       "$(DESC_SECOND)" "$(DESC_MINUTE)" "$(DESC_HOUR)" "$(DESC_PLURAL)" \
       "$(DESC_PROGRESS)" "$(DESC_REMAINING)" \
       /TIMEOUT=30000 "$URL_DOTNET" "$PLUGINSDIR\dotnetfx.exe"
    Pop $0
    StrCmp "$0" "success" lbl_continue
    DetailPrint "$(DESC_DOWNLOADFAILED) $0"
    Abort
 
    lbl_continue:
      DetailPrint "$(DESC_INSTALLING) $(DESC_SHORTDOTNET)..."
      Banner::show /NOUNLOAD "$(DESC_INSTALLING) $(DESC_SHORTDOTNET)..."
      nsExec::ExecToStack '"$PLUGINSDIR\dotnetfx.exe" /q /c:"install.exe /q"'
      pop $DOTNET_RETURN_CODE
      Banner::destroy
      SetRebootFlag true
      ; silence the compiler
      Goto lbl_NoDownloadRequired
      lbl_NoDownloadRequired:
 
      ; obtain any error code and inform the user ($DOTNET_RETURN_CODE)
      ; If nsExec is unable to execute the process,
      ; it will return "error"
      ; If the process timed out it will return "timeout"
      ; else it will return the return code from the executed process.
      StrCmp "$DOTNET_RETURN_CODE" "" lbl_NoError
      StrCmp "$DOTNET_RETURN_CODE" "0" lbl_NoError
      StrCmp "$DOTNET_RETURN_CODE" "3010" lbl_NoError
      StrCmp "$DOTNET_RETURN_CODE" "8192" lbl_NoError
      StrCmp "$DOTNET_RETURN_CODE" "error" lbl_Error
      StrCmp "$DOTNET_RETURN_CODE" "timeout" lbl_TimeOut
      ; It's a .Net Error
      StrCmp "$DOTNET_RETURN_CODE" "4101" lbl_Error_DuplicateInstance
      StrCmp "$DOTNET_RETURN_CODE" "4097" lbl_Error_NotAdministrator
      StrCmp "$DOTNET_RETURN_CODE" "1633" lbl_Error_InvalidPlatform lbl_FatalError
      ; all others are fatal
 
    lbl_Error_DuplicateInstance:
    DetailPrint "$(ERROR_DOTNET_DUPLICATE_INSTANCE)"
    GoTo lbl_Done
 
    lbl_Error_NotAdministrator:
    DetailPrint "$(ERROR_NOT_ADMINISTRATOR)"
    GoTo lbl_Done
 
    lbl_Error_InvalidPlatform:
    DetailPrint "$(ERROR_INVALID_PLATFORM)"
    GoTo lbl_Done
 
    lbl_TimeOut:
    DetailPrint "$(DESC_DOTNET_TIMEOUT)"
    GoTo lbl_Done
 
    lbl_Error:
    DetailPrint "$(ERROR_DOTNET_INVALID_PATH)"
    GoTo lbl_Done
 
    lbl_FatalError:
    DetailPrint "$(ERROR_DOTNET_FATAL)[$DOTNET_RETURN_CODE]"
    GoTo lbl_Done
 
    lbl_Done:
    DetailPrint "$(FAILED_DOTNET_INSTALL)"
    lbl_NoError:
    lbl_IsSilent:
SectionEnd
 