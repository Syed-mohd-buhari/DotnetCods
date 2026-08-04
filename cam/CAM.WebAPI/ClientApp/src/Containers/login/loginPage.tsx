import React, { FC, useState, useEffect, useRef } from "react";
import "./loginPage.css";
import { Button, Form, Modal } from "react-bootstrap";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import Loader from "../../Components/Loader";
import LoginScreen from "../../screen/Login/Login";

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
    <div className="login_page">
      <section className="login_header">
        <img className="vd_logo" src={require("../../img/vd_logo.png")} />
        <span>Vodafone</span>
      </section>
      <section className="login_body">
        <div className="login_body_left">
          {" "}
          <img
            className="tems_logo"
            alt="Cam"
            src={require("../../img/logoCAM_R.png")}
          />
          <h3>WELCOME TO TEMS</h3>
        </div>
        <div className="login_body_right">
          <div className="login_container">
            <div className="login_form">
              <p className="tems_title">
                Telecoms Engineering <br /> Management System
              </p>
              <p className="tems_description">
                A flexible and secure decision support system
              </p>
              {!loginMode && (
                <>
                  <p className="login_title">Authentication required</p>
                  <p className="login_description">
                    Please login to access the application
                  </p>
                  <div className="login_btns">
                    <button onClick={() => setLoginMode("local")}>
                      Local user
                    </button>
                    <button
                      onClick={() => {
                        setLoginMode("azure");
                        localStorage.setItem("TYPE", "AZURE");
                      }}
                    >
                      Azure user
                    </button>
                  </div>
                </>
              )}
              {loginMode === "local" && (
                <div className="login_local_form">
                  <Form
                    noValidate
                    validated={validated}
                    onSubmit={handelSubmitted}
                  >
                    <input
                      type="email"
                      placeholder="Email Address"
                      name="email"
                      required
                      value={loginData.email}
                      onChange={handelChangeInput}
                    />
                    <input
                      type="password"
                      placeholder="Password"
                      required
                      name="password"
                      onChange={handelChangeInput}
                      value={loginData.password}
                    />

                    <Button
                      className="mr-2 login_btn login_btn_cancel"
                      onClick={() => {
                        setLoginMode("");
                        setShow(false);
                      }}
                    >
                      Cancel
                    </Button>
                    <Button
                      variant="danger"
                      className="login_btn"
                      type="submit"
                      disabled={getLoader}
                    >
                      Login
                    </Button>
                  </Form>
                </div>
              )}
              {loginMode === "azure" && (
                <div className="admin_local_btns">
                  {process.env.REACT_APP_ALLOW_LOCAL_LOGIN === "false" && (
                    <button
                      className="mr-2"
                      onClick={() => {
                        setLoginMode("");
                        setShow(false);
                      }}
                    >
                      Cancel
                    </button>
                  )}
                  <button
                    onClick={azureLogin}
                    disabled={getLoader}
                    className="azure_login_btn"
                  >
                    Login
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default LoginPage;
