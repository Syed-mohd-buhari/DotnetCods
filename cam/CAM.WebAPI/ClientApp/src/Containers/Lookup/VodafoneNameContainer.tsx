import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import AdditionalFiltersMenu from "../../Components/AdditionalFiltersMenu";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import Paginate from "../../Components/PaginationComponent";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useAuth } from "../../Hook/useAuth";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { CustomGridRender } from "../../Model/Common";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import {
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../Model/LookUp/LookUpGenericModel";
import { GetVodafoneNameCreateResource } from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameCreateAction";
import {
  DeleteDeepVodafoneName,
  GetRelatedRecordsVodafoneName,
} from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameDeleteAction";
import { GetVodafoneNameDownload } from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameDownloadAction";
import { GetVodafoneNameEditResource } from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameEditAction";
import {
  GetVodafoneNameGrid,
  GetVodafoneNameGridALL,
} from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import VodafoneNameForm from "../../screen/Lookup/VodafoneName/VodafoneNameForm";
import VodafoneNameGrid from "../../screen/Lookup/VodafoneName/VodafoneNameGrid";
import SetupColumns from "../../screen/Shared/SetupColumns";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQueryTipologiche: TipologicheQueryObjectGrid = {
  id: [],
  description: [],
  productName: [],
  riskCluster: [],
  riskLevel: [],
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
  showButtons?: boolean;
  isPopup?: boolean;
}

const VodafoneName: React.FC<Props> = (props) => {
  const { readonly, isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [orphanColor, setOrphanColor] = useState(false);

  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.vodafoneNameGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.vodafoneNameGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [isVisibleModalRefactor, setIsVisibleModalRefactor] = useState(false);

  const refresh = () => {
    closeModal();
    GetVodafoneNameGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetVodafoneNameGrid : undefined
  );

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
    GetVodafoneNameCreateResource,
    GetVodafoneNameEditResource,
    DeleteDeepVodafoneName,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    // GetVodafoneNameGrid(paginationQueryTipologiche);
  }, []);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const InvocheDownload = async () => {
    let result = await GetVodafoneNameDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const closeModalSetup = (changed: boolean) => {
    GetVodafoneNameGrid(query).then((x) => setIsVisibleModalSetup(false));
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
    }
    // GetVodafoneNameGridALL();
  }, [GridDto]);

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsVodafoneName(id);
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
              {edit ? "Edit Vodafone Name" : "Create Vodafone Name"}
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
          <VodafoneNameForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></VodafoneNameForm>
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
          {props.isPopup ? (
            <h3 className={"text-dark voda-bold"}>Vodafone Name</h3>
          ) : (
            <h3 className={"voda-bold"}>SW App - VF Name</h3>
          )}
        </div>

        <div className="d-flex">
          {!props?.showButtons && !readonly && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn
              "
              onClick={New}
              type="button"
            >
              <img src={require("../../img/plus_1.png")} className="img-15" />
              <span className="fz-14">New Vodafone Name</span>
            </button>
          )}

          {props?.showButtons && (
            <>
              <button
                className="download-to-excel mrl-10 grid-main-btn
                "
                onClick={() => InvocheDownload()}
              >
                {/* <img src={require("../img/excel.png")} /> */}
                Download to Excel
              </button>

              {!readonly && (
                <Dropdown
                  className="d-inline more-options grid-main-btn
                "
                >
                  <Dropdown.Toggle id="dropdown-autoclose-inside">
                    More Options
                  </Dropdown.Toggle>

                  <Dropdown.Menu
                    className="grid-main-btn
"
                  >
                    <Dropdown.Item>Preview Orphans</Dropdown.Item>
                    <AdditionalFiltersMenu
                      query={query}
                      orphanColored={orphanColor}
                      action={{
                        setQuery: setQuery,
                        setIsVisible: setIsVisibleAdditionalFilter,
                        getGrid: GetVodafoneNameGrid,
                        setOrphanColor,
                      }}
                    ></AdditionalFiltersMenu>

                    <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                      Manage Table Content
                    </Dropdown.Item>
                  </Dropdown.Menu>
                </Dropdown>
              )}
            </>
          )}
        </div>
      </div>
      <div className="">
        <VodafoneNameGrid
          showButtons={props?.showButtons}
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          orphanColor={orphanColor}
          isPopup={props.isPopup}
        ></VodafoneNameGrid>
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

export default VodafoneName;
