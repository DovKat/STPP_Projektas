import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
        const apiUrl = `${config.apiBaseUrl}/api/login`;
        const response = await axios.post(apiUrl, { username, password }, {
          withCredentials: true, // Ensure cookies are sent with the request
      });
      console.log('Login successful', response.data);
      // On successful login, navigate to the home/dashboard page
      navigate('/home'); // Adjust the redirect path as per your app
    } catch (error) {
      console.error('Login failed', error);
      alert('Login failed. Please check your credentials.');
    }
  };

  const goToRegister = () => {
    navigate('/register');
  };

  return (
    <div className="login-page">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div>
          <label>Username</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            required 
          />
        </div>
        <div>
          <label>Password</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <button onClick={goToRegister}>Don't have an account? Register here</button>
    </div>
  );
};

export default LoginPage;
