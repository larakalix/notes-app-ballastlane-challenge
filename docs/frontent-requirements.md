# Notes App - Frontend Requirements

## Goal

Create a Nuxt frontend for Nots App that support:

- Register
- Login
- Log out
- View, create, edit and delete notes

## The frontend should communicate with Backend API

- Nuxt (Vue)
- TypeScript
- zod
- TailwindCSS
- Nuxt UI
- Docker

## Pages

```txt
/
    Landing page
/sign-in
    User login page
/sign-up
    User register page
/notes
    Protected notes by user
/notes/[id]
    Protected note page
```

## Frontend Structure

```txt
frontend/
    pages/
        index.vue
        register.vue
        login.vue
        notes/
            index.vue
            [id].vue
    composables/
        useApi.ts
        useAuth.ts
        useNotes.ts
    middleware/
        auth.ts
    types/
        auth.ts
        note.ts
```

## Docker Requirement

The frontend should run in its own Docker container.
