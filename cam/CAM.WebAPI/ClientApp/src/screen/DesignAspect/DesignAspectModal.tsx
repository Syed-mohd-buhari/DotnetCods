import React, { useState, useEffect, useLayoutEffect, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { formatDateWithTime, formatTimeLocal } from "../../Hook/Common";
import { useSelector } from "react-redux";
import Select from "react-select";
import { DesignAspectDtoUpdate } from "../../Model/DesignAspects";
import {
  EditDesignAspect,
  GetUpdateDAMigration,
} from "../../Redux/Action/DesignAspect/DesignAspectEditAction";
import { CreatDesignAspect } from "../../Redux/Action/DesignAspect/DesignAspectCreateAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { Tabs, Tab, Modal } from "react-bootstrap";
import DesignPlannedActivity from "./DesignPlannedActivity";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { GetPlannedActivityCreateResource } from "../../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { deletePlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityDeleteAction";
import { GetPlannedActivityEditResource } from "../../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import { GetPlannedActivityGrid } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { DesignAspectApi } from "../../Business/DesignAspectsBusiness";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityDtoGrid,
  PlannedActivityQueryObjectGrid,
} from "../../Model/PlannedActivity";

import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  CustomGridRender,
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  stateConfirm,
} from "../../Model/Common";

import { dictionaryToArray } from "../../Hook/Dictionary";
import { NetworkElementAsPlannedDtoCreate } from "../../Model/NetworkElementAsPlanned";
import { LocationDto } from "../../Model/LookUp/Location";
import SecurityTireZone from "../../Containers/Lookup/SecurityTireZoneContainer";
import SharedLookUp from "../../Containers/Lookup/SharedLookUpContainer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import {
  GetDesignComponentFamily,
  GetServicesAndNetworks,
} from "../../Redux/Action/DesignAspect/DesignAspectBuildCommonAction";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";
import SupportedService from "../../Containers/Lookup/SupportedServiceContainer";
import { ApiCallWithErrorHandling } from "../../Business/Common/CommonBusiness";
import setLoader from "../../Redux/Action/LoaderAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import AssetsDetailsContainer from "../../Containers/Lookup/AssetsDetailsContainer";
import {
  DaAssetMigrationGrid,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../Model/LookUp/AssetMigrationModels";
import { GetAssetsPlatformMigrationGrid } from "../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import AssetsDetailsGrid from "../Lookup/AssetPlatform/AssetsDetailsGrid";
import Paginate from "../../Components/PaginationComponent";

let paginationQueryPlanning: PlannedActivityQueryObjectGrid = {
  plannedImplementationYear: [],
  activityStatusId: [],
  planningActivityStatusId: [],
  designComponentId: [],
  driver: [],
  benefits: [],
  plannedActivityDescription: [],
  activityDetailsText: [],
  budgetAvailability: [],
  deliveryProjectName: [],
  localApproval: [],
  deliveryStatusId: [],
  responsibilityPhaseId: [],
  plannedCompletion: undefined,
  notes: [],
  riskEngineeringEvaluation: [],
  riskEngineeringNotes: [],
  riskOperationalEvaluation: [],
  riskOperationalNotes: [],
  sortBy: "",
  isSortAscending: false,
  budgetTrackingId: [],
  budgetValueGrid: [],
  deliveryProjectId: [],
  plannedActivityResourceId: [],
  planningRisk: [],
  relatesToId: [],
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
  forLcm: true,
  forNetwork: true,
};

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  // data: DesignAspectDtoUpdate | DesignAspectDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
  idDetail?: number | string | undefined | null;
  fromDesignAspect?: any;
  dcfId: number | undefined;
  dcfName: string;
}

export const paginationQueryAssets: DaAssetMigrationGrid = {
  daAssetMigrationId: [],
  plannedActivityId: [],
  networkElementAsPlannedId: [],
  newelEmentName: [],
  targetDesignComponenetId: [],
  environmentDesc: [],
  deploymentStatusDesc: [],
  opcoDesc: [],
  opcoId: [],
  locationDesc: [],
  rfoDate: undefined,
  rfsDate: undefined,
  migrationCompletionDate: undefined,
  trafficNodePercentage: [],
  oldAssetName: [],
  currentDesignComponenet: [],
  targetDesignComponenet: [],
  newEnvironment: [],
  newDeploymentStatus: [],
  location: [],
  oldEnvironment: [],
  oldDeploymentType: [],
  oldDeploymentStatus: [],
  currentDcfId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
};

const DesignAspectModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("operational");
  const {
    formData,
    checkIsExist,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    setChanged,
    setInputValue,
    confirmForm,
  } = useFormTableCrud<DesignAspectDtoUpdate>(
    CreatDesignAspect,
    EditDesignAspect
  );

  const dtoEditResourceState = (state: RootState) =>
    state.designAspectEditReducer.DesignAspectDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.designAspectCreateReducer.DesignAspectDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const GridAllNetworkFunction = (state: RootState) =>
    state.networkFunctionGridReducer.LookUpGridResultAll;
  const GridDtoAllNetworkFunction = useSelector(GridAllNetworkFunction);

  const [showFurtherSection, setShowFurtherSection] = useState<boolean>(false);

  const GridAllSupportedService = (state: RootState) =>
    state.supportedServiceGridReducer.LookUpGridResultAll;
  const GridDtoAllSupportedService = useSelector(GridAllSupportedService);

  const GridAllTireZone = (state: RootState) =>
    state.securityTireZoneGridReducer.LookUpGridResultAll;
  const GridDtoAllTireZone = useSelector(GridAllTireZone);

  const GridAllSharedLookUp = (state: RootState) =>
    state.sharedLookUpGridReducer.LookUpGridResultAll;
  const GridDtoAllSharedLookUp = useSelector(GridAllSharedLookUp);

  const Grid = (state: RootState) => state.locationGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);

  const GridEnvironments = (state: RootState) =>
    state.environmentGridReducer.LookUpGridResult;
  const GridDtoEnvironemnts = useSelector(GridEnvironments);

  const dtoNewResourceStateNetwork = (state: RootState) =>
    state.networkElementAsPlannedCreateReducer.NetworkElementAsPlannedDtoCreate;
  let createResourceNetwork = useSelector(dtoNewResourceStateNetwork);
  const GridAssetSelector = (state: RootState) =>
    state.daassetsMigrationGridReducer.DAAssetsMigrationGridResult;

  const GridAssetDto: QueryResultDtoOfDAAssetMigrationDtoGrid | null =
    useSelector(GridAssetSelector);

  const { tipologicaPermesso, isPermesso, pageSize } = useAuth();

  const [networkElementToAdd, setNetworkElementToAdd] =
    useState<NetworkElementAsPlannedDtoCreate | null>();

  const [opco, setOpco] = useState<string>();

  const [supportedServiceDCF, setSupportedServiceDCF] = useState<Array<any>>(
    []
  );

  const [networkFunctionDCF, setNetworkFunctionDCF] = useState<Array<any>>([]);
  const [isAssetDetailContainer, setIsAssetDetailsCpntainer] =
    useState<boolean>(false);
  const [assetData, setAssetData] = useState<any[] | undefined>([]);
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const [isFiltriAttivati, setIsFiltriAttivati] = useState(false);

  useEffect(() => {
    paginationQueryAssets.pageSize = pageSize;
  }, [pageSize]);
  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.fromDesignAspect) {
      setFormData(createResource);
      return;
    }
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    let copy = { ...formData } as DesignAspectDtoUpdate;

    if (formData && formData.designComponentFamilyId && !props.edit) {
      GetServicesAndNetworks(formData.designComponentFamilyId).then(
        (response) => {
          copy.supportedServicesResource = response.services;
          copy.usedNetworkFunctionsResource = response.networkFunctions;
          copy.isSupportedAllServices = response.isSupportedAllServices;
          copy.platformSoftware = response.platformSoftware;
          copy.designComponentFamilyName = dictionaryToArray(
            formData.dcFsResource!
          ).find((x) => x.key == formData.designComponentFamilyId)?.value!;
          copy.subNetworkBoundary = response.subNetworkBoundary;
          copy.supportedServicesIds = dictionaryToArray(response?.services).map(
            (x) => x.key
          );
          copy.usedNetworkFunctionsIds = dictionaryToArray(
            response?.networkFunctions
          ).map((x) => x.key);

          console.log("copy in useEffect 1 => ", copy);

          setFormData(copy);
        }
      );
    }
  }, [formData?.designComponentFamilyId]);

  useEffect(() => {
    console.log("props => ", props);
    if (formData && formData.opCoId && !props.dcfId) {
      let copy = { ...formData } as DesignAspectDtoUpdate;
      GetDesignComponentFamily(formData.opCoId).then((res) => {
        copy.dcFsResource = res;
        setFormData(copy);
      });
    }
  }, [formData?.opCoId]);

  const plannedActivity402 = formData?.plannedActivityDto?.find(
    (a) => a.plannedActivityResourceId === 402
  );

  const isQueryReady =
    !!formData?.opCoId &&
    !!formData?.designComponentFamilyId &&
    !!plannedActivity402?.plannedActivityId;

  const {
    query: query1,
    next: next1,
    back: back1,
    setQuery: setQuery1,
  } = useResourceTableCrud(
    paginationQueryAssets,
    isQueryReady && isPermesso ? GetAssetsPlatformMigrationGrid : undefined
  );

  useEffect(() => {
    setAssetData([]);
    setRenderGridState(undefined);
  }, [
    isQueryReady,
    formData?.opCoId,
    formData?.designComponentFamilyId,
    formData?.plannedActivityDto,
  ]);

  const onChangeOpco = (e: any) => {
    let copy = { ...formData } as DesignAspectDtoUpdate;
    if (e && e["key"]) {
      copy.opCoId = e["key"];
      if (!props.dcfId) {
        copy.subNetworkBoundary = "";
        copy.supportedServicesIds = undefined;
        copy.usedNetworkFunctionsIds = undefined;
        copy.designComponentFamilyId = 0;
      }
    } else {
      copy.opCoId = 0;
      if (!props.dcfId) {
        copy.dcFsResource = undefined;
        copy.subNetworkBoundary = "";
        copy.supportedServicesIds = undefined;
        copy.usedNetworkFunctionsIds = undefined;
        copy.designComponentFamilyId = 0;
      }
    }
    setFormData(copy);
  };

  const onChangeDCF = (e: any) => {
    let copy = { ...formData } as DesignAspectDtoUpdate;
    if (e && e["key"]) {
      copy.designComponentFamilyId = e["key"];
    } else {
      copy.subNetworkBoundary = "";
      copy.supportedServicesIds = undefined;
      copy.usedNetworkFunctionsIds = undefined;
      copy.designComponentFamilyId = 0;
    }

    setFormData(copy);
  };

  useEffect(() => {
    if (props.keyTab === "" || props.keyTab == null || !props.edit) {
      setKey("operational");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  useEffect(() => {
    if (checkIsExist) {
      setFormData(props.edit ? editResource : createResource);
    }
  }, [checkIsExist]);

  const OnChangeMultiSelect = (property: string, e: any) => {
    if (property === "eduSpocIds" && e !== null) {
      e = [{ ...e }];
    }
    let array = [] as Array<number>;
    let copy = { ...formData } as DesignAspectDtoUpdate;
    if (e !== null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const SaveOrConfirmPlanned = async () => {
    const plannedActivityId =
      formData?.plannedActivityDto?.[0]?.plannedActivityId;
    const updatedFormData = {
      ...formData,
      opcosResource: {},
      authenicationTypesResource: {},
      instanseResiliencesResource: {},
      licenseModelsResource: {},
      securityManagersResource: {},
      securityTireZoneResource: {},
      siteResilienceMethodsResource: {},
      siteResiliencesResource: {},
      swDeliveryLifeCyclesResource: {},
      thirdPartyAccessResource: {},
    } as DesignAspectDtoUpdate;
    const handleAfterSave = async (saveResult: any) => {
      if (saveResult?.ResultDtoEdit && !saveResult?.ResultDtoEdit.warning) {
        try {
          const migrationResponse = await GetUpdateDAMigration(
            plannedActivityId ?? 0,
            formData?.designComponentFamilyId ?? 0,
            props?.dcfId ?? 0
          );
        } catch (err) {
          console.error("Failed to call GetUpdateDAMigration:", err);
        }
      }
    };

    const confirmPlannedState = {
      title: "Continue without saving planned activities?",
      button: "Continue",
      message:
        "Are you sure you want to continue? All changes in Planned activities will be lost",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setConfirmPlanned(stateConfirm),
        confirm: async () => {
          const saveResult = await Save(
            updatedFormData,
            props.edit,
            validazioneClient,
            refresh,
            RestoreOrphanDeleted,
            orphanDeleted
          );

          await handleAfterSave(saveResult);
          setIsAssetDetailsCpntainer(true);
        },
      },
    };

    if (showForm) {
      let copy = { ...updatedFormData } as DesignAspectDtoUpdate;
      if (validazioneClient(copy).response === true) {
        setConfirmPlanned(confirmPlannedState);
      } else {
        rootStore.dispatch(
          setNotification({
            message: "Check the fields entered in Operational",
            notifyType: NotifyType.warning,
          })
        );
      }
    } else {
      let copy = { ...updatedFormData } as DesignAspectDtoUpdate;
      const saveResult = await Save(
        copy,
        props.edit,
        validazioneClient,
        refresh,
        RestoreOrphanDeleted,
        orphanDeleted
      );

      await handleAfterSave(saveResult);
      setIsAssetDetailsCpntainer(true);
    }
  };
  useEffect(() => {
    if (!isQueryReady) return;

    const plannedActivity402 = formData!.plannedActivityDto!.find(
      (a) => a.plannedActivityResourceId === 402
    );

    if (!plannedActivity402?.plannedActivityId) return;

    let copy = {} as DaAssetMigrationGrid;

    copy.opcoId = [formData!.opCoId];
    copy.currentDcfId = [formData!.designComponentFamilyId];
    copy.plannedActivityId = [plannedActivity402.plannedActivityId];

    setQuery1(copy);
  }, [
    isQueryReady,
    formData?.opCoId,
    formData?.designComponentFamilyId,
    formData?.plannedActivityDto,
  ]);

  useEffect(() => {
    if (GridAssetDto !== undefined || GridAssetDto !== null) {
      setAssetData(GridAssetDto?.items);
      let copy = { ...GridAssetDto?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetReconciliationGrid");
    }
  }, [GridAssetDto]);
  //VALIDAZIONE PRE Save
  const validazioneClient = (copy: DesignAspectDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;
    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opCoId === null ||
      copy?.opCoId === undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }
    if (
      copy?.designComponentFamilyId === null ||
      copy?.designComponentFamilyId === undefined ||
      copy?.designComponentFamilyId === 0
    ) {
      addInvalidProperty("designComponentFamilyId");
    }

    // if (
    //   !copy.isSupportedAllServices &&
    //   (copy?.supportedServicesIds === null ||
    //     copy?.supportedServicesIds === undefined ||
    //     copy.supportedServicesIds.length === 0)
    // ) {
    //   addInvalidProperty("supportedServicesIds");
    // }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  //Modifiche Planning
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryPlanning,
    isPermesso ? GetPlannedActivityGrid : undefined
  );
  const [dataPlanning, setDataPlanning] = useState<
    PlannedActivityDtoGrid[] | undefined
  >([]);

  const { New, Edit, Delete } = useOperationTableCrud<
    PlannedActivityDtoUpdate,
    PlannedActivityDtoCreate
  >(
    GetPlannedActivityCreateResource,
    GetPlannedActivityEditResource,
    deletePlannedActivity,
    refresh
  );

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const OpCoRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.opcosResource)
      formData.opcosResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const SecurityTireZoneRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.securityTireZoneResource)
      formData.securityTireZoneResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SecurityManagerRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.securityManagersResource)
      formData.securityManagersResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const AuthenticationTypeRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.authenicationTypesResource)
      formData.authenicationTypesResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const ThirdPartyRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.thirdPartyAccessResource)
      formData.thirdPartyAccessResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SWDeliveryLifeCycleRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.swDeliveryLifeCyclesResource)
      formData.swDeliveryLifeCyclesResource = obj as {
        [key: string]: string;
      };
    setFormData(formData);
  };

  const LicenseModelRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.licenseModelsResource)
      formData.licenseModelsResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SiteResilienceRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.siteResiliencesResource)
      formData.siteResiliencesResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const InstanceResilienceRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.instanseResiliencesResource)
      formData.instanseResiliencesResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SupportedServiceRefillData = async (value: Array<any>) => {
    setLoader("ADD", "DesignComponentFamilyId");

    if (formData && formData.designComponentFamilyId && !sessionStorage.SPID) {
      let api = new DesignAspectApi();

      let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
        api.getLinkSubnetworkWithSupportedService(
          formData?.designComponentFamilyId! + ""
        )
      );
      setSupportedServiceDCF(result?.data);
    } else {
      let api = new DesignAspectApi();

      let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
        api.getLinkSubnetworkWithSupportedService(
          formData?.designComponentFamilyId! + "",
          sessionStorage.SPID!
        )
      );
      setSupportedServiceDCF(result?.data);
      sessionStorage.clear();
    }

    setLoader("REMOVE", "DesignComponentFamilyId");
  };

  const NetworkFunctionRefillData = async (value: Array<any>) => {
    setLoader("ADD", "usedNetworkFunctionsResource");
    if (formData && formData.designComponentFamilyId && !sessionStorage.SPID) {
      let api = new DesignAspectApi();

      let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
        api.getLinkNetworkFunction(formData?.designComponentFamilyId! + "")
      );
      setNetworkFunctionDCF(result?.data);
    } else {
      let api = new DesignAspectApi();

      let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
        api.getLinkNetworkFunction(
          formData?.designComponentFamilyId! + "",
          sessionStorage.SPID!
        )
      );
      setNetworkFunctionDCF(result?.data);
    }
    setLoader("REMOVE", "usedNetworkFunctionsResource");
  };

  useLayoutEffect(() => {
    let copy = { ...formData } as DesignAspectDtoUpdate;
    let obj = supportedServiceDCF.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    ) as { [key: string]: string };
    copy.supportedServicesResource = obj;
    setFormData(copy);
  }, [supportedServiceDCF]);

  useLayoutEffect(() => {
    let copy = { ...formData } as DesignAspectDtoUpdate;
    let obj = networkFunctionDCF.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    ) as { [key: string]: string };
    copy.usedNetworkFunctionsResource = obj;
    setFormData(copy);
  }, [networkFunctionDCF]);

  const BusinessContinuityMethodRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.siteResilienceMethodsResource)
      formData.siteResilienceMethodsResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const onHideModel = () => {
    if (isVisibleModalLookup === 2) {
      let dataCopy = [...(GridDtoAllTireZone?.items ?? [])];
      SecurityTireZoneRefillData(dataCopy);
    }

    if (isVisibleModalLookup === 3) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      SecurityManagerRefillData(dataCopy);
    }

    if (isVisibleModalLookup === 4) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      AuthenticationTypeRefillData(dataCopy);
    }

    if (isVisibleModalLookup === 5) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      ThirdPartyRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 6) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      SWDeliveryLifeCycleRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 7) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      LicenseModelRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 8) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      SiteResilienceRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 9) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      BusinessContinuityMethodRefillData(dataCopy);
    }

    if (isVisibleModalLookup === 10) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      InstanceResilienceRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 11) {
      let dataCopy = [...(GridDtoAllNetworkFunction?.items ?? [])];
      NetworkFunctionRefillData(dataCopy);
    }
    if (isVisibleModalLookup === 12) {
      let dataCopy = [...(GridDtoAllSupportedService?.items ?? [])];
      SupportedServiceRefillData([]);
    }
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OpcoContainer
            returnObject={OpCoRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OpcoContainer>
        );
      case 2:
        return (
          <SecurityTireZone
            returnObject={SecurityTireZoneRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></SecurityTireZone>
        );
      case 3:
        return (
          <SharedLookUp
            returnObject={SecurityManagerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="SecurityManager"
          />
        );
      case 4:
        return (
          <SharedLookUp
            returnObject={AuthenticationTypeRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="AuthenicationType"
          />
        );

      case 5:
        return (
          <SharedLookUp
            returnObject={ThirdPartyRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="ThirdPartyAccessType"
          />
        );
      case 6:
        return (
          <SharedLookUp
            returnObject={SWDeliveryLifeCycleRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="SWDeliveryLifeCycle"
          />
        );
      case 7:
        return (
          <SharedLookUp
            returnObject={LicenseModelRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="LicenseModel"
          />
        );
      case 8:
        return (
          <SharedLookUp
            returnObject={SiteResilienceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="SiteResilience"
          />
        );
      case 9:
        return (
          <SharedLookUp
            returnObject={BusinessContinuityMethodRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="BusinessContinuityMethod"
          />
        );
      case 10:
        return (
          <SharedLookUp
            returnObject={InstanceResilienceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="InstanceResilience"
          />
        );
      case 11:
        return (
          <NetworkFunction
            returnObject={NetworkFunctionRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 12:
        return (
          <SupportedService
            returnObject={SupportedServiceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return null;
    }
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [disableForm, setDisableForm] = useState<boolean>(false);

  const RestoreOrphanDeleted = (
    id: number | undefined,
    orphanDeletedValue?: boolean
  ) => {
    setOrphanDeleted(orphanDeletedValue ?? true);
    props.action.Edit(id);
    if (orphanDeletedValue === false) {
      setDisableForm(true);
      setChanged(false);
    }
  };

  const AddOnList = (item: PlannedActivityDtoUpdate[]) => {
    let copy = { ...formData } as DesignAspectDtoUpdate;
    if (
      copy.plannedActivityDto == null ||
      copy.plannedActivityDto === undefined
    ) {
      copy.plannedActivityDto = [];
    }
    copy.plannedActivityDto = item;
    setFormData(copy);
  };

  const GetElementFromList = (index: number) => {
    return formData?.plannedActivityDto && formData?.plannedActivityDto[index];
  };

  const callBackGetUpdateLcm = (id: number) => {
    setKey("operational");
    props.action.Edit(id);
    setShowForm(false);
  };

  //PROPS PLANNED ACTIVITY
  useEffect(() => {
    if (formData?.opCoId != undefined && formData.opcosResource != undefined) {
      let opcoString = dictionaryToArray(formData.opcosResource).find(
        (x) => x.key == formData.opCoId
      )?.value;
      setOpco(opcoString);
    } else {
      setOpco("");
    }
  }, [formData?.opCoId]);

  const [validazioneCustom, setValidazioneCustom] = useState<{
    response: boolean;
    property?: string;
    message?: string;
  }>();

  const [showForm, setShowForm] = useState<boolean>(false);
  const [confirmPlanned, setConfirmPlanned] =
    useState<DataModalConfirm>(stateConfirm);

  const onChangeLocation = (obj: any) => {
    let copy = { ...networkElementToAdd } as NetworkElementAsPlannedDtoCreate;
    if (obj && obj["key"]) {
      copy.locationId = obj["key"];
    } else {
      copy.locationId = undefined;
    }

    setNetworkElementToAdd(copy);
  };

  const onChangeNetworkElement = (
    property: string,
    val?: string,
    obj?: any
  ) => {
    let copy = { ...networkElementToAdd } as NetworkElementAsPlannedDtoCreate;
    if (obj && obj["key"]) {
      copy[property] = obj["key"];
    } else if (val != undefined) {
      copy[property] = val;
    }
    setNetworkElementToAdd(copy);
  };

  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />
      <ModalConfirm data={confirmPlanned} />
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          onHideModel();
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                onHideModel();
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {ReturnLookupContainer(isVisibleModalLookup)}
        </DialogContent>
      </Dialog>
      <Tabs
        defaultActiveKey={keyTabs}
        id="uncontrolled-tab-example"
        activeKey={keyTabs}
        onSelect={(x) => setKey(x || "")}
      >
        <Tab eventKey="operational" title="Design Aspect">
          <form onChange={() => setChanged(true)}>
            <div className="row col-12 px-0 mx-0">
              <div className="col-12 p-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mt-3">Implementation Details</label>
                  <div className="row">
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          OpCo <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.opcosResource &&
                                  dictionaryToArray(formData?.opcosResource)
                                }
                                value={
                                  formData?.opcosResource &&
                                  dictionaryToArray(
                                    formData?.opcosResource
                                  ).filter((x) => x.key === formData?.opCoId)
                                }
                                onChange={(e) => {
                                  onChangeOpco(e);
                                }}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                isDisabled={props.edit}
                              ></Select>
                            </div>
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("opCoId") ? (
                            <label className="validation">
                              *OpCo must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 pr-0">
                        <label className="labelForm w-100">
                          <label className="labelForm voda-bold mb-0">
                            Design Component Family
                            <span className="red">*</span>
                          </label>
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.dcFsResource &&
                              dictionaryToArray(formData?.dcFsResource).sort(
                                (a, b) =>
                                  a.value.toLowerCase().trim() <
                                  b.value.toLowerCase().trim()
                                    ? -1
                                    : 1
                              )
                            }
                            value={
                              formData?.dcFsResource
                                ? dictionaryToArray(
                                    formData?.dcFsResource
                                  ).filter(
                                    (x) =>
                                      x.key ===
                                      formData?.designComponentFamilyId
                                  )
                                : null
                            }
                            onChange={(e) => {
                              //onChangeSelect("designComponentFamilyId", e);
                              onChangeDCF(e);
                            }}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            isDisabled={
                              props?.dcfId || props.edit ? true : false
                            }
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            formatOptionLabel={function (data) {
                              return (
                                <span
                                  dangerouslySetInnerHTML={{
                                    __html: data.value,
                                  }}
                                />
                              );
                            }}
                          ></Select>
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes(
                            "designComponentFamilyId"
                          ) ? (
                            <label className="validation">
                              *Design Component family must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>

                    <div className="col-12">
                      <div className="col-12 mb-3 mt-4">
                        <label className="labelForm voda-bold text-uppercase mb-0 w-100 widthAuto">
                          <div className="switchContainer d-flex flex-row align-items-center">
                            <label className="switch mr-2">
                              <input
                                type="checkbox"
                                checked={formData?.platformSoftware ?? false}
                                disabled
                              />

                              <span
                                className="slider round"
                                style={{
                                  opacity: 0.5,
                                  cursor: "not-allowed",
                                }}
                              ></span>
                            </label>
                            Is this a Platform Software (Eg: Broadcom)?
                          </div>
                        </label>
                      </div>
                    </div>

                    <div className="col-12">
                      <div className="col-12 p-0">
                        <label className="labelForm w-100">
                          <label className="labelForm voda-bold mb-0">
                            Used Functional Entities
                          </label>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                className="w-100"
                                options={
                                  formData?.usedNetworkFunctionsResource &&
                                  dictionaryToArray(
                                    formData?.usedNetworkFunctionsResource
                                  )
                                }
                                value={
                                  formData?.usedNetworkFunctionsResource &&
                                  dictionaryToArray(
                                    formData?.usedNetworkFunctionsResource
                                  ).filter((el) =>
                                    formData.usedNetworkFunctionsIds?.includes(
                                      el.key
                                    )
                                  )
                                }
                                onChange={(e) =>
                                  OnChangeMultiSelect(
                                    "usedNetworkFunctionsIds",
                                    e
                                  )
                                }
                                // onKeyUp={(e) => OnChangeMultiSelect("planningRisksNetworkElement", e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isMulti
                                getOptionLabel={(option) =>
                                  option.value.toString()
                                }
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              />
                            </div>
                            {/* {tipologicaPermesso && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(11)}
                                type="button"
                              >
                                <img
                                  style={{ height: 15 }}
                                  src={require("../../img/plus_icon.png")}
                                  alt="plus"
                                />
                              </button>
                            )} */}
                          </div>
                        </label>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>
              <div className="col-12 p-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mt-3">
                    Supported Services Management
                  </label>
                  <div className="row">
                    <div className="col-12">
                      <div className="form-group col-12 p-0">
                        <label className="labelForm voda-bold w-100 d-flex align-items-center mb-3">
                          For the Subnetwork Boundary
                        </label>
                        <p>{formData?.subNetworkBoundary}</p>
                      </div>

                      <div className="col-12 p-0">
                        <div className="form-group">
                          <label className="voda-bold w-100 d-flex align-items-center">
                            Please select the Supported Services
                          </label>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                className="w-100"
                                options={
                                  formData?.supportedServicesResource &&
                                  dictionaryToArray(
                                    formData?.supportedServicesResource
                                  )
                                }
                                value={
                                  formData?.supportedServicesResource &&
                                  dictionaryToArray(
                                    formData?.supportedServicesResource
                                  ).filter((el) =>
                                    formData.supportedServicesIds?.includes(
                                      el.key
                                    )
                                  )
                                }
                                onChange={(e) =>
                                  OnChangeMultiSelect("supportedServicesIds", e)
                                }
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isMulti
                                isDisabled={
                                  formData?.isSupportedAllServices
                                    ? true
                                    : false
                                }
                                getOptionLabel={(option) =>
                                  option.value.toString()
                                }
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              />
                            </div>
                            {/* {tipologicaPermesso &&
                              formData &&
                              !formData.isSupportedAllServices && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(12)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )} */}
                          </div>
                          {/* {validation &&
                          validation.response == false &&
                          validation.property?.includes(
                            "supportedServicesIds"
                          ) ? (
                            <label className="validation">
                              *Supported Services must have a value
                            </label>
                          ) : null} */}
                        </div>
                      </div>

                      <div className="col-6 p-0">
                        <div className="form-group">
                          <label className="voda-bold w-100 d-flex align-items-center">
                            Description
                          </label>
                          <textarea
                            onChange={(e) => onChange("description", e)}
                            className="w-100"
                            cols={6}
                            placeholder="Long descritpion"
                            value={formData?.description}
                            maxLength={2000}
                          ></textarea>
                        </div>
                      </div>
                    </div>
                  </div>
                  {formData?.daMigrationStatusEntity &&
                    formData?.daMigrationStatusEntity.length > 0 && (
                      <div className="col-12 p-0">
                        <div className="form-group col-12 p-0">
                          <label className="voda-bold w-100 d-flex align-items-center">
                            Site Level Infra Readiness Status
                          </label>
                          <table
                            className="w-100 table table-responsive"
                            style={{ justifyItems: "center" }}
                            tabIndex={-1}
                          >
                            <thead>
                              <tr className="intestazione">
                                <th
                                  className="customVolteKPIHead text-left pl-2"
                                  style={{
                                    fontSize: "13px",
                                    padding: "0 20px",
                                  }}
                                >
                                  <div className="h-100 d-flex align-items-center divFilter">
                                    <span>OpCo</span>
                                  </div>
                                </th>
                                <th
                                  className="customVolteKPIHead text-left pl-2"
                                  style={{
                                    fontSize: "13px",
                                    padding: "0 20px",
                                  }}
                                >
                                  <div className="h-100 d-flex align-items-center divFilter">
                                    <span>Location</span>
                                  </div>
                                </th>
                                <th
                                  className="customVolteKPIHead text-left pl-2"
                                  style={{ fontSize: "13px" }}
                                >
                                  <div
                                    className="h-100 d-flex align-items-center divFilter"
                                    style={{ paddingLeft: "20px" }}
                                  >
                                    <span>Status</span>
                                  </div>
                                </th>
                              </tr>
                            </thead>

                            <tbody>
                              {formData?.daMigrationStatusEntity?.map(
                                (item, index) => (
                                  <tr className="dati" key={index}>
                                    <td>
                                      <div>{item.opco}</div>
                                    </td>
                                    <td>
                                      <div>{item.location}</div>
                                    </td>
                                    <td style={{ paddingLeft: "20px" }}>
                                      {item.status}
                                    </td>
                                  </tr>
                                )
                              )}
                            </tbody>
                          </table>
                        </div>
                      </div>
                    )}
                  {assetData && assetData.length > 0 && (
                    <div className="col-12 p-0">
                      <div className="form-group col-12 p-0">
                        <label className="voda-bold w-100 d-flex align-items-center">
                          Asset Migration Status
                        </label>

                        <AssetsDetailsGrid
                          data={assetData ?? []}
                          pagination={query1}
                          renderGrid={renderGridState?.render ?? []}
                          action={{
                            Filter: setQuery1,
                            setIsFiltriAttivati,
                            Edit: () => {},
                            onDelete: () => {},
                            isEnable: false,
                          }}
                        />

                        <Paginate
                          pagination={{
                            page: query1.page,
                            pageSize: query1.pageSize ?? 10,
                          }}
                          totalItems={GridAssetDto?.totalItems}
                          actions={{ next: next1, back: back1 }}
                        />
                      </div>
                    </div>
                  )}
                </fieldset>
              </div>

              <button
                className="mt-20 further-btn"
                type="button"
                onClick={() => setShowFurtherSection(!showFurtherSection)}
              >
                Click for further details
              </button>
              {showFurtherSection && (
                <>
                  <div className="col-12 p-0 mt-3">
                    <label className="text-bb">Security Classification</label>
                    <div className="row">
                      <div className="col-12">
                        <div className="form-group">
                          <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                            <div className="switchContainer d-fe mt-4 mb-3">
                              Critical National Infrastructure
                              <label className="switch">
                                <input
                                  type="checkbox"
                                  onChange={(e) =>
                                    onChange("countrySpecificCriticality", e)
                                  }
                                  className="mr-1"
                                  checked={formData?.countrySpecificCriticality}
                                  disabled={props.edit === true ? true : false}
                                />
                                <span
                                  className={`slider round ${
                                    props.edit === true && "disabledCursor"
                                  }`}
                                ></span>
                              </label>
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Security Tier / Zone
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.securityTireZoneResource &&
                                    dictionaryToArray(
                                      formData?.securityTireZoneResource
                                    )
                                  }
                                  value={
                                    formData?.securityTireZoneResource &&
                                    dictionaryToArray(
                                      formData?.securityTireZoneResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.securityTireZoneId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("securityTireZoneId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(2)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pr-0">
                          <label className="labelForm voda-bold w-100">
                            Security Manager
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.securityManagersResource &&
                                    dictionaryToArray(
                                      formData?.securityManagersResource
                                    )
                                  }
                                  value={
                                    formData?.securityManagersResource &&
                                    dictionaryToArray(
                                      formData?.securityManagersResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.securityManagerId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("securityManagerId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(3)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="col-12 p-0 mt-3">
                    <label className="text-bb mb-4">
                      Authentication Details
                    </label>
                    <div className="row">
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Authentication Type
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.authenicationTypesResource &&
                                    dictionaryToArray(
                                      formData?.authenicationTypesResource
                                    )
                                  }
                                  value={
                                    formData?.authenicationTypesResource &&
                                    dictionaryToArray(
                                      formData?.authenicationTypesResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.authenicationTypeId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("authenicationTypeId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(4)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pr-0">
                          <label className="labelForm voda-bold w-100">
                            3rd Party Access Type
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.thirdPartyAccessResource &&
                                    dictionaryToArray(
                                      formData?.thirdPartyAccessResource
                                    )
                                  }
                                  value={
                                    formData?.thirdPartyAccessResource &&
                                    dictionaryToArray(
                                      formData?.thirdPartyAccessResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.thirdPartyAccessId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("thirdPartyAccessId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(5)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            License Model
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.licenseModelsResource &&
                                    dictionaryToArray(
                                      formData?.licenseModelsResource
                                    )
                                  }
                                  value={
                                    formData?.licenseModelsResource &&
                                    dictionaryToArray(
                                      formData?.licenseModelsResource
                                    ).filter(
                                      (x) => x.key === formData?.licenseModelId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("licenseModelId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(7)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pr-0">
                          <label className="labelForm voda-bold w-100">
                            Functional Release Cycle Period
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.swDeliveryLifeCyclesResource &&
                                    dictionaryToArray(
                                      formData?.swDeliveryLifeCyclesResource
                                    )
                                  }
                                  value={
                                    formData?.swDeliveryLifeCyclesResource &&
                                    dictionaryToArray(
                                      formData?.swDeliveryLifeCyclesResource
                                    ).filter(
                                      (x) =>
                                        x.key ===
                                        formData?.swDeliveryLifeCycleId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("swDeliveryLifeCycleId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(6)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="col-12 p-0 mt-3">
                    <label className="text-bb mb-4">
                      Site Resilience Model
                    </label>
                    <div className="row">
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Site Resilience
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.siteResiliencesResource &&
                                    dictionaryToArray(
                                      formData?.siteResiliencesResource
                                    )
                                  }
                                  value={
                                    formData?.siteResiliencesResource &&
                                    dictionaryToArray(
                                      formData?.siteResiliencesResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.siteResilienceId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("siteResilienceId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(8)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pr-0">
                          <label className="labelForm voda-bold w-100">
                            Method Of Business Continuity
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.siteResilienceMethodsResource &&
                                    dictionaryToArray(
                                      formData?.siteResilienceMethodsResource
                                    )
                                  }
                                  value={
                                    formData?.siteResilienceMethodsResource &&
                                    dictionaryToArray(
                                      formData?.siteResilienceMethodsResource
                                    ).filter(
                                      (x) =>
                                        x.key ===
                                        formData?.businessContinuityMethodId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect(
                                      "businessContinuityMethodId",
                                      e
                                    )
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(9)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Instance Level Resilience
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.instanseResiliencesResource &&
                                    dictionaryToArray(
                                      formData?.instanseResiliencesResource
                                    )
                                  }
                                  value={
                                    formData?.instanseResiliencesResource &&
                                    dictionaryToArray(
                                      formData?.instanseResiliencesResource
                                    ).filter(
                                      (x) =>
                                        x.key === formData?.instanceResilienceId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("instanceResilienceId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(10)}
                                  type="button"
                                >
                                  <img
                                    style={{ height: 15 }}
                                    src={require("../../img/plus_icon.png")}
                                    alt="plus"
                                  />
                                </button>
                              )}
                            </div>
                          </label>
                        </div>
                      </div>

                      <div className="col-6">
                        <div className="col-12">
                          <label className="labelForm voda-bold w-100">
                            Nominal Capacity Limit
                            <input
                              onChange={(e) =>
                                onChange("nominalCapacityLimit", e)
                              }
                              onKeyUp={(e) =>
                                onChange("nominalCapacityLimit", e)
                              }
                              type="text"
                              className="inputForm w-100"
                              value={formData?.nominalCapacityLimit}
                            />
                          </label>
                        </div>
                      </div>

                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Designed Capacity Limit
                            <input
                              onChange={(e) =>
                                onChange("designedCapacityLimit", e)
                              }
                              onKeyUp={(e) =>
                                onChange("designedCapacityLimit", e)
                              }
                              type="text"
                              className="inputForm w-100"
                              value={formData?.designedCapacityLimit}
                            />
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12">
                          <label className="labelForm voda-bold w-100">
                            Max Allowed Loading
                            <input
                              onChange={(e) => onChange("maxAllowedLoading", e)}
                              onKeyUp={(e) => onChange("maxAllowedLoading", e)}
                              type="text"
                              className="inputForm w-100"
                              value={formData?.maxAllowedLoading}
                            />
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>

                  <div className="col-12 mt-4 p-0">
                    <div className="row">
                      <div className="col-6">
                        {props.edit === true ? (
                          <div className="form-group col-12 pl-0">
                            <label className="labelForm voda-bold   w-100">
                              Last Modified
                              <input
                                readOnly={true}
                                className="inputForm w-100 voda-regular"
                                type="text"
                                value={formatDateWithTime(
                                  formData?.lastModified
                                )?.toUpperCase()}
                              />
                            </label>
                          </div>
                        ) : null}
                      </div>
                      <div className="col-6">
                        {props.edit === true ? (
                          <div className="form-group col-12">
                            <label className="labelForm voda-bold w-100">
                              Last Modified By
                              <input
                                readOnly={true}
                                className="inputForm w-100 voda-regular"
                                type="text"
                                value={formData?.lastModifiedBy}
                              />
                            </label>
                          </div>
                        ) : null}
                      </div>
                    </div>
                  </div>
                </>
              )}
            </div>
          </form>
        </Tab>
        <Tab
          eventKey="plannedActivities"
          title="Planned Activities"
          disabled={!props.edit}
        >
          <DesignPlannedActivity
            idDetail={props.idDetail}
            pagination={query}
            action={{
              Delete,
              Edit,
              New,
              setChanged,
              Filter: setQuery,
              AddOnList,
              GetElementFromList,
              setShowForm,
              setConfirm: setConfirmPlanned,
              closeModal: props.action.closeModal,
            }}
            showForm={showForm}
            principalId={0}
            formArray={formData?.plannedActivityDto}
            opco={opco}
            opcoId={formData?.opCoId}
            designComponentFamily={formData?.designComponentFamilyName}
            designComponentFamilyId={formData?.designComponentFamilyId}
            designComponent={undefined}
            productImportance={undefined}
            numberOfNodesInProd={0}
            numberOfNodesInLab={0}
            eduSpoc={undefined}
            edit={props.edit}
            subdomainSpoc={undefined}
            isFromNetworkElement={false}
            isDesignAspect={true}
            plannedActivityTypeForEnum={
              PlannedActivityTypeForEnum["DesignAspect"]
            }
            buildBagIds={0}
            dcfResource={formData?.dcFsResource}
            onRequestParentSave={SaveOrConfirmPlanned}
            isAssetDetailContainer={isAssetDetailContainer}
          />
        </Tab>
      </Tabs>

      <div className="col-12 justify-content-end d-flex footerModal">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(false)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => SaveOrConfirmPlanned()}
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default DesignAspectModal;
