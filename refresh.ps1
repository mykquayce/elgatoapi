docker pull mcr.microsoft.com/dotnet/aspnet:6.0
docker pull mcr.microsoft.com/dotnet/sdk:6.0
docker pull eassbhhtgu/elgatoapi:latest

$base1 = docker image inspect --format '{{.Created}}' mcr.microsoft.com/dotnet/aspnet:6.0
$base2 = docker image inspect --format '{{.Created}}' mcr.microsoft.com/dotnet/sdk:6.0
$target = docker image inspect --format '{{.Created}}' eassbhhtgu/elgatoapi:latest

if ($base1 -gt $target -or $base2 -gt $target) {

	docker build `
		--secret id=ca_crt,src=${env:userprofile}\.aspnet\https\ca.crt `
		--tag eassbhhtgu/elgatoapi:latest `
		.
	if (!$?) { exit 1; }

	docker push eassbhhtgu/elgatoapi:latest
	if (!$?) { exit 1; }
}
