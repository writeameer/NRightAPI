# Load NRightAPI.dll assembly
[Reflection.Assembly]::LoadFile((Get-Location).Path + "\NRightAPI.dll")
$api = new-object RightClient.NRightAPI
 
# Login to RightScale 
$api.Login("<rightscale username>","<rightscale password>","<rightscale account number>")

# Get RightScripts from your RightScale Account
$response = $api.GetRequest("right_scripts.xml",$null)
 
# Output the rightscripts from the HTTP response
[RightClient.NRightAPI]::DisplayRestResponse($response)