import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import {
  ChangeGridOrderDto,
  RelatedRecordsResultDto,
} from "../Model/CommonModels";
import {
  SettingsUpdatePlannedActivityDtoCreate,
  SettingsUpdatePlannedActivityDtoGrid,
  SettingsUpdatePlannedActivityDtoUpdate,
  SettingsUpdatePlannedActivityQueryObjectGrid,
} from "../Model/SettingsUpdatePlannedActivity";
import setLoader from "../Redux/Action/LoaderAction";
import { ChangeGridSettingsUpdatePlannedActivity } from "../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityCommonAction";
import { GetSettingsUpdatePlannedActivityCreateResource } from "../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityCreateAction";
import {
  DeleteDeepSettingsUpdatePlannedActivity,
  GetRelatedRecordsSettingsUpdatePlannedActivity,
} from "../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityDeleteAction";
import { GetSettingsUpdatePlannedActivityEditResource } from "../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityEditAction";
import { GetSettingsUpdatePlannedActivityGrid } from "../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityGridAction";
import { GetSettingUpdatePlannedActivityBuildDownload } from "../Redux/Action/SettingsUpdatePlannedActivity/SettingUpdatePlannedActivityDownloadAction";
import { RootState } from "../Redux/Store/rootStore";
import SettingsUpdatePlannedActivityGrids from "../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityGrids";
import SettingsUpdatePlannedActivityModal from "../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import SetupOrderGrid from "../screen/Shared/SetupOrderGrid";
import { useAuth } from "./../Hook/useAuth";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery = {
  deliveryStatus: [],
  lcmDeploymentStatus: [],
  planningActivityResource: [],
  successorPlannedActivityTypeResource: [],
  ruleforSuccessorPlannedActivityCreation: [],
  planningActivityStatusId: [],
  settingsUpdatePlannedActivityDescription: [],
  budgetAvailability: [],
  localApproval: [],
  deliveryStatusId: [],
  lastModifiedBy: [],
  msStatus: [],
  msStatusDuration: [],
  ruleElementCount: [],
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  principalId: undefined,
} as SettingsUpdatePlannedActivityQueryObjectGrid;

const SettingsUpdatePlannedActivity: React.FC = (props) => {
  //TABS
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  //DTO
  const [data, setData] = useState<
    SettingsUpdatePlannedActivityDtoGrid[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.SettingsUpdatePlannedActivityGridReducer
      .SettingsUpdatePlannedActivityGridResult;
  const GridDto = useSelector(Grid);
  const [orphanColor, setOrphanColor] = useState(false);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const { readonly, isPermesso, tipologicaPermesso, pageSize } = useAuth();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const { darkMode } = useTheme();
  const refresh = () => {
    closeModal();
    GetSettingsUpdatePlannedActivityGrid(query);
  };

  let rules = [
    { key: 0, value: "Do not Create LCM" },
    { key: 1, value: "Create LCM" },
    { key: 2, value: "Create LCM Without Operational Data" },
  ];

  let rulesElementCount = [
    { key: 0, value: "No Rule" },
    { key: 1, value: "Archive Planned Activity and Parent Entry" },
    { key: 2, value: "Archive Planned Activity only" },
  ];

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetSettingsUpdatePlannedActivityGrid : undefined
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
    SettingsUpdatePlannedActivityDtoUpdate,
    SettingsUpdatePlannedActivityDtoCreate
  >(
    GetSettingsUpdatePlannedActivityCreateResource,
    GetSettingsUpdatePlannedActivityEditResource,
    DeleteDeepSettingsUpdatePlannedActivity,
    refresh
  );

  const InvocheDownload = async () => {
    let result = await GetSettingUpdatePlannedActivityBuildDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetSettingsUpdatePlannedActivityGrid");
    if (location.state != null && location.state != undefined) {
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

        let copy = { ...query } as SettingsUpdatePlannedActivityQueryObjectGrid;

        setQuery(copy);
      }
    } else {
      setLoader("REMOVE", "GetSettingsUpdatePlannedActivityGrid");
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetSettingsUpdatePlannedActivityGrid(query).then((x) => {
      setLoader("REMOVE", "GetSettingsUpdatePlannedActivityGrid");
      setIsVisibleModalSetup(false);
    });
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

  //ORDER GRID
  const [isSetupOrder, setIsSetupOrder] = useState<boolean>(false);
  const [changedOrder, SetChangedOrder] = useState<boolean>(false);
  const [isConfirmOrder, SetIsConfirmOrder] = useState<boolean>(false);
  const [dataOrder, SetDataOrder] = useState<ChangeGridOrderDto[]>([]);

  const SaveOrderGrid = async () => {
    await ChangeGridSettingsUpdatePlannedActivity(dataOrder).then((x) => {
      if (!x?.warning) {
        setIsSetupOrder(false);
        SetIsConfirmOrder(false);
        refresh();
      }
    });
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSettingsUpdatePlannedActivity(id);
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
                ? "Update Settings of Planned Activity Type"
                : "Add New Settings to Planned Activity Type"}
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
          <SettingsUpdatePlannedActivityModal
            rulesResource={rules}
            rulesResourceElementCount={rulesElementCount}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
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
              className="d-flex justify-content-center align-items-center mr-3 mb-2 btnEditLink"
              to={{ pathname: localStateHistory?.prevPage }}
            >
              <GoArrowLeft
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">Settings Update Planned Activity</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        {isSetupOrder == true ? (
          <div className="d-flex">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel mr-3 grid-main-btn"
              type="button"
              onClick={() =>
                changedOrder ? SetIsConfirmOrder(true) : setIsSetupOrder(false)
              }
            >
              Cancel
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader grid-main-btn"
              type="button"
              onClick={() => SaveOrderGrid()}
            >
              Save
            </button>
          </div>
        ) : (
          <div className="d-flex">
            {tipologicaPermesso && (
              <>
                <button
                  className="voda-bold btn btn-danger px-4 btnHeader grid-main-btn
                  "
                  onClick={New}
                  type="button"
                >
                  New Setting
                </button>
                <button
                  className="download-to-excel mrl-10 grid-main-btn
                  "
                  onClick={() => InvocheDownload()}
                >
                  Download to Excel
                </button>
                <Dropdown
                  className="d-inline more-options mrl-10 grid-main-btn
"
                >
                  <Dropdown.Toggle id="dropdown-autoclose-inside">
                    More Options
                  </Dropdown.Toggle>

                  <Dropdown.Menu
                    className="grid-main-btn
"
                  >
                    <Dropdown.Item
                      onClick={() => setIsSetupOrder(true)}
                      disabled={isSetupOrder}
                    >
                      Sort
                    </Dropdown.Item>
                    <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                      Manage Table Content
                    </Dropdown.Item>
                  </Dropdown.Menu>
                </Dropdown>
              </>
            )}
          </div>
        )}
      </div>
      {isSetupOrder == true ? (
        <SetupOrderGrid
          action={{
            SetChangedOrder,
            SetDataOrder,
            setIsSetupOrder,
            SetIsConfirmOrder,
          }}
          renderGrid={renderGridState}
          propertyOrder="order"
          data={data}
          isConfirmOrder={isConfirmOrder}
          id={"settingsUpdatePlannedActivityId"}
        ></SetupOrderGrid>
      ) : (
        <div>
          <SettingsUpdatePlannedActivityGrids
            rulesResource={rules}
            rulesResourceElementCount={rulesElementCount}
            data={data}
            pagination={query}
            orphanColor={orphanColor}
            renderGrid={renderGridState?.render ?? []}
            action={{ onDelete, Edit, Filter: setQuery, Restore }}
          ></SettingsUpdatePlannedActivityGrids>
          <Paginate
            pagination={{ page: query.page, pageSize: query.pageSize }}
            totalItems={GridDto?.totalItems}
            actions={{ next, back }}
          />
        </div>
      )}
    </div>
  );
};

export default SettingsUpdatePlannedActivity;
