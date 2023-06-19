import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Feed from './views/pages/Feed';
import ProfileInfo from './views/pages/ProfileInfo';
import Explore from './views/pages/Exploring';
import Settings from './views/pages/Settings';
import DirectMessages from './views/pages/DirectMessages';

export default function RouteManager() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Feed />} />
        <Route path="/profile/:id" element={<ProfileInfo />} />
        <Route path="/explore" element={<Explore />} />
        <Route path="/settings" element={<Settings />} />
        <Route path="/direct_messages" element={<DirectMessages />} />
      </Routes>
    </Router>
  );
}