# script to check integrity


#
# take care:
# after a new mod-version is released,
# it may take a couple of minutes,
# until git replicated the changes to all their servers
#


# prepare to fetch hashes generated while publishing mod from git-repo
[string]$repoURI = "https://raw.githubusercontent.com/mettliebe/CS2_TranslateCS2/refs/heads/master/TranslateCS2.Mod/TranslateCS2.Mod.hashes"
[string]$contentType = "text/plain; charset=utf8"

# fetch hashes
[string]$repoHashes = (Invoke-WebRequest -Uri $repoURI -ContentType $contentType).Content

# split content into lines
$lines = $repoHashes.Replace("`r", "").Split("`n")

# minus one, cause the repo-file contains an empty 'closing' line
[int]$expectedFileCount = $lines.Length - 1


# prepare access to mod-directory
[string]$modsSubscribedPath = "$($Env:LOCALAPPDATA)Low\Colossal Order\Cities Skylines II\.cache\Mods\mods_subscribed"
[string]$modId = "79187"
# the asterisk to skip co's internal counter
$modDirectory = Get-Item "$($modsSubscribedPath)\$($modId)_*"

# get files in mod-directory
$modDirectoriesFiles = Get-ChildItem $modDirectory -File
[int]$currentFileCount = $modDirectoriesFiles.Length


if ($expectedFileCount -ne $currentFileCount) {
    # there are more or less files than expected
    # the mod seems to be corrupt

    Write-Host `r`n`r`n

    Write-Host "TranslateCS2.Mod seems to be corrupt." -ForegroundColor DarkRed
    Write-Host "Expected $($expectedFileCount) files." -ForegroundColor DarkRed
    Write-Host "Found $($currentFileCount) files. " -ForegroundColor DarkRed
    
    Write-Host `r`n`r`n


} else {
    # file-counts are equal
    # time to check each files hash


    [int]$isCorrupt = 0

    foreach ($line in $lines) {

        [string]$expectedHash = $line.Split(" ")[1];
        [string]$expectedFileName = $line.Split(":")[0];
    
        $currentFile = Get-Item "$($modDirectory)\$($expectedFileName)"
        [string]$currentHash = (Get-FileHash $currentFile -Algorithm SHA512).Hash

        if ($currentHash -ne $expectedHash) {
            # current hash differs from expected ones

            $isCorrupt = 1

            Write-Host `r`n`r`n
            Write-Host "TranslateCS2.Mod seems to be corrupt!" -ForegroundColor DarkRed
            Write-Host `r`n
            Write-Host "$($expectedFileName)'s Hash is another one than expected!" -ForegroundColor DarkRed
            Write-Host `r`n
            Write-Host "Location: $($currentFile)" -ForegroundColor DarkRed
            Write-Host `r`n
            Write-Host "current  Hash: $($currentHash)" -ForegroundColor DarkRed
            Write-Host "expected Hash: $($expectedHash)" -ForegroundColor DarkRed
            Write-Host `r`n`r`n
            
            break
        }
    }
    if ($isCorrupt -eq 0) {
        # file-counts match
        # and
        # hashes match

        Write-Host `r`n`r`n
        Write-Host "TranslateCS2.Mod seems to be OK." -ForegroundColor DarkGreen
        Write-Host `r`n`r`n
    }
}