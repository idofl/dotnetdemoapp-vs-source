##### Script Behavior #####
$ErrorActionPreference = “silentlycontinue”

##### Functions #####

function getTimeStamp{
    Get-Date -Format "MM-dd-yyyy-HH:mm:ss"
}

function postBanner ($type) { 
If ($type -eq "begin") {
        Write-Output "*****   C O N T A I N E R   D I A G N O S T I C S   ***** `n"
    } Elseif ($type -eq "end") {
        Write-Output "*****      E N D   O F   D I A G N O S T I C S      ***** `n"
    }
}

##### Main #####
postBanner("begin")
$containerName = hostname
$Timestamp = getTimestamp
Write-Output "[INFO]  Starting script execution at $Timestamp `n"
Write-Output "[INFO]  Container Name: $containerName `n"
$Timestamp = getTimestamp
Write-Output "[INFO]  Script execution complete at $Timestamp`n"
postBanner("end")



