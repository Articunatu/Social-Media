import React from 'react';

const Logout = () => {
  const isAuthenticated = true; // Set the value based on the user's authentication status

  const handleLogout = (e) => {
    e.preventDefault();
    // Implement the logic to perform the logout action using JWT
    console.log('Performing logout...');
  };

  return (
    <div>
      <header>
        {isAuthenticated ? (
          <form className="form-inline" onSubmit={handleLogout}>
            <button type="submit" className="btn btn-dark">Click here to Logout</button>
          </form>
        ) : (
          <p>You have successfully logged out of the application.</p>
        )}
      </header>
    </div>
  );
};

export default Logout;