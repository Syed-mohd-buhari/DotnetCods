import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import setLoader from "../Redux/Action/LoaderAction";
import {
  useNavigate,
  useLocation,
  useParams,
  useSearchParams,
} from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import {
  PlannedActivityDtoGrid,
  PlannedActivityDtoUpdate,
  PlannedActivityDtoCreate,
  PlannedActivityQueryObjectGrid,
} from "../Model/PlannedActivity";
import {
  GetPlannedActivityArchivedGrid,
  GetPlannedActivityGrid,
} from "../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { GetPlannedActivityCreateResource } from "../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { GetPlannedActivityEditResource } from "../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import {
  DeleteDeepPlannedActivity,
  GetRelatedRecordsPlannedActivity,
} from "../Redux/Action/PlannedActivity/PlannedActivityDeleteAction";
import {
  GetPlannedActivityArchivedDownload,
  GetPlannedActivityDownload,
} from "../Redux/Action/PlannedActivity/PlannedActivityDownloadAction";
import PlannedActivitiesGrid from "../screen/PlannedActivities/PlannedActivitiesGrid";
import { CustomGridRender } from "../Model/Common";
import { Dropdown, Modal } from "react-bootstrap";
import SetupColumns from "../screen/Shared/SetupColumns";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { useAuth } from "./../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

interface Props {
  forLcm: boolean;
  forNetworkElement: boolean;
  forDesignAspect: boolean;
  forServicePlan: boolean;
  forReport: boolean;
  filterType?: string;
}

const PlannedActivitiesContainer: React.FC<Props> = (props) => {
  let paginationQueryPlanning: PlannedActivityQueryObjectGrid = {
    opCo: [],
    plannedImplementationYear: [],
    activityStatusId: [],
    planningActivityStatusId: [],
    designComponentId: [],
    originalDesignComponent: [],
    driver: [],
    benefits: [],
    plannedActivityDescription: [],
    activityDetailsText: [],
    budgetAvailability: [],
    deliveryProjectName: [],
    localApproval: [],
    deliveryStatusId: [],
    responsibilityPhaseId: [],
    plannedCompletion: undefined,
    notes: [],
    lcmEngineeringId: [],
    riskEngineeringEvaluation: [],
    riskEngineeringNotes: [],
    riskOperationalEvaluation: [],
    riskOperationalNotes: [],
    sortBy: "",
    isSortAscending: false,
    budgetTrackingId: [],
    budgetValueGrid: [],
    deliveryProjectId: [],
    deliveryProjectPpmId: [],
    plannedActivityResourceId: [],
    planningRisk: [],
    relatesToId: [],
    page: 1,
    pageSize: 10,
    lastModified: undefined,
    principalId: undefined,
    deleted: false,
    orphan: false,
    lastModifiedBy: [],
    forLcm: true,
    forNetwork: true,
    forDesignAspect: true,
    forServicePlan: true,
    projectStatus: [],
  };

  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isLandingRedirect, setIsLandingRedirect] = useState(false);
  const [isReportView, setIsReportView] = useState(false);
  const [isSearchUrlObj, setIsSearchUrlObj] = useState(false);
  const [isPAHomeObj, setIsPAHomeObj] = useState(false);
  const { readonly, isPermesso } = useAuth();
  const location: any = useLocation();

  //DTO
  const [data, setData] = useState<PlannedActivityDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.plannedActivityGridReducer.PlannedActivityGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  const { darkMode } = useTheme();
  const filterObj = location.state;
  const [searchParams] = useSearchParams();
  const { filter } = useParams();

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetPlannedActivityGrid(query).then(() => {
        setLoader("REMOVE", "GetPlannedActivityGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);
  useEffect(() => {
    if (filter === "Asset") {
      let copy = {
        ...query,
        ...(filter === "Asset"
          ? {
              AddEditAssetFilter: true,
            }
          : {}),
      } as PlannedActivityQueryObjectGrid;
      setQuery(copy);
    }
    if (filter === "LCM") {
      let copy = {
        ...query,
        ...(filter === "LCM" ? { forLcmLink: ["True"] } : {}),
      } as PlannedActivityQueryObjectGrid;
      setQuery(copy);
    }
    if (filter === "DesignAspect") {
      let copy = {
        ...query,
        ...(filter === "DesignAspect" ? { forDesignAspectLink: ["True"] } : {}),
      } as PlannedActivityQueryObjectGrid;
      setQuery(copy);
    }
    if (filter === "ServiceLevel") {
      let copy = {
        ...query,
        ...(filter === "ServiceLevel" ? { forServicePlanLink: ["True"] } : {}),
      } as PlannedActivityQueryObjectGrid;
      setQuery(copy);
    }

    if (filter === "All") {
      setIsReportView(true);
    }
  }, [filter]);

  useEffect(() => {
    if (filterObj && Object.keys(filterObj).length > 0) {
      let copy = {
        ...query,
        ...(filterObj ? filterObj : {}),
      } as PlannedActivityQueryObjectGrid;
      GridDto = null;
      setQuery(copy);
    }
  }, [filterObj]);

  useEffect(() => {
    if (searchParams && searchParams["size"] !== 0) {
      setIsSearchUrlObj(true);
      const paramsObj = Object.fromEntries(searchParams?.entries());
      const transformedObj: any = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];
        acc[key] = value?.split(",");
        return acc;
      }, {});
      if (transformedObj.paIndex) {
        const lcmPaIds = Array.isArray(transformedObj.paIndex)
          ? transformedObj.paIndex
          : [transformedObj.paIndex];
        transformedObj.plannedActivityId = lcmPaIds.map(Number);
        setIsPAHomeObj(true);
      }
      if (transformedObj.paId) {
        const lcmPaIds = Array.isArray(transformedObj.paId)
          ? transformedObj.paId
          : [transformedObj.paId];
        transformedObj.plannedActivityId = lcmPaIds.map(Number);
        setIsPAHomeObj(true);
        if (transformedObj.paId && transformedObj.lcmId) {
          const flatLcmId = Array.isArray(transformedObj.lcmId)
            ? transformedObj.lcmId.flat(Infinity)[0]
            : transformedObj.lcmId;

          const flatPaId = Array.isArray(transformedObj.plannedActivityId)
            ? transformedObj.plannedActivityId.flat(Infinity)[0]
            : transformedObj.plannedActivityId;

          navigate(
            {
              pathname: "/lcmEngineering",
              search: "id=" + flatLcmId,
            },
            {
              state: {
                id: flatLcmId,
                tab: "plannedActivitiesFromHome",
                prevPage: "home",
                idDetail: flatPaId,
              },
            }
          );
        }
      }
      if (transformedObj.lcmPaId) {
        const lcmPaIds = Array.isArray(transformedObj.lcmPaId)
          ? transformedObj.lcmPaId
          : [transformedObj.lcmPaId];

        transformedObj.plannedActivityId = lcmPaIds.map(Number);
        setIsSearchUrlObj(false);
        delete transformedObj.lcmPaId;
      }
      if (transformedObj.assetPaId) {
        const assetPaIds = Array.isArray(transformedObj.assetPaId)
          ? transformedObj.assetPaId
          : [transformedObj.assetPaId];

        transformedObj.plannedActivityId = assetPaIds.map(Number);
        setIsSearchUrlObj(false);
        delete transformedObj.assetPaId;
      }
      if (transformedObj.endDate && transformedObj.startDate) {
        transformedObj.plannedCompletionValue = {
          startDate: transformedObj?.startDate[0],
          endDate: transformedObj?.endDate[0],
        };
      }
      if (transformedObj.endDate) {
        transformedObj.plannedCompletionValueEndDate =
          transformedObj?.endDate[0];
        delete transformedObj.endDate;
      }
      if (transformedObj.startDate) {
        transformedObj.plannedCompletionValueStartDate =
          transformedObj?.startDate[0];
        delete transformedObj.startDate;
      }
      GridDto = null;
      let copy = {
        ...query,
        ...(transformedObj ? transformedObj : {}),
      } as PlannedActivityQueryObjectGrid;
      setQuery(copy);
    } else {
      setIsSearchUrlObj(false);
      setIsPAHomeObj(false);
      setIsLandingRedirect(false);
    }
  }, [searchParams]);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryPlanning,
    undefined
  );
  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetPlannedActivityGrid");
    closeModal();
    GetPlannedActivityGrid(query).then(() =>
      setLoader("REMOVE", "GetPlannedActivityGrid")
    );
  };

  const { Edit, confirm, closeModal, Delete, setLocalState, Restore } =
    useOperationTableCrud<PlannedActivityDtoUpdate, PlannedActivityDtoCreate>(
      GetPlannedActivityCreateResource,
      GetPlannedActivityEditResource,
      DeleteDeepPlannedActivity,
      refresh
    );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [isArchived, setIsArchived] = useState<boolean>(false);

  const resetQuery = () => {
    setQuery(paginationQueryPlanning);
    setFilterRedirect(false);
  };

  const openedArchived = () => {
    setIsArchived(true);

    navigate(
      {
        pathname: "/plannedActivities",
        search: "typePA=archived",
      },
      {
        state: {
          filter: filter,
          prevPage: prevPage,
        },
      }
    );
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetPlannedActivityGrid");
    }
  }, [GridDto]);

  const [prevPage, setPrevPage] = useState<string>();

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (isPermesso) {
      setLoader("ADD", "GetPlannedActivityGrid");
      if (location.state != null && location.state != undefined) {
        let localState = location.state as {
          id: number | null;
          tab: string;
          prevPage: string;
        };
        if (localState?.id != null) {
          setLocalState(localState);
          Edit(localState?.id);
          setRedirect(true);
          setFilterRedirect(true);
          let copy = { ...query } as PlannedActivityQueryObjectGrid;
          copy.principalId = localState?.id;
          GetPlannedActivityGrid(copy).then((x) =>
            setLoader("REMOVE", "GetPlannedActivityGrid")
          );
          setQuery(copy);
        }
        if (localState.prevPage && localState.prevPage != "") {
          setPrevPage(localState.prevPage);
        }
      }
      setLoader("REMOVE", "GetPlannedActivityGrid");
    }
  }, [isPermesso]);

  useEffect(() => {
    sessionStorage.setItem("archivedType", "PA");
    if (isPermesso) {
      if (
        new URLSearchParams(location.search).get("typePA") !== "" &&
        new URLSearchParams(location.search).get("typePA") !== undefined &&
        new URLSearchParams(location.search).get("typePA") !== null
      ) {
        sessionStorage.setItem("isArchivedMode", "true");
        setIsArchived(true);

        let archivedQuery: any = { ...query };
        const originFilter = location.state?.filter ?? filter;

        if (originFilter === "ServiceLevel") {
          archivedQuery.forServicePlanLink = ["True"];
          sessionStorage.setItem("paOriginFilter", "ServiceLevel");
        } else if (originFilter === "Asset") {
          archivedQuery.AddEditAssetFilter = true;
          sessionStorage.setItem("paOriginFilter", "Asset");
        } else if (originFilter === "LCM") {
          archivedQuery.forLcmLink = ["True"];
          sessionStorage.setItem("paOriginFilter", "LCM");
        } else if (originFilter === "DesignAspect") {
          archivedQuery.forDesignAspectLink = ["True"];
          sessionStorage.setItem("paOriginFilter", "DesignAspect");
        } else {
          sessionStorage.removeItem("paOriginFilter");
        }

        GetPlannedActivityArchivedGrid(archivedQuery);
      } else {
        sessionStorage.removeItem("isArchivedMode");
        sessionStorage.removeItem("paOriginFilter");
        GetPlannedActivityGrid({ ...query, archived: false });
        setIsArchived(false);
      }
    }
    return () => {
      sessionStorage.removeItem("isArchivedMode");
      sessionStorage.removeItem("paOriginFilter");
    };
  }, [location.search, query, isPermesso]);

  const closeModalSetup = (changed: boolean) => {
    if (isArchived) {
      let archivedQuery: any = { ...query };
      const originFilter = location.state?.filter ?? filter;

      if (originFilter === "ServiceLevel") {
        archivedQuery.forServicePlanLink = ["True"];
      } else if (originFilter === "Asset") {
        archivedQuery.AddEditAssetFilter = true;
      } else if (originFilter === "LCM") {
        archivedQuery.forLcmLink = ["True"];
      } else if (originFilter === "DesignAspect") {
        archivedQuery.forDesignAspectLink = ["True"];
      }

      GetPlannedActivityArchivedGrid(archivedQuery).then((x) =>
        setIsVisibleModalSetup(false)
      );
    } else {
      GetPlannedActivityGrid(query).then((x) => setIsVisibleModalSetup(false));
    }
  };

  const InvocheDownload = async () => {
    let downloadQuery: any = { ...query };
    const originFilter = location.state?.filter ?? filter;

    if (originFilter === "ServiceLevel") {
      downloadQuery.forServicePlanLink = ["True"];
    } else if (originFilter === "Asset") {
      downloadQuery.AddEditAssetFilter = true;
    } else if (originFilter === "LCM") {
      downloadQuery.forLcmLink = ["True"];
    } else if (originFilter === "DesignAspect") {
      downloadQuery.forDesignAspectLink = ["True"];
    }

    let result = isArchived
      ? await GetPlannedActivityArchivedDownload(downloadQuery)
      : await GetPlannedActivityDownload(downloadQuery);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsPlannedActivity(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const filterLabelMap = {
    DesignAspect: "Design Aspect",
    ServiceLevel: "Service Level",
  };

  const filterLabel = filter ? filterLabelMap[filter] ?? filter : "";
  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <ModalConfirm data={confirm} />
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
          {isPAHomeObj ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                size={25}
                onClick={() => {
                  if (isPAHomeObj === true) {
                    window.opener = null;
                    window.open("", "_self");
                    window.close();
                  } else {
                    navigate(-1);
                  }
                }}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : isSearchUrlObj && !isArchived ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{
                pathname: `/plannedactivityreport`,
                search: "",
              }}
            >
              <GoArrowLeft
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          {(redirect === true || isArchived) && (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2 btnEditLink"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => {
                  navigate(-1);
                }}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          )}
          <h3 className="voda-bold">
            {isArchived
              ? "Archived Planned Activities"
              : `${filterLabel} Planned Activities`}
          </h3>
          {redirect == true && filterRedirect == true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
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
              {!isArchived && (
                <Dropdown.Item onClick={openedArchived}>
                  Archived Planned Activities
                </Dropdown.Item>
              )}
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <PlannedActivitiesGrid
        forLcm={true}
        forNetworkElement={true}
        forDesignAspect={true}
        forServicePlan={true}
        forReport={isReportView}
        isLandingRedirect={{
          landingRedirect: isLandingRedirect,
          paId: query["plannedActivityId"],
        }}
        data={data}
        pagination={query}
        renderGrid={renderGridState?.render ?? []}
        action={{ onDelete, refresh, Edit, Filter: setQuery, Restore }}
        isArchived={isArchived}
      ></PlannedActivitiesGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default PlannedActivitiesContainer;
