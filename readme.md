## Docker deploy
The Network Discovery API needs four Docker secrets.  These are the hostname, SSH port, username, and password of the router.
```bash
RouterHost=192.168.1.10
RouterPort=22
RouterUsername=root
RouterPassword=
```
## Running
```bash
curl --header 'Content-Length: 0' --request PUT --url http://localhost:46273/lights
```
```powershell
curl -Headers @{"Content-Length"=0} -Method Put -Uri http://localhost:46273/lights
```
