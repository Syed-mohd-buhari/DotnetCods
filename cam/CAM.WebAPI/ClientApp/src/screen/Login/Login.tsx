import React from "react";
const LoginScreen = ({ onLogin }) => {
  return (
    <div>
      <button
        id="authenticationButton"
        type="button"
        className="btn btn-danger mt-4"
        onClick={() => onLogin()}
      >
        LOGIN
      </button>
    </div>
  );
};

export default LoginScreen;
