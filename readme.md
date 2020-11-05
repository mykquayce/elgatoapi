```bash
dotnet user-secrets set EndPoint 192.168.1.218:9123 --id ef996a87-a0d2-4582-8ba0-d99d39b93af4
```
```bash
curl --header 'Content-Length: 0' --request PUT --url http://localhost:46273/lights
```
```powershell
curl -Headers @{"Content-Length"=0} -Method Put -Uri http://localhost:46273/lights
```
