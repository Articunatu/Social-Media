import React from 'react';
import Button from './Shared/Button';

const varde = 0; 

const AuthPage: React.FC = () => {
    return (
        <div className="flex justify-center items-center h-screen">
            <div className="bg-white p-8 rounded shadow-md">
                <h2 className="text-2xl mb-4">Login</h2>
                {/* Your authentication form goes here */}
                <Button onClick={() => console.log("Signup clicked")}>Sign Up</Button>
                <Button onClick={() => console.log("Login clicked")}>Login</Button>
            </div>          
        </div>
    );
};

export default AuthPage;
