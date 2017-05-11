Param(
  [string]$git_path = "git.exe",
  [string]$svn_path = "svn.exe"
)

function FormatVersionWithRevision([string]$version = "1.0.0.0", [string]$rev = "0") {
    $vSplit = $version.Split('.')
    $ignore = 0
    if($vSplit.Length -ne 4 `
        -Or -Not [System.UInt32]::TryParse($vSplit[0], [ref] $ignore) `
        -Or -Not [System.UInt32]::TryParse($vSplit[1], [ref] $ignore) `
        -Or -Not [System.UInt32]::TryParse($vSplit[2], [ref] $ignore) `
        -Or -Not [System.UInt32]::TryParse($vSplit[3], [ref] $ignore))
    {
        throw "Version number is invalid. Must be in the form of 0.0.0.0"
    }
    $major = $vSplit[0]
    $minor = $vSplit[1]
    $patch = $vSplit[2]
    return "$major.$minor.$patch.$rev"
}

function Get-RevisionNumber([string]$vcs_default = "0", [string]$vcs = "git") {
    $rev = $null
    if ($vcs -eq "git") {
        $rev = Get-GitRevisionNumber
    } elseif ($vcs -eq "svn") {
        $rev = Get-SvnRevisionNumber
    }
    
    if ([System.String]::IsNullOrEmpty($rev))
    {
        $rev = $vcs_default
    }
    else
    {
        $ignore = 0
        if (-Not [System.UInt32]::TryParse($rev, [ref] $ignore))
        {
            $rev = $vcs_default
        }
    }
    
    return $rev
}

function Get-GitRevisionNumber {
    $oldErrorAction = $ErrorActionPreference
    try {
		$ErrorActionPreference = "SilentlyContinue"
		$rev = (. "$git_path" rev-list --count --first-parent HEAD 2> $null)

		if ([System.String]::IsNullOrEmpty($rev))
		{
			$rev = $null
		}

		return $rev
    }
    catch [system.exception]
    {
        return $null
    }
	finally
	{
        $ErrorActionPreference = $oldErrorAction
	}
}

function Get-SvnRevisionNumber {
    $oldErrorAction = $ErrorActionPreference
    try
    {
	    $ErrorActionPreference = "SilentlyContinue"
        $xmlStr = (. "$svn_path" info --xml 2> $null)
        $xml = [xml]$xmlStr
        $rev = $xml.info.entry.revision
        
        if ([System.String]::IsNullOrEmpty($rev))
        {
            $rev = $null
        }
        
        return $rev
    }
    catch [system.exception]
    {
        return $null
    }
	finally
	{
	    $ErrorActionPreference = $oldErrorAction
	}
}