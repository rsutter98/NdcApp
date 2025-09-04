#!/bin/bash

# Build script for iOS
echo "Building iOS App..."

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Install MAUI workload if not already installed
echo "Installing MAUI workload..."
dotnet workload install maui

# Build iOS App
echo "Building iOS App..."
dotnet build NdcApp/NdcApp.csproj -f net8.0-ios -c Release -p:BuildingForRelease=true

echo "iOS build completed!"
echo "App location: NdcApp/bin/Release/net8.0-ios/"
echo "Note: iOS builds require a Mac with Xcode for full deployment"