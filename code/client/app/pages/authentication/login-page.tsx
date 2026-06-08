"use client";

import React from "react";
import Link from "next/link";
import { useLoginForm } from "./hooks/use-login-form";

export const LoginPage: React.FC = () => {
    const { auth, error, handleSubmit, password, setPassword, setTag, tag } = useLoginForm();

    return (
        <div className="flex min-h-screen flex-col items-center justify-center bg-base-200">
            <form onSubmit={handleSubmit} className="card w-full max-w-sm bg-base-100 shadow-sm">
                <div className="card-body">
                    <h2 className="card-title">Login</h2>
                    <p className="text-sm text-base-content/70">
                        Need an account?{" "}
                        <Link href="/signup" className="link link-primary font-semibold">
                            Sign up
                        </Link>
                    </p>

                    <label className="form-control">
                        <span className="label-text mb-1 font-medium">Tag</span>
                        <input
                            type="text"
                            value={tag}
                            onChange={(event) => setTag(event.target.value)}
                            className="input input-bordered"
                            required
                            autoComplete="username"
                        />
                    </label>

                    <label className="form-control">
                        <span className="label-text mb-1 font-medium">Password</span>
                        <input
                            type="password"
                            value={password}
                            onChange={(event) => setPassword(event.target.value)}
                            className="input input-bordered"
                            required
                            autoComplete="current-password"
                        />
                    </label>

                    {error && <div className="alert alert-error py-2 text-sm">{error}</div>}

                    <button type="submit" className="btn btn-primary w-full" disabled={auth?.loading}>
                        {auth?.loading ? "Logging in..." : "Login"}
                    </button>
                </div>
            </form>
        </div>
    );
};
