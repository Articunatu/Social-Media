import React, { useState } from 'react';
import { URL } from '../../../Url';
import { useNavigate } from 'react-router-dom';
import { useAuth } from "../../../controllers/AuthenticationController";

export default function Login() {
  const [tag, setTag] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { setIsLoggedIn, setUserTag } = useAuth();

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

    const data = await response.json();

    if (response.ok) {
      localStorage.setItem('accessToken', data.accessToken);
      setIsLoggedIn(true);
      setUserTag(tag);
      navigate('/feed');
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