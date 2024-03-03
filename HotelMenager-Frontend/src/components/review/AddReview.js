

const AddReview = () => {
    if (rating < 1 || rating > 5) {
      throw new Error("Rating must be a number between 1 and 5.");
    }
  
    if (!comment || comment.length === 0) {
      throw new Error("Comment cannot be empty.");
    }
  
    alert("Review added successfully!");
  };
  
  export default AddReview;