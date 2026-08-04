import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { Alert, Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import SetupColumns from "../screen/Shared/SetupColumns";
import setLoader from "../Redux/Action/LoaderAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import { GetTestInfoReport } from "../Redux/Action/TestInfo/TestInfoDownloadAction";
import { GetTestInfoGrid } from "../Redux/Action/TestInfo/TestInfoGridAction";
import { GetNFVISwCompatibleCreateResource } from "../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleCreateAction";
import {
  DeleteDeepTestInfo,
  RestoreTestInfo,
  deleteTestInfo,
} from "../Redux/Action/TestInfo/TestInfoDeleteAction";
import {
  EditNFVISwCompatible,
  GetNFVISwCompatibleEditResource,
} from "../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleEditAction";
import NFVISoftwareCompatibleModal from "../screen/NFVISoftwareCompatible/NFVISoftwareCompatibleModal";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import {
  NFVISwCompatibleDtoCreate,
  NFVISwCompatibleDtoGrid,
  NFVISwCompatibleDtoUpdate,
  NFVISwCompatibleQueryObjectGrid,
} from "../Model/NFVISoftwareCompatible";
import NFVISoftwareCompatibleGrid from "../screen/NFVISoftwareCompatible/NFVISoftwareCompatibleGrid";
import { GetNFVISwCompatibleGrid } from "../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleGridAction";
import { GetNFVISwCompatibleReport } from "../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleDownloadAction";
import { deleteNFVISwCompatible } from "../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleDeleteAction";
import { handleImportFile } from "../Hook/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";

export let paginationQuery: NFVISwCompatibleQueryObjectGrid = {
  nfviSoftwareCompatibilityId: [],
  minimumSupportedVersion: [],
  plaftFormVersion: [],
  productName: [],
  vendor: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModified: undefined,
  lastModifiedBy: [],
};

const NFVISoftwareCompatible = () => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isViewVisibleModal, setIsViewVisibleModal] = useState(false);
  const [isAddEnable, setIsAddEnable] = useState(false);
  const [isViewEnable, setIsViewEnable] = useState(false);
  const [viewUserInfo, setViewUserInfo] = useState<any>();
  const [orphanColor, setOrphanColor] = useState(false);
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  //DTO
  const [data, setData] = useState<NFVISwCompatibleDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) =>
      state.NFVISwCompatibleGridReducer.NFVISwCompatibleGridResult
  );
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.testInfoCreateReducer.ResultDtoCreate
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, tipologicaPermesso, pageSize } = useAuth();
  const [renderGridState, setRenderGridState] = useState<any>();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const { darkMode } = useTheme();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetNFVISwCompatibleGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    closeModal();
    GetNFVISwCompatibleGrid(query);
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
  } = useOperationTableCrud<
    NFVISwCompatibleDtoUpdate,
    NFVISwCompatibleDtoCreate
  >(
    GetNFVISwCompatibleCreateResource,
    GetNFVISwCompatibleEditResource,
    deleteNFVISwCompatible,
    refresh
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
      setLoader("REMOVE", "GetNFVISwCompatibleGrid");
    }
  }, [GridDto]);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetNFVISwCompatibleGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        ids?: number[];
      };

      if (localState?.id != null) {
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as NFVISwCompatibleQueryObjectGrid;
        copy.principalId = localState?.id;
        setQuery(copy);
        GetNFVISwCompatibleGrid(copy).then((x) =>
          setLoader("REMOVE", "GetNFVISwCompatibleGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetNFVISwCompatibleGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetNFVISwCompatibleGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetNFVISwCompatibleReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const fileHandle = async () => {
    try {
      const sheetName = "NFVI Software Compatibility";
      const apiPath = "nfvisoftwarecompatibility";
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
        setLoader("ADD", "GetNFVISwCompatibleGrid");
        GetNFVISwCompatibleGrid(query).then(() =>
          setLoader("REMOVE", "GetNFVISwCompatibleGrid")
        );
      }
    } catch (error) {
      console.log(error);
    }
  };
  //   const onDelete = async (id: number) => {
  //     const result = await GetRelatedRecordsTestInfo(id);
  //     if (result.data != null) {
  //       setIsVisibleModalRelated(true);
  //       setRelatedRecord(result.data);
  //     } else {
  //       Delete(id);
  //     }
  //   };

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      <ModalConfirm data={myConfirm} />
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
                ? "Edit NFVI Software Compatibility"
                : "Add NFVI Software Compatibility"}
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
          <NFVISoftwareCompatibleModal
            edit={edit}
            keyTab={localStateHistory?.tab}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
      </Dialog>
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
          <h3 className="voda-bold">NFVI Software Compatibility</h3>
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
              <span className="fz-14">Add New Record</span>
            </button>
          )}
          {!readonly && (
            <button
              className="download-to-excel mrl-3 grid-main-btn"
              onClick={() => fileHandle()}
            >
              Import Excel
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn
            "
            onClick={() => InvocheDownload()}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown
            className="d-inline more-options grid-main-btn
"
          >
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu
              className="grid-main-btn
"
            >
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
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
          ></SetupColumns>
        </Modal.Body>
      </Modal>
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
      <NFVISoftwareCompatibleGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          Delete,
          Edit,
          Filter: setQuery,
        }}
      ></NFVISoftwareCompatibleGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default NFVISoftwareCompatible;
