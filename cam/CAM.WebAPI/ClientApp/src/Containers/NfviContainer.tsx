import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import NfviGrid from "../screen/Nfvi/NfviGrid";
import NfviModal from "../screen/Nfvi/NfviModal";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import {
  NFVITransitionQueryObjectGrid,
  NFVITransitionDtoUpdate,
  NFVITransitionDtoCreate,
  NFVITransitionDtoGrid,
} from "../Model/NFVITransition";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import { GetNFVITransitionGrid } from "../Redux/Action/NFVITransition/NFVITransitionGridAction";
import Paginate from "../Components/PaginationComponent";
import { GetNFVITransitionCreateResource } from "../Redux/Action/NFVITransition/NFVITransitionCreateAction";
import { GetNFVITransitionEditResource } from "../Redux/Action/NFVITransition/NFVITransitionEditAction";
import {
  DeleteDeepNFVITransition,
  GetRelatedRecordsNFVITransition,
  RestoreNFVITransition,
} from "../Redux/Action/NFVITransition/NFVITransitionDeleteAction";
import { GetNFVITransitionReport } from "../Redux/Action/NFVITransition/NFVITransitionDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import SetupColumns from "../screen/Shared/SetupColumns";
import { CustomGridRender } from "../Model/Common";
import AdditionalFiltersMenu from "../Components/AdditionalFiltersMenu";
import { useAuth } from "../Hook/useAuth";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery: NFVITransitionQueryObjectGrid = {
  opCo: [],
  nfviSiteDesignation: [],
  nextStep: [],
  status12KSwitch: [],
  statusLabMC: [],
  statusLabSC: [],
  statusLiveMC: [],
  statusLiveSC: [],
  spare1Json: [],
  nfviTransitionId: [],
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

const NfviContainer: React.FC = (props) => {
  //TABS
  const [key, setKey] = useState("structure");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  //DTO
  const [data, setData] = useState<NFVITransitionDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.nFVITransitionGridReducer.NFVITransitionGridResult;
  const GridDto = useSelector(Grid);
  const { readonly, isPermesso,pageSize } = useAuth();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetNFVITransitionGrid(query);
  };

  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetNFVITransitionGrid : undefined
  );
  const [orphanColor, setOrphanColor] = useState(false);

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
  } = useOperationTableCrud<NFVITransitionDtoUpdate, NFVITransitionDtoCreate>(
    GetNFVITransitionCreateResource,
    GetNFVITransitionEditResource,
    DeleteDeepNFVITransition,
    refresh,
    RestoreNFVITransition
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetNFVITransitionGrid");

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

        let copy = { ...query } as NFVITransitionQueryObjectGrid;
        copy.nfviTransitionId = [];
        copy.nfviTransitionId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
    } else {
      // GetNFVITransitionGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetNFVITransitionGrid")
      // );
    }
    setLoader("REMOVE", "GetNFVITransitionGrid");
  }, []);

  const closeModalSetup = (changed: boolean) => {
    GetNFVITransitionGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetNFVITransitionGrid");
    }
  }, [GridDto]);

  const InvocheDownload = async () => {
    let result = await GetNFVITransitionReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsNFVITransition(id);
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
                  ? "Edit NFVI (Transition)"
                  : "Add NFVI (Transition)"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <NfviModal
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
          <h3 className="voda-bold">NFVI (Transition)</h3>
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
              <span className="fz-14">New NFVI (Transition)</span>
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
        <NfviGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery, Restore }}
        ></NfviGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </div>
  );
};

export default NfviContainer;
