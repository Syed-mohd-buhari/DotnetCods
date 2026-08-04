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
  PlannedActivityTypesDtoCreate,
  PlannedActivityTypesDtoGrid,
  PlannedActivityTypesDtoUpdate,
  PlannedActivityTypesQueryObjectGrid,
} from "../Model/PlannedActivityTypes";
import setLoader from "../Redux/Action/LoaderAction";
import { RootState } from "../Redux/Store/rootStore";
import PlannedActivityTypesGrids from "../screen/PlannedActivityTypes/PlannedActivityTypesGrids";
import SetupColumns from "../screen/Shared/SetupColumns";
import SetupOrderGrid from "../screen/Shared/SetupOrderGrid";
import { useAuth } from "./../Hook/useAuth";
import { GetPlannedActivityTypesGrid } from "../Redux/Action/PlannedActivityTypes/PlannedActivityTypesGridAction";
import { deletePlannedActivityTypes } from "../Redux/Action/PlannedActivityTypes/PlannedActivityTypesDeleteAction";
import {
  CreatPlannedActivityTypes,
  PlannedActivityTypeEnableLinkedDC,
} from "../Redux/Action/PlannedActivityTypes/PlannedActivityTypesCreateAction";
import { EditPlannedActivityTypes } from "../Redux/Action/PlannedActivityTypes/PlannedActivityTypesEditAction";
import PlannedActivityTypesEditModal from "../screen/PlannedActivityTypes/PlannedActivityTypesEditModal";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery = {
  plannedActivityTypesId: [],
  plannedActivityTypeDescription: [],
  hwOem: [],
  hwSolution: [],
  hwPlatform: [],
  swOem: [],
  swVersion: [],
  swProductname: [],
  subNetworkService: [],
  linkedDcRule: [],
  forLcm: [],
  forAsset: [],
  forDesignAspect: [],
  forserviceplan: [],
  lastModifiedBy: [],
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  principalId: undefined,
} as PlannedActivityTypesQueryObjectGrid;

const PlannedActivityTypes: React.FC = (props) => {
  //TABS
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  //DTO
  const [data, setData] = useState<PlannedActivityTypesDtoGrid[] | undefined>(
    []
  );
  const [editData, setEditData] = useState<PlannedActivityTypesDtoUpdate>();
  const Grid = (state: RootState) =>
    state.PlannedActivityTypesGridReducer.PlannedActivityTypesGridResult;
  const GridDto = useSelector(Grid);
  const [orphanColor, setOrphanColor] = useState(false);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const { readonly, isPermesso, pageSize } = useAuth();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [isEditModal, setIsEditModal] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetPlannedActivityTypesGrid(paginationQuery);
  };

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
    isPermesso ? GetPlannedActivityTypesGrid : undefined
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
    PlannedActivityTypesDtoUpdate,
    PlannedActivityTypesDtoCreate
  >(
    CreatPlannedActivityTypes,
    EditPlannedActivityTypes,
    deletePlannedActivityTypes,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetPlannedActivityTypesGrid");
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

        let copy = { ...query } as PlannedActivityTypesQueryObjectGrid;

        setQuery(copy);
      }
    } else {
      setLoader("REMOVE", "GetPlannedActivityTypesGrid");
    }
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetPlannedActivityTypesGrid(paginationQuery).then((x) => {
      setLoader("REMOVE", "GetPlannedActivityTypesGrid");
      setIsVisibleModalSetup(false);
    });
  };

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const Enable = (id, linkedDc) => {
    PlannedActivityTypeEnableLinkedDC({
      plannedActivityTypeId: id,
      linkedDcRule: linkedDc ? false : true,
    }).then((x) => x?.warning === false && refresh());
  };

  const onEdit = (item) => {
    setIsEditModal(true);
    setEditData(item);
  };

  const EditPlanned = (item) => {
    EditPlannedActivityTypes(item).then((x) => {
      setIsEditModal(false);
      refresh();
    });
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  return (
    <div className="pageContainer">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />
      <ModalConfirm data={confirm} />
      <Modal
        show={isEditModal}
        onHide={() => setIsEditModal(false)}
        // backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 mt-3">
            <h4>Update Planned Activity Type</h4>
          </div>
        </Modal.Header>
        <Modal.Body>
          <PlannedActivityTypesEditModal
            editData={editData}
            action={{ closeModal: () => setIsEditModal(false), EditPlanned }}
          />
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
          <h3 className="voda-bold">Planned Activity Type</h3>
        </div>
        <div className="d-flex">
          {!readonly && (
            <>
              {/* <button
                    className="  voda-bold btn btn-danger px-4 btnHeader"
                    onClick={New}
                    type="button"
                >
                    New Setting
                </button> */}
              {/* <button
                    className="download-to-excel mrl-10"
                    onClick={() => InvocheDownload()}
                >
                    Download to Excel
                </button> */}
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
                  <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                    Manage Table Content
                  </Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </>
          )}
        </div>
      </div>
      <PlannedActivityTypesGrids
        data={data}
        pagination={query}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{ Delete, onEdit, Filter: setQuery, Restore, Enable }}
      />
      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default PlannedActivityTypes;
