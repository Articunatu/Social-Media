"use client";

import { useQueryClient } from "@tanstack/react-query";
import { useAuth } from "./auth-provider";

export const LogoutButton = () => {
    const auth = useAuth();
    const queryClient = useQueryClient();

    if (!auth?.user) {
        return null;
    }

    const handleLogout = async () => {
        await auth.logout();
        queryClient.clear();
    };

    return (
        <button
            type="button"
            onClick={handleLogout}
            disabled={auth.loading}
            className="btn btn-outline btn-sm"
        >
            {auth.loading ? "Logging out..." : "Logout"}
        </button>
    );
};
