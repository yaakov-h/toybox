FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY src/*.csproj .
RUN dotnet restore --use-current-runtime  

COPY src/. .
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "toybox.dll"]
