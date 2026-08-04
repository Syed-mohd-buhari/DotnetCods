import React, { useEffect, useState } from "react";
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
import { GetVBOMInfoGrid } from "../Redux/Action/VBOMInfo/VBOMInfoGridAction";
import {
  VBOMInfoDtoCreate,
  VBOMInfoDtoGrid,
  VBOMInfoDtoUpdate,
  VBOMInfoQueryObjectGrid,
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
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { Box, Grid, Paper, Typography, styled } from "@mui/material";
import VBOMInstanceGrid from "../screen/VBOMInfo/VBOMInstanceGrid";
import VBOMCapacityGrid from "../screen/VBOMInfo/VBOMCapacityGrid";

export let paginationQuery: VBOMInfoQueryObjectGrid = {
  vnfVmCapacityId: [],
  vnfInfoId: [],
  vnfNameDescritpion: [],
  vmTypeNameDescription: [],
  shortLocation: [],
  nsxt: [],
  intraVmType: [],
  interVmType: [],
  vmWorkLoadType: [],
  vmStorageBlockSize: [],
  opCoDescritpion: [],
  noOfVnfInstances: [],
  noOfVmsPerType: [],
  numa: [],
  socket: [],
  financialYear: [],
  vcpuPerVm: [],
  rxTxCpuCount: [],
  ramPerVm: [],
  dataDisk: [],
  osDisk: [],
  iopsRunning: [],
  iopsLoading: [],
  vmWorkLoadDistribution: [],
  northDouthBoundBandWidth: [],
  eastWestBoundBandWidth: [],
  otherRequirements: [],
  backupRequired: [],
  probIngRequired: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

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

const VBOMContainer = () => {
  //STATE CONFIRM
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

  //DTO
  const [data, setData] = useState<VBOMInfoDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) => state.VBOMInfoGridReducer.VBOMInfoGridResult
  );
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.VBOMInfoCreateReducer.ResultDtoCreate
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

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetVBOMInfoGrid : undefined
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
    GetVBOMInfoGrid(query);
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
    "vnfInfoId",
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
      setInfoId(GridDto?.items?.[0]?.vnfInfoId ?? null);
      setInstanceId(
        GridDto?.items?.[0]?.["_instanceDtoGrid"]?.[0]?.vnfVmInstanceId ?? null
      );
      setCapacityId(
        GridDto?.items?.[0]?.["_instanceDtoGrid"]?.[0]?.[
          "_capacityDtoGrid"
        ]?.[0]?.vnfVmCapacityId ?? null
      );
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetVBOMInfoGrid");
    }
  }, [GridDto]);

  const onChangeInfoDetails = (id) => {
    setInfoId(id);
    setInstanceId(
      data?.filter((res) => res.vnfInfoId === id)?.[0]?.[
        "_instanceDtoGrid"
      ]?.[0]?.vnfVmInstanceId ?? null
    );
    setCapacityId(
      data?.filter((res) => res.vnfInfoId === id)?.[0]?.[
        "_instanceDtoGrid"
      ]?.[0]?.["_capacityDtoGrid"]?.[0]?.vnfVmCapacityId ?? null
    );
  };

  const onChangeInstanceDetails = (id) => {
    setInstanceId(id);
    setCapacityId(
      data
        ?.filter((res) => res.vnfInfoId === infoId)?.[0]
        ?.["_instanceDtoGrid"]?.filter(
          (res) => res.vnfVmInstanceId === id
        )?.[0]?.["_capacityDtoGrid"]?.[0]?.vnfVmCapacityId ?? null
    );
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetVBOMInfoGrid");
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
        setQuery(copy);
        GetVBOMInfoGrid(copy).then((x) =>
          setLoader("REMOVE", "GetVBOMInfoGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetVBOMInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetVBOMInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetVBOMInfoReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
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
        setLoader("ADD", "GetVBOMInfoGrid");
        GetVBOMInfoGrid(query).then(() =>
          setLoader("REMOVE", "GetVBOMInfoGrid")
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

    setDeleteConfirm({
      title: "Delete Entry",
      message: !alowDelete
        ? `Unable to delete this ${typeValue}, as each ${parentTypeValue} must have at least one associated ${typeValue}.`
        : "Do you want to delete this item?",
      button: "Delete",
      item: id,
      isOpen: true,
      actions: {
        cancel: () => setDeleteConfirm(deleteConfirm),
        ...(alowDelete && {
          confirm: () => handleConfirmDelete(type, id),
        }),
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
            <h4>{edit === true ? "Edit VBOM Info" : "Add VBOM Info"}</h4>
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
          <VBOMInfoModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
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
          {!readonly && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn"
              onClick={New}
              type="button"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">Add New Info</span>
            </button>
          )}
          {!readonly && (
            <button
              className="download-to-excel mrl-10 grid-main-btn
            "
              onClick={() => fileHandle()}
            >
              {/* <img src={require("../img/excel.png")} /> */}
              Import Excel
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn
            "
            onClick={() => InvocheDownload()}
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

      <Box sx={{ flexGrow: 1 }}>
        <Grid container sx={{ spacing: 4, alignItems: "flex-bettween" }}>
          <Grid size={{ xs: 4 }}>
            <Paper
              sx={{
                padding: "1rem",
                paddingBottom: "1.5rem",
                backgroundColor: "#f8f9fa",
                height: "49.6rem",
              }}
              elevation={12}
            >
              <Item
                key={"Info Details"}
                elevation={3}
                sx={{
                  marginRight: "-1rem",
                  marginLeft: "-1rem",
                  marginTop: "-1rem",
                  marginBottom: "1.5rem",
                  backgroundColor: "#dee2e6",
                  alignContent: "center",
                }}
              >
                <Typography
                  variant="h6"
                  sx={{ fontWeight: "bold", marginBottom: "0px" }}
                >
                  VNF Info
                </Typography>
              </Item>
              <VBOMInfoGrid
                data={data}
                pagination={query}
                orphanColor={orphanColor}
                renderGrid={renderGridState?.render ?? []}
                action={{
                  Delete: DeleteModal,
                  Edit,
                  Filter: setQuery,
                  onInfoIdChange: (id: any) => onChangeInfoDetails(id),
                }}
                infoId={infoId}
              ></VBOMInfoGrid>
              <Paginate
                pagination={{ page: query.page, pageSize: query.pageSize }}
                totalItems={GridDto?.totalItems}
                actions={{ next, back }}
              />
            </Paper>
          </Grid>

          <Grid size={{ xs: 8 }}>
            <Grid
              container
              sx={{ spacing: 4, display: "flex", flexDirection: "column" }}
            >
              <Grid sx={{ width: "inherit", paddingRight: 4 }}>
                <Paper
                  sx={{
                    padding: "1rem",
                    paddingBottom: "1.5rem",
                    backgroundColor: "#f8f9fa",
                  }}
                  elevation={6}
                >
                  <Item
                    key={"Instance Details"}
                    elevation={3}
                    sx={{
                      marginRight: "-1rem",
                      marginLeft: "-1rem",
                      marginTop: "-1rem",
                      marginBottom: "1.5rem",
                      backgroundColor: "#dee2e6",
                      alignContent: "center",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{ fontWeight: "bold", marginBottom: "0px" }}
                    >
                      Instance Details
                    </Typography>
                  </Item>
                  <VBOMInstanceGrid
                    data={data}
                    pagination={query}
                    orphanColor={orphanColor}
                    renderGrid={renderGridState?.render ?? []}
                    action={{
                      Delete: DeleteModal,
                      Edit,
                      Filter: setQuery,
                      onInstanceIdChange: (id: any) =>
                        onChangeInstanceDetails(id),
                    }}
                    infoId={infoId}
                    instanceId={instanceId}
                  ></VBOMInstanceGrid>
                </Paper>
              </Grid>
              <Grid sx={{ width: "inherit", paddingRight: 4 }}>
                <Paper
                  sx={{
                    padding: "1rem",
                    paddingBottom: "1.5rem",
                    backgroundColor: "#f8f9fa",
                  }}
                  elevation={6}
                >
                  <Item
                    key={"Capacity Details"}
                    elevation={3}
                    sx={{
                      marginRight: "-1rem",
                      marginLeft: "-1rem",
                      marginTop: "-1rem",
                      marginBottom: "1.5rem",
                      backgroundColor: "#dee2e6",
                      alignContent: "center",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{ fontWeight: "bold", marginBottom: "0px" }}
                    >
                      Capacity Details
                    </Typography>
                  </Item>
                  <VBOMCapacityGrid
                    data={data}
                    pagination={query}
                    orphanColor={orphanColor}
                    renderGrid={renderGridState?.render ?? []}
                    action={{
                      Delete: DeleteModal,
                      Edit,
                      Filter: setQuery,
                      onInstanceIdChange: (id: any) => setInstanceId(id),
                      onCapacityIdChange: (id: any) => setCapacityId(id),
                    }}
                    infoId={infoId}
                    instanceId={instanceId}
                    capacityId={capacityId}
                  ></VBOMCapacityGrid>
                </Paper>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default VBOMContainer;
