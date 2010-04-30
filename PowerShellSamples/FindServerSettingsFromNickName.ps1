# dot-sourcing Login.ps1 to login to RightScale
. .\Login.ps1

# Get All Servers
$myServerNickname = "VA.Dev.LAMP_AutoScale_LoadBalance_ebsdb1"
$response = $api.GetRequest("servers.xml","filter=nickname=" + $myServerNickname)

# Get servers from Content body
$servers = [xml] $response.Content.ReadAsString()

# Get server details from XML
$myServer = $servers.servers.server

# Get Server ID from server href
$serverid = $myserver.href.Split("/")[$myserver.href.Split("/").Length - 1 ]

# Call server setting REST method
$response = $api.GetRequest("servers/" + $serverid + "/settings",$null)
[xml] $serverSettings = $response.Content.ReadAsString()

# Output server details
# outputs ip address, ssh key etc

($serverSettings.settings)
