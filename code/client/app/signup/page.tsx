"use client";

import React from "react";
import { RegisterPage } from "../pages/authentication/register-page";
import { useAuth } from "../components/auth-provider";
import FeedPage from "../pages/posts/feed-page";

const SignUpPage: React.FC = () => {
    const auth = useAuth();

    if (auth?.token) {
        return <FeedPage />;
    }

    return <RegisterPage />;
};

export default SignUpPage;
