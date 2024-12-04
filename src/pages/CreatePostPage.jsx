import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from './config'; // Import the base URL config

const CreatePostPage = () => {
  const { forumId } = useParams();
  const navigate = useNavigate();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts`,
        { title, description },
        { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` } } // Assuming you're using JWT stored in localStorage
      );
      navigate(`/forums/${forumId}/posts/${response.data.id}`);
    } catch (error) {
      console.error('Error creating post', error);
    }
  };

  return (
    <div className="create-post">
      <h2>Create New Post</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title</label>
          <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} required />
        </div>
        <div>
          <label>Description</label>
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} required />
        </div>
        <button type="submit">Create Post</button>
      </form>
    </div>
  );
};

export default CreatePostPage;
