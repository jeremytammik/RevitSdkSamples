set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\Serialization\Serialization.dll" goto end
if not exist "%SOURCE_PATH%\Serialization\Serialization\bin\Debug\Serialization.dll" goto end

copy "%SOURCE_PATH%\Serialization\Serialization\bin\Debug\Serialization.dll" "%DESTINATION_PATH%\Serialization\Serialization.dll"
copy "%SOURCE_PATH%\Serialization\Serialization\bin\Debug\Serialization.pdb" "%DESTINATION_PATH%\Serialization\Serialization.pdb"

:end
