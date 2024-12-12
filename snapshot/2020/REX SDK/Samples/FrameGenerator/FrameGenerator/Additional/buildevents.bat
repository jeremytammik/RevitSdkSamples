set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\FrameGenerator\FrameGenerator.dll" goto end
if not exist "%SOURCE_PATH%\FrameGenerator\FrameGenerator\bin\Debug\FrameGenerator.dll" goto end

copy "%SOURCE_PATH%\FrameGenerator\FrameGenerator\bin\Debug\FrameGenerator.dll" "%DESTINATION_PATH%\FrameGenerator\FrameGenerator.dll"
copy "%SOURCE_PATH%\FrameGenerator\FrameGenerator\bin\Debug\FrameGenerator.pdb" "%DESTINATION_PATH%\FrameGenerator\FrameGenerator.pdb"

:end
