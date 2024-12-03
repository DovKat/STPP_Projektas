import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { AiOutlineMenu, AiOutlineClose } from 'react-icons/ai';
import { useAuth } from '../context/AuthContext'; // Import useAuth to access user and logout
import { useNavigate } from 'react-router-dom';  // Ensure this import is correct

import './Navbar.css'; // Import CSS

const Navbar = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { user, logout } = useAuth(); // Get user and logout from context
  const navigate = useNavigate(); // Hook to handle navigation

  const handleLogout = () => {
    logout();
    navigate("/login"); // Navigate to login after logout
  };

  return (
    <nav className="navbar">
      <div className="menu-toggle">
        <button onClick={() => setIsOpen(!isOpen)}>
          {isOpen ? <AiOutlineClose size={24} /> : <AiOutlineMenu size={24} />}
        </button>
      </div>
      <ul className={`menu ${isOpen ? 'block' : 'hidden'} md:flex`}>
        <li className="nav-item">
          <Link to="/home">Home</Link>
        </li>
        <li className="nav-item">
          <Link to="/forums">Forums</Link>
        </li>
        <li className="nav-item">
          <Link to="/contact">Contact</Link>
        </li>
      </ul>
      <div className="auth-section">
        {user && (
          <>
            <span className="username">Logged in as: {user.username}</span>
            <button className="logout-button" onClick={handleLogout}>
              Logout
            </button>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
