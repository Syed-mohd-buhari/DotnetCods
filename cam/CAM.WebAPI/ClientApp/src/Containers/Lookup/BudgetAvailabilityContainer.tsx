import React, { useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import Paginate from "../../Components/PaginationComponent";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import BudgetAvailabilityForm from "../../screen/Lookup/BudgetAvailability/BudgetAvailabilityForm";
import BudgetAvailabilityGrid from "../../screen/Lookup/BudgetAvailability/BudgetAvailabilityGrid";
import { GetBudgetAvailabilityCreateResource } from "../../Redux/Action/LookUp/BudgetAvailability/BudgetAvailabilityCreateAction";
import {
  DeleteDeepBudgetAvailability,
  GetRelatedRecordsBudgetAvailability,
} from "../../Redux/Action/LookUp/BudgetAvailability/BudgetAvailabilityDeleteAction";
import { GetBudgetAvailabilityEditResource } from "../../Redux/Action/LookUp/BudgetAvailability/BudgetAvailabilityEditAction";
import {
  GetBudgetAvailabilityGrid,
  GetBudgetAvailabilityGridALL,
} from "../../Redux/Action/LookUp/BudgetAvailability/BudgetAvailabilityGridAction";
import {
  TipologicheQueryObjectGridCombinationRule,
  TipologicaGridDtoCombinationRule,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: TipologicheQueryObjectGridCombinationRule =
  {
    id: [],
    description: [],
    rule: [],
    projectStatusCombinationRule: [],
    lastModifiedStartDate: undefined,
    lastModifiedEndDate: undefined,
    sortBy: "",
    isSortAscending: false,
    page: 1,
    pageSize: 10,
    principalId: undefined,
    lastModifiedBy: [],
  };

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

const BudgetAvailability: React.FC<Props> = (props) => {
  
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<
    TipologicaGridDtoCombinationRule[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.budgetAvailabilityGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.budgetAvailabilityGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetBudgetAvailabilityGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetBudgetAvailabilityGrid : undefined
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
    TipologicaGridDtoCombinationRule,
    TipologicaGridDtoCombinationRule
  >(
    GetBudgetAvailabilityCreateResource,
    GetBudgetAvailabilityEditResource,
    DeleteDeepBudgetAvailability,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    GetBudgetAvailabilityGrid(paginationQueryTipologiche);
    GetBudgetAvailabilityGridALL();
  }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDtoAll?.items ?? [])];
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
      GetBudgetAvailabilityGridALL();
    }
  }, [GridDto]);

  const rulesResource = [
    { key: 0, value: "No Rule" },
    { key: 1, value: "Like No" },
    { key: 2, value: "Like Yes" },
  ];

  const rulesProjectStatus = [
    { key: 1, value: "YES" },
    { key: 2, value: "NO" },
    // { key: 3, value: "Rejected" },
    // { key: 4, value: "Budget Not Required" },
    // { key: 5, value: "Do Not Check This" }
  ];

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsBudgetAvailability(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  return (
    <div
      className={
        props.modal && props.modal.isModal ? "container" : "pageContainer"
      }
    >
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        action={{ closeModal: () => setIsVisibleModalRelated(false) }}
      />

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
        onHide={closeModal}
      >
        <Modal.Header closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">
                {edit ? " Edit Budget Availability" : "Add Budget Availability"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <BudgetAvailabilityForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
            rulesProjectStatus={rulesProjectStatus}
          ></BudgetAvailabilityForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Budget Availability</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Budget Availability
          </button>
        </div>
      </div>

      <div className="">
        <BudgetAvailabilityGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          rules={rulesResource}
          rulesProjectStatus={rulesProjectStatus}
        ></BudgetAvailabilityGrid>
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

export default BudgetAvailability;
