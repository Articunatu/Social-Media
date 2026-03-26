"use client";
import React, { createContext, useState, useEffect, useContext } from "react";
import authenticationService from "../services/authentication-service";
import { AuthorizeResponse, LoginCommand } from "../models/api/authentication-models";
import { useLogin } from "../hooks/use-login";

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
    const { login: loginHook, loading, error } = useLogin();

    useEffect(() => {
        const storedToken = localStorage.getItem("jwt");
        if (storedToken) {
            setToken(storedToken);
            fetchUser(storedToken);
        }
    }, []);

    const fetchUser = async (jwt: string) => {
        try {
            const response = await authenticationService.authorize(jwt);
            setUser(response.data);
        } catch {
            setUser(null);
        }
    };

    const login = async (credentials: LoginCommand) => {
        const result = await loginHook(credentials);
        if (result.success) {
            // After successful login, get the new token and user
            const storedToken = localStorage.getItem("jwt");
            if (storedToken) {
                setToken(storedToken);
                await fetchUser(storedToken);
            }
            return true;
        } else {
            setToken(null);
            setUser(null);
            return false;
        }
    };

    const logout = async () => {
        try {
            await authenticationService.logout({ userId: user?.userId ?? "" });
        } catch {
            // Handle error if needed
        }
        setToken(null);
        setUser(null);
        localStorage.removeItem("jwt");
    };

    return (
        <AuthContext.Provider value={{ token, user, login, logout, loading, error }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);