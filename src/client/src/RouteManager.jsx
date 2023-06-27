import React from 'react';
import { Routes, Route } from 'react-router-dom';
import StartPage from './views/pages/StartPage';
import Feed from './views/pages/Feed';
import ProfileInfo from './views/pages/ProfileInfo';
import Explore from './views/pages/Explore';
import Settings from './views/pages/Settings';
import DirectMessages from './views/pages/DirectMessages';
import SignUp from './views/pages/authentication/SignUp';
import Login from './views/pages/authentication/Login';

export default function RouteManager() {
  return (
    <Routes>
      <Route path="/" element={<StartPage />} />
      <Route path="/signup" element={<SignUp />} />
      <Route path="/login" element={<Login />} />
      <Route path="/feed" element={<Feed />} />
      <Route path="/profile/:id" element={<ProfileInfo />} />
      <Route path="/explore" element={<Explore />} />
      <Route path="/settings" element={<Settings />} />
      <Route path="/direct_messages" element={<DirectMessages />} />
    </Routes>
  );
}