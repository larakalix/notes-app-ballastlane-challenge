<script setup lang="ts">
import type { FormSubmitEvent } from '@nuxt/ui'
import { type LoginPayload, type SignIn, signInSchema } from '~/types/auth'

const { error, login } = useAuth()

const state = reactive<Partial<SignIn>>({
  email: undefined,
  password: undefined
})

const toast = useToast()

const onSubmit = async (event: FormSubmitEvent<SignIn>) => {
  const result = signInSchema.safeParse(event.data)

  if (!result.success) {
    toast.add({ title: 'Error', description: result.error.issues[0]?.message ?? 'Invalid input.', color: 'error' })
    return
  }

  try {
    await login(result.data as LoginPayload)
    toast.add({ title: 'Success', description: 'The form has been submitted.', color: 'success' })
    await navigateTo('/notes')
  }
  catch (err: unknown) {
    toast.add({ title: 'Error', description: (err as { data?: { error?: string } })?.data?.error ?? 'Unable to create account.', color: 'error' })
  }
}
</script>

<template>
  <div class="mx-auto max-w-md px-4 py-12">
    <h1 class="text-2xl font-semibold">
      Sign in
    </h1>
    <p class="mt-2 text-sm text-muted">
      Sign in to manage your personal notes.
    </p>

    <template v-if="error">
      <AuthError :error="error" />
    </template>

    <template v-else>
      <UForm :schema="signInSchema" :state="state" class="space-y-4" @submit="onSubmit">
        <UFormField label="Email" name="email" class="w-full">
          <UInput v-model="state.email" class="w-full" />
        </UFormField>

        <UFormField label="Password" name="password" class="w-full">
          <UInput v-model="state.password" type="password" class="w-full" />
        </UFormField>

        <UButton type="submit">
          Submit
        </UButton>
      </UForm>
    </template>
  </div>
</template>
