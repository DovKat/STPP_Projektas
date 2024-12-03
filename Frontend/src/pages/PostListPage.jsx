import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import './PostListPage.css'; // Import the CSS file
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

const PostListPage = () => {
  const { forumId } = useParams();
  const navigate = useNavigate();
  const [posts, setPosts] = useState([]);
  const [editingPost, setEditingPost] = useState(null);
  const [editTitle, setEditTitle] = useState('');
  const [editContent, setEditContent] = useState('');
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [newPostTitle, setNewPostTitle] = useState('');
  const [newPostContent, setNewPostContent] = useState('');
  const { accessToken, waitForAccessToken, loading } = useAuth();

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const token = await waitForAccessToken();
        const response = await axios.get(`${config.apiBaseUrl}/api/forums/${forumId}/posts`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setPosts(response.data);
      } catch (error) {
        console.error('Error fetching posts', error);
      }
    };
    fetchPosts();
  }, [forumId, waitForAccessToken]);

  const handleDelete = async (postId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(`${config.apiBaseUrl}/api/forums/${forumId}/posts/${postId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setPosts(posts.filter((post) => post.id !== postId));
    } catch (error) {
      console.error('Error deleting post:', error);
    }
  };

  const handleEditSave = async () => {
    try {
      const token = await waitForAccessToken();
      await axios.put(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts/${editingPost.id}`,
        { title: editTitle, description: editContent },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setPosts(posts.map((post) =>
        post.id === editingPost.id ? { ...post, title: editTitle, description: editContent } : post
      ));
      closeEditModal();
    } catch (error) {
      console.error('Error saving post edits:', error);
    }
  };

  const openEditModal = (post) => {
    setEditingPost(post);
    setEditTitle(post.title);
    setEditContent(post.description);
  };

  const closeEditModal = () => {
    setEditingPost(null);
  };

  // Open Create Post Modal
  const openCreateModal = () => {
    setNewPostTitle('');
    setNewPostContent('');
    setIsCreateModalOpen(true);
  };

  // Close Create Post Modal
  const closeCreateModal = () => {
    setIsCreateModalOpen(false);
  };

  const handleCreateSave = async () => {
    try {
      const token = await waitForAccessToken();
      const response = await axios.post(
        `${config.apiBaseUrl}/api/forums/${forumId}/posts`,
        {
          title: newPostTitle,
          description: newPostContent,
        },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setPosts([response.data, ...posts]);
      closeCreateModal();
    } catch (error) {
      console.error('Error creating post:', error);
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="post-list">
      <h2>Posts in Forum</h2>

      <div className="create-new-button">
        <button className="edit-button" onClick={openCreateModal}>Create New Post</button>
      </div>

      <div>
        {posts.map((post) => (
          <div key={post.id} className="post-item">
            <div className="post-header">
              <h3>
                <a href={`/forums/${forumId}/posts/${post.id}`}>{post.title}</a>
              </h3>
              <p className="created-date">
                {parseDate(post.createdAt)}
              </p>
            </div>
            <p>{post.description}</p>
            <div className="post-actions">
              <button className="edit-button" onClick={() => openEditModal(post)}>Edit</button>
              <button className="delete-button" onClick={() => handleDelete(post.id)}>Delete</button>
            </div>
          </div>
        ))}
      </div>

      {/* Edit Post Modal */}
      {editingPost && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Post</h2>
            <label>Title</label>
            <input
              type="text"
              value={editTitle}
              onChange={(e) => setEditTitle(e.target.value)}
            />
            <label>Description</label>
            <textarea
              value={editContent}
              onChange={(e) => setEditContent(e.target.value)}
            />
            <div className="modal-actions">
              <button onClick={handleEditSave}>Save</button>
              <button onClick={closeEditModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}

      {/* Create Post Modal */}
      {isCreateModalOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Create New Post</h2>
            <label>Title</label>
            <input
              type="text"
              value={newPostTitle}
              onChange={(e) => setNewPostTitle(e.target.value)}
            />
            <label>Description</label>
            <textarea
              value={newPostContent}
              onChange={(e) => setNewPostContent(e.target.value)}
            />
            <div className="modal-actions">
              <button onClick={handleCreateSave}>Create</button>
              <button onClick={closeCreateModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default PostListPage;
