FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Roomby.API.Rooms/Roomby.API.Rooms.csproj", "Roomby.API.Rooms/"]
COPY ["Roomby.API.Rooms.Data/Roomby.API.Rooms.Data.csproj", "Roomby.API.Rooms.Data/"]
COPY ["Roomby.API.Models/Roomby.API.Models.csproj", "Roomby.API.Models/"]
COPY ["Roomby.API.Rooms/Roomby.API.Rooms.xml", "Roomby.API.Rooms.xml"]
RUN dotnet restore "Roomby.API.Rooms/Roomby.API.Rooms.csproj"
COPY . .
WORKDIR "/src/Roomby.API.Rooms"
RUN dotnet build "Roomby.API.Rooms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Roomby.API.Rooms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/Roomby.API.Rooms.xml .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Roomby.API.Rooms.dll"]
