dotnet tool run swagger tofile --output Roomby.API.Users.v1.json bin/Debug/net5.0/Roomby.API.Users.dll v1

dotnet tool run openapi-to-terraform -f Roomby.API.Users.v1.json -o /home/awaldow/source/repos/roomby/roomby.api/terraform/test/users -t terraformVarSub.json