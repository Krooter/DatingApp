FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o res

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR ../DatingApp
COPY --from=build-env ../DatingApp .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]