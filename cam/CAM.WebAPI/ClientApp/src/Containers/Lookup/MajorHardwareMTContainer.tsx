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
import MajorHardwareMTForm from "../../screen/Lookup/MajorHardwareMT/MajorHardwareMTForm";
import MajorHardwareMTGrid from "../../screen/Lookup/MajorHardwareMT/MajorHardwareMTGrid";
import { GetMajorHardwareMTCreateResource } from "../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTCreateAction";
import {
  DeleteDeepMajorHardwareMT,
  GetRelatedRecordsMajorHardwareMT,
  DeleteMajorHardwareMT,
} from "../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTDeleteAction";
import {
  GetMajorHardwareMTGrid,
  GetMajorHardwareMTGridALL,
} from "../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto, ResultDto } from "../../Model/CommonModels";
import {
  MajorHardwareMTQueryObjectGrid,
  MajorHardwareMTDtoGrid,
} from "../../Model/LookUp/MajorHardwareMT";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GetMajorHardwareMTEditResource } from "../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTEditAction";

export let paginationQueryTipologiche: MajorHardwareMTQueryObjectGrid = {
  majorHardwareBuildAsisId: [],
  orgEqpManuFacturerId: [],
  orgEqpManuFacturerDesc: [],
  hardwareSolution: [],
  hardwareSolutionReourceId: [],
  platformId: [],
  buildConstructionId: [],
  hardwareType: [],

  lastModified: undefined,
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

const MajorHardwareMT: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<MajorHardwareMTDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.majorHardwareMTGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.majorHardwareMTGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetMajorHardwareMTGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetMajorHardwareMTGrid : undefined
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
  } = useOperationTableCrud<MajorHardwareMTDtoGrid, MajorHardwareMTDtoGrid>(
    GetMajorHardwareMTCreateResource,
    GetMajorHardwareMTEditResource,
    DeleteDeepMajorHardwareMT,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetLocationGrid(paginationQueryTipologiche);
  //   GetLocationGridALL();
  // }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.data?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.data?.items);
      let copy = { ...GridDto?.data?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridState(copy);
      // GetLocationGridALL();
    }
  }, [GridDto]);
  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsMajorHardwareMT(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  return (
    <div>
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
              {edit ? " Edit Major Hardware" : "Add Major Hardware"}
            </h4>
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
          <MajorHardwareMTForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></MajorHardwareMTForm>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Major Hardware</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Major Hardware
          </button>
        </div>
      </div>

      <div className="">
        <MajorHardwareMTGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ Delete: onDelete, Edit, Filter: setQuery }}
        ></MajorHardwareMTGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.data?.totalItems}
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

export default MajorHardwareMT;
