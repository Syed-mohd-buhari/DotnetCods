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
import { RootState } from "../Redux/Store/rootStore";
import {
  TestInfoDtoCreate,
  TestInfoDtoGrid,
  TestInfoQueryObjectGrid,
  TestInfoDtoUpdate,
} from "../Model/TestInfo";
import TestInfoGrid from "../screen/TestInfo/TestInfoGrid";
import { GetTestInfoReport } from "../Redux/Action/TestInfo/TestInfoDownloadAction";
import { GetTestInfoGrid } from "../Redux/Action/TestInfo/TestInfoGridAction";
import { GetTestInfoCreateResource } from "../Redux/Action/TestInfo/TestInfoCreateAction";
import {
  DeleteDeepTestInfo,
  RestoreTestInfo,
  deleteTestInfo,
} from "../Redux/Action/TestInfo/TestInfoDeleteAction";
import {
  EditTestInfo,
  GetTestInfoEditResource,
} from "../Redux/Action/TestInfo/TestInfoEditAction";
import TestInfoModal from "../screen/TestInfo/TestInfoModal";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: TestInfoQueryObjectGrid = {
  systemVerificationProblemId: [],
  problemId: [],
  opCoId: [],
  systemTypeId: [],
  environmentId: [],
  dateFound: undefined,
  problemCategoryId: [],
  problemDescription: [],
  maintenanceReference: [],
  severity: [],
  statusUrl: [],
  testReport: [],
  standardNir: [],
  ericssonSecReport: [],
  swAndStEntries: [],
  penTestingReport: [],
  mitigation: [],
  solutionDescription: [],
  patchReference: [],
  productUpgradeReference: [],
  suppleMental: [],
  vendorCsr: [],
  subNetwork: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const TestInfo = () => {
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
  const [data, setData] = useState<TestInfoDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) => state.testInfoGridReducer.TestInfoGridResult
  );
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.testInfoCreateReducer.ResultDtoCreate
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
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
    isPermesso ? GetTestInfoGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    closeModal();
    GetTestInfoGrid(query);
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
  } = useOperationTableCrud<TestInfoDtoUpdate, TestInfoDtoCreate>(
    GetTestInfoCreateResource,
    GetTestInfoEditResource,
    deleteTestInfo,
    refresh,
    RestoreTestInfo
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
      setLoader("REMOVE", "GetTestInfoGrid");
    }
  }, [GridDto]);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetTestInfoGrid");
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
        let copy = { ...query } as TestInfoQueryObjectGrid;
        copy.principalId = localState?.id;
        setQuery(copy);
        GetTestInfoGrid(copy).then((x) =>
          setLoader("REMOVE", "GetTestInfoGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetTestInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetTestInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetTestInfoReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
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
                ? "Edit System Verification Problem"
                : "Add System Verification Problem"}
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
          <TestInfoModal
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
          <h3 className="voda-bold">System Verification Problems</h3>
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
              <span className="fz-14">Add New Info</span>
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
      <TestInfoGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          Delete,
          Edit,
          Filter: setQuery,
        }}
      ></TestInfoGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default TestInfo;
