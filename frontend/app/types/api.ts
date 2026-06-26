export type ApiMethod = "GET" | "POST" | "PUT" | "DELETE";

export type ApiOptions = {
    method?: ApiMethod;
    body?: unknown;
    token?: string | null;
};
