import React, { useState } from 'react';
import { useParams } from "react-router-dom";
import api_review from '../../services/api_review';

const ReviewAdd = () => {
  const { roomId } = useParams();
  const [rating, setRating] = useState(1);
  const [text, setText] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
  
    try {
      const review = { rating, Comment: text };
  
      await api_review.createReviewForRoom(roomId, review);
  
      setRating(1);
      setText('');
    } catch (error) {
      console.error('Error submitting review:', error);
    }
  };
  

  return (
    <form onSubmit={handleSubmit} className="add-review-form">
      <h3>Add Review</h3>
      <div className="rating">
        <label htmlFor="rating">Rating:</label>
        <input
          type="number"
          id="rating"
          min={1}
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
