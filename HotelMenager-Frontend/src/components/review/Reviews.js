import React, { useState, useEffect } from 'react';
import Review from './Review';
import api_review from '../../services/api_review';
import Paging from '../Common/Paging';

const Reviews = ({ id }) => {
  const [reviews, setReviews] = useState([]);
  const [totalPages, setTotalPages] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 4;
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchReviews = async (page) => {
      try {
        const roomId = id;
        const response = await api_review.getReviewForRoomPaging(roomId, page, pageSize);
        const reviewsData = response.data.items;
        const totalPages = response.data.totalPages;
        
        setReviews(reviewsData);
        setTotalPages(totalPages);
      } catch (error) {
        console.error('Error fetching reviews:', error);
        setError('Could not fetch reviews. Please try again later.');
      }
    };

    fetchReviews(currentPage);
  }, [id, currentPage]);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };  

  return (
    <div className="reviews">
      <h2>Reviews</h2>
      {error ? (
        <p>{error}</p>
      ) : reviews.length > 0 ? (
        reviews.map((review) => (
          <Review key={review.id} review={review} />
        ))
      ) : (
        <p>No reviews available</p>
      )}
      <Paging totalPages={totalPages} currentPage={currentPage} onPageChange={handlePageChange} />
    </div>
  );
};

export default Reviews;