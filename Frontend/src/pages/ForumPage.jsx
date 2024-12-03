import React, { useState } from 'react';
import Forum from '../components/Forum';

const ForumPage = () => {
  const forums = [
    { id: 1, title: 'General Discussion', description: 'Talk about anything!' },
    { id: 2, title: 'Tech News', description: 'Latest updates in tech.' },
  ];

  return (
    <div>
      <h2 className="text-2xl font-bold mb-4">Forums</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {forums.map((forum) => (
          <Forum
            key={forum.id}
            title={forum.title}
            description={forum.description}
            onClick={() => alert(`Navigate to Forum ${forum.id}`)}
          />
        ))}
      </div>
    </div>
  );
};

export default ForumPage;
