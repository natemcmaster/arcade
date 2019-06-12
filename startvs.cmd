@ECHO OFF
SETLOCAL

:: This command launches a Visual Studio solution with environment variables required to use a local version of the .NET Core SDK.

:: This tells .NET Core to use the same dotnet.exe that build scripts use
SET DOTNET_ROOT=%~dp0.dotnet\

:: Put our local dotnet.exe on PATH first so Visual Studio knows which one to use
SET PATH=%DOTNET_ROOT%;%PATH%


IF NOT EXIST "%DOTNET_ROOT%\dotnet.exe" (
    echo .NET Core has not yet been installed. Run `%~dp0restore.cmd` to install tools
    exit /b 1
)

start %~dp0Arcade.sln
