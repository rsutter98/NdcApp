#!/bin/bash

# NDC Conference Planner - Manual Deployment Script
# This script builds and packages the app for local deployment testing

set -e

echo "🚀 NDC Conference Planner - Manual Deployment Script"
echo "=================================================="

# Configuration
PROJECT_NAME="NdcApp"
OUTPUT_DIR="./dist"
VERSION=$(date +%Y.%m.%d.%H%M)

# Clean previous builds
echo "🧹 Cleaning previous builds..."
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

# Build Core Library and Tests
echo "🔨 Building Core Library..."
dotnet build NdcApp.Core/NdcApp.Core.csproj --configuration Release

echo "🧪 Running Tests..."
dotnet test NdcApp.Tests/NdcApp.Tests.csproj --configuration Release --verbosity minimal

# Build Main App
echo "🏗️ Building Main Application..."
dotnet build NdcApp/NdcApp.csproj --configuration Release

# Create deployment package
echo "📦 Creating Deployment Package..."
PACKAGE_NAME="NdcApp-Manual-v$VERSION"
PACKAGE_DIR="$OUTPUT_DIR/$PACKAGE_NAME"

mkdir -p "$PACKAGE_DIR"

# Copy application files
cp -r NdcApp/bin/Release/net8.0/* "$PACKAGE_DIR/"

# Copy documentation
cp README.md "$PACKAGE_DIR/"
cp DEPLOYMENT.md "$PACKAGE_DIR/"
cp BENUTZERHANDBUCH.md "$PACKAGE_DIR/"

# Create run script
cat > "$PACKAGE_DIR/run.sh" << 'EOF'
#!/bin/bash
echo "Starting NDC Conference Planner..."
dotnet NdcApp.dll
EOF

cat > "$PACKAGE_DIR/run.bat" << 'EOF'
@echo off
echo Starting NDC Conference Planner...
dotnet NdcApp.dll
pause
EOF

chmod +x "$PACKAGE_DIR/run.sh"

# Create archive
echo "🗜️ Creating Archive..."
cd "$OUTPUT_DIR"
zip -r "$PACKAGE_NAME.zip" "$PACKAGE_NAME"
cd ..

echo "✅ Deployment Package Created!"
echo "📍 Location: $OUTPUT_DIR/$PACKAGE_NAME.zip"
echo "📏 Size: $(du -h "$OUTPUT_DIR/$PACKAGE_NAME.zip" | cut -f1)"

echo ""
echo "📋 Manual Installation Instructions:"
echo "1. Extract the ZIP file to desired location"
echo "2. Ensure .NET 8.0 Runtime is installed"
echo "3. Run 'dotnet NdcApp.dll' or use the provided run scripts"
echo ""
echo "🔗 For detailed deployment information, see DEPLOYMENT.md"