import React from 'react';

const Form = ({ onSubmit }) => (
  <form onSubmit={onSubmit} className="space-y-4">
    <input
      type="text"
      placeholder="Title"
      className="border p-2 w-full"
      required
    />
    <textarea
      placeholder="Body"
      className="border p-2 w-full"
      required
    ></textarea>
    <button type="submit" className="bg-blue-600 text-white px-4 py-2">
      Submit
    </button>
  </form>
);

export default Form;
