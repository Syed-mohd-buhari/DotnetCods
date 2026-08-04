import React, { useState, useEffect, useRef, useCallback } from "react";
import DatePicker from "react-datepicker";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  DataModalConfirm,
  stateConfirm,
  PlannedActivityTypeForEnum,
} from "../../Model/Common";
import {
  boolOptions,
  lowerFirstLetter,
  numberIsNullOrZero,
} from "../../Hook/Common";
import {
  dictionaryToArray,
  dictionaryToArraySettingsUpdatePlannedActivityDto,
  dictionaryToArrayCrossSettingsDto,
  dictionaryToArrayLocationDto,
  dictionaryToArrayDeploymentStatusDto,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import ModalConfirm from "../../Components/ModalConfirm";
import Select from "react-select";
import {
  GetCreateUpdatePlannedActivityStatus,
  GetDesignComponentListForPlannedToConnect,
  GetPlannedActivityListForUpdateStatus,
  GetPlannedActivityTypeswithDcIdAndOpcoId,
  GetSettingUpdatePlannedActivityResource,
  GetUpdateUpdatePlannedActivityStatus,
  GetplannedActivityCheckPlannedActivityTypefor,
  SaveUpdatePlannedActivityStatus,
  GetplannedActivityConfirmation,
  GetGetUpdateDaMigrationRecords,
  SavePaWhenDaMigrationCompleted,
  SavePaWithDaMigration,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import {
  UpdatePlannedActivityStatusDto,
  DaPlannedActivityDcfDto,
  PaWithDaMigrationApiResponse,
  DaMigrationStatusDtoGrid,
  PaWhenDaMigrationCompleteApiResponse,
} from "../../Model/PlannedActivity";
import Container from "../../Components/Container";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  LcmEngineeringDtoUpdate,
  NetworkElementAssociated,
} from "../../Model/LcmEngineering";
import { NetworkElementAsPlannedDtoCreate } from "../../Model/NetworkElementAsPlanned";
import { GetNetworkElementAsPlannedCreateResource } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCreateAction";
import { useSelector } from "react-redux";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import {
  Box,
  Checkbox,
  DialogActions,
  DialogProps,
  FormControlLabel,
  IconButton,
  useTheme,
} from "@mui/material";
import GridLink from "../GenerateLcmDb/GridLink";
import { useAuth } from "../../Hook/useAuth";
import { IoClose } from "react-icons/io5";
import Location from "../../Containers/Lookup/LocationContainer";
import { FaSearch } from "react-icons/fa";
import {
  GetAssetsDetailsGrid,
  GetAssetsPlatformMigrationGrid,
} from "../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import AssetsDetailsForm from "../Lookup/AssetPlatform/AssetsDetailsForm";
import {
  DaAssetMigrationGrid,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../Model/LookUp/AssetMigrationModels";
import { MdDelete } from "react-icons/md";
import ClusterModal from "../LcmEngineering/ClusterModal";
import { InfraClusterDtoUpdate } from "../../Model/InfraCluster";
import {
  InfraClusterHardwareResource,
  InfraClusterPaHardwareLevelGet,
  InfraClusterPaLevelGet,
  InfraClusterPaProgramLevelGet,
  InfraClusterResource,
  InfraProgramClusterResource,
} from "../../Redux/Action/LcmEngineering/InfraClusterCreateAction";
import { DropdownInputComponent } from "../../Components/FormField";
import ClusterProgram from "../LcmEngineering/ClusterProgram";

interface Props {
  action: {
    setIsVisibleModalStatus(val: boolean): any;
    updateActivityStatus?(id: number): any;
    changeNodesFromActivityStatus?(
      newNodeLab: number,
      newNodeProd: number
    ): any;
    Refresh?(): any;
    Update?(planId: number, deliveryStatus: number): any;
    callBackForRefreshGetUpdate?(id: number): any;
  };
  planningActivityDetailsResourceId: number | undefined;
  isFromPlannedActivityModal: boolean;
  isFromLandingPage?: boolean;
  lcmId?: number;
  isReleaseDetailUnKnown?: boolean | undefined;
  plannedActivityTypeForEnum?: PlannedActivityTypeForEnum;
  closeLCMModal?(): any;
  isDAPA?: boolean;
  dcfId?: number;
  plannedDcfId?: number;
  opCoId?: number;
  onOpenLookup?: (value: number) => void;
  addOnList?: (skipValidation?: boolean) => Promise<any>; // lowercase
  onRequestParentSave?: () => Promise<void>;
  daAssetMigrationDtoGrid?: any;
}

export interface PlannedActivityConfirmationResponseDto {
  designComponentName: string;
  dcfId: number | null;
  opcoId: number | null;
  opcoName: string;
  lcmId: string;
  lcmPaId: string;
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
  vecDate: undefined,
  startOfAppIntegration: undefined,
  migrationStart: undefined,
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

let infraClusterQuery: InfraClusterDtoUpdate = {
  infraClusterAsPlannedId: [],
  opCoValue: [],
  locationValue: [],
  site: "",
  platformValue: [],
  clustertypeValue: [],
  clusterName: "",
  hardwaretypeValue: [],
  deploymentStatusValue: [],
  verticalResponsibleValue: [],
  paId: 0,
};

const UpdatePlannedActivityStatusModal: React.FC<Props> = ({
  isDAPA = false,
  ...props
}) => {
  const [changed, setChanged] = useState(false);
  const [showLiveDate, setShowLiveDate] = useState(false);
  const [isMandate, setIsMandate] = useState(false);
  const [showDecommissionedDate, setShowDecommissionedDate] = useState(false);
  const [assestFormValid, setAssestFormValid] = useState(false);
  const [addAssetModal, setAddAssetModal] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [plannedActivityTypeForResource, setPlannedActivityTypeForResource] =
    useState<{ [key: string]: string }>();
  const [settingDescription, setSettingDescription] = useState<string>();

  const [plannedActivityTypeForId, setPlannedActivityTypeForId] = useState(
    props?.plannedActivityTypeForEnum
  );
  const [clusterUpgardeData, setClusterUpgradeData] = useState<any>();

  const [isResponseDialogOpen, setIsResponseDialogOpen] = useState(false);
  const [isRelatedRecord, setIsRelatedRecord] =
    useState<PlannedActivityConfirmationResponseDto[]>();
  const [dcfTableData, setDcfTableData] = useState<DaPlannedActivityDcfDto[]>(
    []
  );
  const [clusterData, setClusterData] = useState<any[]>([]);

  const [selectedDcfIds, setSelectedDcfIds] = useState<number[]>([]);
  const [statusOptions, setStatusOptions] = useState<
    { key: number; value: string }[]
  >([]);
  const [locationOptions, setLocationOptions] = useState<
    { key: number; value: string }[]
  >([]);
  const [selectedLocation, setSelectedLocation] = useState<{
    key: number;
    value: string;
  } | null>(null);
  const [selectedStatus, setSelectedStatus] = useState<{
    key: number;
    value: string;
  } | null>(null);
  const [newAssetNameFilter, setNewAssetNameFilter] = useState("");
  const [hardwareTypeOptions, setHardwareTypeOptions] = useState<any>([]);
  const [programClusterFlag, setProgramClusterFlag] = useState<boolean>(false);
  const [clusterProgramData, setClusterProgramData] = useState<any>();
  const [trafficFreeOption, setTrafficFreeOption] = useState<{
    key: number;
    value: string;
  }>();
  const [clusterDropdown, setClusterDropdown] = useState<any[]>([]);
  const [selectedCluster, setSelectedCluster] = useState<string | null>(null);

  const dtoNewResourceStateNetwork = (state: RootState) =>
    state.networkElementAsPlannedCreateReducer.NetworkElementAsPlannedDtoCreate;
  let createResourceNetwork = useSelector(dtoNewResourceStateNetwork);
  const programState = (state: RootState) =>
    state.infraProgramClusterResource?.InfraClusterDtoCreate;
  const programResource = useSelector(programState);
  const [validationErrors, setValidationErrors] = useState({});
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [settingId, setSettingId] = useState<number>();
  const [showLatestProgress, setShowLatestProgress] = useState<boolean>(false);
  const [assetMigrationData, setAssetMigrationData] = useState<any>(null);

  const [assetMigrationResponse, setAssetMigrationResponse] =
    useState<any>(null);
  const [gridData, setGridData] = useState<any[]>([]);
  const [gridLoading, setGridLoading] = useState(false);
  const [formData, setFormData] = useState<UpdatePlannedActivityStatusDto>();
  const [askUser, setAskUser] = useState<boolean>(false);
  const [needPlannedAsset, setNeedPlannedAsset] = useState<boolean>(false);
  const [isPlannedAssetAvailable, setIsPlannedAssetAvailable] =
    useState<boolean>(false);
  const [enableSubmit, setEnableSubmit] = useState<boolean>(false);
  const [dcDropFlag, setDcDropFlag] = useState<boolean>(false);
  const [modalClusterFlag, setModalClusterFlag] = useState<boolean>(false);
  const [updateDC, setUpdateDC] = useState<any>();
  const [isReleaseDcResource, setIsReleaseDcResource] = useState<any>();
  const [validazioneCustom, setValidazioneCustom] = useState<{
    response: boolean;
    property?: string;
    message?: string;
  }>();
  const [networkElementToAdd, setNetworkElementToAdd] =
    useState<NetworkElementAsPlannedDtoCreate | null>();
  const [isLatestSelected, setIsLatestSelected] = useState(false);
  const [daMigrationData, setDaMigrationData] = useState<
    DaMigrationStatusDtoGrid[]
  >([]);
  const [showTableForSecondAndThird, setShowTableForSecondAndThird] =
    useState(false);
  const [isEdit, setIsEdit] = useState<boolean>(false);
  const [editStatusId, setEditStatusId] = useState<any>(null);
  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [hasFetched, setHasFetched] = useState(false);
  const [siteDropdown, setSiteDropdown] = useState<any[]>([]);
  const [selectedSite, setSelectedSite] = useState<string | null>(null);
  const [isRemoveMode, setIsRemoveMode] = useState(false);

  const validationAssetMigration = (copy: any) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (copy.opCoId == 0 || copy.opCoId == undefined) {
      addInvalidProperty("opCoId");
    }
  };
  const validazioneClient = (copy: UpdatePlannedActivityStatusDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (copy.opCoId == 0 || copy.opCoId == undefined) {
      addInvalidProperty("opCoId");
    }
    if (
      !(isDAPA === true && props.plannedActivityTypeForEnum === 3) &&
      (copy.designComponentId === 0 || copy.designComponentId === undefined)
    ) {
      addInvalidProperty("designComponentId");
    }
    if (
      copy.planningActivityDetailsResourceId === 0 ||
      copy.planningActivityDetailsResourceId === undefined
    ) {
      addInvalidProperty("planningActivityDetailsResourceId");
    }
    if (
      copy.settingsUpdatePlannedActivityId == 0 ||
      copy.settingsUpdatePlannedActivityId == undefined
    ) {
      addInvalidProperty("settingsUpdatePlannedActivityId");
    }

    if (
      dcDropFlag &&
      (updateDC?.["key"] === null || updateDC?.["key"] === undefined)
    ) {
      addInvalidProperty("selectDCID");
    }
    if (copy.inEngineeringPhase == undefined) {
      addInvalidProperty("inEngineeringPhase");
    }
    if (
      copy.elementCount &&
      copy.ruleElementCount == 3 &&
      (copy.endNodesInProd == undefined || copy.endNodesInProd?.length == 0)
    ) {
      addInvalidProperty("endNodesInProd");
    }
    if (
      showLiveDate &&
      isMandate &&
      (copy.assetLiveStatusDate == undefined ||
        copy.assetLiveStatusDate == null)
    ) {
      addInvalidProperty("assetLiveStatusDate");
    }
    if (
      showDecommissionedDate &&
      (copy.assetDecommissionedDate == undefined ||
        copy.assetDecommissionedDate == null)
    ) {
      addInvalidProperty("assetDecommissionedDate");
    }
    if (formData?.ruleLinkedDc === 35 && !selectedCluster) {
      addInvalidProperty("clusetrId");
    }
    if (formData?.ruleLinkedDc === 35) {
      const hasClusters = filteredProgram && filteredProgram.length > 0;
      if (!hasClusters) {
        addInvalidProperty("isAtleastOneCluster");
      }
    }
    // console.log("copyValidation", copyValidation);
    setValidation(copyValidation);
    return copyValidation;
  };

  const LocationRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetNetworkElementAsPlannedCreateResource({
        isRefillData: true,
      });
      var getValue: Array<any> = Array.isArray(res?.locationReosurce)
        ? res?.locationReosurce
        : Array.isArray(value)
        ? value
        : [];

      var obj = getValue.reduce(
        (acc, item) => ({
          ...acc,
          [item.id]: item,
        }),
        {}
      );
      setNetworkElementToAdd({ ...networkElementToAdd, locationReosurce: obj });
    } catch (error) {
      console.error("Error in LocationRefillData:", error);
    }
  };
  const GridDto: QueryResultDtoOfDAAssetMigrationDtoGrid | null = useSelector(
    (state: RootState) =>
      state.daassetsMigrationGridReducer.DAAssetsMigrationGridResult
  );
  const dtoNewUpgradeResourceState = (state: RootState) =>
    state.infraClusterUpgradeReducer.InfraClusterDtoCreate;

  let createUpgradeResource = useSelector(dtoNewUpgradeResourceState);
  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 7:
          return (
            <Location
              returnObject={LocationRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            />
          );

        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );

  useEffect(() => {
    if (
      formData?.opCoId &&
      formData?.designComponentId &&
      !props?.planningActivityDetailsResourceId
    ) {
      returnPlannedActivityTypeResources();
    }
  }, [formData?.opCoId, formData?.designComponentId]);

  useEffect(() => {
    if (
      props?.planningActivityDetailsResourceId !== undefined &&
      props?.planningActivityDetailsResourceId !== null
    ) {
      GetEditResource(
        props.planningActivityDetailsResourceId,
        plannedActivityTypeForId! ?? props.plannedActivityTypeForEnum,
        true
      );
      setIsEdit(true);
    } else {
      GetCreateResource(true);
      setIsEdit(false);
    }
  }, [props?.planningActivityDetailsResourceId]);

  useEffect(() => {
    if (
      formData &&
      formData.planningActivityDetailsResourceId !== undefined &&
      formData.planningActivityDetailsResourceId !== null &&
      !props?.planningActivityDetailsResourceId
    ) {
      GetEditResource(
        formData.planningActivityDetailsResourceId,
        plannedActivityTypeForId! ?? props.plannedActivityTypeForEnum,
        true
      );
    }
    if (
      formData?.planningActivityDetailsResourceId !== undefined &&
      formData?.planningActivityDetailsResourceId !== null &&
      formData?.ruleLinkedDc === 34
    ) {
      InfraClusterPaLevelApiCall();
    }
    if (
      formData?.planningActivityDetailsResourceId !== undefined &&
      formData?.planningActivityDetailsResourceId !== null &&
      formData?.ruleLinkedDc === 36
    ) {
      InfraUpgradeClusterPaLevelApiCall();
    }
    if (
      formData?.planningActivityDetailsResourceId !== undefined &&
      formData?.planningActivityDetailsResourceId !== null &&
      formData?.ruleLinkedDc === 35
    ) {
      InfraClusterPaProgramLevelGetApiCall();
    }
  }, [formData?.planningActivityDetailsResourceId]);

  useEffect(() => {
    const fetchData = async () => {
      if (
        props?.planningActivityDetailsResourceId !== undefined &&
        props?.planningActivityDetailsResourceId !== null
      ) {
        const res = await GetGetUpdateDaMigrationRecords(
          props.planningActivityDetailsResourceId
        );

        if (res?.data?.statusKeyPairValue) {
          const options = dictionaryToArray(res.data.statusKeyPairValue);
          setStatusOptions(options);
        }
        if (res?.data?.locationResource) {
          const options = dictionaryToArray(res.data.locationResource);
          setLocationOptions(options);
        }

        if (res?.data?.daMigrationStatusDtoGrids) {
          setDaMigrationData(res.data.daMigrationStatusDtoGrids);
          setEditStatusId(
            res?.data?.daMigrationStatusDtoGrids?.map((val) => {
              return {
                id: val?.daMigrationStatusId,
                statusId: val?.statusId,
                status: val?.status,
              };
            })
          );
        }
      }
    };
    if (
      isDAPA &&
      showTableForSecondAndThird &&
      (formData?.ruleLinkedDc === 18 || formData?.ruleLinkedDc === 19)
    ) {
      fetchData();
    }
  }, [isDAPA, showTableForSecondAndThird, formData?.ruleLinkedDc]);

  useEffect(() => {
    const FechPlannedActivityDetailsAndSettingUpdatePlannedActivityResource =
      async () => {
        await onGetThePlannedActivityDetails();
      };

    if (
      plannedActivityTypeForId !== undefined &&
      plannedActivityTypeForId !== null &&
      formData
    ) {
      FechPlannedActivityDetailsAndSettingUpdatePlannedActivityResource();
    }
  }, [plannedActivityTypeForId]);

  useEffect(() => {
    if (
      formData?.opCoId &&
      formData?.designComponentId &&
      formData?.plannedActivityTypeId &&
      !props?.planningActivityDetailsResourceId
    ) {
      GetplannedActivityCheckPlannedActivityTypefor(
        formData?.designComponentId,
        formData?.opCoId,
        formData?.plannedActivityTypeId
      )
        .then((res) => {
          if (res?.data) {
            const palnnedActivityTyprForResourceArr = dictionaryToArray(
              res.data
            );
            if (palnnedActivityTyprForResourceArr.length === 1) {
              setPlannedActivityTypeForId(
                palnnedActivityTyprForResourceArr[0].key
              );
            }
            setPlannedActivityTypeForResource(res?.data);
          }
        })
        .catch((err) => console.log(err));
    }

    if (
      formData?.plannedActivityTypeId &&
      formData?.planningActivityDetailsResourceId
    ) {
      setShowLatestProgress(false);
    } else {
      setPlannedActivityTypeForId(undefined);
    }
  }, [formData?.plannedActivityTypeId]);

  useEffect(() => {
    if (networkElementToAdd == undefined) {
      GetNetworkElementAsPlannedCreateResource().then((x) => {
        setNetworkElementToAdd(x);
      });
    }
  }, []);

  //on change OpcO
  const onChangeOpcoId = async (e: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    if (e && e["key"]) {
      copy.opCoId = e["key"];
      const result = await GetDesignComponentListForPlannedToConnect(e["key"]);
      copy.designComponentResource = result?.data;
      setDesignComponentDetails(copy, copy.designComponentResource);
      copy.designComponentId = undefined;
      setFormData(copy);
    } else {
      setAskUser(false);
      GetCreateResource(false);
    }

    //Rimuovi Validazione
    if (validation?.property?.includes("opCoId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("opCoId");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  //on change current design compoennt
  const onChangeDesignComponent = async (e: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    copy.plannedActivityTypeId = undefined;
    copy.settingsUpdatePlannedActivityId = undefined;
    copy.planningActivityDetailsResourceId = undefined;
    setPlannedActivityTypeForId(undefined);
    setPlannedActivityTypeForResource(undefined);

    setAskUser(false);
    setSettingId(undefined);

    if (e && e["key"] !== undefined) {
      copy.designComponentId = e["key"];
      setFormData(copy);
    } else {
      copy.designComponentId = undefined;
      copy.inEngineeringPhase = undefined;
      setFormData(copy);
    }

    //Rimuovi Validazione
    if (validation?.property?.includes("designComponentId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("designComponentId");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  //on change planned activity type
  const onChangePlannedActivityType = (e: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    if (e && e["key"]) {
      copy.plannedActivityTypeId = e["key"];
      copy.planningActivityDetailsResourceId = undefined;
      setShowLatestProgress(true);
    } else {
      copy.plannedActivityTypeId = undefined;
      // copy.designComponentId = undefined;
      copy.planningActivityDetailsResourceId = undefined;
      setShowLatestProgress(false);
    }

    setFormData(copy);
  };

  //on change planned activity type For
  const onChangePlannedActivityTypeFor = (e: any) => {
    if (e && e.value) {
      setPlannedActivityTypeForId(e.key);
    } else {
      setPlannedActivityTypeForId(undefined);
    }
  };

  const onChangeStatus = (
    selectedOption: { key: number; value: string } | null,
    rowId: number
  ) => {
    setDaMigrationData((prevData) =>
      prevData.map((item) => {
        if (item.daMigrationStatusId === rowId) {
          return {
            ...item,
            statusId: selectedOption ? selectedOption.key : 0, // or keep old value if needed
          } as DaMigrationStatusDtoGrid; // ensure proper typing
        }
        return item;
      })
    );
  };
  const handleAddEntry = () => {
    let validationResult = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      validationResult.property.push(property);
      validationResult.response = false;
    };

    // === Required Field Validations ===
    if (!selectedLocation || selectedLocation?.key === 0) {
      addInvalidProperty("selectedLocation");
    }

    if (!selectedStatus || selectedStatus?.key === 0) {
      addInvalidProperty("selectedStatus");
    }

    // === Duplicate Check (only if both selectedLocation and status exist) ===
    const isDuplicate =
      selectedLocation &&
      daMigrationData.some((item) => item.locationId === selectedLocation.key);

    if (isDuplicate) {
      addInvalidProperty("duplicateEntry");
    }

    // === Apply validation ===
    setValidation(validationResult);
    if (!validationResult.response) return;

    // === Create new entry (now we're sure selectedLocation and selectedStatus are valid) ===
    const newRow: DaMigrationStatusDtoGrid = {
      daMigrationStatusId: 0,
      location: selectedLocation!.value, // '!' tells TS it's safe here
      locationId: selectedLocation!.key,
      status: selectedStatus!.value,
      statusId: selectedStatus!.key,
      opco: "",
      opcoId: Number(props.opCoId),
      plannedActivityId: Number(props.planningActivityDetailsResourceId),
      migratonStatus: {},
      deleted: false,
      orphan: false,
      lastModified: "",
      lastModifiedBy: "",
    };

    // === Add to table ===
    setDaMigrationData((prev) => [...prev, newRow]);

    // === Reset selections ===
    setSelectedLocation(null);
    setSelectedStatus(null);
  };

  const handleDeleteEntry = (index: number) => {
    setDaMigrationData((prev) => prev.filter((_, i) => i !== index));
  };

  //get  The Planned Activity Details
  const onGetThePlannedActivityDetails = async () => {
    const copy = { ...formData } as UpdatePlannedActivityStatusDto;

    const result = await GetPlannedActivityListForUpdateStatus(
      formData?.designComponentId,
      formData?.opCoId,
      formData?.plannedActivityTypeId,
      plannedActivityTypeForId!
    );

    copy.planningActivityDetailsResource = result?.data;
    if (dictionaryToArray(result?.data).length === 1) {
      copy.planningActivityDetailsResourceId = dictionaryToArray(
        result?.data
      )[0].key;
    }

    onGetSettingUpdatePlannedActivityResource(copy);
  };

  const onGetSettingUpdatePlannedActivityResource = async (
    copy: UpdatePlannedActivityStatusDto
  ) => {
    const result = await GetSettingUpdatePlannedActivityResource(
      formData?.plannedActivityTypeId!,
      plannedActivityTypeForId!
    );

    copy.settingsUpdatePlannedActivityResource = result?.data;
    setFormData(copy);
  };

  const returnPlannedActivityTypeResources = async () => {
    const copy = { ...formData } as UpdatePlannedActivityStatusDto;

    const result = await GetPlannedActivityTypeswithDcIdAndOpcoId(
      formData?.designComponentId,
      formData?.opCoId
    );

    copy.plannedActivityTypeResource = result?.data;
    if (dictionaryToArray(copy?.plannedActivityTypeResource!).length === 1) {
      copy.plannedActivityTypeId = dictionaryToArray(
        copy?.plannedActivityTypeResource!
      )[0].key;
      setShowLatestProgress(true);
    }
    setFormData(copy);
  };

  const GetCreateResource = async (loader: boolean) => {
    await GetCreateUpdatePlannedActivityStatus().then((x) => {
      if (x && x !== undefined) {
        setFormData(x);
        setSettingId(undefined);
      }
    });
  };

  const GetEditResource = async (
    id: number,
    plannedActivityypeForId: number,
    loader
  ) => {
    await GetUpdateUpdatePlannedActivityStatus(
      id,
      plannedActivityypeForId!
    ).then((x) => {
      if (x && x.data !== undefined) {
        //check if Dc Id to updated for the unknown DC
        const resource = x.data.settingsUpdatePlannedActivityResource ?? {};
        const selectedId = x.data.settingsUpdatePlannedActivityId;
        const allItems =
          dictionaryToArraySettingsUpdatePlannedActivityDto(resource);
        const selected = allItems.find((item) => item.key === selectedId);
        const maxOrder = Math.max(
          ...allItems.map((item) => item.value.order ?? 0)
        );
        const isLatest = selected?.value?.order === maxOrder;
        setIsLatestSelected(isLatest);
        const minOrder = Math.min(
          ...allItems.map((item) => item.value.order ?? 0)
        );
        const isSecondOrThird = selected?.value?.order !== minOrder;
        setShowTableForSecondAndThird(isSecondOrThird);
        setDcfTableData(x.data.daPlannedActivtyDcfDto || []);
        setClusterData(
          x.data.infraClusterClusterUpgradeUpsertDto
            ?.infraClusterAsPlannedDtoGrid || []
        );
        setClusterUpgradeData(
          x.data.infraClusterClusterUpgradeUpsertDto
            ?.infraClusterAsPlannedDtoGrid || []
        );
        let checkDC =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.specifyDC;
        let checkReleaseFlag =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.isReleaseDetailsUnknown;
        let checkPAReleaseFlag =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.isPAReleaseDetailsUnknown;

        (checkReleaseFlag || checkPAReleaseFlag) && checkDC
          ? setDcDropFlag(true)
          : setDcDropFlag(false);
        //
        if (props?.planningActivityDetailsResourceId) {
          setFormData({ ...x?.data });
          setIsReleaseDcResource(x?.data?.designComponentResource);
        } else {
          const copy = { ...formData } as UpdatePlannedActivityStatusDto;
          copy.settingsUpdatePlannedActivityResource =
            x?.data?.settingsUpdatePlannedActivityResource;
          copy.settingsUpdatePlannedActivityId =
            x?.data?.settingsUpdatePlannedActivityId;
          copy.crossSettingscResource = x?.data?.crossSettingscResource;
          copy.isCrossSettings = x?.data?.isCrossSettings;
          copy.numberOfNodesInLabInput = x?.data?.numberOfNodesInLabInput;
          copy.numberOfNodesInput = x?.data?.numberOfNodesInput;
          copy.elementCount = x?.data?.elementCount;
          copy.ruleElementCount = x?.data?.ruleElementCount;
          copy.startNodesInLab = x?.data?.startNodesInLab;
          copy.startNodesInProd = x?.data?.startNodesInProd;
          setIsReleaseDcResource(x?.data?.designComponentResource);
          setFormData(copy);
        }
        setSettingId(x.data.settingsUpdatePlannedActivityId);
        setShowLatestProgress(true);
      } else {
        setSettingId(undefined);
      }
    });
  };

  //ONCHANGE
  const onChangeSettings = (e: any, ruleElementCount: number) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    let val = e;

    if (+copy?.settingsUpdatePlannedActivityId! === 1 && +e === 7) {
      copy.endNodesInLab = copy.startNodesInLab;
      copy.endNodesInProd = copy.startNodesInProd;
    }
    copy.settingsUpdatePlannedActivityId = val;
    copy.ruleElementCount = ruleElementCount;

    if (ruleElementCount === 1) {
      copy.endNodesInProd = [];
    }

    let crossIds =
      formData &&
      formData.crossSettingscResource &&
      dictionaryToArrayCrossSettingsDto(formData.crossSettingscResource);

    let target = crossIds?.find(
      (x) => x.value.input == settingId && x.value.output == val
    );

    if (target !== null && target !== undefined) {
      copy.isCrossSettings = true;
    } else {
      copy.isCrossSettings = false;
    }

    //check if Dc Id to updated for the unknown DC
    let checkDC =
      formData?.settingsUpdatePlannedActivityResource?.[val].specifyDC;
    let checkReleaseFlag =
      formData?.settingsUpdatePlannedActivityResource?.[val]
        .isReleaseDetailsUnknown;
    let checkPAReleaseFlag =
      formData?.settingsUpdatePlannedActivityResource?.[val]
        .isPAReleaseDetailsUnknown;
    let LiveStatusFlag =
      formData?.settingsUpdatePlannedActivityResource?.[val]
        .isLiveStatusDateAvailable;
    let DecommissionFlag =
      formData?.settingsUpdatePlannedActivityResource?.[val]
        .isDecommissionedDateAvailable;

    (checkReleaseFlag || checkPAReleaseFlag) && checkDC
      ? setDcDropFlag(true)
      : setDcDropFlag(false);
    //

    if (target !== undefined && target.value.rule === 1) {
      setAskUser(true);
      copy.numberOfNodesInLabOutput = 0;
      copy.numberOfNodesOutput = 0;
    } else {
      setAskUser(false);
      copy.numberOfNodesInLabOutput = 0; //copy.numberOfNodesInLabInput;
      copy.numberOfNodesOutput = 0; //copy.numberOfNodesInput;
    }

    checkForNeedPlannedAssetRule(val)
      ? setNeedPlannedAsset(true)
      : setNeedPlannedAsset(false);

    checkForPlannedAssetAvailable()
      ? setIsPlannedAssetAvailable(true)
      : setIsPlannedAssetAvailable(false);

    if (formData?.ruleLinkedDc == 8 || formData?.ruleLinkedDc == 12) {
      setShowLiveDate(true);
      if (LiveStatusFlag) {
        setIsMandate(true);
      } else {
        setIsMandate(false);
      }
    }

    if (formData?.ruleLinkedDc == 13) {
      if (DecommissionFlag) setShowDecommissionedDate(true);
      else setShowDecommissionedDate(false);
    }

    if (formData?.ruleLinkedDc == 14) {
      if (LiveStatusFlag) {
        setIsMandate(true);
        setShowLiveDate(true);
        setShowDecommissionedDate(false);
      } else if (DecommissionFlag) {
        setShowDecommissionedDate(true);
        setIsMandate(false);
        setShowLiveDate(false);
      } else {
        setShowDecommissionedDate(false);
        setIsMandate(false);
        setShowLiveDate(false);
      }
    }

    setFormData(copy);
    //Rimuovi Validazione
    if (validation?.property?.includes("settingsUpdatePlannedActivityId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(
        "settingsUpdatePlannedActivityId"
      );
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeInEngineering = (e: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    if (e && e["key"]) {
      if (e["key"] == "YES") {
        copy.inEngineeringPhase = true;
      } else {
        copy.inEngineeringPhase = false;
      }
    } else {
      copy.inEngineeringPhase = undefined;
    }
    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes("inEngineeringPhase")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("inEngineeringPhase");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeDate = (property: string, newDate: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    if (newDate) {
      const DateString = `${newDate.getFullYear()}/${
        newDate.getMonth() + 1
      }/${newDate.getDate()}`;
      copy[lowerFirstLetter(property)] = DateString;
    }

    setFormData(copy);
  };

  //to auto papulated the  Current Design Component if options is one element
  const setDesignComponentDetails = (
    copy: UpdatePlannedActivityStatusDto,
    data: { [key: string]: string } | undefined
  ) => {
    if (dictionaryToArray(data!).length === 1) {
      copy.designComponentId = dictionaryToArray(data!)[0].key;
    }
  };

  const onChangePlannedActivityId = async (e: any) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    if (e && e["key"]) {
      copy.planningActivityDetailsResourceId = e["key"];
    } else {
      setAskUser(false);

      copy.settingsUpdatePlannedActivityId = undefined;
      copy.inEngineeringPhase = undefined;
      copy.planningActivityDetailsResourceId = undefined;
      setSettingId(undefined);
    }

    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes("planningActivityDetailsResourceId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(
        "planningActivityDetailsResourceId"
      );
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const rtnStatusClass = (id: number) => {
    // if (formData?.ruleLinkedDc === 19) {
    //   const platformMigrationOrder =
    //     formData?.settingsUpdatePlannedActivityResource
    //       ? dictionaryToArraySettingsUpdatePlannedActivityDto(
    //           formData?.settingsUpdatePlannedActivityResource
    //         )?.map((res: any) => ({ order: res?.value?.order, id: res?.key }))
    //       : null;
    //   const maxOrder = platformMigrationOrder
    //     ? Math.max(...platformMigrationOrder.map((item) => item.order))
    //     : null;
    //   const value =
    //     formData?.settingsUpdatePlannedActivityResource &&
    //     dictionaryToArraySettingsUpdatePlannedActivityDto(
    //       formData?.settingsUpdatePlannedActivityResource
    //     )?.filter((res) => res?.key === id);
    //   if (value?.[0]?.value?.order === maxOrder) {
    //     return true;
    //   }
    // }
    if (formData?.planningActivityDetailsResourceId == undefined) {
      return true;
    } else if (
      settingId == undefined ||
      settingId == null ||
      formData?.settingsUpdatePlannedActivityId == undefined
    ) {
      return true;
    } else if (
      settingId != undefined &&
      formData?.settingsUpdatePlannedActivityId != undefined
    ) {
      let updateOriginal =
        formData?.settingsUpdatePlannedActivityResource &&
        dictionaryToArraySettingsUpdatePlannedActivityDto(
          formData?.settingsUpdatePlannedActivityResource
        ).find((x) => x.key == settingId);
      let updateChecked =
        formData?.settingsUpdatePlannedActivityResource &&
        dictionaryToArraySettingsUpdatePlannedActivityDto(
          formData?.settingsUpdatePlannedActivityResource
        ).find((x) => x.key == id);

      if (updateOriginal && updateOriginal.value.isRollback != undefined) {
        if (updateOriginal.value.isRollback === true) {
          return false;
        }
      }
      if (
        updateChecked &&
        updateChecked.value.order != undefined &&
        updateOriginal &&
        updateOriginal.value.order != undefined &&
        updateOriginal.value.maxOrder != undefined
      ) {
        if (updateChecked.value.order < updateOriginal.value.order) {
          return true;
        }
        // else if (
        //   updateChecked.value.order > updateOriginal.value.maxOrder &&
        //   updateOriginal.value.maxOrder > 0
        // ) {
        //   return true;
        // }
        else {
          return false;
        }
      }
    }
    return false;
  };

  const handleConfirmDialog = async () => {
    const copyValidation = { response: true, property: [] } as CommonValidation;
    const addInvalidProperty = (property: string) => {
      copyValidation.property.push(property);
      copyValidation.response = false;
    };
    if (
      isDAPA &&
      formData?.ruleLinkedDc === 17 &&
      dcfTableData.length > 0 &&
      selectedDcfIds.length === 0
    ) {
      addInvalidProperty("designComponentfamilyId");
    }
    if (!copyValidation.response) {
      setValidation(copyValidation);
      return;
    }
    if (isDAPA && isLatestSelected) {
      try {
        if (props.dcfId !== undefined && props.opCoId !== undefined) {
          const existingIds =
            formData?.ruleLinkedDc !== 17 && props.dcfId !== undefined
              ? Array.isArray(props.dcfId)
                ? props.dcfId
                : [props.dcfId]
              : [];
          const dcfIdArray = Array.from(
            new Set([...existingIds, ...selectedDcfIds])
          );
          const res = await GetplannedActivityConfirmation(
            dcfIdArray,
            props.opCoId
          );

          if (res?.data && res.data.length > 0) {
            setIsRelatedRecord(res.data);
          }
          setIsResponseDialogOpen(true);
        }
      } catch (error) {
        console.error("API Error:", error);
      }
    } else {
      Save();
    }
  };
  const validateRows = (rows: any) => {
    // console.log("rows", rows);
    const errors = {};

    rows?.forEach((item, idx) => {
      const rowErrors: any = {};

      if (
        item?.newelEmentName !== null &&
        item?.newelEmentName !== "" &&
        (item?.newDeploymentStatusId === null ||
          item?.newDeploymentStatusId === undefined)
      ) {
        rowErrors.newDeploymentStatusId = "Required";
      }
      if (
        item?.newelEmentName !== null &&
        item?.newelEmentName !== "" &&
        item?.newDeploymentStatus?.toLowerCase() !== "planned" &&
        (item?.targetDesignComponenetId === null ||
          item?.targetDesignComponenetId === undefined)
      ) {
        rowErrors.targetDesignComponenetId = "Required";
      }

      if (
        item?.newelEmentName !== null &&
        item?.newelEmentName !== "" &&
        item?.newDeploymentStatusId !== null &&
        item?.targetDesignComponenetId !== null &&
        (item?.newEnvironmentId === null ||
          item?.newEnvironmentId === undefined)
      ) {
        rowErrors.newEnvironmentId = "Required";
      }

      if (Object.keys(rowErrors).length > 0) {
        errors[idx] = rowErrors;
      }
    });
    // console.log("Error", errors);
    return errors;
  };

  const handleDcfCheckboxChange = (id: number) => {
    setSelectedDcfIds((prevSelected) =>
      prevSelected.includes(id)
        ? prevSelected.filter((item) => item !== id)
        : [...prevSelected, id]
    );
  };

  const handleSelectAllChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.checked) {
      setSelectedDcfIds(
        dcfTableData.map((item) => item.designComponentfamilyId)
      );
    } else {
      setSelectedDcfIds([]);
    }
  };

  const handleSavePaWithDaMigration = async (
    planningActivityDetailsResourceId: number,
    settingsUpdatePlannedActivityId: number,
    statusOptions: { key: number; value: string }[],
    assetMigrationData: any[],

    daMigrationData: DaMigrationStatusDtoGrid[]
  ): Promise<any> => {
    const payload: PaWithDaMigrationApiResponse = {
      statusKeyPairValue: Object.fromEntries(
        statusOptions.map(({ key, value }) => [key, value])
      ),
      daMigrationStatusDtoGrids: daMigrationData,
      daMigrationData: daMigrationData,
    } as any;

    try {
      return await SavePaWithDaMigration(
        payload,
        planningActivityDetailsResourceId,
        settingsUpdatePlannedActivityId
      );
    } catch (error) {
      console.error("PaDaWithMigration API failed:", error);
      // rootStore.dispatch(
      //   setNotification({
      //     message: "Failed to save migration status. Please try again.",
      //     notifyType: NotifyType.error,
      //   })
      // );
      // throw error;
    }
  };
  const handleSavePaWhenDaMigrationCompleted = async (
    planningActivityDetailsResourceId: number,
    settingsUpdatePlannedActivityId: number,
    daMigrationData: DaMigrationStatusDtoGrid[]
  ): Promise<any> => {
    const payload: PaWhenDaMigrationCompleteApiResponse = {
      daAssetMigrationDtoGrid: daMigrationData,
    };

    try {
      return await SavePaWhenDaMigrationCompleted(
        payload,
        planningActivityDetailsResourceId,
        settingsUpdatePlannedActivityId
      );
    } catch (error) {
      console.error("PaDaWhenMigrationCompleted API failed:", error);
      // rootStore.dispatch(
      //   setNotification({
      //     message: "Failed to save migration status. Please try again.",
      //     notifyType: NotifyType.error,
      //   })
      // );
      // throw error;
    }
  };

  const Save = async () => {
    if (formData != undefined && validazioneClient(formData).response == true) {
      const updatedDcfDto = dcfTableData.map((item) => ({
        ...item,
        dcfStatus: selectedDcfIds.includes(item.designComponentfamilyId)
          ? "Completed"
          : item.dcfStatus,
      }));
      if (
        props.planningActivityDetailsResourceId != undefined &&
        formData.settingsUpdatePlannedActivityId != undefined &&
        formData?.ruleLinkedDc === 18
      ) {
        await handleSavePaWithDaMigration(
          props.planningActivityDetailsResourceId,
          formData.settingsUpdatePlannedActivityId,
          statusOptions,
          assetMigrationData,
          daMigrationData
        );
      }
      if (
        props.planningActivityDetailsResourceId != undefined &&
        formData.settingsUpdatePlannedActivityId != undefined &&
        formData?.ruleLinkedDc === 19
      ) {
        await handleSavePaWhenDaMigrationCompleted(
          props.planningActivityDetailsResourceId,
          formData.settingsUpdatePlannedActivityId,
          assetMigrationData
        );
      }
      if (formData?.ruleLinkedDc === 19) {
        if (props.addOnList) {
          await props.addOnList(true);
        }
      }
      const finalClusterData =
        formData?.ruleLinkedDc === 34
          ? clusterData
          : formData?.ruleLinkedDc === 36
          ? filteredClusters
          : [];
      await SaveUpdatePlannedActivityStatus({
        ...formData,
        daPlannedActivtyDcfDto: updatedDcfDto,
        endNodesInProd:
          formData?.isCrossSettings === true ? formData?.endNodesInProd : [],
        endNodesInLab:
          formData?.isCrossSettings === true ? formData?.endNodesInLab : [],
        plannedActivityTypeFor: plannedActivityTypeForId,
        designComponentId: updateDC
          ? updateDC?.["key"]
          : formData.designComponentId,
        infraClusterClusterUpgradeUpsertDto: {
          plannedHardwareTypeId:
            (formData as any)?.infraClusterClusterUpgradeUpsertDto
              ?.plannedHardwareTypeId ??
            (formData as any)?.plannedHardwareTypeId ??
            null,
          infraClusterAsPlannedDtoGrid: finalClusterData ?? [],
          nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
        },
      })
        .then((x) => {
          if (x && !x.warning) {
            if (props.action.updateActivityStatus) {
              props.action.updateActivityStatus(x.data?.deliveryStatusId);
              if (!formData.elementCount) {
                if (
                  formData?.numberOfNodesInLabInput !== undefined &&
                  formData?.numberOfNodesInLabInput !== null &&
                  formData?.numberOfNodesInLabOutput !== undefined &&
                  formData?.numberOfNodesInLabOutput !== null &&
                  formData?.numberOfNodesInput !== null &&
                  formData?.numberOfNodesInput !== undefined &&
                  formData?.numberOfNodesOutput !== null &&
                  formData?.numberOfNodesOutput !== undefined &&
                  askUser
                ) {
                  const newNodeLab =
                    formData?.numberOfNodesInLabInput -
                    formData?.numberOfNodesInLabOutput;
                  const newNodeProd =
                    formData?.numberOfNodesInput -
                    formData?.numberOfNodesOutput;
                  props.action.changeNodesFromActivityStatus &&
                    props.action.changeNodesFromActivityStatus(
                      newNodeProd,
                      newNodeLab
                    );
                }
              } else {
                const newNodeLab =
                  (formData?.startNodesInLab?.length ?? 0) -
                  (formData?.endNodesInLab?.length ?? 0);
                const newNodeProd =
                  (formData?.startNodesInProd?.length ?? 0) -
                  (formData?.endNodesInProd?.length ?? 0);
                props.action.changeNodesFromActivityStatus &&
                  props.action.changeNodesFromActivityStatus(
                    newNodeProd,
                    newNodeLab
                  );
              }
            }
            if (
              props.action.Update &&
              props.planningActivityDetailsResourceId != undefined &&
              formData.settingsUpdatePlannedActivityResource != undefined
            ) {
              let selected = dictionaryToArraySettingsUpdatePlannedActivityDto(
                formData.settingsUpdatePlannedActivityResource
              ).find((x) => x.key == formData.settingsUpdatePlannedActivityId)
                ?.value.deliveryStatusId;
              props.action.Update &&
                props.action.Update(
                  props.planningActivityDetailsResourceId,
                  selected ?? 0
                );
            }
            if (props?.isFromLandingPage) {
              props.action.setIsVisibleModalStatus(false);
            } else if (props.isFromPlannedActivityModal === false) {
              props.action.setIsVisibleModalStatus(false);
              rootStore.dispatch({ type: "REFRESH", payload: true });
            } else {
              props.action.setIsVisibleModalStatus(false);
              props.action.Refresh && props.action.Refresh();
            }
            // props.action.callBackForRefreshGetUpdate &&
            //   props.lcmId &&
            //   props.action.callBackForRefreshGetUpdate(props.lcmId);
            props?.closeLCMModal && props?.closeLCMModal!();
          }
        })
        .catch((err) => {
          props?.closeLCMModal && props?.closeLCMModal!();
        });
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  const onChangeNodes = (e: any, property: string) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    let value = e.target.value;

    // REMOVE VALIDATION
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    if (value < 0) {
      setValidazioneCustom({
        property: property,
        response: false,
        message: "*Value can not be negative",
      });
    } else if (property == "numberOfNodesOutput") {
      if (value > (copy.numberOfNodesInput ?? 0)) {
        setValidazioneCustom({
          property: property,
          response: false,
          message: "*Value can not be greater",
        });
      } else {
        copy.numberOfNodesOutput = value;
        setValidazioneCustom({ response: true });
      }
    } else if (property == "numberOfNodesInLabOutput") {
      if (value > (copy.numberOfNodesInLabInput ?? 0)) {
        setValidazioneCustom({
          property: property,
          response: false,
          message: "*Value can not be greater",
        });
      } else {
        copy.numberOfNodesInLabOutput = value;
        setValidazioneCustom({ response: true });
      }
    }
    setFormData(copy);
  };

  const selectRow = (
    list: string,
    checked: boolean,
    obj: NetworkElementAssociated,
    arrFrom: string
  ) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    let flag = false;
    const newObj = { ...obj, isFinalAsset: checked };
    if (checked) {
      if (copy[list] != undefined) {
        copy[list].push(newObj);
      } else {
        let arr = [] as NetworkElementAssociated[];
        arr.push(newObj);
        copy[list] = arr;
      }
    } else {
      let index = copy[list].findIndex((x) => x.id == obj.id);
      if (index != -1) {
        copy[list].splice(index, 1);
      }
    }
    copy[arrFrom] = copy[arrFrom]?.map((arr) => {
      if (arr.id === newObj.id) {
        return newObj;
      } else {
        return { ...arr, isFinalAsset: false };
      }
    });

    if (formData && formData?.endNodesInProd) {
      flag = formData.endNodesInProd?.some(
        (ele) => ele.assetsStatus === "PLANNED"
      );
    }
    setFormData(copy);

    return;
  };

  // console.log("Props", props);
  const checkForNeedPlannedAssetRule = (id: number) => {
    const flag = formData?.settingsUpdatePlannedActivityResource
      ? dictionaryToArraySettingsUpdatePlannedActivityDto(
          formData?.settingsUpdatePlannedActivityResource
        ).filter((x) => x.key == id)?.[0]["value"].needPlannedAsset
      : false;

    return flag;
  };

  const checkForPlannedAssetAvailable = () => {
    const flag =
      (formData?.startNodesInLab &&
        formData?.startNodesInLab?.filter((x) => x.assetsStatus === "PLANNED")
          .length > 0) ||
      (formData?.startNodesInProd &&
        formData?.startNodesInProd?.filter((x) => x.assetsStatus === "PLANNED")
          .length > 0)
        ? true
        : false;

    return flag;
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

  const onChangeLocation = (obj: any) => {
    let copy = { ...networkElementToAdd } as NetworkElementAsPlannedDtoCreate;
    if (obj && obj["key"]) {
      copy.locationId = obj["key"];
    } else {
      copy.locationId = undefined;
    }

    setNetworkElementToAdd(copy);
  };

  const clearFormNetworkElement = () => {
    setNetworkElementToAdd(createResourceNetwork);
  };

  const addNetworkElement = () => {
    if (
      networkElementToAdd?.elementName &&
      networkElementToAdd?.environmentId &&
      networkElementToAdd?.locationId
    ) {
      let copy = { ...formData } as UpdatePlannedActivityStatusDto;
      //let obj = copy.startNodesInProd as NetworkElementAsPlannedDtoCreate;
      let env =
        networkElementToAdd?.environmentReosurce &&
        dictionaryToArray(networkElementToAdd?.environmentReosurce).filter(
          (x) => x.key === networkElementToAdd?.environmentId
        )?.[0]["value"];

      let newobj = {
        elementName: networkElementToAdd?.elementName,
        enviroment:
          networkElementToAdd?.environmentReosurce &&
          dictionaryToArray(networkElementToAdd?.environmentReosurce).filter(
            (x) => x.key === networkElementToAdd?.environmentId
          )?.[0]["value"],
        location:
          networkElementToAdd?.locationReosurce &&
          dictionaryToArrayLocationDto(
            networkElementToAdd?.locationReosurce
          ).filter((x) => x.key === networkElementToAdd?.locationId)?.[0][
            "value"
          ].description,
        enviromentId: networkElementToAdd?.environmentId,
        locationId: networkElementToAdd?.locationId,
        assetsStatus: "PLANNED",
        assetsStatusId:
          networkElementToAdd?.deploymentStatusReosurce &&
          dictionaryToArrayDeploymentStatusDto(
            networkElementToAdd?.deploymentStatusReosurce
          ).filter(
            (x) => x.value.deploymentStatusDescription === "PLANNED"
          )?.[0]["key"],
        id: 0,
        isFinalAsset: false,
      } as NetworkElementAssociated;
      //obj.push(newobj);
      if (env === "PRODUCTION") {
        copy.startNodesInProd = formData?.startNodesInProd
          ? [...formData?.startNodesInProd, newobj]
          : [];
      } else {
        copy.startNodesInLab = formData?.startNodesInLab
          ? [...formData?.startNodesInLab, newobj]
          : [];
      }
      setFormData(copy);
      clearFormNetworkElement();
      setEnableSubmit(true);
    }
  };
  const handleOpenInNewTab = (item: {
    lcmPaId?: string;
    assetPaId?: string;
  }) => {
    const openTabWithCommaSeparatedParam = (
      path: string,
      paramName: string,
      csv: string
    ) => {
      const ids = csv
        .split(",")
        .map((id) => id.trim())
        .filter(Boolean);

      if (ids.length === 0) return;

      const queryParams = new URLSearchParams();
      queryParams.set(paramName, ids.join(",")); // Single comma-separated value

      const url = `/plannedActivities/${path}?${queryParams.toString()}`;
      window.open(url, "_blank", "noopener,noreferrer");
    };

    if (item.lcmPaId) {
      openTabWithCommaSeparatedParam("LCM", "lcmPaId", item.lcmPaId);
    }

    if (item.assetPaId) {
      openTabWithCommaSeparatedParam("Asset", "assetPaId", item.assetPaId);
    }
  };
  const initialQuery: DaAssetMigrationGrid = {
    ...paginationQueryAssets,

    plannedActivityId: props.planningActivityDetailsResourceId
      ? [props.planningActivityDetailsResourceId]
      : [],
    currentDcfId: props.dcfId ? [props.dcfId] : [],
    opcoId: props.opCoId ? [props.opCoId] : [],
  };
  const fetchGridData = async () => {
    await GetAssetsPlatformMigrationGrid(initialQuery);
  };

  // Fetch dropdown data using old API
  const fetchDropdownData = async () => {
    const result: any = await GetAssetsDetailsGrid(
      props.opCoId,
      props.dcfId,
      props.plannedDcfId,
      props.planningActivityDetailsResourceId
    );
    setAssetMigrationResponse(result);
  };

  useEffect(() => {
    if (GridDto?.items) {
      setAssetMigrationData(GridDto.items);
    }
  }, [GridDto]); // This runs when Redux data updates

  useEffect(() => {
    const loadData = async () => {
      if (
        isPermesso &&
        formData?.ruleLinkedDc === 19 &&
        props.opCoId &&
        props.dcfId &&
        props.planningActivityDetailsResourceId &&
        !hasFetched
      ) {
        await fetchGridData();
        await fetchDropdownData();
        setHasFetched(true);
      }
    };

    loadData();
  }, [formData?.ruleLinkedDc, hasFetched]);

  const handleInputChange = (e: any, index, field) => {
    // console.log("handleInputChange", e, index, field);
    const value =
      field === "locationId" ||
      field === "newEnvironmentId" ||
      field === "targetDesignComponenetId" ||
      field === "newDeploymentStatusId"
        ? e?.key
        : e;
    const updatedGrid = [...assetMigrationData];

    if (field === "newelEmentName" && e === "") {
      updatedGrid[index] = {
        ...updatedGrid[index],
        newelEmentName: "",
        targetDesignComponenetId: null,
        newDeploymentStatusId: null,
        targetDesignComponenet: "",
        newDeploymentStatus: "",
      };
    } else if (field === "newDeploymentStatusId") {
      updatedGrid[index] = {
        ...updatedGrid[index],
        [field]: e?.key,
        newDeploymentStatus: e?.value,
      };
    } else if (field === "oldAssetName") {
      updatedGrid[index] = {
        ...updatedGrid[index],
        [field]: e?.value,
        networkElementAsPlannedId: e?.key,
      };
    } else {
      updatedGrid[index] = {
        ...updatedGrid[index],
        [field]: value,
      };
    }
    validateRows(updatedGrid);
    // console.log("Data", updatedGrid[index]);
    if (
      (updatedGrid[index]?.newelEmentName !== "" &&
        updatedGrid[index]?.newDeploymentStatusId === null) ||
      (updatedGrid[index]?.newelEmentName !== "" &&
        updatedGrid[index]?.newDeploymentStatus?.toLowerCase() !== "planned") ||
      (updatedGrid[index]?.newelEmentName !== "" &&
        updatedGrid[index]?.targetDesignComponenetId !== null &&
        updatedGrid[index]?.newEnvironmentId === null)
    ) {
      setAssestFormValid(true);
    } else {
      setAssestFormValid(false);
    }
    // if (
    //   field === "newDeploymentStatusId" &&
    //   e?.value?.toLowerCase() !== "planned" &&
    //   updatedGrid[index]?.targetDesignComponenetId === null
    // ) {
    //   setAssestFormValid(true);
    // } else {
    //   setAssestFormValid(false);
    // }
    setAssetMigrationData(updatedGrid);
  };

  // console.log("Props", props, assetMigrationResponse);

  const isDuplicateNewAssetName = (name, currentIndex) => {
    if (!name || name.trim() === "") return false;

    const normalizedName = name.trim().toLowerCase();

    return assetMigrationData.some((item, index) => {
      if (index === currentIndex) return false;
      if (!item.newelEmentName || item.newelEmentName.trim() === "")
        return false;

      return item.newelEmentName.trim().toLowerCase() === normalizedName;
    });
  };

  const hasAnyDuplicate = () => {
    if (!assetMigrationData || !Array.isArray(assetMigrationData)) {
      return false;
    }

    return assetMigrationData.some((item, index) => {
      if (!item || !item.newelEmentName || item.newelEmentName.trim() === "") {
        return false;
      }
      console.log("dup", isDuplicateNewAssetName(item.newelEmentName, index));
      return isDuplicateNewAssetName(item.newelEmentName, index);
    });
  };
  const InfraClusterPaLevelApiCall = async () => {
    const opcoValue =
      formData?.opCoResource && formData.opCoId != undefined
        ? dictionaryToArray(formData?.opCoResource)?.filter(
            (x) => x.key == formData.opCoId
          )[0]?.value
        : null;
    const payload = {
      ...infraClusterQuery,
      opCoId: formData?.opCoId ? [formData?.opCoId] : [],
      paId: formData?.planningActivityDetailsResourceId ?? 0,
    } as InfraClusterDtoUpdate;
    const result = await InfraClusterPaLevelGet(payload);
    setClusterData(result?.InfraClusterGridResult?.items ?? []);
    InfraClusterResource({
      opCoId: formData?.opCoId,
      DcId: formData?.designComponentId,
    });
  };
  const InfraUpgradeClusterPaLevelApiCall = async () => {
    const opcoValue =
      formData?.opCoResource && formData.opCoId != undefined
        ? dictionaryToArray(formData?.opCoResource)?.filter(
            (x) => x.key == formData.opCoId
          )[0]?.value
        : null;
    const payload = {
      ...infraClusterQuery,
      opCoId: formData?.opCoId ? [formData?.opCoId] : [],
      paId: formData?.planningActivityDetailsResourceId ?? 0,
    } as InfraClusterDtoUpdate;
    const result: any = await InfraClusterPaHardwareLevelGet(payload);
    const data =
      result?.InfraClusterGridResult?.flatMap(
        (item) => item.infraClusterAsPlannedDtoGrid ?? []
      ) ?? [];

    setClusterUpgradeData(data ?? []);
    const siteOp = Array.from(
      new Set(result?.InfraClusterGridResult?.map((item) => item.site) ?? [])
    );
    const sites = siteOp.map((site) => ({
      key: site,
      value: site,
    }));
    setSiteDropdown(sites);
    await InfraClusterHardwareResource({
      opCoId: formData?.opCoId,
      DcId: formData?.designComponentId,
    });
    // console.log("Result", result);
  };
  useEffect(() => {
    if (createUpgradeResource && Array.isArray(createUpgradeResource)) {
      const hardwareArray = (createUpgradeResource as any[]) || [];
      const hardware =
        hardwareArray?.map((item: any) => ({
          key: item.key,
          value: item.text,
        })) || [];

      setHardwareTypeOptions(hardware);
    }
  }, [createUpgradeResource]);
  const handleChangeDropdown = (
    { property, value }: { property: string; value?: string[] },
    e: any
  ) => {
    let updated = { ...formData } as any;

    if (property === "plannedHardwareTypeId") {
      updated.infraClusterClusterUpgradeUpsertDto = {
        ...updated.infraClusterClusterUpgradeUpsertDto,
        plannedHardwareTypeId: e?.key ?? null,
      };
    } else {
      updated[property] = e?.key ?? null;
    }

    if (value && value.length > 0) {
      value.forEach((v) => {
        updated[v] = e?.value;
      });
    }

    setFormData(updated);

    if (property === "siteId") {
      setSelectedSite(e.key);
    }
    if (property === "clusetrId") {
      setSelectedCluster(e.key);
    }
    if (validation?.property?.includes(property)) {
      const updatedValidation = {
        ...validation,
        property: validation.property.filter((p: string) => p !== property),
      };
      setValidation(updatedValidation);
    }
  };
  const filteredClusters = formData?.plannedActivityId
    ? clusterUpgardeData
    : selectedSite
    ? clusterUpgardeData.filter((cluster: any) => cluster.site === selectedSite)
    : [];

  useEffect(() => {
    if (clusterUpgardeData?.length > 0) {
      const site = clusterUpgardeData[0]?.site;

      setSelectedSite(site);

      setFormData((prev: any) => ({
        ...prev,
        siteId: site,
      }));
    }
  }, [clusterUpgardeData]);
  const onSaveCluster = (data) => {
    setClusterData([...clusterData, data]);
  };
  //cluster Program//
  const isInService = (statusValue: string) =>
    statusValue?.replace(/-/g, "").toLowerCase() === "inservice";
  const InfraClusterPaProgramLevelGetApiCall = async () => {
    const payload = {
      ...infraClusterQuery,
      opCoId: formData?.opCoId ? [formData?.opCoId] : [],
      paId: formData?.planningActivityDetailsResourceId ?? 0,
    } as InfraClusterDtoUpdate;
    const result: any = await InfraClusterPaProgramLevelGet(payload);
    const data =
      result?.InfraClusterGridResult?.flatMap(
        (item) => item.nwElementClusterAsPlannedUpSertDto ?? []
      ) ?? [];
    console.log("filterData", data);
    setClusterProgramData(data ?? []);
    const clusterOp = Array.from(
      new Set(
        result?.InfraClusterGridResult?.map((item) => item.clusterName) ?? []
      )
    );
    const cluster = clusterOp.map((clusterName) => ({
      key: clusterName,
      value: clusterName,
      infraClusterAsPlannedId:
        result?.InfraClusterGridResult?.find(
          (item) => item.clusterName === clusterName
        )?.infraClusterAsPlannedId || 0,
    }));
    setClusterDropdown(cluster);
    await InfraProgramClusterResource({
      opCoId: formData?.opCoId,
      DcId: formData?.designComponentId,
    });
    // console.log("Result", result);
  };

  const filteredProgram = formData?.plannedActivityId
    ? clusterProgramData || []
    : selectedCluster
    ? (clusterProgramData || []).filter(
        (cluster) =>
          cluster.clusterName === selectedCluster || cluster.isNew === true
      )
    : clusterProgramData?.filter((cluster) => cluster.isNew === true);

  useEffect(() => {
    if (clusterProgramData?.length > 0) {
      const clusterName = clusterProgramData[0]?.clusterName;
      setSelectedCluster(clusterName);

      setFormData((prev: any) => ({
        ...prev,
        clusterId: clusterName,
      }));
    }
  }, [clusterProgramData]);

  const getInfraClusterAsPlannedId = () => {
    const cluster = clusterDropdown?.find((c) => c.key === selectedCluster);
    return cluster?.infraClusterAsPlannedId;
  };

  const onSaveClusterProgram = (data) => {
    console.log("saveclusterData", data);
    const newCluster = {
      ...data,
      clusterName: selectedCluster,
      isNew: true,
      infraClusterAsPlannedId: getInfraClusterAsPlannedId(),
    };
    setClusterProgramData((prev) => [...prev, newCluster]);
  };

  useEffect(() => {
    if (programResource?.deploymentStatuesResources) {
      const depOptions = programResource.deploymentStatuesResources.map(
        (item: any) => ({
          key: item.key,
          value: item.text,
        })
      );

      const trafficFree = depOptions.find(
        (res) => res.value?.toUpperCase() === "TRAFFIC FREE"
      );

      if (trafficFree) {
        setTrafficFreeOption(trafficFree);
      }
    }
  }, [programResource]);

  //end clusterProgram//
  return (
    <>
      <ModalConfirm data={confirm} />
      <Dialog
        open={addAssetModal}
        onClose={() => setAddAssetModal(false)}
        maxWidth="md"
        scroll="body"
        fullWidth
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">{"Add Asset Details"}</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setAddAssetModal(false)}
          sx={{ position: "absolute", right: 8, top: 8 }}
        >
          <IoClose size={25} />
        </IconButton>

        <DialogContent>
          <AssetsDetailsForm
            action={{
              closeModal: () => setAddAssetModal(false),
              onSubmit: (data) => {
                // console.log("FormData", data);
                setAssetMigrationData([...assetMigrationData, data]);
                setAddAssetModal(false);
              },
            }}
            edit={false}
            paId={props.planningActivityDetailsResourceId ?? 0}
            opcoId={props.opCoId ?? 0}
            existingAssetResource={
              assetMigrationResponse?.existingAssetResource ?? []
            }
            locationResource={assetMigrationResponse?.locationReosurce ?? {}}
            environmentResource={
              assetMigrationResponse?.environmentReosurce ?? {}
            }
            targetDesignComponentResource={
              assetMigrationResponse?.targetDesignComponentResource ?? []
            }
            deploymentStatusResource={
              assetMigrationResponse?.deploymentStatusReosurce ?? {}
            }
          />
        </DialogContent>
      </Dialog>
      {isVisibleModalLookup > 0 && (
        <Dialog
          open={isVisibleModalLookup > 0}
          onClose={(event, reason) => {
            if (reason === "backdropClick" || reason === "escapeKeyDown") {
              return;
            } else {
              setIsVisibleModalLookup(0);
            }
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
                  setIsVisibleModalLookup(0);
                }}
              >
                <IoClose size={25} />
              </IconButton>
            </Box>
            {ReturnLookupContainer(isVisibleModalLookup)}
          </DialogContent>
        </Dialog>
      )}
      <Dialog
        open={modalClusterFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setModalClusterFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-2 px-0">
            <h4>Add Cluster </h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setModalClusterFlag(false)}
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
          {" "}
          <ClusterModal
            action={{
              closeModal: () => setModalClusterFlag(false),
              onSaveCluster,
            }}
            edit={false}
          />
        </DialogContent>
      </Dialog>
      <Dialog
        open={programClusterFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setProgramClusterFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-2 px-0">
            <h4>Add Cluster </h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setProgramClusterFlag(false)}
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
          {" "}
          <ClusterProgram
            action={{
              closeModal: () => setProgramClusterFlag(false),
              onSaveClusterProgram,
            }}
            edit={false}
            existingAppClusterNames={
              clusterProgramData?.map((item) => item.appClusterName) || []
            }
          />
        </DialogContent>
      </Dialog>
      <form onChange={() => setChanged(true)}>
        <div className="col-12 d-flex justify-content-center row mx-0 p-0">
          <div className="col-12 row mx-0 p-0 d-flex">
            <div className="col-12 py-3 d-flex row mx-0 p-0 align-content-start">
              <Container
                show={props.planningActivityDetailsResourceId == undefined}
              >
                <div className="form-group col-6">
                  <label className="labelForm voda-bold w-100">
                    Please Select the OpCo <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.opCoResource &&
                            dictionaryToArray(formData?.opCoResource)
                          }
                          value={
                            formData?.opCoResource &&
                            formData.opCoId != undefined
                              ? dictionaryToArray(formData?.opCoResource).find(
                                  (x) => x.key == formData.opCoId
                                )
                              : null
                          }
                          onChange={(e) => onChangeOpcoId(e)}
                          // onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value ?? ""}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("opCoId") ? (
                      <label className="validation">
                        *OpCo must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="form-group col-6 ">
                  <label className="labelForm voda-bold w-100">
                    Please Select The Current Design Component
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.designComponentResource &&
                            dictionaryToArray(formData?.designComponentResource)
                          }
                          value={
                            formData?.designComponentResource &&
                            formData.designComponentId != undefined
                              ? dictionaryToArray(
                                  formData?.designComponentResource
                                ).find(
                                  (x) => x.key == formData.designComponentId
                                )
                              : null
                          }
                          onChange={(e) => onChangeDesignComponent(e)}
                          isDisabled={
                            formData &&
                            (formData?.opCoId == undefined ||
                              formData?.opCoId == 0)
                          }
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value ?? ""}
                          getOptionValue={(option) => option["key"].toString()}
                          formatOptionLabel={function (data) {
                            return (
                              <span
                                dangerouslySetInnerHTML={{
                                  __html: data.value ?? "",
                                }}
                              />
                            );
                          }}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("designComponentId") ? (
                      <label className="validation">
                        *design Component must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="form-group col-6 ">
                  <label className="labelForm voda-bold w-100">
                    The Planned Activity Type <span className="red">*</span>
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={
                          formData?.plannedActivityTypeResource &&
                          dictionaryToArray(
                            formData?.plannedActivityTypeResource
                          )
                        }
                        value={
                          formData?.plannedActivityTypeResource &&
                          formData.plannedActivityTypeId != undefined
                            ? dictionaryToArray(
                                formData?.plannedActivityTypeResource
                              ).find(
                                (x) => x.key == formData.plannedActivityTypeId
                              )
                            : null
                        }
                        onChange={(e) => onChangePlannedActivityType(e)}
                        isDisabled={!formData?.designComponentId}
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value ?? ""}
                        getOptionValue={(option) => option["key"].toString()}
                        formatOptionLabel={function (data) {
                          return (
                            <span
                              dangerouslySetInnerHTML={{
                                __html: data.value ?? "",
                              }}
                            />
                          );
                        }}
                      ></Select>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("plannedActivityTypeId") ? (
                      <label className="validation">
                        *planned Activity must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                {dictionaryToArray(plannedActivityTypeForResource!).length >
                  1 && (
                  <>
                    <div className="form-group col-6 ">
                      <label className="labelForm voda-bold w-100">
                        Planned Activity Type For<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={dictionaryToArray(
                                plannedActivityTypeForResource!
                              )}
                              value={dictionaryToArray(
                                plannedActivityTypeForResource!
                              ).find((x) => x.key === plannedActivityTypeForId)}
                              onChange={(e) =>
                                onChangePlannedActivityTypeFor(e)
                              }
                              isDisabled={!formData?.plannedActivityTypeId}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value ?? ""}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                            ></Select>
                          </div>
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes(
                          "planningActivityDetailsResourceId"
                        ) ? (
                          <label className="validation">
                            *planned Activity must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </>
                )}

                <div className="form-group col-6 ">
                  <label className="labelForm voda-bold w-100">
                    The Planned Activity Details <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.planningActivityDetailsResource &&
                            dictionaryToArray(
                              formData?.planningActivityDetailsResource
                            )
                          }
                          value={
                            formData?.planningActivityDetailsResource &&
                            formData.planningActivityDetailsResourceId !=
                              undefined
                              ? dictionaryToArray(
                                  formData?.planningActivityDetailsResource
                                ).find(
                                  (x) =>
                                    x.key ==
                                    formData.planningActivityDetailsResourceId
                                )
                              : null
                          }
                          onChange={(e) => onChangePlannedActivityId(e)}
                          isDisabled={!formData?.plannedActivityTypeId}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value ?? ""}
                          getOptionValue={(option) => option["key"].toString()}
                          formatOptionLabel={function (data) {
                            return (
                              <span
                                dangerouslySetInnerHTML={{
                                  __html: data.value ?? "",
                                }}
                              />
                            );
                          }}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes(
                      "planningActivityDetailsResourceId"
                    ) ? (
                      <label className="validation">
                        *planned Activity must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </Container>

              {showLatestProgress && (
                <>
                  <div className="form-group col-12 mt-2 ">
                    <label className="text-bb">
                      Please select the latest progress
                      <span className="red">*</span>
                    </label>
                    <label className="labelForm voda-bold w-100">
                      <div className="d-flex pt-1 row mx-0 ">
                        {formData?.settingsUpdatePlannedActivityResource &&
                          dictionaryToArraySettingsUpdatePlannedActivityDto(
                            formData?.settingsUpdatePlannedActivityResource
                          )
                            .sort((a, b) => a.value.order! - b.value.order!)
                            .map((x) => (
                              <div
                                className={`border col-md d-flex row mx-0 align-content-start justify-content-center text-center py-3 statusRadio ${
                                  rtnStatusClass(x.key) == true
                                    ? "disabled"
                                    : ""
                                }`}
                                key={x.key}
                              >
                                <input
                                  type="radio"
                                  name={"delivery" + x?.key}
                                  disabled={rtnStatusClass(x.key)}
                                  value={x.key}
                                  onChange={(e) => {
                                    const selectedKey = e.target.value;
                                    const selectedOrder = x.value.order;

                                    setSettingDescription(
                                      x?.value
                                        ?.settingsUpdatePlannedActivityDescription
                                    );
                                    const allItems =
                                      dictionaryToArraySettingsUpdatePlannedActivityDto(
                                        formData?.settingsUpdatePlannedActivityResource ??
                                          {}
                                      );

                                    const maxOrder = Math.max(
                                      ...allItems.map(
                                        (item) => item.value.order ?? 0
                                      )
                                    );

                                    const isLatest = selectedOrder === maxOrder;
                                    setIsLatestSelected(isLatest);
                                    if (!isLatest) {
                                      setSelectedDcfIds([]);
                                    }
                                    const minOrder = Math.min(
                                      ...allItems.map(
                                        (item) => item.value.order ?? 0
                                      )
                                    );
                                    const isSecondOrThird =
                                      selectedOrder !== minOrder;
                                    setShowTableForSecondAndThird(
                                      isSecondOrThird
                                    );
                                    onChangeSettings(
                                      selectedKey,
                                      x.value.ruleElementCount ?? 0
                                    );
                                  }}
                                  checked={
                                    formData.settingsUpdatePlannedActivityId ==
                                    x.key
                                  }
                                />
                                <label className="mt-2 mb-0 w-100">
                                  {
                                    x.value
                                      ?.settingsUpdatePlannedActivityDescription
                                  }
                                </label>
                              </div>
                            ))}
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes(
                        "settingsUpdatePlannedActivityId"
                      ) ? (
                        <label className="validation">
                          *Latest Progress must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                  <Container show={askUser}>
                    {formData?.elementCount ? (
                      <div className="col-12">
                        <div className="col-12 text-bb">
                          Please select the nodes for migrating into new Design
                          Component
                        </div>
                        <div className="d-flex row mx-0 col-12">
                          <div className="col-6 text-center">
                            <label>
                              <b>N. Of Prod. Network Element (Origin): </b>
                              {(formData?.startNodesInProd?.length ?? 0) -
                                (formData?.endNodesInProd?.length ?? 0)}
                            </label>
                          </div>
                          <div className="col-6 text-center">
                            <label>
                              <b>N. Of Lab. Network Element (Origin): </b>
                              {(formData?.startNodesInLab?.length ?? 0) -
                                (formData?.endNodesInLab?.length ?? 0)}
                            </label>
                          </div>
                        </div>
                        <div className="form-group col-12 px-0 mt-1 d-flex flex-row">
                          <div
                            className={`col-6 py-3 ${
                              formData.ruleElementCount === 1 ? "disabled" : ""
                            }`}
                          >
                            <div className="col-12 d-flex justify-content-center voda-bold">
                              <h6>PRODUCTION NETWORK ELEMENTS</h6>
                            </div>
                            <table className="w-100  ">
                              <thead>
                                <tr className="intestazione">
                                  <th className="pl-2 py-0">Element Name</th>
                                  <th className="pl-2 py-0">Environment</th>
                                  <th className="pl-2 py-0">Location</th>
                                  <th className="pl-2 py-0"></th>
                                </tr>
                              </thead>
                              <tbody>
                                {formData?.startNodesInProd?.map((item, i) => (
                                  <tr
                                    className={
                                      settingDescription
                                        ?.toLowerCase()
                                        .replaceAll(" ", "") != "rfsacheived"
                                        ? `dati`
                                        : ""
                                    }
                                    key={i}
                                    style={{
                                      background:
                                        settingDescription
                                          ?.toLowerCase()
                                          .replaceAll(" ", "") != "rfsacheived"
                                          ? ""
                                          : "#c1c1c1",
                                    }}
                                  >
                                    <td>{item.elementName}</td>
                                    <td>{item.enviroment}</td>
                                    <td>{item.location}</td>
                                    <td>
                                      <input
                                        type="checkbox"
                                        checked={
                                          item.id != undefined &&
                                          formData.endNodesInProd != undefined
                                            ? formData.endNodesInProd?.findIndex(
                                                (x) => x.id == item.id
                                              ) != -1
                                            : false
                                        }
                                        onChange={(e) => {
                                          item.id &&
                                            selectRow(
                                              "endNodesInProd",
                                              e.target.checked,
                                              item,
                                              "startNodesInProd"
                                            );
                                        }}
                                      ></input>
                                    </td>
                                  </tr>
                                ))}
                              </tbody>
                            </table>
                          </div>
                          <div className="col-6 py-3">
                            <div className="col-12 d-flex justify-content-center voda-bold">
                              <h6>LAB NETWORK ELEMENTS</h6>
                            </div>
                            <table className="w-100  ">
                              <thead>
                                <tr className="intestazione">
                                  <th className="pl-2 py-0">Element Name</th>
                                  <th className="pl-2 py-0">Environment</th>
                                  <th className="pl-2 py-0">Location</th>
                                  <th className="pl-2 py-0"></th>
                                </tr>
                              </thead>
                              <tbody>
                                {formData?.startNodesInLab?.map((item, i) => (
                                  <tr className={`dati`} key={i}>
                                    <td>{item.elementName}</td>
                                    <td>{item.enviroment}</td>
                                    <td>{item.location}</td>
                                    <td>
                                      <input
                                        type="checkbox"
                                        checked={
                                          item.id != undefined &&
                                          formData.endNodesInLab != undefined
                                            ? formData.endNodesInLab?.findIndex(
                                                (x) => x.id == item.id
                                              ) != -1
                                            : false
                                        }
                                        onChange={(e) => {
                                          item.id &&
                                            selectRow(
                                              "endNodesInLab",
                                              e.target.checked,
                                              item,
                                              "startNodesInLab"
                                            );
                                        }}
                                      ></input>
                                    </td>
                                  </tr>
                                ))}
                              </tbody>
                            </table>
                          </div>
                        </div>
                      </div>
                    ) : (
                      <div className="form-group col-12 mt-2 row mx-0">
                        <div className="col-12 text-bb">
                          Please select the migrated nodes to the new Design
                          Component
                        </div>
                        <div className="col-6">
                          <label className="text-bb">Original Nodes</label>
                          <div className="col-12 pl-0">
                            <label className="labelForm voda-bold   w-100">
                              No. of Nodes In Lab
                              <input
                                type="number"
                                readOnly
                                className="inputForm w-100"
                                value={formData?.numberOfNodesInLabInput}
                              />
                            </label>
                          </div>
                          <div className="col-12 pl-0">
                            <label className="labelForm voda-bold w-100">
                              No. of Nodes In Prod
                              <input
                                type="number"
                                readOnly
                                className="inputForm w-100"
                                value={formData?.numberOfNodesInput}
                              />
                            </label>
                          </div>
                        </div>

                        <div className="col-6">
                          <label className="text-bb mb-40 mt-50">
                            Migrated Nodes
                          </label>
                          <div className="col-12 pr-0">
                            <label className="labelForm voda-bold w-100">
                              No. of Nodes In Lab
                              <input
                                type="number"
                                min={0}
                                max={formData?.numberOfNodesInLabInput}
                                className="inputForm w-100"
                                onChange={(e) =>
                                  onChangeNodes(e, "numberOfNodesInLabOutput")
                                }
                                onKeyUp={(e) =>
                                  onChangeNodes(e, "numberOfNodesInLabOutput")
                                }
                                value={formData?.numberOfNodesInLabOutput}
                              />
                            </label>
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes(
                              "numberOfNodesInLabOutput"
                            ) ? (
                              <label className="validation">
                                *Number of Nodes value is not valid
                              </label>
                            ) : null}
                            {validazioneCustom &&
                            validazioneCustom.response == false &&
                            validazioneCustom.property ==
                              "numberOfNodesInLabOutput" ? (
                              <label className="validation">
                                {validazioneCustom.message}
                              </label>
                            ) : null}
                          </div>
                          <div className="col-12 pr-0">
                            <label className="labelForm voda-bold   w-100">
                              No. of Nodes In Prod
                              <input
                                type="number"
                                min={0}
                                max={formData?.numberOfNodesInput}
                                className="inputForm w-100"
                                onChange={(e) =>
                                  onChangeNodes(e, "numberOfNodesOutput")
                                }
                                onKeyUp={(e) =>
                                  onChangeNodes(e, "numberOfNodesOutput")
                                }
                                value={formData?.numberOfNodesOutput}
                                disabled={
                                  +formData?.settingsUpdatePlannedActivityId! ===
                                  5
                                }
                              />
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "numberOfNodesOutput"
                              ) ? (
                                <label className="validation">
                                  *Number of Nodes value is not valid
                                </label>
                              ) : null}
                              {validazioneCustom &&
                              validazioneCustom.response == false &&
                              validazioneCustom.property ==
                                "numberOfNodesOutput" ? (
                                <label className="validation">
                                  {validazioneCustom.message}
                                </label>
                              ) : null}
                            </label>
                          </div>
                        </div>
                      </div>
                    )}
                  </Container>
                  <Container
                    show={needPlannedAsset && !isPlannedAssetAvailable}
                  >
                    <div>
                      <label className="text-bb mb-4 mt-4">
                        Please Enter the Asset details to continue
                        <span className="red">*</span>
                      </label>

                      <fieldset className="fieldset mt-4">
                        <div className="row">
                          <div className="form-group col-3 pl-0">
                            <label className="labelForm voda-bold w-100">
                              Element Name
                              <input
                                type="text"
                                onChange={(e) =>
                                  onChangeNetworkElement(
                                    "elementName",
                                    e.target.value
                                  )
                                }
                                className="inputForm w-100"
                                value={networkElementToAdd?.elementName ?? ""}
                                style={{ position: "relative", top: "-6px" }}
                              />
                            </label>
                          </div>
                          <div className="form-group col-3 pl-0 pr-0">
                            <label className="labelForm voda-bold   w-100">
                              Environment
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition={"fixed"}
                                    options={
                                      networkElementToAdd?.environmentReosurce &&
                                      dictionaryToArray(
                                        networkElementToAdd?.environmentReosurce
                                      ).sort((a, b) =>
                                        a.value.toLowerCase() <
                                        b.value.toLowerCase()
                                          ? -1
                                          : 1
                                      )
                                    }
                                    value={
                                      networkElementToAdd?.environmentReosurce &&
                                      dictionaryToArray(
                                        networkElementToAdd?.environmentReosurce
                                      ).filter(
                                        (x) =>
                                          x.key ===
                                          networkElementToAdd?.environmentId
                                      )
                                    }
                                    onChange={(e) => {
                                      onChangeNetworkElement(
                                        "environmentId",
                                        undefined,
                                        e
                                      );
                                      //setEnvironmentSelect(true);
                                    }}
                                    //onBlur={() => setInputValue("")}
                                    isSearchable
                                    isClearable
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
                                </div>
                                {/* {tipologicaPermesso && !props.formDisabed && (
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
                                )} */}
                              </div>
                              {validation &&
                              validation.response === false &&
                              validation.property?.includes("environmentId") ? (
                                <label className="validation h-16">
                                  *Environment must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                          <div className="form-group col-3 pr-0">
                            <label className="labelForm voda-bold   w-100">
                              Location
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition={"fixed"}
                                    options={
                                      networkElementToAdd?.locationReosurce &&
                                      dictionaryToArrayLocationDto(
                                        networkElementToAdd?.locationReosurce
                                      )
                                        .map((x) => {
                                          return {
                                            key: x.key,
                                            value: x.value,
                                          };
                                        })
                                        .filter((x) => {
                                          return (
                                            x.value.opcoId == formData?.opCoId
                                          );
                                        })
                                    }
                                    value={
                                      networkElementToAdd?.locationId &&
                                      networkElementToAdd?.locationId !==
                                        undefined &&
                                      networkElementToAdd?.locationReosurce
                                        ? dictionaryToArrayLocationDto(
                                            networkElementToAdd?.locationReosurce
                                          )
                                            .map((x) => {
                                              return {
                                                key: x.key,
                                                value: x.value,
                                              };
                                            })
                                            .filter(
                                              (x) =>
                                                x.key ==
                                                networkElementToAdd?.locationId
                                            )
                                        : null
                                    }
                                    onChange={(e) => onChangeLocation(e)}
                                    //onBlur={() => setInputValue("")}
                                    isSearchable
                                    isClearable
                                    getOptionLabel={(option) =>
                                      option.value.description ?? ""
                                    }
                                    getOptionValue={(option) =>
                                      option["key"].toString()
                                    }
                                    formatOptionLabel={function (data) {
                                      return (
                                        <span
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              data.value.description ?? "",
                                          }}
                                        />
                                      );
                                    }}
                                  ></Select>
                                </div>
                              </div>
                              {validation &&
                              validation.response === false &&
                              validation.property?.includes("locationId") ? (
                                <label className="validation h-16">
                                  *location must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                          <div className="form-group col-3 pr-0">
                            <label className="labelForm voda-bold w-100">
                              Status
                              <input
                                type="text"
                                // onChange={(e) =>
                                //   onChangeNetworkElement(
                                //     "elementName",
                                //     e.target.value
                                //   )
                                // }
                                className="inputForm w-100"
                                value={"PLANNED"}
                                disabled
                                style={{ position: "relative", top: "-6px" }}
                              />
                            </label>
                          </div>
                          <div className="form-group col-12 px-0 d-flex justify-content-center">
                            <button
                              className="clearBtn"
                              onClick={() => clearFormNetworkElement()}
                              type="button"
                            >
                              Clear
                            </button>
                            <button
                              className={
                                `br-0 voda-bold btn btn-danger px-4 btnHeader`
                                //${
                                //checkAddedNetwork ? "spinner" : ""
                                //}`
                              }
                              // disabled={
                              //   validazioneNetworkElement() === false ||
                              //   props.formDisabed
                              // }
                              onClick={() => addNetworkElement()}
                              type="button"
                              style={{ position: "relative" }}
                            >
                              Add Element
                            </button>
                          </div>
                        </div>
                      </fieldset>
                      <fieldset className="fieldset p-0">
                        <legend className="voda-bold mb-4 fz-18">
                          Network Element Details
                        </legend>
                        <div className="row">
                          <div className="w-100">
                            <table className="w-80">
                              <thead>
                                <tr className="mt-4 head">
                                  <th className="pl-2 ptb-12">Element Name</th>
                                  <th className="pl-2 ptb-12">Environment</th>
                                  <th className="pl-2 ptb-12">Location</th>
                                  <th className="pl-2 ptb-12">Status</th>
                                </tr>
                              </thead>
                              <tbody>
                                {formData?.startNodesInLab &&
                                  formData?.startNodesInLab.map((item, i) => (
                                    <tr
                                      className={`dati ${
                                        numberIsNullOrZero(item.id) ? "" : ""
                                      }`}
                                      key={i}
                                    >
                                      <td>{item.elementName}</td>
                                      <td>{item.enviroment}</td>
                                      <td>{item.location}</td>
                                      <td>{item.assetsStatus}</td>
                                      {/* <td>
                                        {numberIsNullOrZero(item.id) ? (
                                          <img
                                            onClick={() =>
                                              removeNetworkElement(i)
                                            }
                                            className="btnEdit op-55"
                                            src={require("../../img/delete.png")}
                                          />
                                        ) : null}
                                      </td> */}
                                    </tr>
                                  ))}
                                {formData?.startNodesInProd &&
                                  formData?.startNodesInProd.map((item, i) => (
                                    <tr
                                      className={`dati ${
                                        numberIsNullOrZero(item.id) ? "" : ""
                                      }`}
                                      key={i}
                                    >
                                      <td>{item.elementName}</td>
                                      <td>{item.enviroment}</td>
                                      <td>{item.location}</td>
                                      <td>{item.assetsStatus}</td>
                                      {/* <td>
                                        {numberIsNullOrZero(item.id) ? (
                                          <img
                                            onClick={() =>
                                              removeNetworkElement(i)
                                            }
                                            className="btnEdit op-55"
                                            src={require("../../img/delete.png")}
                                          />
                                        ) : null}
                                      </td> */}
                                    </tr>
                                  ))}
                              </tbody>
                            </table>
                          </div>
                        </div>
                      </fieldset>
                    </div>
                  </Container>
                  <Container
                    show={formData?.ruleLinkedDc === 34 ? true : false}
                  >
                    <fieldset className="fieldset mx-0">
                      <div className="col-12 d-flex justify-content-between my-2 px-2">
                        <legend
                          className="voda-bold mb-0 pb-0"
                          style={{ fontSize: "20px", alignContent: "center" }}
                        >
                          Cluster Details
                        </legend>
                        <button
                          className={`voda-bold btn btn-danger px-4 btnHeader w-100`}
                          style={{ maxWidth: "10rem" }}
                          onClick={() => setModalClusterFlag(true)}
                          type="button"
                        >
                          Add New Cluster
                        </button>
                      </div>
                      <div className="mx-0 px-0 py-3 flex-row">
                        <table
                          className="table table-borderless table-responsive"
                          style={{ minHeight: "inherit" }}
                        >
                          <thead>
                            <tr className="intestazione">
                              <th key={"opCoId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Opco"}</label>
                                </div>
                              </th>
                              <th key={"locationId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Location"}</label>
                                </div>
                              </th>
                              <th key={"site"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Site"}</label>
                                </div>
                              </th>
                              <th key={"platformId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Platform"}</label>
                                </div>
                              </th>
                              <th key={"clustertypeId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Cluster Type"}</label>
                                </div>
                              </th>
                              <th key={"clusterName"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Cluster Name"}</label>
                                </div>
                              </th>
                              <th key={"hardwaretypeId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Hardware Type"}</label>
                                </div>
                              </th>
                              <th key={"deploymentStatusId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Deployment Status"}</label>
                                </div>
                              </th>
                              <th key={"verticalResponsibleId"}>
                                <div className="h-100 d-flex align-items-center divFilter">
                                  <label>{"Vertical Responsible"}</label>
                                </div>
                              </th>
                              <th className="  customWidth"></th>
                            </tr>
                          </thead>
                          <tbody>
                            {clusterData &&
                              clusterData?.map((item, index) => {
                                document
                                  .querySelectorAll<HTMLTableDataCellElement>(
                                    "tbody > tr > td"
                                  )
                                  .forEach((td) => {
                                    td.style.backgroundColor = "";
                                    td.style.left = "";
                                    td.style.position = "";
                                    td.style.zIndex = "";
                                    td.style.borderRight = "";
                                    td.style.boxShadow = "";
                                    td.classList.remove("table_tr_bg");
                                    td.classList.remove("table_tr_even_bg");
                                  });

                                return (
                                  <tr
                                    className="dati"
                                    key={`${item.infraClusterAsPlannedId}-${index}`}
                                  >
                                    <td>{item.opCoValue}</td>
                                    <td>{item.locationValue}</td>
                                    <td>{item.site}</td>
                                    <td>{item.platformValue}</td>
                                    <td>{item.clustertypeValue}</td>
                                    <td>{item.clusterName}</td>
                                    <td>{item.hardwaretypeValue}</td>
                                    <td>{item.deploymentStatusValue}</td>
                                    <td>{item.verticalResponsibleValue}</td>

                                    <td className="actions">
                                      <div className="d-flex flex-row">
                                        {numberIsNullOrZero(
                                          item.infraClusterAsPlannedId
                                        ) ? (
                                          <button
                                            type="button"
                                            title="Delete"
                                            className="btn btn-link"
                                            onClick={() =>
                                              setClusterData((prev) =>
                                                prev?.filter(
                                                  (_, idx) => index !== idx
                                                )
                                              )
                                            }
                                          >
                                            <MdDelete color={"black"} />
                                          </button>
                                        ) : null}
                                      </div>
                                    </td>
                                  </tr>
                                );
                              })}
                          </tbody>
                        </table>
                      </div>
                    </fieldset>
                  </Container>
                </>
              )}

              <div className="form-group row col-12 mt-2 pr-0">
                {/* <div className="col-12 text-bb">
                  Please select the migrated nodes to the new Design Component
                </div> */}
                <div className="col-3">
                  <label className="labelForm voda-bold w-100">
                    In Engineering Phase <span className="red">*</span>
                    <div className="w-100">
                      <Select
                        options={boolOptions}
                        value={
                          formData?.planningActivityDetailsResourceId ===
                          undefined
                            ? null
                            : formData?.inEngineeringPhase === true
                            ? boolOptions.find((x) => x.key === "YES")
                            : formData?.inEngineeringPhase === false
                            ? boolOptions.find((x) => x.key === "NO")
                            : null
                        }
                        onChange={(e) => onChangeInEngineering(e)}
                        isDisabled={
                          formData?.planningActivityDetailsResourceId ===
                          undefined
                        }
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value ?? ""}
                        getOptionValue={(option) => option["key"].toString()}
                      ></Select>
                    </div>
                  </label>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("inEngineeringPhase") ? (
                    <label className="validation">
                      *Engineering Phase must have a value
                    </label>
                  ) : null}
                </div>
                {formData?.ruleLinkedDc === 35 && (
                  <div className="col-md-6">
                    <div className="col-md-12 pl-0">
                      <DropdownInputComponent
                        label={"Infra Cluster"}
                        labelCSS="mb-0 text-left"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable
                        isClearable={false}
                        disabled={true}
                        required
                        value={
                          clusterDropdown &&
                          clusterDropdown.filter(
                            (x: any) =>
                              x.key ===
                              ((formData as any)?.clusetrId || selectedCluster)
                          )
                        }
                        options={clusterDropdown}
                        isError={
                          validation?.property?.includes("clusetrId") ?? false
                        }
                        error="Cluster must have a value."
                        onChange={(e: any) =>
                          handleChangeDropdown(
                            {
                              property: "clusetrId",
                              value: ["clusterName"],
                            },
                            e
                          )
                        }
                      />
                    </div>
                  </div>
                )}
                {formData?.ruleLinkedDc === 36 && (
                  <>
                    <div className="col-md-4">
                      <div className="col-md-12 pl-0">
                        <DropdownInputComponent
                          label={"Site"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable
                          isClearable={false}
                          disabled={true}
                          required
                          value={
                            siteDropdown &&
                            siteDropdown.filter(
                              (x: any) =>
                                x.key ===
                                ((formData as any)?.siteId || selectedSite)
                            )
                          }
                          options={siteDropdown}
                          isError={
                            validation?.property?.includes("siteId") ?? false
                          }
                          error="Site must have a value."
                          onChange={(e: any) =>
                            handleChangeDropdown(
                              {
                                property: "siteId",
                                value: ["siteValue"],
                              },
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                    <div className="col-md-4">
                      <div className="col-md-12 pr-0">
                        <DropdownInputComponent
                          label={"Hardware Type"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable
                          isClearable={false}
                          required
                          value={hardwareTypeOptions?.filter(
                            (x: any) =>
                              x.key ===
                              ((formData as any)
                                ?.infraClusterClusterUpgradeUpsertDto
                                ?.plannedHardwareTypeId ??
                                (formData as any)?.plannedHardwareTypeId)
                          )}
                          options={hardwareTypeOptions}
                          isError={
                            validation?.property?.includes(
                              "plannedHardwareTypeId"
                            ) ?? false
                          }
                          error="Hardware Type must have a value."
                          onChange={(e: any) =>
                            handleChangeDropdown(
                              {
                                property: "plannedHardwareTypeId",
                                value: ["hardwaretypeValue"],
                              },
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                  </>
                )}
                {formData?.ruleLinkedDc === 19 && (
                  <>
                    <div className={"col-auto d-flex"}>
                      <div
                        className="col-9 pr-0"
                        style={{
                          alignContent: "center",
                        }}
                      >
                        <div className="input-group">
                          <div className="input-group-prepend">
                            <span className="input-group-text bg-white border-end-0">
                              <FaSearch className="text-muted" />
                            </span>
                          </div>
                          <input
                            type="text"
                            className="form-control"
                            placeholder="Filter by New Asset Name..."
                            value={newAssetNameFilter || ""}
                            onChange={(e) =>
                              setNewAssetNameFilter(e.target.value)
                            }
                          />
                        </div>
                      </div>
                      <div
                        className="col-auto"
                        style={{
                          alignContent: "center",
                        }}
                      >
                        {/* <button
                          className="voda-bold btn btn-danger px-4 btnHeader"
                          type="button"
                          onClick={() => setAddAssetModal(true)}
                        >
                          Add Asset
                        </button> */}
                      </div>
                    </div>
                    <div className="col-12 mt-2 pr-0">
                      <table
                        className="w-100 table table-responsive"
                        style={{
                          minHeight: "25rem",
                          maxWidth: "70rem",
                        }}
                      >
                        <thead>
                          <tr className="intestazione">
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "12rem" }}
                              >
                                <label>
                                  Current Asset Name
                                  <span className="red">*</span>
                                </label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "14rem" }}
                              >
                                <label>New Asset Name</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "14rem" }}
                              >
                                <label>Deployment Status</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "16rem" }}
                              >
                                <label>Target Design Component</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>BOM Submitted</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>HW PO Raised</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>HW PO Arrived</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{
                                  width: "10rem",
                                  minWidth: "10rem",
                                  whiteSpace: "normal",
                                  wordBreak: "break-word",
                                  lineHeight: "1.2",
                                }}
                              >
                                <label>
                                  VEC infra ready /CNIS -Workload cluster config
                                </label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>RFA Date</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>RFO Date</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{
                                  width: "10rem",
                                  minWidth: "10rem",
                                  whiteSpace: "normal",
                                  wordBreak: "break-word",
                                  lineHeight: "1.2",
                                }}
                              >
                                <label>Start of app Integration</label>
                              </div>
                            </th>

                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>RFS Date</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "8rem" }}
                              >
                                <label>Migration Start</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ minWidth: "8rem" }}
                              >
                                <label>Migration Completion Date</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "12rem" }}
                              >
                                <label>Traffic Node Percentage</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "12rem" }}
                              >
                                <label>Location</label>
                              </div>
                            </th>
                            <th>
                              <div
                                className="h-100 d-flex align-items-center divFilter"
                                style={{ width: "14rem" }}
                              >
                                <label>Environment</label>
                              </div>
                            </th>
                          </tr>
                        </thead>

                        <tbody>
                          {assetMigrationData
                            ?.filter((item) => {
                              if (!newAssetNameFilter) return true;

                              const newAssetName = item.newelEmentName || "";
                              return newAssetName
                                .toLowerCase()
                                .includes(newAssetNameFilter.toLowerCase());
                            })
                            ?.map((item, index) => {
                              document
                                .querySelectorAll<HTMLTableDataCellElement>(
                                  "tbody > tr > td"
                                )
                                .forEach((td) => {
                                  td.style.backgroundColor = "";
                                  td.style.left = "";
                                  td.style.position = "";
                                  td.style.zIndex = "";
                                  td.style.borderRight = "";
                                  td.style.boxShadow = "";
                                  td.style.filter = "";
                                  td.classList.remove("table_tr_bg");
                                  td.classList.remove("table_tr_even_bg");
                                });
                              return (
                                <tr
                                  className="dati"
                                  key={index}
                                  style={{ background: "white" }}
                                >
                                  <td style={{ padding: "8px 10px" }}>
                                    <Select
                                      options={
                                        assetMigrationResponse?.existingAssetResource ??
                                        []
                                      }
                                      value={assetMigrationResponse?.existingAssetResource?.filter(
                                        (res) =>
                                          res.key ===
                                          item.networkElementAsPlannedId
                                      )}
                                      onChange={(e) =>
                                        handleInputChange(
                                          e,
                                          index,
                                          "oldAssetName"
                                        )
                                      }
                                      isSearchable
                                      isClearable={false}
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.value.toString()
                                      }
                                      isDisabled={true}
                                    />
                                  </td>
                                  <td style={{ padding: "8px 10px" }}>
                                    <div>
                                      <input
                                        className={`form-control ${
                                          isDuplicateNewAssetName(
                                            item.newelEmentName,
                                            index
                                          )
                                            ? "is-invalid"
                                            : ""
                                        }`}
                                        type="text"
                                        value={item.newelEmentName || ""}
                                        onChange={(e) =>
                                          handleInputChange(
                                            e?.target?.value,
                                            index,
                                            "newelEmentName"
                                          )
                                        }
                                      />
                                      {isDuplicateNewAssetName(
                                        item.newelEmentName,
                                        index
                                      ) && (
                                        <div className="text-danger small mt-1">
                                          This asset name already exists
                                        </div>
                                      )}
                                    </div>
                                  </td>
                                  <td style={{ padding: "8px 10px" }}>
                                    <Select
                                      options={
                                        dictionaryToArray(
                                          assetMigrationResponse?.deploymentStatusReosurce
                                        )?.map((res: any) => ({
                                          key: res?.key,
                                          value:
                                            res?.value
                                              ?.deploymentStatusDescription,
                                        })) ?? []
                                      }
                                      value={dictionaryToArray(
                                        assetMigrationResponse?.deploymentStatusReosurce
                                      )
                                        ?.map((res: any) => ({
                                          key: res?.key,
                                          value:
                                            res?.value
                                              ?.deploymentStatusDescription,
                                        }))
                                        ?.filter(
                                          (res) =>
                                            res.key ===
                                            item.newDeploymentStatusId
                                        )}
                                      onChange={(e) =>
                                        handleInputChange(
                                          e,
                                          index,
                                          "newDeploymentStatusId"
                                        )
                                      }
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.value.toString()
                                      }
                                      isDisabled={
                                        item.newelEmentName === "" ||
                                        item.newelEmentName === undefined ||
                                        item.newelEmentName === null
                                          ? true
                                          : false
                                      }
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <Select
                                      options={
                                        assetMigrationResponse?.targetDesignComponentResource ??
                                        []
                                      }
                                      value={assetMigrationResponse?.targetDesignComponentResource?.filter(
                                        (res) =>
                                          res.key ===
                                          item.targetDesignComponenetId
                                      )}
                                      onChange={(e) =>
                                        handleInputChange(
                                          e,
                                          index,
                                          "targetDesignComponenetId"
                                        )
                                      }
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.value.toString()
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
                                      isDisabled={
                                        (item.newelEmentName === "" ||
                                          item.newelEmentName === undefined ||
                                          item.newelEmentName === null) &&
                                        item.newDeploymentStatus?.toLowerCase() !==
                                          "in-service" &&
                                        item.newDeploymentStatus?.toLowerCase() !==
                                          "in commissioning"
                                          ? true
                                          : false
                                      }
                                    />
                                    {validationErrors[index]
                                      ?.newDeploymentStatusId && (
                                      <label
                                        className="validation"
                                        style={{ padding: 0, bottom: 0 }}
                                      >
                                        *Required Field
                                      </label>
                                    )}
                                    {(item?.targetDesignComponenetId === null ||
                                      item?.targetDesignComponenetId ===
                                        undefined) &&
                                      item?.newelEmentName !== null &&
                                      item?.newDeploymentStatus &&
                                      (item?.newDeploymentStatus?.toLowerCase() ===
                                        "in-service" ||
                                        item.newDeploymentStatus?.toLowerCase() ===
                                          "in commissioning") && (
                                        <label
                                          className="validation"
                                          style={{ padding: 0, bottom: 0 }}
                                        >
                                          *Required Field
                                        </label>
                                      )}
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.bomSubmittedDate
                                          ? new Date(item.bomSubmittedDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "bomSubmittedDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.hwPoRaisedDate
                                          ? new Date(item.hwPoRaisedDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "hwPoRaisedDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.hwPoArrivedDate
                                          ? new Date(item.hwPoArrivedDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "hwPoArrivedDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>
                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.vecDate
                                          ? new Date(item.vecDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "vecDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.rfaDate
                                          ? new Date(item.rfaDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "rfaDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.rfoDate
                                          ? new Date(item.rfoDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "rfoDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>
                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.startOfAppIntegration
                                          ? new Date(item.startOfAppIntegration)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "startOfAppIntegration"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.rfsDate
                                          ? new Date(item.rfsDate)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "rfsDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>
                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.migrationStart
                                          ? new Date(item.migrationStart)
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "migrationStart"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <DatePicker
                                      selected={
                                        item.migrationCompletionDate
                                          ? new Date(
                                              item.migrationCompletionDate
                                            )
                                          : null
                                      }
                                      onChange={(date) =>
                                        handleInputChange(
                                          date,
                                          index,
                                          "migrationCompletionDate"
                                        )
                                      }
                                      className="form-control"
                                      dateFormat="dd-MM-yyyy"
                                      placeholderText="Select Date"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <input
                                      className="form-control"
                                      type="number"
                                      value={item.trafficNodePercentage || ""}
                                      onChange={(e) =>
                                        handleInputChange(
                                          e?.target?.value,
                                          index,
                                          "trafficNodePercentage"
                                        )
                                      }
                                      min="0"
                                      max="100"
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <Select
                                      options={
                                        dictionaryToArray(
                                          assetMigrationResponse?.locationReosurce
                                        ) ?? []
                                      }
                                      value={dictionaryToArray(
                                        assetMigrationResponse?.locationReosurce
                                      )?.filter(
                                        (res) => res.key === item.locationId
                                      )}
                                      onChange={(e) =>
                                        handleInputChange(
                                          e,
                                          index,
                                          "locationId"
                                        )
                                      }
                                      isSearchable
                                      isClearable={false}
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.value.toString()
                                      }
                                      isDisabled={true}
                                    />
                                  </td>

                                  <td style={{ padding: "8px 10px" }}>
                                    <Select
                                      options={
                                        assetMigrationResponse?.environmentReosurce &&
                                        dictionaryToArray(
                                          assetMigrationResponse?.environmentReosurce
                                        ).sort((a, b) =>
                                          a.value.toLowerCase() <
                                          b.value.toLowerCase()
                                            ? -1
                                            : 1
                                        )
                                      }
                                      value={
                                        assetMigrationResponse?.environmentReosurce &&
                                        dictionaryToArray(
                                          assetMigrationResponse?.environmentReosurce
                                        ).filter(
                                          (x) => x.key === item.newEnvironmentId
                                        )
                                      }
                                      onChange={(e) =>
                                        handleInputChange(
                                          e,
                                          index,
                                          "newEnvironmentId"
                                        )
                                      }
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.value.toString()
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
                                      isDisabled={true}
                                    />
                                  </td>
                                </tr>
                              );
                            })}
                        </tbody>
                      </table>
                    </div>
                  </>
                )}

                {showLiveDate && (
                  <div className="col-6 pr-0">
                    <div className="col-md-12">
                      <label className="labelForm voda-bold w-100">
                        Asset Live Date
                        {isMandate && <span className="red">*</span>}
                        <DatePicker
                          selected={
                            formData?.assetLiveStatusDate &&
                            new Date(formData?.assetLiveStatusDate)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("assetLiveStatusDate", newDate);
                          }}
                          className={`inputForm w-100 mt-0`}
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                        {validation &&
                        isMandate &&
                        validation.response === false &&
                        validation.property?.includes("assetLiveStatusDate") ? (
                          <label className="validation">
                            *Asset Live Date must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                )}
                {showDecommissionedDate && (
                  <div className="col-6 pr-0">
                    <div className="col-md-12">
                      <label className="labelForm voda-bold w-100">
                        Asset Decommissioning Date
                        <span className="red">*</span>
                        <DatePicker
                          selected={
                            formData?.assetDecommissionedDate &&
                            new Date(formData?.assetDecommissionedDate)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("assetDecommissionedDate", newDate);
                          }}
                          className={`inputForm w-100 mt-0`}
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "assetDecommissionedDate"
                        ) ? (
                          <label className="validation">
                            *Asset Decommissioning Date must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                )}
                {dcDropFlag && (
                  <div className="col-6 pl-0">
                    <label className="labelForm w-100">
                      <label
                        className="labelForm voda-bold mb-0"
                        title={
                          isReleaseDcResource &&
                          dictionaryToArray(isReleaseDcResource)
                            .find((x) => x.key === formData?.designComponentId)
                            ?.value.replace('<b class="text-lowercase">', "")
                            .replace("</b>", "")
                        }
                      >
                        Select Design Component
                        <span className="red">*</span>
                      </label>
                      <Select
                        menuPosition={"fixed"}
                        options={
                          isReleaseDcResource &&
                          dictionaryToArray(isReleaseDcResource)
                        }
                        value={updateDC}
                        onChange={(e) => setUpdateDC(e)}
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
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
                      validation.property?.includes("selectDCID") ? (
                        <label className="validation h-16">
                          {isReleaseDcResource &&
                          dictionaryToArray(isReleaseDcResource).length > 0
                            ? "*Design Component must have a value."
                            : "*Kindly create a Design Component and select here."}
                        </label>
                      ) : null}
                    </label>
                  </div>
                )}
              </div>
              <Container show={formData?.ruleLinkedDc === 36 ? true : false}>
                <>
                  <fieldset className="fieldset mt-4 mx-3">
                    <div className="col-12 d-flex justify-content-between my-2 px-0">
                      <legend
                        className="voda-bold mb-0 pb-0"
                        style={{
                          fontSize: "20px",
                          alignContent: "center",
                        }}
                      >
                        Cluster Upgrade
                        <span className="red">*</span>
                      </legend>

                      {validation &&
                      validation.response === false &&
                      validation.property?.includes("isAtleastOneCluster") ? (
                        <label className="validation">
                          *Please add atleast one new cluster.
                        </label>
                      ) : null}
                    </div>

                    <div className="mx-0 px-0 py-3 flex-row">
                      <table
                        className="table table-borderless table-responsive"
                        style={{ minHeight: "inherit" }}
                      >
                        <thead>
                          <tr className="intestazione">
                            <th key={"opCoId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Opco"}</label>
                              </div>
                            </th>
                            <th key={"locationId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Location"}</label>
                              </div>
                            </th>
                            <th key={"site"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Site"}</label>
                              </div>
                            </th>
                            <th key={"platformId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Platform"}</label>
                              </div>
                            </th>
                            <th key={"clustertypeId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Cluster Type"}</label>
                              </div>
                            </th>
                            <th key={"clusterName"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Cluster Name"}</label>
                              </div>
                            </th>
                            <th key={"hardwaretypeId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Hardware Type"}</label>
                              </div>
                            </th>
                            <th key={"deploymentStatusId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Deployment Status"}</label>
                              </div>
                            </th>
                            <th key={"verticalResponsibleId"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Vertical Responsible"}</label>
                              </div>
                            </th>
                          </tr>
                        </thead>
                        <tbody>
                          {filteredClusters &&
                            filteredClusters?.map((item, index) => {
                              document
                                .querySelectorAll<HTMLTableDataCellElement>(
                                  "tbody > tr > td"
                                )
                                .forEach((td) => {
                                  td.style.backgroundColor = "";
                                  td.style.left = "";
                                  td.style.position = "";
                                  td.style.zIndex = "";
                                  td.style.borderRight = "";
                                  td.style.boxShadow = "";
                                  td.classList.remove("table_tr_bg");
                                  td.classList.remove("table_tr_even_bg");
                                });

                              return (
                                <tr
                                  className="dati"
                                  key={`${item.infraClusterAsPlannedId}-${index}`}
                                >
                                  <td>{item.opCoValue}</td>
                                  <td>{item.locationValue}</td>
                                  <td>{item.site}</td>
                                  <td>{item.platformValue}</td>
                                  <td>{item.clustertypeValue}</td>
                                  <td>{item.clusterName}</td>
                                  <td>{item.hardwaretypeValue}</td>
                                  <td>{item.deploymentStatusValue}</td>
                                  <td>{item.verticalResponsibleValue}</td>
                                </tr>
                              );
                            })}
                        </tbody>
                      </table>
                    </div>
                  </fieldset>
                </>
              </Container>
              <Container show={formData?.ruleLinkedDc === 35 ? true : false}>
                <fieldset className="fieldset mt-4 mx-3">
                  <div className="col-12 d-flex justify-content-between my-2 px-0">
                    <legend
                      className="voda-bold mb-0 pb-0"
                      style={{
                        fontSize: "20px",
                        alignContent: "center",
                      }}
                    >
                      App Cluster Details
                      <span className="red">*</span>
                    </legend>
                    {!isRemoveMode && (
                      <>
                        <button
                          className={`voda-bold btn btn-danger px-4 btnHeader w-100`}
                          style={{ maxWidth: "13rem" }}
                          onClick={() => setProgramClusterFlag(true)}
                          type="button"
                        >
                          Add App Cluster
                        </button>
                        <button
                          className="voda-bold btn btn-danger px-4 btnHeader w-100"
                          style={{ maxWidth: "13rem" }}
                          onClick={() => setIsRemoveMode(true)}
                          type="button"
                        >
                          Remove APP Cluster
                        </button>
                      </>
                    )}
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("isAtleastOneCluster") ? (
                      <label className="validation">
                        *Please add atleast one new cluster.
                      </label>
                    ) : null}
                  </div>

                  <div className="mx-0 px-0 py-3 flex-row">
                    <table
                      className="table table-borderless table-responsive"
                      style={{ minHeight: "inherit" }}
                    >
                      <thead>
                        <tr className="intestazione">
                          <th key={"appClusterName"}>
                            <div className="h-100 d-flex align-items-center divFilter">
                              <label>{"App Cluster Name"}</label>
                            </div>
                          </th>
                          <th key={"applicationName"}>
                            <div className="h-100 d-flex align-items-center divFilter">
                              <label>{"Application Name"}</label>
                            </div>
                          </th>
                          <th key={"deploymentStatusId"}>
                            <div className="h-100 d-flex align-items-center divFilter">
                              <label>{"Deployment Status"}</label>
                            </div>
                          </th>
                          {isRemoveMode && (
                            <th key={"Select"}>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>{"Select"}</label>
                              </div>
                            </th>
                          )}
                        </tr>
                      </thead>
                      <tbody>
                        {filteredProgram &&
                          filteredProgram?.map((item, index) => {
                            document
                              .querySelectorAll<HTMLTableDataCellElement>(
                                "tbody > tr > td"
                              )
                              .forEach((td) => {
                                td.style.backgroundColor = "";
                                td.style.left = "";
                                td.style.position = "";
                                td.style.zIndex = "";
                                td.style.borderRight = "";
                                td.style.boxShadow = "";
                                td.classList.remove("table_tr_bg");
                                td.classList.remove("table_tr_even_bg");
                              });

                            return (
                              <tr
                                className="dati"
                                key={`${item.infraClusterAsPlannedId}-${index}`}
                              >
                                <td>{item.appClusterName}</td>
                                <td>{item.applicationName}</td>

                                <td>{item.deploymentStatusValue}</td>

                                <td className="actions">
                                  <div className="d-flex flex-row">
                                    {item.isNew ? (
                                      <button
                                        type="button"
                                        title="Delete"
                                        className="btn btn-link"
                                        onClick={() =>
                                          setClusterProgramData((prev) =>
                                            prev?.filter(
                                              (_, idx) => idx !== index
                                            )
                                          )
                                        }
                                      >
                                        <MdDelete color={"black"} />
                                      </button>
                                    ) : isRemoveMode &&
                                      isInService(
                                        item.deploymentStatusValue
                                      ) ? (
                                      <input
                                        type="checkbox"
                                        checked={item.selected || false}
                                        onChange={(e) => {
                                          const checked = e.target.checked;
                                          setClusterProgramData((prev) =>
                                            prev?.map((itm, idx) =>
                                              idx === index
                                                ? {
                                                    ...itm,
                                                    selected: checked,
                                                  }
                                                : itm
                                            )
                                          );
                                        }}
                                      />
                                    ) : null}
                                  </div>
                                </td>
                              </tr>
                            );
                          })}
                      </tbody>
                    </table>
                  </div>
                  {isRemoveMode && (
                    <div className="d-flex justify-content-end gap-3 mt-3">
                      <button
                        type="button"
                        className="voda-bold btn px-4 btnHeader btn-link cancel"
                        onClick={() => {
                          setIsRemoveMode(false);

                          setClusterProgramData((prev) =>
                            prev?.map((item) => ({
                              ...item,
                              selected: false,
                            }))
                          );
                        }}
                      >
                        Cancel
                      </button>

                      <button
                        type="button"
                        className="voda-bold btn btn-danger ml-3 px-4 btnHeader"
                        onClick={() => {
                          if (!trafficFreeOption) return;

                          setClusterProgramData((prev) =>
                            prev
                              ?.map((item) => {
                                if (item.selected) {
                                  if (!item.isNew) {
                                    if (
                                      isInService(item.deploymentStatusValue)
                                    ) {
                                      return {
                                        ...item,
                                        deploymentStatusId:
                                          trafficFreeOption.key,
                                        deploymentStatusValue:
                                          trafficFreeOption.value,
                                        selected: false,
                                      };
                                    }
                                    return item;
                                  } else {
                                    return null;
                                  }
                                }
                                return item;
                              })
                              .filter(Boolean)
                          );

                          setIsRemoveMode(false);
                        }}
                      >
                        Confirm
                      </button>
                    </div>
                  )}
                </fieldset>
              </Container>
              {isDAPA &&
                isLatestSelected &&
                dcfTableData.length > 0 &&
                formData?.ruleLinkedDc === 17 && (
                  <div className="form-group row col-12 mt-2">
                    <label className="text-bb" style={{ borderBottom: "0px" }}>
                      Please select the Design Component Family
                      <span className="red">*</span>
                    </label>
                    <table className="table-responsive" tabIndex={-1}>
                      <thead>
                        <tr className="intestazione">
                          <th
                            className="customVolteKPIHead text-left pl-2"
                            style={{
                              fontSize: "13px",
                              padding: "0 20px",
                              verticalAlign: "middle",
                            }}
                          >
                            <div className="h-100 d-flex align-items-center divFilter">
                              <FormControlLabel
                                control={
                                  <Checkbox
                                    checked={
                                      dcfTableData.length > 0 &&
                                      selectedDcfIds.length ===
                                        dcfTableData.length
                                    }
                                    indeterminate={
                                      selectedDcfIds.length > 0 &&
                                      selectedDcfIds.length <
                                        dcfTableData.length
                                    }
                                    onChange={handleSelectAllChange}
                                  />
                                }
                                label="" // no label in header
                              />
                            </div>
                          </th>
                          <th
                            className="customVolteKPIHead text-left pl-2"
                            style={{ fontSize: "13px", padding: "0 20px" }}
                          >
                            <div className="h-100 d-flex align-items-center divFilter">
                              <span>Design Component Family</span>
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
                        {dcfTableData.map((item) => (
                          <tr
                            className="dati"
                            key={item.daPlannedActivityDcfId}
                          >
                            <td>
                              <FormControlLabel
                                control={
                                  <Checkbox
                                    value={item.designComponentfamilyId}
                                    checked={selectedDcfIds.includes(
                                      item.designComponentfamilyId
                                    )}
                                    onChange={() =>
                                      handleDcfCheckboxChange(
                                        item.designComponentfamilyId
                                      )
                                    }
                                    disabled={item.dcfStatus === "Completed"}
                                  />
                                }
                                label=""
                              />
                            </td>
                            <td>
                              <div
                                dangerouslySetInnerHTML={{
                                  __html: item.designComponentFamilyName ?? "",
                                }}
                              />
                            </td>
                            <td style={{ paddingLeft: "20px" }}>
                              {item.dcfStatus}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("designComponentfamilyId") ? (
                      <label className="validation">
                        *Please select at least one Design Component Family
                      </label>
                    ) : null}
                  </div>
                )}

              {isDAPA &&
                showTableForSecondAndThird &&
                formData?.ruleLinkedDc === 18 &&
                daMigrationData.length > 0 && (
                  <>
                    {/* === Dropdown Form Section (New) === */}
                    <div className="col-12 pl-0">
                      <fieldset className="fieldset mt-4">
                        <legend className="voda-bold mb-4 fz-18">
                          DA Migration Entry
                        </legend>

                        <div className="row">
                          {/* Location Dropdown */}

                          <div className="form-group col-3 pr-0">
                            <label className="labelForm voda-bold   w-100">
                              Location
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition="fixed"
                                    options={locationOptions}
                                    value={selectedLocation}
                                    onChange={(selected) =>
                                      setSelectedLocation(selected)
                                    }
                                    onBlur={() => {}}
                                    isSearchable
                                    isClearable
                                    isDisabled={false}
                                    getOptionLabel={(option) =>
                                      option.value ?? ""
                                    }
                                    getOptionValue={(option) =>
                                      option.key.toString()
                                    }
                                    formatOptionLabel={(data) => (
                                      <span
                                        dangerouslySetInnerHTML={{
                                          __html: data.value ?? "",
                                        }}
                                      />
                                    )}
                                  />
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
                              {validation?.property?.includes(
                                "duplicateEntry"
                              ) && (
                                <label className="validation">
                                  *This location has already been added.
                                </label>
                              )}
                            </label>
                          </div>

                          {/* Status Dropdown */}
                          <div className="form-group col-3 pl-0 pr-0">
                            <label className="labelForm voda-bold w-100">
                              Status <span className="red">*</span>
                              <Select
                                menuPosition="fixed"
                                options={statusOptions}
                                value={selectedStatus}
                                onChange={(selected) =>
                                  setSelectedStatus(selected)
                                }
                                isSearchable
                                isClearable
                                isDisabled={false}
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option.key.toString()
                                }
                              />
                            </label>
                          </div>

                          {/* Buttons */}
                          <div className="form-group col-12 px-0 d-flex justify-content-center">
                            <button
                              className="clearBtn"
                              onClick={() => {}}
                              type="button"
                              disabled={false}
                            >
                              Clear
                            </button>
                            <button
                              className={`br-0 voda-bold btn btn-danger px-4 btnHeader`}
                              onClick={handleAddEntry}
                              type="button"
                              disabled={false}
                            >
                              Add Entry
                            </button>
                          </div>
                        </div>
                      </fieldset>
                    </div>

                    {daMigrationData.length > 0 && (
                      <div className="col-12 mt-3 mb-4">
                        <label
                          className="text-bb"
                          style={{ borderBottom: "0px" }}
                        >
                          Please Select Status <span className="red">*</span>
                        </label>

                        <table
                          className="table-responsive"
                          style={{
                            position: "relative",
                            maxHeight: "25rem !important",
                            justifyItems: "center",
                          }}
                        >
                          <thead>
                            <tr className="intestazione">
                              <th
                                className="customVolteKPIHead text-left pl-2"
                                style={{ fontSize: "13px", padding: "0 20px" }}
                              >
                                Location
                              </th>
                              <th
                                className="customVolteKPIHead text-left pl-2"
                                style={{ fontSize: "13px", padding: "0 20px" }}
                              >
                                Status
                              </th>
                              <th
                                className="customVolteKPIHead text-left pl-2"
                                style={{ fontSize: "13px", padding: "0 20px" }}
                              >
                                Action
                              </th>
                            </tr>
                          </thead>
                          <tbody>
                            {daMigrationData.map((item, index) => {
                              const currentStatusOption =
                                statusOptions.find(
                                  (opt) => opt.key === item.statusId
                                ) || null;
                              const found = editStatusId.find(
                                (obj) => obj.id === item?.daMigrationStatusId
                              );
                              const isCompleted =
                                isEdit && found && found.status === "Completed"
                                  ? true
                                  : false;
                              return (
                                <tr
                                  className="dati"
                                  key={item.daMigrationStatusId}
                                >
                                  <td
                                    style={{
                                      paddingLeft: "20px",
                                      minWidth: 200,
                                    }}
                                  >
                                    {item.location}
                                  </td>
                                  <td
                                    style={{
                                      paddingLeft: "20px",
                                      minWidth: 150,
                                    }}
                                  >
                                    <Select
                                      options={statusOptions}
                                      value={currentStatusOption}
                                      onChange={(selected) =>
                                        onChangeStatus(
                                          selected,
                                          item.daMigrationStatusId
                                        )
                                      }
                                      isDisabled={isCompleted}
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option.key.toString()
                                      }
                                      isClearable={false}
                                      isSearchable
                                      menuPortalTarget={document.body}
                                      menuShouldScrollIntoView={false}
                                      styles={{
                                        menuPortal: (base) => ({
                                          ...base,
                                          zIndex: 9999,
                                        }),
                                        control: (base, state) => ({
                                          ...base,
                                          backgroundColor: state.isDisabled
                                            ? "#f5f5f5"
                                            : "white",
                                          cursor: state.isDisabled
                                            ? "not-allowed"
                                            : "default",
                                        }),
                                      }}
                                    />
                                  </td>
                                  <td>
                                    <img
                                      onClick={() => handleDeleteEntry(index)}
                                      className="btnEdit op-55"
                                      src={require("../../img/delete.png")}
                                    />
                                  </td>
                                </tr>
                              );
                            })}
                          </tbody>
                        </table>
                      </div>
                    )}
                  </>
                )}
            </div>
            <div className="col-12 justify-content-end d-flex mb-3">
              <button
                className="  voda-bold btn btn-link px-4 btnHeader cancel"
                onClick={() => {
                  props.action.setIsVisibleModalStatus(false);
                }}
                type="button"
              >
                Cancel
              </button>
              <button
                className="  voda-bold btn btn-danger px-4 btnHeader"
                type="button"
                disabled={
                  Object.keys(validateRows(assetMigrationData))?.length !== 0 ||
                  (needPlannedAsset &&
                    !isPlannedAssetAvailable &&
                    !enableSubmit)
                }
                onClick={() => handleConfirmDialog()}
              >
                Submit
              </button>
            </div>
            {isDAPA && (
              <div>
                <Dialog
                  open={isResponseDialogOpen}
                  onClose={() => setIsResponseDialogOpen(false)}
                  maxWidth="sm"
                  fullWidth
                >
                  <DialogTitle
                    sx={{
                      textAlign: "center",
                    }}
                  >
                    {isRelatedRecord && isRelatedRecord.length > 0 && (
                      <div>
                        <strong>Below are the open planned activities.</strong>
                      </div>
                    )}
                    <div>Are you sure you want to proceed?</div>
                  </DialogTitle>

                  <DialogContent>
                    <div className="mx-0 col-12 p-0 justify-content-center">
                      <div
                        className="mx-0 px-0 flex-row"
                        style={{ position: "relative" }}
                      >
                        {isRelatedRecord && isRelatedRecord.length > 0 && (
                          <table className="table-responsive" tabIndex={-1}>
                            <thead>
                              <tr className="intestazione">
                                {/* <th
          className="customVolteKPIHead text-left pl-2"
          style={{ fontSize: "13px", padding: "0 20px" }}
        >
          <div className="h-100 d-flex align-items-center divFilter">
                    <span>Opco</span>
          </div>
                </th> */}
                                <th
                                  className="customVolteKPIHead text-left pl-2"
                                  style={{
                                    fontSize: "13px",
                                    padding: "0 20px",
                                    width: "260px",
                                    maxWidth: "260px",
                                  }}
                                >
                                  <div className="h-100 d-flex align-items-center divFilter">
                                    <span>Design Component</span>
                                  </div>
                                </th>
                              </tr>
                            </thead>

                            <tbody>
                              {isRelatedRecord.map((item, index) => (
                                <tr className="dati" key={index}>
                                  {/* <td>{item.opcoName}</td> */}
                                  <td>
                                    <a
                                      href="#"
                                      onClick={(e) => {
                                        e.preventDefault();
                                        handleOpenInNewTab(item);
                                      }}
                                      style={{
                                        cursor: "pointer",
                                        color: "#007bff",
                                        textDecoration: "underline",
                                      }}
                                      dangerouslySetInnerHTML={{
                                        __html: item.designComponentName,
                                      }}
                                    />
                                  </td>
                                </tr>
                              ))}
                            </tbody>
                          </table>
                        )}
                      </div>
                    </div>
                  </DialogContent>
                  <DialogActions>
                    <div className="col-12 justify-content-end d-flex mb-3">
                      <button
                        className="  voda-bold btn btn-link px-4 btnHeader cancel"
                        onClick={() => setIsResponseDialogOpen(false)}
                        type="button"
                      >
                        No
                      </button>
                      <button
                        className="  voda-bold btn btn-danger px-4 btnHeader"
                        type="button"
                        onClick={() => {
                          Save();
                          setIsResponseDialogOpen(false); // close dialog after save
                        }}
                      >
                        Proceed
                      </button>
                    </div>
                  </DialogActions>
                </Dialog>
              </div>
            )}
          </div>
        </div>
      </form>
    </>
  );
};

export default UpdatePlannedActivityStatusModal;
