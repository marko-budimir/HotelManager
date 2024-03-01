const AuthenticationFormRow = ({ inputName, inputType, value, handleChange }) => {
    const placeholder = `Enter your ${inputName}`
    const name = inputName.charAt(0).toUpperCase() + inputName.slice(1);
    return (
        <p className="authentication-form-row">
            <label htmlFor={inputName}>{name}</label>
            <input type={inputType} id={inputName} name={inputName} value={value} onChange={handleChange} placeholder={placeholder} />
        </p>
    );
}

export default AuthenticationFormRow;