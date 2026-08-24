"use client";

import React, { useMemo, useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../../../authentication/auth-provider";
import authenticationService from "../../../services/authentication-service";
import type { LoginCommand } from "../../../models/api/authentication-models";
import {
    hasSignUpErrors,
    toSignUpCommand,
    validateSignUp,
} from "../validations/sign-up-validation";
import type {
    SignUpErrors,
    SignUpField,
    SignUpFormValues,
} from "../validations/sign-up-validation";

const initialValues: SignUpFormValues = {
    tag: "",
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    confirmPassword: "",
};

export function useRegisterForm() {
    const [values, setValues] = useState<SignUpFormValues>(initialValues);
    const [errors, setErrors] = useState<SignUpErrors>({});
    const [localError, setLocalError] = useState<string | null>(null);
    const auth = useAuth();
    const router = useRouter();
    const canSubmit = useMemo(() => !auth?.loading, [auth?.loading]);

    const handleChange =
        (field: SignUpField) =>
        (event: React.ChangeEvent<HTMLInputElement>) => {
            const nextValues = { ...values, [field]: event.target.value };
            setValues(nextValues);
            setErrors(validateSignUp(nextValues));
            setLocalError(null);
        };

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setLocalError(null);

        if (!auth) {
            return;
        }

        const validationErrors = validateSignUp(values);
        setErrors(validationErrors);

        if (hasSignUpErrors(validationErrors)) {
            return;
        }

        try {
            const command = toSignUpCommand(values);
            const response = await authenticationService.signUp(command);

            if (response.status < 200 || response.status >= 300) {
                setLocalError(auth.error ?? "Registration failed");
                return;
            }

            const loginCommand: LoginCommand = { tag: command.tag, password: command.password };
            const loggedIn = await auth.login(loginCommand);

            if (!loggedIn) {
                setLocalError(auth.error ?? "Registration failed");
                return;
            }

            router.push("/");
        } catch (error: unknown) {
            setLocalError(error instanceof Error ? error.message : "Registration failed");
        }
    };

    return {
        auth,
        canSubmit,
        errors,
        handleChange,
        handleSubmit,
        localError,
        values,
    };
}
