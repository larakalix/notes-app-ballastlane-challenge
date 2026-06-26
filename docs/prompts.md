# Prompts I used

## 1st Step: Helping agent to understand the project purpose

```text
We are in a project names Notes App.

We already have a boilerplate for frontend in nuxt and backend in .net10, so before coding, there are 2 files, so understanding them is crucial for you, read `docs/user-story.md` and `docs/api-requirements.md`.

I already managed the dependencies in every project so you should not take care about that.

For now, let's review the /backend and implement the main entities, validations, contracts, repositories, and follow the `api-requirements.md` file instructions.

Then let's add some features in `application` layer:

1. Domain entities:
- User
- Notes

2. Auth DTOs:
- Auth/RegisterRequest
- Auth/LoginRequest
- Auth/AuthResponse

3. User DTOs:
- Users/UserResponse

4. Notes DTOs:
- Notes/CreateNote
- Notes/UpdateNote
- Notes/NoteResponse

5. Interfaces for Helpers, Repositories and Services:
- IUserRepository
- INoteRepository
- IUserService
- INotesService
- IPasswordHasher
- ITokenService

6. Services for Helpers, Repositories and Services:
- UserRepository
- NoteRepository
- UserService
- NoteService
- PasswordHasher
- TokenService

We are going to manage every Entity and DTO validation using FluentApi
```

## 2nd Step:

```text
We are going to continue with implementating the services in API controllers, so the instructions for endpoints config are in `docs/api-requirements.md`, so make sure of that.

We also going to implement layers in `program.cs` file, in order to configure dependencies.
```

## 3rd Step:

```text
Now let's implement the API in frontend project, before coding, read the `docs/frontend-requirements.md` file, which has the instructions for frontend.
We are going to use TailwindCSS for styling, NuxtUI for components, every validation is going to be manage by zod.

There is also a folder structure to follow in order to have a clean components structure, make sure to break composables, types and components in each folder, to follow Nuxt best practices.
```

## 4th Step:

```text
Now that we have all the complete endpoints workflows, focus on tests implementation, do not change anything in the app behaviour unless a test exposes a real bug, so add the tests following a TDD structure, the backend already
have the projects, so review it and read the use cases in `api-requirements.md` file.
```
