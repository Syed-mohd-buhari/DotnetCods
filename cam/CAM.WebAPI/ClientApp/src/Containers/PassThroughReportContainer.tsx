import React, { useCallback, useEffect, useState } from "react";
import { Tabs, Tab, Modal, Dropdown, Form, Alert } from "react-bootstrap";
import Select from "react-select";

import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useDispatch, useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import PassThroughReportGrid from "../screen/PassThroughReport/PassThroughReportGrid";
import PassThroughHardwareReportGrid from "../screen/PassThroughReport/PassThroughHardwareReportGrid";
import PassThroughSoftwareReportGrid from "../screen/PassThroughReport/PassThroughSoftwareReportGrid";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";

import {
  PassThroughReportQueryObjectGrid,
  PassThroughReportDtoGrid,
} from "../Model/Report/PassThroughReportExport";
import {
  PassThroughHardwareReportQueryObjectGrid,
  PassThroughHardwareReportDtoGrid,
} from "../Model/Report/PassThroughHardwareReportExport";
import {
  GetPassThroughReportGrid,
  GetTSRReportVerticalGrid,
} from "../Redux/Action/Report/PassThroughReportGridAction";
import { GetPassThroughHardwareReportGrid } from "../Redux/Action/Report/PassThroughHardwareGridAction";
import { GetPassThroughSoftwareReportGrid } from "../Redux/Action/Report/PassThroughSoftwareGridAction";
import { DownloadPassThroughReport } from "../Redux/Action/Report/PassThroughReportDownloadAction";
import { DownloadPassThroughHardwareReport } from "../Redux/Action/Report/PassThroughHardwareReportDownloadAction";
import { DownloadPassThroughSoftwareReport } from "../Redux/Action/Report/PassThroughSoftwareReportDownloadAction";

import Software from "../screen/GenerateLcmDb/GenerateLcmDbSoftware";
import {
  ReportSoftwareDtoGrid,
  ReportSoftwareQueryObjectGrid,
} from "../Model/Report/ReportSoftwareModel";
import { GetReportSoftwareGrid } from "../Redux/Action/Report/ReportSoftwareGridAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";

import SetupColumns from "../screen/Shared/SetupColumns";
import { CustomGridRender, ReportViewMode } from "../Model/Common";
import { PassThroughReportQueryAllDto } from "../Model/Report/PassThroughReportExport";
import { useAuth } from "../Hook/useAuth";
import { handleImportFile } from "../Hook/Common";
import SharedLookUp, {
  paginationQueryTipologiche,
} from "./Lookup/SharedLookUpContainer";
import { dictionaryToArray } from "../Hook/Dictionary";
import { FaPlus } from "react-icons/fa";
import { GoArrowLeft, GoPlus } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions, Link } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import { DropdownInputComponent } from "../Components/FormField";
import TSRReportVerticalContainer from "./TSRReportVerticalContainer";
import { rtnColorDuplicate } from "../Hook/Duplicates";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";

export let paginationQueryPassthrough: PassThroughReportQueryObjectGrid = {
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 50,

  lastModified: undefined,
  lastModifiedValue: undefined,

  principalId: undefined,
  deleted: undefined,
  orphan: undefined,

  lastModifiedBy: [],
  passThroughIds: [],
  nonTemsVertical: [],
  vodafoneUniqueIdentifier: [],
  assetName: [],
  assetDescriptionOrPurpose: [],
  assetTypeTsr: [],
  businessOwner: [],
  supportOwner: [],
  supportTeam: [],
  supportTeamsPlaceInTheOrganisation: [],
  assetFunction: [],
  deploymentOrLifeCycleStatus: [],
  relatedriskidsFromRiskRegisters: [],
  regulatoryScope: [],
  countryWhereAssetisLocated: [],
  geoLocation: [],
  infrastructure: [],
  upstreamDependencies: [],
  downStreamDependencies: [],
  changesToTheAssetSinceDeployment: [],
  cloudhostedAsset: [],
  cloudType: [],
  cloudVendor: [],
  equipmentName: [],
  hostLocationWithinPhysicalLocation: [],
  softwareVendorName: [],
  firmwareVersionPatchLevel: [],
  maintenanceSupportSupplier: [],
  dependantHardware: [],
  instanceType: [],
  operatingSystemName: [],
  operatingSystemswvVersion: [],
  operatingSystemswVersionPatchLevel: [],
  systemNamedns: [],
  systemNameManagementIpaddress: [],
  systemNamenetbios: [],
  systemNameHostName: [],
  hardwareVendorName: [],
  maintenanceSupportSupplierSecond: [],
  dependantSystemSoftware: [],
  resilienceModel: [],
  geographicSiteResilience: [],
  localSiteResilience: [],
  nameOfProductsDependantonAsset: [],
  technicalServiceNames: [],
  customer: [],
  privilegedAccessLogging: [],
  boardorModuleNamecomponentName: [],
  exposedEdge: [],
  externallyFacingSystem: [],
  managementPlane: [],
  networkOverSightFunction: [],
  pecn: [],
  pecs: [],
  securityCriticalFunction: [],
  productImportance: [],
  critical: [],
  criticalityType: [],
  partNumber: [],
  descriptionofPlannedAction: [],
  identifiedAction: [],
  prodorLab: [],
  localmarketOwnership: [],
  budgetestimated: [],
  bundleBudget: [],
  assuranceCall: [],
  commentonProjectStatus: [],
  projectStatus: [],
  serviceLevel: [],
  lastPenTestRefNo: [],
  piData: [],
  encryptedPiData: [],
  recordclassifier: undefined,
  vendorHardwareEndofSupportDate: [],
  maintenanceHardwareEndofSupportDate: [],
  dateAssetMovedtoliveStatus: [],
  dateAssetDecommissioned: [],
  vendorSoftwareEndofSupportDate: [],
  maintenanceSoftwareEndofSupportDate: [],
  lastUpgradeDate: [],
  lastPenTestDate: [],
  projectEndDate: [],
  model: [],
  firmwareVersion: [],
  boardorModuleTypeComponentSubType: [],
  boardorModuleTypeComponentVersionNumber: [],
  serialNumber: [],
  hardwareTypeofHardwareAsset: [],
  hwEndofSale: [],
  softwareProductType: [],
  softwareProductVersion: [],
  applicationHostedonSoftware: [],
  uuidorSerialNumberofSoftware: [],
  swEndofSale: [],
  verticalEngineeringTeam: [],
  verticalSubDomain: [],
  platform: [],
  riskCluster: [],
  operationsContactPoint: [],
  assetClass: [],
  operationsMaintenanceContract: [],
  vendorEndofMaintenanceDate: [],
  opsMaintenanceContractEndDate: [],
  incidentClass: [],
  occurrenceProbability: [],
  organizationorPersonGroup: [],
  typeofNetworkElement: [],
  application: [],
  physicalServerHostName: [],
  physicalServerIpaddress: [],
  physicalServerSerialNumber: [],
  physicalServerHwModel: [],
  physicalServerVendor: [],
  virtualServerHostedon: [],
  virtualServerManufacturer: [],
  virtualServerTypeofDevice: [],
  virtualMachineType: [],
  virtualServerSerialNumber: [],
  virtualServerType: [],
  osStartDate: [],
  osInstallationDate: [],
  osStatus: [],
  softwareName: [],
  version: [],
  release: [],
  language: [],
  lcmExportDescription: undefined,
};
export let paginationQueryPassthroughHardware: PassThroughHardwareReportQueryObjectGrid =
  {
    sortBy: "",
    isSortAscending: true,
    page: 1,
    pageSize: 50,

    lastModified: undefined,
    lastModifiedValue: undefined,

    principalId: undefined,
    deleted: undefined,
    orphan: undefined,

    lastModifiedBy: [],
    passThroughLcmId: [],
    nonTemsVertical: [],
    reportId: [],
    localMarket: [],
    verticalEngineeringTeam: [],
    verticalSubDomain: [],
    engineeringContactPoint: [],
    assetCategory: [],
    assetClass: [],
    assetType: [],
    assetDescription: [],
    assetVirtualized: [],
    productImportance: [],
    hardwareModel: [],
    productCode: [],
    numberOfNodes: [],
    handedOverToOperation: [],
    contractRenewalPlan: [],
    lcmStatus: [],
    identifiedAction: [],
    descriptionOfPlannedAction: [],
    plannedSoftwareVersion: [],
    projectStatus: [],
    reasonfornoPlan: [],
    commentonProjectStatus: [],
    projectEndDate: [],
    ragStatus: [],
    trackingNumberProjectName: [],
    program: [],
    wbsCode: [],
    bptID: [],
    ppmID: [],
    scopeOfSimplification: [],
    dataSource: [],
    projectOwner: [],
    budgetEstimated: [],
    notes: [],
    bundleBudget: [],
    bundleId: [],
    assetServiceFunctionality: [],
    platform: [],
    engRiskEvaluation: [],
    engRiskEvaluationNotes: [],
    opsRiskEvaluation: [],
    opsRiskEvaluationNotes: [],
    incidentClass: [],
    occurrenceProbability: [],
    newopsRiskEvaluation: [],
    overallRiskEvaluation: [],
    riskCluster: [],
    securityRiskPotential: [],
    vulnerabilityScore: [],
    comments: [],
    qId: [],
    requestID: [],
    vulnerabilityRating: [],
    securityRiskEffective: [],
    securityMitigation: [],
    securityRiskOverall: [],
    includedinSecurityScanning: [],
    raId: [],
    lcmCumulativeRiskId: [],
    lcmCumulativeRiskLevel: [],
    cyberRiskRequestId: [],
    criticality: [],
    assetStatus: [],
    gdprRelevant: [],
    lastScanDate: [],
    lastUpgradeDate: [],
    assetOutofScopeForReportingPurposes: [],
    mainOrganization: [],
    isExtendedSupportOfferedByVendor: [],
    eomControl: [],
    engUpdateTracker: [],
    opsUpdateTracker: [],
    typeOfNetworkElement: [],
    engKpi2: [],
    ipAddress: [],
    serialNumber: [],
    hostname: [],
    exNetworks: [],
    originalLcmId: [],
    hwOperationsContactPoint: [],
    hwVendor: [],
    hwOperationsMaintenanceContract: [],
    hwVendorEndOfMaintenanceDate: [],
    plannedHWModel: [],
    hwopsMaintenanceContractEndDate: [],
    lcmStatusEngHardware: [],
    lcmStatusOpsHardware: [],
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

const PassThroughReport: React.FC = () => {
  const [keyTabs, setKeyTabs] = useState("lcm-software");
  const [importTab, setImportTab] = useState<string>("lcm-software");
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const { readonly, isPermesso, tipologicaPermesso } = useAuth();
  const [iserorShowModal, setIserorShowModal] = useState<boolean>(false);

  const [dataPassthrough, setDataPassthrough] = useState<
    PassThroughReportDtoGrid[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.passThroughReportGridReducer.PassThroughReportGridResult;
  const GridDto = useSelector(Grid);
  const GridHardware = (state: RootState) =>
    state.passThroughHardwareReportGridReducer
      .PassThroughHardwareReportGridResult;
  const GridDtoHardware = useSelector(GridHardware);
  const GridSoftware = (state: RootState) =>
    state.passThroughSoftwareReportGridReducer
      .PassThroughSoftwareReportGridResult;
  const GridDtoSoftware = useSelector(GridSoftware);
  const [downloadTab, setDownloadTab] = useState<string>("asset");
  const [downloadVertical, setDownloadVertical] = useState<any>(null);
  const [versionObj, setVersionObj] = useState<any>();
  const [pageTitle, setPageTitle] = useState("");
  const [selectedVersion, setSelectedVersion] = useState<{
    key: number;
    value: string;
    appSettingConfigurationId?: number;
  } | null>(null);
  const [versionResources, setVersionResources] = useState<
    { key: number; value: string; appSettingConfigurationId?: number }[]
  >([]);
  const [importErrorDetails, setImportErrorDetails] = useState<string[]>([]);
  const [importExcelPopup, setImportExcelPopup] = useState<Boolean>(false);
  const [verticalSelectedImport, setVerticalSelectedImport] =
    useState<any>(null);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    {
      ...paginationQueryPassthrough,
    },
    undefined
  );

  const {
    query: queryHardware,
    setQuery: setQueryHardware,
    next: nextHardware,
    back: backHardware,
    updatePageSize: updatePageSizeHardware,
  } = useResourceTableCrud(
    { ...paginationQueryPassthroughHardware },
    undefined
  );

  const {
    query: querySoftware,
    setQuery: setQuerySoftware,
    next: nextSoftware,
    back: backSoftware,
    updatePageSize: updatePageSizeSoftware,
  } = useResourceTableCrud(
    { ...paginationQueryPassthroughHardware },
    undefined
  );

  useEffect(() => {
    if (isPermesso && selectedVersion) {
      GetPassThroughReportGrid({
        ...query,
        nonTemsVertical: [selectedVersion.key],
      });
    }
  }, [selectedVersion, query, setQuery, next, back]);

  useEffect(() => {
    if (isPermesso && selectedVersion) {
      GetPassThroughHardwareReportGrid({
        ...queryHardware,
        nonTemsVertical: [selectedVersion.key],
      });
    }
  }, [
    selectedVersion,
    queryHardware,
    setQueryHardware,
    nextHardware,
    backHardware,
  ]);

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
    if (isPermesso && selectedVersion) {
      GetPassThroughSoftwareReportGrid({
        ...querySoftware,
        nonTemsVertical: [selectedVersion.key],
      });
    }
  }, [
    selectedVersion,
    querySoftware,
    setQuerySoftware,
    nextSoftware,
    backSoftware,
  ]);

  useEffect(() => {
    if (GridDtoSoftware != undefined && isPermesso) {
      setDataSoftware(GridDtoSoftware?.items);
      let copy = { ...GridDtoSoftware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSw(copy);
    }
  }, [GridDtoSoftware]);

  const [queryAll, setQueryAll] = useState<PassThroughReportQueryAllDto>({});
  const [excelPopup, setExcelPopUp] = useState<Boolean>(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [dataHardware, setDataHardware] = useState<any[] | undefined>([]);
  const [renderGridStateHw, setRenderGridStateHw] = useState<
    CustomGridRender | undefined
  >();
  const [dataSoftware, setDataSoftware] = useState<any[] | undefined>([]);
  const [renderGridStateSw, setRenderGridStateSw] = useState<
    CustomGridRender | undefined
  >();

  const onChangeDescriptionList = (e: any) => {
    const PassthroughCopy = {
      ...query,
    } as PassThroughReportQueryObjectGrid;

    const HardwareCopy = {
      ...queryHardware,
    } as PassThroughHardwareReportQueryObjectGrid;

    const SoftwareCopy = {
      ...querySoftware,
    } as PassThroughHardwareReportQueryObjectGrid;

    if (e) {
      PassthroughCopy.nonTemsVertical = [e["key"]];
      HardwareCopy.nonTemsVertical = [e["key"]];
      SoftwareCopy.nonTemsVertical = [e["key"]];
      setPageTitle(e["value"]);
      setQuery(PassthroughCopy);
      setQueryHardware(HardwareCopy);
      setQuerySoftware(SoftwareCopy);
    } else {
      PassthroughCopy.nonTemsVertical = [];
      HardwareCopy.nonTemsVertical = [];
      SoftwareCopy.nonTemsVertical = [];
      setPageTitle("");
      setQuery(PassthroughCopy);
      setQueryHardware(HardwareCopy);
      setQuerySoftware(SoftwareCopy);
    }
  };

  const InvocheDownload = async () => {
    if (!downloadVertical) {
      setAlertStatus({
        message: "Please select a domain before downloading.",
        class: "danger",
      });
      setShow(true);
      return;
    }

    setExcelPopUp(false);

    const downloadVerticalArray = [downloadVertical.key];

    let result;

    switch (downloadTab) {
      case "lcm-hardware":
        result = await DownloadPassThroughHardwareReport({
          ...queryHardware,
          nonTemsVertical: downloadVerticalArray,
        });
        break;
      case "lcm-software":
        result = await DownloadPassThroughSoftwareReport({
          ...querySoftware,
          nonTemsVertical: downloadVerticalArray,
        });
        break;
      case "asset":
      default:
        result = await DownloadPassThroughReport({
          ...query,
          nonTemsVertical: downloadVerticalArray,
        });
        break;
    }

    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }

    setDownloadVertical(null);
    setDownloadTab(keyTabs);
  };

  const [dataType, setDataType] = useState(1);

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const onHideModel = () => {
    if (isVisibleModalLookup === 1) {
    }
  };

  const VersionsRefillData = (value: any[]) => {
    const versionRes = value?.map((val: any) => {
      return {
        key: val.key,
        value: val.text,
        appSettingConfigurationId: val.key,
      } as {
        key: number;
        value: string;
        appSettingConfigurationId?: number;
      };
    });

    if (versionRes && versionRes.length > 0) {
      setSelectedVersion(versionRes[0]);
      setVersionResources(versionRes);
      setVersionObj(value);
      setPageTitle(versionRes[0].value);
      setQuery({
        ...(query as PassThroughReportQueryObjectGrid),
        nonTemsVertical: [versionRes[0].key],
      } as PassThroughReportQueryObjectGrid);
      setQueryHardware({
        ...(queryHardware as PassThroughHardwareReportQueryObjectGrid),
        nonTemsVertical: [versionRes[0].key],
      } as PassThroughHardwareReportQueryObjectGrid);
      setQuerySoftware({
        ...(querySoftware as PassThroughHardwareReportQueryObjectGrid),
        nonTemsVertical: [versionRes[0].key],
      } as PassThroughHardwareReportQueryObjectGrid);
    }
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
      case 2:
        return (
          <TSRReportVerticalContainer
            returnObject={callSharedLookUpApi}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return null;
    }
  };

  useEffect(() => {
    if (isPermesso) {
      callSharedLookUpApi();
    }
  }, [isPermesso]);

  const callSharedLookUpApi = async () => {
    const payload = {
      appSettingsId: [2],
      sortBy: "",
      isSortAscending: false,
      page: 1,
      pageSize: 10,
      lastModifiedBy: [],
    };
    const rtn: any = await GetTSRReportVerticalGrid(payload);

    if (rtn !== undefined && rtn !== null) {
      const dataArray = rtn.LookUpGridResult;

      if (dataArray && dataArray.length > 0) {
        VersionsRefillData(dataArray);
      }
    }
  };
  useEffect(() => {
    let copy = { ...queryAll } as PassThroughReportQueryAllDto;
    if (query && isPermesso) {
      copy.query = query;
      copy.activeTab = keyTabs;
      setQueryAll(copy);
    }
  }, [query, keyTabs, pageTitle]);

  useEffect(() => {
    if (GridDto != undefined && isPermesso) {
      setDataPassthrough(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  const refresh = () => {
    switch (importTab) {
      case "lcm-hardware":
        GetPassThroughHardwareReportGrid({
          ...queryHardware,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
        break;
      case "lcm-software":
        GetPassThroughSoftwareReportGrid({
          ...querySoftware,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
        break;
      case "asset":
      default:
        GetPassThroughReportGrid({
          ...query,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
        break;
    }
  };

  const fileHandle = async () => {
    try {
      if (!verticalSelectedImport) {
        setAlertStatus({
          message: "Please select a domain before importing.",
          class: "danger",
        });
        setShow(true);
        return;
      }

      let sheetName: string;
      let apiPath: string;
      const verticalId = [verticalSelectedImport?.key || 0];

      switch (importTab) {
        case "lcm-hardware":
          sheetName = "Passthrough Hardware Data";
          apiPath = "PassThroughHardwareReport";
          break;
        case "lcm-software":
          sheetName = "Passthrough Software Data";
          apiPath = "PassThroughSoftwareReport";
          break;
        case "asset":
        default:
          sheetName = "Passthrough Data";
          apiPath = "PassThroughReport";
          break;
      }

      const status = await handleImportFile(
        sheetName,
        apiPath,
        undefined,
        verticalId
      );

      if (status && status?.["warning"] === true) {
        rootStore.dispatch(
          setNotification({
            message: status?.["info"],
            notifyType: NotifyType.success,
          })
        );
        setShow(false);
        setImportExcelPopup(false);
        setVerticalSelectedImport(null);
        setImportTab(keyTabs);
        GetPassThroughReportGrid({
          ...query,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
        GetPassThroughHardwareReportGrid({
          ...queryHardware,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
        GetPassThroughSoftwareReportGrid({
          ...querySoftware,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        });
      } else if (status && status?.["warning"] === false) {
        const errorDetails = status?.["data"]?.errorDetails || [];
        setImportErrorDetails(errorDetails);
        setAlertStatus({
          message: status?.["info"],
          class: "danger",
        });
        setShow(true);
        setImportExcelPopup(false);
        setIserorShowModal(true);
        setVerticalSelectedImport(null);
        setImportTab("asset");
        setLoader("ADD", "GetPassThroughReportGrid");
        GetPassThroughReportGrid({
          ...query,
          nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
        }).then(() => setLoader("REMOVE", "GetPassThroughReportGrid"));
      }
    } catch (error) {
      setAlertStatus({
        message: "An error occurred during import. Please try again.",
        class: "danger",
      });
      setShow(true);
    }
  };

  const closeModalSetup = (changed: boolean) => {
    // Call all three APIs to refresh all grids
    Promise.all([
      GetPassThroughReportGrid({
        ...query,
        nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
      }),
      GetPassThroughHardwareReportGrid({
        ...queryHardware,
        nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
      }),
      GetPassThroughSoftwareReportGrid({
        ...querySoftware,
        nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
      }),
    ]).then(() => setIsVisibleModalSetup(false));
  };
  return (
    <div className="pageContainer">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          onHideModel();
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="xl"
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

      <Dialog
        open={importExcelPopup === true}
        onClose={() => {
          setImportExcelPopup(false);
          setVerticalSelectedImport(null);
          setImportTab(keyTabs);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Import Excel</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => {
            setImportExcelPopup(false);
            setVerticalSelectedImport(null);
            setImportTab(keyTabs);
          }}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <Form style={{ padding: "0px 10px " }}>
            <div className="mb-3">
              <Form.Check
                onClick={() => setImportTab("lcm-software")}
                checked={importTab === "lcm-software"}
                label="LCM Software"
                name="importGroup"
                type="radio"
                id="radio-import-lcm-software"
              />
              <Form.Check
                onClick={() => setImportTab("lcm-hardware")}
                checked={importTab === "lcm-hardware"}
                label="LCM Hardware"
                name="importGroup"
                type="radio"
                id="radio-import-lcm-hardware"
                className="mt-2"
              />
              <Form.Check
                onClick={() => setImportTab("asset")}
                checked={importTab === "asset"}
                label="Asset"
                name="importGroup"
                type="radio"
                id="radio-import-asset"
                className="mt-2"
              />
            </div>

            <div className="col-6 pl-0 mt-2">
              <DropdownInputComponent
                label={"Please Select Domain"}
                placeholderText="Select"
                labelCSS="mb-0 text-left"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                value={verticalSelectedImport}
                onChange={(e: any) => setVerticalSelectedImport(e)}
                options={versionResources}
                required={true}
                isError={verticalSelectedImport ? false : true}
                error={"Please select Domain"}
                // isAdd={tipologicaPermesso ? true : false}
                onAddClicked={() => setIsVisibleModalLookup(2)}
              />
            </div>
          </Form>
        </DialogContent>
        <DialogActions>
          <div className="d-flex mb-2">
            <button
              className="download-to-excel"
              onClick={() => {
                setImportExcelPopup(false);
                setVerticalSelectedImport(null);
                setImportTab(keyTabs);
              }}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => fileHandle()}
              disabled={!verticalSelectedImport}
            >
              Import Excel
            </button>
          </div>
        </DialogActions>
      </Dialog>

      <Dialog
        open={excelPopup === true}
        onClose={() => {
          setExcelPopUp(false);
          setDownloadVertical(null);
          setDownloadTab(keyTabs);
        }}
        maxWidth="lg"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Download to Excel</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => {
            setExcelPopUp(false);
            setDownloadVertical(null);
            setDownloadTab(keyTabs);
          }}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <Form style={{ padding: "0px 10px" }}>
            <div className="mb-3">
              <Form.Check
                onClick={() => setDownloadTab("lcm-software")}
                checked={downloadTab === "lcm-software"}
                label="LCM Software"
                name="downloadGroup"
                type="radio"
                id="radio-lcm-software"
              />
              <Form.Check
                onClick={() => setDownloadTab("lcm-hardware")}
                checked={downloadTab === "lcm-hardware"}
                label="LCM Hardware"
                name="downloadGroup"
                type="radio"
                id="radio-lcm-hardware"
                className="mt-2"
              />
              <Form.Check
                onClick={() => setDownloadTab("asset")}
                checked={downloadTab === "asset"}
                label="Asset"
                name="downloadGroup"
                type="radio"
                id="radio-asset"
                className="mt-2"
              />
            </div>

            <div className="col-6 pl-0 mt-3">
              <DropdownInputComponent
                label="Please Select Domain"
                placeholderText="Select Domain"
                labelCSS="mb-2 text-left"
                inputCSS="labelForm voda-bold"
                isSearchable={true}
                isClearable={true}
                value={downloadVertical}
                onChange={(e: any) => setDownloadVertical(e)}
                options={versionResources}
                required={true}
                isError={downloadVertical ? false : true}
                error="Please select Domain"
              />
            </div>
          </Form>
        </DialogContent>
        <DialogActions>
          <div className="d-flex mb-2">
            <button
              className="download-to-excel"
              onClick={() => {
                setExcelPopUp(false);
                setDownloadVertical(null);
                setDownloadTab(keyTabs);
              }}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => InvocheDownload()}
              disabled={!downloadVertical}
            >
              Download
            </button>
          </div>
        </DialogActions>
      </Dialog>
      <Dialog
        open={isVisibleModalSetup}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setIsVisibleModalSetup(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4>Setup Grid Informations</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalSetup(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>

        <DialogContent>
          <SetupColumns
            renderGrid={
              keyTabs === "lcm-hardware"
                ? renderGridStateHw
                : keyTabs === "lcm-software"
                ? renderGridStateSw
                : renderGridState
            }
            action={{ closeModalSetup }}
          ></SetupColumns>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold d-flex">Passthrough Data</h3>

        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="download-to-excel mrl-3 grid-main-btn"
              onClick={() => {
                setImportExcelPopup(true);
                setVerticalSelectedImport(null);
                setImportTab(keyTabs);
              }}
            >
              Import Excel
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => {
              setExcelPopUp(true);
              setDownloadTab(keyTabs);
              setDownloadVertical(selectedVersion);
            }}
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
                      {keyTabs === "asset" ? (
                        <>
                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                            <div className="legendaElement green"></div>
                            <span className="legendaElement">Asset</span>
                          </div>
                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                            <div className="legendaElement lightgreenv"></div>
                            <span className="legendaElement">Hardware</span>
                          </div>
                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                            <div className="legendaElement amber"></div>
                            <span className="legendaElement">Software</span>
                          </div>
                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                            <div className="legendaElement blu"></div>
                            <span className="legendaElement">
                              LCM Engineering
                            </span>
                          </div>
                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                            <div className="legendaElement yellow"></div>
                            <span className="legendaElement">
                              Virtualization
                            </span>
                          </div>
                        </>
                      ) : (
                        <>
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
                        </>
                      )}
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

      {show && (
        <div className="mt-2">
          <Alert
            variant={alerStatus?.class}
            onClose={() => setShow(false)}
            dismissible
          >
            <p>{alerStatus?.message}</p>
          </Alert>
        </div>
      )}

      <div className="row mb-3">
        <div className="col-2">
          {versionResources?.length > 0 && (
            <DropdownInputComponent
              label="Please Select Domain"
              labelCSS="mb-2 text-left"
              inputCSS="labelForm voda-bold"
              isSearchable={true}
              isAdd={false}
              onAddClicked={() => setIsVisibleModalLookup(1)}
              isClearable={true}
              value={selectedVersion}
              options={versionResources}
              onChange={(e: any) => {
                setSelectedVersion(e);
                onChangeDescriptionList(e);
              }}
            />
          )}
        </div>
      </div>

      <Tabs
        defaultActiveKey="lcm-software"
        id="passthrough-tabs"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="lcm-software" title="LCM Software">
          {keyTabs === "lcm-software" && versionResources?.length > 0 && (
            <>
              <PassThroughSoftwareReportGrid
                data={dataSoftware}
                pagination={querySoftware}
                renderGrid={renderGridStateSw?.render ?? []}
                action={{
                  Filter: setQuerySoftware,
                }}
              />
              <MUIPaginationComponent
                pagination={{
                  page: querySoftware.page,
                  pageSize: querySoftware.pageSize,
                }}
                totalItems={GridDtoSoftware?.totalItems}
                actions={{
                  next: nextSoftware,
                  back: backSoftware,
                  updatePageSize: updatePageSizeSoftware,
                }}
              />
            </>
          )}
        </Tab>
        <Tab eventKey="lcm-hardware" title="LCM Hardware">
          {keyTabs === "lcm-hardware" && versionResources?.length > 0 && (
            <>
              <PassThroughHardwareReportGrid
                data={dataHardware}
                pagination={queryHardware}
                renderGrid={renderGridStateHw?.render ?? []}
                action={{
                  Filter: setQueryHardware,
                }}
              />
              <MUIPaginationComponent
                pagination={{
                  page: queryHardware.page,
                  pageSize: queryHardware.pageSize,
                }}
                totalItems={GridDtoHardware?.totalItems}
                actions={{
                  next: nextHardware,
                  back: backHardware,
                  updatePageSize: updatePageSizeHardware,
                }}
              />
            </>
          )}
        </Tab>

        <Tab eventKey="asset" title="Asset">
          {keyTabs === "asset" && versionResources?.length > 0 && (
            <>
              <PassThroughReportGrid
                data={dataPassthrough}
                pagination={query}
                renderGrid={renderGridState?.render ?? []}
                action={{
                  Filter: setQuery,
                }}
              />

              <MUIPaginationComponent
                pagination={{
                  page: query.page,
                  pageSize: query.pageSize,
                }}
                totalItems={GridDto?.totalItems}
                actions={{
                  next: next,
                  back: back,
                  updatePageSize: updatePageSize,
                }}
              />
            </>
          )}
        </Tab>
      </Tabs>
      <Dialog
        open={iserorShowModal}
        onClose={() => {
          setIserorShowModal(false);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Import Error</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => {
            setIserorShowModal(false);
          }}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <div className="mt-2">
            <Alert variant={alerStatus?.class}>
              {importErrorDetails.length > 0 && (
                <div className="mt-3">
                  <strong>Error Details:</strong>
                  <ul className="mt-2 pl-3">
                    {importErrorDetails.map((error, index) => (
                      <li key={index} className="mb-1">
                        {error}
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </Alert>
          </div>
        </DialogContent>
        <DialogActions>
          <button
            className="download-to-excel"
            onClick={() => {
              setIserorShowModal(false);
              setImportErrorDetails([]);
              GetPassThroughReportGrid({
                ...query,
                nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
              });
              GetPassThroughHardwareReportGrid({
                ...queryHardware,
                nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
              });
              GetPassThroughSoftwareReportGrid({
                ...querySoftware,
                nonTemsVertical: selectedVersion ? [selectedVersion.key] : [],
              });
            }}
          >
            Close
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default PassThroughReport;
