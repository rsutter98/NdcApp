#!/bin/bash

echo "Building Android APK..."

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Build for Android
echo "Building Android app..."
dotnet publish NdcApp/NdcApp.csproj -f net8.0-android -c Release -p:BuildingForRelease=true -p:AndroidPackageFormat=apk

echo "Android build completed!"
echo "Output: NdcApp/bin/Release/net8.0-android/publish/"