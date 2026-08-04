import React from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "./NotFound.css";

const NotFound = () => {
  const navigate = useNavigate();
  const location: any = useLocation();
  const backHome = () => {
    navigate("/");
  };

  // navigate("/");
  return (
    <div className="NotFound">
      <h1>Page Not Found !</h1>
      <button onClick={backHome}>Back To Home</button>
    </div>
  );
};

export default NotFound;
