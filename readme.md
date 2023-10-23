# xPayApp

## Steps to Test

### Tech:
mySql
dotnet7
Plain Js
Bootstrap
Fontawesome
http-server (npm)

#### Create User Secret:
Navigate to the xPayServer and run the following in the console
- dotnet user-secrets init
- dotnet user-secrets set "xPayDb" "Server=localhost;Database=xPayDb;user={insertUsernameHere};Password={insertPasswordHere};Pooling=true"
NB: Ensure that the user used above has enough auth to create and manipulate databases.
- run dotnet ef database update
- dotnet run

Open another terminal and navigate to the xPayClient directory
and enter http-server
When the browser launches, enter the following into the login form {username: admin, password: 1234}

