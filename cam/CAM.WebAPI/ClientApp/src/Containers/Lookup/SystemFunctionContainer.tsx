import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import Paginate from "../../Components/PaginationComponent";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../Model/LookUp/LookUpGenericModel";
import { GetSystemFunctionCreateResource } from "../../Redux/Action/LookUp/SystemFunction/SystemFunctionCreateAction";
import {
  DeleteDeepSystemFunction,
  GetRelatedRecordsSystemFunction,
} from "../../Redux/Action/LookUp/SystemFunction/SystemFunctionDeleteAction";
import { GetSystemFunctionEditResource } from "../../Redux/Action/LookUp/SystemFunction/SystemFunctionEditAction";
import {
  GetSystemFunctionGrid,
  GetSystemFunctionGridALL,
} from "../../Redux/Action/LookUp/SystemFunction/SystemFunctionGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import SystemFunctionForm from "../../screen/Lookup/SystemFunction/SystemFunctionForm";
import SystemFunctionGrid from "../../screen/Lookup/SystemFunction/SystemFunctionGrid";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: TipologicheQueryObjectGrid = {
  id: [],
  description: [],
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

const SystemFunction: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.systemFunctionGridReducer.LookUpGridResult;
  const GridAll = (state: RootState) =>
    state.systemFunctionGridReducer.LookUpGridResultAll;
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
    GetSystemFunctionGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetSystemFunctionGrid : undefined
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
  } = useOperationTableCrud<TipologicaGridDto, TipologicaGridDto>(
    GetSystemFunctionCreateResource,
    GetSystemFunctionEditResource,
    DeleteDeepSystemFunction,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetSystemFunctionGrid(paginationQueryTipologiche);
  //   GetSystemFunctionGridALL();
  // }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
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
    // GetSystemFunctionGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSystemFunction(id);
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
        // backdrop="static"
        backdropClassName="backdropLookup"
        dialogClassName="dialogLookup"
        className="modalLookup"
        keyboard={false}
        size="lg"
        centered
        onHide={closeModal}
      >
        <Modal.Header closeButton>
          <div className="col-12">
            <h4 className="mb-0">
              {edit ? "Edit System Function" : "Add System Function"}
            </h4>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <SystemFunctionForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></SystemFunctionForm>
        </Modal.Body>
      </Modal>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">System Function</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New System Function
          </button>
        </div>
      </div>
      <div className="">
        <SystemFunctionGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></SystemFunctionGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end d-flex footerModal">
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

export default SystemFunction;
