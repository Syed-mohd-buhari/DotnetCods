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
import DeliveryStatusForm from "../../screen/Lookup/DeliveryStatus/DeliveryStatusForm";
import DeliveryStatusGrid from "../../screen/Lookup/DeliveryStatus/DeliveryStatusGrid";
import { GetDeliveryStatusCreateResource } from "../../Redux/Action/LookUp/DeliveryStatus/DeliveryStatusCreateAction";
import {
  DeleteDeepDeliveryStatus,
  GetRelatedRecordsDeliveryStatus,
} from "../../Redux/Action/LookUp/DeliveryStatus/DeliveryStatusDeleteAction";
import { GetDeliveryStatusEditResource } from "../../Redux/Action/LookUp/DeliveryStatus/DeliveryStatusEditAction";
import {
  GetDeliveryStatusGrid,
  GetDeliveryStatusGridALL,
} from "../../Redux/Action/LookUp/DeliveryStatus/DeliveryStatusGridAction";
import {
  TipologicheQueryObjectGridRule,
  TipologicaGridDtoRule,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQueryTipologiche: TipologicheQueryObjectGridRule = {
  id: [],
  description: [],
  rule: [],
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

const DeliveryStatus: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDtoRule[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.deliveryStatusGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.deliveryStatusGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const refresh = () => {
    closeModal();
    GetDeliveryStatusGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetDeliveryStatusGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

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
  } = useOperationTableCrud<TipologicaGridDtoRule, TipologicaGridDtoRule>(
    GetDeliveryStatusCreateResource,
    GetDeliveryStatusEditResource,
    DeleteDeepDeliveryStatus,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetDeliveryStatusGrid(paginationQueryTipologiche);
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
    // GetDeliveryStatusGridALL();
  }, [GridDto]);

  const rulesResource = [
    { key: 0, value: "NO Rule" },
    { key: 1, value: "Complete" },
  ];

  const rulesProjectStatus = [{ key: 1, value: "Do Not Check This" }];

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsDeliveryStatus(id);
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
      <Dialog
        open={isVisibleModal}
        onClose={() => closeModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">
              {edit ? " Edit Delivery Status" : "Add Delivery Status"}
            </h4>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeModal(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <DeliveryStatusForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
            rulesProjectStatus={rulesProjectStatus}
          ></DeliveryStatusForm>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Delivery Status</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Delivery Status
          </button>
        </div>
      </div>

      <div className="">
        <DeliveryStatusGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          rules={rulesResource}
          rulesProjectStatus={rulesProjectStatus}
        ></DeliveryStatusGrid>
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

export default DeliveryStatus;
