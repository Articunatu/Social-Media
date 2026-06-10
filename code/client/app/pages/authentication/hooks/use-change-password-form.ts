import React from 'react';
import authenticationService from '../../../services/authentication-service';

interface ChangePasswordValues {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}

const initialValues: ChangePasswordValues = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: '',
};

export function useChangePasswordForm(userId: string) {
    const [values, setValues] = React.useState<ChangePasswordValues>(initialValues);
    const [error, setError] = React.useState<string | null>(null);
    const [success, setSuccess] = React.useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = React.useState(false);

    const handleChange = (field: keyof ChangePasswordValues) =>
        (event: React.ChangeEvent<HTMLInputElement>) => {
            setValues((current) => ({ ...current, [field]: event.target.value }));
        };

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setError(null);
        setSuccess(null);

        if (values.newPassword !== values.confirmPassword) {
            setError('New password and confirmation must match.');
            return;
        }

        setIsSubmitting(true);

        try {
            await authenticationService.changePassword({
                userId,
                oldPassword: values.oldPassword,
                newPassword: values.newPassword,
                confirmPassword: values.confirmPassword,
            });
            setValues(initialValues);
            setSuccess('Password changed.');
        } catch (caughtError: unknown) {
            setError(caughtError instanceof Error ? caughtError.message : 'Failed to change password.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return {
        error,
        handleChange,
        handleSubmit,
        isSubmitting,
        success,
        values,
    };
}
