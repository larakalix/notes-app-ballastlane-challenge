<script setup lang="ts">
import type { FormSubmitEvent } from '@nuxt/ui'
import { type Note, type NoteResponse, noteSchema } from '~/types/note'
import { toApiDueDate, toInputDueDate } from '~/utils/date-utils'

const props = defineProps<{
    note?: NoteResponse
}>()

const isEditMode = computed(() => !!props.note)

const { error, createNote, updateNote } = useNotes()

const state = reactive<Partial<Note>>({
    title: '',
    content: '',
    status: 'active',
    dueDate: ''
})

const toast = useToast()

const statusItems = computed(() => [
    { label: 'Active', value: 'active' },
    { label: 'Inactive', value: 'inactive' }
])

const onSubmit = async (event: FormSubmitEvent<Note>) => {
    const result = noteSchema.safeParse(event.data)

    if (!result.success) {
        error.value = result.error.issues[0]?.message ?? 'Invalid input.'
        return
    }

    try {
        const payload = {
            ...result.data,
            dueDate: toApiDueDate(result.data.dueDate)
        }

        if (props.note) {
            await updateNote(props.note.id, payload)
            toast.add({ title: 'Success', description: 'The note has been updated.', color: 'success' })
            await navigateTo('/notes')
        } else {
            await createNote(payload)
            state.title = ''
            state.content = ''
            state.status = 'active'
            state.dueDate = ''
            toast.add({ title: 'Success', description: 'The note has been created.', color: 'success' })
        }
    }
    catch (err: unknown) {
        error.value = (err as { data?: { error?: string } })?.data?.error
            ?? (props.note ? 'Unable to update note.' : 'Unable to create note.')
    }
}

watch(
    () => props.note,
    (newNote) => {
        if (!newNote) {
            return
        }

        Object.assign(state, {
            title: newNote.title,
            content: newNote.content,
            status: newNote.status,
            dueDate: toInputDueDate(newNote.dueDate)
        })
    },
    { immediate: true, deep: true }
)
</script>

<template>
    <section class="space-y-4 rounded-lg border border-default p-4">
        <h2 class="text-lg font-medium">
            {{ isEditMode ? 'Edit note' : 'Create note' }}
        </h2>

        <UForm :schema="noteSchema" :state="state" class="grid grid-cols-1 md:grid-cols-2 gap-4" @submit="onSubmit">
            <UFormField label="Title" name="title" class="w-full">
                <UInput v-model="state.title" class="w-full" />
            </UFormField>

            <UFormField label="Status" name="status" class="w-full">
                <USelect v-model="state.status" :items="statusItems" class="w-full" />
            </UFormField>

            <UFormField label="Due date" name="dueDate" class="w-full">
                <UInput v-model="state.dueDate" type="date" class="w-full" />
            </UFormField>

            <UFormField label="Content" name="content" class="w-full md:col-span-2">
                <UTextarea v-model="state.content" placeholder="Write your note content..." :rows="5"
                    class="w-full md:col-span-2" />
            </UFormField>

            <p v-if="error" class="text-sm text-error">
                {{ error }}
            </p>

            <footer class="md:col-span-2 flex justify-end">
                <UButton type="submit" :label="isEditMode ? 'Save changes' : 'Add note'" />
            </footer>
        </UForm>
    </section>
</template>
