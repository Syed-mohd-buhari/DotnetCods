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
import { GetBuildBagGrid } from "../Redux/Action/BuildBag/BuildBagGridAction";
import {
  BuildBagDtoCreate,
  BuildBagQueryObjectGrid,
  BuildBagDtoGrid,
  BuildBagDtoUpdate,
  BuildBagToCloneDto,
} from "../Model/BuildBag";
import BuildBagGrid from "../screen/BuildBag/BuildBag";
import BuildBagModal from "../screen/BuildBag/BuildBagModal";
import { GetBuildBagCreateResource } from "../Redux/Action/BuildBag/BuildBagCreateAction";
import {
  GetRelatedRecordsBuildBag,
  RestoreBuildBag,
  deleteBuildBag,
} from "../Redux/Action/BuildBag/BuildBagDeleteAction";
import { GetBuildBagEditResource } from "../Redux/Action/BuildBag/BuildBagEditAction";
import { GetBuildBagDownload } from "../Redux/Action/BuildBag/BuildBagDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { useAuth } from "../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import {
  GetSoftwareComponentToClone,
  GetViewBagAndComponent,
} from "../Redux/Action/BuildBag/BuildBagCommonAction";
import SoftwareComponentUpgrade from "../screen/BuildBag/SoftwareComponentUpgradeModal";
import ViewMappedComponent from "./ViewMappedComponent";
import { resourceArrayRefactor } from "../Hook/Dictionary";

export let paginationQuery: BuildBagQueryObjectGrid = {
  buildBagDescription: [],
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

interface Props {
  redirect?: string;
  modal?: {
    isModal: boolean | false;
    setIsBuildBagItemFlag(flag: boolean): any;
  };
  returnObject?(data: Array<any>): any;
}

const BuildBagItemComponent = (props: Props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);

  const { darkMode } = useTheme();

  //DTO
  const [data, setData] = useState<BuildBagDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.buildBagGridReducer.BuildBagGridResult;
  let GridDto = useSelector(Grid);

  const externalRefresh = (state: RootState) =>
    state.externalRefreshReducer.refresh;
  let externalRefreshDto = useSelector(externalRefresh);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (externalRefreshDto === true) {
      GetBuildBagGrid(query).then(() => {
        setLoader("REMOVE", "GetBuildBagGrid");
        rootStore.dispatch({ type: "REFRESH", payload: false });
      });
    }
  }, [externalRefreshDto]);

  const { RefactorUser, readonly, tipologicaPermesso, isPermesso, pageSize } =
    useAuth();

  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleModalUpgrade, setIsVisibleModalUpgrade] = useState(false);
  const [isVisibleModalView, setIsVisibleModalView] = useState(false);
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [dataToClone, setDataToClone] = useState<BuildBagToCloneDto>();
  const [viewData, setViewData] = useState<any>(null);
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
    isPermesso ? GetBuildBagGrid : undefined
  );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetBuildBagGrid");
    closeModal();
    GetBuildBagGrid(query).then(() => setLoader("REMOVE", "GetBuildBagGrid"));
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
  } = useOperationTableCrud<BuildBagDtoUpdate, BuildBagDtoCreate>(
    GetBuildBagCreateResource,
    GetBuildBagEditResource,
    deleteBuildBag,
    refresh,
    RestoreBuildBag
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetBuildBagGrid");
    if (location.state !== null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        lcmId: string;
        tab: string;
        prevPage: string;
      };
      if (localState?.id != null) {
        let copy = { ...query } as BuildBagQueryObjectGrid;
        setRedirect(true);
        setFilterRedirect(true);
        if (localState?.lcmId === null) {
          setLocalState(localState);
          Edit(localState?.id);
          copy.buildBagId = [];
          copy.buildBagId?.push(localState?.id);
          copy.principalId = localState?.id;
        } else {
          New();
        }
        setQuery(copy);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      setLoader("REMOVE", "GetBuildBagGrid");
      // GetBuildBagGrid(paginationQuery).then((x) => {
      //   setLoader("REMOVE", "GetBuildBagGrid");
      // });
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetBuildBagGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetBuildBagGrid");
    }
  }, [GridDto]);

  const InvocheDownload = async () => {
    let result = await GetBuildBagDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const [prevPage, setPrevPage] = useState<string>();

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsBuildBag(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const getSoftwareComponentUpgradeData = async (id: number) => {
    await GetSoftwareComponentToClone(id).then((x) => {
      // if (!x?.warning) {
      setDataToClone(x);
      setIsVisibleModalUpgrade(true);
      // }
    });
  };

  const View = async (id: number) => {
    await GetViewBagAndComponent(id).then((x) => {
      setViewData(x);
      setIsVisibleModalView(true);
    });
  };

  const closeRefillModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsBuildBagItemFlag(false);
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
              {edit == true
                ? "Edit Software Component Bag"
                : "Add Software Component Bag"}
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
          <BuildBagModal
            edit={edit}
            softwareRedirect={redirect ?? false}
            prevPage={prevPage}
            keyTab={localStateHistory?.tab}
            lcmId={location?.state?.lcmId ?? null}
            dcfId={location?.state?.dcfId ?? null}
            opcoId={location?.state?.opCoId ?? null}
            fromLcm={
              location?.state?.prevPage === "lcmengineering" ? true : false
            }
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
            <h4>Upgrade Software Component</h4>
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
          <SoftwareComponentUpgrade
            data={dataToClone}
            action={{ refresh, setIsVisibleModalUpgrade }}
          />
        </DialogContent>
      </Dialog>
      <Dialog
        open={isVisibleModalView}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setIsVisibleModalView(false);
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
          <div className="col-12 px-0 mt-1">
            <h4>View Software Component</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalView(false)}
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
          <ViewMappedComponent
            redirect={"bagComponentScreen"}
            bagId={viewData?.buildBagId}
            bagName={viewData?.componentBagDescription}
            mappedComponentDetails={viewData?.mappedComponentDetails}
            modal={{
              isModal: true,
              setViewBagFlag: (flag) => setIsVisibleModalView(flag),
            }}
          />
        </DialogContent>
      </Dialog>
      <ModalConfirm data={confirm} />

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
          <h3 className="voda-bold">Software Component Bag </h3>
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
              <span className="fz-14">New Sw Component Bag</span>
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
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <BuildBagGrid
        data={data}
        pagination={query}
        renderGrid={renderGridState?.render ?? []}
        orphanColor={orphanColor}
        action={{
          onDelete,
          Edit,
          View,
          Filter: setQuery,
          Restore,
          setIsFiltriAttivati,
          getSoftwareComponentUpgradeData,
        }}
        readonly={readonly}
      ></BuildBagGrid>
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

export default BuildBagItemComponent;
