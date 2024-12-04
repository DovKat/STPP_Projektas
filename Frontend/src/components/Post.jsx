import React from 'react';

const Post = ({ title, body }) => (
  <div className="p-4 border rounded-lg shadow mb-4">
    <h3 className="font-bold">{title}</h3>
    <p>{body}</p>
  </div>
);

export default Post;
