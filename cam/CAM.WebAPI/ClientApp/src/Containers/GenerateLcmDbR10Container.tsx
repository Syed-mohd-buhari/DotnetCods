import React, { useEffect, useState } from "react";
import { Tabs, Tab, Modal, Dropdown, Form } from "react-bootstrap";
import Select from "react-select";

import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import Hardware from "../screen/GenerateLcmDb/GenerateLcmDbHardware";
import {
  ReportHardwareQueryObjectGrid,
  ReportHardwareDtoGrid,
} from "../Model/Report/ReportHardwareModel";
import {
  GetReportHardwareGrid,
  GetSharedLookUpGrid,
} from "../Redux/Action/Report/ReportHardwareGridAction";
import {
  DownloadReport,
  HardwareConfigDownloadReport,
  NetworkLevel2DownloadReport,
  SubNetworkDownloadHWReport,
  SubNetworkDownloadSWReport,
  exportLcmAllReport,
  exportSubnetworkAllReport,
} from "../Redux/Action/Report/ReportDownloadAction";
import Software from "../screen/GenerateLcmDb/GenerateLcmDbSoftware";
import {
  ReportSoftwareDtoGrid,
  ReportSoftwareQueryObjectGrid,
} from "../Model/Report/ReportSoftwareModel";
import { GetReportSoftwareGrid } from "../Redux/Action/Report/ReportSoftwareGridAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useGenerateLcmDbHardware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbHardware";
import { useGenerateLcmDbSoftware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbSoftware";
import SetupColumns from "../screen/Shared/SetupColumns";

import { CustomGridRender, ReportViewModeR10 } from "../Model/Common";
import { ReportQueryAllDto } from "../Model/Report/Export";
import { useAuth } from "../Hook/useAuth";
import { convertEnumToArray } from "../Hook/Common";
import SharedLookUp, {
  paginationQueryTipologiche,
} from "./Lookup/SharedLookUpContainer";
import { dictionaryToArray } from "../Hook/Dictionary";
import { FaPlus } from "react-icons/fa";
import { GoPlus } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import ScrollableTabs from "../Components/ScrollableTabs";
import { useGenerateLcmDbSubBoundHardware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbSubBoundHardware";
import { useGenerateLcmDbSubBoundSoftware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbSubBoundSoftware";
import { useGenerateLcmDbNetworkLevel2 } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbNetworkLevel2";
import {
  ReportQueryDto,
  ReportSubBoundHwSwQueryGrid,
} from "../Model/Report/LcmExportReport";
import {
  GetDCFNames,
  GetReportHardwareConfigGrid,
  GetReportNetworkLevel2Grid,
  GetReportSubBoundHardwareGrid,
  GetReportSubBoundSoftwareGrid,
  GetSupportedService,
} from "../Redux/Action/Report/ReportLcmExportGridAction";
import {
  HardwareConfigReportDtoGrid,
  NetworkLevel2ReportDtoGrid,
  ReportDtoGrid,
  ReportHardwareConfigQueryObjectGrid,
  ReportNetworkLevel2QueryObjectGrid,
  SubBoundHWSwReportDtoGrid,
} from "../Model/Report/ReportLcmExportModel";
import LcmExportSubnetworkBoundaryHwSw from "../screen/GenerateLcmDb/GenerateLcmDbSubBoundSw";
import LcmExportNetworkLevel1 from "../screen/GenerateLcmDb/GenerateLcmDbNetworkLevel2";
import { useGenerateLcmDbHardwareConfig } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbHardwareConfig";
import LcmExportHardwareConfig from "../screen/GenerateLcmDb/GenerateLcmDbHardwareConfig";
import LcmExportSubnetworkBoundaryHw from "../screen/GenerateLcmDb/GenerateLcmDbSubBoundHw";
import LcmExportSubnetworkBoundarySw from "../screen/GenerateLcmDb/GenerateLcmDbSubBoundSw";
import { DropdownInputComponent } from "../Components/FormField";
import { Link, useLocation } from "react-router-dom";

export let paginationQueryHardware: ReportHardwareQueryObjectGrid = {
  lcmStatusEngHardware: [],
  lcmStatusOpsHardware: [],
  outputToLcmHardware: [],
  reportId: [],
  name: "",
  localMarket: [],
  verticalEngineeringTeam: [],
  verticalSubDomain: [],
  engineeringContactPoint: [],
  operationsContactPoint: [],
  assetCategory: [],
  assetClass: [],
  assetType: [],
  assetDescription: [],
  productImportance: [],
  vendor: [],
  hardwareModel: [],
  numberOfNodes: [],
  operationsMaintenanceContract: [],
  vendorEndOfMaintenanceDate: undefined,
  lcmStatus: [],
  plannedAction: [],
  descriptionOfPlannedAction: [],
  plannedSoftwareRelease: [],
  projectStatus: [],
  projectEndDate: undefined,
  trackingNumberProjectName: [],
  notes: [],
  bundleBudget: [],
  bundleId: [],
  assetServiceFunctionality: [],
  platform: [],
  opsMaintenanceConractEnd: undefined,
  engRiskEvaluation: [],
  engRiskEvaluationNotes: [],
  opsRiskEvaluation: [],
  opsRiskEvaluationNotes: [],
  overallRiskEvaluation: [],
  identifiedAction: [],
  budgetEstimated: [],
  assetOutofScopeForReportingPurposes: [],
  managedByGdc: [],
  extendedSupportOptionOfferedByVendor: undefined,
  eomControl: [],
  engUpdateTracker: [],
  opsUpdateTracker: [],
  sortBy: "",
  isSortAscending: false,
  ViewMode: 1,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  lcmExportDescription: undefined,
};

export let paginationQuerySoftware: ReportSoftwareQueryObjectGrid = {
  assetVirtualized: [],
  softwareRelease: [],
  vendorEndOfVulnerabilitySecuritySupportDate: undefined,
  cloudVersion: [],
  outputToLcmSoftware: [],
  lcmStatusOpsSoftware: [],
  lcmStatusEngSoftware: [],
  reportId: [],
  name: "",
  localMarket: [],
  verticalEngineeringTeam: [],
  verticalSubDomain: [],
  engineeringContactPoint: [],
  operationsContactPoint: [],
  assetCategory: [],
  assetClass: [],
  assetType: [],
  assetDescription: [],
  productImportance: [],
  vendor: [],
  hardwareModel: [],
  numberOfNodes: [],
  operationsMaintenanceContract: [],
  vendorEndOfMaintenanceDate: undefined,
  lcmStatus: [],
  plannedAction: [],
  descriptionOfPlannedAction: [],
  plannedSoftwareRelease: [],
  projectStatus: [],
  projectEndDate: undefined,
  trackingNumberProjectName: [],
  notes: [],
  bundleBudget: [],
  bundleId: [],
  assetServiceFunctionality: [],
  platform: [],
  opsMaintenanceConractEnd: undefined,
  engRiskEvaluation: [],
  engRiskEvaluationNotes: [],
  opsRiskEvaluation: [],
  opsRiskEvaluationNotes: [],
  overallRiskEvaluation: [],
  identifiedAction: [],
  budgetEstimated: [],
  assetOutofScopeForReportingPurposes: [],
  managedByGdc: [],
  extendedSupportOptionOfferedByVendor: undefined,
  eomControl: [],
  engUpdateTracker: [],
  opsUpdateTracker: [],
  componentName: [],
  componentResourceKey: [],
  sortBy: "",
  isSortAscending: false,
  ViewMode: 2,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  lcmExportDescription: undefined,
};

export let generalPaginationQuery: ReportQueryDto = {
  reportId: [],
  previousReportId: [],
  name: "",
  localMarket: [],
  localMarketId: [],
  designComponentIndex: [],
  verticalEngineeringTeam: [],
  verticalSubDomain: [],
  engineeringContactPoint: [],
  operationsContactPoint: [],
  assetCategory: [],
  assetClass: [],
  assetType: [],
  assetDescription: [],
  productImportance: [],
  vendor: [],
  hardwareModel: [],
  numberOfNodes: [],
  operationsMaintenanceContract: [],
  vendorEndOfMaintenanceDateValue: undefined,
  lcmStatus: [],
  plannedAction: [],
  descriptionOfPlannedAction: [],
  plannedSoftwareVersion: [],
  plannedHardwareModel: [],
  projectStatus: [],
  projectEndDateValue: undefined,
  vendorEndOfVulnerabilitySecuritySupportDateValueLcm: undefined,
  assetStatusFY26: [],
  assetStatusFY28: [],
  applicationOrOperationgSys: [],
  opsMaintenanceConractEndValueLcm: undefined,
  requestIDLcm: [],
  eoslKpiTarget: [],
  securityRiskOverallLcm: [],
  riskComment: [],
  programLcm: [],
  trackingNumberProjectNameLcm: [],
  eoslKpiFrozen: [],
  eoslKpiForecast: [],
  exceptionFlag: [],
  numberOfNodesLcm: [],
  operationsMaintenanceContractLcm: [],
  dataSourceLcm: [],
  trackingNumberProjectName: [],
  notes: [],
  bundleBudget: [],
  bundleId: [],
  assetServiceFunctionality: [],
  platform: [],
  opsMaintenanceConractEndValue: undefined,
  engRiskEvaluation: [],
  engRiskEvaluationNotes: [],
  opsRiskEvaluation: [],
  opsRiskEvaluationNotes: [],
  overallRiskEvaluation: [],
  identifiedAction: [],
  budgetEstimated: [],
  managedByGdc: [],
  extendedSupportOptionOfferedByVendor: undefined,
  engkpI2: [],
  expLCMstatusatendofFY24: [],
  riskCluster: [],
  criticality: [],
  gdprRelevant: [],
  deliveryPlanAvailable: [],
  hostname: [],
  ipAddress: [],
  ragStatus: [],
  wbsCode: [],
  bptID: [],
  ppmID: [],
  serialNumber: [],
  assetStatus: [],
  program: [],
  projectOwner: [],
  reasonfornoPlan: [],
  commentonProjectStatus: [],
  securityRiskPotential: [],
  securityRiskEffective: [],
  vulnerabilityRating: [],
  securityMitigation: [],
  securityRiskOverall: [],
  includedinSecurityScanning: [],
  raId: [],
  cyberRiskRequestId: [],
  requestID: [],
  lastScanDate: undefined,
  lastScanDateValue: undefined,
  assetOutofScopeForReportingPurposes: [],
  lastUpgradeDate: undefined,
  lastUpgradeDateValue: undefined,
  eomControl: [],
  engUpdateTracker: [],
  opsUpdateTracker: [],
  exNetworks: [],
  newopsRiskEvaluation: [],
  occurrenceProbability: [],
  incidentClass: [],
  productCode: [],
  handedOverToOperation: [],
  contractRenewalPlan: [],
  dataSource: [],
  scopeOfSimplification: [],
  viewMode: 1,
  lcmExportDescription: "",
  verticalEngineeringTeamId: [],
  isPecn: [],
  isPecs: [],
  isScf: [],
  isNof: [],
  exposedEdgeFlag: [],
  externalFacingFlag: [],
  infrastructureLocation: [],
  category: [],
  interfaceType: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
};

export let getDcfNamesPaginationQuery = {
  systemTypeIdentityName: [],
  designComponentFamilyName: [],
  description: [],
  subNetworkBoundary: [],
  subNetworkBoundaryId: [],
  networkFunction: [],
  supportedServices: [],
  vodafoneName: [],
  designComponentFamilyId: [],
  implementation: [],
  systemIsShared: [],
  criticalityRating: [],
  lastModifiedBy: [],
  sharingType: [],
  countrySpecificCriticality: [],
  productName: [],
  lastModifiedValue: undefined,
  systemTypeIdBasedVerticalId: [],
  designContact: [],
  systemTypeIdBasedVerticalValue: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
};

export let networkLevel2PaginationQuery = {
  assetVirtualized: [],
  softwareVersion: [],
  vendorEndOfVulnerabilitySecuritySupportDateValue: undefined,
  outputToLcmSoftware: [],
  lcmStatusOpsSoftware: [],
  lcmStatusEngSoftware: [],
  typeOfNetworkElement: [],
  originalLCMSpreadsheetID: [],
  isExtendedSupportOfferedByVendor: [],
  labSWRelease: [],
  originalSwLcmId: [],
  componentName: [],
  componentResourceKey: [],
};

export let subBoundHwSwPaginationQuery = {
  assetVirtualized: [],
  softwareVersion: [],
  vendorEndOfVulnerabilitySecuritySupportDateValue: undefined,
  outputToLcmSoftware: [],
  lcmStatusOpsSoftware: [],
  lcmStatusEngSoftware: [],
  typeOfNetworkElement: [],
  originalLCMSpreadsheetID: [],
  isExtendedSupportOfferedByVendor: [],
  labSWRelease: [],
  originalSwLcmId: [],
  lcmStatusEngHardware: [],
  lcmStatusOpsHardware: [],
  outputToLcmHardware: [],
  hwIsExtendedSupportOfferedByVendor: [],
  originalHwLcmId: [],
  designComponentFamily: [],
  supportedService: [],
};

export let HardwareConfigPaginationQuery = {
  lcmStatusEngHardware: [],
  lcmStatusOpsHardware: [],
  outputToLcmHardware: [],
  hwIsExtendedSupportOfferedByVendor: [],
  originalHwLcmId: [],
  hardwareProfile: [],
  componentName: [],
  componentResourceKey: [],
};

export const getColor = (text: string): string => {
  switch (text) {
    case "expired":
      return "red";
    case "on expiration":
      return "orange";
    case "on support":
      return "green";
    default:
      return "";
  }
};

const GenerateLcmDbR10: React.FC = (props) => {
  sessionStorage.setItem("sharedName", "LcmExportSetting");
  const location = useLocation();
  //PAGE
  const [keyTabs, setKeyTabs] = useState("Hardware");
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const { readonly, tipologicaPermesso, isPermesso } = useAuth();
  const [versionsList, setVersionsList] = useState();
  const [pageTitle, setPageTitle] = useState("");
  const [dataType, setDataType] = useState(
    location?.state?.isR10Redirect ? 4 : 1
  );
  const [dcfNameResources, setDcfNameResources] = useState<any>(null);
  const [supportServiceResources, setSupportServiceResources] =
    useState<any>(null);
  const { darkMode } = useTheme();

  //DTO HARDWARE
  const [dataHardware, setDataHardware] = useState<
    ReportHardwareDtoGrid[] | undefined
  >([]);
  const GridHardware = (state: RootState) =>
    state.reportHardwareGridReducer.ReportHardwareGridResult;
  let GridDtoHardware = useSelector(GridHardware);

  //DTO SOFTWARE
  const [dataSoftware, setDataSoftware] = useState<
    ReportSoftwareDtoGrid[] | undefined
  >([]);

  const GridSoftware = (state: RootState) =>
    state.reportSoftwareGridReducer.ReportSoftwareGridResult;
  let GridDtoSoftware = useSelector(GridSoftware);

  //DTO NetworkLevel2
  const [dataNetworkLevel2, setDataNetworkLevel2] = useState<
    NetworkLevel2ReportDtoGrid[] | undefined
  >([]);

  const GridNetworkLevel2 = (state: RootState) =>
    state.reportNetworkLevel2GridReducer.ReportNetworkLevel2GridResult;
  let GridDtoNetworkLevel2 = useSelector(GridNetworkLevel2);

  //DTO HardwareConfig
  const [dataHardwareConfig, setDataHardwareConfig] = useState<
    HardwareConfigReportDtoGrid[] | undefined
  >([]);

  const GridHardwareConfig = (state: RootState) =>
    state.reportHardwareConfigGridReducer.ReportHardwareConfigGridResult;
  let GridDtoHardwareConfig = useSelector(GridHardwareConfig);

  //DTO Subnetwork Boundary HARDWARE
  const [dataSubBoundHardware, setDataSubBoundHardware] = useState<
    SubBoundHWSwReportDtoGrid[] | undefined
  >([]);

  const GridSubBoundHardware = (state: RootState) =>
    state.reportSubBoundHardwareGridReducer.ReportSubBoundHardwareGridResult;
  let GridDtoSubBoundHardware = useSelector(GridSubBoundHardware);

  //DTO Subnetwork Boundary SOFTWARE
  const [dataSubBoundSoftware, setDataSubBoundSoftware] = useState<
    SubBoundHWSwReportDtoGrid[] | undefined
  >([]);

  const GridSubBoundSoftware = (state: RootState) =>
    state.reportSubBoundSoftwareGridReducer.ReportSubBoundSoftwareGridResult;
  let GridDtoSubBoundSoftware = useSelector(GridSubBoundSoftware);

  const { queryHardware, setQueryHardware, nextHardware, backHardware } =
    useGenerateLcmDbHardware(paginationQueryHardware, undefined);

  // Subnetwork Boundary Hardware
  const {
    querySubBoundHardware,
    setQuerySubBoundHardware,
    nextSubBoundHardware,
    backSubBoundHardware,
  } = useGenerateLcmDbSubBoundHardware(
    { ...generalPaginationQuery, ...subBoundHwSwPaginationQuery },
    isPermesso && (dataType === 1 || dataType === 5)
      ? GetReportSubBoundHardwareGrid
      : undefined
  );
  // Subnetwork Boundary Software
  const {
    querySubBoundSoftware,
    setQuerySubBoundSoftware,
    nextSubBoundSoftware,
    backSubBoundSoftware,
  } = useGenerateLcmDbSubBoundSoftware(
    { ...generalPaginationQuery, ...subBoundHwSwPaginationQuery },
    isPermesso && (dataType === 1 || dataType === 5)
      ? GetReportSubBoundSoftwareGrid
      : undefined
  );

  // Network Level 1
  const { querySoftware, setQuerySoftware, nextSoftware, backSoftware } =
    useGenerateLcmDbSoftware(
      paginationQuerySoftware,
      isPermesso && (dataType === 2 || dataType === 5)
        ? GetReportSoftwareGrid
        : undefined
    );
  // Network Level 2
  const {
    queryNetworkLevel2,
    setQueryNetworkLevel2,
    nextNetworkLevel2,
    backNetworkLevel2,
  } = useGenerateLcmDbNetworkLevel2(
    { ...generalPaginationQuery, ...networkLevel2PaginationQuery },
    isPermesso && (dataType === 3 || dataType === 5)
      ? GetReportNetworkLevel2Grid
      : undefined
  );
  // Hardware Config
  const {
    queryHardwareConfig,
    setQueryHardwareConfig,
    nextHardwareConfig,
    backHardwareConfig,
  } = useGenerateLcmDbHardwareConfig(
    { ...generalPaginationQuery, ...HardwareConfigPaginationQuery },
    isPermesso && (dataType === 4 || dataType === 5)
      ? GetReportHardwareConfigGrid
      : undefined
  );
  const [queryAll, setQueryAll] = useState<ReportQueryAllDto>({});
  const [excelPopup, setExcelPopUp] = useState<Boolean>(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [disaggregatedType, setDisaggregatedType] = useState(0);
  const [selectedDcf, setSelectedDcf] = useState<{
    key: number;
    value: string;
  } | null>(null);
  const [selectedSupSer, setSelectedSupSer] = useState<{
    key: number;
    value: string;
  } | null>(null);

  const [renderGridStateHw, setRenderGridStateHw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateSw, setRenderGridStateSw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateSubBoundHw, setRenderGridStateSubBoundHw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateSubBoundSw, setRenderGridStateSubBoundSw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateNetworkLevel2, setRenderGridStateNetworkLevel2] =
    useState<CustomGridRender | undefined>();

  const [renderGridStateHarwareConfig, setRenderGridStateHarwareConfig] =
    useState<CustomGridRender | undefined>();

  const onChangeType = (e) => {
    setDataType(e?.key);

    const mappings = {
      1: {
        keyTab: "Hardware",
        queries: () => {
          const subBoundHwSw = {
            ...generalPaginationQuery,
            ...subBoundHwSwPaginationQuery,
          } as ReportSubBoundHwSwQueryGrid;
          GetDcfApiCall();
          GetSupportedServiceApiCall();
          setQuerySubBoundHardware(subBoundHwSw);
          setQuerySubBoundSoftware(subBoundHwSw);
        },
      },
      2: {
        keyTab: "Network Element - Level 1",
        queries: () => {
          const SWCopy = {
            ...paginationQuerySoftware,
          } as ReportSoftwareQueryObjectGrid;

          setQuerySoftware(SWCopy);
        },
      },
      3: {
        keyTab: "Network Element - Level 2",
        queries: () => {
          const NetworkLevel2Copy = {
            ...generalPaginationQuery,
            ...networkLevel2PaginationQuery,
          } as ReportNetworkLevel2QueryObjectGrid;

          setQueryNetworkLevel2(NetworkLevel2Copy);
        },
      },
      4: {
        keyTab: "Hardware",
        queries: () => {
          const HardwareConfigCopy = {
            ...generalPaginationQuery,
            ...HardwareConfigPaginationQuery,
          } as ReportHardwareConfigQueryObjectGrid;

          setQueryHardwareConfig(HardwareConfigCopy);
        },
      },
      5: {
        keyTab: "Subnetwork Boundary - Hardware",
        queries: () => {
          const subBoundHwSw = {
            ...generalPaginationQuery,
            ...subBoundHwSwPaginationQuery,
          } as ReportSubBoundHwSwQueryGrid;
          const SWCopy = {
            ...paginationQuerySoftware,
          } as ReportSoftwareQueryObjectGrid;
          const NetworkLevel2Copy = {
            ...generalPaginationQuery,
            ...networkLevel2PaginationQuery,
          } as ReportNetworkLevel2QueryObjectGrid;
          const HardwareConfigCopy = {
            ...generalPaginationQuery,
            ...HardwareConfigPaginationQuery,
          } as ReportHardwareConfigQueryObjectGrid;

          setQuerySubBoundHardware(subBoundHwSw);
          setQuerySubBoundSoftware(subBoundHwSw);
          setQuerySoftware(SWCopy);
          setQueryNetworkLevel2(NetworkLevel2Copy);
          setQueryHardwareConfig(HardwareConfigCopy);
        },
      },
    };

    const selectedMapping = mappings[e?.key];
    if (selectedMapping) {
      setKeyTabs(selectedMapping.keyTab);
      selectedMapping.queries();
    }
    setSelectedSupSer(null);
    setSelectedDcf(null);
  };

  const onChangeDisaggregatedType = (e: any) => {
    const HWCopy = {
      ...paginationQueryHardware,
    } as ReportHardwareQueryObjectGrid;
    const SWCopy = {
      ...paginationQuerySoftware,
    } as ReportSoftwareQueryObjectGrid;

    HWCopy.ViewMode = 2;
    SWCopy.ViewMode = 2;
    setQueryHardware(HWCopy);
    setQuerySoftware(SWCopy);
  };

  const onChangeDescriptionList = (e: any) => {
    const HWCopy = {
      ...queryHardware,
    } as ReportHardwareQueryObjectGrid;

    const SWCopy = {
      ...querySoftware,
    } as ReportSoftwareQueryObjectGrid;
    if (e) {
      HWCopy.lcmExportDescription = e["value"];
      SWCopy.lcmExportDescription = e["value"];
      setPageTitle(e["value"]);
      setQueryHardware(HWCopy);
      setQuerySoftware(SWCopy);
    } else {
      HWCopy.lcmExportDescription = undefined;
      SWCopy.lcmExportDescription = undefined;
      setPageTitle("");
      setQueryHardware(HWCopy);
      setQuerySoftware(SWCopy);
    }
  };

  const InvocheDownload = async () => {
    const payload = {
      querySoftware: querySoftware,
      activeTab: "software",
      lcmExportDescription: "Network Element - Level 1",
    } as ReportQueryAllDto;

    let result = await DownloadReport(payload);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const ExportSubnetworkAllReport = async () => {
    setExcelPopUp(false);
    const payload = {
      querySubnetworkHardware: querySubBoundHardware,
      querySubnetworkSoftware: querySubBoundSoftware,
      activeTab: queryAll?.activeTab,
      lcmExportDescription: "",
    } as ReportQueryAllDto;

    let result = await exportSubnetworkAllReport(payload);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const ExportAllReport = async () => {
    const payload = {
      querySubnetworkHardware: querySubBoundHardware,
      querySubnetworkSoftware: querySubBoundSoftware,
      querySoftware: querySoftware,
      querySoftwareLevelTwo: queryNetworkLevel2,
      queryHardwareConfiguration: queryHardwareConfig,
      activeTab: "",
      lcmExportDescription: "",
    } as ReportQueryAllDto;
    let result = await exportLcmAllReport(payload);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const NetworkLevel2ReportDownload = async () => {
    setExcelPopUp(false);
    let result = await NetworkLevel2DownloadReport(queryNetworkLevel2);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const HardwareConfigReportDownload = async () => {
    setExcelPopUp(false);
    let result = await HardwareConfigDownloadReport(queryHardwareConfig);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const SubNetworkSWReportDownload = async () => {
    setExcelPopUp(false);
    let result = await SubNetworkDownloadSWReport(querySubBoundSoftware);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const SubNetworkHWReportDownload = async () => {
    setExcelPopUp(false);
    let result = await SubNetworkDownloadHWReport(querySubBoundHardware);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const closeModalSetup = (changed: boolean) => {
    GetReportHardwareGrid(queryHardware).then((x) =>
      GetReportSoftwareGrid(querySoftware).then((x) =>
        setIsVisibleModalSetup(false)
      )
    );
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const onHideModel = () => {
    if (isVisibleModalLookup === 1) {
      //  let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      //  SecurityManagerRefillData(dataCopy);
    }
  };

  const VersionsRefillData = (value) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    setPageTitle(dictionaryToArray(obj)[0].value);
    setVersionsList(obj);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <SharedLookUp
            returnObject={VersionsRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="LcmExportSetting"
          />
        );

      default:
        return null;
    }
  };
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   if (isPermesso) {
  //     GetSharedLookUpGrid(paginationQueryTipologiche)
  //       .then((res) => {
  //         VersionsRefillData(res?.items);
  //       })
  //       .catch((err) => {
  //         console.log(err);
  //       });
  //   }
  // }, [isPermesso]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (isPermesso && !location?.state?.isR10Redirect) {
      GetDcfApiCall();
      GetSupportedServiceApiCall();
    }
  }, [isPermesso]);

  useEffect(() => {
    if (selectedSupSer) {
      GetDcfApiCall();
    }
  }, [selectedSupSer]);
  const GetDcfApiCall = async () => {
    const dcfNames = await GetDCFNames({
      ...getDcfNamesPaginationQuery,
      supportedServices: selectedSupSer ? [selectedSupSer?.key] : [],
    }).then((res: any) =>
      res?.data?.map((val: any) => {
        return { key: val.id, value: val.description } as {
          key: number;
          value: string;
        };
      })
    );
    setDcfNameResources(dcfNames);
  };

  const GetSupportedServiceApiCall = async () => {
    const supportedService = await GetSupportedService().then((res: any) =>
      res?.data?.map((val: any) => {
        return { key: val.id, value: val.description } as {
          key: number;
          value: string;
        };
      })
    );
    setSupportServiceResources(supportedService);
  };

  useEffect(() => {
    let copy = { ...queryAll } as ReportQueryAllDto;
    if (querySoftware && isPermesso) {
      copy.querySoftware = querySoftware;
      copy.lcmExportDescription = "Network Element - Level 1";
      copy.activeTab = "software";
      setQueryAll(copy);
    }
  }, [querySoftware, keyTabs]);

  useEffect(() => {
    let copy = { ...queryAll } as ReportQueryAllDto;
    if (querySubBoundSoftware && querySubBoundHardware && isPermesso) {
      copy.querySubnetworkHardware = querySubBoundHardware;
      copy.querySubnetworkSoftware = querySubBoundSoftware;
      copy.activeTab = keyTabs;
      setQueryAll(copy);
    }
  }, [querySubBoundHardware, querySubBoundSoftware, keyTabs]);

  useEffect(() => {
    let copy = { ...queryAll } as ReportQueryAllDto;
    if (queryHardwareConfig && isPermesso) {
      copy.queryHardwareConfiguration = queryHardwareConfig;
      copy.activeTab = "hardware";
      setQueryAll(copy);
    }
  }, [queryHardwareConfig, keyTabs]);

  useEffect(() => {
    if (isPermesso) {
      const dcfList = selectedDcf ? [selectedDcf?.key] : [];
      const supportedServiceList = selectedSupSer ? [selectedSupSer?.key] : [];
      setQuerySubBoundHardware({
        ...querySubBoundHardware,
        designComponentFamily: dcfList,
        supportedService: supportedServiceList,
      } as ReportSubBoundHwSwQueryGrid);
      setQuerySubBoundSoftware({
        ...querySubBoundSoftware,
        designComponentFamily: dcfList,
        supportedService: supportedServiceList,
      } as ReportSubBoundHwSwQueryGrid);
      if (dataType === 5) {
        setQuerySoftware({
          ...querySoftware,
          designComponentFamily: dcfList,
          supportedService: supportedServiceList,
        } as ReportSoftwareQueryObjectGrid);
        setQueryHardwareConfig({
          ...queryHardwareConfig,
          designComponentFamily: dcfList,
          supportedService: supportedServiceList,
        } as ReportHardwareConfigQueryObjectGrid);
        setQueryNetworkLevel2({
          ...queryNetworkLevel2,
          designComponentFamily: dcfList,
          supportedService: supportedServiceList,
        } as ReportNetworkLevel2QueryObjectGrid);
      }
    }
  }, [selectedDcf, selectedSupSer]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDtoHardware != undefined && isPermesso) {
      setDataHardware(GridDtoHardware?.items);
      let copy = { ...GridDtoHardware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateHw(copy);
    }
  }, [GridDtoHardware]);

  useEffect(() => {
    if (GridDtoSoftware != undefined && isPermesso) {
      setDataSoftware(GridDtoSoftware?.items);
      let copy = { ...GridDtoSoftware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSw(copy);
    }
  }, [GridDtoSoftware]);

  //UPDATE ON CHANGE SubBound DTO

  useEffect(() => {
    if (GridDtoNetworkLevel2 != undefined && isPermesso) {
      setDataNetworkLevel2(GridDtoNetworkLevel2?.items);
      let copy = { ...GridDtoNetworkLevel2?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateNetworkLevel2(copy);
    }
  }, [GridDtoNetworkLevel2]);

  useEffect(() => {
    if (GridDtoHardwareConfig != undefined && isPermesso) {
      setDataHardwareConfig(GridDtoHardwareConfig?.items);
      let copy = { ...GridDtoHardwareConfig?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateHarwareConfig(copy);
    }
  }, [GridDtoHardwareConfig]);

  useEffect(() => {
    if (GridDtoSubBoundHardware != undefined && isPermesso) {
      setDataSubBoundHardware(GridDtoSubBoundHardware?.items);
      let copy = { ...GridDtoSubBoundHardware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSubBoundHw(copy);
    }
  }, [GridDtoSubBoundHardware]);

  useEffect(() => {
    if (GridDtoSubBoundSoftware != undefined && isPermesso) {
      setDataSubBoundSoftware(GridDtoSubBoundSoftware?.items);
      let copy = { ...GridDtoSubBoundSoftware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSubBoundSw(copy);
    }
  }, [GridDtoSubBoundSoftware]);

  const onChangeTab = (value: any) => {
    setKeyTabs(value.label);
  };

  const SubnetworkBoundaryHwGrid = () => {
    return (
      <>
        <LcmExportSubnetworkBoundaryHw
          data={dataSubBoundHardware}
          pagination={querySubBoundHardware}
          renderGrid={renderGridStateSubBoundHw?.render ?? []}
          action={{
            Filter: setQuerySubBoundHardware,
          }}
        ></LcmExportSubnetworkBoundaryHw>
        <Paginate
          pagination={{
            page: querySubBoundHardware.page,
            pageSize: querySubBoundHardware.pageSize,
          }}
          totalItems={GridDtoSubBoundHardware?.totalItems}
          actions={{ next: nextSubBoundHardware, back: backSubBoundHardware }}
        />
      </>
    );
  };

  const SubnetworkBoundarySwGrid = () => {
    return (
      <>
        <LcmExportSubnetworkBoundarySw
          data={dataSubBoundSoftware}
          pagination={querySubBoundSoftware}
          renderGrid={renderGridStateSubBoundSw?.render ?? []}
          action={{
            Filter: setQuerySubBoundSoftware,
          }}
        ></LcmExportSubnetworkBoundarySw>
        <Paginate
          pagination={{
            page: querySubBoundSoftware.page,
            pageSize: querySubBoundSoftware.pageSize,
          }}
          totalItems={GridDtoSubBoundSoftware?.totalItems}
          actions={{ next: nextSubBoundSoftware, back: backSubBoundSoftware }}
        />
      </>
    );
  };

  const NetworkLevel1Grid = () => {
    return (
      <>
        <Software
          data={dataSoftware}
          pagination={querySoftware}
          renderGrid={renderGridStateSw?.render ?? []}
          action={{
            Filter: setQuerySoftware,
          }}
        ></Software>
        <Paginate
          pagination={{
            page: querySoftware.page,
            pageSize: querySoftware.pageSize,
          }}
          totalItems={GridDtoSoftware?.totalItems}
          actions={{ next: nextSoftware, back: backSoftware }}
        />
      </>
    );
  };

  const NetworkLevel2Grid = () => {
    return (
      <>
        <LcmExportNetworkLevel1
          data={dataNetworkLevel2}
          pagination={queryNetworkLevel2}
          renderGrid={renderGridStateNetworkLevel2?.render ?? []}
          action={{
            Filter: setQueryNetworkLevel2,
          }}
        ></LcmExportNetworkLevel1>
        <Paginate
          pagination={{
            page: queryNetworkLevel2.page,
            pageSize: queryNetworkLevel2.pageSize,
          }}
          totalItems={GridDtoNetworkLevel2?.totalItems}
          actions={{ next: nextNetworkLevel2, back: backNetworkLevel2 }}
        />
      </>
    );
  };

  const HardwareConfigGrid = () => {
    return (
      <>
        <LcmExportHardwareConfig
          data={dataHardwareConfig}
          pagination={queryHardwareConfig}
          renderGrid={renderGridStateHarwareConfig?.render ?? []}
          action={{
            Filter: setQueryHardwareConfig,
          }}
        ></LcmExportHardwareConfig>
        <Paginate
          pagination={{
            page: queryHardwareConfig.page,
            pageSize: queryHardwareConfig.pageSize,
          }}
          totalItems={GridDtoHardwareConfig?.totalItems}
          actions={{ next: nextHardwareConfig, back: backHardwareConfig }}
        />
      </>
    );
  };

  const handleDownloadExcel = () => {
    const downloadActions = {
      2: [InvocheDownload],
      3: [NetworkLevel2ReportDownload],
      4: [HardwareConfigReportDownload],
      5: [
        SubNetworkHWReportDownload,
        SubNetworkSWReportDownload,
        InvocheDownload,
        NetworkLevel2ReportDownload,
        HardwareConfigReportDownload,
      ],
    };

    const actions = downloadActions[dataType];
    actions?.forEach((action) => action());
  };

  const getGridInformationSuffix = () => {
    if (
      (dataType === 1 && keyTabs === "Hardware") ||
      (dataType === 5 && keyTabs === "Subnetwork Boundary - Hardware")
    ) {
      return "(Subnetwork Boundary - Hardware)";
    }
    if (
      (dataType === 1 && keyTabs === "Software") ||
      (dataType === 5 && keyTabs === "Subnetwork Boundary - Software")
    ) {
      return "(Subnetwork Boundary - Software)";
    }
    if (
      (dataType === 2 || dataType === 5) &&
      keyTabs === "Network Element - Level 1"
    ) {
      return "(Network Element - Level 1)";
    }
    if (
      (dataType === 3 || dataType === 5) &&
      keyTabs === "Network Element - Level 2"
    ) {
      return "(Network Element - Level 2)";
    }
    if ((dataType === 4 || dataType === 5) && keyTabs === "Hardware") {
      return "(Hardware)";
    }
    return "";
  };

  const getRenderGridState = () => {
    if (
      (dataType === 1 && keyTabs === "Hardware") ||
      (dataType === 5 && keyTabs === "Subnetwork Boundary - Hardware")
    ) {
      return renderGridStateSubBoundHw;
    }
    if (
      (dataType === 1 && keyTabs === "Software") ||
      (dataType === 5 && keyTabs === "Subnetwork Boundary - Software")
    ) {
      return renderGridStateSubBoundSw;
    }
    if (
      (dataType === 2 || dataType === 5) &&
      keyTabs === "Network Element - Level 1"
    ) {
      return renderGridStateSw;
    }
    if (
      (dataType === 3 || dataType === 5) &&
      keyTabs === "Network Element - Level 2"
    ) {
      return renderGridStateNetworkLevel2;
    }
    if ((dataType === 4 || dataType === 5) && keyTabs === "Hardware") {
      return renderGridStateHarwareConfig;
    }
    return undefined;
  };

  return (
    <div className="pageContainer">
      <Modal
        show={isVisibleModalSetup}
        backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-1">{`Setup Grid Informations ${getGridInformationSuffix()}`}</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <SetupColumns
            renderGrid={getRenderGridState()}
            action={{ closeModalSetup: () => setIsVisibleModalSetup(false) }}
          ></SetupColumns>
        </Modal.Body>
      </Modal>
      <Modal
        show={excelPopup === true}
        backdrop="static"
        keyboard={false}
        size="lg"
        centered
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">Download To Excel</div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <Form style={{ padding: "0px 10px " }}>
            {["radio"].map((type) => (
              <div key={`inline-${type}`} className="mb-3">
                <Form.Check
                  onClick={() =>
                    setQueryAll({ ...queryAll, activeTab: "hardware" })
                  }
                  inline
                  checked={queryAll.activeTab === "hardware" ? true : false}
                  label="Hardware"
                  name="group1"
                  type={"radio"}
                  id={`inline-${type}-1`}
                />
                <Form.Check
                  onClick={() =>
                    setQueryAll({ ...queryAll, activeTab: "software" })
                  }
                  inline
                  checked={queryAll.activeTab === "software" ? true : false}
                  label="Software"
                  name="group1"
                  type={"radio"}
                  id={`inline-${type}-2`}
                />
                <Form.Check
                  onClick={() =>
                    setQueryAll({ ...queryAll, activeTab: "both" })
                  }
                  inline
                  label="Both"
                  name="group1"
                  type={"radio"}
                  id={`inline-${type}-2`}
                />
              </div>
            ))}
          </Form>
        </Modal.Body>
        <Modal.Footer className="headerPage row mx-0">
          <button
            className="download-to-excel"
            onClick={() => setExcelPopUp(false)}
          >
            Cancel
          </button>
          <button
            className="btn btn-danger mrl-10"
            onClick={() => ExportSubnetworkAllReport()}
          >
            Download
          </button>
        </Modal.Footer>
      </Modal>

      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          onHideModel();
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                onHideModel();
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {ReturnLookupContainer(isVisibleModalLookup)}
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">{"Disaggregated Reports"}</h3>
        </div>
        <div className="d-flex">
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() =>
              dataType === 1 ? setExcelPopUp(true) : handleDownloadExcel()
            }
          >
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              {!readonly && (
                <>
                  <Dropdown.Item
                    onClick={() => setVisibleLegenda(!isVisibleLegenda)}
                  >
                    Legend
                  </Dropdown.Item>
                  <div
                    className="bubbleMenuLegenda"
                    onMouseLeave={() => setVisibleLegenda(false)}
                  >
                    <div className="triangleBubbleTop-right"></div>
                    <div className="col-12 row mx-0 px-2 my-2">
                      <div className="w-100 mx-0 py-1 d-flex align-items-center">
                        <div className="legendaElement red"></div>
                        <span className="legendaElement">
                          Mandatory Field (Engineering)
                        </span>
                      </div>
                      <div className="w-100 mx-0 py-1 d-flex align-items-center">
                        <div className="legendaElement green"></div>
                        <span className="legendaElement">
                          Mandatory Field (Operations)
                        </span>
                      </div>
                      <div className="w-100 mx-0 py-1 d-flex align-items-center">
                        <div className="legendaElement gray"></div>
                        <span className="legendaElement">
                          Nice to have field
                        </span>
                      </div>
                      <div className="w-100 mx-0 py-2 d-flex align-items-center">
                        <div className="legendaElement blu"></div>
                        <span className="legendaElement">
                          Automatic Calculation
                        </span>
                      </div>
                    </div>
                  </div>
                </>
              )}

              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <div className="row mb-3">
        <div className="col-3">
          <DropdownInputComponent
            label={"Select Report Type"}
            //placeholderText="Select Supported Service..."
            labelCSS="mb-0 text-left"
            inputCSS="labelForm voda-bold mb-2"
            isSearchable={false}
            isClearable={false}
            value={convertEnumToArray(ReportViewModeR10).filter(
              (x) => x.key === dataType
            )}
            options={convertEnumToArray(ReportViewModeR10)}
            onChange={(e: any) => onChangeType(e)}
          />
        </div>

        {(dataType === 1 || dataType === 5) && (
          <>
            <div className="col-3">
              <DropdownInputComponent
                label={"Supported Service"}
                //placeholderText="Select Supported Service..."
                labelCSS="mb-0 text-left"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                value={selectedSupSer}
                options={supportServiceResources}
                onChange={(e: any) => setSelectedSupSer(e)}
              />
            </div>
            <div className="col-3">
              <DropdownInputComponent
                label={"Design Component Family"}
                //placeholderText="Select DCF(Design Component Family)..."
                labelCSS="mb-0 text-left"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                value={selectedDcf}
                options={dcfNameResources}
                onChange={(e: any) => setSelectedDcf(e)}
              />
            </div>
          </>
        )}
      </div>

      <ScrollableTabs
        key={dataType} // Force component remount when dataType changes
        tabs={
          [
            {
              condition: dataType === 1 || dataType === 5,
              label: `${
                dataType === 5 ? "Subnetwork Boundary - Hardware" : "Hardware"
              }`,
              component: <SubnetworkBoundaryHwGrid />,
            },
            {
              condition: dataType === 1 || dataType === 5,
              label: `${
                dataType === 5 ? "Subnetwork Boundary - Software" : "Software"
              }`,
              component: <SubnetworkBoundarySwGrid />,
            },
            {
              condition: dataType === 2 || dataType === 5,
              label: "Network Element - Level 1",
              component: <NetworkLevel1Grid />,
            },
            {
              condition: dataType === 3 || dataType === 5,
              label: "Network Element - Level 2",
              component: <NetworkLevel2Grid />,
            },
            {
              condition: dataType === 4 || dataType === 5,
              label: "Hardware",
              component: <HardwareConfigGrid />,
            },
          ]
            .filter((tab) => tab.condition) // Only include tabs where condition is true
            .map((tab) => ({
              label: tab.label,
              component: tab.component,
            })) // Map to the desired format
        }
        dataType={dataType}
        onTabChange={onChangeTab}
      />
    </div>
  );
};

export default GenerateLcmDbR10;
