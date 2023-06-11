import React from 'react';

const Lockout = () => {
  return (
    <div>
      <header>
        <h1 className="text-danger">{title}</h1>
        <p className="text-danger">This account has been locked out, please try again later.</p>
      </header>
    </div>
  );
};

export default Lockout;
