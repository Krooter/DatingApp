<<<<<<< HEAD
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR app

COPY . .

RUN dotnet restore ./DatingApp

RUN dotnet publish ./DatingApp -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DatingApp.dll"]
=======
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR app

COPY . .

RUN dotnet restore ./DatingApp

RUN dotnet publish ./DatingApp -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DatingApp.dll"]
>>>>>>> 7250f40e2b1df400eaffd705193436f4a90df4ef
#docker build -f DatingApp/Dockerfile  -t test .