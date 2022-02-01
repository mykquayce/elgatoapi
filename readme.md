## Docker deploy
Docker-compose uses six files from the local file system:

|File|Description|Usage|
|-|-|-|
|```~\.ssh\id_rsa```|Private SSH key|Used by the Discovery API to talk to the router|
|```~\.aspnet\https\ca.crt```|Certificate Authority key|Used by the Elgato API to certify communications with the Discovery API|
|```~\.aspnet\https\discovery.crt```|Discovery API service certificate|Used by the Discovery API to certify communications|
|```~\.aspnet\https\discovery.key```|Discovery API service certificate key|"|
|```~\.aspnet\https\localhost.crt```|Elgato API service certificate|Used by the Elgato API to certify communications|
|```~\.aspnet\https\localhost.key```|Elgato API service certificate key|"|

## Running
```bash
curl --cacert ~/.aspnet/https/ca.crt --request PUT --ssl-no-revoke --url https://localhost:46274/lights
```
```powershell
curl --cacert ${env:userprofile}\.aspnet\https\ca.crt --request PUT --ssl-no-revoke --url https://localhost:46274/lights
```
