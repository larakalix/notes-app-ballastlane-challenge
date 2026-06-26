# Notes App - API Requirements

## Goal

Create .NET C# API for Notes App that support:

- User registration
- User login
- JWT authentication
- Public endpoints for auth
- Protected endpoints for notes
- CRUD operations for notes
- PostgreSQL persistence.
- Clean Architecture
- Unit tests

## Backend Architecture

The backend must use Clean Architecture with the following projects:

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
```

## Tech Requirements

- .NET 10
- ASP.NET Web API
- EF Core
- PostgreSQL
- Docker and Docker compose

## Database Requirements

Use PostgreSQL

### Users
Required fields:

- `Id` -> PK, uuid, required
- `Name` -> text, required
- `Email` -> text, required, unique
- `PasswordHash` -> text, required
- `CreatedAt` -> timestamp, required, now() as default

### Notes
Required fields:

- `Id` -> PK, uuid, required
- `Title` -> text, required
- `Content` -> text, required
- `UserId` -> FK to Users, uuid, required
- `CreatedAt` -> timestamp, required, now() as default
- `UpdatedAt` -> timestamp, required, now() as default

## Repositories methods

Repositories interfaces will be in **Application** layer,
Repositories implementation will be in **Infrastructure** layer.

### User

- GetById
- GetByEmail
- EmailExists
- Add

### Notes

- GetById
- GetByUserId
- Add
- Update
- Delete

### PasswordHasher

- Hash
- Verify

### TokenService

- GenerateToken
---

### Authentication API

| Method | Endpoint             | Authorization | Description                        |
| ------ | -------------------- | ------------- | ---------------------------------- |
| `POST` | `/api/auth/register` | Public        | Register a new user                |
| `POST` | `/api/auth/login`    | Public        | Log in as an existing user         |
| `GET`  | `/api/auth/me`       | Protected     | Get the current authenticated user |

### Notes API

| Method   | Endpoint          | Authorization | Description                              |
| -------- | ----------------- | ------------- | ---------------------------------------- |
| `POST`   | `/api/notes`      | Protected     | Create a new note                        |
| `GET`    | `/api/notes`      | Protected     | Get all notes for the authenticated user |
| `GET`    | `/api/notes/{id}` | Protected     | Get a specific note by ID                |
| `PUT`    | `/api/notes/{id}` | Protected     | Update a specific note                   |
| `DELETE` | `/api/notes/{id}` | Protected     | Delete a specific note                   |

### Public API

| Method | Endpoint             | Authorization | Description                 |
| ------ | -------------------- | ------------- | --------------------------- |
| `GET`  | `/api/public/status` | Public        | Check if the API is running |

---

## Testing Strategy

The project will follow a Test-Driven Development approach.

The tests will cover:

1. User registration business rules.
2. User login business rules.
3. Note creation validation.
4. Note retrieval by authenticated user.
5. Note update validation.
6. Note deletion validation.
7. Prevention of unauthorized access.
8. Repository/data access behavior.
9. API endpoint responses.
10. Protected and public endpoint behavior.
