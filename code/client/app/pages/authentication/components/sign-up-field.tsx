import React from "react";
import type { SignUpField as SignUpFieldId } from "../validations/sign-up-validation";

type SignUpFieldProps = {
    id: SignUpFieldId;
    label: string;
    value: string;
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    error?: string;
    type?: string;
    maxLength?: number;
    autoComplete?: string;
};

export const SignUpField = ({
    id,
    label,
    value,
    onChange,
    error,
    type = "text",
    maxLength,
    autoComplete,
}: SignUpFieldProps) => (
    <div>
        <label htmlFor={id} className="label py-1">
            <span className="label-text font-semibold text-gray-800">{label}</span>
        </label>
        <input
            id={id}
            name={id}
            type={type}
            value={value}
            onChange={onChange}
            maxLength={maxLength}
            autoComplete={autoComplete}
            aria-invalid={!!error}
            aria-describedby={error ? `${id}-error` : undefined}
            className="input input-bordered w-full"
            required
        />
        {error && (
            <p id={`${id}-error`} className="mt-1 text-sm text-error">
                {error}
            </p>
        )}
    </div>
);
