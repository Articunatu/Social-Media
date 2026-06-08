"use client";
import React, { useMemo, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useAuth } from "../../authentication/auth-provider";
import authenticationService from '../../services/authentication-service';
import type { LoginCommand } from '../../models/api/authentication-models';
import { SignUpField } from "./components/sign-up-field";
import {
    hasSignUpErrors,
    signUpLimits,
    toSignUpCommand,
    validateSignUp,
} from "./utils/sign-up-validation";
import type { SignUpErrors, SignUpField as SignUpFieldName, SignUpFormValues } from "./utils/sign-up-validation";

const initialValues: SignUpFormValues = {
    tag: "",
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    confirmPassword: "",
};

export const RegisterPage: React.FC = () => {
    const [values, setValues] = useState<SignUpFormValues>(initialValues);
    const [errors, setErrors] = useState<SignUpErrors>({});
    const auth = useAuth();
    const [localError, setLocalError] = useState<string | null>(null);
    const router = useRouter();
    const canSubmit = useMemo(() => !auth?.loading, [auth?.loading]);

    const handleChange =
        (field: SignUpFieldName) =>
        (event: React.ChangeEvent<HTMLInputElement>) => {
            const nextValues = { ...values, [field]: event.target.value };
            setValues(nextValues);
            setErrors(validateSignUp(nextValues));
            setLocalError(null);
        };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLocalError(null);
        if (!auth) return;

        const validationErrors = validateSignUp(values);
        setErrors(validationErrors);

        if (hasSignUpErrors(validationErrors)) {
            return;
        }

        try {
            const cmd = toSignUpCommand(values);
            const signUpResp = await authenticationService.signUp(cmd);
            if (signUpResp.status >= 200 && signUpResp.status < 300) {
                const loginCmd: LoginCommand = { tag: cmd.tag, password: cmd.password };
                const loggedIn = await auth.login(loginCmd);
                if (loggedIn) {
                    router.push("/");
                    return;
                }
            }
            setLocalError(auth.error ?? "Registration failed");
        } catch (err) {
            const message = err instanceof Error ? err.message : "Registration failed";
            setLocalError(message);
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 px-4 py-10 text-black">
            <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-5xl items-center">
                <div className="grid w-full gap-8 lg:grid-cols-[0.85fr_1.15fr] lg:items-center">
                    <section className="space-y-4">
                        <p className="text-sm font-semibold uppercase text-blue-700">Social Media</p>
                        <h1 className="text-4xl font-bold leading-tight sm:text-5xl">Create your account</h1>
                        <p className="max-w-md text-base text-gray-600">
                            Join the conversation and continue straight into your feed.
                        </p>
                    </section>

                    <form onSubmit={handleSubmit} className="w-full rounded border border-gray-200 bg-white p-6 shadow-sm sm:p-8" noValidate>
                        <div className="mb-6 flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
                            <div>
                                <h2 className="text-2xl font-bold">Sign up</h2>
                                <p className="text-sm text-gray-600">Already have an account? <Link href="/" className="font-semibold text-blue-700 hover:text-blue-900">Log in</Link></p>
                            </div>
                        </div>

                        <div className="grid gap-4 sm:grid-cols-2">
                            <SignUpField
                                id="firstName"
                                label="First name"
                                value={values.firstName}
                                onChange={handleChange("firstName")}
                                error={errors.firstName}
                                maxLength={signUpLimits.firstNameMaxLength}
                                autoComplete="given-name"
                            />
                            <SignUpField
                                id="lastName"
                                label="Last name"
                                value={values.lastName}
                                onChange={handleChange("lastName")}
                                error={errors.lastName}
                                maxLength={signUpLimits.lastNameMaxLength}
                                autoComplete="family-name"
                            />
                        </div>

                        <div className="mt-4 grid gap-4">
                            <SignUpField
                                id="tag"
                                label="Tag"
                                value={values.tag}
                                onChange={handleChange("tag")}
                                error={errors.tag}
                                maxLength={signUpLimits.tagMaxLength}
                                autoComplete="username"
                            />
                            <SignUpField
                                id="email"
                                label="Email"
                                type="email"
                                value={values.email}
                                onChange={handleChange("email")}
                                error={errors.email}
                                maxLength={signUpLimits.emailMaxLength}
                                autoComplete="email"
                            />
                            <SignUpField
                                id="password"
                                label="Password"
                                type="password"
                                value={values.password}
                                onChange={handleChange("password")}
                                error={errors.password}
                                maxLength={signUpLimits.passwordMaxLength}
                                autoComplete="new-password"
                            />
                            <SignUpField
                                id="confirmPassword"
                                label="Confirm password"
                                type="password"
                                value={values.confirmPassword}
                                onChange={handleChange("confirmPassword")}
                                error={errors.confirmPassword}
                                maxLength={signUpLimits.passwordMaxLength}
                                autoComplete="new-password"
                            />
                        </div>

                {(localError || (auth && auth.error)) && (
                            <div className="mt-4 rounded border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{localError || auth?.error}</div>
                )}

                        <button
                            type="submit"
                            className="mt-6 w-full rounded bg-blue-700 px-4 py-3 font-semibold text-white transition hover:bg-blue-800 disabled:cursor-not-allowed disabled:opacity-60"
                            disabled={!canSubmit}
                        >
                            {auth?.loading ? "Creating account..." : "Create account"}
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};
