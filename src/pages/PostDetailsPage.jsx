import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import config from '../config'; // Import the base URL config
import './PostDetailsPage.css'; // Import the CSS for styling
import { useAuth } from '../context/AuthContext';
import dayjs from 'dayjs';

const parseDate = (dateString) => {
  try {
    return dayjs(dateString).format('MMMM D, YYYY h:mm A');
  } catch (error) {
    console.error('Error parsing date:', dateString);
    return 'Invalid Date';
  }
};

const PostDetailsPage = () => {
  const { forumId, postId } = useParams();
  const [post, setPost] = useState(null);
  const [comments, setComments] = useState([]);
  const [comment, setComment] = useState('');
  const [showCreateCommentModal, setShowCreateCommentModal] = useState(false);
  const [editingComment, setEditingComment] = useState(null);  // New state for editing comment
  const [editCommentText, setEditCommentText] = useState('');  // State for edited comment text
  const { accessToken, waitForAccessToken, loading } = useAuth();

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const token = await waitForAccessToken();
        const postResponse = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}`);
        setPost(postResponse.data);

        const commentsResponse = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments`);
        setComments(commentsResponse.data);
      } catch (error) {
        console.error('Error fetching post or comments', error);
      }
    };
    fetchPost();
  }, [forumId, postId, waitForAccessToken]);

  const handleCommentSubmit = async (e) => {
    e.preventDefault();
    try {
      const token = await waitForAccessToken();
      const response = await axios.post(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments`,
        { description: comment },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setComments([...comments, response.data]);
      setComment('');
      setShowCreateCommentModal(false); // Close the modal after posting
    } catch (error) {
      console.error('Error posting comment', error);
    }
  };

  const handleDeleteComment = async (commentId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments/${commentId}`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setComments(comments.filter((comment) => comment.id !== commentId));
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      }
      else
      {
        console.error('Error deleting comment', error);
      }
    }
  };

  const handleEditComment = (commentId) => {
    // Find the comment to edit and open the modal
    const commentToEdit = comments.find(comment => comment.id === commentId);
    setEditingComment(commentToEdit);
    setEditCommentText(commentToEdit.description);  // Set the current comment text for editing
  };

  const handleEditCommentSave = async () => {
    try {
      const token = await waitForAccessToken();
      const response = await axios.put(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}/comments/${editingComment.id}`,
        { description: editCommentText },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      // Update the comments state with the updated comment
      setComments(comments.map((comment) =>
        comment.id === editingComment.id ? { ...comment, description: editCommentText } : comment
      ));
      setEditingComment(null); // Close the edit modal
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      }
      console.error('Error updating comment', error);
    }
  };

  const handleEditCommentCancel = () => {
    setEditingComment(null); // Close the edit modal without saving
  };

  return (
    <div className="post-details">
      {post && (
        <>
          <div className="post-item">
            <h2>{post.title}</h2>
            <p>{post.description}</p>
          </div>

          <h3>Comments</h3>

          <div className="comment-list">
            {comments.map((comment) => (
              <div key={comment.id} className="comment-item">
                <p className="created-date">{parseDate(comment.createdAt)}</p>

                <p>{comment.description}</p>
                <div className="comment-actions">
                  <button className="edit-button" onClick={() => handleEditComment(comment.id)}>
                    Edit
                  </button>
                  <button className="delete-button" onClick={() => handleDeleteComment(comment.id)}>
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>

          <button className="create-comment-button" onClick={() => setShowCreateCommentModal(true)}>
            Create New Comment
          </button>

          {/* Create comment modal */}
          {showCreateCommentModal && (
            <div className="modal">
              <div className="modal-content">
                <h4>Create Comment</h4>
                <form onSubmit={handleCommentSubmit}>
                  <textarea
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    required
                    placeholder="Write your comment here..."
                  />
                  <div className="modal-actions">
                    <button type="submit">Post Comment</button>
                    <button type="button" onClick={() => setShowCreateCommentModal(false)}>
                      Cancel
                    </button>
                  </div>
                </form>
              </div>
            </div>
          )}

          {/* Edit comment modal */}
          {editingComment && (
            <div className="modal">
              <div className="modal-content">
                <h4>Edit Comment</h4>
                <textarea
                  value={editCommentText}
                  onChange={(e) => setEditCommentText(e.target.value)}
                  required
                />
                <div className="modal-actions">
                  <button onClick={handleEditCommentSave}>Save</button>
                  <button onClick={handleEditCommentCancel}>Cancel</button>
                </div>
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default PostDetailsPage;
