#! /bin/bash

# the Network Discovery API uses four Docker secrets
# promote environment variables to Docker secrets
for s in RouterHost RouterPort RouterUsername RouterPassword
do
	docker secret ls | tail --line=2 | grep $s

	if [ $? -ne 0 ]; then
		echo -n ${!s} | docker secret create $s -
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
