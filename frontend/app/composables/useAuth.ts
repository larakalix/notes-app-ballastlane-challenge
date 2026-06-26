import type {
    AuthResponse,
    LoginPayload,
    RegisterPayload,
    UserResponse,
} from "~/types/auth";

export const useAuth = () => {
    const { request } = useApi();

    const token = useCookie<string | null>("notes_token", {
        default: () => null,
        sameSite: "lax",
        path: "/",
        maxAge: 60 * 60 * 24 * 7,
    });

    const user = useState<UserResponse | null>("auth-user", () => null);
    const loading = useState<boolean>("auth-loading", () => false);
    const error = useState<string | null>("auth-error", () => null);

    const isAuthenticated = computed(() => Boolean(token.value));

    const register = async (body: RegisterPayload) => {
        loading.value = true;
        try {
            const response = await request<AuthResponse>("/api/auth/register", {
                method: "POST",
                body,
            });

            token.value = response.token;
            user.value = response.user;

            return response;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const login = async (body: LoginPayload) => {
        loading.value = true;

        try {
            const response = await request<AuthResponse>("/api/auth/login", {
                method: "POST",
                body,
            });

            token.value = response.token;
            user.value = response.user;

            return response;
        } catch (err) {
            error.value = (err as Error).message;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const fetchMe = async () => {
        if (!token.value) {
            user.value = null;
            return null;
        }

        const me = await request<UserResponse>("/api/auth/me", {
            token: token.value,
        });

        user.value = me;
        return me;
    };

    const logout = () => {
        token.value = null;
        user.value = null;
    };

    return {
        token,
        user,
        loading,
        isAuthenticated,
        error,
        register,
        login,
        fetchMe,
        logout,
    };
};
