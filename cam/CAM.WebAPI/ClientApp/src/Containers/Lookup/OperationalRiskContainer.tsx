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
import OperationalRiskForm from "../../screen/Lookup/OperationalRisk/OperationalRiskForm";
import OperationalRiskGrid from "../../screen/Lookup/OperationalRisk/OperationalRiskGrid";
import { GetOperationalRiskCreateResource } from "../../Redux/Action/LookUp/OperationalRisk/OperationalRiskCreateAction";
import {
  DeleteDeepOperationalRisk,
  GetRelatedRecordsOperationalRisk,
} from "../../Redux/Action/LookUp/OperationalRisk/OperationalRiskDeleteAction";
import { ChangeGridOrderOperationalRisk } from "../../Redux/Action/LookUp/OperationalRisk/OperationalRiskCommonAction";
import { GetOperationalRiskEditResource } from "../../Redux/Action/LookUp/OperationalRisk/OperationalRiskEditAction";
import {
  GetOperationalRiskGrid,
  GetOperationalRiskGridALL,
} from "../../Redux/Action/LookUp/OperationalRisk/OperationalRiskGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import {
  OperationalRiskDto,
  OperationalRiskQueryDto,
} from "../../Model/LookUp/OperationalRisk";
import SetupOrderGrid from "../../screen/Shared/SetupOrderGrid";
import {
  ChangeGridOrderDto,
  RelatedRecordsResultDto,
} from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQueryTipologiche: OperationalRiskQueryDto = {
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

// let severityResource = [{ key: 0, value: "NONE" }, { key: 1, value: "LOW" }, { key: 2, value: "MID" }, { key: 3, value: "HIGH" }]

const OperationalRisk: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<OperationalRiskDto[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.operationalRiskGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.operationalRiskGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const refresh = () => {
    closeModal();
    GetOperationalRiskGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetOperationalRiskGrid : undefined
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
  } = useOperationTableCrud<OperationalRiskDto, OperationalRiskDto>(
    GetOperationalRiskCreateResource,
    GetOperationalRiskEditResource,
    DeleteDeepOperationalRisk,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  // useEffect(() => {
  //   GetOperationalRiskGrid(paginationQueryTipologiche);
  //   GetOperationalRiskGridALL();
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
      // GetOperationalRiskGridALL();
    }
  }, [GridDto]);

  //ORDER GRID
  const [isSetupOrder, setIsSetupOrder] = useState<boolean>(false);
  const [changedOrder, SetChangedOrder] = useState<boolean>(false);
  const [isConfirmOrder, SetIsConfirmOrder] = useState<boolean>(false);
  const [dataOrder, SetDataOrder] = useState<ChangeGridOrderDto[]>([]);

  const SaveOrderGrid = async () => {
    await ChangeGridOrderOperationalRisk(dataOrder).then((x) => {
      if (!x?.warning) {
        setIsSetupOrder(false);
        SetIsConfirmOrder(false);
        refresh();
      }
    });
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsOperationalRisk(id);
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
              {edit ? " Edit Risk Evaluation" : "Add Risk Evaluation"}
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
          <OperationalRiskForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></OperationalRiskForm>
        </DialogContent>
      </Dialog>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Risk Evaluation</h3>
        </div>
        <div className="">
          <button
            disabled={isSetupOrder}
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Risk Evaluation
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
      <div className="">
        {isSetupOrder == true ? (
          <SetupOrderGrid
            action={{
              SetChangedOrder,
              SetDataOrder,
              setIsSetupOrder,
              SetIsConfirmOrder,
            }}
            renderGrid={renderGridState}
            propertyOrder="severity"
            data={data}
            isConfirmOrder={isConfirmOrder}
          ></SetupOrderGrid>
        ) : (
          <div>
            <OperationalRiskGrid
              data={data}
              pagination={query}
              renderGrid={renderGridState?.render ?? []}
              action={{ onDelete, Edit, Filter: setQuery }}
            ></OperationalRiskGrid>
            <Paginate
              pagination={{ page: query.page, pageSize: query.pageSize }}
              totalItems={GridDto?.totalItems}
              actions={{ next, back }}
            />
          </div>
        )}
      </div>
      {props.modal && props.modal.isModal ? (
        isSetupOrder == true ? (
          <div className="col-12 justify-content-end mt-2 d-flex footerModal">
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

export default OperationalRisk;
