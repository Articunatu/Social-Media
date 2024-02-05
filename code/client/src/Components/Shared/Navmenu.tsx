import React from "react";
import { Link } from "react-router-dom";
import logo from "./logo.png";

interface Props {}

const Navbar = (props: Props) => {
    return (
        <nav className="fixed bottom-0 left-0 w-full bg-white border-t border-gray-200 p-4">
            <div className="container mx-auto flex items-center justify-between">
                <div className="flex items-center space-x-20">
                    <Link to="/">
                        <img src={logo} alt="" />
                    </Link>
                    <div className="hidden lg:flex">
                        <Link to="/search" className="text-black hover:text-darkBlue">
                            Search
                        </Link>
                    </div>
                </div>
                <div className="hidden lg:flex items-center space-x-6 text-black">
                    <div className="hover:text-darkBlue">Login</div>
                    <a
                        href=""
                        className="px-8 py-3 font-bold rounded text-white bg-lightGreen hover:opacity-70"
                    >
                        Signup
                    </a>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
