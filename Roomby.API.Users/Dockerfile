FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Roomby.API.Users/Roomby.API.Users.csproj", "Roomby.API.Users/"]
COPY ["Roomby.API.Users.Data/Roomby.API.Users.Data.csproj", "Roomby.API.Users.Data/"]
COPY ["Roomby.API.Models/Roomby.API.Models.csproj", "Roomby.API.Models/"]
COPY ["Roomby.API.Users/Roomby.API.Users.xml", "Roomby.API.Users.xml"]
RUN dotnet restore "Roomby.API.Users/Roomby.API.Users.csproj"
COPY . .
WORKDIR "/src/Roomby.API.Users"
RUN dotnet build "Roomby.API.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Roomby.API.Users.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/Roomby.API.Users.xml .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Roomby.API.Users.dll"]
