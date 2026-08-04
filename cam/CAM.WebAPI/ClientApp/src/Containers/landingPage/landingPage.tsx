import React, { FC, useState, useEffect, useContext } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Container, Row, Col } from "react-bootstrap";
import { useModal } from "../../Hook/useModal";
import ModalConfirm from "../../Components/ModalConfirm";
import TreeViewMenu from "../../Components/TreeViewMenu/TreeViewMenu";
import { OptionContext } from "../../Context/MenuOptionContext";
import { stateConfirm, DataModalConfirm } from "../../Model/Common";
import { ResetForeignIndex } from "../../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import { useAuth } from "../../Hook/useAuth";

import "./landingPage.css";
import logo from "./skins/logo___transparent_1.png";

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

const LandingPage = () => {
  const navigate = useNavigate();
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
  const [menuDescriptions, setMenuDescriptions] = useState<MenuItemDescription>(
    { title: "", description: "", sub: "" }
  );

  const { selectedOption } = useContext(OptionContext);
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

  const getDescriptionForOption = (option: string) => {
    if (option === "Product & Design") {
      return {
        title: "Product & Design Library",
        description: "",
        sub: "",
      };
    } else if (option === "Network Plan") {
      return {
        title: "Network Plan",
        description: "",
        sub: "",
      };
    } else if (option === "Manage Network Plan") {
      return {
        title: "Manage Network Plan",
        description: "",
        sub: "",
      };
    } else if (option === "Manage Transformation") {
      return {
        title: "Manage Transformation",
        description: "",
        sub: "",
      };
    } else if (option === "Reports") {
      return {
        title: "Reports",
        description: "",
        sub: "",
      };
    } else if (option === "Administration") {
      return {
        title: "Administration",
        description: "",
        sub: "",
      };
    } else {
      return {
        title: "WELCOME TO TEMS",
        description: "Telecoms Engineering Management System",
        sub: "A flexible and secure decision support system",
      };
    }
  };
  const getMenusForOption = (option: string): MenuItem[] => {
    if (option === "Product & Design") {
      return [
        {
          title: "Major Hardware Build",
          path: "/majorhardware",
          category: "Product Library",
        },
        {
          title: "Major Software Build",
          path: "/majorsoftware",
        },
        {
          title: "Component SW Build",
          path: "/componentswbuild",
        },
        {
          title: "System Type",
          path: "/systemtype",
        },
        {
          title: "System Verification Problems",
          path: "/systemverificationproblems",
        },
        // { title: "", path: "" },
        // {
        //   title: "Product Lifecycle Constraints",
        //   path: "setIsVisibleModalProductLifecycle(true)",
        // },
        // {
        //   title: readonly ? "" : "Initialize New Product",
        //   path: readonly ? "" : "setIsVisibleModalInitializeNewProduct(true)",
        // },
        {
          title: "Design Component",
          path: "/designcomponent",
          category: "Design Library",
        },
        {
          title: "Design Component Family",
          path: "/designcomponentfamily",
        },
        // {
        //   title: "Design Aspects",
        //   path: "/designAspect",
        // },
        {
          title: "Subnetwork Boundary",
          path: "/subnetwork",
        },
      ];
    } else if (option === "Network Plan") {
      return [
        {
          title: "LCM Engineering",
          path: "/lcmengineering",
          category: "Network Plan",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Design Aspects",
          path: "/designAspect",
        },
        {
          title: "Assets",
          path: "/asplanned",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Identities",
          path: "/Identities",
          disable: admin || readonly || simpleUser ? false : true,
        },
        { title: "", path: "" },
        {
          title: "Software Components Bag",
          path: "/componentswbag",
          category: "Bag Configuration",
          disable: admin || readonly || simpleUser ? false : true,
        },
        { title: "", path: "" },
        { title: "", path: "" },
        { title: "", path: "" },
        // {
        //   title: "Planned Activities",
        //   path: "/plannedActivities",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        {
          title: "",
          path: "",
          disable: admin || readonly || simpleUser ? false : true,
        },
        // {
        //   title: "",
        //   path: "",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },

        // {
        //   title: "Network Element As-Is",
        //   path: "/asis",
        //   category: "Feedback Loop",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        // {
        //   title: "Reconciliation",
        //   path: "/reconciliation",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        // {
        //   title: "",
        //   path: "",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        // {
        //   title: "",
        //   path: "",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        // {
        //   title: "",
        //   path: "",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        // {
        //   title: "",
        //   path: "",
        //   disable: admin || readonly || simpleUser ? false : true,
        // },
        {
          title: "Network Element",
          path: "/feedbackloop/networkelement",
          category: "Discovered Network Data",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Discovered Identities",
          path: "/feedbackloop/identity",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Hardware Configurations",
          path: "/feedbackloop/hardwareconfiguration",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Software Configurations",
          path: "/feedbackloop/softwareconfigurations",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Software Components",
          path: "/feedbackloop/softwarecomponent",
          disable: admin || readonly || simpleUser ? false : true,
        },
        // {
        //   title: KPIAdmin ? "VoLTE KPI Worklog & Approval" : "",
        //   path: "/targetmonthlyapprovals",
        //   category: KPIAdmin ? "Demand Tracking" : "",
        //   disable: KPIAdmin ? false : true,
        // },
        // {
        //   title: KPIAdmin || KPIEditor || readonly ? "VoLTE KPI Dashboard" : "",
        //   path: "/dashboard",
        //   category:
        //     !KPIAdmin && (KPIEditor || readonly) ? " Demand Tracking" : "",
        //   disable: KPIAdmin || KPIEditor || readonly ? false : true,
        // },
        // { title: "", path: "" },
        // { title: "", path: "" },
        // { title: "", path: "" },
        {
          title: "Network Element",
          path: "/asis",
          category: "Feedback Loop",
          disable: admin || readonly || simpleUser ? false : true,
          bracket: "As-Is",
        },
        { title: "", path: "" },
        { title: "", path: "" },
        { title: "", path: "" },
        { title: "", path: "" },
        // {
        //   title: "Bundle Upgrade Initiative",
        //   path: "/bundleupgradeinitiative",
        // },
        // { title: "NFVI (Transition)", path: "/nfvi" },
        // { title: "VNF (Transition)", path: "/vnf" },
      ];
    } else if (option === "Manage Network Plan") {
      return [
        {
          title: "LCM",
          path: "/lcmengineering",
          disable: admin || readonly || simpleUser ? false : true,
          category: "Create Planned Activity",
        },
        {
          title: "Assets",
          path: "/asplanned",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Design Aspects",
          path: "/designAspect",
          disable: admin || readonly || simpleUser ? false : true,
        },
        { title: "", path: "" },
        { title: "", path: "" },
        {
          title: "LCM - PA",
          path: "/plannedActivities/LCM",
          disable: admin || readonly || simpleUser ? false : true,
          category: "Manage Planned Activities (PA)",
        },
        {
          title: "Assets - PA",
          path: "/plannedActivities/Asset",
          disable: admin || readonly || simpleUser ? false : true,
        },
        {
          title: "Design Aspects - PA",
          path: "/plannedActivities/Design Aspect",
          disable: admin || readonly || simpleUser ? false : true,
        },
      ];
    } else if (option === "Manage Transformation") {
      return [
        // { title: "Planned Activities", path: "/plannedActivities" },
        {
          title: "Initialize New Product",
          path: "setIsVisibleModalInitializeNewProduct(true)",
          disable: admin || simpleUser ? false : true,
          category: "Application Utilities",
        },
        {
          title: "Manage Migrations",
          path: "setIsVisibleModalManage(true)",
          disable: admin || simpleUser ? false : true,
        },
        {
          title: "Update Planned Activity Status",
          path: "setIsVisibleModalStatus(true)",
          disable: admin || simpleUser ? false : true,
        },
        { title: "", path: "", disable: admin || simpleUser ? false : true },
        { title: "", path: "", disable: admin || simpleUser ? false : true },
        {
          title: "Reconciliation",
          path: "/reconciliation",
          disable: admin || readonly || simpleUser ? false : true,
          category: "Integration Utilities",
        },
        {
          title: `${admin || simpleUser ? "Delivery Tracking" : ""}`,
          path: `${admin || simpleUser ? "/deliveryTracking" : ""}`,
        },

        {
          title: "",
          path: "",
          disable: admin || simpleUser || readonly ? false : true,
        },
        {
          title: "",
          path: "",
          disable: admin || simpleUser || readonly ? false : true,
        },
        {
          title: "",
          path: "",
          disable: admin || simpleUser || readonly ? false : true,
        },
        {
          title: "Product Lifecycle Constraints",
          path: "setIsVisibleModalProductLifecycle(true)",
          category: "Product Lifecycle Constraints",
        },
      ];
    } else if (option === "Reports") {
      return [
        {
          title: "LCM Export",
          path: "/generatelcmdb",
          disable: admin || simpleUser ? false : true,
          category: "LCM Export",
        },
        {
          title: "Disaggregated Reports",
          path: "/generatelcmdbR10",
          disable: admin || simpleUser ? false : true,
        },
        {
          title: "VAI Export",
          path: "/vaiexport",
          disable: admin || simpleUser ? false : true,
        },
        {
          title: "Planned Activity Tracker",
          path: "/plannedActivityTracker",
          disable: admin || readonly || simpleUser || KPIAdmin ? false : true,
          category: admin || simpleUser ? "" : "LCM Export",
        },
        {
          title: "User Defined Reports",
          path: "/genericreports",
          disable: admin || readonly || simpleUser || KPIAdmin ? false : true,
        },
        // {
        //   title: "Generate VoLTE Dashboard",
        //   path: "/generatevoltedashboard",
        //   disable: !admin && true,
        // },
        {
          title: "",
          path: "",
          disable: admin || KPIEditor ? true : false,
        },
        {
          title: "",
          path: "",
          disable: admin || simpleUser || !readonly ? true : false,
        },
        {
          title: "",
          path: "",
          disable: admin || simpleUser || !readonly ? true : false,
        },
        {
          title: "",
          path: "",
          disable:
            !KPIAdmin ||
            admin ||
            (simpleUser && KPIAdmin && !KPIEditor) ||
            (readonly && KPIAdmin)
              ? true
              : false,
        },
        {
          title: "",
          path: "",
          disable:
            !KPIAdmin ||
            admin ||
            (simpleUser && KPIAdmin && KPIEditor) ||
            (simpleUser && KPIAdmin) ||
            (readonly && KPIAdmin)
              ? true
              : false,
        },
        {
          title: "",
          path: "",
          disable:
            (simpleUser && KPIEditor && !KPIAdmin && !admin) ||
            (readonly && KPIEditor) ||
            (KPIEditor && KPIAdmin && !simpleUser && !admin)
              ? false
              : true,
        },
        {
          title: KPIAdmin ? "VoLTE KPI Worklog & Approval" : "",
          path: "/targetmonthlyapprovals",
          category: KPIAdmin ? "Demand Tracking" : "",
          disable: KPIAdmin ? false : true,
        },
        {
          title: KPIAdmin || KPIEditor || readonly ? "VoLTE KPI Dashboard" : "",
          path: "/dashboard",
          category:
            !KPIAdmin && (KPIEditor || readonly) ? "Demand Tracking" : "",
          disable: KPIAdmin || KPIEditor || readonly ? false : true,
        },
        {
          title: "Generate VoLTE Dashboard",
          path: "/generatevoltedashboard",
          disable: !admin && true,
          category: KPIAdmin || KPIEditor ? "" : "Demand Tracking",
        },
        { title: "", path: "", disable: simpleUser && !admin ? true : false },
        { title: "", path: "", disable: simpleUser && !admin ? true : false },
        {
          title: "",
          path: "",
          disable: !admin || KPIAdmin ? true : false,
        },
        {
          title: "",
          path: "",
          disable: !admin || KPIAdmin || KPIEditor ? true : false,
        },
        {
          title: "PA Report",
          path: "/plannedactivityreport",
          category: "Graphical Reports",
          disable: admin ? false : true,
        },
        {
          title: "Asset Overview by Market",
          path: "/assetoverviewbymarket",
          disable: admin ? false : true,
        },
        {
          title: "Network Visualizer (Under Development)",
          path: "/networkvisualizer",
          disable: admin ? false : true,
        },
        // {
        //   title: "LCM @Glance - V1",
        //   path: "/lcmatglance/v1",
        //   disable: admin ? false : true,
        // },
        {
          title: "LCM @Glance",
          path: "/lcmatglance",
          disable: admin ? false : true,
        },
        {
          title: "Exodus @Glance (Under Development)",
          path: "/exodusatglance",
          disable: admin ? false : true,
        },
        {
          title: "Exodus-Asset Timeline (Under Development)",
          path: "/exodusassettimeline",
          disable: admin ? false : true,
        },
        {
          title: "",
          path: "",
          disable: !admin ? true : false,
        },
        {
          title: "Asset Pivot By Location",
          path: "/assetpivotbylocation",
          category: "Grid Reports",
          disable: admin ? false : true,
        },
        {
          title: "All - PA",
          path: "/plannedActivities/All",
          disable: admin || readonly || simpleUser ? false : true,
        },
      ];
    } else if (option === "Administration") {
      return [
        {
          title: "Settings Update Planned Activity",
          path: "/settingsupdateplannedactivity",
          category: "Application Settings",
        },
        {
          title: "Configure Planned Activity Type",
          path: "/plannedactivitytype",
        },
        { title: "SW App - VF Name", path: "/vodafoneName" },
        { title: "General Settings Page Size", path: "/generalsettings" },
        // { title: "Glossary", path: "/glossary" },
        // {
        //   title: "Generic Report Definition",
        //   path: "/genericreporting",
        //   disable: !admin && true,
        // },
        { title: "", path: "" },
        { title: "", path: "" },
        // { title: "Refactor", path: "ResetForeignIndexConfirm()" },
        {
          title: "Resource Key Master",
          path: "/resourcekeymaster",
          category: "Resource Lifecycle",
        },
        { title: "DCF Life Cycle", path: "/dcflifecycle" },
        { title: "", path: "" },
        { title: "", path: "" },
        { title: "", path: "" },
        {
          title: "Log Management",
          path: "/userLogLevel",
          category: "Application Management",
        },
        { title: "User Management", path: "/usermanagement" },
        // { title: "Organization Info", path: "/organizationinfo" },
        { title: "", path: "" },
        { title: "", path: "" },
        {
          title: "Worklog & Approval",
          path: "/feedbackloop/audit",
          category: "Discovered Data",
        },
        {
          title: "Asset Mapping",
          path: "/assetmapinfo",
          category: "Discovered Data",
        },
        // { title: "Graph", path: "/charts" },
      ];
    } else {
      return [
        {
          title: "Hardware",
          path: "/majorhardware",
        },
        {
          title: "Software",
          path: "/majorsoftware",
        },
        {
          title: "LCM Engineering",
          path: "/lcmengineering",
        },
        {
          title: "Assets",
          path: "/asplanned",
        },
        {
          title: "Product Lifecycle",
          path: "setIsVisibleModalProductLifecycle(true)",
        },
        {
          title: "Design Component",
          path: "/designcomponent",
        },
        {
          title: "Design Aspects",
          path: "/designAspect",
        },
        {
          title: "Planned Activity Tracker",
          path: "/plannedActivities",
        },
        {
          title: "Identities",
          path: "/feedbackloop/identity",
        },
        {
          title: "LCM Export",
          path: "/generatelcmdb",
        },
        {
          title: "Disaggregated Reports",
          path: "/generatelcmdbR10",
        },
        // {
        //   title: "Explore",
        //   path: "/",
        // },
        // {
        //   title: "TEMS Home",
        //   path: "/",
        // },
      ];
    }
  };

  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };
  const ConfirmReset = async () => {
    await ResetForeignIndex().then((x) => {
      setConfirm(stateConfirm);
    });
  };
  const ResetForeignIndexConfirm = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to reset Refactor Foreign Index Session?",
      button: "Reset",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => ConfirmReset(),
      },
    });
  };

  useEffect(() => {
    setMenus(getMenusForOption(selectedOption));
    setMenuDescriptions(getDescriptionForOption(selectedOption));
  }, [selectedOption, readonly, KPIAdmin, KPIEditor, admin, simpleUser]);

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
      <Container>
        {menuDescriptions && (
          <div
            className={`  ${
              selectedOption ? "landing_sub_header" : "landing_header"
            }`}
          >
            <img
              src={logo}
              className={`  ${
                selectedOption
                  ? "landing_sub_header_logo"
                  : "landing_header_logo"
              }`}
              alt="TEMS Logo"
            />
            <div
              className={` ${
                selectedOption
                  ? "landing_sub_header_title"
                  : "landing_header_title"
              }`}
            >
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

      {/* menus */}
      <div data-nameid="MainContent-Wireframe" className="main_menus">
        {/* items */}
        <>
          {/* <div className="mb-4"></div> */}

          {selectedOption && (
            <Row className="row-cols-5 mt-5" role="submenu">
              {menus?.map((menu, index) => {
                return (
                  <React.Fragment key={index}>
                    {menu.title && !menu.disable ? (
                      <Col
                        key={menu.title}
                        // onClick={() => handleMenuClick(menu.path)}
                        className={`${selectedOption ? "pt-2" : ""}`}
                      >
                        {menu.category && (
                          <span className="category">{menu.category}</span>
                        )}
                        {menu.path.includes("/") ? (
                          <>
                            <a
                              tabIndex={2}
                              className="menu_items"
                              href={menu.path}
                              onKeyDown={(e) => {
                                if (e.key === "Enter") {
                                  handleMenuClick(menu.path);
                                }
                              }}
                              onClick={(event) => {
                                event.preventDefault();
                                const newTab = menu.path.includes("/")
                                  ? event.ctrlKey || event.metaKey
                                  : false;
                                handleMenuClick(menu.path, newTab);
                              }}
                            >
                              <div>
                                <p>{menu.title}</p>
                                {menu.bracket && <p>({menu.bracket})</p>}
                              </div>
                            </a>
                          </>
                        ) : (
                          <>
                            <div
                              tabIndex={2}
                              className="menu_items"
                              onKeyDown={(e) => {
                                if (e.key === "Enter") {
                                  handleMenuClick(menu.path);
                                }
                              }}
                              onClick={(event) => {
                                event.preventDefault();
                                const newTab = menu.path.includes("/")
                                  ? event.ctrlKey || event.metaKey
                                  : false;
                                handleMenuClick(menu.path, newTab);
                              }}
                            >
                              <p>{menu.title}</p>
                            </div>
                          </>
                        )}
                      </Col>
                    ) : (
                      <Col
                        key={index}
                        style={menu.disable ? { display: "none" } : {}}
                      ></Col>
                    )}
                  </React.Fragment>
                );
              })}
            </Row>
          )}

          {!selectedOption && <TreeViewMenu />}
        </>
      </div>
      <ModalConfirm data={confirm} />
    </div>
  );
};

export default LandingPage;
