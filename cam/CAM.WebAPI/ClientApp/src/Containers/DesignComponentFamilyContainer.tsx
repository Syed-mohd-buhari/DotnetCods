import React, { useEffect, useState } from "react";
import { Dropdown, Modal, Pagination } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { InitializeForeignIndex } from "../Hook/Common";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import {
  DesignComponentFamilyDtoCreate,
  DesignComponentFamilyDtoGrid,
  DesignComponentFamilyDtoUpdate,
  DesignComponentFamilyQueryObjectGrid,
} from "../Model/DesignComponentFamily";
import { GetDesignComponentFamilyCreateResource } from "../Redux/Action/DesignComponentFamily/DesignComponentFamilyCreateAction";
import {
  DeleteDeepDesignComponentFamily,
  GetRelatedRecordsDesignComponentFamily,
} from "../Redux/Action/DesignComponentFamily/DesignComponentFamilyDeleteAction";
import { GetDesignComponentFamilyReport } from "../Redux/Action/DesignComponentFamily/DesignComponentFamilyDownloadAction";
import { GetDesignComponentFamilyEditResource } from "../Redux/Action/DesignComponentFamily/DesignComponentFamilyEditAction";
import { GetDesignComponentFamilyGrid } from "../Redux/Action/DesignComponentFamily/DesignComponentFamilyGridAction";
import setLoader from "../Redux/Action/LoaderAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import DesignComponentFamilyGrid from "../screen/DesignComponentFamily/DesignComponentFamilyGrids";
import ModalDesignComponentFamily from "../screen/DesignComponentFamily/DesignComponentFamilyModal";
import RefactorForeignIndexModal from "../screen/Shared/RefactorForeignIndexModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";

export let paginationQuery: DesignComponentFamilyQueryObjectGrid = {
  serviceBoundary: [],
  gdprRelevant: [],
  criticality: [],
  implementation: [],
  internetFacing: [],
  systemIsShared: [],
  systemTypeIdentityName: [],
  systemFunction: [],
  description: [],
  sharingType: [],
  lastModified: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const DesignComponentFamily: React.FC = (props) => {
  //TABS
  const [key, setKey] = useState("structure");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);

  //DTO
  const [data, setData] = useState<DesignComponentFamilyDtoGrid[] | undefined>(
    []
  );
  const Grid = (state: RootState) =>
    state.designComponentFamilyGridReducer.DesignComponentFamilyGridResult;
  const GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  const { darkMode } = useTheme();

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetDesignComponentFamilyGrid(query).then(() => {
        setLoader("REMOVE", "GetDesignComponentFamilyGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const [orphanColor, setOrphanColor] = useState(false);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  const { RefactorUser, readonly, isPermesso, pageSize } = useAuth();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [editIS, setEditIS] = useState<number>();

  const refresh = () => {
    closeModal();
    GetDesignComponentFamilyGrid(query);
  };

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetDesignComponentFamilyGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
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
    DesignComponentFamilyDtoUpdate,
    DesignComponentFamilyDtoCreate
  >(
    GetDesignComponentFamilyCreateResource,
    GetDesignComponentFamilyEditResource,
    DeleteDeepDesignComponentFamily,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (location.state != null && location.state != undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        ids?: number[];
      };
      if (localState.ids != undefined && localState.ids.length > 0) {
        let copy = { ...query } as DesignComponentFamilyQueryObjectGrid;
        copy.page = 1;
        copy.pageSize = 0;
        copy.designComponentFamilyId = [] as number[];
        if (copy.designComponentFamilyId != undefined) {
          localState.ids.map((x) => {
            copy.designComponentFamilyId?.push(x);
          });
        }
        setRedirect(true);
        setFilterRedirect(true);
        setLocalState(localState);
        return;
      }
      if (localState?.id != null) {
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);

        let copy = { ...query } as DesignComponentFamilyQueryObjectGrid;
        copy.designComponentFamilyId = [];
        copy.designComponentFamilyId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetDesignComponentFamilyGrid(paginationQuery);
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetDesignComponentFamilyGrid(query).then((x) =>
      setIsVisibleModalSetup(false)
    );
  };

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const onOpenImplementationPopUp = (id?: number) => {
    setEditIS(id);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  const InvocheDownload = async () => {
    let result = await GetDesignComponentFamilyReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsDesignComponentFamily(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
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
            <h4>
              {edit === true
                ? "Edit Design Component Family"
                : "New design Component Family"}
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
          <ModalDesignComponentFamily
            keyTab={localStateHistory?.tab}
            edit={edit}
            dcfRedirect={redirect ?? false}
            prevPage={prevPage}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
            onOpenImplementationPopUp={onOpenImplementationPopUp}
          />
        </DialogContent>
      </Dialog>

      <Modal
        show={isVisibleModalRefactor}
        backdrop="static"
        keyboard={false}
        size="xl"
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
            entityType={3}
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
            {/* <ErrorNotification OnModal={true} /> */}
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
          {redirect === true ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: localStateHistory?.prevPage }}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">Design Component Family</h3>
          {redirect === true && filterRedirect === true ? (
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
          <Dropdown className="d-inline mrl-10 more-options grid-main-btn">
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
                      getGrid: GetDesignComponentFamilyGrid,
                      setOrphanColor,
                    }}
                  ></AdditionalFiltersMenu>
                </>
              )}

              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
              {/* {!readonly && (
                <Dropdown.Item
                  onClick={() =>
                    InitializeForeignIndex(setIsVisibleModalRefactor)
                  }
                >
                  Initialize Foreign Index
                </Dropdown.Item>
              )} */}
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <div className="">
        <DesignComponentFamilyGrid
          data={data}
          pagination={query}
          orphanColor={orphanColor}
          renderGrid={renderGridState?.render ?? []}
          editIS={editIS}
          onOpenImplementationPopUp={onOpenImplementationPopUp}
          action={{
            onDelete,
            Edit,
            Filter: setQuery,
            Restore,
            setIsFiltriAttivati,
          }}
          readonly={readonly}
        ></DesignComponentFamilyGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </div>
  );
};

export default DesignComponentFamily;
