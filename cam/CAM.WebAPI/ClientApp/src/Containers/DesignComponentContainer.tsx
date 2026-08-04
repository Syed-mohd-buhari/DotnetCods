import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import DesignComponentGrid from "../screen/DesignComponent/DesignComponentGrids";
import ModalDesignComponent from "../screen/DesignComponent/DesignComponentModal";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import {
  DesignComponentDtoGrid,
  DesignComponentQueryObjectGrid,
  DesignComponentDtoUpdate,
  DesignComponentDtoCreate,
} from "../Model/DesignComponent";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import { GetDesignComponentGrid } from "../Redux/Action/DesignComponent/DesignComponentGridAction";
import Paginate from "../Components/PaginationComponent";
import { GetDesignComponentCreateResource } from "../Redux/Action/DesignComponent/DesignComponentCreateAction";
import { GetDesignComponentEditResource } from "../Redux/Action/DesignComponent/DesignComponentEditAction";
import {
  DeleteDeepDesignComponent,
  GetRelatedRecordsDesignComponent,
  RestoreDesignComponent,
} from "../Redux/Action/DesignComponent/DesignComponentDeleteAction";
import { GetDesignComponentReport } from "../Redux/Action/DesignComponent/DesignComponentDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
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
import MUIPaginationComponent from "../Components/MUIPaginationComponent";

export let paginationQuery: DesignComponentQueryObjectGrid = {
  designComponentId: [],
  systemType: [],
  designComponentFamily: [],
  designComponentFamilyId: [],
  equipmentManufacturer: [],
  productName: [],
  softwareReleaseNumber: [],
  hardwarePlatform: [],
  serviceBoundary: [],
  hardwareType: [],
  systemTypeIdBasedVerticalValue: [],
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

const DesignComponent: React.FC = (props) => {
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  //TABS
  const [key, setKey] = useState("structure");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);

  //DTO
  const [data, setData] = useState<DesignComponentDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.designComponentGridReducer.DesignComponentGridResult;
  const GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  const { darkMode } = useTheme();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetDesignComponentGrid(query).then(() => {
        setLoader("REMOVE", "GetDesignComponentGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const [orphanColor, setOrphanColor] = useState(false);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetDesignComponentGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetDesignComponentGrid : undefined
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
  } = useOperationTableCrud<DesignComponentDtoUpdate, DesignComponentDtoCreate>(
    GetDesignComponentCreateResource,
    GetDesignComponentEditResource,
    DeleteDeepDesignComponent,
    refresh,
    RestoreDesignComponent
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
        let copy = { ...query } as DesignComponentQueryObjectGrid;
        copy.page = 1;
        copy.pageSize = 0;
        copy.designComponentId = [] as number[];
        if (copy.designComponentId != undefined) {
          localState.ids.map((x) => {
            copy.designComponentId?.push(x);
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

        let copy = { ...query } as DesignComponentQueryObjectGrid;
        copy.systemType = [];
        copy.systemType?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetDesignComponentGrid(paginationQuery);
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetDesignComponentGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
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
    let result = await GetDesignComponentReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsDesignComponent(id);
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
              {edit === true ? "Edit Design Component" : "New Design Component"}
            </h4>
            {/* <ErrorNotification OnModal={true} /> */}
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
          <ModalDesignComponent
            keyTab={localStateHistory?.tab}
            edit={edit}
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
          <h3 className="voda-bold">Design Component</h3>
          {redirect === true && filterRedirect === true ? (
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
              <span className="fz-14">New Design Component</span>
            </button>
          )}

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
              {!readonly && (
                <>
                  <Dropdown.Item>Preview Orphans</Dropdown.Item>
                  <AdditionalFiltersMenu
                    query={query}
                    orphanColored={orphanColor}
                    action={{
                      setQuery: setQuery,
                      setIsVisible: setIsVisibleAdditionalFilter,
                      getGrid: GetDesignComponentGrid,
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

      <div className="">
        <DesignComponentGrid
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
        ></DesignComponentGrid>
        <MUIPaginationComponent
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back, updatePageSize }}
        />
      </div>
    </div>
  );
};

export default DesignComponent;
