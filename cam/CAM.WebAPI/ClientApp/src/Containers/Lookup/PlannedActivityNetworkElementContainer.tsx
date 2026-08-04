import React, { useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import Paginate from "../../Components/PaginationComponent";
import setLoader from "../../Redux/Action/LoaderAction";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import PlannedActivityNetworkElementForm from "../../screen/Lookup/PlannedActivityNetworkElement/PlannedActivityNetworkElementForm";
import PlannedActivityNetworkElementGrid from "../../screen/Lookup/PlannedActivityNetworkElement/PlannedActivityNetworkElementGrid";
import { GetPlannedActivityNetworkElementCreateResource } from "../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementCreateAction";
import { deletePlannedActivityNetworkElement } from "../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementDeleteAction";
import { GetPlannedActivityNetworkElementEditResource } from "../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementEditAction";
import {
  GetPlannedActivityNetworkElementGrid,
  GetPlannedActivityNetworkElementGridALL,
} from "../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import { Modal } from "react-bootstrap";
import {
  PlannedActivityNetworkElementQueryObjectGrid,
  PlannedActivityNetworkElementDtoGrid,
} from "../../Model/LookUp/PlannedActivityNetworkElement";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: PlannedActivityNetworkElementQueryObjectGrid =
  {
    plannedActivityResourceId: [],
    plannedActivityNetworkElementId: [],
    plannedActivityResourceDescription: [],
    plannedActivityNetworkElementDescription: [],
    rule: [],
    lastModifiedStartDate: undefined,
    lastModifiedEndDate: undefined,
    sortBy: "",
    isSortAscending: false,
    page: 1,
    pageSize: 10,
    principalId: undefined,
    deleted: undefined,
    orphan: undefined,
    lastModifiedBy: [],
    driverText: [],
    benefitText: [],
  };

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: PlannedActivityNetworkElementDtoGrid[]): any;
}

const PlannedActivityNetworkElement: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<
    PlannedActivityNetworkElementDtoGrid[] | undefined
  >([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.plannedActivityNetworkElementGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.plannedActivityNetworkElementGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const refresh = () => {
    closeModal();
    GetPlannedActivityNetworkElementGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetPlannedActivityNetworkElementGrid : undefined
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
  } = useOperationTableCrud<
    PlannedActivityNetworkElementDtoGrid,
    PlannedActivityNetworkElementDtoGrid
  >(
    GetPlannedActivityNetworkElementCreateResource,
    GetPlannedActivityNetworkElementEditResource,
    deletePlannedActivityNetworkElement,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    GetPlannedActivityNetworkElementGrid(paginationQueryTipologiche);
  }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [
        ...(GridDtoAll?.items ?? []),
      ] as PlannedActivityNetworkElementDtoGrid[];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
    GetPlannedActivityNetworkElementGridALL();
  }, [GridDto]);

  const rulesResource = [
    { key: 0, value: "Other" },
    { key: 1, value: "New Instance" },
    { key: 2, value: "FoA System" },
  ];

  return (
    <div
      className={
        props.modal && props.modal.isModal ? "container" : "pageContainer"
      }
    >
      <ModalConfirm data={confirm} />
      <Modal
        show={isVisibleModal}
        backdrop="static"
        backdropClassName="backdropLookup"
        dialogClassName="dialogLookup"
        className="modalLookup"
        keyboard={false}
        size="lg"
        centered
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">
                {edit ? " Edit Lookup Record" : "Create Lookup Record"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <PlannedActivityNetworkElementForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
          ></PlannedActivityNetworkElementForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold  ">Planned Activity</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Planned Activity
          </button>
        </div>
      </div>

      <div className="">
        <PlannedActivityNetworkElementGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ Delete, Edit, Filter: setQuery }}
          rules={rulesResource}
        ></PlannedActivityNetworkElementGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
          {/* <button className="  voda-bold btn btn-link px-4 btnHeader cancel" type="button">Close</button> */}
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => chiudiModal()}
          >
            Close
          </button>
        </div>
      ) : null}
    </div>
  );
};

export default PlannedActivityNetworkElement;
