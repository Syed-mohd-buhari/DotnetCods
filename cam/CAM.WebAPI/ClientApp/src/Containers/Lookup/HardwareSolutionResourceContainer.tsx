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
import HardwareSolutionResourceForm from "../../screen/Lookup/HardwareSolutionResource/HardwareSolutionResourceForm";
import HardwareSolutionResourceGrid from "../../screen/Lookup/HardwareSolutionResource/HardwareSolutionResourceGrid";
import { GetHardwareSolutionResourceCreateResource } from "../../Redux/Action/LookUp/HardwareSolutionResource/HardwareSolutionResourceCreateAction";
import {
  DeleteDeepHardwareSolutionResource,
  deleteHardwareSolutionResource,
  GetRelatedRecordsHardwareSolutionResource,
} from "../../Redux/Action/LookUp/HardwareSolutionResource/HardwareSolutionResourceDeleteAction";
import { GetHardwareSolutionResourceEditResource } from "../../Redux/Action/LookUp/HardwareSolutionResource/HardwareSolutionResourceEditAction";
import {
  GetHardwareSolutionResourceGrid,
  GetHardwareSolutionResourceGridALL,
} from "../../Redux/Action/LookUp/HardwareSolutionResource/HardwareSolutionResourceGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../Model/LookUp/LookUpGenericModel";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import { Box, IconButton } from "@mui/material";
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
}

const HardwareSolutionResource: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.hardwareSolutionResourceGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.hardwareSolutionResourceGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetHardwareSolutionResourceGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetHardwareSolutionResourceGrid : undefined
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
    GetHardwareSolutionResourceCreateResource,
    GetHardwareSolutionResourceEditResource,
    DeleteDeepHardwareSolutionResource,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    GetHardwareSolutionResourceGrid(paginationQueryTipologiche);
    GetHardwareSolutionResourceGridALL();
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
      GetHardwareSolutionResourceGridALL();
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsHardwareSolutionResource(id);
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
                {edit ? " Edit Lookup Record" : "Create Lookup Record"}
              </h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body>
          <HardwareSolutionResourceForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></HardwareSolutionResourceForm>
        </Modal.Body>
      </Modal>

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
          <h3 className="voda-bold  ">Hardware Solution</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Hardware Solution
          </button>
        </div>
      </div>

      <div className="">
        <HardwareSolutionResourceGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        ></HardwareSolutionResourceGrid>
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

export default HardwareSolutionResource;
