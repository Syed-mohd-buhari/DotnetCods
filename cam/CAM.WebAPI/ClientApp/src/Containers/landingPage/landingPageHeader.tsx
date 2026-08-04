import React, { FC, useContext, useEffect, useState, useRef } from "react";
import { useSelector } from "react-redux";
import { Dropdown } from "react-bootstrap";
import { useLocation, useNavigate } from "react-router-dom";

import { useAuth } from "../../Hook/useAuth";
import { useTheme } from "../../Context/ThemeContext";
import ModalConfirm from "../../Components/ModalConfirm";
import DarkMode from "../../Components/DarkMode/DarkMode";
import { OptionContext } from "../../Context/MenuOptionContext";
import { getAzureData } from "../../Business/getAzureBusiness";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";

import {
  setAccessToken,
  removeAccessToken,
} from "../../Redux/Action/AuthenticationAction";
import setLoader from "../../Redux/Action/LoaderAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { ChangeDbMode } from "../../Redux/Action/ViaExport/ViaExportDownloadAction";

import "./landingPageHeader.css";
import userDark from "./skins/user_dark.png";
import logoutRed from "./skins/logout_red.png";
import userPng from "../../../src/img/user.png";
import color_mode from "./skins/color_mode.png";
import color_modeLight from "./skins/Colour mode icon - dark mode.png";
import arrowDark from "./skins/arrowDark.png";
import arrowLight from "./skins/Chevron-down - dark mode.png";
import settingDark from "./skins/settingDark.png";
import settingLight from "./skins/settings icon_dark mode.png";
import operation_mode from "./skins/operation_mode.png";
import operation_modeLight from "./skins/Operating mode icon - dark mode.png";
import logo from "./skins/vf_tv_play_clr_logo_white_cmyk_1.png";
import { MdAutoAwesome, MdHome } from "react-icons/md";
import { FaHome } from "react-icons/fa";

const LandingPageHeader: FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { selectedOption, setSelectedOption } = useContext(OptionContext);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [dbModeMessage, setDbModeMessage] = useState("normal");
  const [profileInfoView, setProfileInfoView] = useState(false);
  const { darkMode } = useTheme();
  const [settingOpen, setSettingOpen] = useState(false);
  const [arrowRotation, setArrowRotation] = useState(270);
  const [menuTabList, setMenuTabList] = useState<any>(null);
  const [operatingModeStatus, setOperatingModeStatus] = useState(false);
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
  const { toggleDarkMode } = useTheme();

  const profileRef = useRef<HTMLDivElement>(null);
  const profileTriggerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (
        profileRef.current &&
        !profileRef.current?.contains(event.target) &&
        !profileTriggerRef.current?.contains(event.target)
      ) {
        setProfileInfoView(false);
      }
    };

    document.body.addEventListener("click", handleClickOutside);

    return () => {
      document.body.removeEventListener("click", handleClickOutside);
    };
  }, []);
  useEffect(() => {
    const status = operatingMode === "normal" ? false : true;
    setOperatingModeStatus(status);
  }, [operatingMode]);
  const goToHomepage = (clearMenu) => {
    if (location.pathname !== "/home" && clearMenu) {
      navigate("/home");
    }
    if (clearMenu) setSelectedOption("");
  };

  const handleOptionChange = (e, url = "") => {
    setSelectedOption(e.target.textContent);
    navigate(`/${url}`);
    // goToHomepage(false);
  };
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };

  useEffect(() => {
    if (userPrefrenceDetails) {
      const combinedUserPrefrenceMenus = [
        ...userPrefrenceDetails?.pagePrefrenceDetail,
        ...userPrefrenceDetails?.popupPagePrefrenceDetail,
      ];
      const menuList = [
        ...new Set(combinedUserPrefrenceMenus?.map((item) => item.menu)),
      ];
      setMenuTabList(menuList);
    }
  }, [userPrefrenceDetails]);
  const handleModeChange = () => {
    setOperatingModeStatus((prev) => !prev);
    let mode = operatingMode === "training" ? "normal" : "training";
    ChangeDbMode(mode)
      .then(async (res) => {
        console.log(mode);
        await getAzureData({
          email: localStorage.getItem("UserName")!,
          isAuthenicated: true,
        }).then((res) => {
          if (res.data.token !== "") {
            setAccessToken(
              res.data.token,
              +res.data.period,
              res.data.refreshToken.token
            );
          } else {
            // localStorage.removeItem("DEV_token");
            localStorage.clear();

            rootStore.dispatch(
              setNotification({
                message: res.data.errorMessage,
                notifyType: NotifyType.error,
              })
            );
            setLoader("REMOVE", "ChangeDbMode");
            setTimeout(() => {
              window.location.href = "/";
            }, 3000);
            return;
          }
          // setLoader("REMOVE", "");
        });
        setDbModeMessage(mode!);
        window.location.href = "/";
      })
      .catch((err) => {});
  };
  const ChangeModeConfirm = () => {
    var x = localStorage.getItem("UserName");
    setConfirm({
      title: "Operating Mode",
      message: "Please select operating mode.",
      button: "Confirm",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: (mode?: string) => {
          ChangeDbMode(mode)
            .then(async (res) => {
              console.log(mode);
              await getAzureData({
                email: localStorage.getItem("UserName")!,
                isAuthenicated: true,
              }).then((res) => {
                if (res.data.token !== "") {
                  setAccessToken(
                    res.data.token,
                    +res.data.period,
                    res.data.refreshToken.token
                  );
                } else {
                  // localStorage.removeItem("DEV_token");
                  localStorage.clear();

                  rootStore.dispatch(
                    setNotification({
                      message: res.data.errorMessage,
                      notifyType: NotifyType.error,
                    })
                  );
                  setLoader("REMOVE", "ChangeDbMode");
                  setTimeout(() => {
                    window.location.href = "/";
                  }, 3000);
                  return;
                }
                // setLoader("REMOVE", "");
              });
              setDbModeMessage(mode!);
              window.location.href = "/";
            })
            .catch((err) => {
              console.log("catch: ", err);
            });
        },
      },
    });
  };
  const LogOutConfirm = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to quit? Unsaved changes will be lost.",
      button: "Logout",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => {
          setSelectedOption("");
          onLogout();
        },
      },
    });
  };
  const LogOutAndChangeModeConfirm = () => {
    setConfirm({
      title: "Choose Action",
      message: "Here you can logout or change operating mode",
      button: "Logout",
      buttonSecond: "Change Operating Mode",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => LogOutConfirm(),
        confirmSecond: () => ChangeModeConfirm(),
      },
    });
  };
  const onLogout = () => {
    console.log('localStorage.getItem("TYPE")', localStorage.getItem("TYPE"));
    navigate("/");
    if (localStorage.getItem("TYPE") === "LOC") {
      goToHomepage(false);
    } else {
      //authProvider.logout();
      goToHomepage(false);
    }
    removeAccessToken();
  };
  const handleSetting = () => {
    setSettingOpen((prev) => !prev);
  };

  useEffect(() => {
    if (settingOpen) {
      setArrowRotation(0);
    } else {
      setArrowRotation(270);
    }
  }, [settingOpen]);

  return (
    <div className="main_header" tabIndex={-1}>
      <ModalConfirm data={confirm} />
      <section onClick={() => goToHomepage(true)} tabIndex={-1}>
        <img src={logo} className="logo__main" alt="vodafone logo" />
        {/* <p className="title__main">TEMS</p> */}
      </section>
      <section className="main_menu" role="menu">
        {menuTabList && menuTabList?.includes("Product and Design") && (
          <p
            onClick={(e) => handleOptionChange(e, "productanddesign")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
              }
            }}
            className={
              selectedOption === "Product & Design" ? "selected_option" : ""
            }
            tabIndex={1}
            role="menuitem"
          >
            Product & Design
          </p>
        )}
        {menuTabList && menuTabList?.includes("Network Plan") && (
          <p
            onClick={(e) => handleOptionChange(e, "networkplan")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleOptionChange(e, "networkplan");
              }
            }}
            className={
              selectedOption === "Network Plan" ? "selected_option" : ""
            }
            tabIndex={1}
            role="menuitem"
          >
            Network Plan
          </p>
        )}
        {menuTabList && menuTabList?.includes("Manage Network Plan") && (
          <p
            onClick={(e) => handleOptionChange(e, "managenetworkplan")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleOptionChange(e, "managenetworkplan");
              }
            }}
            className={
              selectedOption === "Manage Network Plan" ? "selected_option" : ""
            }
            tabIndex={1}
            role="menuitem"
          >
            Manage Network Plan
          </p>
        )}
        {menuTabList && menuTabList?.includes("Manage Transformation") && (
          <p
            onClick={(e) => handleOptionChange(e, "managetransformation")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleOptionChange(e, "managetransformation");
              }
            }}
            className={
              selectedOption === "Manage Transformation"
                ? "selected_option"
                : ""
            }
            tabIndex={1}
            role="menuitem"
          >
            Manage Transformation
          </p>
        )}

        {menuTabList && menuTabList?.includes("Reports") && (
          <p
            onClick={(e) => handleOptionChange(e, "reports")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleOptionChange(e, "Reports");
              }
            }}
            className={selectedOption === "Reports" ? "selected_option" : ""}
            tabIndex={1}
            role="menuitem"
          >
            Reports
          </p>
        )}
        {menuTabList && menuTabList?.includes("Administration") && (
          <p
            onClick={(e) => handleOptionChange(e, "administration")}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleOptionChange(e, "Administration");
              }
            }}
            className={
              selectedOption === "Administration" ? "selected_option" : ""
            }
            tabIndex={1}
            role="menuitem"
          >
            Administration
          </p>
        )}
      </section>
      <section style={{ position: "relative" }} tabIndex={-1}>
        {operatingMode === "training" && (
          <span
            className="voda-bold btn btnHeader flex flex-gab"
            style={{ background: "#E60000", color: "white" }}
            tabIndex={-1}
          >
            You are currently running on {operatingMode} mode
          </span>
        )}

        {/* {location?.pathname !== "/home" ? (
          <div onClick={() => goToHomepage(true)}>
            <MdAutoAwesome size={24} color="white" />
            <MdHome size={24} color="white" style={{ marginLeft: "-4px" }} />
          </div>
        ) : (
          <button
            className="home-button"
            onClick={() => navigate("/oldlandingpage")}
          >
            <MdHome size={24} />
          </button>
        )} */}
        <div
          className="avatar flex profile-section"
          ref={profileTriggerRef}
          onClick={(e) => {
            e.stopPropagation();
            setProfileInfoView((prev) => !prev);
          }}
          tabIndex={1}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              e.stopPropagation();
              setProfileInfoView((prev) => !prev);
            }
          }}
        >
          <Dropdown tabIndex={-1}>
            <Dropdown.Toggle id="user-menu" tabIndex={-1}>
              <a className="w-100" style={{ display: "block" }} tabIndex={-1}>
                <img
                  src={userPng}
                  alt="user icon"
                  title="User Icon"
                  style={{ width: "100%", height: "100%" }}
                />
              </a>
            </Dropdown.Toggle>
          </Dropdown>
        </div>
        {profileInfoView && (
          <div className="profile-card" ref={profileRef}>
            <div className="profile">
              <div className="profile_pic">
                <img src={userDark} alt="Profile icon" title="Profile Icon" />
              </div>
              <div className="profile_info">
                <div className="user_name">
                  {userInfo?.account.name
                    .split("@")[0]
                    .trim()
                    .replace(/\./g, " ")
                    .replace(/(^\w|\.\s*\w)/g, function (char) {
                      return char.toUpperCase();
                    })}
                </div>
                <div className="user_mail">{userInfo?.account.name}</div>
              </div>
            </div>
            <div
              className="setting"
              tabIndex={1}
              onClick={handleSetting}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  e.stopPropagation();
                  handleSetting();
                }
              }}
            >
              <div className="setting-info">
                <img
                  src={darkMode ? settingLight : settingDark}
                  alt="setting-icon"
                  title="setting-icon"
                  className="setting-icon"
                />
                <span className="setting-title">Settings</span>
              </div>
              <div className="setting-options">
                <img
                  src={darkMode ? arrowLight : arrowDark}
                  alt="arrow-icon"
                  title="arrow-icon"
                  style={{ transform: `rotate(${arrowRotation}deg)` }}
                  className="setting-option-icon"
                />
              </div>
            </div>
            {/* Settings */}
            {settingOpen && (
              <div className="settings">
                <div
                  className="options"
                  tabIndex={1}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      e.stopPropagation();
                      toggleDarkMode();
                    }
                  }}
                >
                  <div className="option-info">
                    <img
                      src={darkMode ? color_modeLight : color_mode}
                      alt="logOut"
                      title="logout"
                      className="option-icon"
                    />
                    <span className="option-title">Colour mode</span>
                  </div>
                  <div className="option-toggler">
                    <DarkMode />
                    <div className="active-option">
                      {darkMode ? "Dark" : "Light"}
                    </div>
                  </div>
                </div>
                <div
                  className="options"
                  tabIndex={1}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      e.stopPropagation();
                      handleModeChange();
                    }
                  }}
                >
                  <div className="option-info">
                    <img
                      src={darkMode ? operation_modeLight : operation_mode}
                      alt="logOut"
                      title="logout"
                      className="option-icon"
                    />
                    <span className="option-title">Operating mode</span>
                  </div>
                  <div className="option-toggler">
                    <div className="operating_mode">
                      <input
                        className="operating_mode_input"
                        type="checkbox"
                        id="operating_mode-toggle"
                        onChange={handleModeChange}
                        checked={operatingModeStatus}
                      />
                      <label
                        className="operating_mode_label"
                        htmlFor="operating_mode-toggle"
                      ></label>
                    </div>
                    <div className="active-option">
                      {operatingMode === "training" ? "Training" : "Normal"}
                    </div>
                  </div>
                </div>
              </div>
            )}
            <div
              className="logout"
              tabIndex={1}
              onClick={LogOutConfirm}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  e.stopPropagation();
                  LogOutConfirm();
                }
              }}
              onBlur={() => setProfileInfoView(false)}
            >
              <img src={logoutRed} alt="logOut" title="logout" />
              <span>Log out</span>
            </div>
          </div>
        )}
      </section>
    </div>
  );
};

export default LandingPageHeader;
