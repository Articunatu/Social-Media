"use client";

import React from "react";
import { RegisterForm } from "./components/register-form";

export const RegisterPage: React.FC = () => (
    <div className="min-h-screen bg-base-200 px-4 py-10 text-base-content">
        <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-5xl items-center">
            <div className="grid w-full gap-8 lg:grid-cols-[0.85fr_1.15fr] lg:items-center">
                <section className="space-y-4">
                    <p className="text-sm font-semibold uppercase text-primary">Social Media</p>
                    <h1 className="text-4xl font-bold leading-tight sm:text-5xl">Create your account</h1>
                    <p className="max-w-md text-base text-base-content/70">
                        Join the conversation and continue straight into your feed.
                    </p>
                </section>

                <RegisterForm />
            </div>
        </div>
    </div>
);
