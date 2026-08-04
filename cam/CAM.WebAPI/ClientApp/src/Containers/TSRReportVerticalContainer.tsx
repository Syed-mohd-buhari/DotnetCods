import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import { GetProductImportanceEditResource } from "../Redux/Action/LookUp/ProductImportance/ProductImportanceEditAction";
import {
  GetProductImportanceGrid,
  GetProductImportanceGridALL,
} from "../Redux/Action/LookUp/ProductImportance/ProductImportanceGridAction";
import {
  DeleteDeepProductImportance,
  GetRelatedRecordsProductImportance,
} from "../Redux/Action/LookUp/ProductImportance/ProductImportanceDeleteAction";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { useAuth } from "../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GetTSRReportVerticalGrid } from "../Redux/Action/TSRReport/TemsTSRReportGridAction";
import TSRReportVerticalGrid from "../screen/TSRReport/TSRReportVerticalGrid";
import TSRVerticalForm from "../screen/TSRReport/TSRVerticalForm";
import { GetTSRReportVerticalCreateResource } from "../Redux/Action/TSRReport/TSRVerticalGridCreateAction";
import { GetTSRVerticalEditResource } from "../Redux/Action/TSRReport/TSRVerticalEditAction";
import { DeleteDeepTSRVertical } from "../Redux/Action/TSRReport/TSRVerticalDeleteAction";

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

export let paginationQueryTipologiche: TipologicheQueryObjectGrid = {
  appSettingsId: [2],
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  lastModifiedBy: [],
};

const TSRReportVerticalContainer: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.tSRReportVerticalGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.productImportanceGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetTSRReportVerticalGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetTSRReportVerticalGrid : undefined
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
  } = useOperationTableCrud<TipologicaGridDto, TipologicaGridDto>(
    GetTSRReportVerticalCreateResource,
    GetTSRVerticalEditResource,
    DeleteDeepTSRVertical,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetTSRReportVerticalGrid(paginationQueryTipologiche);
  // }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
    // GetProductImportanceGridALL();
  }, [GridDto]);

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  const onDelete = async (id: number) => {
    Delete(id);
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
            <h4 className="mb-0">{edit ? "Edit Vertical" : "New Vertical"}</h4>
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
          <TSRVerticalForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></TSRVerticalForm>
        </DialogContent>
      </Dialog>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Vertical for Non-Tems</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            Add New Vertical
          </button>
        </div>
      </div>
      <div className="">
        <TSRReportVerticalGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></TSRReportVerticalGrid>
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

export default TSRReportVerticalContainer;
