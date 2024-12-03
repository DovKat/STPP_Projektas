import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import AuthContext from '../context/AuthContext';

const ProtectedRoute = ({ children }) => {
  const { user, loading } = useContext(AuthContext);

  // Show a loading spinner if the user is not available and loading is true
  if (loading) {
    return (
      <div className="loading-spinner">
        <span>Loading...</span>
        {/* Add your spinner component here */}
        <div className="spinner"></div> {/* Example spinner */}
      </div>
    );
  }

  // If user is not authenticated, redirect to login
  return user ? children : <Navigate to="/login" />;
};

export default ProtectedRoute;
