@echo off
setlocal enabledelayedexpansion
for /f "delims=" %%i in ('dir /ad-h /b /s') do (
set n=0
for /f "delims=" %%j in ('dir "%%i" /a /b') do set /a n+=1
if !n!==0 copy readme.md "%%i"
)

pause