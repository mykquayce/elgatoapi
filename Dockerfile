FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
WORKDIR /app
COPY . .
RUN dotnet restore --source https://api.nuget.org/v3/index.json --source http://nuget/v3/index.json
RUN dotnet publish ElgatoApi.WebApplication/ElgatoApi.WebApplication.csproj --configuration Release --output /app/publish --runtime linux-x64

FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80/tcp
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "ElgatoApi.WebApplication.dll"]
