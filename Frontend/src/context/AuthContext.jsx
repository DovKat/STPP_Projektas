import React, { createContext, useState, useEffect, useContext } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import config from '../config';
const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [accessToken, setAccessToken] = useState(null);
  const [loading, setLoading] = useState(true); // Add loading state
  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
    const timeout = setTimeout(() => {
    setLoading(false); // Set loading to false after checking for user

    }, 200);
  }, []);


  // Login function
  const login = async (username, password) => {
    try {
      const apiUrl = `${config.apiBaseUrl}/api/login`;
      const response = await axios.post(apiUrl, { username, password }, { withCredentials: true });
      setUser({ username }); // Update user state
      localStorage.setItem('user', JSON.stringify({ username })); // Store user in localStorage
      console.log("Login successful:", response.data);
    } catch (error) {
      console.error("Login failed:", error);
    }
  };

  

  // Register function
  const register = async (username, email, password) => {
    try {
      const apiUrl = `${config.apiBaseUrl}/api/register`;
      await axios.post(apiUrl, { username, email, password }, { withCredentials: true });
      await login(username, password); // After successful registration, login the user
    } catch (error) {
      console.error('Registration failed:', error);
    }
  };

  const logout = async () => {
    try {
      // Retrieve the refresh token from cookies
      const refreshToken = document.cookie
        .split('; ')
        .find(row => row.startsWith('RefreshToken'))
        ?.split('=')[1];
      
      if (!refreshToken) {
        console.error('No refresh token found');
        return;
      }
  
      // Call the logout API
      const apiUrl = `${config.apiBaseUrl}/api/logout`;
      await axios.post(apiUrl, { refreshToken }, { withCredentials: true });
  
      // Clear cookies by setting them to expire
  
      // Clear local storage and reset user context
      localStorage.clear();
      setUser(null);
      setAccessToken(null);
  
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };
  

  const refreshAccessToken = async () => {
    setLoading(true); // Set loading to true while refreshing token
    try {
      const apiUrl = `${config.apiBaseUrl}/api/accessToken`;
      const response = await axios.post(apiUrl, {}, { withCredentials: true });
      setAccessToken(response.data.accessToken);
    } catch (error) {
      console.error('Failed to refresh access token:', error);
    } finally {
      setLoading(false); // Loading is false after attempt
    }
  };
  
  const waitForAccessToken = async () => {
    if (!accessToken) {
      await refreshAccessToken();
    }
    return accessToken;
  };
  useEffect(() => {
    const interval = setInterval(refreshAccessToken, 5 * 60 * 1000); // Refresh every 5 minutes
    return () => clearInterval(interval); // Clear interval on component unmount
  }, []);

  return (
    <AuthContext.Provider value={{ user, login, register, logout, accessToken, loading, setAccessToken, waitForAccessToken }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);

export default AuthContext;
