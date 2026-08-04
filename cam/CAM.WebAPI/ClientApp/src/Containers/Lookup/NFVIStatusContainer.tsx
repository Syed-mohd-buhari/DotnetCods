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
import NFVIStatusForm from "../../screen/Lookup/NFVIStatus/NFVIStatusForm";
import NFVIStatusGrid from "../../screen/Lookup/NFVIStatus/NFVIStatusGrid";
import { GetNFVIStatusCreateResource } from "../../Redux/Action/LookUp/NFVIStatus/NFVIStatusCreateAction";
import {
  DeleteDeepNFVIStatus,
  deleteNFVIStatus,
  GetRelatedRecordsNFVIStatus,
} from "../../Redux/Action/LookUp/NFVIStatus/NFVIStatusDeleteAction";
import { GetNFVIStatusEditResource } from "../../Redux/Action/LookUp/NFVIStatus/NFVIStatusEditAction";
import {
  GetNFVIStatusGrid,
  GetNFVIStatusGridALL,
} from "../../Redux/Action/LookUp/NFVIStatus/NFVIStatusGridAction";
import {
  NFVIStatusDtoGrid,
  NFVIStatusQueryObjectGrid,
} from "../../Model/LookUp/NFVIStatus";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";

import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: NFVIStatusQueryObjectGrid = {
  id: [],
  description: [],
  color: [],
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

const NFVIStatus: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<NFVIStatusDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.nFVIStatusGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.nFVIStatusGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetNFVIStatusGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetNFVIStatusGrid : undefined
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
  } = useOperationTableCrud<NFVIStatusDtoGrid, NFVIStatusDtoGrid>(
    GetNFVIStatusCreateResource,
    GetNFVIStatusEditResource,
    DeleteDeepNFVIStatus,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetNFVIStatusGrid(paginationQueryTipologiche);
  //   GetNFVIStatusGridALL();
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
    // GetNFVIStatusGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsNFVIStatus(id);
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
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">
                {edit ? " Edit NFVI Status" : "Add NFVI Status"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <NFVIStatusForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></NFVIStatusForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text- ">NFVI (Transition)</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New NFVI (Transition)
          </button>
        </div>
      </div>

      <div className="">
        <NFVIStatusGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></NFVIStatusGrid>
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

export default NFVIStatus;
