FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR app

COPY . .

RUN dotnet restore ./DatingApp

RUN dotnet publish ./DatingApp -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DatingApp.dll"]

#docker build -f DatingApp/Dockerfile  -t test .
#docker run -d -p 5000:80 --name datingapp_api datingapp-api