# Notes App

Notes App is a full-stack demo project where users can register, log in, and manage their own notes.
It includes authentication, protected API endpoints, and a frontend that consumes the backend API.

## Project Purpose

This project is meant as a practical Clean Architecture example with:

- User authentication with JWT
- Notes CRUD (create, read, update, delete)
- Protected and public endpoints
- Docker-based local development
- Automated tests for application logic, repositories, and API endpoints

## Tech Stack

### Frontend

- Nuxt 4 (Vue 3)
- TypeScript
- Nuxt UI
- Tailwind CSS
- zod (form validation)

### Backend

- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core
- PostgreSQL
- JWT authentication

### Tooling

- Docker + Docker Compose
- xUnit + FluentAssertions + Moq

## Running the Project

Before running the project, make sure you have these tools installed:

- Docker Desktop (or Docker Engine + Docker Compose plugin)
- Git

Optional (only if you want to run backend tests locally outside Docker):

- .NET SDK 10

Run everything with Docker Compose from the repository root:

```bash
docker compose up --build
```

Services:

- Frontend: `http://localhost:3080`
- Backend API: `http://localhost:8080`
- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- PostgreSQL (host): `localhost:5433`

## Demo Seed Data

On first startup (empty DB), the backend seeds demo data:

- 2 users
- 3 notes per user
- Password for all demo users: `demo00123`

Demo users:

- `demo1@notesapp.local`
- `demo2@notesapp.local`

If you need to reset and reseed:

```bash
docker compose down -v
docker compose up --build
```

## Running Tests

Run all backend tests:

```bash
cd backend
dotnet test
```

Or by layer:

```bash
dotnet test tests/NotesApp.Application.Tests/NotesApp.Application.Tests.csproj -v minimal
dotnet test tests/NotesApp.Infrastructure.Tests/NotesApp.Infrastructure.Tests.csproj -v minimal
dotnet test tests/NotesApp.Api.Tests/NotesApp.Api.Tests.csproj -v minimal
```

## Repository Structure

```txt
backend/
  src/
    NotesApp.Domain/
    NotesApp.Application/
    NotesApp.Infrastructure/
    NotesApp.Api/
  tests/
    NotesApp.Application.Tests/
    NotesApp.Infrastructure.Tests/
    NotesApp.Api.Tests/

frontend/
docs/
docker-compose.yml
```

## Implementation Steps

1. After understanding the main goal, I worked on the user story and acceptance criteria to clearly define the purpose of the task.
2. I created dedicated markdown files for API and Frontend requirements to define goals, architecture, and detailed requirements for both projects.
3. I connected everything step by step, implementing features incrementally and fixing issues/details as they appeared.
4. After polishing errors and final details, I prepared complete documentation so the project is easy to run and test.

All the documentation is in `docs/` folder.
