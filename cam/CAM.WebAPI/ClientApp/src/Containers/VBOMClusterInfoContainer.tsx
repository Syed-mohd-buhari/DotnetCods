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
import {
  GetFilterColumnVBOMClusterInfo,
  GetFilterColumnVBOMInstanceCapacity,
  GetVBOMClusterInfoGrid,
  GetVnfInfoAndCapacityDetails,
} from "../Redux/Action/VBOMInfo/VBOMInfoGridAction";
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
import { DropdownInputComponent } from "../Components/FormField";
import { resourceArrayRefactor } from "../Hook/Dictionary";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";

export let paginationQuery: VBOMClusterInfoQueryObjectGrid = {
  vnfClusterInfoId: [],
  vnfInfoId: [],
  opCoId: [],
  opCoDescritpion: [],
  locationName: [],
  siteName: [],
  shortLocationId: [],
  clusterDescription: [],
  clusterId: [],
  vnfNameDescritpion: [],
  hardwareType: [],
  noOfBlades: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

export let instancePaginationQuery: VBOMClusterInfoQueryObjectGrid = {
  vnfClusterInfoId: [],
  vnfInfoId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

interface VBOMClusterInfoModalRef {
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

const VBOMClusterInfoContainer = () => {
  //STATE CONFIRM
  const vBomRef = useRef<VBOMClusterInfoModalRef>(null);
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
  const [hardwareDropdown, setHardwareDropdown] = useState<any>([]);
  const [vnfDropdown, setVnfDropdown] = useState<any>([]);
  const [opcoValue, setOpcoValue] = useState<any>(null);
  const [hardwareValue, setHardwareValue] = useState<any>(null);
  const [vnfValue, setVnfValue] = useState<any>(null);
  //DTO
  const [data, setData] = useState<VBOMClusterInfoDtoGrid[] | undefined>([]);
  const [instanceData, setInstanceData] = useState<any>(null);
  const [capacityData, setCapacityData] = useState<any[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) =>
      state.VBOMClusterInfoGridReducer.VBOMClusterInfoGridResult
  );
  let GridInstanceDto = useSelector(
    (state: RootState) =>
      state.VBOMClusterInstanceCapacityGridReducer
        .VBOMVnfInstanceAndCapacityGridResult
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  const [renderGridState, setRenderGridState] = useState<any>();
  const [downloadModalFlag, setDownloadModalFlag] = useState<boolean>(false);
  const [renderInstanceGridState, setRenderInstanceGridState] = useState<any>();
  const [renderCapacityGridState, setRenderCapacityGridState] = useState<any>();
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
      vnfClusterInfoId: infoId !== null ? [infoId] : [],
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
    GetVBOMClusterInfoGrid(query);
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
  } = useOperationTableCrud<VBOMInfoDtoUpdate, VBOMInfoDtoCreate>(
    GetVBOMInfoCreateResource,
    GetVBOMInfoEditResource,
    DeleteVBOMInfo,
    refresh
  );

  const [deleteConfirm, setDeleteConfirm] =
    useState<DataModalConfirm>(stateConfirm);

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const infoRenderList = [
    "vnfNameId",
    "vnfNameDescritpion",
    "vnfClusterInfoId",
    "vmtypenameid",
    "vmWorkLoadType",
    "vmTypeNameDescription",
    "clusterDescription",
    "clusterId",
    "interVmType",
    "intraVmType",
    "nsxt",
  ];
  const instanceRenderList = [
    "vnfVmInstanceId",
    "opCoDescritpion",
    "locationName",
    "noOfVmsPerType",
    "noOfVnfInstances",
    "numa",
    "opCoId",
    "shortLocation",
    "shortLocationId",
    "socket",
  ];
  const capacityRenderList = [
    "backupRequired",
    "dataDisk",
    "eastWestBoundBandWidth",
    "financialYear",
    "iopsLoading",
    "iopsRunning",
    "northDouthBoundBandWidth",
    "osDisk",
    "otherRequirements",
    "probIngRequired",
    "ramPerVm",
    "rxTxCpuCount",
    "vcpuPerVm",
    "vmWorkLoadDistribution",
    "vnfVmCapacityId",
  ];

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      const clusterInfoId = GridDto?.items?.[0]?.vnfClusterInfoId ?? null;
      setInfoId(GridDto?.items?.[0]?.vnfClusterInfoId ?? null);
      if (clusterInfoId !== null && isPermesso) {
        setInstanceQuery({
          ...instancePaginationQuery,
          vnfNameDescritpion: vnfValue !== null ? [vnfValue?.key] : [],
          vnfClusterInfoId: [clusterInfoId],
        });
        // infoCapacityApi({ vnfClusterInfoId: [clusterInfoId] });
      } else {
        setInstanceData(null);
      }
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetVBOMClusterInfoGrid");
    }
  }, [GridDto]);

  // UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridInstanceDto !== undefined || GridInstanceDto !== null) {
      setInstanceData(GridInstanceDto?.items);
      setInstanceId(GridInstanceDto?.items?.[0]?.vnfInfoId ?? null);
      setCapacityId(
        GridInstanceDto?.items?.[0]?.["_vnfVbomCapacityDtoGrid"]?.[0]
          ?.vnfVmCapacityId ?? null
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
    let opcoDropdown: any = await GetFilterColumnVBOMClusterInfo(
      "opCoDescritpion",
      "",
      query
    );
    let hardwareDropdown = null as any;
    let vnfDropdownRes = null as any;

    if (opcoDropdown && opcoDropdown.filter && opcoDropdown.filter.length > 0) {
      setLoader("REMOVE", "GetFilterOpcoColumnCBOM");
      setLoader("ADD", "GetFilterHardwareColumnCBOM");
      const firstKey =
        opcoDropdown?.filter
          ?.sort((a, b) => b.text.localeCompare(a.text))
          .map((res) => ({
            key: res.value,
            text: res.text,
          }))[0]?.key ?? null;

      hardwareDropdown = await GetFilterColumnVBOMClusterInfo(
        "hardwareType",
        "",
        { ...query, opCoDescritpion: firstKey !== null ? [firstKey] : [] }
      );
      setLoader("REMOVE", "GetFilterHardwareColumnCBOM");
      const firstHardwareKey = hardwareDropdown?.filter?.[0]?.value ?? null;
      vnfDropdownRes = await GetFilterColumnVBOMInstanceCapacity(
        "vnfNameDescritpion",
        "",
        {
          ...query,
          opCoDescritpion: firstKey !== null ? [firstKey] : [],
          hardwareType: firstHardwareKey !== null ? [firstHardwareKey] : [],
        }
      );
    }
    setLoader("REMOVE", "GetFilterOpcoColumnCBOM");
    const opcoRes = opcoDropdown?.filter
      ?.sort((a, b) => b.text.localeCompare(a.text))
      ?.map((res) => {
        return { key: res.value, text: res.text };
      });
    const hardwareRes = hardwareDropdown?.filter?.map((res) => {
      return { key: res.value, text: res.text };
    });
    const vnfRes = vnfDropdownRes?.filter?.map((res) => ({
      key: res.value,
      text: res.text,
    }));
    let copy = { ...query } as VBOMInfoQueryObjectGrid;
    copy.opCoDescritpion =
      opcoRes !== null ? [opcoRes?.[0]?.key] : copy.opCoDescritpion;
    copy.hardwareType =
      hardwareRes !== null ? [hardwareRes?.[0]?.key] : copy.hardwareType;
    copy.vnfNameDescritpion =
      vnfRes !== null ? [vnfRes?.[0]?.key] : copy.vnfNameDescritpion;
    setQuery(copy);
    setOpcoDropdown(opcoRes ?? null);
    setHardwareDropdown(hardwareRes ?? null);
    setVnfDropdown(vnfRes ?? null);
    setOpcoValue(opcoRes?.[0] ?? null);
    setHardwareValue(hardwareRes?.[0] ?? null);
    setVnfValue(vnfRes?.[0] ?? null);
  };

  const getHardwareFilter = async (e) => {
    const hardwareDropdown = await GetFilterColumnVBOMClusterInfo(
      "hardwareType",
      "",
      {
        ...query,
        opCoDescritpion: e !== null ? [e?.key] : [],
        hardwareType: [],
      }
    );

    const hardwareRes = hardwareDropdown?.filter?.map((res) => {
      return { key: res.value, text: res.text };
    });

    setQuery({
      ...query,
      opCoDescritpion: e !== null ? [e?.key] : [],
      hardwareType:
        e !== null && hardwareRes !== null ? [hardwareRes?.[0]?.key] : [],
      vnfNameDescritpion: [],
    } as any);

    setHardwareDropdown(hardwareRes ?? null);

    if (e !== null) {
      setHardwareValue(hardwareRes?.[0] ?? null);
    }
  };

  const getVnfFilter = async (e) => {
    const vnfDropdownData = await GetFilterColumnVBOMInstanceCapacity(
      "vnfNameDescritpion",
      "",
      {
        ...query,
        hardwareType: e !== null ? [e?.key] : [],
        vnfNameDescritpion: [],
      }
    );

    const vnfRes = vnfDropdownData?.filter?.map((res) => {
      return { key: res.value, text: res.text };
    });

    setQuery({
      ...query,
      hardwareType: e !== null ? [e?.key] : [],
      vnfNameDescritpion:
        e !== null && vnfRes !== null ? [vnfRes?.[0]?.key] : [],
    } as any);

    setVnfDropdown(vnfRes ?? null);

    if (e !== null) {
      setVnfValue(vnfRes?.[0] ?? null);
    }
  };

  useEffect(() => {
    if (query && isPermesso) {
      setLoader("ADD", "GetVBOMClusterInfoGrid");
      GetVBOMClusterInfoGrid(query).then((x) =>
        setLoader("REMOVE", "GetVBOMClusterInfoGrid")
      );
      setLoader("REMOVE", "GetVBOMClusterInfoGrid");
      setHardwareValue(
        query?.["hardwareType"]?.length > 0
          ? query?.["hardwareType"]?.map((val) => {
              return { key: val, value: val };
            })[0]
          : null
      );
      setOpcoValue(
        query?.["opCoDescritpion"]?.length > 0
          ? query?.["opCoDescritpion"]?.map((val) => {
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
      GetVnfInfoAndCapacityDetails(instanceQuery);
      setVnfValue(
        instanceQuery?.["vnfNameDescritpion"] &&
          instanceQuery?.["vnfNameDescritpion"] !== undefined &&
          instanceQuery?.["vnfNameDescritpion"]?.length > 0
          ? instanceQuery?.["vnfNameDescritpion"]?.map((val) => {
              return {
                key: val,
                value: vnfDropdown?.filter((res) => res.key == val)[0]?.text,
              };
            })[0]
          : null
      );
    }
  }, [instanceQuery]);

  const onChangeInfoDetails = async (id) => {
    setInfoId(id);
    const result = data
      ? data.filter((res) => id === res.vnfClusterInfoId)[0]
      : null;
    const updatedPayload = {
      ...instancePaginationQuery,
      vnfClusterInfoId: result?.vnfClusterInfoId
        ? [result.vnfClusterInfoId]
        : [],
    };
    if (id !== infoId) {
      setInstanceQuery(updatedPayload);
    }
  };

  const infoCapacityApi = async (updatedPayload) => {
    const response: any = await GetVnfInfoAndCapacityDetails({
      ...instancePaginationQuery,
      ...updatedPayload,
    });
  };

  const onChangeInstanceDetails = (id) => {
    setInstanceId(id);
    setCapacityId(
      instanceData?.items?.filter((res) => res.vnfInfoId === id)?.[0]?.[
        "_vnfVbomCapacityDtoGrid"
      ]?.[0]?.vnfVmCapacityId ?? null
    );
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetVBOMClusterInfoGrid");
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
        let copy = { ...query } as VBOMInfoQueryObjectGrid;
        copy.principalId = localState?.id;
        // setQuery(copy);
        // GetVBOMClusterInfoGrid(copy).then((x) =>
        //   setLoader("REMOVE", "GetVBOMClusterInfoGrid")
        // );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetVBOMClusterInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetVBOMClusterInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetVBOMInfoReport({
      ...instanceQuery,
      ...query,
      // vnfClusterInfoId: infoId !== null ? [infoId] : [],
      vnfInfoId: instanceId !== null ? [instanceId] : [],
      opCoDescritpion: opcoValue !== null ? [opcoValue?.key] : [],
      hardwareType: hardwareValue !== null ? [hardwareValue?.key] : [],
      vnfNameDescritpion: vnfValue !== null ? [vnfValue?.key] : [],
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
      const sheetName = "VBOM VNF Info";
      const apiPath = "VBOMInfo";
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
        setLoader("ADD", "GetVBOMClusterInfoGrid");
        GetVBOMClusterInfoGrid(query).then(() =>
          setLoader("REMOVE", "GetVBOMClusterInfoGrid")
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
    const res: any = await VBOMInfoDelete(id, type);
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
          {edit === true ? "Edit VBOM Info" : "Add VBOM Info"}
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
          <VBOMClusterInfoModal
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
          <h3 className="voda-bold">VBOM Info</h3>
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
              className="download-to-excel ml-3 grid-main-btn
            "
              onClick={() => fileHandle()}
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
            <div className="col-4">
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
                    getHardwareFilter(e);
                  }}
                />
              </div>
            </div>
            <div className="col-4">
              <div className="col-12 p-0">
                <DropdownInputComponent
                  label={"Select HW Type"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  value={
                    hardwareDropdown &&
                    resourceArrayRefactor(hardwareDropdown).filter(
                      (x) => x?.key === hardwareValue?.key
                    )
                  }
                  options={
                    hardwareDropdown && resourceArrayRefactor(hardwareDropdown)
                  }
                  onChange={(e: any) => {
                    setHardwareValue(e);
                    getVnfFilter(e);
                  }}
                />
              </div>
            </div>
            <div className="col-4">
              <div className="col-12 p-0">
                <DropdownInputComponent
                  label={"Select VNF Name"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={false}
                  value={
                    vnfDropdown &&
                    resourceArrayRefactor(vnfDropdown).filter(
                      (x) => x?.key === vnfValue?.key
                    )
                  }
                  options={vnfDropdown && resourceArrayRefactor(vnfDropdown)}
                  onChange={(e: any) => {
                    setVnfValue(e);
                    setQuery({
                      ...query,
                      vnfNameDescritpion: e !== null ? [e?.key] : [],
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
            disabled={
              opcoValue === null || hardwareValue === null || vnfValue === null
            }
            style={{ borderRadius: "20px" }}
          >
            Download
          </Button>
        </DialogActions>
      </Dialog>
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
                      <div className="col-4">
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
                              getHardwareFilter(e);
                            }}
                          />
                        </div>
                      </div>
                      <div className="col-4">
                        <div className="col-12 p-0">
                          <DropdownInputComponent
                            label={"Select HW Type"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              hardwareDropdown &&
                              resourceArrayRefactor(hardwareDropdown).filter(
                                (x) => x?.key === hardwareValue?.key
                              )
                            }
                            options={
                              hardwareDropdown &&
                              resourceArrayRefactor(hardwareDropdown)
                            }
                            onChange={(e: any) => {
                              setHardwareValue(e);
                              getVnfFilter(e);
                            }}
                          />
                        </div>
                      </div>
                      <div className="col-4">
                        <div className="col-12 p-0">
                          <DropdownInputComponent
                            label={"Select VNF Name"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              vnfDropdown &&
                              resourceArrayRefactor(vnfDropdown).filter(
                                (x) => x?.key === vnfValue?.key
                              )
                            }
                            options={
                              vnfDropdown && resourceArrayRefactor(vnfDropdown)
                            }
                            onChange={(e: any) => {
                              setVnfValue(e);
                              setQuery({
                                ...query,
                                vnfNameDescritpion: e !== null ? [e?.key] : [],
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
                    height: "56.5rem",
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
                      VNF Cluster Info
                    </Typography>
                  </Item>
                  {/* <Divider /> */}
                  <div className="col-12" style={{ padding: "15px" }}>
                    {data ? (
                      <>
                        <VBOMClusterInfoGrid
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
                        ></VBOMClusterInfoGrid>
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
                        <VBOMClusterInstanceGrid
                          data={instanceData ?? []}
                          pagination={{
                            ...instanceQuery,
                            vnfClusterInfoId: infoId !== null ? [infoId] : [],
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
                        ></VBOMClusterInstanceGrid>
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
                        <VBOMClusterCapacityGrid
                          data={instanceData ?? []}
                          pagination={{
                            ...instanceQuery,
                            vnfInfoId: instanceId !== null ? [instanceId] : [],
                            vnfClusterInfoId: [],
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
                        ></VBOMClusterCapacityGrid>
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

export default VBOMClusterInfoContainer;
