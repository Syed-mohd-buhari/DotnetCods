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
import PlannedActivityResourceForm from "../../screen/Lookup/PlannedActivityResource/PlannedActivityResourceForm";
import PlannedActivityResourceGrid from "../../screen/Lookup/PlannedActivityResource/PlannedActivityResourceGrid";
import {
  GetPlannedActivityResourceCreateResource,
  GetPlannedActivityResourceForDropdown,
} from "../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceCreateAction";
import {
  DeleteDeepPlannedActivityResource,
  GetRelatedRecordsPlannedActivityResource,
} from "../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceDeleteAction";
import { GetPlannedActivityResourceEditResource } from "../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceEditAction";
import {
  GetPlannedActivityResourceGrid,
  GetPlannedActivityResourceGridALL,
} from "../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalRelated from "../../Components/ModalRelated";
import { Modal } from "react-bootstrap";
import {
  PlannedActivityResourceQueryObjectGrid,
  PlannedActivityResourceDtoGrid,
  PlannedActivityResourceDto,
} from "../../Model/LookUp/PlannedActivityResource";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { GetPlannedRuleConfig } from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import { useAuth } from "../../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQueryTipologiche: PlannedActivityResourceQueryObjectGrid =
  {
    plannedActivityResourceId: [],
    plannedActivityResourceDescription: [],
    plannedDesignComponentRequiredNetworkElement: [],
    lcmLabelSoftware: [],
    lcmLabelHardware: [],
    ruleActicvityDetails: [],
    driverTextNetworkElement: [],
    benefitTextNetworkElement: [],
    planningRisksNetworkElement: [],
    driverTextLcm: [],
    benefitTextLcm: [],
    planningRisksLcm: [],
    planningRisksDesignAspect: [],
    forEditNetworkElement: [],
    forCreateNetworkElement: [],
    ruleLinkedDc: [],
    ruleLinkedDcPlannedActivityTypeDescription: [],
    ruleNetworkElement: [],
    lastModifiedStartDate: undefined,
    lastModifiedEndDate: undefined,
    sortBy: "",
    isSortAscending: false,
    page: 1,
    pageSize: 10,
    principalId: undefined,
    exportable: [],
    designAspectExportable: [],
    deleted: undefined,
    lcmHardware: [],
    lcmSoftware: [],
    networkElementHardware: [],
    networkElementSoftware: [],
    onBareMetalNetworkElement: [],
    onVirtualizedNetworkElement: [],
    jsonFormResource: [],
    orphan: undefined,
    lastModifiedBy: [],
    activityDetailsNetworkElement: [],
    forLcm: [],
    forNetworkElement: [],
    forAddAsset: [],
    forEditAsset: [],
  };

interface Props {
  modal?: {
    isModal: boolean | false;
    setIsVisibleModalLookup(value: number): any;
  };
  returnObject?(data: { [key: string]: PlannedActivityResourceDto }): any;
  isFromNetworkElement?: boolean;
  isDesignAspect?: boolean;
  isForAddAsset?: boolean;
  isForEditAsset?: boolean;
}

const PlannedActivityResource: React.FC<Props> = (props) => {
  let paginationQueryTipologicheCondizionato = {
    ...paginationQueryTipologiche,
  };
  paginationQueryTipologicheCondizionato.forLcm = [];
  paginationQueryTipologicheCondizionato.forDesignAspect = [];
  paginationQueryTipologicheCondizionato.forAddAsset = [];
  paginationQueryTipologicheCondizionato.forEditAsset = [];
  if (props.isDesignAspect) {
    paginationQueryTipologicheCondizionato.forDesignAspect = [true];
  } else if (props.isForAddAsset) {
    paginationQueryTipologicheCondizionato.forAddAsset = [true];
  } else if (props.isForEditAsset) {
    paginationQueryTipologicheCondizionato.forEditAsset = [true];
  } else {
    paginationQueryTipologicheCondizionato.forLcm = [true];
  }

  const { isPermesso } = useAuth();
  //DTO
  const [data, setData] = useState<
    PlannedActivityResourceDtoGrid[] | undefined
  >([]);
  const [changed, setChanged] = useState<boolean>();
  const Grid = (state: RootState) =>
    state.plannedActivityResourceGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.plannedActivityResourceGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [rulesLinkedDesignComponent, setRulesLinkedDesignComponent] = useState<
    { key: number; value: string }[]
  >([]);
  const [initialStateQuery, setInitialStateQuery] =
    useState<PlannedActivityResourceQueryObjectGrid>(
      paginationQueryTipologiche
    );

  const refresh = () => {
    closeModal();
    GetPlannedActivityResourceGrid(query);
  };

  useEffect(() => {
    let copy = {
      ...initialStateQuery,
    } as PlannedActivityResourceQueryObjectGrid;
    copy.forLcm = [];
    copy.forNetworkElement = [];

    if (props.isForEditAsset) {
      copy.forEditAsset = [true];
    } else if (props.isForAddAsset) {
      copy.forAddAsset = [true];
    } else if (props.isDesignAspect) {
      copy.forDesignAspect = [true];
    } else {
      copy.forLcm = [true];
    }
    setInitialStateQuery(copy);
    setQuery(copy);
    let pagewiseId = props.isDesignAspect
      ? 4
      : props.isForAddAsset || props.isForEditAsset
      ? 2
      : copy.forLcm
      ? 0
      : 0;
    GetPlannedRuleConfig(pagewiseId).then((res: any) => {
      setRulesLinkedDesignComponent(
        res?.map((x) => {
          return {
            key: x.plannedActivityTypesId,
            value: x.plannedActivityTypeDescription,
          };
        })
      );
    });
  }, []);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryTipologicheCondizionato,
    isPermesso ? GetPlannedActivityResourceGrid : undefined
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
  } = useOperationTableCrud<
    PlannedActivityResourceDtoGrid,
    PlannedActivityResourceDtoGrid
  >(
    GetPlannedActivityResourceCreateResource,
    GetPlannedActivityResourceEditResource,
    DeleteDeepPlannedActivityResource,
    refresh
  );

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1 E LISTA FILTRATA LCM/NEAP

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
    // GetPlannedActivityResourceGridALL();
  }, [GridDto]);

  const resetQuery = () => {
    setQuery(paginationQueryTipologiche);
  };

  const chiudiModal = () => {
    GetPlannedActivityResourceForDropdown().then((x) => {
      if (props.returnObject) {
        props.returnObject(x?.data);
      }
      props.modal && props.modal.setIsVisibleModalLookup(0);
    });
  };

  const rulesResource = [
    { key: 1, value: "Solution" },
    { key: 2, value: "OEM" },
    { key: 3, value: "Major Release" },
    { key: 4, value: "Virtualize System" },
    { key: 5, value: "Free Text from User" },
    { key: 6, value: "Fixed Text from Admin" },
    { key: 7, value: "Upgrade HW Components" },
    { key: 8, value: "Add/Remove HW Components" },
  ];

  const addRemoveActivityDetailsDropdownOptions = [
    { key: 0, value: "Add additional hardware in node" },
    { key: 1, value: "Remove existing hardware from node" },
  ];

  const rulesNetworkElement = [
    { key: 0, value: "Other" },
    { key: 1, value: "New Instance" },
    { key: 2, value: "FoA System" },
    { key: 3, value: "Upgrade HW Components" },
  ];
  // const rulesLinkedDesignComponent = [
  //   { key: 0, value: "No Rule" },
  //   { key: 1, value: "SW Architecture Upgrade | SW Major Release" },
  //   { key: 2, value: "HW Refresh" },
  //   { key: 3, value: "HW Replace" },
  //   { key: 4, value: "System Replace" },
  //   { key: 5, value: "System Refresh" },
  //   { key: 6, value: "Upgrade HW Components" },
  // ];
  const rulesActivityDetailsNetworkElement = [
    { key: 1, value: "Virtualize System" },
    { key: 2, value: "Free Text from User" },
    { key: 3, value: "Fixed Text from Admin" },
    { key: 4, value: "Network Element & Location" },
    { key: 5, value: "Upgrade HW Components" },
    { key: 6, value: "Add/Remove HW Components" },
  ];

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsPlannedActivityResource(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };

  return (
    <div
      className={props.modal && props.modal.isModal ? "w-100" : "pageContainer"}
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
              {edit ? "Edit Activity Details" : "New Activity Details"}
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
          <PlannedActivityResourceForm
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh }}
            rules={rulesResource}
            rulesActivityDetailsNetworkElement={
              rulesActivityDetailsNetworkElement
            }
            rulesNetworkElement={rulesNetworkElement}
            rulesLinkedDesignComponent={rulesLinkedDesignComponent}
            addRemoveActivityDetailsDropdownOptions={
              addRemoveActivityDetailsDropdownOptions
            }
          ></PlannedActivityResourceForm>
        </DialogContent>
      </Dialog>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold text-dark">Planned Activity</h3>
        </div>
        <div className="">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={New}
            type="button"
          >
            New Planned Activity
          </button>
        </div>
      </div>
      <div className="">
        <PlannedActivityResourceGrid
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{ onDelete, Edit, Filter: setQuery }}
          rules={rulesResource}
          rulesNetworkElement={rulesNetworkElement}
          rulesLinkedDesignComponent={rulesLinkedDesignComponent}
          ruleActivityDetailsNetworkElement={rulesActivityDetailsNetworkElement}
        ></PlannedActivityResourceGrid>
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

export default PlannedActivityResource;
