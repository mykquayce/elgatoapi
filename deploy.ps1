docker pull eassbhhtgu/elgatoapi:latest
if (!$?) { return; }

docker stack deploy --compose-file .\docker-compose.yml elgato
