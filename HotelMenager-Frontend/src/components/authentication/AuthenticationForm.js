import AuthenticationFormRow from "./AuthenticationFormRow";

const AuthenticationForm = ({ formType, formFields, formValues, handleChange, handleSubmit }) => {
    return (
        <form className="authentication-form" onSubmit={handleSubmit}>
            <h2>{formType}</h2>
            {formFields.map((field, index) => {
                return (
                    <AuthenticationFormRow
                        key={index}
                        inputName={field}
                        inputType={field}
                        value={formValues[field]}
                        handleChange={handleChange}
                    />
                );
            })}
            <button type="submit" onSubmit={handleSubmit}>{formType}</button>
        </form>
    );
}

export default AuthenticationForm;