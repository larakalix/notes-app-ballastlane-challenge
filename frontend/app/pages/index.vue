<script setup lang="ts">
const { isAuthenticated } = useAuth()
</script>

<template>
  <div class="mx-auto grid max-w-5xl gap-6 px-4 py-12">
    <h1 class="text-3xl font-bold">
      Your personal notes, in one place.
    </h1>

    <p class="text-lg text-muted">
      Create an account, sign in, and manage your notes securely.
    </p>

    <div class="flex flex-wrap gap-3">
      <template v-if="!isAuthenticated">
        <UButton to="/sign-up" label="Create account" />

        <UButton to="/sign-in" color="neutral" variant="soft" label="Sign in" />
      </template>
      <template v-else>
        <UButton to="/notes" color="neutral" variant="ghost" label="Go to notes" />
      </template>
    </div>
  </div>
</template>
