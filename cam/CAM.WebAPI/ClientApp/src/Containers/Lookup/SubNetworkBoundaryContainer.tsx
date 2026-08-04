import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import Paginate from "../../Components/PaginationComponent";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import {
  SubNetworkBoundaryGridDto,
  SubNetworkBoundaryQueryDto,
} from "../../Model/LookUp/SubnetworkBoundry";
import { GetSubNetworkBoundaryCreateResource } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryCreateAction";
import {
  DeleteDeepSubNetworkBoundary,
  GetRelatedRecordsSubNetworkBoundary,
} from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryDeleteAction";
import { GetSubNetworkBoundaryEditResource } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundaryEditAction";
import { GetSubNetworkBoundaryGrid } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import SubNetworkBoundaryForm from "../../screen/Lookup/SubNetworkBoundary/SubNetworkBoundaryForm";
import SubNetworkBoundaryGrid from "../../screen/Lookup/SubNetworkBoundary/SubNetworkBoundaryGrid";
import AdditionalFiltersMenu from "../../Components/AdditionalFiltersMenu";
import { useAuth } from "../../Hook/useAuth";
import { GetSubnetworkBoundaryReport } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubnetworkBoundaryDownloadAction";
import SetupColumns from "../../screen/Shared/SetupColumns";
import SubNetworkBoundaryGridPopUp from "../../screen/Lookup/SubNetworkBoundary/SubNetworkBoundaryGridPopUp";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery = {
  subNetworkBoundaryId: undefined,
  vodafoneName: undefined,
  subNetworkBoundaryDescription: undefined,
  sortBy: undefined,
  isSortAscending: undefined,
  page: 1,
  pageSize: 10,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: undefined,
  options: undefined,
} as SubNetworkBoundaryQueryDto;

interface Props {
  vodafoneNameId?: number;
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: SubNetworkBoundaryGridDto[] | undefined): any;
  productName?: string;
  showButtons?: boolean;
  isPopup?: boolean;
}

const SubNetworkBoundary: React.FC<Props> = (props) => {
  const { RefactorUser, readonly, tipologicaPermesso, isPermesso, pageSize } =
    useAuth();
  //DTO
  const [data, setData] = useState<SubNetworkBoundaryGridDto[] | undefined>([]);
  const [orphanColor, setOrphanColor] = useState(false);

  const Grid = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResult;
  const GridAll = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResultAll;

  const GridDto = useSelector(Grid);
  const GridDtoAll = useSelector(GridAll);

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetSubNetworkBoundaryGrid(query);
  };

  let updatedQuery = {
    ...paginationQuery,
    vodafoneName: props?.vodafoneNameId ? [props?.vodafoneNameId] : undefined,
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    updatedQuery,
    isPermesso ? GetSubNetworkBoundaryGrid : undefined
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
  } = useOperationTableCrud<
    SubNetworkBoundaryGridDto,
    SubNetworkBoundaryGridDto
  >(
    GetSubNetworkBoundaryCreateResource,
    GetSubNetworkBoundaryEditResource,
    DeleteDeepSubNetworkBoundary,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1

  const resetQuery = () => {
    setQuery(paginationQuery);
  };

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);

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
      if (props.productName) {
        GridDto?.items?.push({});
      }
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  const InvocheDownload = async () => {
    let result = await GetSubnetworkBoundaryReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsSubNetworkBoundary(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  const closeModalSetup = (changed: boolean) => {
    GetSubNetworkBoundaryGrid(query).then((x) => setIsVisibleModalSetup(false));
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
              {edit ? "Edit SubNetwork Boundary" : "Add SubNetwork Boundary"}
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
          <SubNetworkBoundaryForm
            vodafoneNameId={props?.vodafoneNameId}
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            productName={props?.productName}
          ></SubNetworkBoundaryForm>
        </DialogContent>
      </Dialog>

      <Modal
        show={isVisibleModalSetup}
        backdrop="static"
        keyboard={false}
        size="lg"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        <Modal.Body className="plr-30">
          <SetupColumns
            renderGrid={renderGridState}
            action={{ closeModalSetup }}
          ></SetupColumns>
        </Modal.Body>
      </Modal>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3
            className={`${props.isPopup ? "text-dark voda-bold" : "voda-bold"}`}
          >
            SubNetwork Boundary
          </h3>
        </div>
        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader grid-main-btn"
              onClick={New}
              type="button"
            >
              New SubNetwork Boundary
            </button>
          )}

          {props?.showButtons && (
            <>
              <button
                className="download-to-excel mrl-10 grid-main-btn"
                onClick={() => InvocheDownload()}
              >
                Download to Excel
              </button>

              <Dropdown className="d-inline more-options grid-main-btn">
                <Dropdown.Toggle id="dropdown-autoclose-inside">
                  More Options
                </Dropdown.Toggle>

                <Dropdown.Menu className="grid-main-btn">
                  {!readonly && (
                    <>
                      <Dropdown.Item>Preview Orphans</Dropdown.Item>
                      <AdditionalFiltersMenu
                        query={query}
                        orphanColored={orphanColor}
                        action={{
                          setQuery: setQuery,
                          setIsVisible: setIsVisibleAdditionalFilter,
                          getGrid: GetSubNetworkBoundaryGrid,
                          setOrphanColor,
                        }}
                      ></AdditionalFiltersMenu>
                    </>
                  )}

                  <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                    Manage Table Content
                  </Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </>
          )}
        </div>
      </div>

      <div className="">
        <SubNetworkBoundaryGrid
          data={data}
          pagination={query}
          orphanColor={orphanColor}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          readonly={readonly}
          isPopup={props.isPopup}
        ></SubNetworkBoundaryGrid>

        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end d-flex footerModal">
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

export default SubNetworkBoundary;
