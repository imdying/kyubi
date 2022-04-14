@echo off

:: Null
if [%1] == [] (
    echo Missing parameter. & exit /b
)

:: rclone mount "Remote:" * --volname "DRIVE_NAME" --vfs-cache-mode full --network-mode
kyubi mount %1 "Remote:" "Remote Drive"