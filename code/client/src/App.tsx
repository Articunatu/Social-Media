import React from "react";
import { Outlet } from "react-router";
import Navbar from "./Components/Shared/Navbar";
import Navmenu from "./Components/Shared/Navmenu";
import NavbarLeft from "./Components/Shared/NavbarLeft";
import NavbarRight from "./Components/Shared/NavbarRight";

export default function App() {
  return (
    <>
      <Navbar/>
      <Outlet/>
      <Navmenu/>
      <NavbarLeft/>
      <NavbarRight/>
    </>
  )
}