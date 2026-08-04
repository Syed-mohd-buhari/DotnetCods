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
import TypeForm from "../../screen/Lookup/Type/TypeForm";
import TypeGrid from "../../screen/Lookup/Type/TypeGrid";
import { GetTypeCreateResource } from "../../Redux/Action/LookUp/Type/TypeCreateAction";
import {
  DeleteDeepType,
  GetRelatedRecordsType,
} from "../../Redux/Action/LookUp/Type/TypeDeleteAction";
import { GetTypeEditResource } from "../../Redux/Action/LookUp/Type/TypeEditAction";
import {
  GetTypeGrid,
  GetTypeGridALL,
} from "../../Redux/Action/LookUp/Type/TypeGridAction";
import { TypeDtoGrid, TypeQueryObjectGrid } from "../../Model/LookUp/Type";
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

export let paginationQueryTipologiche: TypeQueryObjectGrid = {
  id: [],
  classId: [],
  description: [],
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  sortBy: "",
  isSortAscending: false,
  classDescription: [],
  page: 1,
  pageSize: 10,
  categoryDescription: [],
  lastModifiedBy: [],
};

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

const Type: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TypeDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) => state.TypeGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.TypeGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);
  console.log(GridDtoAll, "GridDtoAll");
  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetTypeGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetTypeGrid : undefined
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
  } = useOperationTableCrud<TypeDtoGrid, TypeDtoGrid>(
    GetTypeCreateResource,
    GetTypeEditResource,
    DeleteDeepType,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetTypeGrid(paginationQueryTipologiche);
  //   GetTypeGridALL();
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
    console.log(GridDto, "GridDto");

    // GetTypeGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsType(id);
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
            <h4 className="mb-0">{edit ? "Edit Type" : "Add Type"}</h4>
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
          <TypeForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></TypeForm>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark ">Type</h3>
        </div>
        <div className="">
          <button
            className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab"
            onClick={New}
            type="button"
          >
            <img src={require("../../img/Plus_white.png")} className="img-15" />
            <span className="fz-14">New Type</span>
          </button>
        </div>
      </div>

      <div className="">
        <TypeGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></TypeGrid>
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

export default Type;
