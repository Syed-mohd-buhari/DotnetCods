import React, { useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import Paginate from "../../Components/PaginationComponent";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../../Model/Common";
import SubDomainSpocForm from "../../screen/Lookup/SubDomainSpoc/SubDomainSpocForm";
import SubDomainSpocGrid from "../../screen/Lookup/SubDomainSpoc/SubDomainSpocGrid";
import { GetSubDomainSpocCreateResource } from "../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocCreateAction";
import {
  DeleteDeepSubDomainSpoc,
  GetRelatedRecordsSubDomainSpoc,
} from "../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocDeleteAction";
import { GetSubDomainSpocEditResource } from "../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocEditAction";
import {
  GetSubDomainSpocGrid,
  GetSubDomainSpocGridALL,
} from "../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
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
  isEdu: [],
  isSubDomain: [],
};

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
  removedObject?(id: any): any;
  subDomain: boolean;
}

const SubDomainSpoc: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const [confirmModal, setConfirmModal] =
    useState<DataModalConfirm>(stateConfirm);
  const [updateSpocValue, setUpdateSpocValue] = useState<boolean>(false);
  const Grid = (state: RootState) =>
    state.subDomainSpocGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.subDomainSpocGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetSubDomainSpocGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetSubDomainSpocGrid : undefined
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
    GetSubDomainSpocCreateResource,
    GetSubDomainSpocEditResource,
    DeleteDeepSubDomainSpoc,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetSubDomainSpocGrid(paginationQueryTipologiche);
  //   GetSubDomainSpocGridALL();
  // }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    if (updateSpocValue) {
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject && props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      // console.log("copy => ", copy);
      setRenderGridState(copy);
      // GetSubDomainSpocGridALL();
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSubDomainSpoc(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      setConfirmModal({
        title: "Delete Entry",
        message: "Do you want to delete this item?",
        button: "Delete",
        item: id,
        isOpen: true,
        actions: {
          cancel: () => setConfirmModal(stateConfirm),
          confirm: async () => {
            setConfirmModal(stateConfirm);
            const result = await DeleteDeepSubDomainSpoc(id);
            if (result.data !== null) {
              props.removedObject && props.removedObject(result.data);
              refresh();
            }
          },
        },
      });
    }
  };

  const onAdd = () => {
    New();
    setUpdateSpocValue(true);
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
      <ModalConfirm data={confirmModal} />
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
            <h4 className="mb-0">{edit ? " Edit Contact" : "Add Contact"}</h4>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <SubDomainSpocForm
            subDomain={props.subDomain}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></SubDomainSpocForm>
        </Modal.Body>
      </Modal>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">
            {props.subDomain ? "Sub-Domain Spoc" : "Add Contact"}
          </h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={onAdd}
            type="button"
          >
            {props.subDomain ? "New Sub-Domain Spoc" : "Add Contact"}
          </button>
        </div>
      </div>
      <div className="">
        <SubDomainSpocGrid
          subDomain={props.subDomain}
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{
            onDelete,
            Edit: (id: any) => {
              Edit(id);
              setUpdateSpocValue(true);
            },
            Filter: (val: any) => {
              setQuery(val);
              setUpdateSpocValue(false);
            },
          }}
        ></SubDomainSpocGrid>
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
            className="voda-bold btn btn-danger px-4 btnHeader"
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

export default SubDomainSpoc;
