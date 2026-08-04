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
import NFVIBundleIDForm from "../../screen/Lookup/NFVIBundleID/NFVIBundleIDForm";
import NFVIBundleIDGrid from "../../screen/Lookup/NFVIBundleID/NFVIBundleIDGrid";
import { GetNFVIBundleIDCreateResource } from "../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDCreateAction";
import {
  DeleteDeepNFVIBundleID,
  deleteNFVIBundleID,
  GetRelatedRecordsNFVIBundleID,
} from "../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDDeleteAction";
import { GetNFVIBundleIDEditResource } from "../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDEditAction";
import {
  GetNFVIBundleIDGrid,
  GetNFVIBundleIDGridALL,
} from "../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import {
  NFVIBundleIDDtoGrid,
  NFVIBundleIDQueryObjectGrid,
} from "../../Model/LookUp/NFVIBundleId";
import SetupOrderGrid from "../../screen/Shared/SetupOrderGrid";
import {
  ChangeGridOrderDto,
  RelatedRecordsResultDto,
} from "../../Model/CommonModels";
import { ChangeGridNFVIBundleID } from "../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDCommonAction";
import { useAuth } from "../../Hook/useAuth";

export let paginationQueryTipologiche: NFVIBundleIDQueryObjectGrid = {
  id: [],
  description: [],
  order: [],
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  sortBy: "order",
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

const NFVIBundleID: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<NFVIBundleIDDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.nFVIBundleIDGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.nFVIBundleIDGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetNFVIBundleIDGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetNFVIBundleIDGrid : undefined
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
  } = useOperationTableCrud<NFVIBundleIDDtoGrid, NFVIBundleIDDtoGrid>(
    GetNFVIBundleIDCreateResource,
    GetNFVIBundleIDEditResource,
    DeleteDeepNFVIBundleID,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetNFVIBundleIDGrid(paginationQueryTipologiche);
  //   GetNFVIBundleIDGridALL();
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
      // GetNFVIBundleIDGridALL();
    }
  }, [GridDto]);

  //ORDER GRID
  const [isSetupOrder, setIsSetupOrder] = useState<boolean>(false);
  const [changedOrder, SetChangedOrder] = useState<boolean>(false);
  const [isConfirmOrder, SetIsConfirmOrder] = useState<boolean>(false);
  const [dataOrder, SetDataOrder] = useState<ChangeGridOrderDto[]>([]);

  const SaveOrderGrid = async () => {
    await ChangeGridNFVIBundleID(dataOrder).then((x) => {
      if (!x?.warning) {
        setIsSetupOrder(false);
        SetIsConfirmOrder(false);
        refresh();
      }
    });
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsNFVIBundleID(id);
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
              {edit ? "Edit NFVI Bundle ID" : "Add NFVI Bundle ID"}
            </h4>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <NFVIBundleIDForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></NFVIBundleIDForm>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">NFVI Bundle ID</h3>
        </div>
        <div className="">
          <button
            disabled={isSetupOrder}
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New NFVI Bundle ID
          </button>
          <button
            disabled={isSetupOrder}
            className="  voda-bold btn btn-danger ml-2 px-4 btnHeader"
            onClick={() => setIsSetupOrder(true)}
            type="button"
          >
            <img
              style={{ height: 24, marginBottom: "3px" }}
              src={require("../../img/sort.png")}
              alt="sort"
            />
          </button>
        </div>
      </div>
      {isSetupOrder == true ? (
        <SetupOrderGrid
          action={{
            SetChangedOrder,
            SetDataOrder,
            setIsSetupOrder,
            SetIsConfirmOrder,
          }}
          renderGrid={renderGridState}
          propertyOrder="order"
          data={data}
          isConfirmOrder={isConfirmOrder}
        ></SetupOrderGrid>
      ) : (
        <div className="">
          <NFVIBundleIDGrid
            data={data}
            pagination={query}
            renderGrid={renderGridState?.render ?? []}
            action={{ onDelete, Edit, Filter: setQuery }}
          ></NFVIBundleIDGrid>
          <Paginate
            pagination={{ page: query.page, pageSize: query.pageSize }}
            totalItems={GridDto?.totalItems}
            actions={{ next, back }}
          />
        </div>
      )}
      {props.modal && props.modal.isModal ? (
        isSetupOrder == true ? (
          <div className="col-12 justify-content-end d-flex footerModal">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel mr-3"
              type="button"
              onClick={() =>
                changedOrder ? SetIsConfirmOrder(true) : setIsSetupOrder(false)
              }
            >
              Cancel
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => SaveOrderGrid()}
            >
              Save
            </button>
          </div>
        ) : (
          <div className="col-12 justify-content-end d-flex footerModal">
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => chiudiModal()}
            >
              Close
            </button>
          </div>
        )
      ) : null}
    </div>
  );
};

export default NFVIBundleID;
