import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import BundleUpgradeGrid from "../screen/BundleUpgradeInitiative/BundleUpgradeInitiativeGrid";
import BundleUpgradeModal from "../screen/BundleUpgradeInitiative/BundleUpgradeInitiativeModal";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import {
  BundleUpgradeInitiativeQueryObjectGrid,
  BundleUpgradeInitiativeDtoUpdate,
  BundleUpgradeInitiativeDtoCreate,
  BundleUpgradeInitiativeDtoGrid,
} from "../Model/BundleUpgradeIniziative";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import { GetBundleUpgradeInitiativeGrid } from "../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeGridAction";
import Paginate from "../Components/PaginationComponent";
import { GetBundleUpgradeInitiativeCreateResource } from "../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeCreateAction";
import { GetBundleUpgradeInitiativeEditResource } from "../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeEditAction";
import {
  DeleteDeepBundleUpgradeInitiative,
  GetRelatedRecordsBundleUpgradeInitiative,
  RestoreBundleUpgradeInitiative,
} from "../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeDeleteAction";
import { GetBundleUpgradeInitiativeReport } from "../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import { CustomGridRender } from "../Model/Common";
import { useAuth } from "../Hook/useAuth";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery: BundleUpgradeInitiativeQueryObjectGrid = {
  bundleUpgradeInitiativeId: [],
  oemCertifiedRelease: [],
  originalEquipmentManufacturer: [],
  remarks: [],
  spare1Json: [],
  verticalOwner: [],
  vnfType: [],
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

const BundleUpgradeInitiative: React.FC = (props) => {
  //TABS
  const [key, setKey] = useState("structure");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  //DTO
  const [data, setData] = useState<
    BundleUpgradeInitiativeDtoGrid[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.bundleUpgradeInitiativeGridReducer.BundleUpgradeInitiativeGridResult;
  const GridDto = useSelector(Grid);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [orphanColor, setOrphanColor] = useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetBundleUpgradeInitiativeGrid(query);
  };
  const { readonly, isPermesso, pageSize } = useAuth();

  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetBundleUpgradeInitiativeGrid : undefined
  );

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

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
    BundleUpgradeInitiativeDtoUpdate,
    BundleUpgradeInitiativeDtoCreate
  >(
    GetBundleUpgradeInitiativeCreateResource,
    GetBundleUpgradeInitiativeEditResource,
    DeleteDeepBundleUpgradeInitiative,
    refresh,
    RestoreBundleUpgradeInitiative
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    //  ;
    setLoader("ADD", "GetBundleUpgradeInitiativeGrid");

    if (location.state != null && location.state != undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
      };

      if (localState?.id != null) {
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);

        let copy = { ...query } as BundleUpgradeInitiativeQueryObjectGrid;
        copy.bundleUpgradeInitiativeId = [];
        copy.bundleUpgradeInitiativeId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
    } else {
      // GetBundleUpgradeInitiativeGrid(paginationQuery) ;
      // GetBundleUpgradeInitiativeGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetBundleUpgradeInitiativeGrid")
      // );
    }
    setLoader("REMOVE", "GetBundleUpgradeInitiativeGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetBundleUpgradeInitiativeGrid(query).then((x) =>
      setIsVisibleModalSetup(false)
    );
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
    let result = await GetBundleUpgradeInitiativeReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsBundleUpgradeInitiative(id);
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
      <Modal
        show={isVisibleModal}
        onHide={closeModal}
        backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3">
              <h4>
                {edit === true
                  ? "Edit Bundle Upgrade Initiative"
                  : "Add Bundle Upgrade Initiative"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <BundleUpgradeModal
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
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
          <h3 className="voda-bold">Bundle Upgrade Initiative</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {!readonly && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab"
              onClick={New}
              type="button"
            >
              <img src={require("../img/plus_1.png")} className="img-15" />
              <span className="fz-14">New Bundle</span>
            </button>
          )}

          <button
            className="download-to-excel mrl-10"
            onClick={() => InvocheDownload()}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown className="d-inline more-options">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>

      <div className="mt-4">
        <BundleUpgradeGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery, Restore }}
        ></BundleUpgradeGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </div>
  );
};

export default BundleUpgradeInitiative;
