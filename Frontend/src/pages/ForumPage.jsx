import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config'; // Import the base URL config

const ForumPage = () => {
  const [forums, setForums] = useState([]);

  useEffect(() => {
    const fetchForums = async () => {
      try {
        const response = await axios.get(`${config.apiBaseUrl}/api/forums`);
        setForums(response.data);
      } catch (error) {
        console.error('Error fetching forums', error);
      }
    };
    fetchForums();
  }, []);

  return (
    <div className="forum-list">
      <h2>Forums</h2>
      <ul>
        {forums.map((forum) => (
          <li key={forum.id}>
            <a href={`/forums/${forum.id}`}>{forum.title}</a>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ForumPage;
