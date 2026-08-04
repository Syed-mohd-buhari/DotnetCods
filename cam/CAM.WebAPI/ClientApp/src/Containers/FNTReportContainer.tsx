import React, { useCallback, useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { Alert, Dropdown, Form, Modal, Tab, Tabs } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import SetupColumns from "../screen/Shared/SetupColumns";
import setLoader from "../Redux/Action/LoaderAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import { GetTestInfoCreateResource } from "../Redux/Action/TestInfo/TestInfoCreateAction";
import {
  DeleteDeepTestInfo,
  RestoreTestInfo,
  deleteTestInfo,
} from "../Redux/Action/TestInfo/TestInfoDeleteAction";
import {
  EditTestInfo,
  GetTestInfoEditResource,
} from "../Redux/Action/TestInfo/TestInfoEditAction";
import TestInfoModal from "../screen/TestInfo/TestInfoModal";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import {
  GetTemsFNTReportGrid,
  GetTSRReportVerticalDropdown,
} from "../Redux/Action/FNTReport/TemsFNTReportGridAction";
import { GetNonTemsFNTReportGrid } from "../Redux/Action/FNTReport/NonTemsFNTReportGridAction";
import FNTReportGrid from "../screen/FNTReport/TemsFNTReportGrid";
import { FNTReportDtoGrid, FNTReportQueryObjectGrid } from "../Model/FNTReport";
import { TestInfoDtoCreate, TestInfoDtoUpdate } from "../Model/TestInfo";
import { GetTemsFNTReportExport } from "../Redux/Action/FNTReport/TemsFNTReportDownloadAction";
import { GetNonTemsFNTReportExport } from "../Redux/Action/FNTReport/NonTemsFNTReportDownloadAction";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { useNonTemsFNTReport } from "../Hook/FNTReportOverrideHook/useNonTemsFNTReport";
import { useTemsFNTReport } from "../Hook/FNTReportOverrideHook/useTemsFNTReport";
import TemsFNTReportGrid from "../screen/FNTReport/TemsFNTReportGrid";
import NonTemsFNTReportGrid from "../screen/FNTReport/NonTemsFNTReportGrid";
import FNTRefreshDataReport from "./FNTRefreshContainer";
import DialogActions from "@mui/material/DialogActions";
import { GetLatestRefreshStatus } from "../Redux/Action/FNTReport/TemsFNTReportImportAction";
import {
  DropdownInputComponent,
  ToggleInputComponent,
} from "../Components/FormField";
import { FNTReportApi } from "../Business/FNTReportBusiness";
import { Box } from "@mui/material";
import TSRReportVerticalContainer from "./TSRReportVerticalContainer";
import { dictionaryToArray } from "../Hook/Dictionary";

export let paginationQuery: FNTReportQueryObjectGrid = {
  temsFntReportId: [],
  hostName: [],
  serialNumberOfHardwareAsset: [],
  locationOfHardwareAsset: [],
  hardwareTypeOfHardwareAsset: [],
  vendorFnt: [],
  ipAddressOfHardwareAsset: [],
  market: [],
  hwEndOfLife: undefined,
  hwEndOfSupport: undefined,
  hwEndOfSale: undefined,
  hardwareModules: [],
  softwareProductType: [],
  softwareProductVersion: [],
  softwareIsVirtualized: [],
  operatingSystemOfVirtualMachine: [],
  applicationHostedOnSoftware: [],
  uuidSerialNumberOfSoftware: [],
  softwareVendor: [],
  locationOfSoftware: [],
  serviceType: [],
  swEndOfLife: undefined,
  swEndOfSupport: undefined,
  swEndOfSale: undefined,
  verticalEngineeringTeam: [],
  verticalSubdomain: [],
  platform: [],
  riskCluster: [],
  operationsContactPoint: [],
  assetCategory: [],
  assetClass: [],
  assetTypeFnt: [],
  assetDescriptionFnt: [],
  productImportanceFnt: [],
  operationsMaintenanceContract: [],
  vendorEndOfMaintenanceDateFnt: undefined,
  identifiedActionFnt: [],
  descriptionOfPlannedActionFnt: [],
  plannedHwModel: [],
  businessServiceName: [],
  opMaintenanceContractendDate: undefined,
  incidentClass: [],
  occurrenceProbability: [],
  meverticalResposible: [],
  assetStatus: [],
  typeOfNetworkElement: [],
  localMarket: [],
  application: [],
  cloud: [],
  dataCenterocation: [],
  physicalServerHostname: [],
  physicalServerIpaddress: [],
  physicalServerSerialNumber: [],
  physicalServerHwModel: [],
  physicalServerVendor: [],
  virtualServerHostedOn: [],
  virtualServerManufacturer: [],
  virtualServerTypeOfDevice: [],
  virtualMachineType: [],
  virtualServerIpaddress: [],
  virtualServerSerialNumber: [],
  virtualServerType: [],
  osName: [],
  osVersion: [],
  osStartDate: undefined,
  osInstallationDate: undefined,
  osStatus: [],
  softwareName: [],
  version: [],
  release: [],
  manufacturer: [],
  language: [],
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
  model: [],
  passThroughId: [],
  physicalServerOsName: [],
  physicalServerOsVersion: [],
  physicalServerOsStartDate: undefined,
  physicalServerOsInstallationDate: undefined,
  physicalServerOsStatus: [],
  hwOperationsContactPoint: [],
  hwOpsContractEndDate: undefined,
  hwOpsContractStatus: [],
  swOperationsContactPoint: [],
  swOpsContractEndDate: undefined,
  swOpsContractStatus: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  // added for NON-TEMS vertical filter, used by PassThrough-style dropdown
  nonTemsVertical: [],
};

const FNTReport = () => {
  //STATE CONFIRM
  const [keyTabs, setKeyTabs] = useState("tems");
  const [downloadOption, setDownloadOption] = useState("tems");
  const [importOption, setImportOption] = useState("tems");
  const [lastRefreshDate, setLastRefreshDate] = useState<any>(undefined);
  const [lastRefreshStatus, setLastRefreshStatus] = useState(false);
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState<number | null>(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [refreshFntModal, setRefreshFntModal] = useState(false);
  const [verticalSelected, setVerticalSelected] = useState<any>(null);
  const [orphanColor, setOrphanColor] = useState(false);
  const [excelPopup, setExcelPopUp] = useState<Boolean>(false);
  const [importExcelPopup, setImportExcelPopup] = useState<Boolean>(false);
  const [opCoRes, setOpCoRes] = useState<any>(null);
  const [opcoLastTime, setOpcoLastTime] = useState<any>(null);
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  //DTO TEMS
  const [temsData, setTemsData] = useState<FNTReportDtoGrid[] | undefined>([]);
  const [isVisibleScheduleModal, setIsVisibleScheduleModal] = useState(false);
  const [scheduleFlag, setScheduledFlag] = useState<boolean>(false);
  const [exportFilePath, setExportFilePath] = useState<any>(null);
  const [exportFileFormat, setExportFileFormat] = useState<any>(null);
  const [reportSchedulerId, setReportSchedulerId] = useState<number | null>(
    null
  );
  const [scheduledDate, setScheduledDate] = useState<any>(null);
  const [exportType, setExportType] = useState<any>(null);
  const [errorField, setErrorField] = useState<any>([]);
  const [scheduleDayType, setScheduleDayType] = useState<{
    scheduledType: "1" | "2" | "";
  }>({
    scheduledType: "1",
  });
  const [fileType, setFileType] = useState<"excel" | "csv">("excel");
  const [isGlossary, setIsGlossary] = useState(false);

  let GridDtoTems = useSelector(
    (state: RootState) => state.temsFntReportGridReducer.TemsFNTReportGridResult
  );
  //DTO NON-TEMS
  const [nonTemsData, setNonTemsData] = useState<
    FNTReportDtoGrid[] | undefined
  >([]);
  let GridDtoNonTems = useSelector(
    (state: RootState) =>
      state.nonTemsFntReportGridReducer.NonTemsFNTReportGridResult
  );
  let fileFormat = [{ key: "excel", value: "Excel" }];
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.testInfoCreateReducer.ResultDtoCreate
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, tipologicaPermesso, pageSize } = useAuth();
  const [renderTemsGridState, setRenderTemsGridState] = useState<any>();
  const [renderNonTemsGridState, setRenderNonTemsGridState] = useState<any>();
  const [verticalResource, setVerticalResource] = useState<any>();

  const [versionResources, setVersionResources] = useState<any[]>([]);
  const [selectedVersion, setSelectedVersion] = useState<any>(null);

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const { darkMode } = useTheme();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { queryTems, setQueryTems, nextTems, backTems } = useTemsFNTReport(
    paginationQuery,
    undefined
  );

  const { queryNonTems, setQueryNonTems, nextNonTems, backNonTems } =
    useNonTemsFNTReport(paginationQuery, undefined);

  useEffect(() => {
    if (isPermesso) {
      setLoader("ADD", "GetTemsFNTReportGrid");
      GetTemsFNTReportGrid({
        ...queryTems,
        // recordClassifier: [1],
      });
      GetVerticalGrid();
    }
  }, [isPermesso, queryTems, setQueryTems, nextTems, backTems]);

  useEffect(() => {
    if (isPermesso && selectedVersion) {
      const nonTemsVertical =
        (queryNonTems as any).nonTemsVertical?.length > 0
          ? (queryNonTems as any).nonTemsVertical
          : selectedVersion && selectedVersion.key
          ? [selectedVersion.key]
          : [];

      GetNonTemsFNTReportGrid({
        ...queryNonTems,
        // recordClassifier: [2],
        nonTemsVertical,
      });
    }
  }, [
    isPermesso,
    queryNonTems,
    setQueryNonTems,
    nextNonTems,
    backNonTems,
    selectedVersion,
  ]);
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    closeModal();
    GetTemsFNTReportGrid({
      ...queryTems,
      // recordClassifier: [1],
    });
    GetNonTemsFNTReportGrid({
      ...queryNonTems,
      // recordClassifier: [2],
      nonTemsVertical:
        selectedVersion && selectedVersion.key ? [selectedVersion.key] : [],
    });
  };
  const fetchScheduledReports = async () => {
    try {
      setLoader("ADD", "FetchScheduledReports");

      const api = new FNTReportApi();
      const payload = {
        exportFileFormat,
        scheduledDayInWeek: "",
        exportFilePath,
        scheduledDate,
        reportName: ["FNT"],
      };

      const result = await api.ReportSchedulerGet(payload);

      if (result && result.items) {
        if (result.items.length > 0) {
          const latestSchedule = result.items[0];
          const isScheduledFlag =
            latestSchedule.isScheduled === "Yes" ? true : false;
          setScheduledFlag(isScheduledFlag);
          setReportSchedulerId(latestSchedule.reportSchedulerId || null);

          if (latestSchedule.exportFileFormat) {
            const formatOption = fileFormat.find(
              (f) => f.key === latestSchedule.exportFileFormat
            );
            setExportFileFormat(formatOption || null);
          }

          if (
            latestSchedule.scheduledDate !== null &&
            latestSchedule.scheduledDate !== undefined
          ) {
            setScheduleDayType({ scheduledType: "1" });

            const dateOptions = Array.from({ length: 32 }, (_, i) => ({
              key: i.toString(),
              value: i === 0 ? "All Day" : scheduledMsgFormat(i.toString()),
            }));

            const selectedDateOption = dateOptions.find(
              (option) =>
                option.key === latestSchedule.scheduledDate!.toString()
            );

            setScheduledDate(selectedDateOption || null);
          }

          if (latestSchedule.scheduledDayInWeek) {
            setScheduleDayType({ scheduledType: "2" });
            setScheduledDate({
              key: latestSchedule.scheduledDayInWeek,
              value: latestSchedule.scheduledDayInWeek,
            });
          }
        }
      }

      setLoader("REMOVE", "FetchScheduledReports");
    } catch (error) {
      setLoader("REMOVE", "FetchScheduledReports");
      console.error("Error fetching scheduled reports:", error);
    }
  };

  const resetMoreOptionData = () => {
    setExportFileFormat(null);
    setExportFilePath(null);
    setExportType(null);
    setScheduledDate(null);
    setScheduleDayType({ scheduledType: "1" });
    setErrorField([]);
    setReportSchedulerId(null);
  };

  const handleMoreOptions = async () => {
    const errors: string[] = [];

    if (exportFileFormat === null) {
      errors.push("exportFileFormat");
    }
    if (scheduledDate === null) {
      errors.push("scheduledDate");
    }
    if (scheduledDate?.value === undefined) {
      errors.push("scheduledDate");
    }

    setErrorField(errors);

    if (
      exportFileFormat !== null &&
      scheduledDate !== null &&
      scheduledDate?.value !== undefined
    ) {
      try {
        setLoader("ADD", "ScheduleReport");

        const api = new FNTReportApi();

        const schedulePayload = {
          reportSchedulerId: reportSchedulerId ?? 0,
          reportName: "FNT",
          isScheduled: scheduleFlag ? "Yes" : "No",

          exportFileFormat: exportFileFormat.key,
          scheduledDate:
            scheduleDayType.scheduledType === "1"
              ? parseInt(scheduledDate.key)
              : 0,
          scheduledDayInWeek:
            scheduleDayType.scheduledType === "2" ? scheduledDate.value : null,
          lastModifiedBy: "",
        };

        const response = await api.ReportSchedulerUpdate(schedulePayload);

        setLoader("REMOVE", "ScheduleReport");

        if (response) {
          rootStore.dispatch(
            setNotification({
              message: `Report scheduled successfully for ${
                scheduleDayType.scheduledType === "1"
                  ? scheduledDate.key === "0"
                    ? "all days of the month"
                    : `${scheduledDate.value} of the month`
                  : scheduledDate.value
              }`,
              notifyType: NotifyType.success,
            })
          );

          resetMoreOptionData();
          setIsVisibleScheduleModal(false);
          setErrorField([]);
        }
      } catch (error) {
        setLoader("REMOVE", "ScheduleReport");
        console.error("Error scheduling report:", error);

        rootStore.dispatch(
          setNotification({
            message: "Failed to schedule report. Please try again.",
            notifyType: NotifyType.error,
          })
        );
      }
    }
  };

  const scheduledMsgFormat = (val: string) => {
    const num = Number(val);
    if (num) {
      const suffixes = ["th", "st", "nd", "rd"];
      const v = num % 100;
      return num + (suffixes[(v - 20) % 10] || suffixes[v] || suffixes[0]);
    } else return "";
  };

  const {
    New,
    Edit,
    isVisibleModal,
    edit,
    confirm,
    closeModal,
    Delete,
    localStateHistory,
    setLocalState,
    Restore,
  } = useOperationTableCrud<TestInfoDtoUpdate, TestInfoDtoCreate>(
    GetTestInfoCreateResource,
    GetTestInfoEditResource,
    deleteTestInfo,
    refresh,
    RestoreTestInfo
  );

  const resetQuery = () => {
    setQueryTems(paginationQuery);
    setQueryNonTems(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO TEMS
  useEffect(() => {
    if (GridDtoTems !== undefined || GridDtoTems !== null) {
      setTemsData(GridDtoTems?.items);
      let copy = { ...GridDtoTems?.gridRender } as CustomGridRender | undefined;
      setRenderTemsGridState(copy);
      setLoader("REMOVE", "GetTemsFNTReportGrid");
    }
  }, [GridDtoTems]);

  //UPDATE ON CHANGE DTO NON-TEMS
  useEffect(() => {
    if (GridDtoNonTems !== undefined || GridDtoNonTems !== null) {
      setNonTemsData(GridDtoNonTems?.items);
      let copy = { ...GridDtoNonTems?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderNonTemsGridState(copy);
      setLoader("REMOVE", "GetNonTemsFNTReportGrid");
    }
  }, [GridDtoNonTems]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetFNTReportGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        ids?: number[];
      };

      if (localState?.id != null) {
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copyTems = { ...queryTems } as FNTReportQueryObjectGrid;
        copyTems.principalId = localState?.id;
        // copyTems.recordClassifier = [1];
        setQueryTems(copyTems);
        GetTemsFNTReportGrid(copyTems).then((x) =>
          setLoader("REMOVE", "GetTemsFNTReportGrid")
        );
        let copyNonTems = { ...queryNonTems } as FNTReportQueryObjectGrid;
        copyNonTems.principalId = localState?.id;
        // copyNonTems.recordClassifier = [2];
        setQueryTems(copyNonTems);
        GetNonTemsFNTReportGrid(copyNonTems).then((x) =>
          setLoader("REMOVE", "GetNonTemsFNTReportGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetFNTReportGrid");
  }, []);

  const GetVerticalGrid = async () => {
    const payload = {
      appSettingsId: [2],
      sortBy: "",
      isSortAscending: false,
      page: 1,
      pageSize: 10,
      lastModifiedBy: [],
    };

    const result: any = await GetTSRReportVerticalDropdown(payload);

    if (result !== undefined && result !== null) {
      const dataArray = result.LookUpGridResult;

      if (dataArray && dataArray.length > 0) {
        const mapped = dataArray.map((val: any) => ({
          key: val.key,
          value: val.text,
          appSettingConfigurationId: val.key,
        }));

        setVerticalResource(mapped);
        setVersionResources(mapped);

        if (mapped.length > 0) {
          setSelectedVersion(mapped[0]);
          setQueryNonTems({
            ...(queryNonTems as FNTReportQueryObjectGrid),
            nonTemsVertical: [mapped[0].key],
          } as FNTReportQueryObjectGrid);
        }
      }
    }
  };

  const closeModalSetup = (changed?: boolean) => {
    GetTemsFNTReportGrid({
      ...queryTems,
    }).then((x) => setIsVisibleModalSetup(false));
    GetNonTemsFNTReportGrid({
      ...queryNonTems,
    }).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    if (downloadOption === "nontems" && !verticalSelected) {
      rootStore.dispatch(
        setNotification({
          message: "Please select a vertical for NON-TEMS export",
          notifyType: NotifyType.error,
        })
      );
      return;
    }

    let payload: FNTReportQueryObjectGrid = {
      ...queryTems,
      isGlossary: isGlossary,
    };

    // Set recordClassifier based on download option
    if (downloadOption === "tems") {
      payload.recordClassifier = [1];
    } else if (downloadOption === "nontems") {
      payload.recordClassifier = [2];
      if (verticalSelected != null && verticalSelected != undefined) {
        payload.nonTemsVertical = [verticalSelected?.key];
      }
    }

    let result;

    if (downloadOption === "tems") {
      result = await GetTemsFNTReportExport(payload);
    } else if (downloadOption === "nontems") {
      result = await GetNonTemsFNTReportExport(payload);
    }

    if (result !== undefined) {
      setExcelPopUp(false);
      setIsGlossary(false);
      const url = window.URL.createObjectURL(result.file);
      const a = document.createElement("a");

      const fileName = result.fileName.endsWith(".xlsx")
        ? result.fileName
        : result.fileName.replace(/\.(xlsx|csv)$/, "") + ".xlsx";

      a.href = url;
      a.download = fileName;
      a.click();
    }
  };
  const fileHandle = async () => {
    try {
      const sheetName = "FNT Report";
      const apiPath = "FNTReport";
      let mode = [0];
      let verticalId = [0];
      if (importOption === "nontems") {
        mode = [2];
        verticalId = [verticalSelected.key];
      }
      const status = await handleImportFile(
        sheetName,
        apiPath,
        mode,
        verticalId
      );
      if (status && status?.["warning"] === true) {
        rootStore.dispatch(
          setNotification({
            message: status?.["info"],
            notifyType: NotifyType.success,
          })
        );
        refresh();
        setShow(false);
        setImportExcelPopup(false);
      } else if (status && status?.["warning"] === false) {
        setAlertStatus({
          message: status?.["info"],
          class: "danger",
        });
        setShow(true);
        setImportExcelPopup(false);
        setLoader("ADD", "GetFNTReportGrid");
        closeModal();
        GetTemsFNTReportGrid({
          ...queryTems,
          // recordClassifier: [1],
        }).then(() => setLoader("REMOVE", "GetTemsFNTReportGrid"));
        // GetNonTemsFNTReportGrid({
        //   ...queryNonTems,
        //   recordClassifier: [2],
        // }).then(() => setLoader("REMOVE", "GetNonTemsFNTReportGrid"));
      }
    } catch (error) {
      console.log(error);
    }
  };

  const checkRefreshData = async () => {
    const filterId = 2;
    const result: any = await GetLatestRefreshStatus(filterId);
    if (result?.isRefreshCompleted) {
      setOpCoRes(
        result?.opCoList !== null ? dictionaryToArray(result?.opCoList) : null
      );
      setOpcoLastTime(
        result?.opcoWiseLastModifiedDate !== null
          ? dictionaryToArray(result?.opcoWiseLastModifiedDate)
          : null
      );
      setLastRefreshDate(result?.endTime ?? undefined);
      setLastRefreshStatus(false);
      setRefreshFntModal(true);
    } else {
      setOpCoRes(null);
      setLastRefreshDate(undefined);
      setRefreshFntModal(false);
      setLastRefreshStatus(true);
      setTimeout(() => {
        setLastRefreshStatus(false);
      }, 60 * 2 * 1000);
    }
  };

  const refreshFntMsg = (obj) => {
    setRefreshFntModal(false);
    setShow(!obj?.warning);
    setAlertStatus({
      message: obj?.info,
      class: !obj?.warning ? "success" : "error",
    });
  };

  return (
    <div className="pageContainer">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => setIsVisibleModalLookup(0)}
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
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {/* {ReturnLookupContainer(isVisibleModalLookup)} */}
        </DialogContent>
      </Dialog>
      <ModalConfirm data={confirm} />
      <ModalConfirm data={myConfirm} />
      <Dialog
        open={isVisibleModal}
        onClose={() => closeModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>FNT Report</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeModal(false)}
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
          <TestInfoModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
      </Dialog>
      <Dialog
        open={excelPopup === true}
        onClose={() => setExcelPopUp(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Download To Excel</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setExcelPopUp(false)}
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
            {["radio"].map((type) => (
              <>
                <span className="my-4 voda-bold">
                  Please select Export Type.
                </span>
                <div key={`inline-excel-${type}`} className="mt-2 mb-3"></div>
                <div key={`inline-${type}`} className="mb-3">
                  <Form.Check
                    onClick={() => setDownloadOption("tems")}
                    inline
                    checked={downloadOption === "tems" ? true : false}
                    label="TEMS"
                    name="group1"
                    type={"radio"}
                    id={`inline-${type}-1`}
                  />
                  <Form.Check
                    onClick={() => setDownloadOption("nontems")}
                    inline
                    checked={downloadOption === "nontems" ? true : false}
                    label="NON-TEMS"
                    name="group1"
                    type={"radio"}
                    id={`inline-${type}-2`}
                  />

                  {downloadOption === "nontems" && (
                    <div className="col-6 pl-0 mt-2">
                      <DropdownInputComponent
                        label={"Please Select Domain"}
                        placeholderText="Select"
                        labelCSS="mb-0 text-left"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable={true}
                        isClearable={true}
                        value={verticalSelected}
                        onChange={(e: any) => setVerticalSelected(e)}
                        options={verticalResource}
                      />
                    </div>
                  )}
                </div>
              </>
            ))}
          </Form>
        </DialogContent>
        <DialogActions>
          <div className="d-flex mb-2">
            <button
              className="download-to-excel"
              onClick={() => {
                setExcelPopUp(false);
                setIsGlossary(false);
              }}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => InvocheDownload()}
            >
              Download
            </button>
          </div>
        </DialogActions>
      </Dialog>
      <Dialog
        open={importExcelPopup === true}
        onClose={() => setImportExcelPopup(false)}
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
          onClick={() => setImportExcelPopup(false)}
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
            {["radio"].map((type) => (
              <div key={`inline-${type}`} className="mb-3">
                <Form.Check
                  onClick={() => setImportOption("tems")}
                  inline
                  checked={importOption === "tems" ? true : false}
                  label="TEMS"
                  name="group1"
                  type={"radio"}
                  id={`inline-${type}-1`}
                />
              </div>
            ))}
          </Form>
        </DialogContent>
        <DialogActions>
          <div className="d-flex mb-2">
            <button
              className="download-to-excel"
              onClick={() => setImportExcelPopup(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => fileHandle()}
            >
              Import Excel
            </button>
          </div>
        </DialogActions>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          {redirect === true ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">FNT Report</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && keyTabs === "tems" && (
            <button
              className="download-to-excel mrl-3 grid-main-btn"
              onClick={() => {
                setImportExcelPopup(true);
                setVerticalSelected(null);
              }}
            >
              Import Excel
            </button>
          )}
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => {
              setExcelPopUp(true);
              setDownloadOption(keyTabs);
              setVerticalSelected(
                keyTabs === "nontems" ? selectedVersion : null
              );
            }}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown
            className="d-inline more-options grid-main-btn
"
          >
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu
              className="grid-main-btn
"
            >
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
              {keyTabs === "tems" ? (
                <>
                  <Dropdown.Item
                    onClick={() => !lastRefreshStatus && checkRefreshData()}
                  >
                    Refresh FNT Data
                  </Dropdown.Item>
                </>
              ) : null}
              <Dropdown.Item
                onClick={() => {
                  fetchScheduledReports();
                  setIsVisibleScheduleModal(true);
                }}
              >
                Schedule Report
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <div className="row mx-0 justify-content-end">
        <span className="voda-bold align-center">
          {`${
            lastRefreshStatus
              ? "A FNT refresh data is already in progress. Please wait until it is complete"
              : ""
          }`}
        </span>
      </div>
      <Modal
        show={isVisibleModalSetup}
        backdrop="static"
        keyboard={false}
        size="lg"
        onHide={closeModal}
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body className="plr-30">
          <SetupColumns
            renderGrid={
              keyTabs === "tems" ? renderTemsGridState : renderNonTemsGridState
            }
            action={{ closeModalSetup }}
          ></SetupColumns>
        </Modal.Body>
      </Modal>

      <Dialog
        open={refreshFntModal}
        onClose={() => setRefreshFntModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Refresh FNT Data</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setRefreshFntModal(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent className="pt-0">
          <FNTRefreshDataReport
            endTime={lastRefreshDate}
            redirect={"fntReport"}
            opcoRes={opCoRes}
            opcoLastTime={opcoLastTime}
            modal={{
              isModal: true,
              setRefreshFntModal,
              closeModalSetup,
              refreshFntMsg,
            }}
          />
        </DialogContent>
      </Dialog>
      {show && (
        <div className="mt-2">
          <Alert
            variant={alerStatus?.class}
            onClose={() => setShow(false)}
            dismissible
          >
            <p className="mb-0">{alerStatus?.message}</p>
          </Alert>
        </div>
      )}
      <Modal
        show={isVisibleScheduleModal}
        backdrop="static"
        keyboard={false}
        onHide={() => {
          resetMoreOptionData();
          setIsVisibleScheduleModal(false);
        }}
        size="lg"
        centered
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-1">Schedule Report</h4>
            </div>
          </div>
        </Modal.Header>

        <Modal.Body className="mx-2">
          <div className="col-12 p-0 mt-4 mx-2">
            <fieldset className="fieldset p-0">
              <div className="d-flex flex-column w-100 px-3 mb-3">
                <div className="mb-2">
                  <ToggleInputComponent
                    label={"Do you want to schedule this report?"}
                    value={scheduleFlag ?? false}
                    required={false}
                    onChange={(e: any) => {
                      setScheduledFlag(!scheduleFlag);
                    }}
                  />
                </div>
              </div>

              {scheduleFlag && (
                <>
                  <div className="row">
                    <div className="form-group col-6 pl-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold mb-2">
                          Schedule Type
                        </label>
                        <div className="btn-group w-100" role="group">
                          <button
                            type="button"
                            style={{
                              backgroundColor:
                                scheduleDayType.scheduledType === "1"
                                  ? "red"
                                  : "transparent",
                              color:
                                scheduleDayType.scheduledType === "1"
                                  ? "white"
                                  : "black",
                              border: "1px solid red",
                            }}
                            className="btn btn-sm "
                            onClick={() => {
                              setScheduleDayType({ scheduledType: "1" });
                              setScheduledDate(null);
                            }}
                          >
                            Monthly
                          </button>
                          <button
                            type="button"
                            style={{
                              backgroundColor:
                                scheduleDayType.scheduledType === "2"
                                  ? "red"
                                  : "transparent",
                              color:
                                scheduleDayType.scheduledType === "2"
                                  ? "white"
                                  : "black",
                              border: "1px solid red",
                            }}
                            className="btn btn-sm "
                            onClick={() => {
                              setScheduleDayType({ scheduledType: "2" });
                              setScheduledDate(null);
                            }}
                          >
                            Weekly
                          </button>
                        </div>
                      </div>
                    </div>

                    <div className="form-group col-6 pl-0">
                      <div className="col-12">
                        <DropdownInputComponent
                          label={"Scheduled Day"}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          isError={
                            ((scheduledDate?.value === undefined ||
                              scheduledDate === null) &&
                              errorField.includes("scheduledDate")) ??
                            false
                          }
                          error={
                            scheduleDayType.scheduledType === "2"
                              ? "Schedule day is required."
                              : "Schedule date is required."
                          }
                          value={scheduledDate ? scheduledDate : null}
                          options={
                            scheduleDayType.scheduledType === "2"
                              ? [
                                  { key: "Monday", value: "Monday" },
                                  { key: "Tuesday", value: "Tuesday" },
                                  { key: "Wednesday", value: "Wednesday" },
                                  { key: "Thursday", value: "Thursday" },
                                  { key: "Friday", value: "Friday" },
                                  { key: "Saturday", value: "Saturday" },
                                  { key: "Sunday", value: "Sunday" },
                                ]
                              : Array.from({ length: 32 }, (_, i) => ({
                                  key: i.toString(),
                                  value:
                                    i === 0
                                      ? "All Day"
                                      : scheduledMsgFormat(i.toString()),
                                }))
                          }
                          onChange={(e: any) => {
                            setScheduledDate(e);
                          }}
                          successMessage={
                            scheduleDayType.scheduledType === "1"
                              ? scheduledDate?.key === "0"
                                ? "Report Scheduled on All days of the Month"
                                : scheduledDate?.value
                                ? `Report Scheduled on ${scheduledDate.value} of the Month`
                                : undefined
                              : scheduledDate?.value
                              ? `Report scheduled on ${scheduledDate.value}`
                              : undefined
                          }
                        />
                      </div>
                    </div>
                  </div>

                  <div className="row pt-4 w-100">
                    <div className="form-group col-6 pl-0">
                      <div className="col-12">
                        <DropdownInputComponent
                          label={"Export File Format"}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          isError={
                            (exportFileFormat === null &&
                              errorField.includes("exportFileFormat")) ??
                            false
                          }
                          error="Export file format is required."
                          value={exportFileFormat}
                          options={fileFormat}
                          onChange={(e: any) => setExportFileFormat(e)}
                        />
                      </div>
                    </div>
                  </div>
                </>
              )}

              <div className="col-12 justify-content-end d-flex footerModal">
                <button
                  className="voda-bold btn btn-link px-4 btnHeader cancel"
                  onClick={() => {
                    resetMoreOptionData();
                    setIsVisibleScheduleModal(false);
                  }}
                  type="button"
                >
                  Cancel
                </button>
                <button
                  className="voda-bold btn btn-link px-4 btnHeader cancel-mr"
                  type="button"
                  onClick={() => handleMoreOptions()}
                >
                  Save
                </button>
              </div>
            </fieldset>
          </div>
        </Modal.Body>
      </Modal>
      <Tabs
        defaultActiveKey="tems"
        id="fntReport"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="tems" title="TEMS">
          {keyTabs === "tems" && (
            <>
              <TemsFNTReportGrid
                data={temsData}
                pagination={{
                  ...queryTems,
                  // recordClassifier: keyTabs === "tems" ? [1] : [],
                }}
                orphanColor={orphanColor}
                renderGrid={renderTemsGridState?.render ?? []}
                action={{
                  Delete,
                  Edit,
                  Filter: setQueryTems,
                }}
              ></TemsFNTReportGrid>
              <Paginate
                pagination={{
                  page: queryTems.page,
                  pageSize: queryTems.pageSize,
                }}
                totalItems={GridDtoTems?.totalItems}
                actions={{ next: nextTems, back: backTems }}
              />
            </>
          )}
        </Tab>
        <Tab eventKey="nontems" title="NON-TEMS">
          {keyTabs === "nontems" && (
            <>
              {versionResources?.length > 0 && (
                <div className="row mb-3 mt-2">
                  <div className="col-3">
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
                        setQueryNonTems({
                          ...queryNonTems,
                          nonTemsVertical: e ? [e.key] : [],
                          page: 1,
                        } as FNTReportQueryObjectGrid);
                      }}
                    />
                  </div>
                </div>
              )}

              <NonTemsFNTReportGrid
                data={nonTemsData}
                pagination={{
                  ...queryNonTems,
                  // recordClassifier: keyTabs === "nontems" ? [2] : [],
                }}
                orphanColor={orphanColor}
                renderGrid={renderNonTemsGridState?.render ?? []}
                action={{
                  Delete,
                  Edit,
                  Filter: setQueryNonTems,
                }}
              ></NonTemsFNTReportGrid>
              <Paginate
                pagination={{
                  page: queryNonTems.page,
                  pageSize: queryNonTems.pageSize,
                }}
                totalItems={GridDtoNonTems?.totalItems}
                actions={{ next: nextNonTems, back: backNonTems }}
              />
            </>
          )}
        </Tab>
      </Tabs>
    </div>
  );
};

export default FNTReport;
