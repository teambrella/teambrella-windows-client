IF [%1]==[] SET Config=Debug
ELSE SET Config=%1

Install-TeambrellaService.bat %Config% /u