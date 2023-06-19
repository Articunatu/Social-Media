import React from 'react';
import NavMenu from './views/components/menu/NavMenu';
import RouteManager from './RouteManager';

export default function App() {
  return (
    <>
      <NavMenu />
      <RouteManager />
    </>
  );
}