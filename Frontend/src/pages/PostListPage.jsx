import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import config from './config'; // Import the base URL config

const PostListPage = () => {
  const { forumId } = useParams();
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts`);
        setPosts(response.data);
      } catch (error) {
        console.error('Error fetching posts', error);
      }
    };
    fetchPosts();
  }, [forumId]);

  return (
    <div className="post-list">
      <h2>Posts in Forum</h2>
      <ul>
        {posts.map((post) => (
          <li key={post.id}>
            <a href={`/forums/${forumId}/posts/${post.id}`}>{post.title}</a>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PostListPage;
