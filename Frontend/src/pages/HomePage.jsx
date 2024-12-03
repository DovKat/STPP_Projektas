import React from 'react';
import { Link } from 'react-router-dom';
const HomePage = () => {
  return (
    <div className="home-page">
      <h2>Welcome to the Forum</h2>
      <p>Join the discussion, share your ideas, and interact with others in the community!</p>
      <Link to="/forums">Get started</Link>
    </div>
  );
};

export default HomePage;
