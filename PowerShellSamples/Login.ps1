# Load NRightAPI.dll assembly
[Reflection.Assembly]::LoadFile((Get-Location).Path + "\NRightAPI.dll")

# Get Credentials from System Variables
$username = [System.Environment]::GetEnvironmentVariable("RS_username","machine")
$password = [System.Environment]::GetEnvironmentVariable("RS_password","machine")
$account = [System.Environment]::GetEnvironmentVariable("RS_acct","machine")


# I'm using system variable to avoid accidentally checking in my RightScale credentials :)
# Username, password and account can be specified below as string

# Instantiate NRightAPI using RS Accout number
$api = new-object RightClient.NRightAPI($account)

# Login to RightScale 
$api.Login($username,$password)