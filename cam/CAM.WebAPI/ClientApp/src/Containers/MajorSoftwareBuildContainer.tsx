import React, { useEffect, useState, useCallback, useMemo } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { CustomGridRender } from "../Model/Common";
import { useDispatch, useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { GetMajorSoftwareBuildGrid } from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";
import {
  MajorSoftwareBuildDtoCreate,
  MajorSoftwareBuildQueryObjectGrid,
  MajorSoftwareBuildDtoGrid,
  MajorSoftwareBuildDtoUpdate,
  MajorSoftwareBuildToCloneDto,
} from "../Model/MajorSoftwareBuild";
import MajorSoftwareGrid from "../screen/MajorSoftwareBuild/MajorSoftwareGrid";
import MajorSoftwareModal from "../screen/MajorSoftwareBuild/MajorSoftwareModal";
import { GetMajorSoftwareBuildCreateResource } from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCreateAction";
import {
  DeleteDeepMajorSoftwareBuild,
  GetRelatedRecordsMajorSoftwareBuild,
  RestoreMajorSoftwareBuild,
} from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildDeleteAction";
import { GetMajorSoftwareBuildEditResource } from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildEditAction";
import { GetMajorSoftwareBuildDownload } from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildDownloadAction";
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
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { GetMajorSoftwareToClone } from "../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import MajorSoftwareModalUpgrade from "../screen/MajorSoftwareBuild/MajorSoftwareVersionModal";
import RefactorForeignIndexModal from "../screen/Shared/RefactorForeignIndexModal";
import { InitializeForeignIndex } from "../Hook/Common";
import { useAuth } from "../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import TourGuide from "../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../Redux/Action/tourActions";
import { getSwBuildTourSteps } from "../Constant/TourSteps";

export let paginationQuery: MajorSoftwareBuildQueryObjectGrid = {
  majorSoftwareBuildId: [],
  originalEquipmentManufacturer: [],
  softwareVersion: [],
  productNamesId: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  generaAvailableDate: undefined,
  deliveryMethod: [],
  vulnerabilityStatus: [],
  operatingSystem: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const MajorSoftwareBuild: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [tourFlag, setTourFlag] = useState(false);
  const [jumpStepIndex, setJumpStepIndex] = useState<number | null>(null);
  const { darkMode } = useTheme();
  const [isSearchUrlObj, setIsSearchUrlObj] = useState(false);
  const [searchParams] = useSearchParams();
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const majorSwBuildIndex = useSelector(
    (state: RootState) => state.tourStep.majorSwBuildIndex
  );
  const handleEndTour = () => dispatch(endGuideTour());

  //DTO
  const [data, setData] = useState<MajorSoftwareBuildDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.majorSoftwareBuildGridReducer.MajorSoftwareBuildGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetMajorSoftwareBuildGrid(query).then(() => {
        setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const { RefactorUser, readonly, tipologicaPermesso, isPermesso, pageSize } =
    useAuth();

  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleModalUpgrade, setIsVisibleModalUpgrade] = useState(false);
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetMajorSoftwareBuildGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetMajorSoftwareBuildGrid");
    closeModal();
    GetMajorSoftwareBuildGrid(query).then(() =>
      setLoader("REMOVE", "GetMajorSoftwareBuildGrid")
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
    MajorSoftwareBuildDtoUpdate,
    MajorSoftwareBuildDtoCreate
  >(
    GetMajorSoftwareBuildCreateResource,
    GetMajorSoftwareBuildEditResource,
    DeleteDeepMajorSoftwareBuild,
    refresh,
    RestoreMajorSoftwareBuild
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetMajorSoftwareBuildGrid");
    if (location.state !== null && location.state !== undefined) {
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
        let copy = { ...query } as MajorSoftwareBuildQueryObjectGrid;
        copy.majorSoftwareBuildId = [];
        copy.majorSoftwareBuildId?.push(localState?.id);
        copy.principalId = localState?.id;
        // GetMajorSoftwareBuildGrid(copy).then((x) => {
        //   setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
        // });
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
      // GetMajorSoftwareBuildGrid(paginationQuery).then((x) => {
      //   setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
      // });
    }
  }, []);

  useEffect(() => {
    if (majorSwBuildIndex !== null && majorSwBuildIndex >= 0) {
      localStorage.setItem("majorSwBuild", String(majorSwBuildIndex));
      setJumpStepIndex(majorSwBuildIndex);
      setTourFlag(true);
    } else {
      setJumpStepIndex(null);
    }
  }, [majorSwBuildIndex]);

  const closeModalSetup = (changed: boolean) => {
    GetMajorSoftwareBuildGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetMajorSoftwareBuildGrid");
    }
  }, [GridDto]);

  useEffect(() => {
    if (searchParams && searchParams["size"] !== 0) {
      setIsSearchUrlObj(true);
      const paramsObj = Object.fromEntries(searchParams?.entries());
      const transformedObj: any = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];
        acc[key] = value?.split(",");
        return acc;
      }, {});
      if (transformedObj.majorSoftwareBuildId) {
        const lcmPaIds = Array.isArray(transformedObj.majorSoftwareBuildId)
          ? transformedObj.majorSoftwareBuildId
          : [transformedObj.majorSoftwareBuildId];

        transformedObj.majorSoftwareBuildId = lcmPaIds.map(Number);
        Edit(transformedObj.majorSoftwareBuildId);
      }
      GridDto = null;
      let copy = {
        ...query,
        ...(transformedObj ? transformedObj : {}),
      } as MajorSoftwareBuildQueryObjectGrid;
      setQuery(copy);
    } else {
      setIsSearchUrlObj(false);
    }
  }, [searchParams]);

  const InvocheDownload = async () => {
    let result = await GetMajorSoftwareBuildDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const [prevPage, setPrevPage] = useState<string>();

  const [dataToClone, setDataToClone] =
    useState<MajorSoftwareBuildToCloneDto>();

  const GetMajorSoftwareCloneData = async (id: number) => {
    await GetMajorSoftwareToClone(id).then((x) => {
      if (!x?.warning) {
        setDataToClone(x?.data);
        setIsVisibleModalUpgrade(true);
      }
    });
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsMajorSoftwareBuild(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
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
              {edit == true ? "Edit Major SW Build" : "Add Major SW Build"}
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
          {" "}
          <MajorSoftwareModal
            edit={edit}
            softwareRedirect={redirect ?? false}
            prevPage={prevPage}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
          />
        </DialogContent>
      </Dialog>
      <Dialog
        open={isVisibleModalUpgrade}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setIsVisibleModalUpgrade(false);
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
            <h4>Create Upgraded Software Product</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalUpgrade(false)}
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
          {" "}
          <MajorSoftwareModalUpgrade
            data={dataToClone}
            action={{ refresh, setIsVisibleModalUpgrade }}
          />
        </DialogContent>
      </Dialog>
      <ModalConfirm data={confirm} />

      <Modal
        show={isVisibleModalRefactor}
        backdrop="static"
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
            entityType={2}
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
          {isSearchUrlObj || redirect == true ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => {
                  if (isSearchUrlObj === true) {
                    window.opener = null;
                    window.open("", "_self");
                    window.close();
                  } else {
                    navigate(-1);
                  }
                }}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">Major Software Build</h3>
          {redirect == true && filterRedirect == true ? (
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
              id="swBuild_addButton_tour"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New Major SW Build</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
            id="swBuild_downloadButton_tour"
          >
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options">
            <Dropdown.Toggle id="dropdown-autoclose-inside grid-main-btn">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              {!readonly && (
                <>
                  <Dropdown.Item>Preview Orphans</Dropdown.Item>
                  <AdditionalFiltersMenu
                    query={query}
                    orphanColored={orphanColor}
                    action={{
                      setQuery: setQuery,
                      setIsVisible: setIsVisibleAdditionalFilter,
                      getGrid: GetMajorSoftwareBuildGrid,
                      setOrphanColor,
                    }}
                  />
                  {RefactorUser ? (
                    <Dropdown.Item
                      onClick={() =>
                        InitializeForeignIndex(setIsVisibleModalRefactor)
                      }
                    >
                      Activate Refactor Session
                    </Dropdown.Item>
                  ) : null}
                </>
              )}
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <MajorSoftwareGrid
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
          GetMajorSoftwareCloneData,
        }}
        readonly={readonly}
      ></MajorSoftwareGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />

      {tourStarted && (
        <TourGuide
          start={tourStarted}
          jumpStep={jumpStepIndex}
          tourSteps={getSwBuildTourSteps}
          page={"majorSwBuild"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default MajorSoftwareBuild;
