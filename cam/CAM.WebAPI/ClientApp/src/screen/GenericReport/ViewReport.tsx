import React, { useEffect, useState } from "react";
import { Alert, Dropdown, Form, Modal, Tab, Tabs } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useAuth } from "../../Hook/useAuth";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import setLoader from "../../Redux/Action/LoaderAction";
import PreviewReportGrid from "./PreviewReportGrid";
import { CustomGridRender } from "../../Model/Common";
import { RootState } from "../../Redux/Store/rootStore";
import { AuditDtoCreate, AuditDtoUpdate } from "../../Model/Audit";
import SetupColumns from "../Shared/SetupColumns";
import {
  GenericReportDtoGrid,
  GenericReportQueryObjectGrid,
  GenericPreviewReportQueryObjectGrid,
  GenericViewReportQueryObjectGrid,
  CreateGenericReportBody,
} from "../../Model/GenericReport";
import {
  CreateGenericReport,
  DeleteReport,
  GetGenericAggregatedReportGrid,
  GetGenericDisAggregatedReportGrid,
  GetGenericReportGrid,
  PreviewReport,
  UpdateReportStatus,
} from "../../Redux/Action/GenericReport/GenericReportCommonAction";
import Paginate from "../../Components/PaginationComponent";
import {
  GetAuditOverrideStatus,
  GetAuditRejectStatus,
} from "../../Redux/Action/Audit/AuditGridAction";
import ModalConfirm from "../../Components/ModalConfirm";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { GetNetworkElementAsIsCreateResource } from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import {
  DeleteDeepNetworkElementAsIs,
  RestoreNetworkElementAsIs,
} from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsDeleteAction";
import { GetNetworkElementAsIsEditResource } from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";
import { useGenericAggregatedReport } from "../../Hook/GenericReportOverrideHook/useGenericAggregatedReport";
import { useGenericDisAggregatedReport } from "../../Hook/GenericReportOverrideHook/useGenericDisAggregatedReport";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";

export let paginationQuery: GenericViewReportQueryObjectGrid = {
  lcmEngineeringId: [],
  designComponentId: [],
  opCoId: [],
  opCo: [],
  softwareEndOfWarrantyDate: undefined,
  numberOfNodes: [],
  productImportanceId: [],
  warranty: [],
  numberOfNodesInLab: [],
  hardwareSheetIndex: [],
  softwareSheetIndex: [],
  onHardware: [],
  onSoftware: [],
  fullOrPartialSupportId: [],
  hardwareEndOfSupportContract: undefined,
  hardwareSupportedId: [],
  renewalinProgress: [],
  softwareEndOfSupportContract: undefined,
  softwareSupportedId: [],
  sparesProvisioned: [],
  vendorEndMntDateHw: undefined,
  vendorEndMnteDateSw: undefined,
  elementCount: [],
  hardwareSupportProvider: [],
  hardwareSupportType: [],
  lcmStatusEngHardware: [],
  lcmStatusEngSoftware: [],
  lcmStatusHardware: [],
  lcmStatusOpsHardware: [],
  lcmStatusOpsSoftware: [],
  lcmStatusSoftware: [],
  outputToLcmHardware: [],
  outputToLcmSoftware: [],
  softwareSupportProvider: [],
  softwareSupportType: [],
  fullOrPartialSupportHwId: [],
  operationalContract: [],
  lcmSpreadSheetHwId: [],
  lcmSpreadSheetSwId: [],
  archived: [],
  lcmDeploymentStatus: [],
  resourceKey: [],
  previousResourceKey: [],
  isExtendedSupportOfferedByVendor: [],
  hwIsExtendedSupportOfferedByVendor: [],
  engKpi2: [],
  ipAddress: [],
  managedByGdc: [],
  lcmAncillaryDataId: [],
  productCode: [],
  lastScanDate: undefined,
  lastUpgradeDate: undefined,
  expLcmStatusAtEndOfFY24: [],
  engineeringContactPoint: [],
  operationsContactPoint: [],
  vendorEndOfVulnerabilitySecuritySupportDateValue: undefined,
  assetServiceFunctionality: [],
  networkElementAsPlannedId: [],
  automatedFeedback: [],
  plannedAction: [],
  environment: [],
  deploymentStatusId: [],
  deploymentTypeId: [],
  locationName: [],
  nfviBundleidId: [],
  orgEqpManufacturerId: [],
  capacityPlanReference: [],
  elementName: [],
  additionalInformation1: [],
  networkConstruct: [],
  additionalInformation2: [],
  hwResourceKey: [],
  previousHwResourceKey: [],
  swResourceKey: [],
  previousSwResourceKey: [],
  meStatus: [],
  meVirtualFlg: [],
  meDeploymentType: [],
  meType: [],
  meSerialNumber: [],
  meExternalConnectionFlg: [],
  hardwareModules: [],
  hardwareManfacturer: [],
  lastCheckedTime: [],
  plannedActivityId: [],
  plannedImplementationYear: [],
  activityStatusId: [],
  planningActivityStatusId: [],
  plannedActivity: [],
  activityDetails: [],
  ragStatus: [],
  deliveryProjectName: [],
  localApproval: [],
  deliveryStatusId: [],
  responsibilityPhaseId: [],
  plannedCompletion: undefined,
  plannedSpareFieldsJson: [],
  relatestoId: [],
  budgetValue: [],
  plannedActivityResource: [],
  activityDetailsText: [],
  budgetAvailabilityId: [],
  engineeringRiskId: [],
  operationalRiskId: [],
  linkedToPlannedActivityId: [],
  isNewServiceArchitecture: [],
  isReplacementExistingSolution: [],
  benefitId: [],
  driverId: [],
  planningRiskId: [],
  currency: [],
  notes: [],
  overAllRiskEvaluation: [],
  deliveryProjectId: [],
  planningRisk: [],
  projectStatus: [],
  riskEngineeringNotes: [],
  riskOperationalNotes: [],
  budgetTrackingId: [],
  designAspectId: [],
  designComponentFamilyId: [],
  startDate: undefined,
  forAddAsset: [],
  forEditAsset: [],
  originalLcmEngineeringId: [],
  deliveryPlanAvailable: [],
  program: [],
  projectOwner: [],
  budgetEstimated: [],
  bundleBudget: [],
  bundleId: [],
  systemTypeId: [],
  systemTypeNameVodafone: [],
  systemTypeName3gpp: [],
  systemTypeNameOem: [],
  majorSoftwareBuildsId: [],
  constraintsCaling: undefined,
  endOfMaintenanceValue: undefined,
  verticalResponsible: [],
  buildConstruction: [],
  subDomainResponsible: [],
  subDomainsPoc: [],
  assetCategoryId: [],
  assetClassId: [],
  assetTypeId: [],
  assetClass: [],
  constraintLcm: [],
  vodafoneName: [],
  assetCategory: [],
  takeFromAssetTypeTable: [],
  id: [],
  subDescription: [],
  default: [],
  alias: [],
  order: [],
  swApplicationName: [],
  gdprRelevant: [],
  internetFacing: [],
  lcmPolicy: [],
  criticality: [],
  securityElement: [],
  gdprClassification: [],
  pciSox: [],
  c3C4: [],
  missionCritical: [],
  productNameId: [],
  vodafoneNameId: [],
  productImportance: [],
  orgEqpmanufacturer: [],
  softwareVersion: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  majEndOfMaintenance: undefined,
  endOfSupport: undefined,
  generaAvailableDate: undefined,
  deliveryMethod: [],
  majSpareFieldsJson: [],
  operatingSystemId: [],
  vulnerabilityStatus: [],
  eomStatus: [],
  criticalAssetType: [],
  productName: [],
  majorDescription: [],
  platform: [],
  majorHardwareId: [],
  hardwareSolution: [],
  otherHardwareInfo: [],
  hardwareLastTimeBuyNew: undefined,
  hardwareLastTimeBuyUpgrades: undefined,
  hardwareLastTimeBuyExpansions: undefined,
  hardwareEndofmaintenance: undefined,
  hardwareEndofsupport: undefined,
  proprietaryHardware: [],
  platformId: [],
  buildconstructionid: [],
  hardwareType: [],
  operatingSystem: [],
  typeOfProcessor: [],
  hardwareVulnerabilityStatus: [],
  hardwareEomStatus: [],
  generalAvailabledate: undefined,
  hardwareModel: [],
  hardwareDescription: [],
  riskId: [],
  severity: [],
  riskDescription: [],
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  dynamicReportId: [],
  userId: [],
  jsonGridCustomizationData: [],
  published: [],
  isScheduled: undefined,
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
  reportName: [],
  ViewMode: undefined,
};

interface Props {
  action: {
    closeModal(): any;
    onDownload(
      id: number,
      tabType: string,
      query: GenericViewReportQueryObjectGrid
    ): any;
  };
  viewReportId: number;
  isVisibleModal: boolean;
  reportName: string;
}

const ViewReport: React.FC<Props> = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalPreview, setIsVisibleModalPreview] = useState(
    props?.isVisibleModal
  );
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const [keyTabs, setKeyTabs] = useState("disaggregated");
  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    readonly,
    role,
    pageSize,
  } = useAuth();
  //DTO
  const [aggregatedData, setAggregatedData] = useState<
    GenericReportDtoGrid[] | undefined
  >([]);
  const GridAggregated = (state: RootState) =>
    state.genericAggregatedReportGridReducer.GenericAggregatedReportGridResult;
  let GridDtoAggregated = useSelector(GridAggregated);
  console.log("Tems GridDtoAggregated", GridDtoAggregated);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const [disaggregatedData, setDisAggregatedData] = useState<
    GenericReportDtoGrid[] | undefined
  >([]);
  const GridDisAggregated = (state: RootState) =>
    state.genericDisAggregatedReportGridReducer
      .GenericDisAggregatedReportGridResult;
  let GridDtoDisAggregated = useSelector(GridDisAggregated);
  // console.log("Tems GridDtoDisAggregated", GridDtoDisAggregated);

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const [renderAggregatedGridState, setRenderAggregatedGridState] =
    useState<any>();
  const [renderDisAggregatedGridState, setRenderDisAggregatedGridState] =
    useState<any>();

  // console.log("Tems renderGridState", renderGridState);

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const {
    queryAggregated,
    setQueryAggregated,
    nextAggregated,
    backAggregated,
  } = useGenericAggregatedReport(
    { ...paginationQuery, dynamicReportId: [props.viewReportId], ViewMode: 1 },
    GetGenericAggregatedReportGrid
  );

  const {
    queryDisAggregated,
    setQueryDisAggregated,
    nextDisAggregated,
    backDisAggregated,
  } = useGenericDisAggregatedReport(
    { ...paginationQuery, dynamicReportId: [props.viewReportId], ViewMode: 2 },
    GetGenericDisAggregatedReportGrid
  );

  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });
  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setAggregatedData([]);
    setDisAggregatedData([]);
    let copy1 = { ...queryAggregated } as GenericViewReportQueryObjectGrid;
    copy1.dynamicReportId = [];
    copy1.dynamicReportId?.push(props?.viewReportId);
    setQueryAggregated(copy1);
    let copy2 = { ...queryDisAggregated } as GenericViewReportQueryObjectGrid;
    copy2.dynamicReportId = [];
    copy2.dynamicReportId?.push(props?.viewReportId);
    setQueryDisAggregated(copy2);
    setLoader("ADD", "GetGenericAggregatedReportGrid");
    setLoader("ADD", "GetGenericDisAggregatedReportGrid");
    closeModal();
    GetGenericAggregatedReportGrid(paginationQuery).then(() =>
      setLoader("REMOVE", "GetGenericAggregatedReportGrid")
    );
    GetGenericDisAggregatedReportGrid(paginationQuery).then(() =>
      setLoader("REMOVE", "GetGenericDisAggregatedReportGrid")
    );
  };
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
    Restore,
  } = useOperationTableCrud<CreateGenericReportBody, CreateGenericReportBody>(
    CreateGenericReport,
    UpdateReportStatus,
    DeleteReport,
    refresh
  );

  const resetQuery = () => {
    setQueryAggregated(paginationQuery);
    setQueryDisAggregated(paginationQuery);
    setFilterRedirect(false);
  };
  useEffect(() => {
    setAggregatedData([]);
    setDisAggregatedData([]);
    setLoader("ADD", "GetGenericAggregatedReportGrid");
    setLoader("ADD", "GetGenericDisAggregatedReportGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };

      if (localState?.id != null) {
        setLocalState(localState);
        let copy1 = { ...queryAggregated } as GenericViewReportQueryObjectGrid;
        copy1.dynamicReportId = [];
        copy1.dynamicReportId?.push(props?.viewReportId);
        setQueryAggregated(copy1);

        let copy2 = {
          ...queryDisAggregated,
        } as GenericViewReportQueryObjectGrid;
        copy2.dynamicReportId = [];
        copy2.dynamicReportId?.push(props?.viewReportId);
        setQueryDisAggregated(copy2);
        GetGenericAggregatedReportGrid(paginationQuery).then(() =>
          setLoader("REMOVE", "GetGenericAggregatedReportGrid")
        );
        GetGenericDisAggregatedReportGrid(paginationQuery).then(() =>
          setLoader("REMOVE", "GetGenericDisAggregatedReportGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetGenericAggregatedReportGrid");
    setLoader("REMOVE", "GetGenericDisAggregatedReportGrid");
  }, []);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDtoAggregated !== undefined || GridDtoAggregated !== null) {
      setAggregatedData(GridDtoAggregated?.items);
      let copy = { ...GridDtoAggregated?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderAggregatedGridState(copy);
      setLoader("REMOVE", "GetAuditGridAggregated");
    }
  }, [GridDtoAggregated]);

  useEffect(() => {
    if (GridDtoDisAggregated !== undefined || GridDtoDisAggregated !== null) {
      setDisAggregatedData(GridDtoDisAggregated?.items);
      let copy = { ...GridDtoDisAggregated?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderDisAggregatedGridState(copy);
      setLoader("REMOVE", "GetAuditGridDisAggregated");
    }
  }, [GridDtoDisAggregated]);

  const closeModalSetup = (changed: boolean) => {
    if (isVisibleModalPreview) {
      setIsVisibleModalSetup(false);
    }
    // else {
    //   GetGenericAggregatedReportGrid(queryAggregated).then(() =>
    //       setIsVisibleModalSetup(false)
    //     );
    //     GetGenericDisAggregatedReportGrid(queryDisAggregated).then(() =>
    //       setIsVisibleModalSetup(false)
    //   )
    // }
  };
  const onCloseModel = () => {
    setAggregatedData([]);
    setDisAggregatedData([]);
    setRenderAggregatedGridState([]);
    setRenderDisAggregatedGridState([]);
    props.action.closeModal();
  };

  const DownloadReport = () => {
    props.action.onDownload(
      props?.viewReportId,
      keyTabs,
      keyTabs === "aggregated" ? queryAggregated : queryDisAggregated
    );
    // props.action.onDownload(props?.viewReportId, keyTabs, queryDisAggregated);
  };

  // console.log(renderAggregatedGridState);
  return (
    <>
      <div className="headerPage row mx-0 justify-content-between mt-0">
        <label className="labelForm voda-bold mb-0">
          Report Name : {props.reportName}
        </label>
        <div className="row mx-0 justify-content-between">
          <div className="d-flex">
            <button
              className="download-to-excel mrl-10 pointer"
              onClick={() => DownloadReport()}
            >
              Download to Excel
            </button>
            <Dropdown className="d-inline more-options">
              <Dropdown.Toggle id="dropdown-autoclose-inside">
                More Options
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {!readonly && isVisibleModalPreview && (
                  <>
                    <Dropdown.Item
                    // onClick={() => setVisibleLegenda(!isVisibleLegenda)}
                    >
                      Legend
                    </Dropdown.Item>
                    <div
                      className="bubbleMenuLegenda"
                      // onMouseLeave={() => setVisibleLegenda(false)}
                    >
                      <div className="triangleBubbleTop-right"></div>
                      <div className="col-12 row mx-0 px-2 my-2">
                        <div className="w-100 mx-0 py-1 d-flex align-items-center">
                          <div className="legendaElement red"></div>
                          <span className="legendaElement">
                            Mandatory Field (Engineering)
                          </span>
                        </div>
                        <div className="w-100 mx-0 py-1 d-flex align-items-center">
                          <div className="legendaElement green"></div>
                          <span className="legendaElement">
                            Mandatory Field (Operations)
                          </span>
                        </div>
                        <div className="w-100 mx-0 py-1 d-flex align-items-center">
                          <div className="legendaElement gray"></div>
                          <span className="legendaElement">
                            Nice to have field
                          </span>
                        </div>
                        <div className="w-100 mx-0 py-2 d-flex align-items-center">
                          <div className="legendaElement blu"></div>
                          <span className="legendaElement">
                            Automatic Calculation
                          </span>
                        </div>
                      </div>
                    </div>
                  </>
                )}
                <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                  Manage Table Content
                </Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </div>
        </div>
      </div>
      <Dialog
        open={isVisibleModalSetup}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        onClose={() => setIsVisibleModalSetup(false)}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">Setup Grid Informations</div>
        </DialogTitle>
        <DialogContent>
          <SetupColumns
            renderGrid={
              keyTabs === "aggregated"
                ? {
                    ...renderAggregatedGridState,
                    render: renderAggregatedGridState?.render?.filter(
                      (x) => x.order !== -1
                    ),
                  }
                : {
                    ...renderDisAggregatedGridState,
                    render: renderDisAggregatedGridState?.render?.filter(
                      (x) => x.order !== -1
                    ),
                  }
            }
            action={{ closeModalSetup: () => setIsVisibleModalSetup(false) }}
            tab={""}
          ></SetupColumns>
        </DialogContent>
      </Dialog>
      <Tabs
        defaultActiveKey="Aggregated"
        id="report"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="aggregated" title="Aggregated">
          <PreviewReportGrid
            data={aggregatedData}
            pagination={queryAggregated}
            orphanColor={orphanColor}
            renderGrid={renderAggregatedGridState?.render ?? []}
            action={{
              Filter: setQueryAggregated,
              // Restore,
              closeModal: () => onCloseModel(),
              setIsFiltriAttivati,
            }}
          ></PreviewReportGrid>
          <Paginate
            pagination={{
              page: queryAggregated.page,
              pageSize: queryAggregated.pageSize,
            }}
            totalItems={GridDtoAggregated?.totalItems}
            actions={{ next: nextAggregated, back: backAggregated }}
          />
        </Tab>
        <Tab eventKey="disaggregated" title="DisAggregated">
          <PreviewReportGrid
            data={disaggregatedData}
            pagination={queryDisAggregated}
            orphanColor={orphanColor}
            renderGrid={renderDisAggregatedGridState?.render ?? []}
            action={{
              Filter: setQueryDisAggregated,
              // Restore,
              closeModal: () => onCloseModel(),
              setIsFiltriAttivati,
            }}
          ></PreviewReportGrid>
          <Paginate
            pagination={{
              page: queryDisAggregated.page,
              pageSize: queryDisAggregated.pageSize,
            }}
            totalItems={GridDtoDisAggregated?.totalItems}
            actions={{ next: nextDisAggregated, back: backDisAggregated }}
          />
        </Tab>
      </Tabs>
    </>
  );
};

export default ViewReport;
