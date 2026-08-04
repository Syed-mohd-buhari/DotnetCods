import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { useCustomisePage } from "./landingPage/MenuItem";
import { useAuth } from "../Hook/useAuth";
import Menu from "../Components/Menu";

interface MenuItem {
    title: string;
    path: string;
    category?: string;
    disable?: boolean;
    bracket?: string;
  }
const GeneralSettings = () => {
    const {
        tipologicaPermesso,
        VerifyIsInRole,
        KPIAdmin,
        KPIEditor,
        admin,
        simpleUser,
        readonly,
        role,
      } = useAuth();
    const [menus, setMenus] = useState<MenuItem[]>([]);
    const customPages = useCustomisePage();
    const navigate = useNavigate();
    const getMenusForOption = (option: string): MenuItem[] => {
        if (option === "CustomisedPageSize") {
          return customPages;
    
        } 
        return [];
      };
    useEffect(() => {
        setMenus(getMenusForOption("CustomisedPageSize"));
        
      }, []);
    
      useEffect(() => {
        setMenus(getMenusForOption("CustomisedPageSize"));
      }, [readonly, KPIAdmin, KPIEditor, admin, simpleUser]);
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

  return (
    <div className="landing_container">
        <Menu groupedMenus={groupedMenus} handleMenuClick={handleMenuClick} />
    </div>
  );
};

export default GeneralSettings;
