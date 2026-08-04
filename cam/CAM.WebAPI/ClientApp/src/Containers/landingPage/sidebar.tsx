// Sidebar.js
import React, { FC, useState, useEffect, useRef, useContext } from "react";
import "./sidebar.css";
import drawerOpenIcon from "./skins/drawer_open.png";
import drawerCloseIcon from "./skins/drawer_close.png";
import drawerOpenLightIcon from "./skins/drawer_open_light.png";
import drawerCloseLightIcon from "./skins/drawer_close_light.png";
import { FormControl, InputGroup } from "react-bootstrap";
import { FaSearch } from "react-icons/fa";
import { useLocation, useNavigate } from "react-router";
import { useModal } from "../../Hook/useModal";
import { useTheme } from "../../Context/ThemeContext";
import { OptionContext } from "../../Context/MenuOptionContext";
import { useAuth } from "../../Hook/useAuth";
import { GiDetour } from "react-icons/gi";
import Logo from "./skins/vf_tv_play_clr_logo_white_cmyk_1.png";
import HomeBlack from "./skins/Home_black.png";
import AboutBlack from "./skins/About icon_black.png";
import ContactBlack from "./skins/Contact icon_black.png";
import LibraryBlack from "./skins/Library icon_black.png";
import HomeWhite from "./skins/Home_white.png";
import AboutWhite from "./skins/About icon_white.png";
import ContactWhite from "./skins/Contact icon_white.png";
import LibraryWhite from "./skins/Library icon_white (2).png";
import GlossaryDark from "./skins/Glossary_black.png";
import GlossaryLight from "./skins/Glossary_white.png";
import GlossaryGrey from "./skins/Glossary_grey.png";
import PlannedActivityLight from "./skins/Planned Activity_white.png";
import PlannedActivityDark from "./skins/Planned Activity_dark.png";
import PlannedActivityGrey from "./skins/Planned Activity_grey.png";
import ReportLight from "./skins/Report_white.png";
import ReportDark from "./skins/Report_black.png";
import ReportGrey from "./skins/Report_grey.png";
import LCMExportLight from "./skins/Export_white.png";
import LCMExportDark from "./skins/Export_black.png";
import LCMExportGrey from "./skins/Export_grey.png";
import LCMEngLight from "./skins/LCM Eng_white.png";
import LCMEngDark from "./skins/LCM Eng_black.png";
import LCMEngGrey from "./skins/LCM Eng_grey.png";
import GenReportLight from "./skins/Gen Report_white.png";
import GenReportDark from "./skins/Gen Report_black.png";
import GenReportGrey from "./skins/Gen Report_grey.png";
import { Link } from "react-router-dom";
import { MdTour } from "react-icons/md";
import { useDispatch } from "react-redux";
import { startGuideTour, startTour } from "../../Redux/Action/tourActions";
import { FaPersonChalkboard, FaPersonMilitaryPointing } from "react-icons/fa6";
import { IoBarChartOutline } from "react-icons/io5";
import { BsRocketTakeoff } from "react-icons/bs";
type MenuItem = {
  title: string;
  icon: any;
  isIcon?: boolean;
  newTab: boolean;
  path?: string;
  iconDark?: string;
  iconAlt?: string;
  disable?: boolean;
  children?: MenuItem[];
};
const Sidebar = () => {
  const sidebarRef = useRef<HTMLDivElement>(null);
  const { darkMode } = useTheme();
  const location = useLocation();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [hoveredMenu, setHoveredMenu] = useState("");

  const { selectedOption, setSelectedOption } = useContext(OptionContext);
  const { onClickLink } = useModal();

  const handleMenuClick = (path: string, newTab: boolean) => {
    if (path.includes("/")) {
      setSelectedOption("");
      if (location.pathname !== path) {
        if (newTab) {
          window.open(path, "_blank");
        } else {
          navigate(path);
        }
      }
    }
  };

  const {
    operatingMode,
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
  const SWITCHABLE_ROLES = ["SW Product Owner", "HW Product Owner"];
  const filteredRoleList = (role ?? []).filter((r: string) =>
    SWITCHABLE_ROLES.includes(r)
  );

  const [menusList, setMenusList] = useState<MenuItem[]>([]);

  const menus: MenuItem[] = [
    {
      title: "Home",
      icon: Logo,
      path: "/home",
      iconAlt: "Vodafone Icon",
      isIcon: true,
      newTab: false,
    },
    {
      title: "Glossary",
      icon: darkMode ? GlossaryLight : GlossaryGrey,
      isIcon: true,
      newTab: false,
      path: "/glossary",
      iconDark: GlossaryDark,
      // disable: !tipologicaPermesso,
      iconAlt: "Glossary Icon",
    },
    {
      title: "New Report Definition",
      icon: darkMode ? GenReportLight : GenReportGrey,
      isIcon: true,
      newTab: false,
      path: "/genericreporting",
      iconDark: GenReportDark,
      // disable: !admin,
      // iconAlt: "New Report Definition Icon",
    },
    {
      title: "LCM Engineering",
      icon: darkMode ? LCMEngLight : LCMEngGrey,
      isIcon: true,
      newTab: false,
      path: "/lcmengineering",
      iconDark: LCMEngDark,
      // disable: !(admin || simpleUser || readonly),
      iconAlt: "LCM Engineering Icon",
    },
    {
      title: "Exports",
      icon: darkMode ? LCMExportLight : LCMExportGrey,
      isIcon: true,
      newTab: false,
      iconDark: LCMExportDark,
      disable: !(admin || simpleUser || readonly),
      children: [
        {
          title: "LCM Export",
          newTab: false,
          icon: darkMode ? LCMExportLight : LCMExportGrey,
          path: "/generatelcmdb",
          // disable: !(admin || simpleUser || readonly),
        },
        {
          title: "Disaggregated Reports",
          newTab: false,
          icon: darkMode ? LCMExportLight : LCMExportGrey,
          path: "/generatelcmdbR10",
          // disable: !(admin || simpleUser || readonly),
        },
      ],
    },
    {
      title: "Lcm @Glance",
      icon: <IoBarChartOutline size={20} color="#000000" />,
      isIcon: false,
      newTab: true,
      path: "/lcmatglance",
      iconDark: LCMExportDark,
      // disable: !(admin || simpleUser || readonly),
      iconAlt: "LCM Glance Icon",
    },
  ];

  useEffect(() => {
    if (userPrefrenceDetails) {
      const combinedUserPrefrenceMenus = [
        ...userPrefrenceDetails?.pagePrefrenceDetail,
        ...userPrefrenceDetails?.popupPagePrefrenceDetail,
      ];
      const userPreferencePathList: any = new Set(
        combinedUserPrefrenceMenus?.map((item) => item.path)
      );

      const filteredResult: any = menus
        .filter((item) => {
          if (item.path === "/home" || item.path === "/glossary") return true;
          if (item.path === "/genericreporting") {
            const genericReportPermission =
              userPrefrenceDetails?.pagePrefrenceDetail?.find(
                (item) => item.path === "/genericreports"
              )?.screenPermission;
            return genericReportPermission === 7;
          }
          if (item.path && userPreferencePathList.has(item.path)) return true;
          if (
            item.children &&
            item.children.length > 0 &&
            item.children.some((child) =>
              userPreferencePathList.has(child.path)
            )
          )
            return true;
          return false;
        })
        .map((item) => {
          if (item.children && item.children.length > 0) {
            return {
              ...item,
              children: item.children.filter((child) =>
                userPreferencePathList.has(child.path)
              ),
            };
          }
          return item;
        });
      setMenusList(filteredResult);
    }
  }, [userPrefrenceDetails]);

  const handleTourGuideClick = () => {
    dispatch(startGuideTour());
    handleMenuClick("/home", false);
  };

  const handleSwTourGuideClick = () => {
    dispatch(startGuideTour());
    handleMenuClick("/majorsoftware", false);
  };

  return (
    <nav ref={sidebarRef} className="sidebar">
      <ul className="sidebar-nav nav-list topSidebar" role="menu">
        {menusList.map((menu) => {
          if (menu.disable) return null;

          const isHovered = hoveredMenu === menu.title;
          return (
            <li
              key={menu.title}
              onClick={() => {
                if (menu.path) handleMenuClick(menu.path, menu.newTab);
                setHoveredMenu("");
              }}
              tabIndex={1}
              role="menuitem"
              onMouseEnter={() => setHoveredMenu(menu.title)}
              onFocus={() => setHoveredMenu(menu.title)}
              onMouseLeave={() => setHoveredMenu("")}
              onBlur={() => setHoveredMenu("")}
              className={`${
                menu.title !== "Home" && isHovered && "sidebar_list"
              }`}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  e.stopPropagation();
                  if (menu.path) handleMenuClick(menu.path, menu.newTab);
                }
              }}
            >
              <div
                className={`${
                  menu.title === "Home" ? "logo" : "side_menu_items"
                }`}
              >
                {!menu.isIcon ? (
                  menu.icon
                ) : (
                  <img
                    src={
                      menu.title !== "Home" && isHovered
                        ? menu.iconDark
                        : menu.icon
                    }
                    className="menu_items_image"
                    alt={menu.iconAlt}
                  />
                )}
                {/* Parent title or tooltip */}
                {!menu.children && menu.title !== "Home" && (
                  <span className="tooltip">{menu.title}</span>
                )}
              </div>

              {menu?.children && isHovered && (
                <div className="submenu-wrapper">
                  <div className="submenu">
                    {menu.children.map((child, index) => (
                      <div
                        key={index}
                        className="submenu-item"
                        onClick={() => {
                          if (child.path)
                            handleMenuClick(child.path, menu.newTab);
                          setHoveredMenu("");
                        }}
                        onKeyDown={(e) => {
                          if (e.key === "Enter") {
                            e.stopPropagation();
                            if (child.path)
                              handleMenuClick(child.path, menu.newTab);
                          }
                        }}
                        tabIndex={1}
                      >
                        <span className="submenu-text">{child.title}</span>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </li>
          );
        })}
      </ul>
      {filteredRoleList?.length > 0 && (
        <ul className="sidebar-nav" role="menu">
          <li
            onClick={() => navigate("/overview")}
            tabIndex={1}
            role="menuitem"
            className={`sidebar_list`}
          >
            <div className="side_menu_items">
              <BsRocketTakeoff size={22} color="#343a40" />
              <span className="tooltip">Switch to New Portal</span>
            </div>
          </li>
        </ul>
      )}
      {/* Tour guide start */}
      {/* <ul className="sidebar-nav" role="menu">
        <li
          onClick={handleTourGuideClick}
          tabIndex={1}
          role="menuitem"
          onMouseEnter={() => setHoveredMenu("Start Tour")}
          onFocus={() => setHoveredMenu("Start Tour")}
          onMouseLeave={() => setHoveredMenu("")}
          onBlur={() => setHoveredMenu("")}
          className={`sidebar_list tour-guide-item ${
            hoveredMenu === "Start Tour" ? "hovered" : ""
          }`}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              e.stopPropagation();
              handleTourGuideClick();
            }
          }}
        >
          <div className="side_menu_items">
            <FaPersonChalkboard size={25} color="#343a40" />
            <span className="tooltip">Start Tour</span>
          </div>
        </li>
      </ul> */}
    </nav>
  );
};

export default Sidebar;
