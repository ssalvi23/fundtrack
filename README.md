# FundTrack

Mutual fund and SIP portfolio tracker built as a full-stack learning project: .NET 8 Web API and Angular.

## Prerequisites

- .NET SDK 9.0.x (`global.json` pins it; the projects target `net8.0`)
- Node.js 20.19+ or 22.12+
- npm 11 (`npm install -g npm@11`; npm 10.9.2 crashes installing this project)

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
