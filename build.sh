#! /bin/bash

docker pull mcr.microsoft.com/dotnet/aspnet:5.0
docker pull mcr.microsoft.com/dotnet/sdk:5.0

docker build --tag eassbhhtgu/elgatoapi:latest .

docker push eassbhhtgu/elgatoapi:latest
