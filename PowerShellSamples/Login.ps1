function Login
{

	# Load NRightAPI.dll assembly
	[Reflection.Assembly]::LoadFile((Get-Location).Path + "\NRightAPI.dll")

	# Get Credentials from System Variables
	$username = [System.Environment]::GetEnvironmentVariable("VA_RS_username","machine")
	$password = [System.Environment]::GetEnvironmentVariable("VA_RS_password","machine")
	$account = [System.Environment]::GetEnvironmentVariable("VA_RS_account","machine")

	# Instantiate NRightAPI using RS Accout number
	$global:api = new-object RightClient.NRightAPI($account)

	# Login to RightScale 
	$global:api.Login($username,$password)

}
