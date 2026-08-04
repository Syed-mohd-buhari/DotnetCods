import React, { useCallback, useEffect, useState } from "react";
import { Dropdown, Modal, Alert } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";

import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { GetDeliveryTrackingGrid } from "../Redux/Action/DeliveryTracking/DeliveryTrackingGridAction";
import {
  DeliveryTrackingDtoGrid,
  DeliveryTrackingDtoUpdate,
  DeliveryTrackingDtoCreate,
  DeliveryTrackingQueryObjectGrid,
} from "../Model/DeliveryTracking";
import DeliveryTrackingTableGrid from "../screen/DeliveryTracking/DeliveryTrackingGrid";

import { useNavigate, useLocation } from "react-router-dom";
import setLoader from "../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";

import RefactorForeignIndexModal from "../screen/Shared/RefactorForeignIndexModal";

import { handleImportFile } from "../Hook/Common";
import { useAuth } from "../Hook/useAuth";
import DeliveryTrackingModal from "../screen/DeliveryTracking/DeliveryTrackingModal";
import { GetDeliveryTrackingEditResource } from "../Redux/Action/DeliveryTracking/DeliveryTrackingEditAction";
import {
  DeleteDeepDeliveryTracking,
  GetRelatedRecordsDeliveryTracking,
  RestoreDeliveryTracking,
} from "../Redux/Action/DeliveryTracking/DeliveryTrackingDeleteAction";
import { GetDeliveryTrackingReport } from "../Redux/Action/DeliveryTracking/DeliveryTrackingDownloadAction";
import { GetDeliveryTrackingCreateResource } from "../Redux/Action/DeliveryTracking/DeliveryTrackingCreateAction";
import { setNotification } from "./../Redux/Action/NotificationAction";
import { NotifyType } from "./../Redux/Reducer/NotificationReducer";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GetProjectPlanRefreshStatus } from "../Redux/Action/ProjectPlanning/ProjectPlanImportAction";

export let paginationQuery: DeliveryTrackingQueryObjectGrid = {
  id: [],
  activity: [],
  ms1EventType: [],
  ms1status: [],
  ms2EventType: [],
  ms2status: [],
  ms3EventType: [],
  ms3status: [],
  ms4EventType: [],
  ms4status: [],
  notes1: [],
  notes2: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
};

const DeliveryTracking: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [confirmModal, setConfirmModal] =
    useState<DataModalConfirm>(stateConfirm);
  const [lastRefreshStatus, setLastRefreshStatus] = useState(false);
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  //DTO
  const [data, setData] = useState<DeliveryTrackingDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.deliveryTrackingGridReducer.DeliveryTrackingGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

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
    isPermesso ? GetDeliveryTrackingGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetDeliveryTrackingGrid");
    closeModal();
    GetDeliveryTrackingGrid(query).then(() =>
      setLoader("REMOVE", "GetDeliveryTrackingGrid")
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
    DeliveryTrackingDtoUpdate,
    DeliveryTrackingDtoCreate
  >(
    GetDeliveryTrackingCreateResource,
    GetDeliveryTrackingEditResource,
    DeleteDeepDeliveryTracking,
    refresh,
    RestoreDeliveryTracking
  );

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };
  const [prevPage, setPrevPage] = useState<string>();

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetDeliveryTrackingGrid(query).then(() => {
        setLoader("REMOVE", "GetDeliveryTrackingGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetDeliveryTrackingGrid");
    }
  }, [GridDto]);

  const closeModalSetup = (changed: boolean) => {
    GetDeliveryTrackingGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const fileHandle = async () => {
    try {
      const sheetName = "Delivery Tracking";
      const apiPath = "deliveryTracking";
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
        setAlertStatus({
          message: status?.["info"],
          class: "danger",
        });
        setShow(true);
        setLoader("ADD", "GetDeliveryTrackingGrid");
        closeModal();
        GetDeliveryTrackingGrid(query).then(() =>
          setLoader("REMOVE", "GetDeliveryTrackingGrid")
        );
      }
    } catch (error) {
      console.log(error);
    }
  };

  const InvocheDownload = async () => {
    let result = await GetDeliveryTrackingReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsDeliveryTracking(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const goBack = () => {
    navigate(-1); // Navigate back one step
  };

  // Refresh status checker
  const checkRefreshData = useCallback(async () => {
    const result = await GetProjectPlanRefreshStatus();
    // setLastRefreshStatus(!!result?.warning);
  }, []);

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      <ModalConfirm data={confirmModal} />
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
            <h4 className="pd-15">
              {edit === true
                ? "View Delivery Tracking"
                : "Add Delivery Tracking"}
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
          <DeliveryTrackingModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
          />
        </DialogContent>
      </Dialog>
      <Modal
        show={isVisibleModalRefactor}
        backdrop="static"
        dialogClassName={"modal-dialog-centered"}
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3">
              <h4>Refactor Foreign Index</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <RefactorForeignIndexModal
            action={{
              setIsVisibleModal: setIsVisibleModalRefactor,
              refreshGrid: refresh,
            }}
            entityType={1}
          ></RefactorForeignIndexModal>
        </Modal.Body>
      </Modal>
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
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          {redirect === true ? (
            <button
              className="d-flex justify-content-center align-items-center mr-3 mb-2 btn btn-link"
              onClick={() => goBack()}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </button>
          ) : null}
          <h3 className="voda-bold fz-28">Delivery Tracking</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="download-to-excel mrl-3 grid-main-btn"
              onClick={() =>
                setConfirmModal({
                  title: "Import Excel",
                  message:
                    "Updating milestone dates in delivery tracking will update its project plan respectively. Are you sure you want to proceed?",
                  button: "Import",
                  item: 0,
                  isOpen: true,
                  actions: {
                    cancel: () => setConfirmModal(stateConfirm),
                    confirm: async () => {
                      fileHandle();
                      setConfirmModal(stateConfirm);
                    },
                  },
                })
              }
            >
              Import Excel
            </button>
          )}
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              {/* {!readonly && (
                <>
                  <Dropdown.Item>Preview Orphans</Dropdown.Item>
                  <AdditionalFiltersMenu
                    query={query}
                    orphanColored={orphanColor}
                    action={{
                      setQuery: setQuery,
                      setIsVisible: setIsVisibleAdditionalFilter,
                      getGrid: GetDeliveryTrackingGrid,
                      setOrphanColor,
                    }}
                  ></AdditionalFiltersMenu>
                </>
              )} */}

              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
              <Dropdown.Item
                onClick={() => !lastRefreshStatus && checkRefreshData()}
              >
                Refresh Data
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
      <DeliveryTrackingTableGrid
        data={data}
        pagination={query}
        renderGrid={renderGridState?.render ?? []}
        orphanColor={orphanColor}
        action={{
          onDelete,
          Edit,
          Filter: setQuery,
          Restore,
          setIsFiltriAttivati,
        }}
        readonly={readonly}
      ></DeliveryTrackingTableGrid>

      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default DeliveryTracking;
