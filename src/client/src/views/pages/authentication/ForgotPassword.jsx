import React, { useState } from 'react';

const ForgotPassword = () => {
  const [email, setEmail] = useState('');

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Implement the logic to send the reset password email using JWT
    console.log(`Sending reset password email to: ${email}`);
  };

  return (
    <div>
      <h2>Enter your email.</h2>
      <div className="row">
        <div className="col-md-4">
          <form onSubmit={handleSubmit}>
            <div className="text-danger"></div>
            <div className="form-floating">
              <input
                type="email"
                value={email}
                onChange={handleEmailChange}
                className="form-control"
                autoComplete="username"
                aria-required="true"
              />
              <label className="form-label">Email</label>
              {/* Add validation error display if needed */}
              {/* <span className="text-danger"></span> */}
            </div>
            <button type="submit" className="btn btn-dark">Reset Password</button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default ForgotPassword;
