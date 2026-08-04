import React, { useCallback, useEffect, useState } from "react";
import { Tabs, Tab, Modal, Dropdown, Form, Alert } from "react-bootstrap";
import Select from "react-select";

import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useDispatch, useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import AssetAsisSdGrid from "../screen/AssetAsis/AssetAsisSdGrid";
import AssetAsisSdSwitchGrid from "../screen/AssetAsisSdSwitch/AssetAsisSdSwitchGrid";
import AssetAsisHwAncillaryGrid from "../screen/AssetAsisHwAncillary/AssetAsisHwAncillaryGrid";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";

import {
  AssetAsisSdQueryObjectGrid,
  AssetAsisSdDtoGrid,
} from "../Model/Report/AssetAsisSdExport";
import { AssetAsisSdSwitchQueryObjectGrid } from "../Model/Report/AssetAsisSdSwitchExport";
import { AssetAsisHwAncillaryQueryObjectGrid } from "../Model/Report/AssetAsisHwAncillaryExport";
import { GetAssetAsisSdGrid } from "../Redux/Action/Report/AssetAsisSdGridAction";
import { GetAssetAsisSdSwitchGrid } from "../Redux/Action/Report/AssetAsisSdSwitchGridAction";
import { GetAssetAsisHwAncillaryGrid } from "../Redux/Action/Report/AssetAsisHwAncillaryGridAction";
import { DownloadAssetAsisSd } from "../Redux/Action/Report/AssetAsisSdDownloadAction";
import { DownloadAssetAsisSdSwitch } from "../Redux/Action/Report/AssetAsisSdSwitchDownloadAction";
import { DownloadAssetAsisHwAncillary } from "../Redux/Action/Report/AssetAsisHwAncillaryDownloadAction";

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
import { AssetAsisSdQueryAllDto } from "../Model/Report/AssetAsisSdExport";
import { useAuth } from "../Hook/useAuth";
import { handleImportFile } from "../Hook/Common";

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
import { rtnColorDuplicate } from "../Hook/Duplicates";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";

export let paginationQueryAssetAsisSd: AssetAsisSdQueryObjectGrid = {
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
  assetAsisSdinfoid: [],
  datasourcename: [],
  datasourcetype: [],
  swversion: [],
  firmwareversion: [],
  manufacturer: [],
  model: [],
  tsrmodel: [],
};

export let paginationQueryAssetAsisSdSwitch: AssetAsisSdSwitchQueryObjectGrid =
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
    assetasissdiswitchinfoid: [],
    datasourcename: [],
    switchname: [],
    switchid: [],
    switchadminstate: [],
    switchuniqueid: [],
    switchrole: [],
    switchrack: [],
    switchlabel: [],
    switchserialnumber: [],
    switchopsstate: [],
    switchnetwork: [],
    switchmanufacturer: [],
    switchmodel: [],
    switchipaddress: [],
    switchswversion: [],
  };

export let paginationQueryAssetAsisHwAncillaryData: AssetAsisHwAncillaryQueryObjectGrid =
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
    assetasishwancillarydataid: [],
    networkelementasisid: [],
    site: [],

    datasourcename: [],
    host: [],
    provider: [],
    providertype: [],
    consumer: [],
    consumertype: [],
    consumerrole: [],
    clustername: [],
    partnumber: [],
    systemtype: [],
    manufacturer: [],
    biosversion: [],
    model: [],
    sku: [],

    cpucapacity: [],
    ephemeralstoragecapacity: [],
    memorycapacity: [],
    processorsummarymodel: [],
    kubernetesnodetype: [],
    managementip: [],
    kubernetesnodename: [],
    kubeletversion: [],
    kubernetesnodeos: [],
    kubernetesnoderesourcetype: [],
    kubernetesnodestate: [],
    kubernetesnodestatusupdatetime: undefined,
    chassisdetails: [],
  };

const OMC: React.FC = () => {
  const [keyTabs, setKeyTabs] = useState("assetasis-hwancillary");
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const { readonly, isPermesso, tipologicaPermesso } = useAuth();

  const [dataAssetAsisSd, setAssetAsisSd] = useState<
    AssetAsisSdDtoGrid[] | undefined
  >([]);

  const Grid = (state: RootState) =>
    state.assetAsisSdGridReducer.AssetAsisSdGridResult;
  const GridDto = useSelector(Grid);

  const GridHardware = (state: RootState) =>
    state.assetAsisSdSwitchGridReducer.AssetAsisSdSwitchGridResult;
  const GridDtoHardware = useSelector(GridHardware);

  const GridSoftware = (state: RootState) =>
    state.assetAsisHwAncillaryGridReducer.AssetAsisHwAncillaryGridResult;
  const GridDtoSoftware = useSelector(GridSoftware);

  const [downloadTab, setDownloadTab] = useState<string>("assetasis-sd");
  const [downloadAll, setDownloadAll] = useState<boolean>(false);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    {
      ...paginationQueryAssetAsisSd,
    },
    undefined
  );

  const {
    query: queryHardware,
    setQuery: setQueryHardware,
    next: nextHardware,
    back: backHardware,
    updatePageSize: updatePageSizeHardware,
  } = useResourceTableCrud({ ...paginationQueryAssetAsisSdSwitch }, undefined);

  const {
    query: querySoftware,
    setQuery: setQuerySoftware,
    next: nextSoftware,
    back: backSoftware,
    updatePageSize: updatePageSizeSoftware,
  } = useResourceTableCrud(
    { ...paginationQueryAssetAsisHwAncillaryData },
    undefined
  );

  // Direct API calls without selectedVersion
  useEffect(() => {
    if (isPermesso) {
      GetAssetAsisSdGrid(query);
    }
  }, [query, setQuery, next, back, isPermesso]);

  useEffect(() => {
    if (isPermesso) {
      GetAssetAsisSdSwitchGrid(queryHardware);
    }
  }, [queryHardware, setQueryHardware, nextHardware, backHardware, isPermesso]);

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
    if (isPermesso) {
      GetAssetAsisHwAncillaryGrid(querySoftware);
    }
  }, [querySoftware, setQuerySoftware, nextSoftware, backSoftware, isPermesso]);

  useEffect(() => {
    if (GridDtoSoftware != undefined && isPermesso) {
      setDataSoftware(GridDtoSoftware?.items);
      let copy = { ...GridDtoSoftware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSw(copy);
    }
  }, [GridDtoSoftware]);

  const [queryAll, setQueryAll] = useState<AssetAsisSdQueryAllDto>({});
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

  const InvocheDownload = async () => {
    setExcelPopUp(false);

    if (downloadAll) {
      const results = await Promise.all([
        DownloadAssetAsisSd(query),
        DownloadAssetAsisSdSwitch(queryHardware),
        DownloadAssetAsisHwAncillary(querySoftware),
      ]);

      results.forEach((result) => {
        if (result !== undefined) {
          let url = window.URL.createObjectURL(result.file);
          let a = document.createElement("a");
          a.href = url;
          a.download = result.fileName;
          a.click();
          setTimeout(() => {}, 100);
        }
      });
    } else {
      let result;

      switch (downloadTab) {
        case "assetasis-sdswitch":
          result = await DownloadAssetAsisSdSwitch(queryHardware);
          break;
        case "assetasis-hwancillary":
          result = await DownloadAssetAsisHwAncillary(querySoftware);
          break;
        case "assetasis-sd":
        default:
          result = await DownloadAssetAsisSd(query);
          break;
      }

      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
      }
    }

    setDownloadTab(keyTabs);
    setDownloadAll(false);
  };

  useEffect(() => {
    let copy = { ...queryAll } as AssetAsisSdQueryAllDto;
    if (query && isPermesso) {
      copy.query = query;
      copy.activeTab = keyTabs;
      setQueryAll(copy);
    }
  }, [query, keyTabs]);

  useEffect(() => {
    if (GridDto != undefined && isPermesso) {
      setAssetAsisSd(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  const closeModalSetup = (changed: boolean) => {
    // Refresh all grids
    Promise.all([
      GetAssetAsisSdGrid(query),
      GetAssetAsisSdSwitchGrid(queryHardware),
      GetAssetAsisHwAncillaryGrid(querySoftware),
    ]).then(() => setIsVisibleModalSetup(false));
  };

  return (
    <div className="pageContainer">
      <Dialog
        open={excelPopup === true}
        onClose={() => {
          setExcelPopUp(false);
          setDownloadTab(keyTabs);
          setDownloadAll(false);
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
            setDownloadTab(keyTabs);
            setDownloadAll(false);
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
                onClick={() => {
                  setDownloadAll(true);
                  setDownloadTab("");
                }}
                checked={downloadAll}
                label="Download All"
                name="downloadGroup"
                type="radio"
                id="radio-download-all"
              />
              <Form.Check
                onClick={() => {
                  setDownloadAll(false);
                  setDownloadTab("assetasis-hwancillary");
                }}
                checked={
                  !downloadAll && downloadTab === "assetasis-hwancillary"
                }
                label="CCD INFO"
                name="downloadGroup"
                type="radio"
                id="radio-assetasis-hwancillary"
                className="mt-2"
              />
              <Form.Check
                onClick={() => {
                  setDownloadAll(false);
                  setDownloadTab("assetasis-sdswitch");
                }}
                checked={!downloadAll && downloadTab === "assetasis-sdswitch"}
                label="CHASIS INFO"
                name="downloadGroup"
                type="radio"
                id="radio-assetasis-sdswitch"
                className="mt-2"
              />
              <Form.Check
                onClick={() => {
                  setDownloadAll(false);
                  setDownloadTab("assetasis-sd");
                }}
                checked={!downloadAll && downloadTab === "assetasis-sd"}
                label="SDI INFO"
                name="downloadGroup"
                type="radio"
                id="radio-assetasis-sd"
                className="mt-2"
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
                setDownloadTab(keyTabs);
                setDownloadAll(false);
              }}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => InvocheDownload()}
              disabled={!downloadAll && downloadTab === ""}
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
              keyTabs === "assetasis-sdswitch"
                ? renderGridStateHw
                : keyTabs === "assetasis-hwancillary"
                ? renderGridStateSw
                : renderGridState
            }
            action={{ closeModalSetup }}
          ></SetupColumns>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold d-flex">CNIS INFO</h3>

        <div className="d-flex">
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => {
              setExcelPopUp(true);
              setDownloadTab(keyTabs);
            }}
          >
            Download to Excel
          </button>

          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              {/* {!readonly && (
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
                      {keyTabs === "assetasis-sd" ? (
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
              )} */}

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

      <Tabs
        defaultActiveKey="assetasis-hwancillary"
        id="omc-tabs"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="assetasis-hwancillary" title="CCD INFO">
          {keyTabs === "assetasis-hwancillary" && (
            <>
              <AssetAsisHwAncillaryGrid
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
        <Tab eventKey="assetasis-sdswitch" title="CHASIS INFO">
          {keyTabs === "assetasis-sdswitch" && (
            <>
              <AssetAsisSdSwitchGrid
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

        <Tab eventKey="assetasis-sd" title="SDI INFO">
          {keyTabs === "assetasis-sd" && (
            <>
              <AssetAsisSdGrid
                data={dataAssetAsisSd}
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
    </div>
  );
};

export default OMC;
