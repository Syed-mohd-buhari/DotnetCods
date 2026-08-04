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
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import {
  ServiceLevelPlanDtoUpdate,
  ServicePlanDtoCreate,
  ServicePlanDtoGrid,
  ServicePlanQueryObjectGrid,
} from "../Model/ServicePlan";
import { GetServicePlanGrid } from "../Redux/Action/ServicePlan/ServicePlanGridAction";
import ServiceLevelGrid from "../screen/ServiceLevelPA/ServiceLevelGrid";
import {
  DesignAspectDtoUpdate,
  DesignAspectQueryObjectGrid,
} from "../Model/DesignAspects";
import { GetDesignAspectEditResource } from "../Redux/Action/DesignAspect/DesignAspectEditAction";
import { GetServicePlanCreateResource } from "../Redux/Action/ServicePlan/ServicePlanCreateAction";
import { DeleteDeepServicePlan } from "../Redux/Action/ServicePlan/ServicePlanDeleteAction";
import { GetServicePlanReport } from "../Redux/Action/ServicePlan/ServicePlanDownloadAction";
import ServiceLevelPaModal from "../screen/ServiceLevelPA/ServiceLevelPaModal";
import { ApiCallWithErrorHandling } from "../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../Business/DesignAspectsBusiness";
import ServiceLevelPlannedModal from "../screen/ServiceLevelPA/ServiceLevelPlannedModal";
import ServiceLevelPAModal from "../screen/ServiceLevelPA/ServiceLevelPaModal";
import { GetServicePlanEditResource } from "../Redux/Action/ServicePlan/ServicePlanEditAction";
import ServiceLevelPlannedActivityModal from "../screen/ServiceLevelPA/ServiceLevelPlannedActivityModal";

export let paginationQuery: ServicePlanQueryObjectGrid = {
  opCoId: [],
  serviceMasterId: [],
  servicePlanId: [],
  dcfId: [],
  program: [],
  status: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const ServiceLevelContainer = () => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [dcfId, setDcfId] = useState<number>();
  const [dcfName, setDcfName] = useState<string>("");
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  //DTO
  const [data, setData] = useState<ServicePlanDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) => state.servicePlanGridReducer.LookUpGridResult
  );
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  const [renderGridState, setRenderGridState] = useState<any>();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const { darkMode } = useTheme();

  useEffect(() => {
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetServicePlanGrid : undefined
  );
  const getDCFName = async (dcfId: any) => {
    let api = new DesignAspectApi();

    let DCFName = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.getDCFName(dcfId)
    );

    setDcfName(DCFName?.data?.designComponentName!);
  };

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  const refresh = () => {
    setLoader("ADD", "GetServicePlanGrid");
    closeModal();
    GetServicePlanGrid(query).then(() =>
      setLoader("REMOVE", "GetServicePlanGrid")
    );
    setLoader("REMOVE", "GetServicePlanGrid");
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
    ServiceLevelPlanDtoUpdate,
    ServiceLevelPlanDtoUpdate
  >(
    GetServicePlanCreateResource,
    GetServicePlanEditResource,
    DeleteDeepServicePlan,
    refresh
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const openedArchived = () => {
    navigate(
      {
        pathname: "/plannedActivities",
        search: "typePA=archived",
      },
      {
        state: {
          filter: "ServiceLevel",
          prevPage: prevPage,
        },
      }
    );
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (location.state !== null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        dcfId: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };

      if (localState.prevPage && localState.prevPage !== "") {
        setPrevPage(localState.prevPage);
      }

      if (localState?.id) {
        setLocalState(localState);
        setRedirect(true);
        setFilterRedirect(true);

        let copy = { ...query } as DesignAspectQueryObjectGrid;
        copy.designAspectId = [];
        copy.designAspectId.push(localState?.id);
        copy.principalId = localState?.id;

        setQuery(copy);
        Edit(localState?.id);
      }

      if (localState.dcfId) {
        setLocalState(localState);
        setRedirect(true);
        setFilterRedirect(true);
        setDcfId(localState.dcfId);
        getDCFName(localState.dcfId);
        let copy = { ...query } as DesignAspectQueryObjectGrid;
        copy.designComponentFamilyName = [];
        copy.designComponentFamilyName.push(localState.dcfId);
        copy.principalId = localState.dcfId;

        setTimeout(() => {
          setQuery(copy);
        }, 500);
      }
    } else {
      setLocalState(undefined);
      setRedirect(false);
      setFilterRedirect(false);
      setDcfName("");
      setDcfId(0);
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetServicePlanGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const InvocheDownload = async () => {
    let result = await GetServicePlanReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const confirmDelete = (index: number) => {
    setMyConfirm({
      title: "Confirm",
      message: "Are you sure you want to delete it?",
      button: "Yes",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: () => onDeleteHandle(index),
      },
    });
  };

  const onDeleteHandle = async (id: number) => {
    setLoader("ADD", "DeleteDeepServicePlan");
    const result = await DeleteDeepServicePlan(id);
    if (result.warning === false) {
      setMyConfirm(stateConfirm);
      refresh();
      setLoader("REMOVE", "DeleteDeepServicePlan");
    }
    setLoader("REMOVE", "DeleteDeepServicePlan");
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
            <h4>{edit === true ? "Edit Service Info" : "Add Service Info"}</h4>
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
        <DialogContent sx={{ minHeight: "35rem" }}>
          {/* <ServiceLevelPAModal
            edit={edit}
            action={{ closeModal, refresh, Edit }}
            dcfId={dcfId}
            dcfName={dcfName}
          /> */}
          <ServiceLevelPlannedActivityModal
            edit={edit}
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
          <h3 className="voda-bold">Service Info</h3>
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
              <span className="fz-14">Add Service Info</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvocheDownload()}
          >
            Download to Excel
          </button>
          <Dropdown
            className="d-inline more-options grid-main-btn
"
          >
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu className="grid-main-btn">
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
              <Dropdown.Item onClick={openedArchived}>
                Archived Planned Activities
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
      <ServiceLevelGrid
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{
          onDelete: confirmDelete,
          onEdit: (id: any) => {
            GetServicePlanCreateResource();
            Edit(id);
          },
          Filter: setQuery,
        }}
      ></ServiceLevelGrid>
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default ServiceLevelContainer;
