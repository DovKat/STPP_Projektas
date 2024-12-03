import React from 'react';
import Navbar from './Navbar';

const Header = () => (
  <header className="bg-blue-600 text-white p-4">
    <div className="container mx-auto flex justify-between items-center">
      <h1 className="text-2xl font-bold">Forum App</h1>
      <Navbar />
    </div>
  </header>
);

export default Header;
