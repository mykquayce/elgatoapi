#! /bin/bash


# pull base images
docker pull mcr.microsoft.com/dotnet/aspnet:6.0
docker pull mcr.microsoft.com/dotnet/sdk:6.0


# build
version=$(cat ./ElgatoApi.WebApplication/*.csproj | \
	grep --ignore-case --only-matching --perl-regex "<version>.+?</version>" | \
	sed --quiet --regexp-extended 's/<(.+?)>(.*)<\/\1>/\2/p')

image=eassbhhtgu/elgatoapi
tag1=$image:$version
tag2=$image:latest

docker build --tag $tag1 --tag $tag2 .


# push
docker push $tag1
docker push $tag2
