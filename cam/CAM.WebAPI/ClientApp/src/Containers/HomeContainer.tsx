import React from "react";
import "../Css/App.css";
import "../Css/index.css";
import { useAuth } from "../Hook/useAuth";
import HomeCards from "./home-cards/home-cards";

const HomeContainer: React.FC = (props) => {
  const { role } = useAuth();
  const checkErrorMessage: string = localStorage.getItem("ERROR_MESSAGE")!;

  let templateView;
  if (role?.length) {
    templateView = (
      <>
        <span className="welcome">Welcome to</span>
        <span className="welcome-s">
          Telecoms Engineering Management System
        </span>

        <div className="quick-links">
          <p>Discover TEMS using Quick Links</p>
        </div>
      </>
    );
  }

  if (!role?.length && checkErrorMessage === "") {
    templateView = (
      <span className="welcome-s">You Don't Have Any Roles !</span>
    );
  }

  if (!role?.length && checkErrorMessage !== "") {
    templateView = <span className="welcome-s">{checkErrorMessage}</span>;
  }
  return (
    <div className="homeScreen">
      <div>
        {templateView}
        <HomeCards />
      </div>
    </div>
  );
};

export default HomeContainer;
