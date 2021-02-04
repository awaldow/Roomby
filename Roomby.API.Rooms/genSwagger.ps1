dotnet tool run swagger tofile --output Roomby.API.Rooms.v1.json bin/Release/net5.0/Roomby.API.Rooms.dll v1

dotnet tool run openapi-to-terraform -f Roomby.API.Rooms.v1.json -o /home/awaldow/source/repos/roomby/roomby.api/terraform/test/rooms -t terraformVarSub.json