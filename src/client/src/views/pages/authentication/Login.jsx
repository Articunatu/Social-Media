import React, { useState } from 'react';

export default function Login() {
  const [tag, setTag] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async (event) => {
    event.preventDefault();

    const url = URL.login;
    
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        tag,
        password
      })
    });

    console.log(response)

    const data = await response.json();

    if (response.ok) {
      localStorage.setItem('accessToken', data.accessToken);
    }
  };

  return (
    <form onSubmit={handleLogin}>
      <label>
        Username:
        <input type="text" value={tag} onChange={e => setTag(e.target.value)} />
      </label>
      <label>
        Password:
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
      </label>
      <button type="submit">Login</button>
    </form>
  );
}