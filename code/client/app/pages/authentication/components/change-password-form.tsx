"use client";

import { useChangePasswordForm } from '../hooks/use-change-password-form';

interface ChangePasswordFormProps {
    userId: string;
}

export function ChangePasswordForm({ userId }: ChangePasswordFormProps) {
    const {
        error,
        handleChange,
        handleSubmit,
        isSubmitting,
        success,
        values,
    } = useChangePasswordForm(userId);

    return (
        <form className="card w-full max-w-md border border-base-300 bg-base-100 shadow-sm" onSubmit={handleSubmit}>
            <div className="card-body">
                <h1 className="card-title text-2xl">Change password</h1>

                <label className="form-control">
                    <span className="label-text mb-1 font-medium">Current password</span>
                    <input
                        autoComplete="current-password"
                        className="input input-bordered"
                        onChange={handleChange('oldPassword')}
                        required
                        type="password"
                        value={values.oldPassword}
                    />
                </label>

                <label className="form-control">
                    <span className="label-text mb-1 font-medium">New password</span>
                    <input
                        autoComplete="new-password"
                        className="input input-bordered"
                        maxLength={99}
                        minLength={8}
                        onChange={handleChange('newPassword')}
                        required
                        type="password"
                        value={values.newPassword}
                    />
                </label>

                <label className="form-control">
                    <span className="label-text mb-1 font-medium">Confirm new password</span>
                    <input
                        autoComplete="new-password"
                        className="input input-bordered"
                        maxLength={99}
                        minLength={8}
                        onChange={handleChange('confirmPassword')}
                        required
                        type="password"
                        value={values.confirmPassword}
                    />
                </label>

                {error && <div className="alert alert-error py-2 text-sm">{error}</div>}
                {success && <div className="alert alert-success py-2 text-sm">{success}</div>}

                <button className="btn btn-primary w-full" disabled={isSubmitting} type="submit">
                    {isSubmitting ? 'Changing password...' : 'Change password'}
                </button>
            </div>
        </form>
    );
}
