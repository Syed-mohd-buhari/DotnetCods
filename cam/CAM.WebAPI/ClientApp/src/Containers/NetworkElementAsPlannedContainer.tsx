import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import NetworkElementAsPlannedGrid from "../screen/NetworkElementAsPlanned/NetworkElementAsPlannedGrid";
import NetworkElementAsPlannedModal from "../screen/NetworkElementAsPlanned/NetworkElementAsPlannedModal";
import {
  NetworkElementAsPlannedDtoGrid,
  NetworkElementAsPlannedDtoCreate,
  NetworkElementAsPlannedDtoUpdate,
  NetworkElementAsPlannedQueryObjectGrid,
} from "../Model/NetworkElementAsPlanned";
import { GetNetworkElementAsPlannedGrid } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedGridAction";
import { GetNetworkElementAsPlannedCreateResource } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCreateAction";
import { GetNetworkElementAsPlannedEditResource } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedEditAction";
import {
  DeleteDeepNetworkElementAsPlanned,
  deleteNetworkElementAsPlanned,
  GetRelatedRecordsNetworkElementAsPlanned,
  RestoreNetworkElementAsPlanned,
} from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedDeleteAction";
import { GetNetworkElementAsPlannedReport } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { useAuth } from "./../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";
import HMDetailsModal from "../screen/NetworkElementAsPlanned/HMDetailsModal";

export let paginationQuery: NetworkElementAsPlannedQueryObjectGrid = {
  nodeIndex: [],
  opCo: [],
  designComponent: [],
  elementName: [],
  capacityPlanReference: [],
  additionalInformation1: [],
  additionalInformation2: [],
  automatedFeedback: undefined,
  designComponentIndex: [],
  plannedAction: undefined,
  networkConstruct: [],
  environment: [],
  deploymentStatus: [],
  deploymentType: [],
  location: [],
  nfviBundleID: [],
  subDomainSpoc: [],
  eduspoc: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  hwResourceKey: [],
  previousHWResourceKey: [],
  swResourceKey: [],
  previousSWResourceKey: [],
};

const NetworkElementAsPlanned: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();

  const { darkMode } = useTheme();

  //DTO
  const [data, setData] = useState<
    NetworkElementAsPlannedDtoGrid[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.networkElementAsPlannedGridReducer.NetworkElementAsPlannedGridResult;
  const externalRefreshDto = useSelector(
    (state: RootState) => state.externalRefreshReducer.refresh
  );
  let GridDto = useSelector(Grid);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [otherButtonForRelatedModal, setOtherButtonForRelatedModal] = useState<{
    deleteAllButton: string | undefined;
    deleteButton: string | undefined;
  }>({ deleteAllButton: undefined, deleteButton: undefined });
  const [idPlannedToDelete, setIdPlannedToDelete] = useState<number>();
  const [assestModalFlag, setAssestModalFlag] = useState(false);
  const [isAssetsModal, setIsAssetsModal] = useState<any>({
    type: "",
    id: undefined,
    opCo: undefined,
  });

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true && isPermesso) {
      setLoader("ADD", "GetNetworkElementAsPlannedGrid");
      GetNetworkElementAsPlannedGrid(query).then(() => {
        rootStore.dispatch({ type: "REFRESH", payload: false });
        setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
      });
    }
  }, [externalRefreshDto]);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetNetworkElementAsPlannedGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetNetworkElementAsPlannedGrid");
    closeModal();
    GetNetworkElementAsPlannedGrid(query).then(() =>
      setLoader("REMOVE", "GetNetworkElementAsPlannedGrid")
    );
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
  } = useOperationTableCrud<
    NetworkElementAsPlannedDtoUpdate,
    NetworkElementAsPlannedDtoCreate
  >(
    GetNetworkElementAsPlannedCreateResource,
    GetNetworkElementAsPlannedEditResource,
    deleteNetworkElementAsPlanned,
    refresh,
    RestoreNetworkElementAsPlanned
  );

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const EditAndDetail = (id: number, idDetail) => {
    setLocalState({
      id: id,
      tab: "plannedActivities",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
    setDetailId(idDetail);
  };

  const EditNotDetail = (id: number) => {
    setLocalState({
      id: id,
      tab: "networkelement",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
  };

  const HModalDetails = (type: string, id: number, opCoId: number) => {
    setAssestModalFlag(true);
    setIsAssetsModal({ type: type, id: id, opCoId: Number(opCoId) });
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetNetworkElementAsPlannedGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };

      if (localState?.id != null) {
        if (localState.idDetail && localState.idDetail != null) {
          setLoader("ADD", "GetNetworkElementAsPlannedGrid");
          EditAndDetail(localState?.id, localState.idDetail);
          return;
        }
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as NetworkElementAsPlannedQueryObjectGrid;
        copy.nodeIndex = [];
        copy.nodeIndex?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
        GetNetworkElementAsPlannedGrid(copy).then((x) =>
          setLoader("REMOVE", "GetNetworkElementAsPlannedGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    setLoader("ADD", "GetNetworkElementAsPlannedGrid");
    GetNetworkElementAsPlannedGrid(query).then((x) => {
      setIsVisibleModalSetup(false);
      setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
    });
  };

  const InvocheDownload = async () => {
    let result = await GetNetworkElementAsPlannedReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    setIdPlannedToDelete(id);
    const result = await GetRelatedRecordsNetworkElementAsPlanned(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
      let copyBtnRelated = { ...otherButtonForRelatedModal };
      let hasLinkedPlanned = false;
      result.data.dataRelatedList.map((item) =>
        item.table === "Linked Planned Activity" ||
        item.table === "Network Elements As Is"
          ? (hasLinkedPlanned = true)
          : null
      );
      if (!hasLinkedPlanned) {
        copyBtnRelated.deleteAllButton = "Delete All";
        copyBtnRelated.deleteButton = "Delete Only Planned Activity";
        setOtherButtonForRelatedModal(copyBtnRelated);
      } else {
        copyBtnRelated.deleteAllButton = undefined;
        copyBtnRelated.deleteButton = undefined;
        setOtherButtonForRelatedModal(copyBtnRelated);
      }
    } else {
      setMyConfirm({
        title: "Delete orphan record",
        message:
          "This item is not currently in use in another entity, do you want to delete it?",
        button: "Delete",
        item: idPlannedToDelete ? idPlannedToDelete : "",
        isOpen: true,
        actions: {
          cancel: () => setMyConfirm(stateConfirm),
          confirm: async () => {
            setMyConfirm(stateConfirm);
            await DeleteDeepNetworkElementAsPlanned(id, false);
            refresh();
          },
        },
      });
    }
  };

  const confirmDelete = async (deleteOnlyPlannedActivity: boolean) => {
    if (idPlannedToDelete) {
      await DeleteDeepNetworkElementAsPlanned(
        idPlannedToDelete,
        deleteOnlyPlannedActivity
      );
      refresh();
    }
  };

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        deleteAllButton={otherButtonForRelatedModal.deleteAllButton}
        deleteButton={otherButtonForRelatedModal.deleteButton}
        action={{
          closeModal: () => setIsVisibleModalRelated(false),
          confirm: (deleteOnlyPlannedActivity: boolean) =>
            confirmDelete(deleteOnlyPlannedActivity),
        }}
      />

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
          <div className="col-12 mt-3  ">
            <h4>{edit === true ? "Edit Asset" : "New Asset"}</h4>
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
          <NetworkElementAsPlannedModal
            idDetail={detailId}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
          />
        </DialogContent>
      </Dialog>

      <Dialog
        open={assestModalFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setAssestModalFlag(false);
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
            <h4>View Ancillary Data</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setAssestModalFlag(false)}
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
          <HMDetailsModal
            assetId={isAssetsModal.id}
            opCoId={isAssetsModal.opCoId}
            action={{
              closeModal: () => {
                setAssestModalFlag(false);
              },
              onSaveAndClose: () => {
                setAssestModalFlag(false);
                refresh();
              },
            }}
          />
        </DialogContent>
      </Dialog>

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
        <Modal.Body className="plr-30">
          <SetupColumns
            renderGrid={renderGridState}
            action={{ closeModalSetup }}
            tab={""}
          ></SetupColumns>
        </Modal.Body>
      </Modal>

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
          <h3 className="voda-bold">Assets</h3>
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
              <span className="fz-14">New Asset</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
          >
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <NetworkElementAsPlannedGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          onDelete,
          EditNotDetail,
          EditAndDetail,
          Filter: setQuery,
          Restore,
          setIsFiltriAttivati,
          HModalDetails: (type: string, id: number, opCoId: number) =>
            HModalDetails(type, id, opCoId),
        }}
      ></NetworkElementAsPlannedGrid>
      <MUIPaginationComponent
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back, updatePageSize }}
      />
    </div>
  );
};

export default NetworkElementAsPlanned;
