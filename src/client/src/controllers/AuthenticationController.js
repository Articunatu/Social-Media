import { createContext, useContext } from "react";
import { useState } from "react";
import { useEffect } from "react";

const AuthenticationController = createContext();

export function useAuth() {
    return useContext(AuthenticationController);
}

export function AuthProvider({ children }) {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [userTag, setUserTag] = useState("");

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken');
        const storedUserTag = localStorage.getItem('userTag');
        if (accessToken && storedUserTag) {
            setIsLoggedIn(true);
            setUserTag(storedUserTag);
        }
    }, []);

    const setUser = (tag) => {
        setUserTag(tag);
    };

    const value = {
        isLoggedIn,
        setIsLoggedIn,
        userTag,
        setUserTag: setUser,
    };

    return <AuthenticationController.Provider value={value}>{children}</AuthenticationController.Provider>;
}