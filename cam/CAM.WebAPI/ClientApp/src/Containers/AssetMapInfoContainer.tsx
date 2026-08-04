import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import setLoader from "../Redux/Action/LoaderAction";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import AssetMapInfoForm from "../screen/Lookup/AssetMapInfo/AssetMapInfoForm";
import AssetMapInfoGrid from "../screen/Lookup/AssetMapInfo/AssetMapInfoGrid";
import { GetAssetMapInfoCreateResource } from "../Redux/Action/LookUp/AssetMapInfo/AssetMapInfoCreateAction";
import {
  DeleteDeepAssetMapInfo,
  DeleteAssetMapInfo,
} from "../Redux/Action/LookUp/AssetMapInfo/AssetMapInfoDeleteAction";
import {
  GetAssetMapInfoGrid,
  GetAssetMapInfoGridALL,
} from "../Redux/Action/LookUp/AssetMapInfo/AssetMapInfoGridAction";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import { RelatedRecordsResultDto, ResultDto } from "../Model/CommonModels";
import {
  AssetMapInfoQueryObjectGrid,
  AssetMapInfoDtoGrid,
} from "../Model/LookUp/AssetMapInfo";
import { useAuth } from "../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GetAssetMapInfoExport } from "../Redux/Action/LookUp/AssetMapInfo/AssetMapInfoDownloadAction";
import { GetAssetMapInfoEditResource } from "../Redux/Action/LookUp/AssetMapInfo/AssetMapInfoEditAction";

export let paginationQueryTipologiche: AssetMapInfoQueryObjectGrid = {
  assetMapInfoId: [],
  temsAssetName: [],
  enmAssetName: [],
  site: [],
  dataSourceName: [],
  omcAssetName: [],

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

const AssetMapInfo: React.FC<Props> = (props) => {
  //DTO
  const [data, setData] = useState<AssetMapInfoDtoGrid[] | undefined>([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.assetMapInfoGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.assetMapInfoGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const { readonly, tipologicaPermesso, isPermesso } = useAuth();

  const refresh = () => {
    closeModal();
    GetAssetMapInfoGrid(query);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologiche,
    isPermesso ? GetAssetMapInfoGrid : undefined
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
  } = useOperationTableCrud<AssetMapInfoDtoGrid, AssetMapInfoDtoGrid>(
    GetAssetMapInfoCreateResource,
    GetAssetMapInfoEditResource,
    DeleteDeepAssetMapInfo,
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
      let dataCopy = [...(GridDto?.allItems ?? [])];
      props.returnObject(dataCopy);
    }
    props.modal && props.modal.setIsVisibleModalLookup(0);
  };

  const InvokeDownload = async () => {
    let payload: AssetMapInfoQueryObjectGrid = {
      ...query,
    };

    let result = await GetAssetMapInfoExport(payload);

    if (result !== undefined) {
      const url = window.URL.createObjectURL(result.file);
      const a = document.createElement("a");

      const fileName = result.fileName.endsWith(".xlsx")
        ? result.fileName
        : result.fileName.replace(/\.(xlsx|csv)$/, "") + ".xlsx";

      a.href = url;
      a.download = fileName;
      a.click();
    }
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      // GetLocationGridALL();
    }
  }, [GridDto]);

  return (
    <div className="pageContainer">
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
              {edit ? " Edit Asset Mapping" : "Add Asset Mapping"}
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
          <AssetMapInfoForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
          ></AssetMapInfoForm>
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Asset Mapping</h3>
        </div>
        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader"
              onClick={New}
              type="button"
            >
              New Asset Mapping
            </button>
          )}

          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => InvokeDownload()}
          >
            Download to Excel
          </button>
        </div>
      </div>

      <div className="">
        <AssetMapInfoGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ Delete, Edit, Filter: setQuery }}
        ></AssetMapInfoGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
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

export default AssetMapInfo;
