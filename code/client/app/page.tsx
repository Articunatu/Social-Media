"use client";
import React from 'react';
import FeedPage from './pages/posts/feed-page';
import { useAuth } from './authentication/auth-provider';
import { LoginPage } from './pages/authentication/login-page';

const HomePage: React.FC = () => {
  const auth = useAuth();

  if (!auth || !auth.token) {
    return <LoginPage />;
  }
  return <FeedPage />;
};

export default HomePage;
