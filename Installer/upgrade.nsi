var IsUpgrade
var UpgradePath
function SetupUpgradeSectionIfNeeded

strcpy $0 ""
ReadRegStr $0 HKLM "SOFTWARE\$(^Name)" Path
StrCmp $0 "" try64bit installed

try64bit:
	ReadRegStr $0 HKLM "SOFTWARE\Wow6432Node\$(^Name)" Path
	StrCmp $0 "" end installed
	goto newinstall
installed:
	strcpy $IsUpgrade  "true"
	strcpy $UpgradePath $0
	strcpy $INSTDIR $UpgradePath
	
	goto end
newinstall:
	
end:
functionend 
