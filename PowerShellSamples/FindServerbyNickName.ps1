# dot-sourcing Login.ps1 to login to RightScale
. .\Login.ps1


# Get All Servers
$myServerNickname = "VA.Dev.LAMP_AutoScale_LoadBalance_ebsdb1"

$response = $api.GetRequest("servers.xml","filter=nickname=" + $myServerNickname)


# Get servers from Content body
$servers = [xml] $response.Content.ReadAsString()

# Get server details from XML
$myServer = $servers.servers.server

# Print server details
$myserver
