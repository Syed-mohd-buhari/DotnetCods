import React, { useEffect, useRef, useState } from "react";

import {
  getAccessToken,
  removeAccessToken,
  setAccessToken,
  setErrorMessage,
} from "./Redux/Action/AuthenticationAction";

import { getAzureData } from "./Business/getAzureBusiness";
import setLoader from "./Redux/Action/LoaderAction";
import { decode as base64_decode, encode as base64_encode } from "base-64";
import { verifyPermesso } from "./Redux/Action/AuthenticationCheckAction";
import { NotifyType } from "./Redux/Reducer/NotificationReducer";
import { setNotification } from "./Redux/Action/NotificationAction";

import { stateConfirm, DataModalConfirm } from "./Model/Common";
import ModalConfirm from "./Components/ModalConfirm";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "./Redux/Store/rootStore";
import { useAuth } from "./Hook/useAuth";
import Loader from "./Components/Loader";
import ErrorNotification from "./Components/ErrorNotification";
import Header from "./Containers/header/header";
import {
  Routes,
  Route,
  Navigate,
  useNavigate,
  useLocation,
  Outlet,
} from "react-router-dom";
import SystemType from "./Containers/SystemTypeContainer";
import TestInfo from "./Containers/TestInfoContainer";
import Identities from "./Containers/IdentitiesContainer";
import SettingsUpdatePlannedActivity from "./Containers/SettingsUpdatePlannedActivityContainer";
import NetworkElementAsPlanned from "./Containers/NetworkElementAsPlannedContainer";
import NetworkElementAsIs from "./Containers/NetworkElementAsIsContainer";
import NewNetworkElementAsIs from "./Containers/NewNetworkElementAsIsContainer";
import Identity from "./Containers/IdentityContainer";
import HardwareConfiguration from "./Containers/HardwareConfiguration";
import SoftwareConfiguration from "./Containers/SoftwareConfiguration";
import SoftwareComponent from "./Containers/SoftwareComponent";
import DashboardContainer from "./Containers/VolteKPI/DashboardContainer";
import ViaExportContainer from "./Containers/ViaExportContainer";
import WorklogApprovalContainer from "./Containers/WorklogApprovalContainer";
import DesignComponentFamily from "./Containers/DesignComponentFamilyContainer";
import DesignAspect from "./Containers/DesignAspectsContainer";
import ServiceLevelPA from "./Containers/ServiceLevelPaContainer";
import PlannedActivityTracker from "./Containers/PlannedActivityTracker";
import SoftwareConfigurations from "./Containers/SoftwareConfigurations";
import HomeContainer from "./Containers/HomeContainer";
import BundleUpgradeInitiativeContainer from "./Containers/BundleUpgradeInitiativeContainer";
import GenerateLcmDb from "./Containers/GenerateLcmDbContainer";
import DesignComponent from "./Containers/DesignComponentContainer";
import GenerateVoLTEDashboard from "./Containers/GenerateVoLTEDashboardContainer";
import PlannedActivitiesContainer from "./Containers/PlannedActivitiesContainer";
import LcmEngineering from "./Containers/LcmEngineeringContainer";
import MajorHardware from "./Containers/MajorHardwareBuildContainer";
import MajorSoftware from "./Containers/MajorSoftwareBuildContainer";
import Footer from "./Components/Footer/Footer";
import Vnf from "./Containers/VnfContainer";
import Nfvi from "./Containers/NfviContainer";
import { authProvider } from "./authProvider";

import { useMsal, useIsAuthenticated } from "@azure/msal-react";
import { InteractionStatus } from "@azure/msal-browser";
import { useDispatch } from "react-redux";
import { loginRequest } from "./authProvider";

import LoginScreen from "./screen/Login/Login";
import RegularLogin from "./screen/RegularLogin/RegularLogin";
import { Form } from "react-bootstrap";
import SubNetworkBoundary from "./Containers/Lookup/SubNetworkBoundaryContainer";
import VodafoneName from "./Containers/Lookup/VodafoneNameContainer";
import LogManagement from "./Containers/LogManagementContainer";
import Glossary from "./Components/Glossary";
import DeliveryTracking from "./Containers/DeliveryTrackingTable";
import ResourceKeyMaster from "./Containers/ResourceKeyMasterDetails";
import DCFLifeCycle from "./Containers/DCFLifeCycle";
import Audit from "./Containers/AuditContainer";
import AssetMapInfo from "./Containers/AssetMapInfoContainer";
import GenericReportContainer from "./Containers/GenericReportContainer";
import ReportListContainer from "./Containers/ReportListContainer";
import UserManagementContainer from "./Containers/UserManagementContainer";
import GlossaryContainer from "./Containers/GlossaryContainer";
import PlannedActivityTypes from "./Containers/PlannedActivityTypes";
import Reconciliation from "./Containers/ReconciliationContainer";
import { createSelector } from "@reduxjs/toolkit";
import LandingPage from "./Containers/landingPage/landingPage";
import { OptionProvider } from "./Context/MenuOptionContext";
import { ModalProvider } from "./Context/ModalContext";
import { ThemeProvider } from "./Context/ThemeContext";
import LoginPage from "./Containers/login/loginPage";
import OrganizationInfo from "./Containers/OrganizationInfoContainer";
import Chart from "./Components/Charts/Chart";
import Chart2 from "./Components/Charts/Chart2";
import Chart3 from "./Components/Charts/Chart3";
import Chart4 from "./Components/Charts/Chart4";
import Chart5 from "./Components/Charts/Chart5";
import LCMAtGlanceChart from "./Components/Charts/LCMAtGlanceChart";
import LcmAtGlance from "./Components/Charts/LcmAtGlance";
import AssetPivotByLocation from "./Containers/AssetPivotByLocationContainer";
import GenerateLcmDbR10 from "./Containers/GenerateLcmDbR10Container";
import ComponentSWBuild from "./Containers/ComponentSWBuildContainer";
import BagItemComponent from "./Containers/BuildBagComponent";
import LandingPageHeader from "./Containers/landingPage/landingPageHeader";
import ProductAndDesign from "./Containers/landingPage/ProductAndDesign";
import Networkplan from "./Containers/landingPage/NetworkPlan";
import ManageNetworkPlan from "./Containers/landingPage/ManageNetworkPlan";
import Reports from "./Containers/landingPage/Reports";
import Administration from "./Containers/landingPage/Administration";
import ManageTransformation from "./Containers/landingPage/ManageTransformation";
import TSRReport from "./Containers/TSRReportContainer";
import FNTReport from "./Containers/FNTReportContainer";
import PassThroughReport from "./Containers/PassThroughReportContainer";
import OMC from "./Containers/OMCContainer";
import TSRRefresh from "./Containers/TSRRefreshContainer";
import NFVISoftwareCompatible from "./Containers/NFVISoftwareCompatibleContainer";
import GeneralSettingsContainer from "./Containers/GeneralSettingsContainer";
import GeneralSettings from "./Containers/GeneralSettings";
import VBOMInfoContainer from "./Containers/VBOMInfoContainer";
import NFVICContainer from "./Containers/NFVICContainer";
import VendorDetails from "./screen/Nfviccompatible/VendorDetails";
import MarketDetails from "./screen/Nfviccompatible/MarketDetails";
import VerticalDetails from "./screen/Nfviccompatible/VerticalDetails";
import VodafoneDetails from "./screen/Nfviccompatible/VodafoneDetails ";
import CBOMContainer from "./Containers/CBOMContainer";
import BPTPOC from "./Containers/BPTPOC";
import BPTReportContainer from "./Containers/BPTReportContainer";
import ClusterInfoContainer from "./Containers/ClusterInfoContainer";
import GanttChartContainer from "./Containers/ProjectPlanContainer";
import VBOMClusterInfoContainer from "./Containers/VBOMClusterInfoContainer";
import CBOMClusterInfoContainer from "./Containers/CBOMClusterInfoContainer";
import VBOMReportContainer from "./Containers/VBOMReportContainer";
import CBOMReportContainer from "./Containers/CBOMReportContainer";
import ExodusReportContainer from "./Containers/ExodusReportContainer";
import NewLandingPage from "./Containers/landingPage/NewLandingPage";
import ServiceLevelContainer from "./Containers/ServiceLevelContainer";
import ExodusAtGlance from "./Components/Charts/ExodusAtGlance";
import Dashboard from "./NewLandingScreens/layouts/MainLayout/Dashboard";
import NewLoginPage from "./Containers/login/NewLogin";
import { Skeleton } from "@mui/material";
import ProductComplianceGraph from "./Components/Charts/ProductComplianceGraph";

const App: React.FC<any> = () => {
  const nodeRef = useRef(null);
  //AUTH USER DATA
  const auth = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const { isPermesso, role } = useAuth();
  // const [isPermessoVal, setIsPermessoVal] = useState<any>(null);
  const navigate = useNavigate();
  const location: any = useLocation();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [loginType, setLoginType] = useState<string>("azure");
  const [accessGenerateToken, setAccessGenerateToken] =
    useState<boolean>(false);

  const { instance, accounts, inProgress } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const dispatch = useDispatch();
  const login = () => {
    return instance.loginRedirect(loginRequest);
  };

  useEffect(() => {
    if (isAuthenticated && accounts.length > 0) {
      const a = accounts[0];
      dispatch({
        type: "AAD_LOGIN_SUCCESS",
        payload: {
          account: { ...a, userName: a.username },
          idToken: { claims: a.idTokenClaims },
        },
      });
    } else {
      dispatch({ type: "AAD_LOGOUT_SUCCESS", payload: null });
    }
  }, [isAuthenticated, accounts, dispatch]);

  const redirectTriggered = useRef(false);
  useEffect(() => {
    const hasAppToken = getAccessToken() !== null && getAccessToken() !== "";
    const azureOnly = process.env.REACT_APP_ALLOW_LOCAL_LOGIN === "true";
    const justLoggedOut = sessionStorage.getItem("JUST_LOGGED_OUT") === "true";

    if (
      azureOnly &&
      !isAuthenticated &&
      !hasAppToken &&
      !justLoggedOut &&
      inProgress === InteractionStatus.None &&
      !redirectTriggered.current
    ) {
      console.log("true redirect");
      redirectTriggered.current = true;
      instance.loginRedirect(loginRequest);
    }
  }, [isAuthenticated, inProgress, instance]);

  const checkPermission = async () => {
    await verifyPermesso();
  };

  useEffect(() => {
    // console.log(isPermesso, role, auth, accessGenerateToken, getAccessToken());
    if (
      (role?.length === 0 || role === null) &&
      getAccessToken() !== null &&
      getAccessToken() !== ""
    ) {
      checkPermission();
    }
  }, [isPermesso]);

  useEffect(() => {
    if (auth?.account?.idTokenClaims) {
      if (accessGenerateToken) {
        return;
      } else {
        setAccessGenerateToken(true);
      }
    } else {
      setAccessGenerateToken(false);
    }
  }, [auth]);

  useEffect(() => {
    if (accessGenerateToken && auth) {
      onAuthenticated(auth, "azure");
    }
  }, [accessGenerateToken]);

  const openConfirm = (msg: string) => {
    setConfirm({
      title: "Confirm",
      message: msg,
      button: "Ok",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => {
          setConfirm(stateConfirm);
          navigate("/");
        },
        confirm: () => {
          setConfirm(stateConfirm);
          navigate("/");
        },
      },
    });
  };

  const onAuthenticated = async (userAccountInfo: any, type?: string) => {
    // console.log(userAccountInfo);
    if (type === "azure" && userAccountInfo) {
      if (
        userAccountInfo?.account?.idTokenClaims?.preferred_username ===
        userAccountInfo?.account?.userName
      ) {
        let encoded = base64_encode(userAccountInfo.account.userName);
        localStorage.setItem("UserName", encoded);
        await getAzureData({
          email: encoded,
          isAuthenicated: true,
        })
          .then((res) => {
            if (res.data.token !== "") {
              setAccessToken(
                res.data.token,
                +res.data.period,
                res.data.refreshToken.token
              );
            } else {
              setErrorMessage(res.data.errorMessage);
              setConfirm({
                title: "Confirm",
                message: res.data.errorMessage,
                button: "Ok",
                item: 0,
                isOpen: true,
                actions: {
                  cancel: () => {
                    setConfirm(stateConfirm);
                    authProvider.logout();
                  },
                  confirm: () => {
                    setConfirm(stateConfirm);
                    authProvider.logout();
                  },
                },
              });
            }

            setLoader("REMOVE", "");
          })
          .catch((error) => {
            setNotification({
              message: error.message,
              notifyType: NotifyType.warning,
            });
            //alert(error.message);
            removeAccessToken();
            setLoader("REMOVE", "");
          });
      }
      return;
    }

    if (type === "regular" && userAccountInfo) {
      let encoded = base64_encode(userAccountInfo.email);
      localStorage.setItem("UserName", encoded);

      await getAzureData({
        email: encoded,
        isAuthenicated: true,
      })
        .then((res) => {
          if (res.data.token !== "") {
            setAccessToken(
              res.data.token,
              +res.data.period,
              res.data.refreshToken.token
            );
          } else {
            setErrorMessage(res.data.errorMessage);
            openConfirm(res.data.errorMessage);
          }
          setLoader("REMOVE", "");
        })
        .catch((error) => {
          setNotification({
            message: error.message,
            notifyType: NotifyType.warning,
          });
          alert(error.message);
          setLoader("REMOVE", "");
        });

      return;
    }

    return null;
  };

  const [isOpenSidebar, setIsOpenSidebar] = useState(false);

  const toggleSidebar = () => {
    setIsOpenSidebar(!isOpenSidebar);
  };

  const productOwnerRole = role?.includes("SW Product Owner")
    ? "SW Product Owner"
    : role?.includes("HW Product Owner")
    ? "HW Product Owner"
    : null;

  let AppRoutes = (
    <>
      <Loader></Loader>
      <ErrorNotification OnModal={false} />
      {isPermesso || (getAccessToken() !== null && getAccessToken() !== "") ? (
        <Header />
      ) : null}
      <Routes>
        {/* <JoyrideWrapper steps={mainTourGuide} /> */}
        <Route path="/systemtype" element={<SystemType />} />
        <Route path="/systemverificationproblems" element={<TestInfo />} />
        <Route
          path="/nfvisoftwarecompatibility"
          element={<NFVISoftwareCompatible />}
        />
        <Route path="/bptpoc" element={<BPTPOC />} />
        <Route path="/tsrreport" element={<TSRReport />} />
        <Route path="/fntreport" element={<FNTReport />} />
        <Route path="/passthroughdata" element={<PassThroughReport />} />
        <Route path="/omc" element={<OMC />} />
        <Route path="/tsrrefresh" element={<TSRRefresh />} />
        <Route path="/majorhardware" element={<MajorHardware />} />
        <Route path="/majorsoftware" element={<MajorSoftware />} />
        <Route path="/componentswbuild" element={<ComponentSWBuild />} />
        <Route path="/componentswbag" element={<BagItemComponent />} />
        <Route path="/lcmengineering" element={<LcmEngineering />} />
        <Route path="/Identities" element={<Identities />} />
        <Route
          path="/plannedActivities/:filter?"
          element={
            <PlannedActivitiesContainer
              forLcm={true}
              forNetworkElement={true}
              forDesignAspect={true}
              forServicePlan={true}
              forReport={true}
            />
          }
        />
        <Route path="/asis" element={<NetworkElementAsIs />} />
        <Route path="/reconciliation" element={<Reconciliation />} />
        <Route path="/designcomponent" element={<DesignComponent />} />
        <Route
          path="/designcomponentfamily"
          element={<DesignComponentFamily />}
        />
        <Route path="/generatelcmdb" element={<GenerateLcmDb />} />
        <Route path="/generatelcmdbR10" element={<GenerateLcmDbR10 />} />
        <Route
          path="/generatevoltedashboard"
          element={<GenerateVoLTEDashboard />}
        />
        <Route path="/projectplan" element={<GanttChartContainer />} />
        <Route
          path="/bundleupgradeinitiative"
          element={<BundleUpgradeInitiativeContainer />}
        />
        <Route
          path="/settingsupdateplannedactivity"
          element={<SettingsUpdatePlannedActivity />}
        />
        <Route path="/plannedactivitytype" element={<PlannedActivityTypes />} />
        <Route path="/asplanned" element={<NetworkElementAsPlanned />} />
        <Route path="/dashboard" element={<DashboardContainer />} />
        <Route
          path="/targetmonthlyapprovals"
          element={<WorklogApprovalContainer />}
        />
        <Route path="/vaiexport" element={<ViaExportContainer />} />
        <Route path="/genericreporting" element={<GenericReportContainer />} />
        <Route path="/vnf" element={<Vnf />} />
        <Route path="/nfvi" element={<Nfvi />} />
        {/* <Route path="/home" element={<HomeContainer />}/> */}
        {isPermesso && role && (
          <>
            <Route
              path="/overview"
              element={
                isPermesso && productOwnerRole ? (
                  <Dashboard role={productOwnerRole} />
                ) : (
                  <Navigate to="/home" replace />
                )
              }
            />
            <Route path="/home" element={<LandingPage />} />
          </>
        )}
        <Route path="/productanddesign" element={<ProductAndDesign />} />
        <Route path="/networkplan" element={<Networkplan />} />
        <Route path="/managenetworkplan" element={<ManageNetworkPlan />} />
        <Route
          path="/managetransformation"
          element={<ManageTransformation />}
        />
        <Route path="/reports" element={<Reports />} />
        <Route path="/administration" element={<Administration />} />
        <Route path="/designAspect" element={<DesignAspect />} />
        <Route path="/servicelevel" element={<ServiceLevelContainer />} />
        <Route
          path="/plannedActivityTracker"
          element={<PlannedActivityTracker />}
        />
        <Route
          path="/subnetwork"
          element={<SubNetworkBoundary showButtons={true} />}
        />
        <Route
          path="/vodafoneName"
          element={<VodafoneName showButtons={true} />}
        />
        <Route path="/userLogLevel" element={<LogManagement />} />
        <Route
          path="/feedbackloop/networkelement"
          element={<NewNetworkElementAsIs />}
        />
        <Route path="/feedbackloop/identity" element={<Identity />} />
        <Route
          path="/feedbackloop/hardwareconfiguration"
          element={<HardwareConfiguration />}
        />
        <Route
          path="/feedbackloop/softwareconfiguration"
          element={<SoftwareConfiguration />}
        />
        <Route
          path="/feedbackloop/softwarecomponent"
          element={<SoftwareComponent />}
        />
        <Route
          path="/feedbackloop/softwareConfigurations"
          element={<SoftwareConfigurations />}
        />
        <Route path="/deliveryTracking" element={<DeliveryTracking />} />
        <Route path="/glossary/:name?" element={<Glossary name="" />} />
        <Route path="/resourcekeymaster" element={<ResourceKeyMaster />} />
        <Route path="/dcflifecycle" element={<DCFLifeCycle />} />
        <Route path="/glossaryModule" element={<GlossaryContainer />} />
        <Route path="/feedbackloop/audit" element={<Audit />} />
        <Route path="/assetmapinfo" element={<AssetMapInfo />} />
        <Route path="/genericreports" element={<ReportListContainer />} />
        <Route path="/usermanagement" element={<UserManagementContainer />} />
        <Route path="/organizationinfo" element={<OrganizationInfo />} />
        <Route path="/virtualbom" element={<VBOMClusterInfoContainer />} />
        <Route path="/virtualbomreport" element={<VBOMReportContainer />} />
        <Route path="/cbomreport" element={<CBOMReportContainer />} />
        <Route path="/cbom" element={<CBOMClusterInfoContainer />} />
        {/* {isPermesso &&
        getAccessToken() !== null &&
        getAccessToken() !== "" &&
        role !== null ? (
          <>
            {role !== null && role?.includes("SW Product Owner") ? (
              <Route path="/" element={<Navigate to="/overview" />} />
            ) : (
              <Route path="/" element={<Navigate to="/home" />} />
            )}
          </>
        ) : null} */}
        <Route path="/" element={<Navigate to="/home" />} />
        <Route path="/plannedactivityreport/:filter?" element={<Chart />} />
        <Route path="/assetoverviewbymarket/:filter?" element={<Chart2 />} />
        <Route
          path="/assetpivotbylocation"
          element={<AssetPivotByLocation />}
        />
        <Route path="/networkvisualizer" element={<Chart3 />} />
        <Route path="/charts4" element={<Chart4 />} />
        <Route path="/lcmatglance/:filter?" element={<LcmAtGlance />} />
        <Route path="/exodusatglance/:filter?" element={<ExodusAtGlance />} />
        <Route path="/generalsettings" element={<GeneralSettings />} />
        <Route
          path="/exodusassettimeline/:filter?"
          element={<ProductComplianceGraph />}
        />
        <Route
          path="/customisedpagesize"
          element={<GeneralSettingsContainer />}
        />
        <Route path="/nfvicreport" element={<NFVICContainer />} />
        <Route path="/marketdetails" element={<MarketDetails />} />
        <Route path="/vendordetails" element={<VendorDetails />} />
        <Route path="/verticaldetails" element={<VerticalDetails />} />
        <Route path="/vodafonedetails" element={<VodafoneDetails />} />
        <Route path="/bptreport" element={<BPTReportContainer />} />
        <Route path="/clusterinfo" element={<ClusterInfoContainer />} />
        <Route path="/exodusreport" element={<ExodusReportContainer />} />
      </Routes>
      {location?.pathname !== "/overview" && isPermesso ? <Footer /> : null}
    </>
  );

  const regularLogin = (loginData: {
    email: string;
    password: string;
  }): any => {
    localStorage.setItem("TYPE", "LOC");
    onAuthenticated(loginData, "regular");
  };

  const AzureADContent: React.FC = () => {
    return (
      <div ref={nodeRef}>
        <>
          {isPermesso ||
          (getAccessToken() !== null && getAccessToken() !== "") ? (
            AppRoutes
          ) : (
            <div className="w-100 text-center content-bg">
              {/* <img
                className="logoLogin mb-4"
                alt="Cam"
                src={require("./img/logoCAM_R.png")}
              /> */}
              {accessGenerateToken ? (
                <h3>
                  Please wait while you are being redirected to TEMS homepage
                </h3>
              ) : (
                <>
                  {/* <h3>Authentication required</h3>
                  <h5>Please login to access the application</h5> */}
                  {process.env.REACT_APP_ALLOW_LOCAL_LOGIN === "true" ? (
                    <LoginPage
                      localLogin={regularLogin}
                      azureLogin={login}
                      loginType="azure"
                    />
                  ) : (
                    <>
                      {/* <div className="radio-content flex-mode">
                        <Form.Check
                          type="radio"
                          className="radio"
                          name="userLogin"
                          label="Local User"
                          checked={loginType === "local" ? true : false}
                          value="local"
                          onChange={() => {
                            setLoginType("local");
                          }}
                        />
                        <Form.Check
                          type="radio"
                          className="radio"
                          name="userLogin"
                          label=" Azure User"
                          value="azure"
                          checked={loginType === "azure" ? true : false}
                          onChange={() => {
                            setLoginType("azure");
                            localStorage.setItem("TYPE", "AZURE");
                          }}
                        />
                      </div> */}
                      <LoginPage
                        localLogin={regularLogin}
                        azureLogin={login}
                        loginType=""
                      />

                      {/* {loginType === "local" ? (
                        <RegularLogin onLogin={regularLogin} />
                      ) : (
                        <LoginScreen
                          onLogin={() => {
                            login();
                          }}
        />
                      )} */}
                    </>
                  )}
                </>
              )}
            </div>
          )}
        </>
      </div>
    );
  };

  return (
    <ThemeProvider>
      <OptionProvider>
        <ModalProvider>
          <div id="App" className="App">
            <ModalConfirm data={confirm} showHyperLink={true} />

            <div id="authentication">
              <AzureADContent />
            </div>
          </div>
        </ModalProvider>
      </OptionProvider>
    </ThemeProvider>
  );
};

export default App;
