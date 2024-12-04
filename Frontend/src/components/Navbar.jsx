import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { AiOutlineMenu, AiOutlineClose } from 'react-icons/ai';
import { useAuth } from '../context/AuthContext'; // Import useAuth to access user and logout
import { useNavigate } from 'react-router-dom';  // Ensure this import is correct
import './Navbar.css'; // Import CSS

const Navbar = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [isDesktop, setIsDesktop] = useState(window.innerWidth >= 769); // Set initial screen width state
  const { user, logout } = useAuth(); // Get user and logout from context
  const navigate = useNavigate(); // Hook to handle navigation

  // Handle resizing of the window
  useEffect(() => {
    const handleResize = () => {
      setIsDesktop(window.innerWidth >= 769); // Update the isDesktop state based on window width
    };
    window.addEventListener('resize', handleResize);

    // Cleanup on unmount
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  const handleLogout = () => {
    logout();
    navigate("/login"); // Navigate to login after logout
  };

  return (
    <nav className="navbar">
      {/* Hamburger menu button (only visible on mobile) */}
      <div className="menu-toggle">
        <button onClick={() => setIsOpen(!isOpen)} aria-label="Toggle menu">
          {isOpen ? <AiOutlineClose size={24} /> : <AiOutlineMenu size={24} />}
        </button>
      </div>

      {/* Menu items */}
      {/* On desktop, menu is always visible, on mobile it toggles */}
      <ul className={`menu ${isDesktop || isOpen ? 'block' : 'hidden'} md:flex`}>
        <li className="nav-item">
          <Link className="nav-item-link" to="/">Home</Link>
        </li>
        <li className="nav-item">
          <Link className="nav-item-link" to="/forums">Forums</Link>
        </li>
        <li className="nav-item">
          <Link className="nav-item-link" to="/contact">Contact</Link>
        </li>
      </ul>

      {/* Auth Section */}
      {/* Align the username and logout button to the right on desktop */}
      <div className={`auth-section ${isDesktop ? 'ml-auto' : ''}`}>
        {user && (
          <div className="flex items-center gap-4">
            <span className="username">Logged in as: {user.username}</span>
            <button className="logout-button" onClick={handleLogout}>
              Logout
            </button>
          </div>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
