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
import { GetOrganizationInfoReport } from "../Redux/Action/OrganizationInfo/OrganizationInfoDownloadAction";
import { GetOrganizationInfoCreateResource } from "../Redux/Action/OrganizationInfo/OrganizationInfoCreateAction";
import {
  DeleteDeepOrganizationInfo,
  GetRelatedRecordsOrganizationInfo,
  RestoreOrganizationInfo,
  deleteOrganizationInfo,
} from "../Redux/Action/OrganizationInfo/OrganizationInfoDeleteAction";
import {
  EditOrganizationInfo,
  GetOrganizationInfoEditResource,
} from "../Redux/Action/OrganizationInfo/OrganizationInfoEditAction";
import OrganizationInfoModal from "../screen/OrganizationInfo/OrganizationInfoModal";
import OrganizationInfoGrid from "../screen/OrganizationInfo/OrganizationInfoGrid";
import {
  OrganizationInfoDtoCreate,
  OrganizationInfoDtoGrid,
  OrganizationInfoDtoUpdate,
  OrganizationInfoQueryObjectGrid,
} from "../Model/OrganizationInfo";
import { GetOrganizationInfoGrid } from "../Redux/Action/OrganizationInfo/OrganizationInfoGridAction";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: OrganizationInfoQueryObjectGrid = {
  systemVerificationProblemId: [],
  contactId: [],
  opCoId: [],
  subPracticeContactId: [],
  organisationId: [],
  headOfOrganisationId: [],
  mainOrganisation: [],
  headOfOrganisation: [],
  subdomainResponsibleId: [],
  opCo: [],
  verticalResponsibleId: [],
  solutionDescription: [],
  contact: [],
  subdomainResponsible: [],
  subPractice: [],
  subPracticeContact: [],
  verticalResponsible: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const OrganizationInfo = () => {
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
  const [data, setData] = useState<OrganizationInfoDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) =>
      state.organizationInfoGridReducer.OrganizationInfoGridResult
  );
  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.organizationInfoCreateReducer.ResultDtoCreate
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, pageSize } = useAuth();
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
    isPermesso ? GetOrganizationInfoGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    closeModal();
    GetOrganizationInfoGrid(query);
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
    OrganizationInfoDtoUpdate,
    OrganizationInfoDtoCreate
  >(
    GetOrganizationInfoCreateResource,
    GetOrganizationInfoEditResource,
    deleteOrganizationInfo,
    refresh,
    RestoreOrganizationInfo
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
      setLoader("REMOVE", "GetOrganizationInfoGrid");
    }
  }, [GridDto]);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetOrganizationInfoGrid");
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
        let copy = { ...query } as OrganizationInfoQueryObjectGrid;
        copy.principalId = localState?.id;
        setQuery(copy);
        GetOrganizationInfoGrid(copy).then((x) =>
          setLoader("REMOVE", "GetOrganizationInfoGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetOrganizationInfoGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetOrganizationInfoGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetOrganizationInfoReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsOrganizationInfo(id);
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
      <ModalConfirm data={myConfirm} />

      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
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
                ? "Edit Organization Info"
                : "Add Organization Info"}
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
          <OrganizationInfoModal
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
          <h3 className="voda-bold">Organization Info</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {!readonly && (
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
      <OrganizationInfoGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          onDelete,
          Edit,
          Filter: setQuery,
        }}
      ></OrganizationInfoGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default OrganizationInfo;
