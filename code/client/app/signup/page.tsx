"use client";

import React from "react";
import { useAuth } from "../authentication/auth-provider";
import { RegisterPage } from "../pages/authentication/register-page";
import FeedPage from "../pages/posts/feed-page";

const SignUpPage: React.FC = () => {
    const auth = useAuth();

    if (auth?.token) {
        return <FeedPage />;
    }

    return <RegisterPage />;
};

export default SignUpPage;
