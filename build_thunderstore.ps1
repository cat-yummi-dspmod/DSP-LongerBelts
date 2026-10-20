$ModName = "LongerBelts"
$OutputZip = ".\bin\Release\LongerBelts.zip"
dotnet build -c Release
if ($LASTEXITCODE -ne 0) { exit 1 }
$TempDir = ".\thunderstore_package"
if (Test-Path $TempDir) { Remove-Item $TempDir -Recurse -Force }
New-Item -ItemType Directory -Path $TempDir | Out-Null
Copy-Item ".\manifest.json", ".\README.md", ".\icon.png" -Destination $TempDir -ErrorAction SilentlyContinue
if (Test-Path ".\CHANGELOG.md") { Copy-Item ".\CHANGELOG.md" $TempDir }
Copy-Item ".\bin\Release\net472\$ModName.dll" $TempDir
if (Test-Path $OutputZip) { Remove-Item $OutputZip -Force }
Compress-Archive -Path "$TempDir\*" -DestinationPath $OutputZip -Force
Remove-Item $TempDir -Recurse -Force
Write-Host "Package created: $OutputZip"
