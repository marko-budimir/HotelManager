import React, { useState, useEffect } from 'react';
import Review from './Review';

const Reviews = () => {
  const [reviews, setReviews] = useState([]);

  // Simuliranje dohvaćanja recenzija (zamijenite s API pozivom)
  useEffect(() => {
    const fetchReviews = async () => {
      const mockReviews = [
        {
          id: "eaf4c9b3-88ec-40a1-b610-1be6c07cd81b",
          rating: 4,
          comment: "Comfortable stay",
          userFullName: "Marko Doe",
          dateCreated: "2024-02-16T00:00:00",
        },
      ];
      setReviews(mockReviews);
    };

    fetchReviews();
  }, []);

  return (
    <div className="reviews">
      <h2>Reviews</h2>
      {reviews.map((review) => (
        <Review key={review.id} review={review} />
      ))}
    </div>
  );
};

export default Reviews;
