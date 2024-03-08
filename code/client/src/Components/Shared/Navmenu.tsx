import React from "react";
import { Link } from "react-router-dom";
import logo from "./logo.png";
import Auth from "../Authentification"

interface Props {}

const Navbar = (props: Props) => {
    return (
        <div className="flex">
            {/* Left Nav Menu */}
            <nav className="fixed left-0 top-0 h-full bg-gray-800 text-white w-64 p-4">
                {/* Left Nav Menu Content */}
                <div className="flex flex-col h-full justify-between">
                    <div>
                        <Link to="/">
                            <img src={logo} alt="Logo" className="h-10 w-auto mb-4" />
                        </Link>
                        <Link to="/search" className="text-gray-300 hover:text-white mb-4">
                            Search
                        </Link>
                        {/* Add more links here as needed */}
                    </div>
                    <div className="text-center mb-4">
                        {/* Left Nav Menu Bottom Links */}
                        {/* Add bottom links here if needed */}
                    </div>
                </div>
            </nav>

            {/* Right side content */}
            <div className="flex-1 ml-64">
                {/* Top Nav Menu */}
                <nav className="fixed top-0 left-64 right-0 w-full bg-white border-t border-gray-200 p-4 flex justify-center items-center">
                    <div className="flex items-center space-x-6 text-black">
                        <div className="hover:text-darkBlue">Login</div>
                        <a
                            href=""
                            className="px-8 py-3 font-bold rounded text-white bg-lightGreen hover:opacity-70"
                        >
                            Signup
                        </a>
                    </div>
                </nav>
                {/* Page content */}
                {/* Add page content here */}
            </div>
            <Auth></Auth>
        </div>
    );
};

export default Navbar;
