# dot-sourcing Login.ps1 to login to RightScale
. .\Login.ps1

# Get All Servers
$response = $api.GetRequest("servers.xml",$null)

# Get servers from Content body
$servers = [xml] $response.Content.ReadAsString()

# Print Servers
$servers.servers.server

