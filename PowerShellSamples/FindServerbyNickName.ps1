# dot-sourcing Login.ps1 to login to RightScale
. .\Login.ps1


# Get All Servers
$myServerNickname = "PHP1"

$response = $api.GetRequest("servers.xml","filter=nickname=" + $myServerNickname)


# Get servers from Content body
$serverXml = [xml] $response.Content.ReadAsString()

# Get server details from XML
$myServer = $serverXml.servers.server

# Print server details
$myserver
