"use client";
import React, { createContext, useState, useEffect, useContext } from "react";
import authenticationService from "../services/authentication-service";
import { AuthorizeResponse, LoginCommand } from "../models/api/authentication-models";

interface AuthContextType {
    token: string | null;
    user: AuthorizeResponse | null;
    login: (credentials: LoginCommand) => Promise<boolean>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(null);    
    const [user, setUser] = useState<AuthorizeResponse | null>(null);

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
        } catch (error) {
            setUser(null);
        }
    };

    const login = async (credentials : LoginCommand) => {
        try {
            const response = await authenticationService.login(credentials);
            const jwt = response.data.accessToken;
            setToken(jwt);
            localStorage.setItem("jwt", jwt);
            fetchUser(jwt);
            return true;
        } catch (error) {
            setToken(null);
            setUser(null);
            return false;
        }
    };

    const logout = async () => {
        try {
            await authenticationService.logout({ userId: user?.userId ?? "" }); 
        } catch (error) {
            // Handle error if needed
        }
        setToken(null);
        setUser(null);
        localStorage.removeItem("jwt");
    };

    return (
        <AuthContext.Provider value={{ token, user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);