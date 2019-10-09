$hostsFile = "C:\Windows\System32\drivers\etc\hosts"
$src = [System.IO.File]::ReadAllLines($hostsFile)
$a = $src += ""
$ip = (Test-Connection -ComputerName (hostname) -Count 1).IPV4Address.IPAddressToString
$gw = Get-NetRoute -DestinationPrefix "0.0.0.0/0" | Select-Object -ExpandProperty "NextHop"
For ($i=0; $i -le $a.length; $i++) {
        try {
          if ($a[$i].Contains("host.docker.internal"))
          {
                $a[$i] = "$ip host.docker.internal"
                $a[$i+1] = "$gw gateway.docker.internal"
                [System.IO.File]::WriteAllLines($hostsFile, [string[]]$a)
                exit
          }
        }
        catch {
          Write-Output "There are no records in hosts file. I am adding them."
        }
}
$a = $a += "# Added by Docker for Windows"
$a = $a += "$ip host.docker.internal"
$a = $a += "$gw gateway.docker.internal"
$a = $a += "# End of section"
[System.IO.File]::WriteAllLines($hostsFile, [string[]]$a)
