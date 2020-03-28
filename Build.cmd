@echo off
REM Inspired by https://github.com/dotnet/roslyn/blob/master/Build.cmd
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\build.ps1""" %*"