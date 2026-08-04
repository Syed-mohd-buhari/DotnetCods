import React, { useEffect, useState } from "react";
import { Alert, Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import {
  ReconciliationDtoCreate,
  ReconciliationDtoGrid,
  ReconciliationDtoUpdate,
  ReconciliationQueryObjectGrid,
} from "../Model/Reconciliation";
import setLoader from "../Redux/Action/LoaderAction";
import {
  GetReconciliationGrid,
  UpdateReconciliationStatus,
} from "../Redux/Action/Reconciliation/ReconciliationGridAction";
import { RootState } from "../Redux/Store/rootStore";
import ReconciliationGrid from "../screen/Reconciliation/ReconciliationGrid";
import SetupColumns from "../screen/Shared/SetupColumns";
import {
  NetworkElementAsPlannedDtoCreate,
  NetworkElementAsPlannedDtoUpdate,
} from "../Model/NetworkElementAsPlanned";
import { GetNetworkElementAsPlannedCreateResource } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCreateAction";
import { GetNetworkElementAsPlannedEditResource } from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedEditAction";
import {
  RestoreNetworkElementAsPlanned,
  deleteNetworkElementAsPlanned,
} from "../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedDeleteAction";
import NetworkElementAsPlannedModal from "../screen/NetworkElementAsPlanned/NetworkElementAsPlannedModal";
import UpdatePlannedActivityStatusModal from "../screen/PlannedActivities/UpdatePlannedActivityStatusModal";
import { GetLcmEngineeringCreateResource } from "../Redux/Action/LcmEngineering/LcmEngineeringCreateAction";
import { GetLcmEngineeringEditResource } from "../Redux/Action/LcmEngineering/LcmEngineeringEditAction";
import {
  RestoreLcmEngineering,
  deleteLcmEngineering,
} from "../Redux/Action/LcmEngineering/LcmEngineeringDeleteAction";
import LcmEngineeringModal from "../screen/LcmEngineering/LcmEngineeringModal";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery: ReconciliationQueryObjectGrid = {
  assetsId: [],
  opCo: [],
  oem: [],
  elementName: [],
  newSWVersion: [],
  currentSWVersion: [],
  newHWType: [],
  currentHWType: [],
  deploymentStatus: [],
  status: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
};

const Reconciliation: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState<number>();
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [plannedActivityIdToManage, setPlannedActivityIdToManage] =
    useState<number>();
  const [plannedActivityTypeFor, setplannedActivityTypeFor] =
    useState<number>();
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

  const { darkMode } = useTheme();

  //DTO
  const [data, setData] = useState<ReconciliationDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.reconciliationGridReducer.ReconciliationGridResult;
  let GridDto = useSelector(Grid);

  const PAResult = (state: RootState) =>
    state.lcmEngineeringEditReducer.ResultDtoEdit;
  let PAResultDto = useSelector(PAResult);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, pageSize } = useAuth();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetReconciliationGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetReconciliationGrid");
    GetReconciliationGrid(query).then(() =>
      setLoader("REMOVE", "GetReconciliationGrid")
    );
  };

  // const {
  //   New,
  //   Edit,
  //   isVisibleModal,
  //   edit,
  //   confirm,
  //   closeModal,
  //   Delete,
  //   localStateHistory,
  //   setLocalState,
  //   Restore,
  // } = useOperationTableCrud<
  //   NetworkElementAsPlannedDtoUpdate,
  //   NetworkElementAsPlannedDtoCreate
  // >(
  //   GetNetworkElementAsPlannedCreateResource,
  //   GetNetworkElementAsPlannedEditResource,
  //   deleteNetworkElementAsPlanned,
  //   refresh,
  //   RestoreNetworkElementAsPlanned
  // );

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
    GetLcmEngineeringCreateResource,
    GetLcmEngineeringEditResource,
    deleteLcmEngineering,
    refresh,
    RestoreLcmEngineering
  );

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetReconciliationGrid");
    }
  }, [GridDto]);

  //UPDATE ON Planned Activity
  // useEffect(() => {
  //   if (PAResultDto !== undefined || PAResultDto !== null) {
  //     if (detailId) {
  //       UpdateReconciliationStatus(detailId);
  //     }
  //   }
  // }, [PAResultDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetReconciliationGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };

      if (localState?.id != null) {
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as ReconciliationQueryObjectGrid;
        copy.assetsId = [];
        copy.assetsId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
        GetReconciliationGrid(copy).then((x) =>
          setLoader("REMOVE", "GetReconciliationGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetReconciliationGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetReconciliationGrid")
      // );
    }
    setLoader("REMOVE", "GetReconciliationGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetReconciliationGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  // const EditDetails = (id: number, idDetail) => {
  //   setLocalState({
  //     id: id,
  //     tab: "plannedActivities",
  //     prevPage: localStateHistory?.prevPage ?? "",
  //   });
  //   Edit(id);
  //   setDetailId(idDetail);
  // };

  const EditDetails = (
    id: number,
    idDetail: number,
    formDisabled?: boolean,
    type?: string
  ) => {
    setLocalState({
      id: id,
      tab: type === "originalLcm" ? "operational" : "plannedActivities",
      prevPage: localStateHistory?.prevPage ?? "",
      formDisabed: formDisabled,
    });
    Edit(id);
    setDetailId(idDetail);
  };

  const updatePlannedStatus = (id: number, plannedActivityTypeFor: number) => {
    setPlannedActivityIdToManage(id);
    setplannedActivityTypeFor(plannedActivityTypeFor ?? 0);
    setIsVisibleModalStatus(true);
  };

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <Modal
        show={isVisibleModal}
        onHide={closeModal}
        backdrop="static"
        keyboard={false}
        size="xl"
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3  ">
              <h4>Create Planned Activity</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <LcmEngineeringModal
            idDetail={detailId}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
          />
        </Modal.Body>
      </Modal>
      <Modal
        show={isVisibleModalStatus}
        backdrop="static"
        keyboard={false}
        size="lg"
        onHide={() => setIsVisibleModalStatus(false)}
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-0">Update Planned Activity Status</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <UpdatePlannedActivityStatusModal
            isFromPlannedActivityModal={false}
            action={{
              setIsVisibleModalStatus,
              Refresh: () => refresh(),
            }}
            planningActivityDetailsResourceId={plannedActivityIdToManage}
            plannedActivityTypeForEnum={plannedActivityTypeFor}
          />
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
          <h3 className="voda-bold">Reconciliation</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
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
      <ReconciliationGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          Filter: setQuery,
          setIsFiltriAttivati,
          EditDetails,
          updatePlannedStatus,
        }}
      ></ReconciliationGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default Reconciliation;
