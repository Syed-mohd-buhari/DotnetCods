import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Alert, Button, Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { useDispatch, useSelector } from "react-redux";
import { RootState, rootStore, useAppSelector } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import LcmEngineeringOperational from "../screen/LcmEngineering/LcmEngineeringGrid";
import LcmEngineeringModal from "../screen/LcmEngineering/LcmEngineeringModal";
import {
  LcmEngineeringDtoGrid,
  LcmEngineeringDtoCreate,
  LcmEngineeringDtoUpdate,
  LcmEngineringQueryObjectGrid,
} from "../Model/LcmEngineering";
import {
  GetArchivedLcmEngineeringGrid,
  GetLcmEngineeringGrid,
} from "../Redux/Action/LcmEngineering/LcmEngineeringGridAction";
import { GetLcmEngineeringCreateResource } from "../Redux/Action/LcmEngineering/LcmEngineeringCreateAction";
import { GetLcmEngineeringEditResource } from "../Redux/Action/LcmEngineering/LcmEngineeringEditAction";
import {
  DeleteDeepLcmEngineering,
  deleteLcmEngineering,
  GetRelatedRecordsLcmEngineering,
  RestoreLcmEngineering,
} from "../Redux/Action/LcmEngineering/LcmEngineeringDeleteAction";
import {
  GetLcmEngineeringArchiveReport,
  GetLcmEngineeringReport,
} from "../Redux/Action/LcmEngineering/LcmEngineeringDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useLocation, useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";

import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import SetupColumns from "../screen/Shared/SetupColumns";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { useAuth } from "./../Hook/useAuth";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "./../Redux/Action/NotificationAction";
import { NotifyType } from "./../Redux/Reducer/NotificationReducer";
import LcmEngAuditModal from "../screen/LcmEngineering/LcmEngAuditModal";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";
import TourGuide from "../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../Redux/Action/tourActions";
import { getLcmTourSteps } from "../Constant/TourSteps";

export let paginationQuery: LcmEngineringQueryObjectGrid = {
  designComponent: [],
  opCo: [],
  operationalContact: [],
  hardwareSupportProvider: [],
  hardwareSupportType: [],
  softwareSupportProvider: [],
  softwareEndOfWarrantyDate: undefined,
  softwareSupportType: [],
  numberOfNodes: [],
  subDomainSpoc: [],
  isLcmAncillaryData: [],
  verticalName: [],
  eduspoc: [],
  plannedActivity: [],
  sortBy: "",
  isSortAscending: false,
  productImportanceId: [],
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  lcmEngineeringId: undefined,
  numberOfNodesInLab: [],
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
  warranty: [],
};

const LcmEngineering: React.FC = (props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const handleEndTour = () => dispatch(endGuideTour());
  const [keyTabs, setKeyTabs] = useState("operational");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [auditModalFlag, setAuditModalFlag] = useState(false);
  const [isAuditModal, setIsAuditModal] = useState<any>({
    type: "",
    id: undefined,
    opCo: undefined,
  });
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //DTO
  const [data, setData] = useState<LcmEngineeringDtoGrid[] | undefined>([]);
  const GridDto = useSelector(
    (state: RootState) =>
      state.lcmEngineeringGridReducer.LcmEngineeringGridResult
  );
  const externalRefreshDto = useSelector(
    (state: RootState) => state.externalRefreshReducer.refresh
  );
  const typeLCM = new URLSearchParams(location.search).get("typeLCM");

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetLcmEngineeringGrid(query).then(() => {
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [archivedClicked, setArchivedClicked] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [otherButtonForRelatedModal, setOtherButtonForRelatedModal] = useState<{
    deleteAllButton: string | undefined;
    deleteButton: string | undefined;
  }>({ deleteAllButton: undefined, deleteButton: undefined });
  const [idPlannedToDelete, setIdPlannedToDelete] = useState<number>();

  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const { darkMode } = useTheme();
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    paginationQuery,
    undefined
  );
  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    console.log("refresh");
    closeModal();
    GetLcmEngineeringGrid(query);
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
  } = useOperationTableCrud<LcmEngineeringDtoUpdate, LcmEngineeringDtoCreate>(
    GetLcmEngineeringCreateResource,
    GetLcmEngineeringEditResource,
    deleteLcmEngineering,
    refresh,
    RestoreLcmEngineering
  );

  const resetQuery = () => {
    console.log("resetQuery");
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const EditAndDetail = (
    id: number,
    idDetail,
    formDisabled?: boolean,
    type?: string
  ) => {
    console.log("EditAndDetail");
    setLocalState({
      id: id,
      tab: type === "originalLcm" ? "operational" : "plannedActivities",
      prevPage: localStateHistory?.prevPage ?? "",
      formDisabed: formDisabled,
    });
    Edit(id);
    setDetailId(idDetail);
  };

  const EditNotDetail = (id: number) => {
    console.log("EditNotDetail");
    setLocalState({
      id: id,
      tab: "operational",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
  };

  const AuditModal = (type: string, id: number, opCo: string) => {
    setAuditModalFlag(true);
    setIsAuditModal({ type: type, id: id, opCo: opCo });
  };

  const resetLocalState = (type: boolean) => {
    let localState = location.state as {
      id: number | null;
      tab: string;
      prevPage: string;
      idDetail: number | string;
      ids?: number[];
      formDisabed: false;
    };

    localState = {
      id: null,
      tab: "",
      prevPage: "",
      idDetail: "",
      formDisabed: false,
    };
    setLocalState(localState);
  };

  const goToArchivedLCM = () => {
    console.log("goToArchivedLCM");
    let location = {
      pathname: "/lcmengineering",
      search: "?typeLCM=archived",
    };
    navigate(location);
    setQuery(paginationQuery);
  };

  //GET ARCHIVED / LCM ENGINEERING BASED ON QUERY PARAMS
  useEffect(() => {
    sessionStorage.setItem("archivedType", "LCM");
    if (isPermesso) {
      if (typeLCM !== "" && typeLCM !== undefined && typeLCM !== null) {
        setArchivedClicked(true);
        sessionStorage.setItem("isArchivedMode", "true");
        GetArchivedLcmEngineeringGrid({ ...query, archived: true })
          .then((res) => {})
          .catch((err) => console.log(err));
      } else {
        sessionStorage.removeItem("isArchivedMode");
        GetLcmEngineeringGrid({
          ...query,
          lcmEngineeringId:
            location?.state?.lcmIds !== null
              ? location?.state?.lcmIds
              : location?.state?.id
              ? [location?.state?.id]
              : (query as any).lcmEngineeringId,
        }).then(() => {
          rootStore.dispatch({ type: "REFRESH", payload: false });
        });
        setArchivedClicked(false);
      }
    }
    return () => {
      sessionStorage.removeItem("isArchivedMode");
    };
  }, [location, query, typeLCM, isPermesso]);
  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (isPermesso && location.state !== null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
        lcmIds?: number[];
      };
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
      if (localState?.lcmIds !== null) {
        setRedirect(true);
        setFilterRedirect(true);
        setLocalState(localState);
        return;
      }

      if (localState.ids !== undefined && localState.ids.length > 0) {
        let copy = { ...query } as LcmEngineringQueryObjectGrid;
        copy.page = 1;
        copy.pageSize = 0;
        copy.lcmEngineeringId = [] as number[];
        if (copy.lcmEngineeringId != undefined) {
          localState.ids.map((x) => {
            copy.lcmEngineeringId?.push(x);
          });
        }
        setRedirect(true);
        setFilterRedirect(true);
        setLocalState(localState);
        return;
      }

      if (localState?.id !== null) {
        setLocalState(localState);
        setRedirect(
          localState?.tab === "plannedActivities" ||
            localState?.tab === "plannedActivitiesFromHome"
            ? false
            : true
        );
        setFilterRedirect(true);

        if (localState.idDetail && localState.idDetail !== null) {
          EditAndDetail(localState?.id, localState.idDetail);
          return;
        }

        let copy = { ...query } as LcmEngineringQueryObjectGrid;

        copy.lcmEngineeringId = [];
        copy.lcmEngineeringId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);

        Edit(localState?.id);
      }
    }
  }, [isPermesso]);

  const closeModalSetup = (changed: boolean) => {
    console.log("Close Modal");
    if (archivedClicked) {
      GetArchivedLcmEngineeringGrid(query).then((x) =>
        setIsVisibleModalSetup(false)
      );
    } else {
      console.log("GetLcmEngineeringGrid( - 4");
      GetLcmEngineeringGrid(query).then((x) => setIsVisibleModalSetup(false));
    }
  };

  const InvocheDownload = async () => {
    console.log(InvocheDownload);
    if (archivedClicked) {
      let result = await GetLcmEngineeringArchiveReport(query);
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
      }
    } else {
      let result = await GetLcmEngineeringReport(query);
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
      }
    }
  };

  const onDelete = async (id: number) => {
    console.log("onDelete");
    setIdPlannedToDelete(id);
    const result = await GetRelatedRecordsLcmEngineering(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
      let copyBtnRelated = { ...otherButtonForRelatedModal };
      let hasLinkedPlanned = false;
      result.data.dataRelatedList?.map((item) =>
        item.table === "Linked Planned Activity"
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
            await DeleteDeepLcmEngineering(id, false);
            refresh();
          },
        },
      });
    }
  };

  const confirmDelete = async (deleteOnlyPlannedActivity: boolean) => {
    console.log("confirmDelete");
    if (idPlannedToDelete) {
      await DeleteDeepLcmEngineering(
        idPlannedToDelete,
        deleteOnlyPlannedActivity
      );
      refresh();
    }
  };

  const fileHandle = async () => {
    console.log("fileHandle");
    try {
      const sheetName = "LCM Engineering";
      const apiPath = "lcmEnginnering";
      const status = await handleImportFile(sheetName, apiPath);
      if (status && status?.["warning"] === true) {
        rootStore.dispatch(
          setNotification({
            message: status?.["info"],
            notifyType: NotifyType.success,
          })
        );
        setShow(false);
        refresh();
      } else if (status && status?.["warning"] === false) {
        setAlertStatus({
          message: status?.["info"],
          class: "danger",
        });
        setShow(true);
      }
    } catch (error) {
      console.log(error);
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
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            closeModal(false);
          }
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
            <h4>
              {edit === true ? "Edit LCM ENGINEERING" : "New LCM ENGINEERING"}
            </h4>
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
          <LcmEngineeringModal
            idDetail={detailId}
            keyTab={localStateHistory?.tab}
            formDisabed={localStateHistory?.formDisabed}
            edit={edit}
            lcmRedirect={redirect ?? false}
            prevPage={prevPage}
            action={{ closeModal, refresh, Edit }}
            resetLocalState={resetLocalState}
          />
        </DialogContent>
      </Dialog>

      <Dialog
        open={auditModalFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setAuditModalFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4>
              {isAuditModal.type === "add"
                ? "Add Ancillary Data"
                : "View Ancillary Data"}
            </h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setAuditModalFlag(false)}
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
          <LcmEngAuditModal
            lcmId={isAuditModal.id}
            opCo={isAuditModal.opCo}
            isAdd={isAuditModal.type === "add" ? true : false}
            action={{
              closeModal: () => {
                setAuditModalFlag(false);
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
          ></SetupColumns>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          {location?.state?.tab === "plannedActivities" ||
          location?.state?.tab === "plannedActivitiesFromHome" ||
          redirect === true ||
          archivedClicked ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2 btnEditLink"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => {
                  if (location?.state?.tab === "plannedActivitiesFromHome") {
                    window.opener = null;
                    window.open("", "_self");
                    window.close();
                  }
                  navigate(-1);
                }}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}

          <h3 className="voda-bold">
            {archivedClicked ? "Archived LCM Engineering " : "LCM Engineering"}
          </h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && !archivedClicked && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn"
              onClick={() => {
                New();
              }}
              type="button"
              id="lcm_addButton_tour"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New LCM Engineering</span>
            </button>
          )}

          {tipologicaPermesso && !archivedClicked && (
            <button
              className="download-to-excel ml-3 grid-main-btn"
              onClick={() => fileHandle()}
              id="lcm_importButton_tour"
            >
              Import Excel
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
            id="lcm_downloadButton_tour"
          >
            Download to Excel
          </button>

          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>
            <Dropdown.Menu className="grid-main-btn">
              <Dropdown.Item>Legend</Dropdown.Item>
              <div className="bubbleMenuLegenda">
                <div className="triangleBubbleTop-right"></div>
                <div className="col-12 row mx-0 px-2 my-2">
                  <div className="w-100 mx-0 py-1 d-flex align-items-center">
                    <div className="legendaElement red"></div>
                    <span className="legendaElement">
                      {`Mandatory Field (Engineering)`}
                    </span>
                  </div>
                  <div className="w-100 mx-0 py-1 d-flex align-items-center">
                    <div className="legendaElement green"></div>
                    <span className="legendaElement">{`Mandatory Field (Operations)`}</span>
                  </div>
                </div>
              </div>
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
              {!archivedClicked && (
                <Dropdown.Item onClick={goToArchivedLCM}>
                  Archived LCM
                </Dropdown.Item>
              )}
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
      <div className="">
        <LcmEngineeringOperational
          data={data}
          pagination={query}
          orphanColor={orphanColor}
          renderGrid={renderGridState?.render ?? []}
          archivedMode={archivedClicked}
          action={{
            onDelete,
            EditNotDetail,
            EditAndDetail,
            Filter: setQuery,
            Restore,
            AuditModal: (type: string, id: number, opCo: string) =>
              AuditModal(type, id, opCo),
            setIsFiltriAttivati,
          }}
        ></LcmEngineeringOperational>
        <MUIPaginationComponent
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back, updatePageSize }}
        />
      </div>
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          tourSteps={getLcmTourSteps}
          page={"lcm"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default LcmEngineering;
