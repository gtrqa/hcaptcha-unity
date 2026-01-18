# ==============================
# hCaptcha Unity – SETUP SCRIPT
# Compatible with PowerShell 5.1
# ==============================

$ErrorActionPreference = "Stop"

# ---- PATHS (CHANGE IF NEEDED) ----
$UNITY_PROJECT = "C:\Users\userfg\captcha"
$ANDROID_AAR   = "F:\Users\userfg\AndroidStudioProjects\unithhcapt\unithhcapt-lib\build\outputs\aar\unithhcapt-lib-release.aar"
$REPO_ROOT     = "C:\Users\userfg\hcaptcha-unity"

Write-Host ""
Write-Host "hCaptcha Unity Setup" -ForegroundColor Cyan
Write-Host "--------------------" -ForegroundColor Cyan
Write-Host ""

# ---- CHECK REPO ----
if (-not (Test-Path $REPO_ROOT)) {
    Write-Host "ERROR: Repo not found at:" -ForegroundColor Red
    Write-Host $REPO_ROOT -ForegroundColor Red
    exit 1
}

# ---- CREATE FOLDERS ----
$folders = @(
    "Unity\Runtime\Plugins\Android",
    "Unity\Runtime\Scripts",
    "Unity\Runtime\Prefabs",
    "Unity\Editor",
    "Unity\Samples~\BasicUsage\Scenes",
    "Unity\Samples~\BasicUsage\Scripts"
)

foreach ($folder in $folders) {
    $fullPath = Join-Path $REPO_ROOT $folder
    if (-not (Test-Path $fullPath)) {
        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
    }
}

Write-Host "✓ Folder structure created" -ForegroundColor Green

# ---- COPY AAR ----
if (Test-Path $ANDROID_AAR) {
    Copy-Item `
        -Path $ANDROID_AAR `
        -Destination (Join-Path $REPO_ROOT "Unity\Runtime\Plugins\Android\unithhcapt-lib.aar") `
        -Force

    Write-Host "✓ AAR copied" -ForegroundColor Green
} else {
    Write-Host "⚠ AAR not found:" -ForegroundColor Yellow
    Write-Host $ANDROID_AAR -ForegroundColor Yellow
}

# ---- COPY GRADLE TEMPLATES ----
$gradleSrc = Join-Path $UNITY_PROJECT "Assets\Plugins\Android"

$settingsTemplate = Join-Path $gradleSrc "settingsTemplate.gradle"
$mainTemplate     = Join-Path $gradleSrc "mainTemplate.gradle"

if (Test-Path $settingsTemplate) {
    Copy-Item $settingsTemplate (Join-Path $REPO_ROOT "Unity\Runtime\Plugins\Android") -Force
    Write-Host "✓ settingsTemplate.gradle copied" -ForegroundColor Green
}

if (Test-Path $mainTemplate) {
    Copy-Item $mainTemplate (Join-Path $REPO_ROOT "Unity\Runtime\Plugins\Android") -Force
    Write-Host "✓ mainTemplate.gradle copied" -ForegroundColor Green
}

# ---- COPY C# SCRIPTS ----
$scriptsSrc = Join-Path $UNITY_PROJECT "Assets"

$bridge = Get-ChildItem `
    -Path $scriptsSrc `
    -Recurse `
    -Filter "HCaptchaUnityBridge.cs" `
    -ErrorAction SilentlyContinue `
    | Select-Object -First 1

if ($bridge) {
    Copy-Item $bridge.FullName (Join-Path $REPO_ROOT "Unity\Runtime\Scripts") -Force
    Write-Host "✓ HCaptchaUnityBridge.cs copied" -ForegroundColor Green
} else {
    Write-Host "⚠ HCaptchaUnityBridge.cs not found" -ForegroundColor Yellow
}

$test = Get-ChildItem `
    -Path $scriptsSrc `
    -Recurse `
    -Filter "TestHCaptcha.cs" `
    -ErrorAction SilentlyContinue `
    | Select-Object -First 1

if ($test) {
    Copy-Item $test.FullName (Join-Path $REPO_ROOT "Unity\Samples~\BasicUsage\Scripts") -Force
    Write-Host "✓ TestHCaptcha.cs copied" -ForegroundColor Green
} else {
    Write-Host "⚠ TestHCaptcha.cs not found" -ForegroundColor Yellow
}

# ---- DONE ----
Write-Host ""
Write-Host "✓ SETUP COMPLETE" -ForegroundColor Green
Write-Host ""

Write-Host "NEXT STEPS:" -ForegroundColor Yellow
Write-Host "1. Save artifacts 1–6 to correct folders" -ForegroundColor White
Write-Host "2. In Unity: Create prefab with HCaptchaManager component" -ForegroundColor White
Write-Host "3. Copy prefab to: Unity\Runtime\Prefabs\" -ForegroundColor White
Write-Host "4. Run:" -ForegroundColor White
Write-Host "   git add Unity/" -ForegroundColor White
Write-Host "   git commit" -ForegroundColor White
Write-Host "   git push" -ForegroundColor White
Write-Host ""
