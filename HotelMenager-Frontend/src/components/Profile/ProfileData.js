export const ProfileData = ({ user }) => {
  console.log(Object.entries(user));
  const userArr = Object.entries(user).slice(1);
  console.log(userArr);
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
