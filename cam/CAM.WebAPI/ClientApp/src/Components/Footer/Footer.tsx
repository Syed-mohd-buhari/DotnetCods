import React, { useEffect, useState } from "react";
import "./Footer.css";
import { useNavigate, useLocation } from "react-router-dom";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import ModalConfirm from "../ModalConfirm";
import { useModal } from "../../Hook/useModal";
import { useTheme } from "../../Context/ThemeContext";
import ContactWhite from "./Mobile - contact us_white.png";
import ContactGrey from "./Mobile - contact us_grey.png";
import AboutWhite from "./Info - About us_white.png";
import AboutGrey from "./Info - About us_grey.png";
import SharePointWhite from "./sharepoint icon_white.png";
import SharePointGrey from "./TEMS SharePoint_grey.png";
import ReleaseVersionWhite from "./Version release_white.png";
import ReleaseVersionGrey from "./Version release_grey.png";
import ReportWhite from "./Report_white.png";
import ReportGrey from "./Report_grey.png";

const Footer = () => {
  const location = useLocation();
  const { myConfirm, onClickLink } = useModal();
  const { darkMode } = useTheme();
  return (
    <>
      <div className={"fixed-footer"}>
        <div>
          <span className="footer_tooltip" onClick={() => onClickLink("about")}>
            <img
              src={darkMode ? AboutWhite : AboutGrey}
              className="icon"
              alt="About"
            />
            <span className="footer_tooltiptext">About</span>
          </span>
          <span
            className="footer_tooltip"
            onClick={() => onClickLink("contact")}
          >
            <img
              src={darkMode ? ContactWhite : ContactGrey}
              className="icon"
              alt="Contact"
              style={{ width: "14px" }}
            />
            <span className="footer_tooltiptext">Contact</span>
          </span>
          <span
            className="footer_tooltip"
            onClick={() =>
              window.open(
                "https://vodafone.sharepoint.com/sites/TEMS-Application",
                "_blank"
              )
            }
          >
            <img
              src={darkMode ? SharePointWhite : SharePointGrey}
              className="icon"
              alt="SharePoint"
            />
            <span className="footer_tooltiptext">SharePoint</span>
          </span>
          <span
            className="footer_tooltip"
            onClick={() =>
              window.open(
                "https://vodafone.sharepoint.com/sites/TEMS-Application/SitePages/TEMS-BI-Reports.aspx",
                "_blank"
              )
            }
          >
            <img
              src={darkMode ? ReportWhite : ReportGrey}
              className="icon"
              alt="TEMS BI Reports"
            />
            <span className="footer_tooltiptext">TEMS BI Reports</span>
          </span>
        </div>
        <span>TEMS Vodafone / All rights reserved.</span>
        <span className="footer_icon">
          <img
            src={darkMode ? ReleaseVersionWhite : ReleaseVersionGrey}
            className="icon"
            alt="ReleaseVersion"
          />
          <span className="">7.26 - 3.22.3</span>
        </span>
      </div>
      <ModalConfirm data={myConfirm} />
    </>
  );
};

export default Footer;
