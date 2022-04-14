@echo off
set "rar=C:\Program Files\WinRAR\rar.exe"

dotnet publish Kyubi.csproj /property:GenerateFullPaths=true --configuration Release
call "%rar%" a -ep "%~dp0bin\Kyubi.rar" "%~dp0bin\Release\net6.0-windows\win10-x86\publish\*.*"