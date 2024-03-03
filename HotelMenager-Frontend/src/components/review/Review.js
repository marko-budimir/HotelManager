const Review = ({ review }) => {
    const { rating, comment, userFullName, dateCreated } = review;
  
    const formattedDate = new Date(dateCreated).toLocaleDateString();
  
    return (
      <div className="review" key={review.id}>
        <div className="review-rating">
          <span>Rating:</span> {rating}
        </div>
        <div className="review-text">{comment}</div>
        <div className="review-author">
          <span>By:</span> {userFullName}
        </div>
        <div className="review-date">{formattedDate}</div>
      </div>
    );
  };
  
  export default Review;
  