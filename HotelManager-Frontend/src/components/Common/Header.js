import { Link } from "react-router-dom";
export const Header = () => {
  return (
    <>
      <Link to="/">
        <h1 className="main-title">Hotel Manager</h1>
      </Link>
      <ul className="headerLinks">
        <li className="headerLink">
          <Link to="login">Login</Link>
        </li>
        <li className="headerLink">
          <Link to="register">Register</Link>
        </li>
      </ul>
    </>
  );
};
