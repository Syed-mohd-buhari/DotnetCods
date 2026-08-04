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
  ServiceBoundaryGridDto,
  ServiceBoundaryQueryDto,
} from "../../Model/LookUp/ServiceBoundary";
import { GetServiceBoundaryCreateResource } from "../../Redux/Action/LookUp/ServiceBoundary/ServiceBoundaryCreateAction";
import {
  DeleteDeepServiceBoundary,
  GetRelatedRecordsServiceBoundary,
} from "../../Redux/Action/LookUp/ServiceBoundary/ServiceBoundaryDeleteAction";
import { GetServiceBoundaryEditResource } from "../../Redux/Action/LookUp/ServiceBoundary/ServiceBoundaryEditAction";
import { GetServiceBoundaryGrid } from "../../Redux/Action/LookUp/ServiceBoundary/ServiceBoundaryGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import ServiceBoundaryForm from "../../screen/Lookup/ServiceBoundary/ServiceBoundaryForm";
import ServiceBoundaryGrid from "../../screen/Lookup/ServiceBoundary/ServiceBoundaryGrid";
import { useAuth } from "../../Hook/useAuth";

export let paginationQuery = {
  serviceBoundaryId: undefined,
  serviceBoundaryDescription: undefined,
  sortBy: undefined,
  isSortAscending: undefined,
  page: 1,
  pageSize: 10,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: undefined,
  options: undefined,
} as ServiceBoundaryQueryDto;

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: boolean): any;
  };
  returnObject?(data: ServiceBoundaryGridDto[] | undefined): any;
}

const ServiceBoundary: React.FC<Props> = (props) => {
  const { isPermesso, pageSize } = useAuth();
  //DTO
  const [data, setData] = useState<ServiceBoundaryGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.serviceBoundaryGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.serviceBoundaryGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetServiceBoundaryGrid(paginationQuery);
  };
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetServiceBoundaryGrid : undefined
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
  } = useOperationTableCrud<ServiceBoundaryGridDto, ServiceBoundaryGridDto>(
    GetServiceBoundaryCreateResource,
    GetServiceBoundaryEditResource,
    DeleteDeepServiceBoundary,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1

  const resetQuery = () => {
    setQuery(paginationQuery);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      // let dataCopy = [...(GridDtoAll?.items ?? [])];
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      // GetServiceBoundaryGridALL();
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsServiceBoundary(id);
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
        backdropClassName="upperBackdropLookup"
        dialogClassName="dialogLookup"
        className="upperModalLookup"
        keyboard={false}
        size="lg"
        centered
        onHide={closeModal}
      >
        <Modal.Header closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">
                {edit ? "Edit Service Boundary" : "Add Service Boundary"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <ServiceBoundaryForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></ServiceBoundaryForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Service Boundary</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Service Boundary
          </button>
        </div>
      </div>

      <div className="">
        <ServiceBoundaryGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></ServiceBoundaryGrid>
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

export default ServiceBoundary;
