import * as z from "zod";

type NoteStatus = "active" | "inactive";

export interface NoteResponse {
    id: string;
    title: string;
    content: string;
    status: NoteStatus;
    dueDate: string | null;
    userId: string;
    createdAt: string;
    updatedAt: string;
}

export interface CreateNotePayload {
    title: string;
    content: string;
    status: NoteStatus;
    dueDate: string | null;
}

export interface UpdateNotePayload {
    title: string;
    content: string;
    status: NoteStatus;
    dueDate: string | null;
}

export const noteSchema = z.object({
    title: z.string().min(1, "Title is required."),
    content: z.string().min(1, "Content is required."),
    status: z.enum(["active", "inactive"]).default("active"),
    dueDate: z.string().optional(),
});

export type Note = z.output<typeof noteSchema>;
