import React, { useState } from 'react';

const ReviewAdd = () => {
  const [rating, setRating] = useState(0);
  const [text, setText] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();

    // Clear form after submit
    setRating(0);
    setText('');
  };

  return (
    <form onSubmit={handleSubmit} className="add-review-form">
      <h3>Add Review</h3>
      <div className="rating">
        <label htmlFor="rating">Rating:</label>
        <input
          type="number"
          id="rating"
          min={0}
          max={5}
          value={rating}
          onChange={(e) => setRating(e.target.value)}
          className="rating-input"
        />
      </div>
      <div className="review-text">
        <label htmlFor="review-text">Review Text:</label>
        <textarea
          id="review-text"
          value={text}
          onChange={(e) => setText(e.target.value)}
          className="review-text-area"
        />
      </div>
      <button type="submit" className="submit-button">
        Submit Review
      </button>
    </form>
  );
};

export default ReviewAdd;