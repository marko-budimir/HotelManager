export const ProfileData = ({ user }) => {
  const userArr = Object.entries(user).slice(1);

  const handleKeyString = (key) => {
    console.log(key);
    key = key.toUpperCase();
    console.log(key);
    if (key === "FIRSTNAME" || key === "LASTNAME") {
      const index = key.indexOf("N");
      key = key.slice(0, index) + " " + key.slice(index);
    }
    return key;
  };

  return (
    <div className="profile-data">
      {userArr.map(([key, value]) => {
        return (
          <div className="profile-data-field" key={user.id}>
            <span className="profile-data-field-heading">
              {handleKeyString(key)}
            </span>
            <span> </span>
            <span className="profile-data-field-text">{value}</span>
          </div>
        );
      })}
    </div>
  );
};
