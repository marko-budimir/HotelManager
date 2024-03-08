import { useState } from "react";

export const EditPassword = ({ handleEdit }) => {
  const [passwordData, setPasswordData] = useState({});

  const handleChange = (e) => {
    setPasswordData({ ...passwordData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    handleEdit(passwordData);
    e.target.reset();
  };

  const handlePasswordValidation = () => {
    console.log(passwordData);
  };
  return (
    <div className="edit-profile profile-form">
      <h2 className="form-heading">CHANGE PASSWORD</h2>
      <form className="form" onSubmit={handleSubmit}>
        <input
          placeholder="Current password"
          type="password"
          name="passwordOld"
          id="currentPassword"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="New password"
          type="password"
          id="newPassword"
          name="passwordNew"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="New password again"
          type="password"
          id="newPasswordConfirm"
          className="form-input"
          onChange={handlePasswordValidation}
        />

        <input type="submit" className="form-submit-button" value="Apply" />
      </form>
    </div>
  );
};
