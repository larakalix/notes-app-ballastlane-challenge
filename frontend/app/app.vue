<script setup lang="ts">
useHead({
  meta: [
    { name: 'viewport', content: 'width=device-width, initial-scale=1' }
  ],
  link: [
    { rel: 'icon', href: '/favicon.ico' }
  ],
  htmlAttrs: {
    lang: 'en'
  }
})

const { isAuthenticated, logout } = useAuth()

const title = 'Notes App'
const description = 'Register, log in, and manage your personal notes.'

useSeoMeta({
  title,
  description,
  ogTitle: title,
  ogDescription: description,
  twitterCard: 'summary_large_image'
})

const handleLogout = async () => {
  await logout()
  await navigateTo('/sign-in')
}
</script>

<template>
  <UApp>
    <header class="border-b border-default">
      <div class="mx-auto flex max-w-5xl items-center justify-between px-4 py-4">
        <NuxtLink to="/" class="text-lg font-semibold">
          Notes App
        </NuxtLink>

        <div class="flex items-center gap-2">
          <template v-if="!isAuthenticated">
            <UButton to="/sign-in" color="neutral" variant="ghost" label="Sign in" />
            <UButton to="/sign-up" label="Create account" />
          </template>
          <template v-else>
            <UButton to="/notes" color="neutral" variant="ghost" label="My notes" />
            <UButton color="error" variant="soft" label="Log out" @click="handleLogout" />
          </template>
        </div>
      </div>
    </header>

    <UMain>
      <NuxtPage />
    </UMain>
  </UApp>
</template>
