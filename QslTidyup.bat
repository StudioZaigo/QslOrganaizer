
@echo off

rem		-SourceFolder "%SRC%" は、ソースフォルダです。SRCで指定してください。

set SRC="D:\QSLtest"

powershell.exe -ExecutionPolicy Bypass -File ".\QslTidyup.ps1" ^
    -SourceFolder "%SRC%" 


pause