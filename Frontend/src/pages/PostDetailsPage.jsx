import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import config from './config'; // Import the base URL config

const PostDetailsPage = () => {
  const { forumId, postId } = useParams();
  const [post, setPost] = useState(null);
  const [comments, setComments] = useState([]);
  const [comment, setComment] = useState('');

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const postResponse = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}`);
        setPost(postResponse.data);

        const commentsResponse = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments`);
        setComments(commentsResponse.data);
      } catch (error) {
        console.error('Error fetching post or comments', error);
      }
    };
    fetchPost();
  }, [forumId, postId]);

  const handleCommentSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments`,
        { description: comment },
        { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` } }
      );
      setComments([...comments, response.data]);
      setComment('');
    } catch (error) {
      console.error('Error posting comment', error);
    }
  };

  return (
    <div className="post-details">
      {post && (
        <>
          <h2>{post.title}</h2>
          <p>{post.description}</p>
        </>
      )}
      <h3>Comments</h3>
      <ul>
        {comments.map((comment) => (
          <li key={comment.id}>{comment.description}</li>
        ))}
      </ul>

      <form onSubmit={handleCommentSubmit}>
        <textarea value={comment} onChange={(e) => setComment(e.target.value)} required />
        <button type="submit">Post Comment</button>
      </form>
    </div>
  );
};

export default PostDetailsPage;
