# Notes App

Full-stack app built in **Nuxt**, **.NET 10**, **PostgreSQL** and **Docker Compose**, in order to manage
User auth and notes.

## Tech Stack

- Nuxt (Vue)
- TailwindCSS
- TypeScript
- ASP.NET 10
- Entity Framework Core
- PostgreSQL
- Docker Compose

## Database

The API uses PostgreSQL through Docker Compose.

Database config:

```text
database: notes_app
user: postgres
pwd: postgres
port: 5432
```

In Docker Compose, the API connects to PostgreSQL using the services named `postgres`,
with the following connection string:

```txt
Host=postgres;Port=5432;Database=notes_app;Username=postgres;Password=postgres
```

## Run the project

You should be in route where the `docker-compose.yml` file is, then run the following command:

```bash
docker compose up --build
```

That runs the services and will allow you to access to the projects URLs

### Frontend 
```txt
http://localhost:3080
```

### Backend
```txt
http://localhost:8080
```

### OpenAPI (development)
```txt
http://localhost:8080/openapi/v1.json
```
