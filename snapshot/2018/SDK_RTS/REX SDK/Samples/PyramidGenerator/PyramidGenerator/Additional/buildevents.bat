set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\PyramidGenerator\PyramidGenerator.dll" goto end
if not exist "%SOURCE_PATH%\PyramidGenerator\PyramidGenerator\bin\Debug\PyramidGenerator.dll" goto end

copy "%SOURCE_PATH%\PyramidGenerator\PyramidGenerator\bin\Debug\PyramidGenerator.dll" "%DESTINATION_PATH%\PyramidGenerator\PyramidGenerator.dll"
copy "%SOURCE_PATH%\PyramidGenerator\PyramidGenerator\bin\Debug\PyramidGenerator.pdb" "%DESTINATION_PATH%\PyramidGenerator\PyramidGenerator.pdb"

:end
