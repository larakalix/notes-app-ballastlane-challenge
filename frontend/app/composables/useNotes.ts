import type {
    CreateNotePayload,
    NoteResponse,
    UpdateNotePayload,
} from "~/types/note";

export const useNotes = () => {
    const { request } = useApi();
    const { token } = useAuth();

    const notes = useState<NoteResponse[]>("notes-list", () => []);
    const loading = useState<boolean>("notes-loading", () => false);
    const error = useState<string | null>("notes-error", () => null);

    const requireToken = () => {
        if (!token.value) {
            throw new Error("You are not authenticated.");
        }

        return token.value;
    };

    const fetchNotes = async () => {
        loading.value = true;
        try {
            const result = await request<NoteResponse[]>("/api/notes", {
                token: requireToken(),
            });

            notes.value = result;
            return result;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const getNote = async (id: string) => {
        loading.value = true;
        try {
            const note = await request<NoteResponse>(`/api/notes/${id}`, {
                token: requireToken(),
            });

            return note;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const createNote = async (payload: CreateNotePayload) => {
        loading.value = true;
        try {
            const note = await request<NoteResponse>("/api/notes", {
                method: "POST",
                body: payload,
                token: requireToken(),
            });

            notes.value = [note, ...notes.value];
            return note;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const updateNote = async (id: string, payload: UpdateNotePayload) => {
        loading.value = true;
        try {
            const updated = await request<NoteResponse>(`/api/notes/${id}`, {
                method: "PUT",
                body: payload,
                token: requireToken(),
            });

            notes.value = notes.value.map((note) =>
                note.id === updated.id ? updated : note,
            );
            return updated;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const deleteNote = async (id: string) => {
        loading.value = true;
        try {
            await request<void>(`/api/notes/${id}`, {
                method: "DELETE",
                token: requireToken(),
            });

            notes.value = notes.value.filter((note) => note.id !== id);
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const removeNote = async (id: string) => {
        try {
            await deleteNote(id);
        } catch (err: unknown) {
            error.value =
                (err as { data?: { error?: string } })?.data?.error ??
                "Unable to delete note.";
        }
    };

    return {
        notes,
        loading,
        error,
        fetchNotes,
        getNote,
        createNote,
        updateNote,
        deleteNote,
        removeNote,
    };
};
