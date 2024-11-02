$lines = (Invoke-WebRequest -Uri "https://raw.githubusercontent.com/mettliebe/CS2_TranslateCS2/refs/heads/master/TranslateCS2.hashes").Content
$isCorrupt = 0
foreach ($line in $lines) {
    $expectedHash = $line.Split(" ")[1];
    $expectedFileName = $line.Split(":")[0];

    $currentFile = Get-Item "$($Env:LOCALAPPDATA)Low\Colossal Order\Cities Skylines II\.cache\Mods\mods_subscribed\79187_*\$($expectedFileName)"
    $currentHash = (Get-FileHash $currentFile -Algorithm SHA512).Hash
    if ($currentHash -ne $expectedHash) {
        $isCorrupt = 1
        Write-Host `r`n`r`n
        Write-Host "TranslateCS2.Mod seems to be corrupt!" -ForegroundColor DarkRed
        Write-Host `r`n`r`n
        break
    }
}
if ($isCorrupt -eq 0) {
    Write-Host `r`n`r`n
    Write-Host "TranslateCS2.Mod seems to be OK." -ForegroundColor DarkGreen
    Write-Host `r`n`r`n
}