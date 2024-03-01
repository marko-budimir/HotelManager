import AuthenticationForm from "../components/authentication/AuthenticationForm";
import { useState } from "react";

const LoginPage = () => {
    const [loginForm, setLoginForm] = useState({
        email: "",
        password: ""
    });

    const handleChange = (e) => {
        setLoginForm({
            ...loginForm,
            [e.target.name]: e.target.value
        });
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(loginForm);
    }

    return (
        <div className="login-page">
            <h2>Welcome</h2>
            <h3>Please login</h3>
            <AuthenticationForm
                formType="Login"
                formFields={["email", "password"]}
                formValues={loginForm}
                handleChange={handleChange}
                handleSubmit={handleSubmit}
            />
        </div>
    );
}

export default LoginPage;