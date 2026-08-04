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
// import SystemNameForm from "../../screen/Lookup/SystemName/SystemNameForm";
import SystemNamesForm from "../../screen/Lookup/Domain/DomainForm";
import SystemNamesGrid from "../../screen/Lookup/Domain/DomainGrid";
import { GetSystemNameCreateResource } from "../../Redux/Action/LookUp/Domain/DomainCreateAction";
import {
  deleteDeepSystemName,
  getRelatedRecordsSystemName,
} from "../../Redux/Action/LookUp/Domain/DomainDeleteAction";
import { GetSystemNameEditResource } from "../../Redux/Action/LookUp/Domain/DomainEditAction";
import {
  GetSystemNameGrid,
  GetSystemNameGridALL,
} from "../../Redux/Action/LookUp/Domain/DomainGridAction";

import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import {
  SystemNamesDto,
  SystemNamesDtoGrid,
  SystemNameQuery,
} from "../../Model/LookUp/Domain";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: SystemNameQuery = {
  systemNameId: [],
  systemNameDescription: [],
  sortBy: undefined,
  isSortAscending: undefined,
  page: undefined,
  pageSize: undefined,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
};

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: Array<any>): any;
}

const SystemName: React.FC<Props> = (props) => {
  const { isPermesso, pageSize } = useAuth();

  // DTO State
  const [data, setData] = useState<SystemNamesDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();

  const Grid = (state: RootState) => state.systemGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);

  const GridAll = (state: RootState) =>
    state.systemGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  useEffect(() => {
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetSystemNameGrid(query);
  };

  // Pagination and filtering
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetSystemNameGrid : undefined
  );

  // Modal CRUD hooks
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
  } = useOperationTableCrud<SystemNamesDto, SystemNamesDto>(
    GetSystemNameCreateResource,
    GetSystemNameEditResource,
    deleteDeepSystemName,
    refresh
  );

  // Reset query
  const resetQuery = () => {
    setQuery(paginationQuery);
  };

  const chiudiModal = () => {
    if (props.returnObject) {
      let dataCopy = [...(GridDto?.items ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  useEffect(() => {
    if (GridDto !== undefined) {
      console.log(GridDto);
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await getRelatedRecordsSystemName(id);
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
              {edit ? "Edit System Name" : "Create System Name"}
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
          <SystemNamesForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          />
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">System Names</h3>
        </div>
        <div>
          <button
            className="voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New System Name
          </button>
        </div>
      </div>

      <div>
        <SystemNamesGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
        />
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>

      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end d-flex footerModal">
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

export default SystemName;
