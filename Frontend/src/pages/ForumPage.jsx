import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import { useAuth } from '../context/AuthContext';
import './ForumPage.css';
import dayjs from 'dayjs';

const ForumPage = () => {
  const [forums, setForums] = useState([]);
  const [editingForum, setEditingForum] = useState(null);
  const [editTitle, setEditTitle] = useState('');
  const [editDescription, setEditDescription] = useState('');
  const [newForumTitle, setNewForumTitle] = useState('');
  const [newForumDescription, setNewForumDescription] = useState('');
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);  // New state to handle modal visibility
  const { accessToken, waitForAccessToken, loading } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchForums = async () => {
      try {
        const token = await waitForAccessToken();
        const response = await axios.get(`${config.apiBaseUrl}/api/forums`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setForums(response.data);
      } catch (error) {
        console.error('Error fetching forums:', error);
      }
    };
    fetchForums();
  }, [waitForAccessToken]);

  const parseDate = (dateString) => {
    try {
      return dayjs(dateString).format('MMMM D, YYYY h:mm A');
    } catch (error) {
      console.error('Error parsing date:', dateString);
      return 'Invalid Date';
    }
  };

  const handleDelete = async (forumId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(`${config.apiBaseUrl}/api/forums/${forumId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setForums(forums.filter((forum) => forum.id !== forumId));
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      } else {
        console.error('Error deleting forum:', error);
      }
    }
  };

  const handleEditSave = async () => {
    try {
      const token = await waitForAccessToken();
      await axios.put(
        `${config.apiBaseUrl}/api/forums/${editingForum.id}`,
        { description: editDescription },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setForums(forums.map((forum) =>
        forum.id === editingForum.id
          ? { ...forum, title: editTitle, description: editDescription }
          : forum
      ));
      closeEditModal();
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      } else {
        console.error('Error saving edits:', error);
      }
    }
  };

  const openEditModal = (forum) => {
    setEditingForum(forum);
    setEditTitle(forum.title);
    setEditDescription(forum.description);
  };

  const closeEditModal = () => {
    setEditingForum(null);
  };

  // Open Create Modal
  const openCreateModal = () => {
    setNewForumTitle('');
    setNewForumDescription('');
    setIsCreateModalOpen(true);  // Open modal
  };

  // Close Create Modal
  const closeCreateModal = () => {
    setIsCreateModalOpen(false);  // Close modal
  };

  const handleCreateSave = async () => {
    try {
      const token = await waitForAccessToken();
      const response = await axios.post(
        `${config.apiBaseUrl}/api/forums`,
        {
          title: newForumTitle,
          description: newForumDescription,
        },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setForums([response.data, ...forums]);
      closeCreateModal();
    } catch (error) {
      console.error('Error creating forum:', error);
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="forum-list">
      <h2>Forums</h2>

      <div className="create-new-button">
        <button className="edit-button" onClick={openCreateModal}>Create New Forum</button>
      </div>
      <div>
        {forums.map((forum) => (
          <div key={forum.id} className="forum-item">
            <div className="forum-header">
              <h3>
                <a href={`/forums/${forum.id}`}>{forum.title}</a>
              </h3>
              <p className="created-date">{parseDate(forum.createdAt)}</p>
            </div>
            <p>{forum.description}</p>
            <div className="forum-actions">
              <button className="edit-button" onClick={() => openEditModal(forum)}>Edit</button>
              <button className="delete-button" onClick={() => handleDelete(forum.id)}>Delete</button>
            </div>
          </div>
        ))}
      </div>

      {editingForum && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Forum</h2>
            <h6>{editTitle}</h6>
            <label>Description</label>
            <textarea
              value={editDescription}
              onChange={(e) => setEditDescription(e.target.value)}
            />
            <div className="modal-actions">
              <button onClick={handleEditSave}>Save</button>
              <button onClick={closeEditModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}

      {/* Create Forum Modal */}
      {isCreateModalOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Create New Forum</h2>
            <label>Title</label>
            <input
              type="text"
              value={newForumTitle}
              onChange={(e) => setNewForumTitle(e.target.value)}
            />
            <label>Description</label>
            <textarea
              value={newForumDescription}
              onChange={(e) => setNewForumDescription(e.target.value)}
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

export default ForumPage;
