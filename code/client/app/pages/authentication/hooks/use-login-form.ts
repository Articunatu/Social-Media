"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../../../authentication/auth-provider";

export function useLoginForm() {
    const [tag, setTag] = useState("");
    const [password, setPassword] = useState("");
    const [localError, setLocalError] = useState<string | null>(null);
    const auth = useAuth();
    const router = useRouter();

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setLocalError(null);

        if (!auth) {
            return;
        }

        const success = await auth.login({ tag, password });

        if (!success) {
            setLocalError(auth.error || "Login failed");
            return;
        }

        router.push("/");
    };

    return {
        auth,
        error: localError || auth?.error,
        handleSubmit,
        password,
        setPassword,
        setTag,
        tag,
    };
}
