CALL "%Vs140ComnTools%\..\Tools\VsDevCmd.bat"

IF [%1]==[] SET Config=Debug
ELSE SET Config=%1

InstallUtil .\bin\%Config%\TeambrellaService.exe %2