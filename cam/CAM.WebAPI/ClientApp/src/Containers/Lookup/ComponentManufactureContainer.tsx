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
import ComponentManufacturerForm from "../../screen/Lookup/OriginalEquipmentManufacturer/ComponentManufacturerForm";
import ComponentManufacturerGrid from "../../screen/Lookup/OriginalEquipmentManufacturer/ComponentManufacturerGrid";
import { GetComponentManufacturerCreateResource } from "../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerCreateAction";
import {
  DeleteDeepComponentManufacturer,
  GetRelatedRecordsComponentManufacturer,
} from "../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerDeleteAction";
import { GetCompoentManufacturerEditResource } from "../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerEditAction";
import {
  GetComponentManufacturerGridALL,
  GetCompoentManufacturerGrid,
} from "../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
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
import { Box } from "@mui/material";
import { IoClose } from "react-icons/io5";

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
  lookUpFlag?: string;
}

const ComponentManufacturer: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.componentManufacturerGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.componentManufacturerGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetCompoentManufacturerGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetCompoentManufacturerGrid : undefined
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
    GetComponentManufacturerCreateResource,
    GetCompoentManufacturerEditResource,
    DeleteDeepComponentManufacturer,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetCompoentManufacturerGrid(paginationQueryTipologiche);
  // }, []);

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
      console.log("data copy => ", dataCopy);
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    console.log("GridDto => ", GridDto);
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      console.log(GridDto);
    }
    // GetComponentManufacturerGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsComponentManufacturer(id);
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
              {edit
                ? "Edit Component Manufacturer"
                : "New Component Manufacturer"}
            </h4>
          </div>
        </DialogTitle>
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                chiudiModal();
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          <ComponentManufacturerForm
            lookUpFlag={props?.lookUpFlag}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></ComponentManufacturerForm>
        </DialogContent>
      </Dialog>
      <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
        <IconButton
          aria-label="close"
          onClick={() => {
            chiudiModal();
          }}
        >
          <IoClose size={25} />
        </IconButton>
      </Box>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Component</h3>
        </div>
        <div className="">
          <button
            className="voda-bold btn btn-danger px-4 btnHeader fz-14"
            onClick={New}
            type="button"
          >
            New Component
          </button>
        </div>
      </div>
      <div className="">
        <ComponentManufacturerGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></ComponentManufacturerGrid>
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

export default ComponentManufacturer;
