set SOURCE_PATH=
set DESTINATION_PATH=

if not exist "%DESTINATION_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing.dll" goto end
if not exist "%SOURCE_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing\bin\Debug\DRevitFreezeDrawing.dll" goto end

copy "%SOURCE_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing\bin\Debug\DRevitFreezeDrawing.dll" "%DESTINATION_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing.dll"
copy "%SOURCE_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing\bin\Debug\DRevitFreezeDrawing.pdb" "%DESTINATION_PATH%\DRevitFreezeDrawing\DRevitFreezeDrawing.pdb"

:end
