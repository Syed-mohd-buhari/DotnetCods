import React, { useEffect, useState, useLayoutEffect } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import DesignAspectsGrid from "../screen/DesignAspect/DesignAspectGrid";
import DesignAspectModal from "../screen/DesignAspect/DesignAspectModal";
import {
  DesignAspectDtoGrid,
  DesignAspectDtoCreate,
  DesignAspectDtoUpdate,
  DesignAspectQueryObjectGrid,
} from "../Model/DesignAspects";
import { GetDesignAspectGrid } from "../Redux/Action/DesignAspect/DesignAspectGridAction";
import { GetDesignAspectCreateResource } from "../Redux/Action/DesignAspect/DesignAspectCreateAction";
import { GetDesignAspectEditResource } from "../Redux/Action/DesignAspect/DesignAspectEditAction";
import {
  DeleteDeepDesignAspect,
  deleteDesignAspect,
  GetRelatedRecordsDesignAspect,
  RestoreDesignAspect,
} from "../Redux/Action/DesignAspect/DesignAspectBuildDeleteAction";
import { GetDesignAspectReport } from "../Redux/Action/DesignAspect/DesignAspectDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import SetupColumns from "../screen/Shared/SetupColumns";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { ApiCallWithErrorHandling } from "../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../Business/DesignAspectsBusiness";
import { useAuth } from "./../Hook/useAuth";
import { useTheme } from "../Context/ThemeContext";
import { GoArrowLeft } from "react-icons/go";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: DesignAspectQueryObjectGrid = {
  opcoId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  designAspectId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
  designComponentFamilyName: [],
};

const DesignAspect: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const location: any = useLocation();

  //DTO
  const [data, setData] = useState<DesignAspectDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.designAspectGridReducer.DesignAspectGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  const { darkMode } = useTheme();
  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetDesignAspectGrid(query).then(() => {
        setLoader("REMOVE", "GetDesignAspectGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [otherButtonForRelatedModal, setOtherButtonForRelatedModal] = useState<{
    deleteAllButton: string | undefined;
    deleteButton: string | undefined;
  }>({ deleteAllButton: undefined, deleteButton: undefined });
  const [idPlannedToDelete, setIdPlannedToDelete] = useState<number>();

  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [dcfId, setDcfId] = useState<number>();

  const { readonly, tipologicaPermesso, isPermesso, pageSize } = useAuth();
  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    undefined
  );

  const [isArchived, setIsArchived] = useState<boolean>(false);

  const navigate = useNavigate();

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetDesignAspectGrid");
    closeModal();
    GetDesignAspectGrid(query).then(() =>
      setLoader("REMOVE", "GetDesignAspectGrid")
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
  } = useOperationTableCrud<DesignAspectDtoUpdate, DesignAspectDtoCreate>(
    GetDesignAspectCreateResource,
    GetDesignAspectEditResource,
    deleteDesignAspect,
    refresh,
    RestoreDesignAspect
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const EditAndDetail = (id: number, idDetail) => {
    setLocalState({
      id: id,
      tab: "plannedActivities",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
    setDetailId(idDetail);
  };

  const EditNotDetail = (id: number) => {
    setLocalState({
      id: id,
      tab: "operational",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
  };

  async function getDCFName(dcfId: any) {
    let api = new DesignAspectApi();

    let DCFName = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.getDCFName(dcfId)
    );

    setDcfName(DCFName?.data?.designComponentName!);
  }

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    setData(GridDto?.items);
    let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
    setRenderGridState(copy);
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetDesignAspectGrid");
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
        GetDesignAspectGrid(copy).then((x) => {
          console.log(x);
        });
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
    setLoader("REMOVE", "GetDesignAspectGrid");
  }, [location.state]);

  useEffect(() => {
    sessionStorage.setItem("archivedType", "DA");
    if (isPermesso) {
      if (
        new URLSearchParams(location.search).get("typeDA") !== "" &&
        new URLSearchParams(location.search).get("typeDA") !== undefined &&
        new URLSearchParams(location.search).get("typeDA") !== null
      ) {
        sessionStorage.setItem("isArchivedMode", "true");
        GetDesignAspectGrid({ ...query, archived: true });
        setIsArchived(true);
      } else {
        sessionStorage.removeItem("isArchivedMode");
        GetDesignAspectGrid({ ...query, archived: false });
        setIsArchived(false);
      }
    }
    return () => {
      sessionStorage.removeItem("isArchivedMode");
    };
  }, [location.search, query, isPermesso]);

  const closeModalSetup = (changed: boolean) => {
    GetDesignAspectGrid(isArchived ? { ...query, archived: true } : query).then(
      (x) => setIsVisibleModalSetup(false)
    );
  };

  const openArchievedDesignAspects = () => {
    setIsArchived(true);
    let location = {
      pathname: "/designAspect",
      search: "typeDA=archived",
    };
    navigate(location);
    setQuery(paginationQuery);
  };

  const InvocheDownload = async () => {
    let result = await GetDesignAspectReport({
      ...query,
      archived: isArchived,
    });
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    setIdPlannedToDelete(id);
    const result = await GetRelatedRecordsDesignAspect(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
      let copyBtnRelated = { ...otherButtonForRelatedModal };
      let hasLinkedPlanned = false;
      result.data.dataRelatedList?.map((item) =>
        item.table === "Linked Planned Activity"
          ? (hasLinkedPlanned = true)
          : null
      );
      copyBtnRelated.deleteAllButton = undefined;
      copyBtnRelated.deleteButton = undefined;
      setOtherButtonForRelatedModal(copyBtnRelated);
      // if (!hasLinkedPlanned) {
      //   copyBtnRelated.deleteAllButton = "Delete All";
      //   copyBtnRelated.deleteButton = "Delete Only Planned Activity";
      //   setOtherButtonForRelatedModal(copyBtnRelated);
      // } else {
      //   copyBtnRelated.deleteAllButton = undefined;
      //   copyBtnRelated.deleteButton = undefined;
      //   setOtherButtonForRelatedModal(copyBtnRelated);
      // }
    } else {
      setMyConfirm({
        title: "Delete orphan record",
        message:
          "This item is not currently in use in another entity, do you want to delete it?",
        button: "Delete",
        item: idPlannedToDelete ? idPlannedToDelete : "",
        isOpen: true,
        actions: {
          cancel: () => setMyConfirm(stateConfirm),
          confirm: async () => {
            setMyConfirm(stateConfirm);
            await DeleteDeepDesignAspect(id, false);
            refresh();
          },
        },
      });
    }
  };

  const confirmDelete = async (deleteOnlyPlannedActivity: boolean) => {
    if (idPlannedToDelete) {
      await DeleteDeepDesignAspect(
        idPlannedToDelete,
        deleteOnlyPlannedActivity
      );
      refresh();
    }
  };

  const [dcfName, setDcfName] = useState<string>("");

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        deleteAllButton={otherButtonForRelatedModal.deleteAllButton}
        deleteButton={otherButtonForRelatedModal.deleteButton}
        action={{
          closeModal: () => setIsVisibleModalRelated(false),
          confirm: (deleteOnlyPlannedActivity: boolean) =>
            confirmDelete(deleteOnlyPlannedActivity),
        }}
      />

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
              {edit === true ? "Edit Design Aspect" : "New Design Aspect"}
              {dcfName && <span> for </span>}
              <span
                style={{ fontSize: "14px" }}
                dangerouslySetInnerHTML={{
                  __html: dcfName ?? "",
                }}
              ></span>
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
          <DesignAspectModal
            idDetail={detailId}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
            dcfId={dcfId}
            dcfName={dcfName}
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
          {location?.state?.tab === "plannedActivities" ||
          redirect === true ||
          isArchived ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2 btnEditLink"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}

          <h3 className="voda-bold">
            {isArchived ? "Archived Design Aspects" : "Design Aspects"}
          </h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && !isArchived && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn"
              onClick={() => {
                sessionStorage.setItem(
                  "dcfId",
                  dcfId ? dcfId?.toString() : "null"
                );
                New();
              }}
              type="button"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">New Design Aspect</span>
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

              {!isArchived && (
                <Dropdown.Item onClick={openArchievedDesignAspects}>
                  Archived Design Aspects
                </Dropdown.Item>
              )}
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <>
        <DesignAspectsGrid
          data={data}
          pagination={query}
          orphanColor={orphanColor}
          renderGrid={renderGridState?.render ?? []}
          dcfName={dcfName}
          action={{
            onDelete,
            EditNotDetail,
            EditAndDetail,
            Filter: setQuery,
            Restore,
            setIsFiltriAttivati,
          }}
          readonly={readonly}
          isArchived={isArchived}
        ></DesignAspectsGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </>
    </div>
  );
};

export default DesignAspect;
