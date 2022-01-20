#! /bin/bash

# grab all the certificates files in ~/.aspnet/https, and make them Docker secrets

for path in ~/.aspnet/https/*.crt ~/.aspnet/https/*.key;
do
	
	if [ -f "$path" ]; then
		filename=$(basename "$path")
		docker secret ls --format '{{.Name}}' | findstr "$filename"
		if [ $? -ne 0 ]; then
			docker secret create "$filename" "$path"
		fi
	fi
done


# pull base images
docker pull eassbhhtgu/elgatoapi:latest
docker pull eassbhhtgu/networkdiscoveryapi


# does the stack already exist?
docker stack ls | tail --line=2 | grep elgatoapi

if [ $? -ne 0 ]; then
	# deploy
	docker stack deploy --compose-file ./docker-compose.yml elgatoapi
else
	# update
	docker service ls --format '{{.Image}} {{.Name}}' | \
		grep elgato | \
		awk '{system("docker service update --image " $1 " " $2)}'
fi
