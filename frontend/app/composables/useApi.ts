import type { ApiOptions } from "~/types/api";

export const useApi = () => {
    const config = useRuntimeConfig();

    const request = async <T>(
        path: string,
        options: ApiOptions = {},
    ): Promise<T> => {
        const baseURL = process.server
            ? config.apiBaseServer
            : config.public.apiBase;

        return await $fetch<T>(path, {
            baseURL,
            method: options.method,
            body: options.body ? JSON.stringify(options.body) : undefined,
            headers: options.token
                ? {
                      Authorization: `Bearer ${options.token}`,
                  }
                : undefined,
        });
    };

    return {
        request,
    };
};
