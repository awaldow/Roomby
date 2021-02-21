dotnet tool run swagger tofile --output Roomby.API.Users.v1.json bin/$BUILD_CONFIGURATION/net5.0/Roomby.API.Users.dll v1

dotnet tool run openapi-to-terraform-rev-cli generate -f Roomby.API.Users.v1.json -o revisions.json -a bin/$BUILD_CONFIGURATION/net5.0/Roomby.API.Users.dll

dotnet tool run openapi-to-terraform gen-tf -f Roomby.API.Users.v1.json -o ../terraform/test/users -t terraformVarSub.json -r revisions.json