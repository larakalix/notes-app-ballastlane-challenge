import * as z from "zod";

export interface UserResponse {
    id: string;
    name: string;
    email: string;
    createdAt: string;
}

export interface AuthResponse {
    token: string;
    user: UserResponse;
}

export interface RegisterPayload {
    name: string;
    email: string;
    password: string;
}

export interface LoginPayload {
    email: string;
    password: string;
}

export const signInSchema = z.object({
    email: z.string().email("Invalid email"),
    password: z.string().min(8, "Must be at least 8 characters"),
});

export type SignIn = z.output<typeof signInSchema>;

export const signUpSchema = z.object({
    name: z.string().min(1, "Name is required."),
    email: z.string().email("Invalid email"),
    password: z.string().min(8, "Must be at least 8 characters"),
});

export type SignUp = z.output<typeof signUpSchema>;
