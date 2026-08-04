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
import SupportedServiceForm from "../../screen/Lookup/SupportedService/SupportedServiceForm";
import SupportedServiceGrid from "../../screen/Lookup/SupportedService/SupportedServiceGrid";
import { GetSupportedServiceCreateResource } from "../../Redux/Action/LookUp/SupportedService/SupportedServiceCreateAction";
import {
  DeleteDeepSupportedService,
  GetRelatedRecordsSupportedService,
} from "../../Redux/Action/LookUp/SupportedService/SupportedServiceDeleteAction";
import { GetSupportedServiceEditResource } from "../../Redux/Action/LookUp/SupportedService/SupportedServiceEditAction";
import {
  GetSupportedServiceGrid,
  GetSupportedServiceGridALL,
} from "../../Redux/Action/LookUp/SupportedService/SupportedServiceGridAction";
import {
  SupportedServiceDtoGrid,
  SupportedServiceQueryObjectGrid,
} from "../../Model/LookUp/SupportedService";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";

import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { DesignAspectApi } from "../../Business/DesignAspectsBusiness";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: SupportedServiceQueryObjectGrid = {
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

const SupportedService: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<SupportedServiceDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.supportedServiceGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.supportedServiceGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const refresh = () => {
    closeModal();
    GetSupportedServiceGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetSupportedServiceGrid : undefined
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
  } = useOperationTableCrud<SupportedServiceDtoGrid, SupportedServiceDtoGrid>(
    GetSupportedServiceCreateResource,
    GetSupportedServiceEditResource,
    DeleteDeepSupportedService,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetSupportedServiceGrid(paginationQueryTipologiche);
  //   GetSupportedServiceGridALL();
  // }, []);

  // const resetQuery = () => {
  //   setQuery(paginationQueryTipologiche);
  // };

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
    // GetSupportedServiceGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSupportedService(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  return (
    <div className="col-12">
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
        onHide={() => {
          closeModal();
        }}
      >
        <Modal.Header closeButton>
          <div className="col-12">
            <h4 className="mb-0">
              {edit ? "Edit Supported Service" : "Add Supported Service"}
            </h4>
          </div>
        </Modal.Header>
        <Modal.Body>
          <SupportedServiceForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></SupportedServiceForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Supported service</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Supported Service
          </button>
        </div>
      </div>

      <div className="">
        <SupportedServiceGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></SupportedServiceGrid>
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

export default SupportedService;
