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
                            <img src={logo} alt="Logo" className="h-20 w-auto mb-8 rounded-xl" />
                        </Link>
                        <Link to="/search" className="text-gray-300 hover:text-white mb-4">
                            Search
                        </Link>
                        <p></p>
                        <Link to="/explore" className="text-gray-300 hover:text-white mb-4">
                            Explore
                        </Link>
                        {/* Add more links here as needed */}
                    </div>
                    <div className="mb-4">
                    <Link to="/settings" className="text-gray-300 hover:text-white mb-4">
                            Settings
                        </Link>
                    </div>
                </div>
            </nav>

            {/* Right side content */}
            <div className="flex-1 ml-64">
                {/* Top Nav Menu */}
                <nav className="fixed top-0 left-64 right-0 w-full bg-white border-t border-gray-200 p-4 flex justify-center items-center">
                    <div className="flex items-center space-x-6 text-black">
                        <h1>Följer</h1>
                        <h1>|</h1>
                        <h1>Utforskar</h1>
                    </div>
                </nav>
                {/* Page content */}
                {/* Add page content here */}
            </div>
        </div>
    );
};

export default Navbar;