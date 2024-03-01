import AuthenticationFormRow from "./AuthenticationFormRow";

const AuthenticationForm = ({ formType, formLabels, formValues, handleChange, handleSubmit }) => {
    const formNames = Object.keys(formValues);
    return (
        <form className="authentication-form">
            <h3 className="authentication-form-header">{formType}</h3>
            {formNames.map((field, index) => {
                return (
                    <AuthenticationFormRow
                        key={index}
                        inputName={field}
                        inputType={field}
                        label={formLabels[index]}
                        value={formValues[field]}
                        handleChange={handleChange}
                    />
                );
            })}
            <button className="authentication-form-button" type="button" onClick={handleSubmit}>{formType}</button>
        </form>
    );
}

export default AuthenticationForm;