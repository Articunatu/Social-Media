import React, { useState } from "react";
import { Link } from "react-router-dom";
import logo from "./logo.png";
import API from '../../Routes/Api'; // Import the API object
import SmButton from './SmButton';

interface Props {}

const NavbarLeft = (props: Props) => {
    // State to hold user credentials
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async (e: React.MouseEvent<HTMLButtonElement>) => {
            e.preventDefault();
        try {
            const response = await API.profiles.login(email, password);
            console.log("Login successful", response);
        } catch (error: any) { // Asserting the type of error to 'any'
            // Handle login failure
            console.error("Login failed", error.response.data);
        }
    };
    
    return (
        <nav className="fixed left-0 top-0 h-full bg-white border-r border-gray-200 p-4">
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
                <div className="flex flex-col items-center lg:items-start">
                <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    {/* Pass email and password to handleLogin */}
                    <button onClick={handleLogin} className="hover:text-darkBlue">
                        Login
                    </button>
                    <Link to="/signup" className="px-8 py-3 mt-4 font-bold rounded text-white bg-lightGreen hover:opacity-70">
                        Signup
                    </Link>
                </div>
            </div>
        </nav>
    );
};

export default NavbarLeft;
