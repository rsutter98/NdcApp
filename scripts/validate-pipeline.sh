#!/bin/bash

# Pipeline validation script
echo "🔍 Validating deployment pipeline configuration..."

# Check for required files
echo "Checking required files..."
files=(
    ".github/workflows/ci.yml"
    ".github/workflows/release.yml"
    "scripts/build/build-android.sh"
    "scripts/build/build-windows.bat"
    "scripts/build/build-ios.sh"
    "DEPLOYMENT.md"
    "BUILD.md"
)

missing_files=()
for file in "${files[@]}"; do
    if [ ! -f "$file" ]; then
        missing_files+=("$file")
    else
        echo "✅ $file"
    fi
done

if [ ${#missing_files[@]} -ne 0 ]; then
    echo "❌ Missing files:"
    printf '%s\n' "${missing_files[@]}"
    exit 1
fi

echo ""
echo "🔧 Validating project configuration..."

# Check if project builds
echo "Testing standard build..."
if dotnet build --verbosity quiet; then
    echo "✅ Standard build successful"
else
    echo "❌ Standard build failed"
    exit 1
fi

# Check if tests pass
echo "Testing unit tests..."
if dotnet test --verbosity quiet --nologo; then
    echo "✅ All tests pass"
else
    echo "❌ Tests failed"
    exit 1
fi

# Check if release build works
echo "Testing release build..."
if dotnet build --configuration Release --verbosity quiet; then
    echo "✅ Release build successful"
else
    echo "❌ Release build failed"
    exit 1
fi

echo ""
echo "📋 Checking YAML syntax..."

# Validate YAML files
if python3 -c "import yaml; yaml.safe_load(open('.github/workflows/ci.yml'))" 2>/dev/null; then
    echo "✅ CI workflow YAML valid"
else
    echo "❌ CI workflow YAML invalid"
    exit 1
fi

if python3 -c "import yaml; yaml.safe_load(open('.github/workflows/release.yml'))" 2>/dev/null; then
    echo "✅ Release workflow YAML valid"
else
    echo "❌ Release workflow YAML invalid"
    exit 1
fi

echo ""
echo "📦 Checking project targets..."

# Check if multi-targeting is configured
if grep -q "net8.0-android;net8.0-ios;net8.0-windows10.0.19041.0" NdcApp/NdcApp.csproj; then
    echo "✅ Multi-targeting configured"
else
    echo "❌ Multi-targeting not configured"
    exit 1
fi

# Check MSIX configuration
if grep -q "WindowsPackageType>MSIX" NdcApp/NdcApp.csproj; then
    echo "✅ MSIX packaging configured"
else
    echo "❌ MSIX packaging not configured"
    exit 1
fi

echo ""
echo "🎉 Pipeline validation completed successfully!"
echo ""
echo "Next steps:"
echo "1. Push to GitHub to test CI workflow"
echo "2. Create a git tag to test release workflow:"
echo "   git tag v1.0.0"
echo "   git push origin v1.0.0"
echo "3. Check GitHub Actions for build results"