import React, { useEffect, useState } from "react";
import { Alert, Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";

import { CustomGridRender } from "../Model/Common";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { GetIdentityAsIsGrid } from "../Redux/Action/IdentityAsIs/IdentityAsIsGridAction";
import {
  IdentityAsIsDtoGrid,
  IdentityAsIsDtoUpdate,
  IdentityAsIsDtoCreate,
  IdentityAsIsQueryObjectGrid,
} from "../Model/LookUp/Identities";
import IdentityAsIsGrid from "../screen/IdentityAsIs/IdentityAsIsGrid";
import IdentitiesModal from "../screen/IdentityAsIs/IdentitiesModal";
import { GetIdentityAsIsCreateResource } from "../Redux/Action/IdentityAsIs/IdentityAsIsCreateAction";
import {
  DeleteDeepIdentityAsIs,
  RestoreIdentityAsIs,
} from "../Redux/Action/IdentityAsIs/IdentityAsIsDeleteAction";
import { GetIdentityAsIsEditResource } from "../Redux/Action/IdentityAsIs/IdentityAsIsEditAction";
import { GetIdentityAsIsReport } from "../Redux/Action/IdentityAsIs/IdentityAsIsDownloadAction";
import { useNavigate, useLocation } from "react-router-dom";
import setLoader from "../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { RelatedRecordsResultDto } from "../Model/CommonModels";

import RefactorForeignIndexModal from "../screen/Shared/RefactorForeignIndexModal";

import { useAuth } from "../Hook/useAuth";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: IdentityAsIsQueryObjectGrid = {
  id: [],
  value: "",
  lastModified: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  previousResourceKey: "",
  categoryId: undefined,
  resourceKey: "",
  typeId: undefined,
  categoryDescription: "",
  classDescription: "",
  typeDescription: "",
  opcoId: undefined,
  dcfId: undefined,
  designComponentFamily: [],
};

const Identities: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const { RefactorUser, readonly, tipologicaPermesso, isPermesso, pageSize } =
    useAuth();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  //DTO
  const [data, setData] = useState<IdentityAsIsDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.IdentityAsIsGridReducer.IdentityAsIsGridResult;
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

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetIdentityAsIsGrid : undefined
  );

  const goBack = () => {
    navigate(-1); // Navigate back one step
  };

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetIdentityAsIsGrid");
    closeModal();
    GetIdentityAsIsGrid(query).then(() =>
      setLoader("REMOVE", "GetIdentityAsIsGrid")
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
  } = useOperationTableCrud<IdentityAsIsDtoUpdate, IdentityAsIsDtoCreate>(
    GetIdentityAsIsCreateResource,
    GetIdentityAsIsEditResource,
    DeleteDeepIdentityAsIs,
    refresh,
    RestoreIdentityAsIs
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
      GetIdentityAsIsGrid(query).then(() => {
        setLoader("REMOVE", "GetIdentityAsIsGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetIdentityAsIsGrid");
    }
  }, [GridDto]);

  const closeModalSetup = (changed: boolean) => {
    GetIdentityAsIsGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const fileHandle = async () => {
    try {
      const sheetName = "Identities";
      const apiPath = "IdentityAsIs";
      const status = await handleImportFile(sheetName, apiPath);
      if (status && status?.["warning"] === true) {
        rootStore.dispatch(
          setNotification({
            message: status?.["info"],
            notifyType: NotifyType.success,
          })
        );
        refresh();
        setShow(false);
      } else if (status && status?.["warning"] === false) {
        setAlertStatus({
          message: status?.["info"],
          class: "danger",
        });
        setShow(true);
      }
    } catch (error) {
      console.log(error);
    }
  };

  const InvocheDownload = async () => {
    let result = await GetIdentityAsIsReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    // const result = await GetRelatedRecordsIdentityAsIs(id);
    // if (result.data != null) {
    //   setIsVisibleModalRelated(true);
    //   setRelatedRecord(result.data);
    // } else {
    //   Delete(id);
    // }
    Delete(id);
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
            <h4 className="pd-15">
              {edit == true ? "Edit Identity" : "Add Identity"}
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
          <IdentitiesModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh, Edit }}
            wizardMode={false}
          />
        </DialogContent>
      </Dialog>
      <Modal
        show={isVisibleModalRefactor}
        backdrop="static"
        dialogClassName={"modal-dialog-centered"}
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
            entityType={1}
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
          <h3 className="voda-bold fz-28">Identities</h3>
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
              <span className="fz-14"> New Identity</span>
            </button>
          )}
          {tipologicaPermesso && (
            <button
              className="download-to-excel ml-3 grid-main-btn"
              onClick={() => fileHandle()}
            >
              Import Excel
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
      <IdentityAsIsGrid
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
      ></IdentityAsIsGrid>

      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default Identities;
