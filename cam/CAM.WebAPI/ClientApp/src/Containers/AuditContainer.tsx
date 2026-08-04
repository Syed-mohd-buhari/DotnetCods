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
  AuditDtoCreate,
  AuditDtoGrid,
  AuditDtoUpdate,
  AuditQueryObjectGrid,
} from "../Model/Audit";
import setLoader from "../Redux/Action/LoaderAction";
import { GetNetworkElementAsIsCreateResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import {
  DeleteDeepNetworkElementAsIs,
  GetRelatedRecordsNetworkElementAsIs,
  RestoreNetworkElementAsIs,
} from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsDeleteAction";
import { GetAuditReport } from "../Redux/Action/Audit/AuditDownloadAction";
import { GetNetworkElementAsIsEditResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";
import {
  GetAuditApproveStatus,
  GetAuditGrid,
  GetAuditOverrideStatus,
  GetAuditRejectStatus,
} from "../Redux/Action/Audit/AuditGridAction";
import { RootState } from "../Redux/Store/rootStore";
import AuditGrid from "../screen/Audit/AuditGrid";
import NetworkElementAsIsModal from "../screen/NetworkElementAsIs/NetworkElementAsIsModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery: AuditQueryObjectGrid = {
  identityid: [],
  opCo: [],
  oem: [],
  networkFunction: [],
  nodeType: [],
  elementDeploymentName: [],
  location: [],
  systemTypeId: [],
  softwareReleaseInformationSystemLevel: [],
  softwareProductNumberSystemLevel: [],
  softwareProductionDate: undefined,
  softwareInstallDate: undefined,
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  hardwareAcquisition: [],
  manualOverride: [],
  hardwareSystemId: [],
  dataAcquisitionDate: undefined,
  dataAcquisitionMethod: [],
  elementManager: [],
  elementManagerExportFileFormat: [],
  spareFieldsJson: [],
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

const Audit: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);

  //DTO
  const [data, setData] = useState<AuditDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) => state.auditGridReducer.AuditGridResult;
  let GridDto = useSelector(Grid);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso,pageSize } = useAuth();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetAuditGrid : undefined
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
    setLoader("ADD", "GetAuditGrid");
    closeModal();
    GetAuditGrid(query).then(() => setLoader("REMOVE", "GetAuditGrid"));
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
  } = useOperationTableCrud<AuditDtoUpdate, AuditDtoCreate>(
    GetNetworkElementAsIsCreateResource,
    GetNetworkElementAsIsEditResource,
    DeleteDeepNetworkElementAsIs,
    refresh,
    RestoreNetworkElementAsIs
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
    // setDetailId(idDetail);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetAuditGrid");
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetAuditGrid");
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
          EditAndDetail(localState?.id, localState.idDetail);
          return;
        }
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as AuditQueryObjectGrid;
        copy.identityid = [];
        copy.identityid?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
        GetAuditGrid(copy).then((x) => setLoader("REMOVE", "GetAuditGrid"));
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetAuditGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetAuditGrid")
      // );
    }
    setLoader("REMOVE", "GetAuditGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetAuditGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetAuditReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsNetworkElementAsIs(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const onApprove = async (approveList) => {
    const result = await GetAuditApproveStatus(approveList);

    if (result.warning === true) {
      setShow(false);
      refresh();
    } else {
      setAlertStatus({
        message: result?.["info"] || "",
        class: "danger",
      });
      setShow(true);
    }
  };

  const onReject = async (rejectList) => {
    const result = await GetAuditRejectStatus(rejectList);
    if (result.warning === true) {
      setShow(false);
      refresh();
    } else {
      setAlertStatus({
        message: result?.["info"] || "",
        class: "danger",
      });
      setShow(true);
    }
  };

  const onOverride = async (overrideList) => {
    const result = await GetAuditOverrideStatus(overrideList);
    if (result.warning === true) {
      setShow(false);
      refresh();
    } else {
      setAlertStatus({
        message: result?.["info"] || "",
        class: "danger",
      });
      setShow(true);
    }
  };

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <ModalConfirm data={confirm} />
      <Modal
        show={isVisibleModal}
        onHide={closeModal}
        backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3">
              <h4>
                {edit === true
                  ? "Edit Network Element As-Is"
                  : "Add Network Element As-Is"}
              </h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <NetworkElementAsIsModal
            edit={edit}
            action={{ closeModal, refresh, Edit }}
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
            {/* <ErrorNotification OnModal={true} /> */}
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
          <h3 className="voda-bold">Worklog & Approval</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {/* {!readonly && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab"
              onClick={New}
              type="button"
            >
              <img src={require("../img/plus_1.png")} className="img-15" />
              <span className="fz-14">New Identity</span>
            </button>
          )} */}

          <button
            className="download-to-excel mrl-10"
            onClick={() => InvocheDownload()}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu>
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
      {data ? (
        <div>
          <AuditGrid
            data={data}
            pagination={query}
            orphanColor={orphanColor}
            renderGrid={renderGridState?.render ?? []}
            action={{
              onDelete,
              onApprove,
              onReject,
              onOverride,
              EditNotDetail,
              EditAndDetail,
              Filter: setQuery,
              Restore,
              closeModal,
              setIsFiltriAttivati,
            }}
          ></AuditGrid>
          <Paginate
            pagination={{ page: query.page, pageSize: query.pageSize }}
            totalItems={GridDto?.totalItems}
            actions={{ next, back }}
          />
        </div>
      ) : (
        ""
      )}
    </div>
  );
};

export default Audit;
