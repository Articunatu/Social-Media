import React, { useState } from "react";
import { Outlet } from "react-router";
import Navmenu from "./Components/Shared/Navmenu";
import AuthPage from "./Components/Authentification";

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(true);

  return (
    <div className="flex flex-col min-h-screen">
      {!isLoggedIn && <AuthPage />}
      {isLoggedIn && <Navmenu />}
      <div className="flex-grow">
        <Outlet />
      </div>
    </div>
  );
}
