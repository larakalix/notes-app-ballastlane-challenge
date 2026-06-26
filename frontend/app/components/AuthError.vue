<script setup lang="ts">
const props = defineProps<{
    error: string
}>()
</script>

<template>
    <div class="rounded-lg bg-error/10 my-2 p-4 text-sm text-error">
        {{ props.error }}
    </div>
</template>