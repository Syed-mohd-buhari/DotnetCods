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
import { GetCBOMGrid } from "../Redux/Action/CBOM/CBOMGridAction";
import {
  CBOMDtoCreate,
  CBOMDtoGrid,
  CBOMDtoUpdate,
  CBOMQueryObjectGrid,
} from "../Model/CBOM";
import { GetCBOMReport } from "../Redux/Action/CBOM/CBOMDownloadAction";
import CBOMGrid from "../screen/CBOM/CBOMGrid";
import { DeleteCBOM, CBOMDelete } from "../Redux/Action/CBOM/CBOMDeleteAction";
import { GetCBOMCreateResource } from "../Redux/Action/CBOM/CBOMCreateAction";
import { GetCBOMEditResource } from "../Redux/Action/CBOM/CBOMEditAction";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { Box, Grid, Paper, Typography, styled } from "@mui/material";
import VBOMInstanceGrid from "../screen/CBOM/CBOMInstanceGrid";
import VBOMCapacityGrid from "../screen/CBOM/CBOMCapacityGrid";

export let paginationQuery: CBOMQueryObjectGrid = {
  cnfCapacityId: [],
  financialYear: [],
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
  cnfInfoId: [],
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

const CBOMContainer = () => {
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
  const [data, setData] = useState<CBOMDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) => state.CBOMGridReducer.CBOMGridResult
  );
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.CBOMCreateReducer.ResultDtoCreate
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, pageSize } = useAuth();
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
    isPermesso ? GetCBOMGrid : undefined
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
    GetCBOMGrid(query);
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
      setInfoId(GridDto?.items?.[0]?.cnfInfoId ?? null);
      setInstanceId(
        GridDto?.items?.[0]?.["_cbomClusterInfoDtoGrid"]?.[0]
          ?.cnfClusterInfoInstanceId ?? null
      );
      setCapacityId(
        GridDto?.items?.[0]?.["_cbomClusterInfoDtoGrid"]?.[0]?.[
          "_cbomClusterCapacityDtoGrid"
        ]?.[0]?.cnfCapacityId ?? null
      );
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetCBOMGrid");
    }
  }, [GridDto]);

  const onChangeInfoDetails = (id) => {
    setInfoId(id);
    setInstanceId(
      data?.filter((res) => res.cnfInfoId === id)?.[0]?.[
        "_cbomClusterInfoDtoGrid"
      ]?.[0]?.cnfClusterInfoInstanceId ?? null
    );
    setCapacityId(
      data?.filter((res) => res.cnfInfoId === id)?.[0]?.[
        "_cbomClusterInfoDtoGrid"
      ]?.[0]?.["_cbomClusterCapacityDtoGrid"]?.[0]?.cnfCapacityId ?? null
    );
  };

  const onChangeInstanceDetails = (id) => {
    setInstanceId(id);
    setCapacityId(
      data
        ?.filter((res) => res.cnfInfoId === infoId)?.[0]
        ?.["_cbomClusterInfoDtoGrid"]?.filter(
          (res) => res.cnfClusterInfoInstanceId === id
        )?.[0]?.["_cbomClusterCapacityDtoGrid"]?.[0]?.cnfCapacityId ?? null
    );
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetCBOMGrid");
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
        setQuery(copy);
        GetCBOMGrid(copy).then((x) => setLoader("REMOVE", "GetCBOMGrid"));
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetCBOMGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetCBOMGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetCBOMReport(query);
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
      const apiPath = "CBOM";
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
        setLoader("ADD", "GetCBOMGrid");
        GetCBOMGrid(query).then(() => setLoader("REMOVE", "GetCBOMGrid"));
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
    const res: any = await CBOMDelete(id, type);
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
      {/* <Dialog
        open={isVisibleModal}
        onClose={() => closeModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{paper : { sx: { borderRadius: "15px" } }}}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>{edit === true ? "Edit CBOM Info" : "Add CBOM Info"}</h4>
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
          <CBOMModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
      </Dialog> */}
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
        <Grid container spacing={4} sx={{ alignItems: "flex-bettween" }}>
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
                  CNF Info
                </Typography>
              </Item>
              <CBOMGrid
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
              ></CBOMGrid>
              <Paginate
                pagination={{ page: query.page, pageSize: query.pageSize }}
                totalItems={GridDto?.totalItems}
                actions={{ next, back }}
              />
            </Paper>
          </Grid>

          <Grid size={{ xs: 8 }}>
            <Grid container sx={{ direction: "column" }} spacing={4}>
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

export default CBOMContainer;
