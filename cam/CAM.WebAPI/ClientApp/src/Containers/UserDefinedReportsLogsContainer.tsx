import React, { useEffect, useState } from "react";
import { Alert, Dropdown, Form, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router";
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
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
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
import { GetNetworkElementAsIsEditResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";

import { RootState, rootStore } from "../Redux/Store/rootStore";
import NetworkElementAsIsModal from "../screen/NetworkElementAsIs/NetworkElementAsIsModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import {
  DownloadUserDefinedReportsLogs,
  GetUserDefinedReportsLogsGrid,
} from "../Redux/Action/UserDefinedReportsLogs/UserDefinedReportsLogsCommonAction";
import {
  UserDefinedReportsLogsDtoGrid,
  UserDefinedReportsLogsQueryObjectGrid,
} from "../Model/UserDefinedReportsLogs";
import UserDefinedReportsLogsGrid from "../screen/UserDefinedReportsLogs/UserDefinedReportsLogsGrid";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";

export let paginationQuery: UserDefinedReportsLogsQueryObjectGrid = {
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  lastModifiedBy: [],
  userDefinedReportsLogId: [],
  reportName: [],
  reportDownloadedPath: [],
  reportStatus: [],
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
};

const UserDefinedReportsLogsContainer: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalPreview, setIsVisibleModalPreview] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [viewReportId, setViewReportId] = useState<number>();
  const [viewReportName, setViewReportName] = useState<string>("");
  const [orphanColor, setOrphanColor] = useState(false);
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const [nonAdminLength, setNonAdminLength] = useState<number>();
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [cloneReportName, setCloneReportName] = useState("");
  const [cloneVisibleModal, setCloneVisibleModal] = useState(false);
  const [reportDetails, setReportDetails] = useState<any>();
  const [excelPopup, setExcelPopup] = useState(false);
  const [excelPopupData, setExcelPopupData] = useState<{
    id: number | undefined;
    type: string;
    mode: string;
    queryList: any;
  }>({
    id: undefined,
    type: "excel",
    mode: "disaggregated",
    queryList: undefined,
  });
  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    readonly,
    role,
    pageSize,
  } = useAuth();
  //DTO
  const [data, setData] = useState<UserDefinedReportsLogsDtoGrid[] | undefined>(
    []
  );
  const Grid = (state: RootState) =>
    state.UserDefinedReportsLogsGridReducer.UserDefinedReportsLogsGridResult;
  let GridDto = useSelector(Grid);
  // console.log("Tems GridDto", GridDto);
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  // console.log("Tems renderPreviewGridState", renderPreviewGridState);

  const [renderPreviewGridState, setPreviewRenderGridState] = useState<any>();

  // console.log("Tems renderGridState", renderGridState);

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const location = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    GetUserDefinedReportsLogsGrid
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
    setLoader("ADD", "GetUserDefinedReportsLogsGrid");
    closeModal();
    GetUserDefinedReportsLogsGrid(query).then(() =>
      setLoader("REMOVE", "GetUserDefinedReportsLogsGrid")
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
      // Get Non Admin Data Length
      const nonAdimData = GridDto?.items?.filter(
        (x) => x.published === "Published"
      );
      if (GridDto?.totalItems !== undefined && nonAdimData !== undefined) {
        const length = GridDto?.totalItems - nonAdimData?.length;
        setNonAdminLength(length);
      }
      //
      let copy = { ...GridDto?.gridRender } as unknown as
        | CustomGridRender
        | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetAuditGrid");
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetUserDefinedReportsLogsGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };
      if (localState.id != null) {
        setLocalState(localState);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as UserDefinedReportsLogsQueryObjectGrid;
        copy.principalId = localState.id;
        setQuery(copy);
        GetUserDefinedReportsLogsGrid(copy).then((x) => {
          setLoader("REMOVE", "GetUserDefinedReportsLogsGrid");
        });
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    if (isVisibleModalPreview) {
      setIsVisibleModalSetup(false);
    } else {
      GetUserDefinedReportsLogsGrid(query).then((x) =>
        setIsVisibleModalSetup(false)
      );
    }
  };

  const DownloadReport = async () => {
    let result = await DownloadUserDefinedReportsLogs(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
      onCloseModal();
    }
  };

  const onCloseModal = () => {
    closeModal();
    setIsVisibleModalPreview(false);
    setViewReportId(undefined);
    setExcelPopup(false);
    setExcelPopupData({
      id: undefined,
      type: "excel",
      mode: "disaggregated",
      queryList: undefined,
    });
  };

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      <ModalConfirm data={myConfirm} />
      <Modal
        show={excelPopup}
        backdrop="static"
        backdropClassName="backdropLookup"
        dialogClassName="dialogLookup"
        className="modalLookup"
        keyboard={false}
        size="lg"
        centered
      ></Modal>

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
          <h3 className="voda-bold">User Defined Reports Logs</h3>
        </div>
        <div className="row mx-0 justify-content-between">
          <div className="d-flex">
            <button
              className="download-to-excel mrl-10"
              onClick={() => DownloadReport()}
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
      </div>
      <div className="">
        <UserDefinedReportsLogsGrid
          data={data}
          pagination={query}
          orphanColor={orphanColor}
          renderGrid={renderGridState?.render ?? []}
          action={{
            onDownload: (id, mode) => {
              setExcelPopupData({ ...excelPopupData, id: id, mode: mode });
              setExcelPopup(true);
            },
            EditNotDetail,
            EditAndDetail,
            Filter: setQuery,
            Restore,
            closeModal,
            setIsFiltriAttivati,
          }}
        ></UserDefinedReportsLogsGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </div>
  );
};

export default UserDefinedReportsLogsContainer;
