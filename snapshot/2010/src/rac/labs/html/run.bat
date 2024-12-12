@echo off
if %1.==-d. goto delete

net share | find "html" > nul
if not errorlevel 1 goto share_exists
net share html=C:\a\j\adn\revit\rac_api\Labs\html /users:1 /r:"This folder is temporarily shared."

:share_exists
set a=%1
if .%a%==. set a=index.htm
start \\%computername%\html\%a%
goto exit

:delete
net share html /d

:exit
