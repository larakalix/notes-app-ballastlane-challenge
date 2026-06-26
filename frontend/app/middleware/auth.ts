export default defineNuxtRouteMiddleware(async () => {
  const auth = useAuth()

  if (!auth.isAuthenticated.value) {
    return navigateTo('/sign-in')
  }

  if (!auth.user.value) {
    try {
      await auth.fetchMe()
    }
    catch {
      auth.logout()
      return navigateTo('/sign-in')
    }
  }
})
