@echo off
set "exec=kyubi"

:: Null
if [%1] == [] (
    echo Missing parameter. & exit /b
)

if [%2] == [] (
    %exec% aes app.txt %1 true false
) else (
    %exec% aes %2 %1 true false
)