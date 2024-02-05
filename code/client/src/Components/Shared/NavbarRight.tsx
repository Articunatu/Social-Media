import React from "react";
import { Link } from "react-router-dom";
import logo from "./logo.png";

interface Props {}

const NavbarRight = (props: Props) => {
    return (
        <nav className="fixed right-0 top-0 h-full bg-white border-l border-gray-200 p-4">
            <div className="container mx-auto flex flex-col justify-between h-full">
                <div>
                    <Link to="/">
                        <img src={logo} alt="" />
                    </Link>
                    <div className="hidden lg:block">
                        <Link to="/search" className="text-black hover:text-darkBlue">
                            Search
                        </Link>
                    </div>
                </div>
                <div className="flex flex-col items-center lg:items-end">
                    <div className="hover:text-darkBlue">Login</div>
                    <a
                        href=""
                        className="px-8 py-3 mt-4 font-bold rounded text-white bg-lightGreen hover:opacity-70"
                    >
                        Signup
                    </a>
                </div>
            </div>
        </nav>
    );
};

export default NavbarRight;
