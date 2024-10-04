import React from "react";
import { Outlet } from "react-router";
import NavbarLeft from "./Components/Shared/NavbarLeft";
import NavbarRight from "./Components/Shared/NavbarRight";

export default function App() {
  return (
    <div className="relative min-h-screen flex">
      <div className="w-1/5">
        <NavbarLeft />
      </div>
      <div className="flex-1 flex justify-center">
        <div className="items-center h-screen bg-black text-white font-sans w-full max-w-4xl">
          <Outlet />
        </div>
      </div>
      <div className="w-1/5">
        <NavbarRight />
      </div>
    </div>
  );
}
