import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import Paginate from "../../Components/PaginationComponent";
import "../../Css/App.css";
import "../../Css/NetworkElement.css";
import "../../Css/index.css";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../Model/LookUp/LookUpGenericModel";
import { GetSharingTypeCreateResource } from "../../Redux/Action/LookUp/SharingType/SharingTypeCreateAction";
import {
  DeleteDeepSharingType,
  GetRelatedRecordsSharingType,
} from "../../Redux/Action/LookUp/SharingType/SharingTypeDeleteAction";
import { GetSharingTypeEditResource } from "../../Redux/Action/LookUp/SharingType/SharingTypeEditAction";
import {
  GetSharingTypeGrid,
  GetSharingTypeGridALL,
} from "../../Redux/Action/LookUp/SharingType/SharingTypeGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import SharingTypeForm from "../../screen/Lookup/SharingType/SharingTypeForm";
import SharingTypeGrid from "../../screen/Lookup/SharingType/SharingTypeGrid";
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

const SharingType: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.sharingTypeGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.sharingTypeGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetSharingTypeGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetSharingTypeGrid : undefined
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
    GetSharingTypeCreateResource,
    GetSharingTypeEditResource,
    DeleteDeepSharingType,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetSharingTypeGrid(paginationQueryTipologiche);
  //   GetSharingTypeGridALL();
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
      // GetSharingTypeGridALL();
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSharingType(id);
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
                {edit ? " Edit Sharing Type" : "Create Sharing Type"}
              </h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <SharingTypeForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></SharingTypeForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Sharing Type</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Sharing Type
          </button>
        </div>
      </div>

      <div className="">
        <SharingTypeGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></SharingTypeGrid>
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

export default SharingType;
