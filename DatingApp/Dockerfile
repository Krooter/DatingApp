FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR app

COPY *.sln .
COPY /DatingApp/DatingApp.csproj ./DatingApp/
COPY /DatingApp.Core/DatingApp.Core.csproj ./DatingApp.Core/
COPY /DatingApp.Data/DatingApp.Data.csproj ./DatingApp.Data/
COPY /UnitTest/UnitTest.csproj ./UnitTest/

RUN dotnet restore ./DatingApp

COPY . ./DatingApp/
COPY . ./DatingApp.Core/
COPY . ./DatingApp.Data/

RUN dotnet publish ./DatingApp -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]