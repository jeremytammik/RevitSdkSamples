set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\ElementReportHTML\ElementReportHTML.dll" goto end
if not exist "%SOURCE_PATH%\ElementReportHTML\ElementReportHTML\bin\Debug\ElementReportHTML.dll" goto end

copy "%SOURCE_PATH%\ElementReportHTML\ElementReportHTML\bin\Debug\ElementReportHTML.dll" "%DESTINATION_PATH%\ElementReportHTML\ElementReportHTML.dll"
copy "%SOURCE_PATH%\ElementReportHTML\ElementReportHTML\bin\Debug\ElementReportHTML.pdb" "%DESTINATION_PATH%\ElementReportHTML\ElementReportHTML.pdb"

:end
