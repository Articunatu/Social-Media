"use client";
import React from 'react';
import FeedPage from './pages/posts/feed-page';
import { useAuth } from './components/auth-provider';
import { LoginPage } from './pages/authentication/login-page';

const HomePage: React.FC = () => {
  const auth = useAuth();

  if (!auth || !auth.token) {
    return <LoginPage />;
  }

  const { user, token } = auth;

  return <FeedPage />;
};

export default HomePage;