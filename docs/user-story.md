# Notes App - User Story

## Goal

Build a web application where users can register, log in, and manage their notes. The app will include:

- A Nuxt frontend.
- An ASP.NET Web API backend.
- A PostgreSQL database.
- Authentication.
- CRUD operations for notes.
- Protected and public API endpoints.
- Docker Compose for local dev.

## Main User Story

As a user (registered), I want to log in to a personal notes application so that I can create, view, update, and delete my own notes.

## User Flow

- A guest user opens the application.
- The guest user can register in a new account, or can log in with credentials.
- After login, the user receives access to protected features.
- The authenticated user can view their notes, create, a new note, edit and/or delete an existing one.

## Business Rules

- A user must provide a name, email, and password to register.
- User emails must be unique.
- Passwords must be stored as hashes, not plain text.
- A user must log in before managing notes.
- A note must have a title and content.
- Each note belongs to one user.
- Users can only manage their own notes.

## Acceptance Criteria

### Authentication

- A guest user can register.
- A registered user can log in.
- A successful login returns an authentication token.
- A logged-in user can access protected pages.
- A logged-out user cannot access protected pages.

### Notes

- An authenticated user can create a note.
- An authenticated user can view a list of their notes.
- An authenticated user can view one note.
- An authenticated user can update a note.
- An authenticated user can delete a note.
- A user cannot access another user's notes.

### Project

- The frontend is built with Nuxt.
- The backend is built with ASP.NET Web API.
- The database is PostgreSQL.
- The project runs locally using Docker Compose.

## Definition of Done

The project is considered complete when:

1. Users can register and log in.
2. User data is stored in PostgreSQL.
3. Authenticated users can create, read, update, and delete their own notes.
4. Users cannot access notes that belong to another user.
5. Public and protected API endpoints are implemented.
6. The backend follows Clean Architecture principles.
7. The application includes a data access layer.
8. The application includes a business logic layer.
9. Unit tests are implemented for the main application components.
10. The Nuxt frontend can interact with the backend API.
11. The project can run locally using Docker Compose.

