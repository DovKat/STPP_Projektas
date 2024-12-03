import React, { useState } from 'react';
import Post from '../components/Post';
import Comment from '../components/Comment';
import Modal from '../components/Modal';
import Form from '../components/Form';

const PostPage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const post = {
    title: 'Sample Post',
    body: 'This is a sample post description.',
  };

  const comments = [
    { id: 1, author: 'User1', text: 'Great post!' },
    { id: 2, author: 'User2', text: 'Thanks for sharing!' },
  ];

  return (
    <div>
      <Post title={post.title} body={post.body} />
      <h4 className="text-xl font-bold mt-4">Comments</h4>
      <div>
        {comments.map((comment) => (
          <Comment key={comment.id} author={comment.author} text={comment.text} />
        ))}
      </div>
      <button
        className="bg-green-500 text-white px-4 py-2 mt-4"
        onClick={() => setIsModalOpen(true)}
      >
        Add Comment
      </button>
      <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}>
        <Form onSubmit={(e) => { e.preventDefault(); setIsModalOpen(false); }} />
      </Modal>
    </div>
  );
};

export default PostPage;
