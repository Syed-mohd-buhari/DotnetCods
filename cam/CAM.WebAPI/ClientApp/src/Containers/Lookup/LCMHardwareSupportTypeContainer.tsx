import React, { useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import Paginate from "../../Components/PaginationComponent";
import ErrorNotification from "../../Components/ErrorNotification";
import setLoader from "../../Redux/Action/LoaderAction";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import LCMHardwareSupportTypeForm from "../../screen/Lookup/LCMHardwareSupportType/LCMHardwareSupportTypeForm";
import LCMHardwareSupportTypeGrid from "../../screen/Lookup/LCMHardwareSupportType/LCMHardwareSupportTypeGrid";
import { GetLCMHardwareSupportTypeCreateResource } from "../../Redux/Action/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeCreateAction";
import { deleteLCMHardwareSupportType } from "../../Redux/Action/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeDeleteAction";
import { GetLCMHardwareSupportTypeEditResource } from "../../Redux/Action/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeEditAction";
import {
  GetLCMHardwareSupportTypeGrid,
  GetLCMHardwareSupportTypeGridALL,
} from "../../Redux/Action/LookUp/LCMHardwareSupportType/LCMHardwareSupportTypeGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import { Modal } from "react-bootstrap";
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

const LCMHardwareSupportType: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.lCMHardwareSupportTypeGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.lCMHardwareSupportTypeGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const refresh = () => {
    closeModal();
    GetLCMHardwareSupportTypeGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetLCMHardwareSupportTypeGrid : undefined
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
    GetLCMHardwareSupportTypeCreateResource,
    GetLCMHardwareSupportTypeEditResource,
    deleteLCMHardwareSupportType,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    GetLCMHardwareSupportTypeGrid(paginationQueryTipologiche);
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
    }
    GetLCMHardwareSupportTypeGridALL();
  }, [GridDto]);

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
          <LCMHardwareSupportTypeForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></LCMHardwareSupportTypeForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold  ">Hardware Support Type</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Hardware Support Type
          </button>
        </div>
      </div>

      <div className="">
        <LCMHardwareSupportTypeGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ Delete, Edit, Filter: setQuery }}
        ></LCMHardwareSupportTypeGrid>
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

export default LCMHardwareSupportType;
