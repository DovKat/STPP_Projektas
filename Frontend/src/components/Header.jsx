import React from 'react';
import Navbar from './Navbar';
import './Header.css';
import logo from '../assets/scorpio-svgrepo-com.svg';

const Header = () => (
  <header className="p-4 w-full flex justify-between items-center">
    <div className="flex items-center gap-4 title-container">
    <img 
      src={logo} 
      alt="Logo" 
      className="w-12 h-12 logo"
    />
      <h1 className="title">Forum App</h1>
    </div>
    <Navbar />
    
  </header>
);

export default Header;
