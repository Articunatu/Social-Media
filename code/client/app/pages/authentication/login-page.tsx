"use client";
import React, { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useAuth } from "../../components/auth-provider";

export const LoginPage: React.FC = () => {
    const [tag, setTag] = useState("");
    const [password, setPassword] = useState("");
    const auth = useAuth();
    const [localError, setLocalError] = useState<string | null>(null);
    const router = useRouter();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLocalError(null);
        if (!auth) return;
        const success = await auth.login({ tag, password });
        if (success) {
            router.push("/");
            return;
        } 
        setLocalError(auth.error || "Login failed");
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow-md w-80">
                <h2 className="text-xl font-bold mb-4">Login</h2>
                <p className="mb-4 text-sm text-gray-600">
                    Need an account? <Link href="/signup" className="font-semibold text-blue-700 hover:text-blue-900">Sign up</Link>
                </p>
                <div className="mb-4">
                    <label htmlFor="tag" className="block mb-1 font-medium">
                        Tag
                    </label>
                    <input
                        id="tag"
                        type="text"
                        value={tag}
                        onChange={(e) => setTag(e.target.value)}
                        className="w-full border px-3 py-2 rounded"
                        required
                        autoComplete="username"
                    />
                </div>
                <div className="mb-4">
                    <label htmlFor="password" className="block mb-1 font-medium">
                        Password
                    </label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="w-full border px-3 py-2 rounded"
                        required
                        autoComplete="current-password"
                    />
                </div>
                {(localError || (auth && auth.error)) && (
                    <div className="text-red-500 mb-2">{localError || auth?.error}</div>
                )}
                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded font-semibold"
                    disabled={auth?.loading}
                >
                    {auth?.loading ? "Logging in..." : "Login"}
                </button>
            </form>
        </div>
    );
};
