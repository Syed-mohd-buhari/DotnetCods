import React, { useEffect, useState } from "react";
import { Dropdown } from "react-bootstrap";
import {
  removeAccessToken,
  setAccessToken,
  setErrorMessage,
} from "../../../Redux/Action/AuthenticationAction";
import { DataModalConfirm, stateConfirm } from "../../../Model/Common";

import "./nav-header.css";
import ModalConfirm from "../../../Components/ModalConfirm";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../../Redux/Store/rootStore";
import { useNavigate, useLocation } from "react-router-dom";
import { ChangeDbMode } from "../../../Redux/Action/ViaExport/ViaExportDownloadAction";
import { useAuth } from "./../../../Hook/useAuth";
import { getAzureData } from "../../../Business/getAzureBusiness";
import setLoader from "../../../Redux/Action/LoaderAction";
import { setNotification } from "../../../Redux/Action/NotificationAction";
import { NotifyType } from "../../../Redux/Reducer/NotificationReducer";
// change_db_mode
const NavHeader = () => {
  const navigate = useNavigate();
  const location: any = useLocation();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [dbModeMessage, setDbModeMessage] = useState("normal");

  const { operatingMode } = useAuth();

  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );

  const CancelConfirm = () => {
    setConfirm(stateConfirm);
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
    if (localStorage.getItem("TYPE") === "LOC") {
      onGetHome();
    } else {
      //authProvider.logout();
      onGetHome();
    }
    removeAccessToken();
  };

  const onGetHome = () => {
    navigate("/");
  };

  // useEffect(() => {
  //   console.log("operatingMode in header => ", operatingMode);
  // }, [operatingMode]);

  //account

  return (
    // ChangeModeConfirm
    <div className="nav-c">
      <div className="container-fluid flex">
        <img
          src={require("../../../img/Vodafone_Logo.png")}
          onClick={onGetHome}
        />
        {operatingMode === "training" && (
          <p className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab">
            You are currently running on {operatingMode} mode
          </p>
        )}

        <div className="avatar flex">
          <p>{userInfo?.account.name} </p>
          <Dropdown>
            <Dropdown.Toggle id="user-menu">
              <a
                onClick={LogOutAndChangeModeConfirm}
                className="w-100"
                style={{ display: "block" }}
              >
                <img
                  src={require("../../../img/user.png")}
                  alt="logOut"
                  title="logout"
                />
              </a>
            </Dropdown.Toggle>
          </Dropdown>
        </div>
      </div>
      <ModalConfirm data={confirm} />
    </div>
  );
};

export default NavHeader;
