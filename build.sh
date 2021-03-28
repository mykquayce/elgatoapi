#! /bin/bash

# pull base images
docker pull mcr.microsoft.com/dotnet/aspnet:6.0
docker pull mcr.microsoft.com/dotnet/sdk:6.0

# build
docker build --tag eassbhhtgu/elgatoapi:latest .

# push
docker push eassbhhtgu/elgatoapi:latest
