. "..\default.ps1"

$baseVersion = "1.0.0.0"
$packageVersion = FormatVersionWithRevision $baseVersion (Get-RevisionNumber)
$includeSymbols = $true

Install-Module VSSetup -Scope CurrentUser

$vsSetup = Get-VSSetupInstance | Select-VSSetupInstance -Latest
$msbuildLocation = ($vsSetup.InstallationPath+"\MSBuild\15.0\Bin\MSBuild.exe")
if (-Not (Test-Path $msbuildLocation)) {
  echo "Could not detect msbuild.exe path"
  return
}

. "$msbuildLocation" "/p:Configuration=Release" "/p:PackageOutputPath=../distribution" "/p:Version=$packageVersion" "/p:IncludeSymbols=$($includeSymbols.ToString().ToLower())" "/t:Clean;Restore;Build;Pack"
