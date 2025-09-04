@echo off

REM Build script for Windows MSIX
echo Building Windows MSIX...

REM Restore dependencies
echo Restoring dependencies...
dotnet restore

REM Install MAUI workload if not already installed
echo Installing MAUI workload...
dotnet workload install maui

REM Build Windows MSIX
echo Building Windows MSIX...
dotnet publish NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0 -c Release -p:BuildingForRelease=true -p:PublishProfile=MSIX -p:GenerateAppxPackageOnBuild=true -p:AppxPackageSigningEnabled=false

echo Windows MSIX build completed!
echo MSIX location: NdcApp/bin/Release/net8.0-windows10.0.19041.0/win10-x64/AppPackages/