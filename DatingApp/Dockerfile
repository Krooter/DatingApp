FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY DatingApp.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o res

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]