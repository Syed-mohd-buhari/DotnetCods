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
import ActivityStatusForm from "../../screen/Lookup/ActivityStatus/ActivityStatusForm";
import ActivityStatusGrid from "../../screen/Lookup/ActivityStatus/ActivityStatusGrid";
import { GetActivityStatusCreateResource } from "../../Redux/Action/LookUp/ActivityStatus/ActivityStatusCreateAction";
import {
  DeleteDeepActivityStatus,
  GetRelatedRecordsActivityStatus,
} from "../../Redux/Action/LookUp/ActivityStatus/ActivityStatusDeleteAction";
import { GetActivityStatusEditResource } from "../../Redux/Action/LookUp/ActivityStatus/ActivityStatusEditAction";
import {
  GetActivityStatusGrid,
  GetActivityStatusGridALL,
} from "../../Redux/Action/LookUp/ActivityStatus/ActivityStatusGridAction";
import {
  TipologicaGridDtoCombinationRule,
  TipologicheQueryObjectGridCombinationRule,
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

const ActivityStatus: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<
    TipologicaGridDtoCombinationRule[] | undefined
  >([]);
  const Grid = (state: RootState) =>
    state.activityStatusGridReducer.LookUpGridResult;
  const GridAll = (state: RootState) =>
    state.activityStatusGridReducer.LookUpGridResultAll;
  const GridDto = useSelector(Grid);
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetActivityStatusGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetActivityStatusGrid : undefined
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
  } = useOperationTableCrud<
    TipologicaGridDtoCombinationRule,
    TipologicaGridDtoCombinationRule
  >(
    GetActivityStatusCreateResource,
    GetActivityStatusEditResource,
    DeleteDeepActivityStatus,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetActivityStatusGrid");
    GetActivityStatusGrid(paginationQueryTipologiche).then((x) =>
      setLoader("REMOVE", "GetActivityStatusGrid")
    );
    GetActivityStatusGridALL();
  }, []);

  const resetQuery = () => {
    //  ;
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
    }
    GetActivityStatusGridALL();
  }, [GridDto]);

  const rulesResource = [
    { key: 0, value: "No Rule" },
    { key: 1, value: "In Planning" },
    { key: 2, value: "In Mobilisation" },
    { key: 3, value: "In Delivery Engineering" },
    { key: 4, value: "In Delivery Operations" },
    { key: 5, value: "Complete" },
  ];

  const rulesProjectStatus = [
    { key: 1, value: "In Planning" },
    { key: 2, value: "In Mobilisation" },
    { key: 3, value: "In Delivery Engineering" },
    { key: 4, value: "In Delivery Operations" },
    { key: 5, value: "Complete" },
  ];

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsActivityStatus(id);
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
                {edit ? " Edit Activity Status" : "Create Activity Status"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <ActivityStatusForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
            rulesProjectStatus={rulesProjectStatus}
          ></ActivityStatusForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Activity Status</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Activity Status
          </button>
        </div>
      </div>

      <div className="">
        <ActivityStatusGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          rules={rulesResource}
          rulesProjectStatus={rulesProjectStatus}
        ></ActivityStatusGrid>
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

export default ActivityStatus;
