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
            className="rounded border border-black bg-white px-4 py-2 text-sm font-semibold text-black transition hover:bg-gray-100 disabled:cursor-not-allowed disabled:opacity-60"
        >
            {auth.loading ? "Logging out..." : "Logout"}
        </button>
    );
};
