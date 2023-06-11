import React, { useState } from 'react';

const Register = () => {
  const [fullname, setFullname] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleFullnameChange = (e) => {
    setFullname(e.target.value);
  };

  const handleUsernameChange = (e) => {
    setUsername(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };

  const handleConfirmPasswordChange = (e) => {
    setConfirmPassword(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Implement the logic to register the user using JWT
    console.log('Registering user...');
    console.log('Fullname:', fullname);
    console.log('Username:', username);
    console.log('Password:', password);
    console.log('Confirm Password:', confirmPassword);
  };

  return (
    <div className="register-wrapper">
      <h1>Enter info about you</h1>
      <form onSubmit={handleSubmit}>
        <label>
          <p>Fullname</p>
          <input type="text" value={fullname} onChange={handleFullnameChange} />
        </label>
        <label>
          <p>Username</p>
          <input type="text" value={username} onChange={handleUsernameChange} />
        </label>
        <label>
          <p>Choose a password</p>
          <input type="password" value={password} onChange={handlePasswordChange} />
        </label>
        <label>
          <p>Confirm password</p>
          <input type="password" value={confirmPassword} onChange={handleConfirmPasswordChange} />
        </label>
        <div>
          <button type="submit">Submit</button>
        </div>
      </form>
    </div>
  );
};

export default Register;