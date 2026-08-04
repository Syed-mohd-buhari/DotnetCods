import React, { useEffect, useRef, useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import Loader from "../../Components/Loader";
import "./RegularLogin.css";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";

const RegularLogin = ({ onLogin }) => {
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
      onLogin(loginData);
    }
  };

  const handelChangeInput = (e) => {
    setLogin({ ...loginData, [e.target.name]: e.target.value });
  };

  return (
    <div>
      <Modal show={show} onHide={() => setShow(show)}>
        <Modal.Header>
          <Modal.Title>Login</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form noValidate validated={validated} onSubmit={handelSubmitted}>
            <Form.Group className="mb-3" controlId="formBasicEmail">
              <Form.Label>Email address</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter email"
                name="email"
                required
                value={loginData.email}
                onChange={handelChangeInput}
              />
              <Form.Control.Feedback>Valid Email</Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formBasicPassword">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Password"
                required
                name="password"
                onChange={handelChangeInput}
                value={loginData.password}
              />
            </Form.Group>
            <Button
              variant="secondary"
              onClick={() => setShow(false)}
              className="button"
            >
              Close
            </Button>
            <Button
              className="button"
              variant="primary"
              type="submit"
              disabled={getLoader}
            >
              LOGIN
            </Button>
          </Form>
          <Loader isFullScreen={false} />
        </Modal.Body>
      </Modal>
      <button className="btn btn-danger mt-4" onClick={() => setShow(true)}>
        LOGIN
      </button>
    </div>
  );
};

export default RegularLogin;
