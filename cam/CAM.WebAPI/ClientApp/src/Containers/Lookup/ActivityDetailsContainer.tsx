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
import ActivityDetailsForm from "../../screen/Lookup/ActivityDetails/ActivityDetailsForm";
import ActivityDetailsGrid from "../../screen/Lookup/ActivityDetails/ActivityDetailsGrid";
import { GetActivityDetailsCreateResource } from "../../Redux/Action/LookUp/ActivityDetails/ActivityDetailsCreateAction";
import {
  deleteActivityDetails,
  DeleteDeepActivityDetails,
  GetRelatedRecordsActivityDetails,
} from "../../Redux/Action/LookUp/ActivityDetails/ActivityDetailsDeleteAction";
import { GetActivityDetailsEditResource } from "../../Redux/Action/LookUp/ActivityDetails/ActivityDetailsEditAction";
import {
  GetActivityDetailGrid,
  GetActivityDetailGridALL,
} from "../../Redux/Action/LookUp/ActivityDetails/ActivityDetailsGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";

import { Modal } from "react-bootstrap";
import { ActivityDetailsDtoGrid } from "../../Model/LookUp/ActivityDetails";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";

export let paginationQuery = {
  forVirtualized: [],
  id: undefined,
  description: undefined,
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
} as ActivityDetailsDtoGrid;

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

const ActivityDetails: React.FC<Props> = (props) => {
  const { isPermesso, pageSize } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.activityDetailsGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.activityDetailsGridReducer.LookUpGridResultAll;
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
    GetActivityDetailGrid(paginationQuery);
  };
  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetActivityDetailGrid : undefined
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
    GetActivityDetailsCreateResource,
    GetActivityDetailsEditResource,
    DeleteDeepActivityDetails,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1

  const resetQuery = () => {
    setQuery(paginationQuery);
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
      GetActivityDetailGridALL();
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsActivityDetails(id);
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
                {edit ? " Edit Lookup Record" : "Create Lookup Record"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <ActivityDetailsForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></ActivityDetailsForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Activity Details</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Activity Details
          </button>
        </div>
      </div>

      <div className="">
        <ActivityDetailsGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></ActivityDetailsGrid>
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

export default ActivityDetails;
