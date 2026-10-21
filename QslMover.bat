@ECHO OFF

rem		-SourceFolder "%SRC%" は、ソースフォルダです。SRCで指定してください。
rem		-DestinationFolder "%DST%"は、送り先フォルダです。DSTで指定してください。
rem		-FolderMode Qは、送り先フォルダの構造です。”Q"は、QslOrganizerの構造、”H”は、HAMLOGの構造です。
rem		-RenameOutputは、ファイルをリネーム指定です。指定がない場合、ファイル名は、そのまま出力されます。

set SRC=D:\QSLtest
set DST=D:\HAMLOG
set STRUCT=H

powershell.exe -ExecutionPolicy Bypass -File ".\QslMover.ps1" ^
    -SourceFolder "%SRC%" ^
    -DestinationFolder "%DST%" ^
    -FolderMode "%STRUCT%" ^
    -RenameOutput

pause