# Installs the unity-gamedev-team into a Unity project.
# Usage (from this repo):  .\install.ps1 -Project "D:\Projects\MyGame"
# Re-running is safe: files you edited in the project are kept unless you pass -Force.

param(
    [Parameter(Mandatory = $true)][string]$Project,
    [switch]$Force
)

$ErrorActionPreference = "Stop"
$src = Join-Path $PSScriptRoot "claude"
$dst = Join-Path $Project ".claude"

if (-not (Test-Path (Join-Path $Project "Assets"))) {
    Write-Warning "No Assets folder in $Project - is this a Unity project root?"
}

$copied = 0; $kept = 0
Get-ChildItem $src -Recurse -File | ForEach-Object {
    $rel = $_.FullName.Substring($src.Length + 1)
    $target = Join-Path $dst $rel
    New-Item -ItemType Directory -Force -Path (Split-Path $target) | Out-Null
    if ((Test-Path $target) -and -not $Force) {
        if ((Get-FileHash $target).Hash -ne (Get-FileHash $_.FullName).Hash) {
            Write-Host "kept (edited):  $rel"; $kept++; return
        }
    }
    Copy-Item $_.FullName $target -Force; $copied++
}

# CLAUDE.md block
$block = Get-Content (Join-Path $PSScriptRoot "CLAUDE.md") -Raw
$claudeMd = Join-Path $Project "CLAUDE.md"
if (Test-Path $claudeMd) {
    $existing = Get-Content $claudeMd -Raw
    if ($existing -match "unity-gamedev-team:start") {
        $pattern = "(?s)<!-- unity-gamedev-team:start -->.*?<!-- unity-gamedev-team:end -->"
        $existing = [regex]::Replace($existing, $pattern, $block.Trim())
        Set-Content $claudeMd $existing -NoNewline
        Write-Host "CLAUDE.md: block updated"
    } else {
        Add-Content $claudeMd "`n$block"
        Write-Host "CLAUDE.md: block appended"
    }
} else {
    Set-Content $claudeMd $block
    Write-Host "CLAUDE.md: created"
}

Write-Host "Done: $copied files installed, $kept kept. Open Claude Code in $Project and run /stage."
