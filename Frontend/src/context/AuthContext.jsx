import React, { createContext, useState, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [accessToken, setAccessToken] = useState(null);

  const login = async (username, password) => {
    try {
      const response = await axios.post('/api/login', { username, password });
      setAccessToken(response.data.accessToken);
      setUser({ username });
    } catch (error) {
      console.error('Login failed:', error);
    }
  };

  const register = async (username, email, password) => {
    try {
      await axios.post('/api/register', { username, email, password });
      await login(username, password);
    } catch (error) {
      console.error('Registration failed:', error);
    }
  };

  const logout = async () => {
    try {
      await axios.post('/api/logout');
      setUser(null);
      setAccessToken(null);
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  const refreshAccessToken = async () => {
    try {
      const response = await axios.post('/api/accessToken');
      setAccessToken(response.data.accessToken);
    } catch (error) {
      console.error('Failed to refresh access token:', error);
    }
  };

  useEffect(() => {
    const interval = setInterval(refreshAccessToken, 5 * 60 * 1000); // Refresh every 5 minutes
    return () => clearInterval(interval);
  }, []);

  return (
    <AuthContext.Provider value={{ user, login, register, logout, accessToken }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
