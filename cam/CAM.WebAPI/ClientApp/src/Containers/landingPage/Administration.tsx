import React, { useContext, useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import { OptionContext } from "../../Context/MenuOptionContext";

import "./landingPageHeader.css";
import logo from "./skins/logo___transparent_1.png";

import { Container, Row, Col } from "react-bootstrap";
import { useModal } from "../../Hook/useModal";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import { ResetForeignIndex } from "../../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import TreeViewMenu from "../../Components/TreeViewMenu/TreeViewMenu";
import Menu from "../../Components/Menu";
import { useAdministration } from "./MenuItem";

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

const Administration = () => {
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
  const administration = useAdministration();

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
    setMenuDescriptions(getDescriptionForOption("Administration"));
  }, []);

  useEffect(() => {
    setMenuDescriptions(getDescriptionForOption("Administration"));
  }, [readonly, KPIAdmin, KPIEditor, admin, simpleUser]);

  useEffect(() => {
    if (userPrefrenceDetails) {
      const getMenusList = getMenusForOption("Administration");
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
    return option === "Administration"
      ? { title: "Administration", description: " ", sub: "" }
      : {
          title: "WELCOME TO TEMS",
          description: "Telecoms Engineering Management System",
          sub: "A flexible and secure decision support system",
        };
  };
  const getMenusForOption = (option: string): MenuItem[] => {
    if (option === "Administration") {
      return administration;
    }
    return [];
  };

  const handleMenuClick = (path: string, newTab: boolean = false) => {
    if (path.includes("/")) {
      if (newTab) {
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
      if (!acc["Others"]) acc["Others"] = [];
      acc["Others"].push(menu);
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

export default Administration;
