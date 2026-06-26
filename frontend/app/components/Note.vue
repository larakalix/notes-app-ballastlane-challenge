<script setup lang="ts">
import type { NoteResponse } from "~/types/note";

const props = defineProps<{
    note: NoteResponse
}>()

const emit = defineEmits<{
    remove: [id: NoteResponse['id']]
}>()

const handleDelete = () => {
    emit('remove', props.note.id)
}
</script>

<template>
    <UPageCard>
        <template #header>
            <div class="flex items-center justify-between gap-2">
                <h3 class="font-medium">
                    {{ props.note.title }}
                </h3>

                <UBadge :color="props.note.status === 'active' ? 'success' : 'neutral'" variant="subtle"
                    :label="props.note.status" />
            </div>
        </template>

        <template #body>
            <p v-if="props.note.dueDate" class="mb-2 text-xs text-muted">
                Due: {{ new Date(props.note.dueDate).toLocaleDateString() }}
            </p>

            <p class="text-sm text-muted line-clamp-3">
                {{ props.note.content }}
            </p>
        </template>

        <template #footer>
            <div class="flex justify-between gap-2 items-center w-full">
                <UButton :to="`/notes/${props.note.id}`" size="sm" color="neutral" variant="soft" label="Edit"
                    icon="i-lucide-edit-2" />

                <UButton size="sm" color="error" variant="ghost" label="Delete" icon="i-lucide-trash"
                    @click="handleDelete" />
            </div>
        </template>
    </UPageCard>
</template>
