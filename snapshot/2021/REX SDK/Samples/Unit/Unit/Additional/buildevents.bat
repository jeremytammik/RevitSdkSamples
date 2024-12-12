set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\Unit\Unit.dll" goto end
if not exist "%SOURCE_PATH%\Unit\Unit\bin\Debug\Unit.dll" goto end

copy "%SOURCE_PATH%\Unit\Unit\bin\Debug\Unit.dll" "%DESTINATION_PATH%\Unit\Unit.dll"
copy "%SOURCE_PATH%\Unit\Unit\bin\Debug\Unit.pdb" "%DESTINATION_PATH%\Unit\Unit.pdb"

:end
