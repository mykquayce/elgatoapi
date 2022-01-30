#! /bin/bash


# pull base images
docker pull mcr.microsoft.com/dotnet/aspnet:latest
docker pull mcr.microsoft.com/dotnet/sdk:latest


# build
docker build --tag eassbhhtgu/elgatoapi:latest .


# push
docker push eassbhhtgu/elgatoapi:latest
