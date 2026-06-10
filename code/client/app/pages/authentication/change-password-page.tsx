"use client";

import { useAuth } from '../../authentication/auth-provider';
import { LoginPage } from './login-page';
import { ChangePasswordForm } from './components/change-password-form';

export function ChangePasswordPage() {
    const auth = useAuth();

    if (!auth?.token || !auth.user) {
        return <LoginPage />;
    }

    return (
        <div className="flex min-h-screen items-center justify-center bg-base-200 p-4">
            <ChangePasswordForm userId={auth.user.userId} />
        </div>
    );
}
