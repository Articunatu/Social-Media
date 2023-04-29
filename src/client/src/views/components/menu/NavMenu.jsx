import { Link, useMatch, useResolvedPath } from "react-router-dom"

export default function Navbar() {
  return (
    <nav className="nav">
      <Link to="/" className="site-title">
        Site Name
      </Link>
      <ul>
        <CustomLink to="/pricing">Pricing</CustomLink>
        <CustomLink to="/about">About</CustomLink>
      </ul>
    </nav>
  )
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

// import AppRoutes from "./AppRoutes";
// import { useRef } from "react";
// import React from "react"

// export default function NavMenu() {

//     const navRef = useRef();

//     const showNavbar = () => {
//         navRef.current.classList.toggle("responsive_nav");
//     }

//     return (
//     <header>
//         <h3>Logo</h3>
//         <nav >
//             <a href="/#">Feed</a>
//             <a href="/#">Profile</a>
//             <a href="/#">Explore</a>
//             <a href="/#">Settings</a>
//             <button className="nav-btn nav-close-btn" >
//                 Test
//             </button>
//         </nav>
//         <nav>
//             <button className="nav-btn nav-close-btn">
//                 Go To
//             </button>
//         </nav>
//     </header>
//     )
// };


{/* <div className="menu_container">
          <p>Menyerna</p>
            <li>
              <ul>{AppRoutes.Feed}</ul>
              <ul>{AppRoutes.Profile}</ul>
              <ul>{AppRoutes.Explore}</ul>
              <ul>{AppRoutes.Settings}</ul>
            </li>
        </div> */}