import React from 'react';

const Comment = ({ author, text }) => (
  <div className="p-3 border-t">
    <strong>{author}</strong>
    <p>{text}</p>
  </div>
);

export default Comment;
