set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\ContentGeneratorWPF\ContentGeneratorWPF.dll" goto end
if not exist "%SOURCE_PATH%\ContentGeneratorWPF\ContentGeneratorWPF\bin\Debug\ContentGeneratorWPF.dll" goto end

copy "%SOURCE_PATH%\ContentGeneratorWPF\ContentGeneratorWPF\bin\Debug\ContentGeneratorWPF.dll" "%DESTINATION_PATH%\ContentGeneratorWPF\ContentGeneratorWPF.dll"
copy "%SOURCE_PATH%\ContentGeneratorWPF\ContentGeneratorWPF\bin\Debug\ContentGeneratorWPF.pdb" "%DESTINATION_PATH%\ContentGeneratorWPF\ContentGeneratorWPF.pdb"

:end
