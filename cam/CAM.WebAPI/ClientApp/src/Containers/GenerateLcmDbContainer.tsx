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
  GetHistoricalDropDown,
  GetReportHardwareGrid,
  GetSharedLookUpGrid,
} from "../Redux/Action/Report/ReportHardwareGridAction";
import { DownloadReport } from "../Redux/Action/Report/ReportDownloadAction";
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

import { CustomGridRender, ReportViewMode } from "../Model/Common";
import { ReportQueryAllDto } from "../Model/Report/Export";
import { useAuth } from "../Hook/useAuth";
import { convertEnumToArray } from "../Hook/Common";
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
  isHistorical: false,
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
  isHistorical: false,
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

const GenerateLcmDb: React.FC = (props) => {
  sessionStorage.setItem("sharedName", "LcmExportSetting");
  //PAGE
  const [keyTabs, setKeyTabs] = useState("hardware");
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const { readonly, tipologicaPermesso, isPermesso } = useAuth();

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

  const [versionsList, setVersionsList] = useState();
  const [versionObj, setVersionObj] = useState<any>();
  const [isHistoricalFlag, setIsHistoricalFlag] = useState<boolean>(false);
  const [isDefaultFlag, setIsDefaultFlag] = useState<boolean>(true);
  const [viewHistorical, setViewHistorical] = useState<boolean>(false);
  const [pageTitle, setPageTitle] = useState("");
  const [selectedVersion, setSelectedVersion] = useState<{
    key: number;
    value: string;
    isHistorical: boolean;
    isDefault: boolean;
  } | null>(null);
  const [versionResources, setVersionResources] = useState<
    { key: number; value: string; isHistorical: boolean; isDefault: boolean }[]
  >([]);

  const GridSoftware = (state: RootState) =>
    state.reportSoftwareGridReducer.ReportSoftwareGridResult;
  let GridDtoSoftware = useSelector(GridSoftware);

  const getHardwareQuery = () => {
    return {
      ...paginationQueryHardware,
      lcmExportDescription:
        isPermesso && versionResources?.length > 0 && selectedVersion
          ? pageTitle
          : "",
    };
  };

  const { queryHardware, setQueryHardware, nextHardware, backHardware } =
    useGenerateLcmDbHardware(
      {
        ...paginationQueryHardware,
        isHistorical: isHistoricalFlag,
        lcmExportDescription: pageTitle,
      },
      undefined
    );
  const { querySoftware, setQuerySoftware, nextSoftware, backSoftware } =
    useGenerateLcmDbSoftware(
      {
        ...paginationQuerySoftware,
        isHistorical: isHistoricalFlag,
        lcmExportDescription: pageTitle,
      },
      undefined
    );

  useEffect(() => {
    if (isPermesso && selectedVersion) {
      GetReportSoftwareGrid({
        ...querySoftware,
        isHistorical: isHistoricalFlag,
        lcmExportDescription: pageTitle,
      });
    }
  }, [
    isPermesso,
    selectedVersion,
    querySoftware,
    setQuerySoftware,
    nextSoftware,
    backSoftware,
  ]);

  useEffect(() => {
    if (isPermesso && selectedVersion) {
      GetReportHardwareGrid({
        ...queryHardware,
        isHistorical: isHistoricalFlag,
        lcmExportDescription: pageTitle,
      });
    }
  }, [
    selectedVersion,
    queryHardware,
    setQueryHardware,
    nextHardware,
    backHardware,
  ]);
  const [queryAll, setQueryAll] = useState<ReportQueryAllDto>({});
  const [excelPopup, setExcelPopUp] = useState<Boolean>(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);

  const [disaggregatedType, setDisaggregatedType] = useState(0);

  const [renderGridStateHw, setRenderGridStateHw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateSw, setRenderGridStateSw] = useState<
    CustomGridRender | undefined
  >();

  const { darkMode } = useTheme();

  const onChangeType = (e) => {
    setDataType(e?.key);
    if (e && e?.key === 1) {
      const HWCopy = {
        ...paginationQueryHardware,
      } as ReportHardwareQueryObjectGrid;

      const SWCopy = {
        ...paginationQuerySoftware,
      } as ReportSoftwareQueryObjectGrid;

      HWCopy.ViewMode = e?.key;
      SWCopy.ViewMode = e?.key;
      setQueryHardware(HWCopy);
      setQuerySoftware(SWCopy);
      setDisaggregatedType(0);
    } else {
      const e = { value: "Muli Level", key: 2 };
      onChangeDisaggregatedType(e);
    }
  };

  const onChangeDisaggregatedType = (e: any) => {
    const HWCopy = {
      ...paginationQueryHardware,
      isHistorical: isHistoricalFlag,
    } as ReportHardwareQueryObjectGrid;
    const SWCopy = {
      ...paginationQuerySoftware,
      isHistorical: isHistoricalFlag,
    } as ReportSoftwareQueryObjectGrid;

    HWCopy.ViewMode = 2;
    SWCopy.ViewMode = 2;
    setQueryHardware(HWCopy);
    setQuerySoftware(SWCopy);
  };

  const onChangeDescriptionList = (e: any) => {
    let flag =
      versionResources?.length > 0
        ? versionResources?.filter((res: any) => res?.key == e?.key)[0]
            .isHistorical
        : false;
    const HWCopy = {
      ...queryHardware,
      isHistorical: flag,
    } as ReportHardwareQueryObjectGrid;

    const SWCopy = {
      ...querySoftware,
      isHistorical: flag,
    } as ReportSoftwareQueryObjectGrid;
    setIsHistoricalFlag(flag);
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
    setExcelPopUp(false);
    let result = await DownloadReport(queryAll);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };
  const [dataType, setDataType] = useState(1);

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
    const versionRes = value?.map((val: any) => {
      return {
        key: val.id,
        value: val.description,
        isHistorical: val.isHistorical,
      } as {
        key: number;
        value: string;
        isHistorical: boolean;
      };
    });
    setSelectedVersion(
      versionRes?.filter((res) => res.isDefault)[0] ?? versionRes[0]
    );
    setIsHistoricalFlag(versionRes[0]?.isHistorical ?? false);
    setIsDefaultFlag(versionRes[0]?.isDefault ?? false);
    setVersionResources(versionRes);
    setVersionObj(value);
    setPageTitle(versionRes[0].value);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <SharedLookUp
            returnObject={VersionsRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="LcmExportSetting"
            isHistoricalTab={viewHistorical}
          />
        );

      default:
        return null;
    }
  };
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    // setQueryHardware(paginationQueryHardware);
    // setQuerySoftware(paginationQuerySoftware);
    if (isPermesso) {
      callSharedLookUpApi();
    }
  }, [isPermesso, viewHistorical]);

  const callSharedLookUpApi = async () => {
    if (!viewHistorical) {
      const result = await GetSharedLookUpGrid({});
      if (result?.items !== undefined) {
        VersionsRefillData(
          result?.items.filter((item) => item.isHistorical == false)
        );
      }
    } else {
      const rtn = await GetHistoricalDropDown();
      if (rtn?.items !== undefined) {
        VersionsRefillData(rtn?.items);
      }
    }
  };

  useEffect(() => {
    let copy = { ...queryAll } as ReportQueryAllDto;
    if (queryHardware && querySoftware && isPermesso) {
      copy.queryHardware = queryHardware;
      copy.querySoftware = querySoftware;
      copy.activeTab = keyTabs;
      copy.lcmExportDescription = pageTitle;
      setQueryAll(copy);
    }
  }, [queryHardware, querySoftware, keyTabs, pageTitle]);

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

  const onApiCallChange = (hwQuery, swQuery) => {
    if (hwQuery !== null) {
      setQueryHardware(hwQuery);
    }
    if (swQuery !== null) {
      setQuerySoftware(swQuery);
    }
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
              <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <SetupColumns
            renderGrid={
              keyTabs === "hardware" ? renderGridStateHw : renderGridStateSw
            }
            action={{ closeModalSetup }}
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
            onClick={() => InvocheDownload()}
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

      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold d-flex">
          {viewHistorical ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-0 btnEditLink"
              //to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => setViewHistorical(false)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          {viewHistorical ? "Historical -" + pageTitle : pageTitle}
        </h3>

        <div className="d-flex">
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => setExcelPopUp(true)}
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
              <Dropdown.Item onClick={() => setViewHistorical(true)}>
                Historical Reports
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <div className="row mb-3">
        {/* <div className="col-3">
          <Select
            className="w-100"
            options={convertEnumToArray(ReportViewMode)}
            value={convertEnumToArray(ReportViewMode).filter(
              (x) => x.key === dataType
            )}
            onChange={(e) => onChangeType(e)}
            getOptionLabel={(option) => option.value}
            getOptionValue={(option) => option["key"].toString()}
          ></Select>
        </div> */}

        {/* {dataType === 2 && (
          <div className="col-3">
            <Select
              options={[
                { value: "Single Level", key: 1 },
                { value: "Muli Level", key: 2 },
              ]}
              value={[
                { value: "Single Level", key: 1 },
                { value: "Muli Level", key: 2 },
              ].find((x) => x.key === disaggregatedType)}
              onChange={(e) => onChangeDisaggregatedType(e)}
              defaultValue={{ value: "Single Level", key: 2 }}
              getOptionLabel={(option) => option.value}
              getOptionValue={(option) => option["key"].toString()}
            ></Select>
          </div>
        )} */}

        <div className="col-3">
          <div className="d-flex">
            {versionResources?.length > 0 && (
              <DropdownInputComponent
                inputCSS="labelForm voda-bold mb-0"
                isSearchable={true}
                isAdd={tipologicaPermesso ? true : false}
                onAddClicked={() => setIsVisibleModalLookup(1)}
                isClearable={true}
                value={
                  (versionResources &&
                    versionResources?.filter(
                      (x) => x.key == selectedVersion?.key
                    )) ??
                  null
                }
                options={versionResources}
                onChange={(e: any) => {
                  setSelectedVersion(e);
                  onChangeDescriptionList(e);
                }}
              />
            )}
            {/* {versionsList && (
              <div className="w-100">
                <Select
                  options={dictionaryToArray(versionsList!)}
                  onChange={(e) => onChangeDescriptionList(e)}
                  defaultValue={dictionaryToArray(versionsList!)[0]}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
              </div>
            )} */}

            {/* {tipologicaPermesso && (
              <button className="btn btn-link" type="button">
                {darkMode ? (
                  <GoPlus
                    onClick={() => setIsVisibleModalLookup(1)}
                    size={30}
                    color={"white"}
                  />
                ) : (
                  <img
                    style={{ height: 20 }}
                    src={require("../img/plus_icon.png")}
                    onClick={() => setIsVisibleModalLookup(1)}
                    alt="+"
                  />
                )}
              </button>
            )} */}
          </div>
        </div>
      </div>
      <Tabs
        defaultActiveKey="hardware"
        id="report"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="hardware" title="Hardware">
          {keyTabs === "hardware" && versionResources?.length > 0 && (
            <>
              <Hardware
                data={dataHardware}
                isHistorical={isHistoricalFlag}
                pagination={queryHardware}
                renderGrid={renderGridStateHw?.render ?? []}
                action={{
                  Filter: setQueryHardware,
                }}
              ></Hardware>
              <Paginate
                pagination={{
                  page: queryHardware.page,
                  pageSize: queryHardware.pageSize,
                }}
                totalItems={GridDtoHardware?.totalItems}
                actions={{ next: nextHardware, back: backHardware }}
              />
            </>
          )}
        </Tab>
        <Tab eventKey="software" title="Software">
          {keyTabs === "software" && versionResources?.length > 0 && (
            <>
              <Software
                data={dataSoftware}
                isHistorical={isHistoricalFlag}
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
          )}
        </Tab>
      </Tabs>
    </div>
  );
};

export default GenerateLcmDb;
