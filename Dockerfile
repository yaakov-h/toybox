FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY src/*.csproj ./src/
RUN dotnet restore src/toybox.csproj

COPY src/. ./app/
WORKDIR /source/app
RUN dotnet publish src/toybox.csproj -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "toybox.dll"]
