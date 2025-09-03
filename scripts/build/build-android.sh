#!/bin/bash

# Build script for Android APK
echo "Building Android APK..."

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Install MAUI workload if not already installed
echo "Installing MAUI workload..."
dotnet workload install maui

# Build Android APK
echo "Building Android APK..."
dotnet publish NdcApp/NdcApp.csproj -f net8.0-android -c Release -p:BuildingForRelease=true -p:AndroidPackageFormat=apk

echo "Android APK build completed!"
echo "APK location: NdcApp/bin/Release/net8.0-android/publish/"