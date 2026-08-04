import React, { useEffect, useState } from "react";
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
import { GetMajorHardwareBuildGrid } from "../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import {
  MajorHardwareBuildDtoGrid,
  MajorHardwareBuildDtoUpdate,
  MajorHardwareBuildDtoCreate,
  MajorHardwareBuildQueryObjectGrid,
} from "../Model/MajorHardwareBuild";
import MajorHardwareGrid from "../screen/MajorHardwareBuild/MajorHardwareGrid";
import MajorHardwareModal from "../screen/MajorHardwareBuild/MajorHardwareModal";
import { GetMajorHardwareBuildCreateResource } from "../Redux/Action/MajorHardwareBuild/MajorHardwareBuildCreateAction";
import {
  DeleteDeepMajorHardwareBuild,
  RestoreMajorHardwareBuild,
  GetRelatedRecordsMajorHardwareBuild,
} from "../Redux/Action/MajorHardwareBuild/MajorHardwareBuildDeleteAction";
import { GetMajorHardwareBuildEditResource } from "../Redux/Action/MajorHardwareBuild/MajorHardwareBuildEditAction";
import { GetMajorHardwareBuildReport } from "../Redux/Action/MajorHardwareBuild/MajorHardwareBuildDownloadAction";
import { useNavigate, useLocation } from "react-router-dom";
import setLoader from "../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";

import RefactorForeignIndexModal from "../screen/Shared/RefactorForeignIndexModal";

import { InitializeForeignIndex } from "../Hook/Common";
import { useAuth } from "../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { endGuideTour, startGuideTour } from "../Redux/Action/tourActions";
import { IoClose } from "react-icons/io5";
import { getHwBuildTourSteps } from "../Constant/TourSteps";
import TourGuide from "../Components/TourGuide";

export let paginationQuery: MajorHardwareBuildQueryObjectGrid = {
  majorHardwareBuildId: [],
  name: "",
  originalEquipmentManufacturer: [],
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  vulnerabilityStatus: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  buildConstruction: [],
  principalId: undefined,
  principalIdList: [],
  proprietaryHardware: [],
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
};

const SystemType: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [jumpStepIndex, setJumpStepIndex] = useState<number | null>(null);
  const majorHwBuildIndex = useSelector(
    (state: RootState) => state.tourStep.majorHwBuildIndex
  );
  const dispatch = useDispatch();
  const handleEndTour = () => dispatch(endGuideTour());
  const [tourFlag, setTourFlag] = useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const { RefactorUser, readonly, isPermesso, pageSize, tipologicaPermesso } =
    useAuth();
  //DTO
  const [data, setData] = useState<MajorHardwareBuildDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.majorHardwareBuildGridReducer.MajorHardwareBuildGridResult;
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

  const { darkMode } = useTheme();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetMajorHardwareBuildGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetMajorHardwareBuildGrid");
    closeModal();
    GetMajorHardwareBuildGrid(query).then(() =>
      setLoader("REMOVE", "GetMajorHardwareBuildGrid")
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
    MajorHardwareBuildDtoUpdate,
    MajorHardwareBuildDtoCreate
  >(
    GetMajorHardwareBuildCreateResource,
    GetMajorHardwareBuildEditResource,
    DeleteDeepMajorHardwareBuild,
    refresh,
    RestoreMajorHardwareBuild
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
      GetMajorHardwareBuildGrid(query).then(() => {
        setLoader("REMOVE", "GetMajorHardwareBuildGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetMajorHardwareBuildGrid");
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
        let copy = { ...query } as MajorHardwareBuildQueryObjectGrid;
        copy.majorHardwareBuildId = [];
        copy.majorHardwareBuildId?.push(localState?.id);
        copy.principalId = localState?.id;
        GetMajorHardwareBuildGrid(copy).then((x) =>
          setLoader("REMOVE", "GetMajorHardwareBuildGrid")
        );
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetMajorHardwareBuildGrid");
  }, []);
  useEffect(() => {
    if (majorHwBuildIndex !== null && majorHwBuildIndex >= 0) {
      localStorage.setItem("majorSwBuild", String(majorHwBuildIndex));
      setJumpStepIndex(majorHwBuildIndex);
      setTourFlag(true);
    } else {
      setJumpStepIndex(null);
    }
  }, [majorHwBuildIndex]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetMajorHardwareBuildGrid");
    }
  }, [GridDto]);

  const closeModalSetup = (changed: boolean) => {
    GetMajorHardwareBuildGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetMajorHardwareBuildReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsMajorHardwareBuild(id);
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
            <h4 className="">
              {edit == true ? "Edit Major HW Build" : "Add Major HW Build"}
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
          <MajorHardwareModal
            edit={edit}
            hardwareRedirect={redirect ?? false}
            prevPage={prevPage}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
          />
        </DialogContent>
      </Dialog>
      <Modal
        show={isVisibleModalRefactor}
        // backdrop="static"
        dialogClassName={"modal-dialog-centered"}
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0 ">
            <div className="col-12 ">
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
        // backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 ">Setup Grid Informations</h4>
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
          {redirect == true ? (
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
          <h3 className="voda-bold fz-28">Major Hardware Build</h3>
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
              id="hwBuild_addButton_tour"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New Major HW Build</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
            id="hwBuild_downloadButton_tour"
          >
            {/* <img src={require("../img/excel.png")} /> */}
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
                      getGrid: GetMajorHardwareBuildGrid,
                      setOrphanColor,
                    }}
                  ></AdditionalFiltersMenu>

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
      <MajorHardwareGrid
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
      ></MajorHardwareGrid>

      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          jumpStep={jumpStepIndex}
          tourSteps={getHwBuildTourSteps}
          page={"majorHwBuild"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default SystemType;
