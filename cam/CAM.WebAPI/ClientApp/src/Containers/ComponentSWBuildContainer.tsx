import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { CustomGridRender } from "../Model/Common";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { GetComponentSwBuildGrid } from "../Redux/Action/ComponentSwBuild/ComponentSwBuildGridAction";
import {
  ComponentSwBuildDtoCreate,
  ComponentSwBuildQueryObjectGrid,
  ComponentSwBuildDtoGrid,
  ComponentSwBuildDtoUpdate,
  ComponentSwBuildToCloneDto,
} from "../Model/ComponentSwBuild";
import ComponentSwGrid from "../screen/ComponentSwBuild/ComponentSwGrid";
import ComponentSwModal from "../screen/ComponentSwBuild/ComponentSwModal";
import { GetComponentSwBuildCreateResource } from "../Redux/Action/ComponentSwBuild/ComponentSwBuildCreateAction";
import {
  DeleteDeepComponentSwBuild,
  GetRelatedRecordsComponentSwBuild,
  RestoreComponentSwBuild,
  deleteComponentSwBuild,
} from "../Redux/Action/ComponentSwBuild/ComponentSwBuildDeleteAction";
import { GetComponentSwBuildEditResource } from "../Redux/Action/ComponentSwBuild/ComponentSwBuildEditAction";
import { GetComponentSwBuildDownload } from "../Redux/Action/ComponentSwBuild/ComponentSwBuildDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { GetComponentSwToClone } from "../Redux/Action/ComponentSwBuild/ComponentSwBuildCommonAction";
import ComponentSwModalUpgrade from "../screen/ComponentSwBuild/ComponentSwVersionModal";
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

export let paginationQuery: ComponentSwBuildQueryObjectGrid = {
  originalEquipmentManufacturer: [],
  softwareVersion: [],
  productName: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenanceValue: undefined,
  endOfsupportValue: undefined,
  generaAvailableDateValue: undefined,
  deliveryMethod: [],
  vulnerabilityStatus: [],
  operatingSystem: [],
  spareFieldsJson: [],
  networkFunction: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

interface Props {
  redirect?: string;
  modal?: {
    isModal: boolean | false;
    setIsComponentModalFlag(flag: boolean): any;
  };
  returnObject?(data: Array<any>): any;
}

const ComponentSWBuild = (props: Props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);

  const { darkMode } = useTheme();

  //DTO
  const [data, setData] = useState<ComponentSwBuildDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.componentSwBuildGridReducer.ComponentSwBuildGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetComponentSwBuildGrid(query).then(() => {
        setLoader("REMOVE", "GetComponentSwBuildGrid");
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
    isPermesso ? GetComponentSwBuildGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetComponentSwBuildGrid");
    closeModal();
    GetComponentSwBuildGrid(query).then(() =>
      setLoader("REMOVE", "GetComponentSwBuildGrid")
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
    ComponentSwBuildDtoUpdate,
    ComponentSwBuildDtoCreate
  >(
    GetComponentSwBuildCreateResource,
    GetComponentSwBuildEditResource,
    deleteComponentSwBuild,
    refresh,
    RestoreComponentSwBuild
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetComponentSwBuildGrid");
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
        let copy = { ...query } as ComponentSwBuildQueryObjectGrid;
        // copy.majorSoftwareBuildId = [];
        // copy.majorSoftwareBuildId?.push(localState?.id);
        copy.principalId = localState?.id;
        // GetComponentSwBuildGrid(copy).then((x) => {
        //   setLoader("REMOVE", "GetComponentSwBuildGrid");
        // });
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      setLoader("REMOVE", "GetComponentSwBuildGrid");
      // GetComponentSwBuildGrid(paginationQuery).then((x) => {
      //   setLoader("REMOVE", "GetComponentSwBuildGrid");
      // });
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetComponentSwBuildGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetComponentSwBuildGrid");
    }
  }, [GridDto]);

  const InvocheDownload = async () => {
    let result = await GetComponentSwBuildDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const [prevPage, setPrevPage] = useState<string>();

  const [dataToClone, setDataToClone] = useState<ComponentSwBuildToCloneDto>();

  const GetComponentSwCloneData = async (id: number) => {
    await GetComponentSwToClone(id).then((x) => {
      if (!x?.warning) {
        setDataToClone(x?.data);
        setIsVisibleModalUpgrade(true);
      }
    });
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsComponentSwBuild(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const closeRefillModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsComponentModalFlag(false);
  };

  return (
    <div
      className={`${
        props?.redirect === "bagScreen" ? "mt-3" : "pageContainer"
      }`}
    >
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
              {edit == true ? "Edit Software Component Bag" : "New Component"}
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
          <ComponentSwModal
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
            <h4>Upgrade Component Software Version</h4>
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
          <ComponentSwModalUpgrade
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
          {redirect == true ? (
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
          <h3 className="voda-bold">Component Software</h3>
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
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New Component</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
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
                      getGrid: GetComponentSwBuildGrid,
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
      <ComponentSwGrid
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
          GetComponentSwCloneData,
        }}
        readonly={readonly}
      ></ComponentSwGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end d-flex footerModal">
          {/* <button className="  voda-bold btn btn-link px-4 btnHeader cancel" type="button">Close</button> */}
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => closeRefillModal()}
          >
            Close
          </button>
        </div>
      ) : null}
    </div>
  );
};

export default ComponentSWBuild;
