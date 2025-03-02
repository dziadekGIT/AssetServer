# AssetServer
Asset Server - app for store, view and publish own 3d assets.

## Spis treści

- [StateOfArt](#stateofart)
- [Funcionalities](#funcionalities)
- [Technologies](#technologies)
- [Run](#run)

## StateOfArt
**AssetServer developement roadmap**
- 
<img src="https://github.com/dziadekGIT/AssetServer/blob/feature/initial-code/readme/State%20of%20art.jpg?raw=true" alt ="roadmap" width="600"/>

**Frontend view - inital stage of project - basic functionality**
-
<img src="https://github.com/dziadekGIT/AssetServer/blob/feature/initial-code/readme/front.png" alt ="frontend" width="400"/>

## Funcionalities

- Add and store 3d assets in fbx format
- API RESTful for integration
- Contenerization with docker and docker compose
- In future - cloud deployment w with K8s

## Technologies

- **Backend**: C#, ASP.NET Core, Entity framework
- **Frontend**: Python, Django
- **DB**: PostgreSQL
- **Contenerization**: Docker, Docker Compose

## Run
- Build : docker compose up --build -d
- DB connect : dotnet ef database update --connection "Host=localhost;Port=5432;Database=assetdb;Username=myuser;Password=mypassword"
- http://127.0.0.1:8000

