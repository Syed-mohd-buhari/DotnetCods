import React, { useEffect, useState, useRef } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { Alert, Dropdown, Modal } from "react-bootstrap";
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
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GetVnfInfoAndCapacityDetails } from "../Redux/Action/VBOMInfo/VBOMInfoGridAction";
import {
  VBOMClusterInfoDtoGrid,
  VBOMClusterInfoQueryObjectGrid,
  VBOMInfoDtoCreate,
  VBOMInfoDtoGrid,
  VBOMInfoDtoUpdate,
  VBOMInfoQueryObjectGrid,
  VBOMVnfInfoAndCapacityDtoGrid,
  VbomCapacityQueryDto,
} from "../Model/VBOMInfo";
import { GetVBOMInfoReport } from "../Redux/Action/VBOMInfo/VBOMInfoDownloadAction";
import VBOMInfoGrid from "../screen/VBOMInfo/VBOMInfoGrid";
import {
  DeleteVBOMInfo,
  VBOMInfoDelete,
} from "../Redux/Action/VBOMInfo/VBOMInfoDeleteAction";
import VBOMInfoModal from "../screen/VBOMInfo/VBOMInfoModal";
import { GetVBOMInfoCreateResource } from "../Redux/Action/VBOMInfo/VBOMInfoCreateAction";
import { GetVBOMInfoEditResource } from "../Redux/Action/VBOMInfo/VBOMInfoEditAction";
import VBOMInfoModal2 from "../screen/VBOMInfo/VBOMInfoModal2";
import { handleImportFile, safeNumber } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import {
  Box,
  Button,
  DialogActions,
  Divider,
  Grid,
  Paper,
  Typography,
  styled,
} from "@mui/material";
import VBOMInstanceGrid from "../screen/VBOMInfo/VBOMInstanceGrid";
import VBOMCapacityGrid from "../screen/VBOMInfo/VBOMCapacityGrid";
import VBOMClusterInfoGrid from "../screen/VBOMInfo/VBOMClusterInfoGrid";
import VBOMClusterInstanceGrid from "../screen/VBOMInfo/VBOMClusterInstanceGrid";
import VBOMClusterCapacityGrid from "../screen/VBOMInfo/VBOMClusterCapacityGrid";
import VBOMClusterInfoModal from "../screen/VBOMInfo/VBOMClusterInfoModal";
import {
  GetCBOMClusterInfoGrid,
  GetCnfInstanceAndCapacityDetails,
  GetFilterColumnCBOMClusterInfo,
} from "../Redux/Action/CBOM/CBOMGridAction";
import {
  CBOMCnfInstanceAndCapacityDtoGrid,
  CBOMDtoCreate,
  CBOMDtoGrid,
  CBOMDtoUpdate,
  CBOMInstanceDtoGrid,
  CBOMQueryObjectGrid,
} from "../Model/CBOM";
import CBOMClusterInfoGrid from "../screen/CBOM/CBOMClusterInfoGrid";
import { CBOMDelete, DeleteCBOM } from "../Redux/Action/CBOM/CBOMDeleteAction";
import { GetCBOMReport } from "../Redux/Action/CBOM/CBOMDownloadAction";
import CBOMClusterCapacityGrid from "../screen/CBOM/CBOMClusterCapacityGrid";
import CBOMClusterInstanceGrid from "../screen/CBOM/CBOMClusterInstanceGrid";
import { DropdownInputComponent } from "../Components/FormField";
import { resourceArrayRefactor } from "../Hook/Dictionary";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";
import { GetCBOMCreateResource } from "../Redux/Action/CBOM/CBOMCreateAction";
import { GetCBOMEditResource } from "../Redux/Action/CBOM/CBOMEditAction";
import CBOMClusterInfoModal from "../screen/CBOM/CBOMClusterInfoModal";

export let paginationQuery: CBOMQueryObjectGrid = {
  cnfCapacityId: [],
  cnfPodInfoId: [],
  financialYear: [],
  financialVersion: [],
  noOfCnfInstancesPersite: [],
  numberOfPodsPerPodType: [],
  vcpuRequestForPodType: [],
  memRequestForPodType: [],
  nonPresistentStorageForProdType: [],
  isPresistentVolumesRequired: [],
  persistentVolumNeaccessMode: [],
  persistentStorageForPodType: [],
  storageIopsForPodType: [],
  storagerWorkloadDistribution: [],
  northSouthBandWidthForPodType: [],
  eastWestBandWidthForPodType: [],
  specialRequirementPerPodType: [],
  listOfCapacitySpecialRequirement: [],
  cnfClusterInfoId: [],
  cnfClusterName: [],
  podTypeInfoName: [],
  opcoName: [],
  shortLocation: [],
  locationName: [],
  functionStandardName: [],
  priorityId: [],
  podRoleDescription: [],
  daemonSetPod: [],
  intraPodRules: [],
  interPodRules: [],
  isEnhancedHa: [],
  podTypeQos: [],
  isPersistanceStorageFlag: [],
  isProdhPaEnable: [],
  cnfNameDescription: [],
  nodePoolBreakUp: [],
  specialRequirements: [],
  hyperThreading: [],
  overProvisioning: [],
  workerNodeConfiguration: [],
  hardware: [],
  cpuKubelet: [],
  memKubelet: [],
  cpuSystem: [],
  memSystem: [],
  verticalDomain: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

let instancePaginationQuery = {
  cnfClusterInfoId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

interface CBOMClusterInfoModalRef {
  onSaveFormData: () => void;
}

const Item = styled(Paper)(({ theme }) => ({
  ...theme.typography.body2,
  textAlign: "center",
  backgroundColor: "#cccccc00",
  color: theme.palette.text.secondary,
  height: 45,
  lineHeight: "41px",
  marginBottom: "1rem",
  fontFamily: "VodafoneRg",
  fontWeight: "bold",
  fontSize: "15px",
}));

const CBOMClusterInfoContainer = () => {
  //STATE CONFIRM
  const vBomRef = useRef<CBOMClusterInfoModalRef>(null);
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isViewVisibleModal, setIsViewVisibleModal] = useState(false);
  const [isAddEnable, setIsAddEnable] = useState(false);
  const [isViewEnable, setIsViewEnable] = useState(false);
  const [viewUserInfo, setViewUserInfo] = useState<any>();
  const [infoId, setInfoId] = useState<number | null>(null);
  const [instanceId, setInstanceId] = useState<number | null>(null);
  const [capacityId, setCapacityId] = useState<number | null>(null);
  const [orphanColor, setOrphanColor] = useState(false);
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [statusInfo, setStatusInfo] = useState<string>("");
  const [editApiFrom, setEditApiFrom] = useState<string>("");
  const [opcoDropdown, setOpcoDropdown] = useState<any>([]);
  const [siteDropdown, setSiteDropdown] = useState<any>([]);
  const [opcoValue, setOpcoValue] = useState<any>(null);
  const [siteValue, setSiteValue] = useState<any>(null);
  const [downloadModalFlag, setDownloadModalFlag] = useState<boolean>(false);
  //DTO
  const [data, setData] = useState<CBOMDtoGrid[] | undefined>([]);
  const [instanceData, setInstanceData] = useState<any>(null);
  const [capacityData, setCapacityData] = useState<any[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) =>
      state.CBOMClusterInfoGridReducer.CBOMClusterInfoGridResult
  );
  let GridInstanceDto = useSelector(
    (state: RootState) =>
      state.CBOMClusterInstanceCapacityGridReducer
        .CBOMCnfInstanceAndCapacityGridResult
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  const [renderGridState, setRenderGridState] = useState<any>();
  const [renderInstanceGridState, setRenderInstanceGridState] = useState<any>();
  const [renderCapacityGridState, setRenderCapacityGridState] = useState<any>();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const { darkMode } = useTheme();

  // useEffect(() => {
  //   // Update paginationQuery with the pageSize from useAuth whenever it changes
  //   paginationQuery.pageSize = pageSize;
  // }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    paginationQuery,
    undefined
  );
  const {
    query: instanceQuery,
    setQuery: setInstanceQuery,
    next: instanceNext,
    back: instanceBack,
    updatePageSize: instanceUpdatePageSize,
  } = useResourceTableCrud(
    {
      ...instancePaginationQuery,
      cnfClusterInfoId: infoId !== null ? [infoId] : [],
    },
    undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    closeModal();
    GetCBOMClusterInfoGrid(query);
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
  } = useOperationTableCrud<CBOMDtoUpdate, CBOMDtoCreate>(
    GetCBOMCreateResource,
    GetCBOMEditResource,
    DeleteCBOM,
    refresh
  );

  const [deleteConfirm, setDeleteConfirm] =
    useState<DataModalConfirm>(stateConfirm);

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };
  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      setInfoId(GridDto?.items?.[0]?.cnfClusterInfoId ?? null);
      const clusterInfoId = GridDto?.items?.[0]?.cnfClusterInfoId ?? null;
      if (clusterInfoId !== null && isPermesso) {
        setInstanceQuery({
          ...instancePaginationQuery,
          cnfClusterInfoId: [clusterInfoId],
        });
        // infoCapacityApi({ cnfClusterInfoId: [clusterInfoId] });
      } else {
        setInstanceData(null);
      }
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetCBOMClusterInfoGrid");
    }
  }, [GridDto]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridInstanceDto !== undefined || GridInstanceDto !== null) {
      setInstanceData(GridInstanceDto?.items);
      setInstanceId(GridInstanceDto?.items?.[0]?.cnfPodInfoId ?? null);
      setCapacityId(
        GridInstanceDto?.items?.[0]?.["_cnfCapacityDtoGrid"]?.[0]
          ?.cnfCapacityId ?? null
      );
      let copy = { ...GridInstanceDto?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderInstanceGridState(copy);
    }
  }, [GridInstanceDto]);

  useEffect(() => {
    if (isPermesso) {
      callFilterApi();
    }
  }, [isPermesso]);

  const callFilterApi = async () => {
    setLoader("ADD", "GetFilterOpcoColumnCBOM");
    const opcoDropdown = await GetFilterColumnCBOMClusterInfo(
      "opcoName",
      "",
      query
    );
    let siteDropdown = null as any;

    if (opcoDropdown && opcoDropdown.filter && opcoDropdown.filter.length > 0) {
      setLoader("REMOVE", "GetFilterOpcoColumnCBOM");
      setLoader("ADD", "GetFilterSiteColumnCBOM");
      const firstKey =
        opcoDropdown?.filter
          ?.sort((a, b) => b.text.localeCompare(a.text))
          .map((res) => ({
            key: res.value,
            text: res.text,
          }))[0]?.key ?? null;

      siteDropdown = await GetFilterColumnCBOMClusterInfo("site", "", {
        ...query,
        opcoName: firstKey !== null ? [firstKey] : [],
      });
      setLoader("ADD", "GetFilterSiteColumnCBOM");
    }
    setLoader("REMOVE", "GetFilterOpcoColumnCBOM");
    const opcoRes = opcoDropdown?.filter
      ?.sort((a, b) => b.text.localeCompare(a.text))
      ?.map((res) => {
        return { key: res.value, text: res.text };
      });
    const siteRes = siteDropdown?.filter?.map((res) => {
      return { key: res.value, text: res.text };
    });
    let copy = { ...query } as any;
    copy.opcoName = opcoRes !== null ? [opcoRes?.[0]?.key] : copy.opcoName;
    copy.site = siteRes !== null ? [siteRes?.[0]?.key] : copy.site;
    setQuery(copy);
    setOpcoDropdown(opcoRes ?? null);
    setSiteDropdown(siteRes ?? null);
    setOpcoValue(opcoRes?.[0] ?? null);
    setSiteValue(siteRes?.[0] ?? null);
  };

  const getSiteFilter = async (e) => {
    const siteDropdown = await GetFilterColumnCBOMClusterInfo("site", "", {
      ...query,
      opcoName: e !== null ? [e?.key] : [],
      site: [],
    } as any);
    const siteRes = siteDropdown?.filter?.map((res) => {
      return { key: res.value, text: res.text };
    });
    setQuery({
      ...query,
      opcoName: e !== null ? [e?.key] : [],
      site: e !== null && siteRes !== null ? [siteRes?.[0]?.key] : [],
    } as any);
    setSiteDropdown(siteRes ?? null);
    if (e !== null) {
      setSiteValue(siteRes?.[0] ?? null);
    }
  };

  useEffect(() => {
    if (query && isPermesso) {
      setLoader("ADD", "GetCBOMClusterInfoGrid");
      GetCBOMClusterInfoGrid(query).then((x) =>
        setLoader("REMOVE", "GetCBOMClusterInfoGrid")
      );
      setLoader("REMOVE", "GetCBOMClusterInfoGrid");
      setSiteValue(
        query?.["site"]?.length > 0
          ? query?.["site"]?.map((val) => {
              return { key: val, value: val };
            })[0]
          : null
      );
      setOpcoValue(
        query?.["opcoName"]?.length > 0
          ? query?.["opcoName"]?.map((val) => {
              return {
                key: val,
                value: opcoDropdown?.filter((res) => res.key === val)[0]?.text,
              };
            })[0]
          : null
      );
    }
  }, [query]);

  useEffect(() => {
    if (instanceQuery && isPermesso) {
      GetCnfInstanceAndCapacityDetails(instanceQuery);
    }
  }, [instanceQuery]);

  const onChangeInfoDetails = async (id) => {
    setInfoId(id);
    const result = data
      ? data.filter((res) => id === res.cnfClusterInfoId)[0]
      : null;
    const updatedPayload = {
      ...instancePaginationQuery,
      cnfClusterInfoId: result?.cnfClusterInfoId
        ? [result.cnfClusterInfoId]
        : [],
    };
    if (id !== infoId) {
      setInstanceQuery(updatedPayload);
    }
  };

  const infoCapacityApi = async (updatedPayload) => {
    const response: any = await GetCnfInstanceAndCapacityDetails(
      updatedPayload
    );
  };

  const onChangeInstanceDetails = (id) => {
    setInstanceId(id);
    setCapacityId(
      GridInstanceDto?.items?.filter((res) => res.cnfPodInfoId === id)?.[0]?.[
        "_cnfCapacityDtoGrid"
      ]?.[0]?.cnfCapacityId ?? null
    );
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetCBOMClusterInfoGrid");
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
        let copy = { ...query } as CBOMQueryObjectGrid;
        copy.principalId = localState?.id;
        // setQuery(copy);
        // GetCBOMClusterInfoGrid(copy).then((x) =>
        //   setLoader("REMOVE", "GetCBOMClusterInfoGrid")
        // );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetCBOMClusterInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetCBOMClusterInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetCBOMReport({
      ...query,
      ...instanceQuery,
    });
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
    setDownloadModalFlag(false);
  };

  const fileHandle = async () => {
    try {
      const sheetName = "CBOM VNF Info";
      const apiPath = "CBOMInfo";
      const status = await handleImportFile(sheetName, apiPath);
      if (status && status?.["warning"] === true) {
        rootStore.dispatch(
          setNotification({
            message: status?.["info"],
            notifyType: NotifyType.success,
          })
        );
        refresh();
        setShow(false);
      } else if (status && status?.["warning"] === false) {
        setStatusInfo(status?.["info"] || "");
        setAlertStatus({
          message: status?.["data"],
          class: "danger",
        });
        setShow(true);
        setLoader("ADD", "GetCBOMClusterInfoGrid");
        GetCBOMClusterInfoGrid(query).then(() =>
          setLoader("REMOVE", "GetCBOMClusterInfoGrid")
        );
      }
    } catch (error) {
      console.log(error);
    }
  };

  const DeleteModal = (type: string, id: number, alowDelete: boolean) => {
    // Map for type values and their parent
    const typeHierarchy: Record<string, { label: string; parent?: string }> = {
      infoId: { label: "Info" },
      instanceId: { label: "Instance", parent: "Info" },
      capacityId: { label: "Capacity", parent: "Instance" },
    };

    const current = typeHierarchy[type];
    const typeValue = current?.label || "";
    const parentTypeValue = current?.parent || "";

    // setDeleteConfirm({
    //   title: "Delete Entry",
    //   message: !alowDelete
    //     ? `Unable to delete this ${typeValue}, as each ${parentTypeValue} must have at least one associated ${typeValue}.`
    //     : "Do you want to delete this item?",
    //   button: "Delete",
    //   item: id,
    //   isOpen: true,
    //   actions: {
    //     cancel: () => setDeleteConfirm(deleteConfirm),
    //     ...(alowDelete && {
    //       confirm: () => handleConfirmDelete(type, id),
    //     }),
    //   },
    // });
    setDeleteConfirm({
      title: "Delete Entry",
      message: "Do you want to delete this item?",
      button: "Delete",
      item: id,
      isOpen: true,
      actions: {
        cancel: () => setDeleteConfirm(deleteConfirm),
        confirm: () => handleConfirmDelete(type, id),
      },
    });
  };

  const handleConfirmDelete = async (type: string, id: number) => {
    const res: any = await CBOMDelete(id, type);
    if (res) {
      setDeleteConfirm(deleteConfirm);
      refresh();
    }
  };

  const handleSubmit = () => {
    vBomRef.current?.onSaveFormData();
  };
  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      <ModalConfirm data={deleteConfirm} />
      <ModalConfirm data={myConfirm} />

      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <Dialog
        open={isVisibleModal}
        onClose={(event, reason) => {
          if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
            closeModal(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="xl"
        scroll="paper"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle id="scroll-dialog-title">
          {edit === true ? "Edit CBOM Info" : "Add CBOM Info"}
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
        <DialogContent dividers>
          <CBOMClusterInfoModal
            ref={vBomRef}
            edit={edit}
            instanceKey={
              editApiFrom === "instance" || editApiFrom === "capacity"
                ? instanceId
                : null
            }
            capacityKey={editApiFrom === "capacity" ? capacityId : null}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
        <DialogActions
          sx={{
            marginRight: "1rem",
            padding: "1rem",
            // justifyContent: "space-between",
          }}
        >
          <Button
            variant="outlined"
            color="inherit"
            onClick={() => closeModal(false)}
            style={{ borderRadius: "20px" }}
          >
            Close
          </Button>
          <Button
            variant="contained"
            color="error"
            onClick={handleSubmit}
            style={{ borderRadius: "20px" }}
          >
            Submit
          </Button>
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
          <h3 className="voda-bold">CBOM Info</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn"
              onClick={New}
              type="button"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">Add New Info</span>
            </button>
          )}
          {tipologicaPermesso && (
            <button
              className="download-to-excel ml-3 grid-main-btn"
              // style={{ backgroundColor: "#e9ecef", cursor: "not-allowed" }}
              onClick={() => fileHandle()}
              // disabled={true}
            >
              Import Excel
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn
            "
            onClick={() => setDownloadModalFlag(true)}
          >
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
            </Dropdown.Menu>
          </Dropdown>
        </div>
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
            renderGrid={renderGridState}
            action={{ closeModalSetup }}
          ></SetupColumns>
        </Modal.Body>
      </Modal>
      <Modal
        show={show}
        onHide={() => setShow(false)}
        backdrop="static"
        size="lg"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title>
            {statusInfo || "Import completed with some issues."}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body style={{ maxHeight: "60vh", overflowY: "auto" }}>
          {Array.isArray(alerStatus.message) ? (
            <>
              {alerStatus.message
                .filter((item) => item.table === "Common")
                .map((item, index) => (
                  <div key={index} className="mb-3">
                    <h6 className="mb-1">{item.table}</h6>
                    <ul className="pl-3">
                      {item.values.map((msg, idx) => (
                        <li key={idx} style={{ fontSize: "16px" }}>
                          {msg}
                        </li>
                      ))}
                    </ul>
                  </div>
                ))}
            </>
          ) : (
            <p>{alerStatus.message}</p>
          )}
        </Modal.Body>
      </Modal>

      <Dialog
        open={downloadModalFlag}
        onClose={(event, reason) => {
          if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
            setDownloadModalFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="paper"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle id="scroll-dialog-title">Download to Excel</DialogTitle>

        <IconButton
          aria-label="close"
          onClick={() => setDownloadModalFlag(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent dividers>
          <div className="row ">
            <div className="col-3">
              <div className="col-12 p-0">
                <DropdownInputComponent
                  label={"Select Opco"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  value={
                    opcoDropdown &&
                    resourceArrayRefactor(opcoDropdown).filter(
                      (x) => x?.key === opcoValue?.key
                    )
                  }
                  options={opcoDropdown && resourceArrayRefactor(opcoDropdown)}
                  onChange={(e: any) => {
                    setOpcoValue(e);
                    getSiteFilter(e);
                  }}
                />
              </div>
            </div>
            <div className="col-3">
              <div className="col-12 p-0">
                <DropdownInputComponent
                  label={"Select Site"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  value={
                    siteDropdown &&
                    resourceArrayRefactor(siteDropdown).filter(
                      (x) => x?.key === siteValue?.key
                    )
                  }
                  options={siteDropdown && resourceArrayRefactor(siteDropdown)}
                  onChange={(e: any) => {
                    setSiteValue(e);
                    setQuery({
                      ...query,
                      site: e !== null ? [e?.key] : [],
                    } as any);
                  }}
                />
              </div>
            </div>
          </div>
        </DialogContent>
        <DialogActions
          sx={{
            marginRight: "1rem",
            padding: "1rem",
            // justifyContent: "space-between",
          }}
        >
          <Button
            variant="outlined"
            color="inherit"
            onClick={() => setDownloadModalFlag(false)}
            style={{ borderRadius: "20px" }}
          >
            Close
          </Button>
          <Button
            variant="contained"
            color="error"
            onClick={InvocheDownload}
            disabled={opcoValue === null || siteValue === null}
            style={{ borderRadius: "20px" }}
          >
            Download
          </Button>
        </DialogActions>
      </Dialog>
      <Box sx={{ flexGrow: 1 }}>
        <Grid container spacing={4} sx={{ alignItems: "flex-start" }}>
          <Grid size={{ xs: 4 }}>
            <Grid container sx={{ direction: "column" }} spacing={4}>
              <Grid sx={{ width: "inherit", paddingRight: 4 }}>
                <Paper elevation={6}>
                  <Item
                    key={"Filter Details"}
                    sx={{
                      lineHeight: "24px !important",
                      height: "100% !important",
                      alignContent: "center",
                    }}
                  >
                    <div className="row mx-1 mt-2">
                      <div className="col-6">
                        <div className="col-12 p-0">
                          <DropdownInputComponent
                            label={"Select Opco"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              opcoDropdown &&
                              resourceArrayRefactor(opcoDropdown).filter(
                                (x) => x?.key === opcoValue?.key
                              )
                            }
                            options={
                              opcoDropdown &&
                              resourceArrayRefactor(opcoDropdown)
                            }
                            onChange={(e: any) => {
                              setOpcoValue(e);
                              getSiteFilter(e);
                            }}
                          />
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 p-0">
                          <DropdownInputComponent
                            label={"Select Site"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              siteDropdown &&
                              resourceArrayRefactor(siteDropdown).filter(
                                (x) => x?.key === siteValue?.key
                              )
                            }
                            options={
                              siteDropdown &&
                              resourceArrayRefactor(siteDropdown)
                            }
                            onChange={(e: any) => {
                              setSiteValue(e);
                              setQuery({
                                ...query,
                                site: e !== null ? [e?.key] : [],
                              } as any);
                            }}
                          />
                        </div>
                      </div>
                    </div>
                  </Item>
                </Paper>
              </Grid>
              <Grid
                sx={{
                  width: "inherit",
                  paddingRight: 4,
                  paddingTop: "16px !important",
                }}
              >
                <Paper
                  sx={{
                    height: "49.5rem",
                    paddingBottom: "1.5rem",
                  }}
                  elevation={6}
                >
                  <Item
                    key={"Info Details"}
                    sx={{
                      alignContent: "center",
                      marginBottom: "0rem !important",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: "bold",
                        marginBottom: "0px",
                        marginLeft: "1rem",
                        textAlign: "left",
                      }}
                    >
                      CNF Cluster Info
                    </Typography>
                  </Item>
                  {/* <Divider /> */}
                  <div className="col-12" style={{ padding: "15px" }}>
                    {data ? (
                      <>
                        <CBOMClusterInfoGrid
                          data={data}
                          pagination={query}
                          orphanColor={orphanColor}
                          renderGrid={renderGridState?.render ?? []}
                          action={{
                            Delete: DeleteModal,
                            Edit: (data: any) => {
                              setEditApiFrom("info");
                              Edit(data);
                            },
                            Filter: setQuery,
                            onInfoIdChange: (id: any) =>
                              onChangeInfoDetails(id),
                          }}
                          infoId={infoId}
                        ></CBOMClusterInfoGrid>
                        <MUIPaginationComponent
                          pagination={{
                            page: query.page,
                            pageSize: query.pageSize,
                          }}
                          totalItems={GridDto?.totalItems}
                          actions={{ next, back, updatePageSize }}
                        />
                      </>
                    ) : (
                      <Typography
                        variant="h6"
                        sx={{
                          marginBottom: "0px",
                          textAlign: "left",
                          paddingLeft: "1rem",
                          height: "18rem",
                          justifySelf: "center",
                          alignContent: "center",
                          fontWeight: 200,
                        }}
                      >
                        No Data Found
                      </Typography>
                    )}
                  </div>
                </Paper>
              </Grid>
            </Grid>
          </Grid>

          <Grid size={{ xs: 8 }}>
            <Grid container sx={{ direction: "column" }} spacing={4}>
              <Grid sx={{ width: "inherit", paddingRight: 4 }}>
                <Paper elevation={6}>
                  <Item
                    key={"Instance Details"}
                    sx={{
                      alignContent: "center",
                      marginBottom: "0rem !important",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: "bold",
                        marginBottom: "0px",
                        textAlign: "left",
                        paddingLeft: "1rem",
                      }}
                    >
                      Instance Details
                    </Typography>
                  </Item>
                  <div className="col-12" style={{ padding: "15px" }}>
                    {instanceData ? (
                      <>
                        <CBOMClusterInstanceGrid
                          data={instanceData ?? []}
                          pagination={{
                            ...instanceQuery,
                            cnfClusterInfoId: infoId !== null ? [infoId] : [],
                          }}
                          orphanColor={orphanColor}
                          renderGrid={renderInstanceGridState?.render ?? []}
                          action={{
                            Delete: DeleteModal,
                            Edit: (data: any) => {
                              setEditApiFrom("instance");
                              Edit(data);
                            },
                            Filter: setInstanceQuery,
                            onInstanceIdChange: (id: any) =>
                              onChangeInstanceDetails(id),
                          }}
                          infoId={infoId}
                          instanceId={instanceId}
                        ></CBOMClusterInstanceGrid>
                        <MUIPaginationComponent
                          pagination={{
                            page: instanceQuery.page,
                            pageSize: instanceQuery.pageSize,
                          }}
                          totalItems={GridInstanceDto?.totalItems}
                          actions={{
                            next: instanceNext,
                            back: instanceBack,
                            updatePageSize: instanceUpdatePageSize,
                          }}
                        />
                      </>
                    ) : (
                      <Typography
                        variant="h6"
                        sx={{
                          marginBottom: "0px",
                          textAlign: "left",
                          paddingLeft: "1rem",
                          height: "18rem",
                          justifySelf: "center",
                          alignContent: "center",
                          fontWeight: 200,
                        }}
                      >
                        No Data Found
                      </Typography>
                    )}
                  </div>
                </Paper>
              </Grid>
              <Grid sx={{ width: "inherit", paddingRight: 4 }}>
                <Paper elevation={6}>
                  <Item
                    key={"Capacity Details"}
                    sx={{
                      alignContent: "center",
                      marginBottom: "0rem !important",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: "bold",
                        marginBottom: "0px",
                        textAlign: "left",
                        paddingLeft: "1rem",
                      }}
                    >
                      Capacity Details
                    </Typography>
                  </Item>
                  <div className="col-12" style={{ padding: "15px" }}>
                    {instanceData ? (
                      <>
                        <CBOMClusterCapacityGrid
                          data={instanceData}
                          pagination={{
                            ...instanceQuery,
                            cnfPodInfoId:
                              instanceId !== null ? [instanceId] : [],
                            cnfClusterInfoId: [],
                          }}
                          orphanColor={orphanColor}
                          renderGrid={renderInstanceGridState?.render ?? []}
                          action={{
                            Delete: DeleteModal,
                            Edit: (data: any) => {
                              setEditApiFrom("capacity");
                              Edit(data);
                            },
                            Filter: setInstanceQuery,
                            onInstanceIdChange: (id: any) => setInstanceId(id),
                            onCapacityIdChange: (id: any) => setCapacityId(id),
                          }}
                          infoId={infoId}
                          instanceId={instanceId}
                          capacityId={capacityId}
                        ></CBOMClusterCapacityGrid>
                        {/* <MUIPaginationComponent
                          pagination={{
                            page: instanceQuery.page,
                            pageSize: instanceQuery.pageSize,
                          }}
                          totalItems={GridInstanceDto?.totalItems}
                          actions={{
                            next: instanceNext,
                            back: instanceBack,
                            updatePageSize: instanceUpdatePageSize,
                          }}
                        /> */}
                      </>
                    ) : (
                      <Typography
                        variant="h6"
                        sx={{
                          marginBottom: "0px",
                          textAlign: "left",
                          paddingLeft: "1rem",
                          height: "18rem",
                          justifySelf: "center",
                          alignContent: "center",
                          fontWeight: 200,
                        }}
                      >
                        No Data Found
                      </Typography>
                    )}
                  </div>
                </Paper>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default CBOMClusterInfoContainer;
