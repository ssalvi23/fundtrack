# FundTrack

Mutual fund and SIP portfolio tracker built as a full-stack learning project: .NET 8 Web API and Angular.

## Prerequisites

- .NET SDK 9.0.x (`global.json` pins it; the projects target `net8.0`)
- Node.js 20.19+ or 22.12+
- npm 11 (`npm install -g npm@11`; npm 10.9.2 crashes installing this project)

## First-time database setup

Needs Docker Desktop running. The first start of SQL Server can take a few minutes.

```powershell
# Create .env from .env.example and set your own password, then:
docker compose up -d --wait

cd backend
dotnet tool restore
dotnet user-secrets set "Database:ConnectionString" "Server=127.0.0.1,1433;Database=FundTrack;User Id=sa;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True" --project src/FundTrack.Api
dotnet ef database update --project src/FundTrack.Infrastructure --startup-project src/FundTrack.Api
cd ..
```

## Run locally

```powershell
# Terminal 1: API on http://localhost:5049 (Swagger at /swagger)
dotnet run --project backend/src/FundTrack.Api --launch-profile http

# Terminal 2: Angular on http://localhost:4200
cd frontend
npm install
npm start
```

## Quality checks

```powershell
dotnet test backend/FundTrack.sln

cd frontend
npm run lint
npm run format:check
npm test -- --watch=false
```

## Branching

- `main`: always releasable
- `develop`: integration branch
- `feature/*`: one piece of work, merged into `develop` through a PR
