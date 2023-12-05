import React from "react";
import { Link, useMatch, useResolvedPath } from "react-router-dom";
import { useAuth } from "../../../controllers/AuthenticationController";
// import { useEffect, useState } from "react";
// import axios from "axios";

export default function NavMenu() {
  const { isLoggedIn, userTag } = useAuth();

  return (
    <nav className="nav">
      <Link to="/" className="site-title">
        Home
      </Link>
      <ul>
        {isLoggedIn ? (
          <>
            <CustomLink to="/feed">Feed</CustomLink>
            <CustomLink to="/about">About</CustomLink>
            <li>{userTag}</li>
          </>
        ) : (
          <CustomLink to="/login">Login</CustomLink>
        )}
      </ul>
    </nav>
  );
}

function CustomLink({ to, children, ...props }) {
  const resolvedPath = useResolvedPath(to)
  const isActive = useMatch({ path: resolvedPath.pathname, end: true })

  return (
    <li className={isActive ? "active" : ""}>
      <Link to={to} {...props}>
        {children}
      </Link>
    </li>
  )
}