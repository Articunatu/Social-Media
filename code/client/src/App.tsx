import React, { useState } from "react";
import { Outlet } from "react-router";
import Navmenu from "./Components/Shared/Navmenu";
import AuthPage from "./Components/Authentification";

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  return (
    <>
      {!isLoggedIn && <AuthPage />}
      {isLoggedIn && <Navmenu />}
      <Outlet/>
    </>
  );
}
