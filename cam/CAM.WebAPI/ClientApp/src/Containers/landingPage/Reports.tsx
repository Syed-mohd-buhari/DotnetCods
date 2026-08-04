import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";

import "./landingPageHeader.css";
import logo from "./skins/logo___transparent_1.png";

import { Container, Row, Col } from "react-bootstrap";
import { useModal } from "../../Hook/useModal";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import { ResetForeignIndex } from "../../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import Menu from "../../Components/Menu";
import { useReports } from "./MenuItem";

// import "./landingPage.css";

interface MenuItem {
  title: string;
  path: string;
  category?: string;
  disable?: boolean;
  bracket?: string;
}
interface MenuItemDescription {
  title: string;
  description: string;
  sub: string;
}

const Reports = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    readonly,
    role,
    userPrefrenceDetails,
  } = useAuth();
  const reports = useReports();
  const [menus, setMenus] = useState<MenuItem[]>([]);
  const [menuDescriptions, setMenuDescriptions] = useState<MenuItemDescription>(
    { title: "", description: "", sub: "" }
  );

  const {
    isVisibleModalManage,
    setIsVisibleModalManage,
    isVisibleModalInitializeNewProduct,
    setIsVisibleModalInitializeNewProduct,
    isVisibleModalProductLifecycle,
    setIsVisibleModalProductLifecycle,
    isVisibleModalStatus,
    setIsVisibleModalStatus,
  } = useModal();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  useEffect(() => {
    setMenuDescriptions(getDescriptionForOption("Reports"));
  }, []);

  useEffect(() => {
    setMenuDescriptions(getDescriptionForOption("Reports"));
  }, [readonly, KPIAdmin, KPIEditor, admin, simpleUser]);

  useEffect(() => {
    if (userPrefrenceDetails) {
      const getMenusList = getMenusForOption("Reports");
      const combinedUserPrefrenceMenus = [
        ...userPrefrenceDetails?.pagePrefrenceDetail,
        ...userPrefrenceDetails?.popupPagePrefrenceDetail,
      ];
      const userPreferencePathList: any = new Set(
        combinedUserPrefrenceMenus?.map((item) => item.path)
      );

      const filteredResult = getMenusList.filter((item) =>
        userPreferencePathList.has(item.path)
      );
      setMenus(filteredResult);
    }
  }, [userPrefrenceDetails]);

  const getDescriptionForOption = (option: string) => {
    return option === "Reports"
      ? { title: "Reports", description: " ", sub: "" }
      : {
          title: "WELCOME TO TEMS",
          description: "Telecoms Engineering Management System",
          sub: "A flexible and secure decision support system",
        };
  };
  const getMenusForOption = (option: string): MenuItem[] => {
    if (option === "Reports") {
      return reports;
    }
    return [];
  };
  const handleMenuClick = (path: string, newTab: boolean = false) => {
    const isNewTab = [
      "/assetoverviewbymarket",
      "/lcmatglance",
      "/exodusatglance",
      "/plannedactivityreport",
      "/nfvicreport",
    ].includes(path);
    if (path.includes("/")) {
      if (newTab || isNewTab) {
        window.open(path, "_blank");
      } else {
        navigate(path);
      }
    } else {
      try {
        eval(path);
      } catch (error) {
        console.error("Error executing function:", error);
      }
    }
  };

  const groupedMenus = menus.reduce((acc, menu) => {
    if (menu.category) {
      if (!acc[menu.category]) acc[menu.category] = [];
      acc[menu.category].push(menu);
    } else {
      if (!acc[""]) acc[""] = [];
      acc[""].push(menu);
    }
    return acc;
  }, {} as Record<string, MenuItem[]>);
  return (
    <div className="landing_container">
      <Container>
        {menuDescriptions && (
          <div className={`  ${"landing_sub_header"}`}>
            <img
              src={logo}
              className={`  ${"landing_sub_header_logo"}`}
              alt="TEMS Logo"
            />
            <div className={` ${"landing_sub_header_title"}`}>
              <div className="main_title">{menuDescriptions.title}</div>
              <div className="head_title">{menuDescriptions.description}</div>
              {/* {menuDescriptions.sub ? (
                <span className="head_sub_title">{menuDescriptions.sub}</span>
              ) : (
                <br />
              )} */}
            </div>
          </div>
        )}
      </Container>

      {}
      <Menu groupedMenus={groupedMenus} handleMenuClick={handleMenuClick} />

      <ModalConfirm data={confirm} />
    </div>
  );
};

export default Reports;
