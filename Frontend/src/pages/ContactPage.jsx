import React from 'react';

const ContactPage = () => {
  return (
    <div className="contact-page">
      <h2>Contact Us</h2>
      <p>If you have any questions, feel free to reach out to us!</p>
      <form>
        <label>Name</label>
        <input type="text" placeholder="Enter your name" required />
        <label>Email</label>
        <input type="email" placeholder="Enter your email" required />
        <label>Message</label>
        <textarea placeholder="Your message..." required></textarea>
        <button type="submit">Submit</button>
      </form>
    </div>
  );
};

export default ContactPage;
