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
import setLoader from "../Redux/Action/LoaderAction";
import { GetSoftwareComponentReport } from "../Redux/Action/SoftwareComponent/SoftwareComponentDownloadAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import SetupColumns from "../screen/Shared/SetupColumns";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import ClusterInfoGrid from "../screen/ClusterInfo/ClusterInfoGrid";
import {
  ClusterInfoDtoGrid,
  ClusterInfoQueryObjectGrid,
} from "../Model/ClusterInfo";
import { GetClusterInfoGrid } from "../Redux/Action/ClusterInfo/ClusterInfoGridAction";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { GetClusterInfo } from "../Redux/Action/ClusterInfo/ClusterInfoDownloadAction";
import { GetLatestBptRefreshStatus } from "../Redux/Action/BPTReport/BPTReportImportAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";

export let paginationQuery: ClusterInfoQueryObjectGrid = {
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  appClusterName: [],
  applicationId: [],
  infraClusterAsPlannedId: [],
  opCoValue: [],
  locationValue: [],
  site: [],
  platformValue: [],
  clustertypeValue: [],
  clusterName: [],
  hardwareTypeValue: [],
  deploymentStatusValue: [],
  verticalResponsibleValue: [],
  applicationName: [],
  opCoId: [],
  // paId: [],
};

const ClusterInfoContainer: React.FC = () => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const { darkMode } = useTheme();

  //DTO
  const [data, setData] = useState<ClusterInfoDtoGrid[] | undefined>([]);

  const Grid = (state: RootState) =>
    state.clusterInfoGridReducer.ClusterInfoGridResult;
  let GridDto = useSelector(Grid);

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
    isPermesso ? GetClusterInfoGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetClusterInfoGrid");
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetClusterInfoGrid");
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
          //   EditAndDetail(localState?.id, localState.idDetail);
          return;
        }
        // setLocalState(localState);
        // Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as ClusterInfoQueryObjectGrid;
        // copy.principalId = localState?.id;
        // setQuery(copy);
        GetClusterInfoGrid(copy).then((x) =>
          setLoader("REMOVE", "GetClusterInfoGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetClusterInfoGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetClusterInfoGrid")
      // );
    }
    setLoader("REMOVE", "GetClusterInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetClusterInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetClusterInfo(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      {/* <ModalConfirm data={confirm} /> */}

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
          <h3 className="voda-bold">Cluster Info</h3>
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
              <span className="fz-14">New SoftwareComponent</span>
            </button>
          )} */}

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
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <ClusterInfoGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          Filter: setQuery,
          setIsFiltriAttivati,
        }}
      ></ClusterInfoGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default ClusterInfoContainer;
