import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';  // Correct import of the custom hook
import './LoginPage.css';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();  // Get the login function from AuthContext

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      await login(username, password);  // Call the login function from AuthContext
      navigate('/');  // Redirect to home page after login
    } catch (error) {
      alert('Login failed. Please check your credentials.');
    }
  };
  const goToRegister = () => {
    navigate('/register');
  };
  return (
    <div className="login-page">
      <div className="login-form">
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
    </div>
  );
};

export default LoginPage;
