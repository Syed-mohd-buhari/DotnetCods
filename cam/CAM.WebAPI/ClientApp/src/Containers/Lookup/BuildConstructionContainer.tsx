import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";

import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { useSelector } from "react-redux";

import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import Paginate from "../../Components/PaginationComponent";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import {
  TipologicaGridDtoRule,
  TipologicheQueryObjectGridRule,
} from "../../Model/LookUp/LookUpGenericModel";
import { GetBuildConstructionCreateResource } from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCreateAction";
import {
  DeleteDeepBuildConstruction,
  GetRelatedRecordsBuildConstruction,
} from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionDeleteAction";
import { GetBuildConstructionEditResource } from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionEditAction";
import {
  GetBuildConstructionGrid,
  GetBuildConstructionGridALL,
} from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import BuildConstructionForm from "../../screen/Lookup/BuildConstruction/BuildConstructionForm";
import BuildConstructionGrid from "../../screen/Lookup/BuildConstruction/BuildConstructionGrid";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

export let paginationQueryTipologiche: TipologicheQueryObjectGridRule = {
  id: [],
  description: [],
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  rule: [],
  lastModifiedBy: [],
};

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

const BuildConstruction: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDtoRule[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.buildConstructionGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.buildConstructionGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetBuildConstructionGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetBuildConstructionGrid : undefined
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
  } = useOperationTableCrud<TipologicaGridDtoRule, TipologicaGridDtoRule>(
    GetBuildConstructionCreateResource,
    GetBuildConstructionEditResource,
    DeleteDeepBuildConstruction,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetBuildConstructionGrid(paginationQueryTipologiche);
  //   GetBuildConstructionGridALL();
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
      // GetBuildConstructionGridALL();
    }
  }, [GridDto]);

  const rulesResource = [
    { key: 0, value: "No Rule" },
    { key: 1, value: "As Proprietary HW" },
    { key: 2, value: "As COTS or Other" },
    { key: 3, value: "As NFVI" },
    { key: 4, value: "As NFCI" },
  ];

  const cloudTypeBuildResource = [
    { key: 0, value: "Private Cloud" },
    { key: 1, value: "Public Cloud" },
    { key: 2, value: "Hybrid" },
  ];

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsBuildConstruction(id);
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
        maxWidth="md"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">
              {edit ? "Edit Build Construction" : "Create Build Construction"}
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
          <BuildConstructionForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
            cloudTypeBuild={cloudTypeBuildResource}
          ></BuildConstructionForm>
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
          <h3 className="voda-bold text-dark">Build Construction</h3>
        </div>
        <div className="">
          <button
            className="fz-20 voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Build Construction
          </button>
        </div>
      </div>

      <div className="">
        <BuildConstructionGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          rules={rulesResource}
        ></BuildConstructionGrid>
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

export default BuildConstruction;
