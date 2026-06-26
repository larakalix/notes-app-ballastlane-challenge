<script setup lang="ts">
definePageMeta({
  middleware: ['auth']
})

const { notes, loading, error, fetchNotes, removeNote } = useNotes()

await fetchNotes()
</script>

<template>
  <div class="mx-auto grid max-w-4xl gap-8 px-4 py-8">
    <section>
      <h1 class="text-2xl font-semibold">
        My notes
      </h1>

      <p class="mt-1 text-sm text-muted">
        Create and manage your notes.
      </p>
    </section>

    <NoteForm />

    <section class="space-y-3">
      <h2 class="text-lg font-medium">
        Notes list
      </h2>

      <div v-if="loading" class="flex items-center gap-4">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-2">
          <USkeleton v-for="i in 6" :key="i" class="h-32 w-full" />
        </div>
      </div>

      <UEmpty v-else-if="notes.length === 0" icon="i-lucide-file" title="You do not have notes yet."
        description="Create your first note using the form above." />

      <UPageGrid :ui="{
        base: 'gap-4'
      }">
        <Note v-for="note in notes" :key="note.id" :note="note" @remove="removeNote" />
      </UPageGrid>
    </section>
  </div>
</template>
