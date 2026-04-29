"use client";
import React, { useState } from "react";
import { useLogin } from "../../hooks/use-login";

export const LoginPage: React.FC = () => {
    const [tag, setTag] = useState("");
    const [password, setPassword] = useState("");
    const { login, loading, error } = useLogin();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await login({ tag, password });
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow-md w-80">
                <h2 className="text-xl font-bold mb-4">Login</h2>
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
                {error && <div className="text-red-500 mb-2">{error}</div>}
                <button
                type="submit"
                className="w-full bg-blue-600 text-white py-2 rounded font-semibold"
                disabled={loading}
                >
                {loading ? "Logging in..." : "Login"}
                </button>
            </form>
        </div>
    );
};