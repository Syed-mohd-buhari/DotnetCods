import React, { useState, useEffect, useRef } from "react";
import "./NewLogin.css";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import Loader from "../../Components/Loader";
import backgroundImage from "../../img/VF_Login_BG.png";
import logo from "../../img/vfLogo.png";
import { SvgIcon } from "@mui/material";
import { Form } from "react-bootstrap";

// Azure Icon Component
const AzureIcon = (props) => (
  <SvgIcon {...props}>
    <svg
      width="31"
      height="31"
      viewBox="0 0 31 31"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M29.2396 25.992L21.6396 3.19204C21.5074 2.79878 21.2501 2.4596 20.907 2.2263C20.564 1.993 20.1539 1.87842 19.7396 1.90004H10.7336C10.3353 1.89902 9.94673 2.0232 9.62283 2.25504C9.29893 2.48689 9.05609 2.81466 8.92862 3.19204L1.23363 25.992C1.13384 26.286 1.10762 26.6 1.15725 26.9065C1.20688 27.2129 1.33086 27.5026 1.51834 27.7501C1.70581 27.9975 1.95108 28.1953 2.23267 28.326C2.51426 28.4568 2.8236 28.5165 3.13363 28.5H8.70062C9.0957 28.4971 9.48003 28.3711 9.80014 28.1395C10.1202 27.908 10.3602 27.5824 10.4866 27.208L11.6456 23.826L17.3456 28.139C17.6769 28.3789 18.0767 28.5055 18.4856 28.5H27.3776C27.6845 28.5101 27.9893 28.4456 28.2658 28.3122C28.5423 28.1787 28.9824 27.9802 29.1654 27.7337C29.3484 27.4871 29.469 27.1999 29.5168 26.8966C29.5645 26.5933 29.538 26.2829 29.4396 25.992ZM18.5616 27.246C18.4235 27.2462 18.2895 27.1992 18.1816 27.113L7.44663 19.152L7.27563 19.019H12.9756L13.1276 18.62L15.0276 13.813L19.2836 26.41C19.3178 26.5157 19.3242 26.6283 19.3022 26.7371C19.2802 26.8459 19.2305 26.9473 19.1579 27.0313C19.0854 27.1153 18.9924 27.1792 18.8879 27.2168C18.7835 27.2544 18.6711 27.2645 18.5616 27.246ZM27.4346 27.246H20.3666C20.5085 26.8401 20.5085 26.398 20.3666 25.992L12.6716 3.19204H19.7396C19.8738 3.19272 20.0044 3.23516 20.1134 3.31347C20.2223 3.39179 20.3042 3.50208 20.3476 3.62904L28.0426 26.429C28.0686 26.5237 28.0727 26.6231 28.0546 26.7196C28.0365 26.816 27.9966 26.9071 27.938 26.9859C27.8793 27.0646 27.8035 27.129 27.7163 27.1741C27.6291 27.2192 27.5328 27.2438 27.4346 27.246Z"
        fill="currentColor"
        fillOpacity="0.5"
      />
    </svg>
  </SvgIcon>
);

// Local User Icon Component
const LocalUserIcon = (props) => (
  <SvgIcon {...props}>
    <svg
      width="31"
      height="31"
      viewBox="0 0 31 31"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M15.2 0C23.595 0 30.4 6.80503 30.4 15.2C30.4052 18.7079 29.1921 22.1089 26.9678 24.8216L26.9982 24.855L26.7976 25.0252C25.372 26.7111 23.5957 28.0655 21.5926 28.9938C19.5894 29.9221 17.4078 30.402 15.2 30.4C10.716 30.4 6.68801 28.4589 3.90641 25.3733L3.60241 25.0237L3.40177 24.8565L3.43217 24.82C1.20827 22.1078 -0.00486754 18.7074 1.46788e-05 15.2C1.46788e-05 6.80503 6.80505 0 15.2 0ZM15.2 22.8C12.3728 22.8 9.81769 23.6998 7.91465 24.9371C10.0162 26.5132 12.5731 27.3636 15.2 27.36C17.8269 27.3636 20.3838 26.5132 22.4854 24.9371C20.3107 23.5439 17.7827 22.8023 15.2 22.8ZM15.2 3.04C12.9117 3.03993 10.6698 3.68556 8.73199 4.90268C6.79419 6.11979 5.23916 7.85899 4.24562 9.92037C3.25207 11.9818 2.86036 14.2816 3.11548 16.5557C3.37061 18.8298 4.26223 20.9857 5.68785 22.7756C8.15177 21.0079 11.514 19.76 15.2 19.76C18.886 19.76 22.2482 21.0079 24.7122 22.7756C26.1378 20.9857 27.0294 18.8298 27.2845 16.5557C27.5397 14.2816 27.1479 11.9818 26.1544 9.92037C25.1609 7.85899 23.6058 6.11979 21.668 4.90268C19.7302 3.68556 17.4883 3.03993 15.2 3.04ZM15.2 6.07999C16.8125 6.07999 18.359 6.72056 19.4992 7.86078C20.6394 9.001 21.28 10.5475 21.28 12.16C21.28 13.7725 20.6394 15.319 19.4992 16.4592C18.359 17.5994 16.8125 18.24 15.2 18.24C13.5875 18.24 12.041 17.5994 10.9008 16.4592C9.76058 15.319 9.12001 13.7725 9.12001 12.16C9.12001 10.5475 9.76058 9.001 10.9008 7.86078C12.041 6.72056 13.5875 6.07999 15.2 6.07999ZM15.2 9.11999C14.3937 9.11999 13.6205 9.44027 13.0504 10.0104C12.4803 10.5805 12.16 11.3537 12.16 12.16C12.16 12.9662 12.4803 13.7395 13.0504 14.3096C13.6205 14.8797 14.3937 15.2 15.2 15.2C16.0063 15.2 16.7795 14.8797 17.3496 14.3096C17.9197 13.7395 18.24 12.9662 18.24 12.16C18.24 11.3537 17.9197 10.5805 17.3496 10.0104C16.7795 9.44027 16.0063 9.11999 15.2 9.11999Z"
        fill="currentColor"
      />
    </svg>
  </SvgIcon>
);

const LoginPage = ({ localLogin, azureLogin, loginType }) => {
  const [loginMode, setLoginMode] = useState(loginType);
  const nodeRef = useRef(null);
  let getLoader = useSelector((state: RootState) => state.loaderReducer.show);
  const [show, setShow] = useState<boolean>(false);
  const [validated, setValidated] = useState(false);
  const [loginData, setLogin] = useState<{ email: string; password: string }>({
    email: "",
    password: "",
  });

  useEffect(() => {
    if (!getLoader) {
      setShow(false);
    }
  }, [getLoader]);

  const handelSubmitted = (e) => {
    e.preventDefault();
    e.stopPropagation();

    const form = e.currentTarget;
    const emailField = form.elements["email"];

    if (emailField.checkValidity() === true) {
      setValidated(false);
      localLogin(loginData);
    }
  };

  const handelChangeInput = (e) => {
    setLogin({ ...loginData, [e.target.name]: e.target.value });
  };

  return (
    <div className="login-page-wrapper">
      <img src={backgroundImage} alt="" className="login-bg-image" />
      <div
        style={{
          position: "relative",
          zIndex: 10,
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          gap: "clamp(60px, 10vw, 271px)",
          padding: "0 clamp(20px, 5vw, 80px)",
          width: "100%",
          maxWidth: "1440px",
          boxSizing: "border-box",
        }}
      >
        <div className="login-content-row">
          <div className="login-branding">
            <div className="login-logo-wrap">
              <img src={logo} alt="Vodafone Logo" className="login-logo" />
            </div>

            <h1 className="login-title">
              Telecoms Engineering
              <br />
              Management System
            </h1>

            <p className="login-subtitle">
              A flexible and secure decision support system
            </p>
          </div>
        </div>
        <div className="login-card">
          <h2 className="login-welcome">Welcome back</h2>

          <div className="login-divider-row">
            <div className="login-divider-line" />
            <span className="login-divider-label">Select Login method</span>
            <div className="login-divider-line" />
          </div>

          <div className="login-method-row">
            <button
              onClick={() => {
                setLoginMode("azure");
                localStorage.setItem("TYPE", "AZURE");
              }}
              className={`login-method-btn login-method-btn--azure${
                loginMode === "azure" ? " login-method-btn--active" : ""
              }`}
            >
              <AzureIcon className="login-method-btn__icon" />
              <span className="login-method-btn__label">Azure User</span>
            </button>

            <button
              onClick={() => setLoginMode("local")}
              className={`login-method-btn login-method-btn--local${
                loginMode === "local" ? " login-method-btn--active" : ""
              }`}
            >
              <LocalUserIcon className="login-method-btn__icon" />
              <span className="login-method-btn__label">Local User</span>
            </button>
          </div>

          {loginMode === "local" && (
            <Form
              noValidate
              validated={validated}
              onSubmit={handelSubmitted}
              className="login-fields"
            >
              <div className="login-field">
                <label className="login-field__label">Email Address</label>
                <div className="login-field__input-wrap">
                  <input
                    type="text"
                    name="email"
                    required
                    value={loginData.email}
                    onChange={handelChangeInput}
                    className="login-field__input"
                  />
                  <span className="login-field__required-dot" />
                </div>
              </div>

              <div className="login-field">
                <label className="login-field__label">Password</label>
                <input
                  type="password"
                  required
                  onChange={handelChangeInput}
                  value={loginData.password}
                  className="login-field__input login-field__input--password"
                />
              </div>
              <button
                type="submit"
                disabled={getLoader}
                className="login-submit-btn"
              >
                <span className="login-submit-btn__text">Login</span>
              </button>
            </Form>
          )}
          {loginMode === "azure" && (
            <button
              onClick={azureLogin}
              disabled={getLoader}
              className="login-submit-btn"
            >
              <span className="login-submit-btn__text">Login</span>
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
