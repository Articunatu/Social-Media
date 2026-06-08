"use client";
import React, { createContext, useState, useEffect, useContext } from "react";
import authenticationService from "../services/authentication-service";
import type { AuthorizeResponse, LoginCommand } from "../models/api/authentication-models";
import { useLogin } from "./hooks/use-login";

interface AuthContextType {
    token: string | null;
    user: AuthorizeResponse | null;
    login: (credentials: LoginCommand) => Promise<boolean>;
    logout: () => Promise<void>;
    loading: boolean;
    error: string | null;
}

const AuthContext = createContext<AuthContextType | null>(null);


export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<AuthorizeResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const { login: loginHook } = useLogin();

    useEffect(() => {
        const storedToken = localStorage.getItem("jwt");
        if (!storedToken) {
            setLoading(false);
            return;
        }

        setToken(storedToken);
        fetchUser(storedToken);
    }, []);

    const fetchUser = async (jwt: string) => {
        setLoading(true);
        try {
            const response = await authenticationService.authorize(jwt);
            setUser(response.data);
            setLoading(false);
        } catch {
            setUser(null);
            setToken(null);
            localStorage.removeItem("jwt");
            setLoading(false);
            setError("Failed to fetch user info");
        }
    };

    const login = async (credentials: LoginCommand) => {
        setLoading(true);
        setError(null);
        const result = await loginHook(credentials);

        if (!result.success) {
            setToken(null);
            setUser(null);
            setLoading(false);
            setError(result.error || "Login failed");
            return false;
        }

        const storedToken = localStorage.getItem("jwt");
        if (!storedToken) {
            setLoading(false);
            return true;
        }

        setToken(storedToken);
        await fetchUser(storedToken);
        return true;
    };

    const logout = async () => {
        setLoading(true);
        setError(null);

        try {
            if (user?.userId) {
                await authenticationService.logout({ userId: user.userId });
            }
        } catch (error) {
            setError("Logout failed on the server. Your local session was cleared.");
            console.error(error);
        } finally {
            setToken(null);
            setUser(null);
            localStorage.removeItem("jwt");
            setLoading(false);
        }
    };

    return (
        <AuthContext.Provider value={{ token, user, login, logout, loading, error }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
