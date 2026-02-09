# MyKanban

Full-stack Kanban board with a .NET 8 Web API backend and a React (Vite) frontend.

## Tech Stack
- Backend: ASP.NET Core 8, Entity Framework Core, SQL Server
- Frontend: React 19, Vite
- Database: SQL Server (Docker)

## Project Structure
- BackEnd/MyKanban/ - .NET solution and API project
- FrontEnd/myKanban/ - React app
- docker-compose.yml - SQL Server container

## Prerequisites
- .NET SDK 8
- Node.js 18+
- Docker Desktop

## Environment Variables
Frontend environment file:
- FrontEnd/myKanban/.env
  - `VITE_API_BASE_URL=http://localhost:53169`

> If you change the backend port, update this value.

## Run SQL Server (Docker)
From the repository root:
1. `docker compose up -d sqlserver`

Default credentials (see docker-compose.yml):
- User: sa
- Password: Your_strong_password123
- Port: 1433

## Run the Backend API
From BackEnd/MyKanban/:
1. `dotnet restore`
2. `dotnet run --project MyKanban/MyKanban.csproj`

Default URLs (see Properties/launchSettings.json):
- https://localhost:53168
- http://localhost:53169

Swagger UI:
- https://localhost:53168/swagger

## Run the Frontend
From FrontEnd/myKanban/:
1. `npm install`
2. `npm run dev`

App URL:
- http://localhost:5173

## API Endpoints
Base URL: http://localhost:53169
- GET /api/labels
- GET /api/statuses
- GET /api/tasks
- POST /api/tasks
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}

## Notes
- The database is created automatically on API startup.
- CORS is currently open to all origins for local development.

## Troubleshooting
- If `npm install` fails, make sure you run it inside FrontEnd/myKanban/.
- If you see SQL login errors, verify the password matches docker-compose.yml and appsettings.json.
