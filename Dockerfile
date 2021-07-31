# https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restore
WORKDIR /source

# copy csproj and restore as distinct layers
COPY PropertyDemo.sln .
COPY PropertyDemo/*.csproj ./PropertyDemo/
COPY PropertyDemo.Data/*.csproj ./PropertyDemo.Data/
COPY PropertyDemo.Migrations/*.csproj ./PropertyDemo.Migrations/
COPY PropertyDemo.Service/*.csproj ./PropertyDemo.Service/
COPY PropertyDemo/*.csproj ./PropertyDemo/

# copy everything else and build app
COPY PropertyDemo.Service/. ./PropertyDemo.Service/
COPY PropertyDemo.Migrations/. ./PropertyDemo.Migrations/
COPY PropertyDemo.Data/. ./PropertyDemo.Data/
COPY PropertyDemo/. ./PropertyDemo/
COPY PropertyDemo.sln .

FROM restore as build
WORKDIR /source/PropertyDemo
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS mvc

# ENV USERNAME=appuser
# ENV UID=1001
WORKDIR ${HOME}/app

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app ./

EXPOSE 80

ENTRYPOINT [ "dotnet", "PropertyDemo.dll" ]