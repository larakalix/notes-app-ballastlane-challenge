<script setup lang="ts">
import type { NoteResponse } from '~/types/note'

definePageMeta({
  middleware: ['auth']
})

const route = useRoute()
const noteId = computed(() => String(route.params.id ?? ''))

const { getNote, updateNote } = useNotes()

const state = reactive<NoteResponse>({
  id: '',
  title: '',
  content: '',
  status: 'active',
  dueDate: null,
  userId: '',
  createdAt: new Date().toISOString(),
  updatedAt: new Date().toISOString()
})

try {
  const note = await getNote(noteId.value)

  Object.assign(state, note);
}
catch {
  await navigateTo('/notes')
}
</script>

<template>
  <div class="mx-auto max-w-3xl px-4 py-8">
    <h1 class="text-2xl font-semibold">
      Edit note
    </h1>
    <p class="mt-1 text-sm text-muted">
      Update your note and save changes.
    </p>

    <template v-if="state">
      <NoteForm :note="state" />
    </template>
  </div>
</template>
