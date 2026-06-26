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
- Unit tests (xUnit, Moq, FluentAssertions)

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

### TDD

The backend must include automated tests that validate the main behaviour of the API, so that should cover the following layers:

- Application -> business logic
- Data Access -> repositories
- API endpoints
- Auth behaviour

So there are some use cases for testing purpose:

UserService:

- Register succeeds with valid data.
- Register fails when name is empty.
- Register fails when email is empty.
- Register fails when password is empty.
- Register fails when email already exists.
- Register hashes the password before saving.
- Login succeeds with valid credentials.
- Login fails when user does not exist.
- Login fails when password is invalid.
- Get current user succeeds when user exists.
- Get current user fails when user does not exist.

NoteService:

- Create note succeeds with valid data.
- Create note fails when title is empty.
- Create note fails when content is empty.
- Get all notes returns only notes for the current user.
- Get note by ID succeeds when the note belongs to the current user.
- Get note by ID fails when the note belongs to another user.
- Update note succeeds when the note belongs to the current user.
- Update note fails when the note belongs to another user.
- Delete note succeeds when the note belongs to the current user.
- Delete note fails when the note belongs to another user.

UserRepository:

- Add user.
- Get user by ID.
- Get user by email.
- Check email exists.
- Return false when email does not exist.

NoteRepository

- Add note.
- Get note by ID.
- Get notes by user ID.
- Update note.
- Delete note.
- Ensure notes are filtered by user ID.

Public Endpoints

- GET /api/public/status returns 200 OK.
- POST /api/auth/register returns success for valid data.
- POST /api/auth/login returns success for valid credentials.

Protected Endpoints

- GET /api/auth/me returns 401 Unauthorized without token.
- GET /api/notes returns 401 Unauthorized without token.
- POST /api/notes returns 401 Unauthorized without token.

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
