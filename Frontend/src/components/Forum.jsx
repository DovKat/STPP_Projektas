import React from 'react';

const Forum = ({ title, description, onClick }) => (
  <div className="p-4 border rounded-lg shadow">
    <h2 className="text-xl font-bold">{title}</h2>
    <p>{description}</p>
    <button onClick={onClick} className="text-blue-500">View Posts</button>
  </div>
);

export default Forum;
