import { useState } from "react";
import authenticationService from "../services/authentication-service";
import { LoginCommand } from "../models/api/authentication-models";

export interface LoginResult {
    success: boolean;
    error?: string;
}

export function useLogin() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const login = async (credentials: LoginCommand): Promise<LoginResult> => {
        setLoading(true);
        setError(null);
        try {
            const response = await authenticationService.login(credentials);
            setLoading(false);

            if (response.status === 401 || response.data?.error === "Unauthorized") 
                return { success: false, error: "Unauthorized: Invalid username or password" };

            if (response.data && response.data.accessToken) 
                localStorage.setItem("jwt", response.data.accessToken);

            return { success: true };
        } catch (err: unknown) {
            let errorMessage = "Something went wrong. Please try again.";
            if (err instanceof Error) 
                errorMessage = err.message || errorMessage;
            setError(errorMessage);
            setLoading(false);
            return { success: false, error: errorMessage };
        }
    };
    return { login, loading, error };
}