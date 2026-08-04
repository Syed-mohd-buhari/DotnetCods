import React, { useEffect, useState } from "react";
import { Modal, Dropdown } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import Structure from "../screen/SystemType/SystemTypeGrid";
import ModalSystemType from "../screen/SystemType/SystemTypeModal";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";

import {
  SystemTypeDtoGrouped,
  SystemTypeQueryObjectGrid,
  SystemTypeDtoUpdate,
  SystemTypeDtoCreate,
} from "../Model/SystemTypeModel";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import { GetSystemTypeGrid } from "../Redux/Action/SystemType/SystemTypeGridAction";
import Paginate from "../Components/PaginationComponent";
import { GetSystemTypeCreateResource } from "../Redux/Action/SystemType/SystemTypeCreateAction";
import { GetSystemTypeEditResource } from "../Redux/Action/SystemType/SystemTypeEditAction";
import {
  DeleteDeepSystemType,
  GetRelatedRecordsSystemType,
  RestoreSystemType,
} from "../Redux/Action/SystemType/SystemTypeDeleteAction";
import { GetSystemTypeReport } from "../Redux/Action/SystemType/SystemTypeDownloadAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import setLoader from "../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import { CustomGridRender } from "../Model/Common";
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
import { useDispatch } from "react-redux";
import TourGuide from "../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../Redux/Action/tourActions";
import { getSystemTypeTourSteps } from "../Constant/TourSteps";

export let paginationQuery: SystemTypeQueryObjectGrid = {
  systemTypeId: [],
  systemTypeNameVodafone: [],
  systemTypeName3Gpp: [],
  systemTypeNameOem: [],
  majorSoftwareBuild: [],
  systemSolution: [],
  constraintScaling: undefined,
  constraintLcm: [],
  majorHardwareBuild: [],
  endOfMaintenance: undefined,
  lastModified: undefined,
  productImportance: [],
  verticalResponsible: [],
  subDomainResponsible: [],
  subDomainSpoc: [],
  assetCategory: [],
  assetClass: [],
  assetType: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  hardwareOem: [],
  softwareOem: [],
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const SystemType: React.FC = (props) => {
  //TABS
  const [key, setKey] = useState("Export");

  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [tourFlag, setTourFlag] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [jumpStepIndex, setJumpStepIndex] = useState<number | null>(null);
  const dispatch = useDispatch();
  const handleEndTour = () => dispatch(endGuideTour());

  const { readonly, isPermesso, tipologicaPermesso, pageSize } = useAuth();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const systemTypeIndex = useSelector(
    (state: RootState) => state.tourStep.systemTypeIndex
  );

  //DTO
  const [data, setData] = useState<SystemTypeDtoGrouped[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) => state.systemTypeGridReducer.SystemTypeGridResult
  );
  let externalRefreshDto = useSelector(
    (state: RootState) => state.externalRefreshReducer.refresh
  );

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetSystemTypeGrid(query).then(() => {
        setLoader("REMOVE", "GetSystemTypeGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetSystemTypeGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    closeModal();
    setLoader("ADD", "GetSystemTypeGrid");
    GetSystemTypeGrid(query).then(() =>
      setLoader("REMOVE", "GetSystemTypeGrid")
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
  } = useOperationTableCrud<SystemTypeDtoUpdate, SystemTypeDtoCreate>(
    GetSystemTypeCreateResource,
    GetSystemTypeEditResource,
    DeleteDeepSystemType,
    refresh,
    RestoreSystemType
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetSystemTypeGrid");
    }
  }, [GridDto]);

  const [prevPage, setPrevPage] = useState<string>();

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetSystemTypeGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        ids?: number[];
      };
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }

      if (localState.ids != undefined && localState.ids.length > 0) {
        let copy = { ...query } as SystemTypeQueryObjectGrid;
        copy.page = 1;
        copy.pageSize = 0;
        copy.sortBy = "keyGrouped";
        copy.isSortAscending = true;
        copy.systemTypeId = [] as number[];
        if (copy.systemTypeId != undefined) {
          localState.ids.map((x) => {
            copy.systemTypeId?.push(x);
          });
        }
        setRedirect(true);
        setFilterRedirect(true);
        setLocalState(localState);
        setLoader("REMOVE", "GetSystemTypeGrid");
        return;
      }
      if (localState?.id != null) {
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as SystemTypeQueryObjectGrid;
        copy.systemTypeId = [];
        copy.systemTypeId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
    } else {
      // GetSystemTypeGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetSystemTypeGrid")
      // );
    }
    setLoader("REMOVE", "GetSystemTypeGrid");
  }, []);
  useEffect(() => {
    if (systemTypeIndex !== null && systemTypeIndex >= 0) {
      localStorage.setItem("systemType", String(systemTypeIndex));
      setJumpStepIndex(systemTypeIndex);
      setTourFlag(true);
    } else {
      setJumpStepIndex(null);
    }
  }, [systemTypeIndex]);

  const closeModalSetup = (changed: boolean) => {
    GetSystemTypeGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetSystemTypeReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSystemType(id);
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
            <h4>{edit === true ? "Edit System Type" : "Add System Type"}</h4>
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
          <ModalSystemType
            edit={edit}
            systemTypeRedirect={redirect ?? false}
            prevPage={prevPage}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
          />
        </DialogContent>
      </Dialog>
      <Modal
        show={isVisibleModalSetup}
        backdrop="static"
        keyboard={false}
        size="lg"
        onHide={closeModal}
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
            tab={key}
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
          <h3 className="voda-bold">System Type</h3>
          {redirect === true && filterRedirect == true ? (
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
              id="systemType_addButton_tour"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New System Type</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
            id="systemType_downloadButton_tour"
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>

          <Dropdown className="d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
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
                      getGrid: GetSystemTypeGrid,
                      setOrphanColor,
                    }}
                  ></AdditionalFiltersMenu>
                </>
              )}

              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <Structure
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          onDelete,
          Edit,
          Filter: setQuery,
          Restore,
          setIsFiltriAttivati,
        }}
        readonly={readonly}
      ></Structure>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{
          next,
          back,
        }}
      />
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          jumpStep={jumpStepIndex}
          tourSteps={getSystemTypeTourSteps}
          page={"systemType"}
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
