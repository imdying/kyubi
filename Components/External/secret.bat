@echo off
set "exec=kyubi"

:: Null
if [%1] == [] (
    echo Missing parameter. & exit /b
)

if [%2] == [] (
    %exec% aes %1 app.txt false
) else (
    %exec% aes %1 %2 false
)