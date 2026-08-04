import { SetStateAction, forwardRef, useEffect, useState } from "react";
import { Form, Modal } from "react-bootstrap";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useSelector } from "react-redux";
import Select from "react-select";
import Container from "../../Components/Container";
import ModalConfirm from "../../Components/ModalConfirm";
import TH from "../../Components/TableCrud/TableCrudTH";
import ActivityStatusContainer from "../../Containers/Lookup/ActivityStatusContainer";
import Benefits from "../../Containers/Lookup/BenefitsContainer";
import BudgetAvailabilityContainer from "../../Containers/Lookup/BudgetAvailabilityContainer";
import DeliveryStatusContainer from "../../Containers/Lookup/DeliveryStatusContainer";
import Driver from "../../Containers/Lookup/DriverContainer";
import Program from "../../Containers/Lookup/ProgramContainer";
import OperationalRiskContainer from "../../Containers/Lookup/OperationalRiskContainer";
import PlannedActivityResourceContainer from "../../Containers/Lookup/PlannedActivityResourceContainer";
import PlanningActivityStatusContainer from "../../Containers/Lookup/PlanningActivityStatusContainer";
import PlanningRisk from "../../Containers/Lookup/PlanningRiskContainer";
import ResponsibilityPhaseContainer from "../../Containers/Lookup/ResponsibilityPhaseContainer";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  boolOptions,
  colourStyles,
  getCustomStyles,
  convertDateToString,
  currencyOption,
  formatDateWithTime,
  formatTimeLocal,
  numberIsNullOrZero,
  optionValue,
  stringIsNullOrEmpty,
} from "../../Hook/Common";
import {
  dictionaryToArray,
  dictionaryToArrayLinkedLCMPLannedActivities,
  dictionaryToArrayPlannedActivityResourceDto,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import {
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  QueryObjectGrid,
  stateConfirm,
} from "../../Model/Common";
import { DeploymentStatusDto } from "../../Model/LookUp/DeploymentStatus";
import { PlannedActivityResourceDto } from "../../Model/LookUp/PlannedActivityResource";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityQueryObjectGrid,
  PlannedActivityToConnectData,
} from "../../Model/PlannedActivity";
import { setNotification } from "../../Redux/Action/NotificationAction";
import {
  ActivityStatusLogics,
  GetActivityDetails,
  GetConfrontoHardwareType,
  GetLcmEngineeringPlannedActivity,
  GetLinkedDesignComponent,
  GetCreateUnkownDCPALevel,
  GetPlannedActivityRelatedToDeliveryStatus,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import {
  CreatPlannedActivity,
  GetPlannedActivityCreateResource,
  GetPlannedActivityCreateResourceRefill,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { EditPlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import { GetFilterColumPlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import ManageMigration from "../PlannedActivities/ManageMigrationModal";
import UpdatePlannedActivityStatusModal from "../PlannedActivities/UpdatePlannedActivityStatusModal";
import { useNavigate, useLocation } from "react-router-dom";
import React from "react";
import { NetworkElementAssociated } from "../../Model/LcmEngineering";
import { BiCopy } from "react-icons/bi";
import { MdDelete, MdEdit } from "react-icons/md";
import { IoIosRefresh } from "react-icons/io";
import { LuFolderSync } from "react-icons/lu";
import { useTheme } from "../../Context/ThemeContext";
import moment from "moment";
import setLoader from "../../Redux/Action/LoaderAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions, DialogProps } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import {
  DropdownInputComponent,
  TextInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import BuildBagItemComponent from "../../Containers/BuildBagComponent";
import { FaRegEye } from "react-icons/fa";
import ViewMappedComponent from "../../Containers/ViewMappedComponent";
import { GetNetworkElementOpCo } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCommonAction";
import {
  InfraClusterDtoUpdate,
  InfraClusterQueryDto,
} from "../../Model/InfraCluster";
import ClusterModal from "./ClusterModal";
import {
  InfraClusterHardwareResource,
  InfraClusterPaHardwareLevelGet,
  InfraClusterPaLevelGet,
  InfraClusterPaProgramLevelGet,
  InfraClusterResource,
  InfraProgramClusterResource,
} from "../../Redux/Action/LcmEngineering/InfraClusterCreateAction";
import ClusterProgram from "./ClusterProgram";

interface Props {
  action: {
    Delete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    New(): any;
    setChanged(value: boolean): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    AddOnList(obj: PlannedActivityDtoUpdate[]): any;
    GetElementFromList(index: number): PlannedActivityDtoUpdate | undefined;
    setShowForm(val: boolean);
    changeNumberOfNodes?(
      newNodesValueProd: number,
      newNodesValueLab?: number
    ): any;
    updateProdNodes?(obj: NetworkElementAssociated[]): any;
    changeNumberOfNodesLab?(newNumberOfNodes: number): any;
    setConfirm(data: DataModalConfirm): any;
    editLcm?(id: number): any;
    closeModal?(changed: boolean): void;
  };
  formArray: PlannedActivityDtoUpdate[] | undefined;
  pagination: PlannedActivityQueryObjectGrid | undefined;
  networkElementAssociateds?: NetworkElementAssociated[] | undefined;
  // savePlan():any;
  principalId: number;
  lcmDeploymentStatusString?: string;
  both?: boolean;
  onSoftware?: boolean;
  onHardware?: boolean;
  idDetail: number | string | undefined | null;
  formDisabed?: boolean;
  isReleaseDetailUnKnown?: boolean;
  opco: string | undefined;
  opcoId?: number | undefined;
  designComponent?: { key: number | undefined; value: string } | undefined;
  designComponentFamily?: string;
  designComponentFamilyId?: number | undefined;
  productImportance: { key: number | undefined; value: string } | undefined;
  eduSpoc: { key: number | undefined; value: string }[] | undefined;
  subdomainSpoc: { key: number | undefined; value: string }[] | undefined;
  numberOfNodesInProd: number;
  numberOfNodesInLab: number;
  showForm: boolean;
  edit: boolean;
  changedLcm?: boolean;
  lcmId?: number;
  buildBagIds?: number | undefined;
  softwareManufacturer?: { key: number; value: string } | undefined;
  selectedDeploymentStatus?: DeploymentStatusDto;
  isFromNetworkElement?: boolean;
  location?: string;
  elementDeploymentName?: string;
  plannedActivityAllowed?: number[];
  buildBagResources?: any;
  isDesignAspect?: boolean;
  isInLcm?: boolean;
  isForAddAsset?: boolean;
  isForEditAsset?: boolean;
  plannedActivityTypeForEnum?: PlannedActivityTypeForEnum;
  LcmEngineervalues?: any;
  lcmRedirect?: boolean;
  prevPage?: string;
  commentOnProjectStatus?: string;
  reasonForNoPlan?: string;
}

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
// Utility function to parse date strings into Date objects
const parseDate = (dateStr) => {
  const [day, month, year] = dateStr?.split("/").map(Number);
  return new Date(year, month - 1, day);
};

// Function to compare multiple date ranges
const isDateRangeOverlapping = (ranges, newRange) => {
  const newStart = parseDate(newRange.start);
  const newEnd = parseDate(newRange.end);

  return ranges.some((range) => {
    const rangeStart = parseDate(range.start);
    const rangeEnd = parseDate(range.end);
    return newStart > rangeEnd && newEnd > rangeEnd ? false : true;
  });
};

const LcmEngPlannedActivity: React.FC<Props> = (props) => {
  const [isVisibleFiltri, setIsVisibleFiltri] = useState("");
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isBuildBagItemFlag, setIsBuildBagItemFlag] = useState<boolean>(false);
  const [viewBagFlag, setViewBagFlag] = useState<boolean>(false);
  const [modalClusterFlag, setModalClusterFlag] = useState<boolean>(false);
  const [programClusterFlag, setProgramClusterFlag] = useState<boolean>(false);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);
  const navigate = useNavigate();
  const location: any = useLocation();
  const [activityDetailsJson, setActivityDetailsJson] = useState<
    string | undefined
  >();

  const [showfurther, setShowFurther] = useState<boolean>(false);
  const [isDateRange, setIsDateRange] = useState<boolean>(false);
  const [isJson, setIsJson] = useState<boolean>(false);
  const [activityDetailsManual, setActivityDetailsManual] =
    useState<boolean>(false);
  const [resourceDesignComponent, setResourceDesignComponent] = useState<
    { key: number; value: string; color?: string | "#000000" }[] | undefined
  >([]);
  const [unknownDC, setUnknownDC] = useState<
    { key: number; value: string }[] | undefined
  >([]);
  const [transientDC, setTransientDC] = useState<any>();
  const [clusterDataResource, setClusterDataResource] = useState<any>();
  const [clusterData, setClusterData] = useState<any>();
  const [clusterUpgardeData, setClusterUpgradeData] = useState<any>();
  const [clusterProgramData, setClusterProgramData] = useState<any>();
  const [isConsigliati, setIsConsigliati] = useState<boolean>(false);
  const [startEndFlag, setStartEndFlag] = useState<boolean>(false);
  const { tipologicaPermesso } = useAuth();
  const [newNetworkAssocitate, setNewNetworkAssocitate] =
    useState<NetworkElementAssociated[]>();
  const [index, setIndex] = useState<number | undefined>(undefined);
  const [buildBagRes, setBuildBagRes] = useState<
    Array<{ key: number; text: string; isColour: boolean }>
  >([]);
  const [trafficFreeOption, setTrafficFreeOption] = useState<{
    key: number;
    value: string;
  }>();
  const [isRemoveMode, setIsRemoveMode] = useState(false);
  const getFiltersData = (state: RootState) =>
    state.plannedActivityGridReducer.filter;
  let filterData = useSelector(getFiltersData);

  const { resetFilter, checkFilterinValue } =
    useFilterTableCrud<PlannedActivityQueryObjectGrid>(
      props.action.Filter,
      GetFilterColumPlannedActivity,
      props.pagination
    );

  const {
    formData,
    setFormData,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    setChanged,
    setInputValue,
  } = useFormTableCrud<PlannedActivityDtoUpdate>(
    CreatPlannedActivity,
    EditPlannedActivity
  );

  const dtoNewResourceState = (state: RootState) =>
    state.plannedActivityCreateReducer.PlannedActivityDtoCreate;
  let createResource = useSelector(dtoNewResourceState);

  const dtoNewUpgradeResourceState = (state: RootState) =>
    state.infraClusterUpgradeReducer.InfraClusterDtoCreate;

  let createUpgradeResource = useSelector(dtoNewUpgradeResourceState);

  const programState = (state: RootState) =>
    state.infraProgramClusterResource?.InfraClusterDtoCreate;
  const programResource = useSelector(programState);

  const [plannedFormArray, setPlannedFormArray] = useState(
    props.formArray ?? []
  );
  let reasonForNoPlan = [
    { key: 0, value: "Budget dependencies" },
    { key: 1, value: "Network dependencies" },
    { key: 2, value: "Business dependencies" },
    { key: 3, value: "Stable platforms" },
  ];

  const [edit, setEdit] = useState<boolean>(false);
  const [isNoPA, setIsNoPA] = useState<boolean>(false);
  const [noPAFormData, setNoPAFormData] =
    useState<PlannedActivityDtoUpdate | null>();
  const [disabledInitialFunds, setDisabledInitialFunds] = useState<any>(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [DeliveryStatusArr, setDeliveryStatusArr] = useState<any>();
  const [ruleDCIdFlag, setRuleDCIdFlag] = useState<boolean>(false);
  const [customDateFormat, setCustomDateFormat] = useState<any>(null);
  const [lcmDeploymentStatus, setLcmDeploymentStatus] = useState<{
    [key: string]: string;
  }>({});
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [siteDropdown, setSiteDropdown] = useState<any[]>([]);
  const [clusterDropdown, setClusterDropdown] = useState<any[]>([]);
  const [siteOptions, setSiteOptions] = useState<any>([]);
  const [hardwareTypeOptions, setHardwareTypeOptions] = useState<any>([]);
  const [selectedSite, setSelectedSite] = useState<string | null>(null);
  const [selectedCluster, setSelectedCluster] = useState<string | null>(null);

  const thAction = {
    checkFilter: checkFilterinValue,
    settingVisibility: setIsVisibleFiltri,
    resetFilter: resetFilter,
  };

  const onCloseLCMModal = () => {
    //debugger;
    if (props.action?.closeModal) {
      props.action.closeModal(false);
    }
  };

  //VALIDAZIONE-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const validazioneClient = (copy: PlannedActivityDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };
    if (
      newNetworkAssocitate !== undefined &&
      newNetworkAssocitate?.length > 0 &&
      (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
        "in-commissioning" ||
        props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned")
    ) {
      const hasFinalAsset = newNetworkAssocitate.every(
        (item) => item.isFinalAsset === false
      );
      hasFinalAsset && addInvalidProperty("hasFinalAsset");
    }
    if (
      plannedResourceSelected?.ruleLinkedDc === 34 &&
      copy?.plannedActivityId === undefined &&
      !clusterData?.some((item) => item.infraClusterAsPlannedId === 0)
    ) {
      addInvalidProperty("isAtleastOneCluster");
    }

    if (
      copy?.plannedActivityResourceId == null ||
      copy?.plannedActivityResourceId === undefined ||
      copy?.plannedActivityResourceId === 0
    ) {
      addInvalidProperty("plannedActivityResourceId");
    }
    if (isNoPA) {
      if (
        copy?.reasonForNoPlan == null ||
        copy?.reasonForNoPlan === undefined ||
        copy?.reasonForNoPlan === ""
      ) {
        addInvalidProperty("reasonForNoPlan");
      }
      if (
        copy?.commentOnProjectStatus == null ||
        copy?.commentOnProjectStatus === undefined ||
        copy?.commentOnProjectStatus === ""
      ) {
        addInvalidProperty("commentOnProjectStatus");
      }
    } else {
      if (
        props.lcmId &&
        (copy?.activityDetails == null ||
          copy?.activityDetails === undefined ||
          copy?.activityDetails === "" ||
          copy?.activityDetails === "{}")
      ) {
        addInvalidProperty("activityDetails");
      }
      if (
        copy?.plannedCompletion == null ||
        copy?.plannedCompletion == undefined
      ) {
        addInvalidProperty("plannedCompletion");
      }
      if (
        copy?.planningRiskId == null ||
        copy?.planningRiskId === undefined ||
        copy?.planningRiskId === 0
      ) {
        addInvalidProperty("planningRisk");
      }
      if (
        copy?.plannedImplementationYear == null ||
        copy?.plannedImplementationYear === undefined ||
        copy?.plannedImplementationYear.toString().length === 0
      ) {
        addInvalidProperty("plannedImplementationYear");
      }
      if (
        copy?.planningActivityStatusId == null ||
        copy?.planningActivityStatusId === undefined ||
        copy?.planningActivityStatusId === 0
      ) {
        addInvalidProperty("planningActivityStatusId");
      }

      if (
        props?.lcmId &&
        (copy?.buildBagId == null ||
          copy?.buildBagId === undefined ||
          copy?.buildBagId === 0)
      ) {
        addInvalidProperty("buildBagId");
      }

      if (
        props?.lcmId &&
        isConsigliati &&
        !isPADisabled &&
        numberIsNullOrZero(copy.designComponentId) &&
        copy.plannedActivityResource &&
        copy.plannedActivityResourceId &&
        dictionaryToArrayPlannedActivityResourceDto(
          copy.plannedActivityResource
        ).find((y) => y.key == copy.plannedActivityResourceId)?.value
          .exportable == true
      ) {
        addInvalidProperty("designComponentId");
      }
      if (
        copy?.budgetAvailabilityId == null ||
        copy?.budgetAvailabilityId == undefined ||
        copy?.budgetAvailabilityId === 0
      ) {
        addInvalidProperty("budgetAvailabilityId");
      }
      if (
        copy?.localApproval === null ||
        copy?.localApproval === undefined ||
        copy?.localApproval === ""
      ) {
        addInvalidProperty("localApproval");
      }
      if (
        copy?.budgetValue != undefined &&
        copy?.budgetValue != 0 &&
        (formData?.currency == "" || formData?.currency == undefined)
      ) {
        addInvalidProperty("currency");
      }
      if (
        copy.localApproval === "YES" &&
        (copy?.deliveryProjectName === null ||
          copy?.deliveryProjectName === undefined ||
          copy?.deliveryProjectName === "")
      ) {
        addInvalidProperty("deliveryProjectName");
      }
      if (
        copy?.responsibilityPhaseId === null ||
        copy?.responsibilityPhaseId === undefined ||
        copy?.responsibilityPhaseId === 0
      ) {
        addInvalidProperty("responsibilityPhaseId");
      }
      if (
        copy?.deliveryStatusId === null ||
        copy?.deliveryStatusId === undefined ||
        copy?.deliveryStatusId === 0
      ) {
        addInvalidProperty("deliveryStatusId");
      }
      if (
        copy?.activityStatusId === null ||
        copy?.activityStatusId === undefined ||
        copy?.activityStatusId === 0
      ) {
        addInvalidProperty("activityStatusId");
      }
      const IhaveToCheck: boolean =
        (props?.isFromNetworkElement &&
          plannedResourceSelected?.ruleNetworkElement === 2 &&
          formData?.isReplacementExistingSolution) ||
        plannedResourceSelected?.ruleNetworkElement === 3;
      if (!copy?.linkedToPlannedActivityId && IhaveToCheck) {
        addInvalidProperty("linkedToPlannedActivityId");
      }
      if (
        !props?.isFromNetworkElement &&
        !props.isDesignAspect &&
        !copy?.riskEngId
      ) {
        addInvalidProperty("riskEngId");
      }
      if (
        !props?.isFromNetworkElement &&
        !props.isDesignAspect &&
        !copy?.riskOpeId
      ) {
        addInvalidProperty("riskOpeId");
      }
      if (plannedResourceSelected?.ruleLinkedDc === 35 && !selectedCluster) {
        addInvalidProperty("clusetrId");
      }
      if (
        plannedResourceSelected?.ruleLinkedDc === 35 &&
        formData?.plannedActivityResourceId
      ) {
        const hasClusters = filteredProgram && filteredProgram.length > 0;
        if (!hasClusters) {
          addInvalidProperty("isAtleastOneCluster");
        }
      }

      // if (
      //   (props.isReleaseDetailUnKnown || formData?.isPAReleaseDetailUnknown) &&
      //   (copy?.startDate === "" ||
      //     copy?.startDate === null ||
      //     copy?.startDate === undefined)
      // ) {
      //   addInvalidProperty("plannedStartDate");
      // }
      if (
        new Date(copy.startDate!).getTime() >
        new Date(copy.plannedCompletion!).getTime()
      ) {
        addInvalidProperty("plannedStartDate");
      }
    }
    console.log("copyValidation", copyValidation);
    setValidation(copyValidation);
    return copyValidation;
  };

  // useEffect(() => {
  //   if (validation && validation.response == false) {
  //     rootStore.dispatch(
  //       setNotification({
  //         message: "Check the fields entered",
  //         notifyType: NotifyType.warning,
  //       })
  //     );
  //   }
  // }, [validation]);
  //#endregion

  //UPDATE DATA ----------------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  useEffect(() => {
    const formArray = props.formArray
      ? props.formArray.map((el) => {
          return { ...el, opCoId: props.opcoId };
        })
      : [];
    setPlannedFormArray(formArray);

    //const deliverySatusObj = dictionaryToArray(props.formArray.)
  }, [props.formArray]);

  useEffect(() => {
    if (!edit) {
      // debugger;
      let copy = { ...createResource } as PlannedActivityDtoCreate;
      copy.responsibilityPhaseId =
        copy?.responsibilityPhaseResource &&
        dictionaryToArray(copy.responsibilityPhaseResource).find(
          (x) => x.value.toLowerCase() == "engineering"
        )?.key;
      if (props.lcmDeploymentStatusString?.toLowerCase().trim() === "planned") {
        const obj = getPlannedPAId(copy);
        copy.activityDetails = obj?.activityDetails;
        copy.activityDetailsText = obj?.activityDetailsText;
        copy.plannedActivityResourceId = !props?.edit
          ? obj?.plannedActivityResourceId
          : undefined;
      } else {
        copy.plannedActivityResourceId = undefined;
        copy.buildBagId = undefined;
      }
      if (!props.isInLcm) {
        copy.buildBagId = props.buildBagIds;
        copy.designComponentId = props.designComponent?.key;
      }

      setNoPAFormData(copy);
      setFormData(copy);
    }
    if (
      createResource !== undefined &&
      createResource !== null &&
      createResource.transientDesignComponentResource !== undefined
    ) {
      setTransientDC(createResource.transientDesignComponentResource);
    }
  }, [createResource, edit]);

  useEffect(() => {
    setPlannedFormArray(props.formArray ?? []);
    props.isReleaseDetailUnKnown
      ? props.action.New()
      : GetPlannedActivityCreateResource(props?.designComponent?.["key"]);
    if (props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned") {
      const obj = getPlannedPAId(createResource);
      if (!props.isInLcm) {
        obj.buildBagId = props.buildBagIds;
        obj.designComponentId = props.designComponent?.key;
      }
      setFormData(obj);
    } else {
      setFormData({
        ...createResource,
        buildBagId: props.isInLcm ? undefined : props.buildBagIds,
      });
    }
  }, []);

  useEffect(() => {
    if (
      props.networkElementAssociateds !== null &&
      props.networkElementAssociateds !== undefined
    ) {
      const hasNullValue = props.networkElementAssociateds.some(
        (item) => item.isFinalAsset === null
      );
      let newArr = props.networkElementAssociateds;
      if (hasNullValue) {
        newArr = props.networkElementAssociateds.map((item) => {
          if (item.isFinalAsset === null) {
            return { ...item, isFinalAsset: true };
          } else {
            return { ...item };
          }
        });
        setNewNetworkAssocitate(newArr);
        if (props.action.updateProdNodes && newArr) {
          props.action.updateProdNodes(newArr);
        }
      } else {
        setNewNetworkAssocitate(newArr);
      }
    }
  }, [props.networkElementAssociateds]);

  const { darkMode } = useTheme();

  useEffect(() => {
    if (
      props.idDetail &&
      props.idDetail != null &&
      props.formArray != undefined &&
      plannedFormArray != null &&
      plannedFormArray.length > 0 &&
      createResource &&
      formData &&
      formData != null &&
      index === undefined
    ) {
      var numberIndex = findWithAttr(
        plannedFormArray,
        "plannedActivityId",
        props.idDetail as number
      );
      if (numberIndex !== undefined && numberIndex > -1) {
        editForm(props.idDetail as number, numberIndex);
      }
    }
  }, [plannedFormArray, createResource, formData]);

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as PlannedActivityDtoUpdate;
    if (e && e["value"]) {
      copy[fieldSet] = e["value"];
      setFormData(copy);
    }
  };
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

  const CleanForm = (obj: any) => {
    if (obj) {
      var keyNames = Object.keys(obj);
      keyNames.forEach((x, i) => {
        if (obj[x] === null || obj[x] === undefined) {
          obj[x] = "";
        }
      });
      return obj;
    }
  };

  //#endregion

  //PLANNED ACTIVITY RESOURCE-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const [groupedPlannedResource, setGroupedPlannedResource] = useState<
    | {
        label: string;
        options: { key: number; value: PlannedActivityResourceDto }[];
      }[]
    | undefined
  >([]);

  const mapPlannedResource = () => {
    if (formData && formData.plannedActivityResource) {
      let array = [] as {
        label: string;
        options: { key: number; value: PlannedActivityResourceDto }[];
      }[];

      //Added as part of release details unknown
      // if (props?.isReleaseDetailUnKnown === true) {
      //   let RE = dictionaryToArrayPlannedActivityResourceDto(
      //     formData?.plannedActivityResource
      //   ).filter((x) => x.value.ruleLinkedDc == 7);
      //   array.push({ label: "RE", options: RE });
      //   onChangePlannedActivityResourceId(RE?.[0]?.["value"]);
      //   localStorage.setItem(
      //     "plannedActivity",
      //     RE?.[0]?.["value"]?.plannedActivityResourceDescription!
      //   );

      //   setGroupedPlannedResource(array);
      // } else
      if (props?.isFromNetworkElement === false && props.isDesignAspect) {
        let sw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.designAspectHardware === false &&
            x.value.designAspectSoftware === true &&
            x.value.forDesignAspect === true
        );
        let hw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.designAspectHardware === true &&
            x.value.designAspectSoftware === false &&
            x.value.forDesignAspect === true
        );
        let hwSw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.designAspectHardware === true &&
            x.value.designAspectSoftware === true &&
            x.value.forDesignAspect === true
        );

        array.push({ label: "SW", options: sw });
        array.push({ label: "HW", options: hw });
        array.push({ label: "HW & SW", options: hwSw });
        setGroupedPlannedResource(array);
      } else if (
        props?.isFromNetworkElement === false &&
        !props.isDesignAspect &&
        !props.isForAddAsset &&
        !props.isForEditAsset
      ) {
        if (
          (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
            "in-commissioning" ||
            props.lcmDeploymentStatusString?.toLowerCase().trim() ==
              "planned") &&
          props.both
        ) {
          let copy = { ...formData } as PlannedActivityDtoUpdate;
          let sw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === false &&
              x.value.lcmSoftware === true &&
              x.value.forLcm === true
          );
          let hw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === false &&
              x.value.forLcm === true
          );
          let hwSw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === true &&
              x.value.forLcm === true &&
              // x.value.plannedActivityResourceDescription ==
              //   "New System (HW&SW) Solution"
              x.value.ruleLinkedDc == 12
          );
          if (
            props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
          ) {
            hwSw = dictionaryToArrayPlannedActivityResourceDto(
              formData?.plannedActivityResource
            ).filter(
              (x) =>
                x.value.lcmHardware === true &&
                x.value.lcmSoftware === true &&
                x.value.forLcm === true
              //&&
              // x.value.plannedActivityResourceDescription ==
              //   "New System (HW&SW) Solution"
              //x.value.ruleLinkedDc == 12
            );
          }

          // copy.plannedDesignComponentName=props.designComponent
          //onChangeLinkedDesignComponentId(props.designComponent);
          //onChangePlannedActivityResourceId(hwSw?.[0]?.["value"]);
          localStorage.setItem(
            "plannedActivity",
            hwSw?.[0]?.["value"]?.plannedActivityResourceDescription!
          );
          if (
            props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
          ) {
            array.push({ label: "SW", options: sw });
            array.push({ label: "HW", options: hw });
          }
          array.push({ label: "HW & SW", options: hwSw });
          setGroupedPlannedResource(array);
          return;
        }
        if (
          (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
            "in-commissioning" ||
            props.lcmDeploymentStatusString?.toLowerCase().trim() ==
              "planned") &&
          !props.both
        ) {
          let copy = { ...formData } as PlannedActivityDtoUpdate;
          let sw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmSoftware === true &&
              x.value.forLcm === true &&
              x.value.ruleLinkedDc == 8
            // x.value.plannedActivityResourceDescription
            //   ?.toLowerCase()
            //   .trim() == "New NFxI Solution".toLowerCase().trim()
          );
          let hw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === false &&
              x.value.forLcm === true
          );
          let hwSw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === true &&
              x.value.forLcm === true
            //&&
            // x.value.plannedActivityResourceDescription ==
            //   "New System (HW&SW) Solution"
            //x.value.ruleLinkedDc == 12
          );
          if (
            props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
          ) {
            sw = dictionaryToArrayPlannedActivityResourceDto(
              formData?.plannedActivityResource
            ).filter(
              (x) => x.value.lcmSoftware === true && x.value.forLcm === true
              //&&
              //x.value.ruleLinkedDc == 8
              // x.value.plannedActivityResourceDescription
              //   ?.toLowerCase()
              //   .trim() == "New NFxI Solution".toLowerCase().trim()
            );
          }

          // copy.plannedDesignComponentName=props.
          localStorage.setItem(
            "plannedActivity",
            sw?.[0]?.["value"]?.plannedActivityResourceDescription!
          );

          //onChangeLinkedDesignComponentId(props.designComponent);
          //onChangePlannedActivityResourceId(sw?.[0]?.["value"]);
          array.push({ label: "SW", options: sw });
          if (
            props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
          ) {
            array.push({ label: "HW", options: hw });
            array.push({ label: "HW & SW", options: hwSw });
          }
          setGroupedPlannedResource(array);
          return;
        }
        if (
          props.lcmDeploymentStatusString?.toLowerCase().trim() ==
            "in-commissioning" ||
          props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
        ) {
          let copy = { ...formData } as PlannedActivityDtoUpdate;
          let hwSw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === true &&
              x.value.forLcm === true &&
              x.value.ruleLinkedDc == 12
            // x.value.plannedActivityResourceDescription ==
            //   "New System (HW&SW) Solution"
          );
          let sw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === false &&
              x.value.lcmSoftware === true &&
              x.value.forLcm === true
          );
          let hw = dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.lcmHardware === true &&
              x.value.lcmSoftware === false &&
              x.value.forLcm === true
          );
          if (
            props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned"
          ) {
            hwSw = dictionaryToArrayPlannedActivityResourceDto(
              formData?.plannedActivityResource
            ).filter(
              (x) =>
                x.value.lcmHardware === true &&
                x.value.lcmSoftware === true &&
                x.value.forLcm === true
              //&&
              // x.value.plannedActivityResourceDescription ==
              //   "New System (HW&SW) Solution"
              //x.value.ruleLinkedDc == 12
            );
            array.push({ label: "SW", options: sw });
            array.push({ label: "HW", options: hw });
          }
          // copy.plannedDesignComponentName=props.designComponent
          onChangeLinkedDesignComponentId(props.designComponent);
          //onChangePlannedActivityResourceId(hwSw?.[0]?.["value"]);
          array.push({ label: "HW & SW", options: hwSw });
          setGroupedPlannedResource(array);
          return;
        }

        let sw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.lcmHardware === false &&
            x.value.lcmSoftware === true &&
            x.value.forLcm === true
        );
        let hw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.lcmHardware === true &&
            x.value.lcmSoftware === false &&
            x.value.forLcm === true
        );
        let hwSw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.lcmHardware === true &&
            x.value.lcmSoftware === true &&
            x.value.forLcm === true
        );

        array.push({ label: "SW", options: sw });
        array.push({ label: "HW", options: hw });
        array.push({ label: "HW & SW", options: hwSw });
        setGroupedPlannedResource(array);
      } else if (props.isForAddAsset) {
        let sw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            (x.value.addAssetHardware === false ||
              x.value.addAssetHardware === null) &&
            x.value.addAssetSoftware === true &&
            x.value.forAddAsset === true
        );
        let hw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.addAssetHardware === true &&
            (x.value.addAssetSoftware === false ||
              x.value.addAssetSoftware === null) &&
            x.value.forAddAsset === true
        );
        let hwSw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.addAssetHardware === true &&
            x.value.addAssetSoftware === true &&
            x.value.forAddAsset === true
        );

        array.push({ label: "SW", options: sw });
        array.push({ label: "HW", options: hw });
        array.push({ label: "HW & SW", options: hwSw });

        setGroupedPlannedResource(array);
      } else if (props.isForEditAsset) {
        let sw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.editAssetSoftware === true &&
            (x.value.editAssetHardware === false ||
              x.value.editAssetHardware === null) &&
            x.value.forEditAsset === true
        );
        let hw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.editAssetHardware === true &&
            (x.value.editAssetSoftware === false ||
              x.value.editAssetSoftware === null) &&
            x.value.forEditAsset === true
        );
        let hwSw = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.editAssetHardware === true &&
            x.value.editAssetSoftware === true &&
            x.value.forEditAsset === true
        );

        array.push({ label: "SW", options: sw });
        array.push({ label: "HW", options: hw });
        array.push({ label: "HW & SW", options: hwSw });
        setGroupedPlannedResource(array);
      } else {
        let isVirtualized = false;

        if (
          formData?.designComponentIsVirtualizedResource &&
          props.designComponent?.key
        ) {
          isVirtualized =
            formData?.designComponentIsVirtualizedResource[
              props?.designComponent?.key
            ];
        }

        let resource = dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter((x) => x.value.forNetworkElement == true);
        if (
          props.plannedActivityAllowed &&
          props.plannedActivityAllowed?.length > 0
        ) {
          resource = resource.filter(
            (el) =>
              props.plannedActivityAllowed?.includes(el.key) &&
              (isVirtualized
                ? el.value.onVirtualizedNetworkElement === true
                : el.value.onBareMetalNetworkElement === true)
          );
        }
        array.push({ label: "Network Element", options: resource });
        setGroupedPlannedResource(array);
      }
    }
  };

  const [plannedResourceSelected, setPlannedResourceSelected] =
    useState<PlannedActivityResourceDto>();

  useEffect(() => {
    //SE L'ATTIVITA è EXPORTABLE
    if (
      formData &&
      formData.plannedActivityResource &&
      formData.plannedActivityResourceId !== undefined &&
      !numberIsNullOrZero(formData.plannedActivityResourceId) &&
      dictionaryToArrayPlannedActivityResourceDto(
        formData.plannedActivityResource
      ).find((y) => y.key == formData.plannedActivityResourceId)?.value
        .exportable == true
    ) {
      setIsConsigliati(true);
    } else {
      setIsConsigliati(false);
    }

    //SETTO LO STATO OBJ DI PLANNED ACTIVITY RESOURCE SELECTED
    if (
      formData?.plannedActivityResourceId !== undefined &&
      formData.plannedActivityResource != undefined
    ) {
      let selected = dictionaryToArrayPlannedActivityResourceDto(
        formData?.plannedActivityResource
      ).find((x) => x.key == formData?.plannedActivityResourceId);
      setPlannedResourceSelected(selected?.value);
    }
    //MAPPING DELLE PLANNED RESOURCE
    if (formData && formData.plannedActivityResource != undefined) {
      mapPlannedResource();
    }
  }, [
    formData?.plannedActivityResourceId,
    formData?.plannedActivityResource,
    props.plannedActivityAllowed,
  ]);

  useEffect(() => {
    if (formData && formData.plannedActivityResource != undefined) {
      mapPlannedResource();
    }
  }, [props.plannedActivityAllowed]);

  const onGetDeliveryStatus = async (e?: any) => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;

    if ((copy && copy.plannedActivityResourceId !== undefined) || e) {
      copy.plannedActivityResourceId = e ? e : copy.plannedActivityResourceId;
      let deliveryStatusResponse;
      if (copy.plannedActivityResourceId) {
        deliveryStatusResponse =
          await GetPlannedActivityRelatedToDeliveryStatus(
            e ? e : copy.plannedActivityResourceId!,
            props?.plannedActivityTypeForEnum
          );
      }
      const deliveryStatusList =
        deliveryStatusResponse?.length > 0 ? deliveryStatusResponse : null;

      if (deliveryStatusList?.length === 1) {
        copy.deliveryStatusId = deliveryStatusList[0]?.key;
      }

      setDeliveryStatusArr(deliveryStatusList);

      // copy.deliveryStatusResource = deliveryStatusList;
      // setFormData(copy);
    }
  };
  useEffect(() => {
    if (
      DeliveryStatusArr?.length == 1 ||
      (plannedResourceSelected?.ruleLinkedDc == 14 && !edit)
    ) {
      let copy = { ...formData } as PlannedActivityDtoUpdate;
      copy.deliveryStatusId = DeliveryStatusArr?.[0]?.key;
      setFormData(copy);
    }
  }, [DeliveryStatusArr]);

  useEffect(() => {
    if (
      formData?.plannedActivityResourceId !== undefined &&
      !formData?.deliveryStatusResource! &&
      !isNoPA
    ) {
      onGetDeliveryStatus();
    }
  }, [formData?.plannedActivityResourceId]);

  const onChangePlannedActivityResourceId = async (
    obj?: PlannedActivityResourceDto,
    data?: PlannedActivityDtoUpdate
  ) => {
    // alert("s");
    let copy;
    if (data !== undefined) {
      copy = data;
    } else {
      copy = { ...formData } as PlannedActivityDtoUpdate;
    }

    if (obj && obj.plannedActivityResourceId !== undefined) {
      // alert("s")
      // debugger
      copy.plannedActivityResourceId = obj.plannedActivityResourceId;
    } else {
      copy.plannedActivityResourceId = undefined;
    }

    let target = obj as PlannedActivityResourceDto;

    setPlannedResourceSelected(target);

    if (
      target?.ruleLinkedDc === 8 ||
      target?.ruleLinkedDc === 12 ||
      target?.ruleLinkedDc === 16 ||
      target?.ruleLinkedDc === 34 ||
      target?.ruleLinkedDc === 36 ||
      target?.ruleLinkedDc === 35
    ) {
      copy.designComponentId = props.designComponent?.key;
    } else if (
      props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned" &&
      target.designAspectExportable !== true
    ) {
      copy.designComponentId = null;
    }
    const ruleActivityDetailsLcm = target?.ruleActicvityDetails as number;
    const ruleActivityDetailsNetworkElement =
      target?.ruleActicvityDetailsNetworkElement as number;

    const ruleActivityDetailsDesignComponent =
      target.ruleDesignAspect as number;
    const exportable = target.exportable as boolean;

    //SE PROVIENI DA LCM
    if (!props?.isFromNetworkElement && !props.isDesignAspect) {
      copy.activityDetails = undefined;
      copy.activityDetailsText = undefined;
      if (
        // exportable &&
        copy?.designComponentId !== undefined &&
        copy?.designComponentId?.toString() !== ""
      ) {
        if (
          ruleActivityDetailsLcm !== undefined &&
          ruleActivityDetailsLcm !== 0 &&
          copy.designComponentId !== undefined
        ) {
          if (ruleActivityDetailsLcm <= 4) {
            //VECCHIE REGOLE (SERVER)
            if (copy?.designComponentId) {
              let rtnActivityDetails = await GetActivityDetails(
                copy?.designComponentId,
                ruleActivityDetailsLcm
              );
              copy.activityDetails = rtnActivityDetails;
              copy.activityDetailsText = rtnActivityDetails;
              setActivityDetailsManual(false);
            }
          } else {
            //NUOVE REGOLE (CLIENT)
            switch (ruleActivityDetailsLcm) {
              case 5:
                //Free Text from User
                setActivityDetailsManual(true);
                copy.activityDetails = "";
                copy.activityDetailsText = "";
                break;

              case 6:
                copy.activityDetails = target.activityDetailsLcm;
                copy.activityDetailsText = target.activityDetailsLcm;
                break;
              case 8:
                //Fixed Text from Admin
                //Add/Remove HW Components

                copy.activityDetails = target.activityDetailsLcm;
                copy.activityDetailsText = target.activityDetailsLcm;
                break;

              case 7:
                //Upgrade HW Components
                if (copy.designComponentId && props.designComponent?.key) {
                  setActivityDetailsManual(false);
                  let sameHwType = await GetConfrontoHardwareType(
                    props.designComponent.key,
                    copy.designComponentId
                  );

                  let plannedDcDescription =
                    resourceDesignComponent &&
                    resourceDesignComponent.find(
                      (x) => x.key === copy.designComponentId
                    )?.value;

                  if (sameHwType == true) {
                    copy.activityDetails = target.activityDetailsLcm
                      ? target.activityDetailsLcm
                      : plannedDcDescription ?? "";
                    copy.activityDetailsText = target.activityDetailsLcm
                      ? target.activityDetailsLcm
                      : plannedDcDescription ?? "";
                  } else {
                    copy.activityDetails = plannedDcDescription ?? "";
                    copy.activityDetailsText = plannedDcDescription ?? "";
                  }
                }
                break;

              default:
                break;
            }
          }
        } else {
          setActivityDetailsManual(true);
          copy.activityDetails = undefined;
          copy.activityDetailsText = undefined;
        }
      } else {
        setIsJson(false);
        setIsConsigliati(false);
        setActivityDetailsJson(undefined);
        resetResourceDesignComponent();
        copy.activityDetails = undefined;
        copy.activityDetailsText = undefined;
      }
    } else if (!props?.isFromNetworkElement && props.isDesignAspect) {
      switch (ruleActivityDetailsDesignComponent) {
        case 1:
          //Virtualize System
          if (
            formData?.designComponentIsVirtualizedResource &&
            formData?.plannedActivityResourceId
          ) {
            let isVirtualized = false;
            if (
              formData?.designComponentIsVirtualizedResource &&
              props.designComponent?.key
            ) {
              isVirtualized =
                formData?.designComponentIsVirtualizedResource[
                  props?.designComponent?.key
                ];
            }
            if (isVirtualized === true) {
              copy.activityDetails =
                target.activityDetailsForVirtualizedNetworkElement;
              copy.activityDetailsText =
                target.activityDetailsForVirtualizedNetworkElement;
            } else {
              copy.activityDetails = target.activityDetailsNetworkElement;
              copy.activityDetailsText = target.activityDetailsNetworkElement;
            }
          }
          break;

        case 2:
          //Free Text from User
          setActivityDetailsManual(true);
          copy.activityDetails = "";
          copy.activityDetailsText = "";
          break;

        case 3:
        case 6:
          //Fixed Text from Admin
          //Add/Remove HW Components
          copy.activityDetails = target.activityDetailsDesignAspect;
          copy.activityDetailsText = target.activityDetailsDesignAspect;

          setActivityDetailsManual(false);
          break;

        case 4:
          //Network Element & Location
          setActivityDetailsManual(false);
          copy.activityDetails =
            props.elementDeploymentName + " at " + props.location;
          copy.activityDetailsText =
            props.elementDeploymentName + " at " + props.location;
          break;

        case 5:
          if (
            copy.designComponentId != undefined &&
            props.designComponent?.key != undefined
          ) {
            setActivityDetailsManual(false);
            let sameHwType = await GetConfrontoHardwareType(
              props.designComponent.key,
              copy.designComponentId
            );
            if (sameHwType == true) {
              copy.activityDetails = target.activityDetailsLcm;
              copy.activityDetailsText = target.activityDetailsLcm;
            } else {
              let plannedDcDescription =
                resourceDesignComponent &&
                resourceDesignComponent.find(
                  (x) => x.key === copy.designComponentId
                )?.value;
              copy.activityDetails = plannedDcDescription ?? "";
              copy.activityDetailsText = plannedDcDescription ?? "";
            }
          }
          break;

        default:
          break;
      }
    }
    //SE PROVIENI DA N.E. AS PLANNED
    else {
      switch (ruleActivityDetailsNetworkElement) {
        case 1:
          //Virtualize System
          if (
            formData?.designComponentIsVirtualizedResource &&
            formData?.plannedActivityResourceId
          ) {
            let isVirtualized = false;
            if (
              formData?.designComponentIsVirtualizedResource &&
              props.designComponent?.key
            ) {
              isVirtualized =
                formData?.designComponentIsVirtualizedResource[
                  props?.designComponent?.key
                ];
            }
            if (isVirtualized === true) {
              copy.activityDetails =
                target.activityDetailsForVirtualizedNetworkElement;
              copy.activityDetailsText =
                target.activityDetailsForVirtualizedNetworkElement;
            } else {
              copy.activityDetails = target.activityDetailsNetworkElement;
              copy.activityDetailsText = target.activityDetailsNetworkElement;
            }
          }
          break;

        case 2:
          //Free Text from User
          setActivityDetailsManual(true);
          copy.activityDetails = "";
          copy.activityDetailsText = "";
          break;

        case 3:
        case 6:
          //Fixed Text from Admin
          //Add/Remove HW Components
          copy.activityDetails = target.activityDetailsNetworkElement;
          copy.activityDetailsText = target.activityDetailsNetworkElement;

          setActivityDetailsManual(false);
          break;

        case 4:
          //Network Element & Location
          setActivityDetailsManual(false);
          copy.activityDetails =
            props.elementDeploymentName + " at " + props.location;
          copy.activityDetailsText =
            props.elementDeploymentName + " at " + props.location;
          break;

        case 5:
          if (
            copy.designComponentId != undefined &&
            props.designComponent?.key != undefined
          ) {
            setActivityDetailsManual(false);
            let sameHwType = await GetConfrontoHardwareType(
              props.designComponent.key,
              copy.designComponentId
            );
            if (sameHwType == true) {
              copy.activityDetails = target.activityDetailsLcm;
              copy.activityDetailsText = target.activityDetailsLcm;
            } else {
              let plannedDcDescription =
                resourceDesignComponent &&
                resourceDesignComponent.find(
                  (x) => x.key === copy.designComponentId
                )?.value;
              copy.activityDetails = plannedDcDescription ?? "";
              copy.activityDetailsText = plannedDcDescription ?? "";
            }
          }
          break;

        default:
          break;
      }
    }

    //SE NON AVVIENE NESSUN CALCOLO SETTO MANUALE ACTIVITY DETAILS
    if (stringIsNullOrEmpty(copy.activityDetails)) {
      setActivityDetailsManual(true);
      // copy.activityDetails = "";
      // copy.activityDetailsText = "";
    }

    //RULE DESIGN COMPONENT
    if (
      target &&
      target.ruleLinkedDc != undefined &&
      formData &&
      formData.plannedActivityResourceId !== undefined
    ) {
      changeResourceDesignComponent(target.ruleLinkedDc);
    } else {
      resetResourceDesignComponent();
    }
    // copy.designComponentId = undefined;
    setFormData(copy);
  };

  //#endregion

  //DESIGN COMPONENT-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region

  const [plannedDesignComponentRequired, setPlannedDesignComponentRequired] =
    useState<boolean>(false);

  useEffect(() => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (
      formData?.plannedActivityResource &&
      formData?.plannedActivityResourceId != undefined
    ) {
      const checkIsNoPA =
        formData?.plannedActivityResource &&
        dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).filter(
          (x) =>
            x.value.plannedActivityResourceId ===
              formData?.plannedActivityResourceId && x.value.ruleLinkedDc === 15
        ).length === 1;
      setIsNoPA(checkIsNoPA ? true : false);
      let target = dictionaryToArrayPlannedActivityResourceDto(
        formData.plannedActivityResource
      ).find((x) => x.key == formData.plannedActivityResourceId)?.value;
      if (target?.plannedDesignComponentRequiredNetworkElement == true) {
        setPlannedDesignComponentRequired(true);
      } else {
        setPlannedDesignComponentRequired(false);
        if (props?.isFromNetworkElement) {
          // debugger
          copy.designComponentId = undefined;
          setFormData(copy);
        }
      }
    } else {
      setIsNoPA(false);
      setPlannedDesignComponentRequired(false);
    }
  }, [formData?.plannedActivityResourceId]);

  const onChangeLinkedDesignComponentId = async (e: any) => {
    removeValidation("designComponentId");
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (e && e["key"]) {
      // debugger
      copy.designComponentId = e && e["key"];
      copy.plannedDesignComponentName = e["value"];
      setFormData(copy);
    } else {
      // debugger
      copy.designComponentId = undefined;
      setFormData(copy);
    }
    if (
      plannedResourceSelected != undefined &&
      formData &&
      formData.plannedActivityResourceId !== undefined
    ) {
      if (props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned") {
        plannedResourceSelected.designAspectExportable = true;
      }
      onChangePlannedActivityResourceId(plannedResourceSelected, copy);
      if (props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned") {
        plannedResourceSelected.designAspectExportable = true;
      }
    }
    // debugger
    setFormData(copy);
  };

  useEffect(() => {
    if (
      props.designComponent &&
      formData &&
      formData.plannedActivityResource &&
      formData.plannedActivityResourceId !== undefined
    ) {
      let rule = dictionaryToArrayPlannedActivityResourceDto(
        formData.plannedActivityResource
      ).find((x) => x.key === formData.plannedActivityResourceId)?.value
        .ruleLinkedDc;
      if (rule && rule != undefined) {
        changeResourceDesignComponent(rule);
      } else {
        resetResourceDesignComponent();
      }
    }
    if (
      plannedResourceSelected != undefined &&
      formData &&
      formData.plannedActivityResourceId !== undefined &&
      props.lcmDeploymentStatusString?.toLowerCase().trim() !== "planned"
    ) {
      onChangePlannedActivityResourceId(plannedResourceSelected);
    }
  }, [props.designComponent, formData?.plannedActivityResourceId]);

  useEffect(() => {
    if (
      (props.onHardware || props.onSoftware) &&
      props.designComponent &&
      (props.lcmDeploymentStatusString?.toLowerCase().trim() === "planned" ||
        props.lcmDeploymentStatusString?.toLowerCase().trim() ===
          "in-commissioning")
    ) {
      const newResObject = getPlannedPAId(createResource);
      setFormData((prev) => ({
        ...prev,
        plannedActivityResourceId: newResObject.plannedActivityResourceId,
        activityDetails: newResObject.activityDetails,
        activityDetailsText: newResObject.activityDetailsText,
      }));
    }
  }, [props.onHardware, props.onSoftware]);

  const getPlannedPAId = (resObj) => {
    let copy = { ...createResource } as PlannedActivityDtoCreate;
    if (
      copy !== null &&
      copy !== undefined &&
      copy.plannedActivityResource &&
      (props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned" ||
        props.lcmDeploymentStatusString?.toLowerCase().trim() ===
          "in-commissioning")
    ) {
      let hwSw = dictionaryToArrayPlannedActivityResourceDto(
        copy?.plannedActivityResource
      ).filter(
        (x) =>
          x.value.lcmHardware === true &&
          x.value.lcmSoftware === true &&
          x.value.forLcm === true &&
          x.value.ruleLinkedDc === 12
      );
      let sw = dictionaryToArrayPlannedActivityResourceDto(
        copy?.plannedActivityResource
      ).filter(
        (x) =>
          x.value.lcmHardware === false &&
          x.value.lcmSoftware === true &&
          x.value.forLcm === true &&
          x.value.ruleLinkedDc === 8
      );
      if (
        props.onHardware === true &&
        props.onSoftware === true &&
        props.isInLcm === true
      ) {
        copy.plannedActivityResourceId =
          hwSw[0]?.value.plannedActivityResourceId;
        copy.activityDetails =
          hwSw[0]?.value.activityDetailsLcm ?? copy.activityDetails;
        copy.activityDetailsText =
          hwSw[0]?.value.activityDetailsLcm ?? copy.activityDetailsText;
      } else if (
        props.onHardware === false &&
        props.onSoftware === true &&
        props.isInLcm === true
      ) {
        copy.plannedActivityResourceId = sw[0]?.value.plannedActivityResourceId;
        copy.activityDetails =
          sw[0]?.value.activityDetailsLcm ?? copy.activityDetails;
        copy.activityDetailsText =
          sw[0]?.value.activityDetailsLcm ?? copy.activityDetailsText;
      }
    }
    return copy;
  };

  //SET DESIGN COMPONENT CONSIGLIATI
  const changeResourceDesignComponent = async (rule: number) => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (
      props.designComponent?.key != undefined &&
      resourceDesignComponent != undefined
    ) {
      await GetLinkedDesignComponent(props.designComponent?.key, rule).then(
        (c) => {
          if (resourceDesignComponent != undefined && c != undefined) {
            const tDCR = transientDC
              ? Object.keys(transientDC)
                  .filter((key) => !isNaN(parseInt(key, 10))) // Filter for valid numeric keys
                  .map((key) => parseInt(key, 10))
              : [];
            const mergeLinkedTDC: any = [];
            const mergeLinkedDC: any = [];
            const mergeNonLinkedDC: any = [];
            const newRes = resourceDesignComponent?.map((x) => {
              return {
                key: x.key,
                value: x.value,
                color: c.includes(x.key)
                  ? "#0000dd"
                  : tDCR.includes(x.key)
                  ? "#80400B"
                  : "#000000",
                isLinked:
                  c.includes(x.key) || tDCR.includes(x.key) ? true : false,
              };
            });
            // resourceDesignComponent.map((x) => {
            //   if (c.includes(x.key)) {
            //     mergeLinkedDC.push({
            //       key: x.key,
            //       value: x.value,
            //       color: "#0000dd",
            //       isLinked: true,
            //     });
            //   } else if (tDCR.includes(x.key)) {
            //     mergeLinkedTDC.push({
            //       key: x.key,
            //       value: x.value,
            //       color: "#80400B",
            //       isLinked: true,
            //     });
            //   } else {
            //     mergeNonLinkedDC.push({
            //       key: x.key,
            //       value: x.value,
            //       color: "#000000",
            //       isLinked: false,
            //     });
            //   }
            // });
            // let merge = [
            //   ...mergeLinkedDC,
            //   ...mergeLinkedTDC,
            //   ...mergeNonLinkedDC,
            // ];
            // const mergeLinked = merge.filter((a) => a.isLinked);
            // if (props?.isFromNetworkElement) {
            //   setIsConsigliati(true);
            //   setResourceDesignComponent(mergeLinked);
            // } else {
            //   const otherDesignComponent = merge
            //     .filter((a) => !a.isLinked)
            //     .sort((a, b) =>
            //       a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
            //     );
            //   const finalArray = mergeLinked.concat(otherDesignComponent);
            //   setResourceDesignComponent(finalArray);
            // }
            setResourceDesignComponent(newRes);
          } else {
            resetResourceDesignComponent();
          }
        }
      );
    }
  };

  //RESET
  const resetResourceDesignComponent = async () => {
    let copy = { ...formData } as PlannedActivityDtoCreate;
    // setIsConsigliati(false);
    if (props?.isFromNetworkElement) {
      setResourceDesignComponent([]);
    } else {
      let newDC: any = copy.designComponentResource;
      let designComponentResource: any =
        transientDC !== undefined
          ? { ...copy.designComponentResource, ...transientDC }
          : { ...copy.designComponentResource };

      const tDCR = transientDC
        ? Object.keys(transientDC)
            .filter((key) => !isNaN(parseInt(key, 10))) // Filter for valid numeric keys
            .map((key) => parseInt(key, 10))
        : [];
      const newMerge = newDC.map((x) => {
        return {
          key: x.key,
          value: x.value,
          color: tDCR.includes(x.key) ? "#80400B" : "#000000",
          isLinked: tDCR.includes(x.key),
        };
      });
      // const merge = dictionaryToArray(designComponentResource).map((x) => {
      //   return {
      //     key: x.key,
      //     value: x.value,
      //     color: tDCR.includes(x.key) ? "#80400B" : "#000000",
      //     isLinked: tDCR.includes(x.key),
      //   };
      // });
      // const mergeLinked = merge
      //   .filter((a) => a.isLinked)
      //   .sort((a, b) =>
      //     a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
      //   );
      // const otherDesignComponent = merge
      //   .filter((a) => !a.isLinked)
      //   .sort((a, b) =>
      //     a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
      //   );
      // const finalArray = mergeLinked.concat(otherDesignComponent);
      // setResourceDesignComponent(finalArray);
      setResourceDesignComponent(newMerge);
    }
  };

  //AUTO RESET
  useEffect(() => {
    if (formData && formData.designComponentResource != undefined) {
      resetResourceDesignComponent();
    }
  }, [
    formData?.designComponentResource,
    formData?.transientDesignComponentResource,
  ]);
  //#endregion

  //ACTIVITY STATUS---------------------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  useEffect(() => {
    if (
      formData?.budgetAvailabilityId !== null ||
      (formData?.responsibilityPhaseId !== 1 &&
        formData?.responsibilityPhaseId !== null) ||
      formData?.deliveryStatusId !== null ||
      formData?.localApproval !== null
    ) {
      launchActivityStatusLogic();
    }
  }, [
    formData?.budgetAvailabilityId,
    formData?.responsibilityPhaseId,
    formData?.deliveryStatusId,
    formData?.localApproval,
  ]);

  const [ActivityStatusFromLogic, setActivityStatusFromLogic] = useState<
    number[]
  >([]);

  const [radioActivityStatusChecked, setRadioActivityStatusChecked] =
    useState<boolean>(false);

  const launchActivityStatusLogic = async () => {
    await ActivityStatusLogics(
      formData?.deliveryStatusId,
      formData?.budgetAvailabilityId,
      formData?.responsibilityPhaseId,
      formData?.localApproval
    ).then((x) => {
      setActivityStatusFromLogic(x);
    });
  };

  useEffect(() => {
    if (ActivityStatusFromLogic?.length == 1) {
      // debugger
      let copy = { ...formData } as PlannedActivityDtoUpdate;
      copy.activityStatusId = ActivityStatusFromLogic?.[0];
      setFormData(copy);
    }
  }, [ActivityStatusFromLogic]);

  //EDIT / NEW / ADD / DELETE-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const isThisACheck = (property: string, event: any) => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (property === "isReplacementExistingSolution") {
      copy.isReplacementExistingSolution = true;
      copy.isNewServiceArchitecture = false;
    } else {
      copy.isReplacementExistingSolution = false;
      copy.isNewServiceArchitecture = true;
    }
    setFormData(copy);
  };

  const editForm = async (
    id: number | undefined,
    index: number,
    item?: any
  ) => {
    props.action.setShowForm(false);
    setChanged(false);
    setValidation({ response: true });
    let plannedToEdit;

    if (id) {
      plannedToEdit = plannedFormArray.find((el) => el.plannedActivityId == id);
    } else {
      plannedToEdit = plannedFormArray[index];
    }

    plannedToEdit = CleanForm(plannedToEdit);
    if (
      plannedToEdit?.plannedImplementationYear &&
      plannedToEdit?.plannedImplementationYear != null
    ) {
      setStartDate(
        new Date("01/01/" + plannedToEdit.plannedImplementationYear)
      );
    } else {
      setStartDate(undefined);
    }
    setRuleDCIdFlag(plannedToEdit?.isPAReleaseDetailUnknown);
    if (
      plannedToEdit &&
      plannedToEdit?.isPAReleaseDetailUnknown &&
      plannedToEdit.designComponentId &&
      plannedToEdit.plannedDesignComponentName
    ) {
      let c: any = [
        {
          key: `${plannedToEdit.designComponentId}`,
          value: plannedToEdit?.plannedDesignComponentName ?? "",
        },
      ];
      setUnknownDC(c);
      setFormData({
        ...plannedToEdit,
        designComponentId: c[0].key,
        plannedDesignComponentName: c[0].value,
        activityDetails: formData?.activityDetails,
        activityDetailsText: formData?.activityDetailsText,
        deliveryStatusResource: formData?.deliveryStatusResource,
        reasonForNoPlan: props.reasonForNoPlan ?? undefined,
        commentOnProjectStatus: props.commentOnProjectStatus ?? undefined,
      });
    } else {
      setUnknownDC(undefined);
      setResourceDesignComponent(plannedToEdit?.designComponentResource);
      setFormData({
        ...plannedToEdit,
        deliveryStatusResource: formData?.deliveryStatusResource,
        reasonForNoPlan: props.reasonForNoPlan ?? undefined,
        commentOnProjectStatus: props.commentOnProjectStatus ?? undefined,
      });
    }
    setIndex(index);
    setEdit(true);
    props.action.setShowForm(true);
  };

  const createForm = async () => {
    setIsNoPA(false);
    setRuleDCIdFlag(false);
    setEdit(false);
    setIndex(-1);
    setCustomDateFormat(null);
    setStartDate(undefined);
    if (props.lcmDeploymentStatusString?.toLowerCase().trim() === "planned") {
      const obj: PlannedActivityDtoCreate = getPlannedPAId(createResource);
      if (!props.isInLcm) {
        obj.buildBagId = props.buildBagIds;
      }
      setFormData(obj);
    } else {
      setFormData({
        ...createResource,
        buildBagId: props.isInLcm ? undefined : props.buildBagIds,
      });
    }
    if (props.isReleaseDetailUnKnown) {
      props.action.setShowForm(true);
      props.action.New();
    } else {
      const result: any = await GetPlannedActivityCreateResource(
        props?.designComponent?.["key"]
      );
      if (result !== null && result !== undefined) {
        GetNetworkElementOpCo({
          dcId: props.designComponent?.key,
          opCoId: props.opcoId,
          lcmBagId: props.buildBagIds,
        })
          .then((x) => {
            if (x && Array.isArray(x)) {
              setBuildBagRes(x);
            } else {
              setBuildBagRes([]);
            }
          })
          .finally(() => {
            props.action.setShowForm(true);
          });
      }
    }
    setActivityDetailsJson("");
  };

  let data = plannedFormArray.map((x) => {
    if (x && x != null) {
      let deliveryStatusName = DeliveryStatusArr
        ? DeliveryStatusArr.filter((y) => y.key === x.deliveryStatusId)[0]
            ?.value
        : x?.deliveryStatusResource &&
          dictionaryToArray(x.deliveryStatusResource).find(
            (y) => y.key === x?.deliveryStatusId
          )?.value;
      const checkIsNoPA =
        x?.plannedActivityResource &&
        dictionaryToArrayPlannedActivityResourceDto(
          x?.plannedActivityResource
        ).filter(
          (res) =>
            res.value.plannedActivityResourceId ===
              x?.plannedActivityResourceId && res.value.ruleLinkedDc === 15
        ).length === 1;
      return {
        // plannedAction: `${x.plannedImplementationYear} | ${
        //   x?.activityStatusResource &&
        //   dictionaryToArray(x.activityStatusResource).find(
        //     (y) => y.key === x?.activityStatusId
        //   )?.value
        // } | ${
        //   x?.planningActivityStatusResource &&
        //   dictionaryToArray(x?.planningActivityStatusResource).find(
        //     (y) => y.key === x?.planningActivityStatusId
        //   )?.value
        // }`,
        plannedAction: checkIsNoPA
          ? `${
              x.plannedActivityResource &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x?.plannedActivityResourceId)?.value
                ?.plannedActivityResourceDescription
            }`
          : `${parseDate(x?.plannedCompletionValue).toLocaleDateString(
              "en-us",
              {
                month: "short",
              }
            )} - ${parseDate(x?.plannedCompletionValue).getFullYear()} | ${
              x.plannedActivityResource &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x?.plannedActivityResourceId)?.value
                ?.plannedActivityResourceDescription
            } | ${deliveryStatusName} `,
        lastModified: x.lastModified,
        forAddAsset: x.forAddAsset,
        forEditAsset: x.forEditAsset,
        designComponent: props.designComponent?.value,
        designComponentFamily: props.designComponentFamily,
        productImportance: props.productImportance?.value,
        opco: props.opco,
        eduSpoc: props.eduSpoc,
        subDomainSpoc: props.subdomainSpoc,
        plannedActivityId: x.plannedActivityId,
        deliveryStatusId: x.deliveryStatusId,
        activityStatus:
          x?.activityStatusResource &&
          dictionaryToArray(x.activityStatusResource).find(
            (y) => y.key === x?.activityStatusId
          )?.value,
        deliveryStatusName:
          x?.deliveryStatusResource &&
          dictionaryToArray(x.deliveryStatusResource).find(
            (y) => y.key === x?.deliveryStatusId
          )?.value,
        plannedImplementationYear: x?.plannedImplementationYear,
        currentBuildBagDescription: x?.currentBuildBagDescription,
        plannedBuildBagDescription:
          x?.buildBagResources?.find((val) => val.key === x.buildBagId)?.text ??
          x?.plannedBuildBagDescription,
        plannedDesignComponentName: isNoPA
          ? props.designComponent?.value
          : x?.plannedDesignComponentName!,
        plannedCompletionValue: x?.plannedCompletionValue,
        preBaseLineDateValue: x?.preBaseLineDateValue,
        planningActivityStatus:
          x?.planningActivityStatusResource &&
          dictionaryToArray(x?.planningActivityStatusResource).find(
            (y) => y.key === x?.planningActivityStatusId
          )?.value,
      };
    }
    return {};
  });

  const Delete = (index: number) => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to Delete?",
      button: "Delete",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => {
          confirmDelete(index);
        },
      },
    });
  };

  const handlePlannedCategoryChange = (obj, propertyId, propertyName) => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    //setFormData({...formData,proprtyId:e.key,propertyName:e.value})
    if (propertyId !== null) {
      copy[propertyId] = obj && obj["key"];
    }
    if (propertyName === "lcmCategories") {
      copy[propertyName] = obj && obj["key"];
    } else {
      copy[propertyName] = obj && obj["value"];
    }
    setFormData(copy);
  };
  const deletePA = (index: number) => {
    let copyFormArray = [
      ...(plannedFormArray ?? []),
    ] as PlannedActivityDtoUpdate[];
    const paRes: any = copyFormArray[index].plannedActivityResource;
    const paID: any = copyFormArray[index].plannedActivityResourceId;
    const isPACheck =
      dictionaryToArrayPlannedActivityResourceDto(paRes).filter(
        (res) =>
          res.value.plannedActivityResourceId == paID &&
          res.value.ruleLinkedDc == 15
      ).length > 0
        ? true
        : false;
    if (isPACheck) {
      copyFormArray.splice(index, 1);
      setPlannedFormArray(copyFormArray);
      props.action.AddOnList(copyFormArray);
    } else {
      Delete(index);
    }
  };

  const confirmNoPADelete = (index: number) => {
    setConfirm({
      title: "Confirm",
      message:
        "By clicking on create new planned activity, your existing No planned activity will be deleted?",
      button: "Yes",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => {
          confirmDelete(index);
          createForm();
        },
      },
    });
  };

  const confirmDelete = (index: number) => {
    let copyFormArray = [
      ...(plannedFormArray ?? []),
    ] as PlannedActivityDtoUpdate[];
    props.action.AddOnList(copyFormArray);
    copyFormArray.splice(index, 1);
    setConfirm(stateConfirm);
  };

  const addOnList = async () => {
    // if (
    //   formData &&
    //   formData.startDate &&
    //   formData.plannedCompletion &&
    //   (props?.isReleaseDetailUnKnown || formData?.isPAReleaseDetailUnknown) &&
    //   plannedFormArray &&
    //   plannedFormArray.length > 0
    // ) {
    //   const startDateObj =  (formData?.startDate === null || formData?.startDate === "") ? new Date() : new Date(formData.startDate);
    //   const endDateObj = new Date(formData.plannedCompletion);
    //   const formattedStartDate = `${startDateObj.getDate()}/${
    //     startDateObj.getMonth() + 1
    //   }/${startDateObj.getFullYear()}`;
    //   const formattedEndDate = `${endDateObj.getDate()}/${
    //     endDateObj.getMonth() + 1
    //   }/${endDateObj.getFullYear()}`;
    //   const isDateInvalid = validateStartEndDate({
    //     start: formattedStartDate,
    //     end: formattedEndDate,
    //   });
    //   if (!isDateInvalid) {
    //     rootStore.dispatch(
    //       setNotification({
    //         message: "Planned activity already exist with the date range.",
    //         notifyType: NotifyType.warning,
    //       })
    //     );
    //     return;
    //   }
    // }
    if (formData !== null && formData !== undefined) {
      const { isReleaseDetailUnKnown } = props;
      const { plannedCompletion, isPAReleaseDetailUnknown, designComponentId } =
        formData;
      if (plannedFormArray && plannedFormArray.length > 0) {
        const endDate = plannedCompletion ? new Date(plannedCompletion) : null;
        const compareOtherPA =
          plannedFormArray.filter((item, i) => i !== index) ?? [];
        if (endDate && compareOtherPA) {
          let valid = false;
          for (const item of compareOtherPA) {
            const comparePaDc = item.designComponentId;
            const comparePaEndDate = item.plannedCompletion
              ? new Date(item.plannedCompletion)
              : null;
            if (isReleaseDetailUnKnown) {
              if (comparePaEndDate?.getTime() === endDate?.getTime()) {
                valid = true;
                break;
              } else {
                valid = false;
                continue;
              }
            } else {
              if (
                comparePaDc === designComponentId &&
                comparePaEndDate?.getTime() === endDate?.getTime()
              ) {
                valid = true;
                break;
              } else {
                valid = false;
                continue;
              }
            }
          }
          if (valid) {
            rootStore.dispatch(
              setNotification({
                message: "Planned activity already exists with the same date.",
                notifyType: NotifyType.warning,
              })
            );
            return;
          }
        }
      }
    }

    const copyFormArray: PlannedActivityDtoUpdate[] = props.formArray
      ? props.formArray.map((el) => {
          return {
            ...el,
            opCoId: props.opcoId,
            designComponentFamilyId: props.designComponentFamilyId,
          };
        })
      : [];
    //  props.savePlan()
    if (formData !== null && formData !== undefined) {
      const selectedPA =
        formData?.plannedActivityResource &&
        dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).find((x) => x.key == formData?.plannedActivityResourceId);
      const copy = {
        ...formData,
        opCoId: props.opcoId,
        currency:
          formData?.budgetValue == null ||
          formData?.budgetValue == undefined ||
          formData?.budgetValue == ""
            ? undefined
            : formData?.currency,
        designComponentFamilyId: props.designComponentFamilyId,
        designComponentId:
          formData?.plannedActivityResource &&
          dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.plannedActivityResourceId ===
                formData?.plannedActivityResourceId &&
              x.value.ruleLinkedDc === 13
          ).length === 1
            ? props.designComponent?.key
            : props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                "in-commissioning" ||
              (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                "planned" &&
                (selectedPA?.value.ruleLinkedDc === 8 ||
                  selectedPA?.value.ruleLinkedDc === 12)) ||
              selectedPA?.value.ruleLinkedDc === 16
            ? props.designComponent?.["key"]
            : formData?.designComponentId,
      } as PlannedActivityDtoUpdate;
      setFormData(copy);
      if (validazioneClient(copy)?.response === true) {
        if (edit && index !== undefined) {
          if (isNoPA) {
            console.log("final", filteredProgram);
            copyFormArray[index] = {
              ...copy,
              opCoId: props.opcoId,
              // activityDetails: "",
              // activityDetailsText: "",
              // deliveryStatusId: 0,
              // activityStatusId: 0,
              // budgetAvailabilityId: 0,
              // benefitId: 0,
              // budgetTrackingId: "",
              // budgetValue: "",
              // deliveryStatusName: "",
              // deleted: false,
              // riskEngId: 0,
              // planningRiskId: 0,
              // riskOpeId: 0,
              // plannedImplementationYear: 0,
              // plannedActivityDescription: "",
              // deliveryStatusResource: {},
              // deliveryProjectName: "",
              // localApproval: "",
              // currency: "",
              // plannedCompletionValue: "",
              // projectStatus: "",
              // startDate: "",
              // startDateValue: "",
              // networkElementAsPlannedId: "",
              // designAspectId: "",
              // plannedImplementationYear: 0,
              // plannedActivityDescription: "",
              // activityDetails: "",
              // deliveryProjectName: "",
              // localApproval: "",
              // plannedCompletion: "",
              // plannedCompletionValue: "",
              // notes: "",
              // riskEngineeringNotes: "",
              // riskOperationalNotes: "",
              // spareFieldsJson: "",
              // deliveryProjectId: "",
              // isPAReleaseDetailUnknown: false,
              designComponentId: props.designComponent
                ? props.designComponent?.["key"]
                : 0,
              plannedDesignComponentName:
                resourceDesignComponent &&
                resourceDesignComponent.filter((x) =>
                  props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                    "in-commissioning" ||
                  (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                    "planned" &&
                    (selectedPA?.value.ruleLinkedDc === 8 ||
                      selectedPA?.value.ruleLinkedDc == 12)) ||
                  selectedPA?.value.ruleLinkedDc == 16
                    ? x.key === props?.designComponent?.["key"]
                    : x.key === copy.designComponentId
                )[0]?.value!,
              planningActivityStatusId: 0,
              designComponentFamilyId: props.designComponentFamilyId ?? 0,
              plannedActivityResourceId: formData?.plannedActivityResourceId,
              reasonForNoPlan: formData?.reasonForNoPlan,
              commentOnProjectStatus: formData?.commentOnProjectStatus,
              responsibilityPhaseId: 0,
              deliveryPlanAvailable: false,
              currency:
                formData?.budgetValue == null ||
                formData?.budgetValue == undefined ||
                formData?.budgetValue == ""
                  ? undefined
                  : formData?.currency,
              infraClusterClusterUpgradeUpsertDto: {
                plannedHardwareTypeId: (formData as any)
                  ?.infraClusterClusterUpgradeUpsertDto?.plannedHardwareTypeId,
                infraClusterAsPlannedDtoGrid:
                  (clusterData || filteredClusters) ?? [],
                nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
              },
            };

            props.action.AddOnList(copyFormArray);
          } else {
            props.isDesignAspect || props?.isFromNetworkElement
              ? (copyFormArray[index] = {
                  ...copy,
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                })
              : props.isInLcm
              ? (copyFormArray[index] = {
                  ...copy,
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                  plannedDesignComponentName:
                    formData?.isPAReleaseDetailUnknown && unknownDC
                      ? unknownDC.filter(
                          (x) => x.key === copy?.designComponentId
                        )[0]?.value!
                      : resourceDesignComponent &&
                        resourceDesignComponent.filter(
                          (x) => x.key === copy?.designComponentId
                        )[0]?.value!,

                  infraClusterClusterUpgradeUpsertDto: {
                    plannedHardwareTypeId: (formData as any)
                      ?.infraClusterClusterUpgradeUpsertDto
                      ?.plannedHardwareTypeId,
                    infraClusterAsPlannedDtoGrid:
                      (clusterData || filteredClusters) ?? [],
                    nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
                  },
                })
              : // copy?.designComponentId ?  x.key === copy?.designComponentId :
                (copyFormArray[index] = {
                  ...copy,
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                  infraClusterClusterUpgradeUpsertDto: {
                    plannedHardwareTypeId: (formData as any)
                      ?.infraClusterClusterUpgradeUpsertDto
                      ?.plannedHardwareTypeId,
                    infraClusterAsPlannedDtoGrid:
                      (clusterData || filteredClusters) ?? [],
                    nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
                  },
                });

            props.action.AddOnList(copyFormArray);
          }
        } else {
          // debugger
          if (isNoPA) {
            const newFormArray = {
              ...formData,
              opCoId: props.opcoId,
              // activityDetails: "",
              // activityDetailsText: "",
              // deliveryStatusId: 0,
              // activityStatusId: 0,
              // budgetAvailabilityId: 0,
              // activityStatusName: "",
              // benefitId: 0,
              // budgetTrackingId: "",
              // budgetValue: "",
              // deliveryStatusName: "",
              // originalLcmEngineeringId: null,
              // deleted: false,
              // riskEngId: 0,
              // planningRiskId: 0,
              // riskOpeId: 0,
              // plannedImplementationYear: 0,
              // plannedActivityDescription: "",
              // deliveryStatusResource: {},
              // deliveryProjectName: "",
              // localApproval: "",
              // currency: "",
              // plannedStartDate: undefined,
              // plannedCompletion: undefined,
              // plannedCompletionValue: "",
              // projectStatus: "",
              // startDate: "",
              // startDateValue: "",
              // archived: false,
              // isPAReleaseDetailUnknown: false,
              currency:
                formData?.budgetValue == null ||
                formData?.budgetValue == undefined ||
                formData?.budgetValue == ""
                  ? undefined
                  : formData?.currency,
              designComponentId: props.designComponent
                ? props.designComponent?.["key"]
                : 0,
              plannedDesignComponentName:
                resourceDesignComponent &&
                resourceDesignComponent.filter((x) =>
                  props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                    "in-commissioning" ||
                  props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                    "planned"
                    ? x.key === props?.designComponent?.["key"]
                    : x.key === copy.designComponentId
                )[0]?.value!,
              planningActivityStatusId: 0,
              designComponentFamilyId: props.designComponentFamilyId,
              plannedActivityResourceId: formData?.plannedActivityResourceId,
              reasonForNoPlan: formData?.reasonForNoPlan,
              commentOnProjectStatus: formData?.commentOnProjectStatus,
              responsibilityPhaseId: 0,
              deliveryPlanAvailable: false,
              infraClusterClusterUpgradeUpsertDto: {
                plannedHardwareTypeId: (formData as any)
                  ?.infraClusterClusterUpgradeUpsertDto?.plannedHardwareTypeId,
                infraClusterAsPlannedDtoGrid:
                  (clusterData || filteredClusters) ?? [],
                nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
              },
            } as PlannedActivityDtoUpdate;
            copyFormArray.push(newFormArray);

            props.action.AddOnList(copyFormArray);
          } else {
            const newItem = props.isDesignAspect
              ? {
                  ...copy,
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                }
              : {
                  ...copy,
                  infraClusterClusterUpgradeUpsertDto: {
                    plannedHardwareTypeId: (formData as any)
                      ?.infraClusterClusterUpgradeUpsertDto
                      ?.plannedHardwareTypeId,
                    infraClusterAsPlannedDtoGrid:
                      (clusterData || filteredClusters) ?? [],
                    nwElementClusterAsPlannedUpSertDto: filteredProgram ?? [],
                  },
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                  plannedDesignComponentName: !props.isInLcm
                    ? undefined
                    : formData?.isPAReleaseDetailUnknown && unknownDC
                    ? unknownDC.filter((x) =>
                        props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                          "in-commissioning" ||
                        props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                          "planned"
                          ? x.key === props?.designComponent?.["key"]
                          : x.key === copy.designComponentId
                      )[0]?.value!
                    : resourceDesignComponent &&
                      resourceDesignComponent.filter((x) =>
                        props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                          "in-commissioning" ||
                        (props.lcmDeploymentStatusString
                          ?.toLowerCase()
                          .trim() == "planned" &&
                          (selectedPA?.value.ruleLinkedDc === 8 ||
                            selectedPA?.value.ruleLinkedDc == 12))
                          ? x.key === props?.designComponent?.["key"]
                          : x.key === copy.designComponentId
                      )[0]?.value!,
                  designComponentId:
                    props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                      "in-commissioning" ||
                    (props.lcmDeploymentStatusString?.toLowerCase().trim() ==
                      "planned" &&
                      (selectedPA?.value.ruleLinkedDc === 8 ||
                        selectedPA?.value.ruleLinkedDc == 12))
                      ? props?.designComponent?.["key"]
                      : copy.designComponentId,
                  // plannedActivityResourceId: props.isInLcm ? copy.plannedActivityResourceId : undefined
                };

            const plannedActivityExist = copyFormArray?.filter(
              (item) =>
                item?.designComponentId === newItem?.designComponentId &&
                item?.plannedActivityResourceId ===
                  newItem?.plannedActivityResourceId
            );
            const checkPAunkownRelease = copyFormArray?.filter(
              (item) => item.isPAReleaseDetailUnknown
            );
            if (
              checkPAunkownRelease.length === 0 &&
              plannedActivityExist?.length > 0 &&
              !props.isReleaseDetailUnKnown
            ) {
              rootStore.dispatch(
                setNotification({
                  message: "Planned Activity Already Exist !",
                  notifyType: NotifyType.warning,
                })
              );
              return;
            }
            copyFormArray.push(newItem);

            props.action.AddOnList(copyFormArray);
          }
        }
      } else {
        // show warnning message in save planned activity  when validation error exist
        rootStore.dispatch(
          setNotification({
            message: "Check the fields entered",
            notifyType: NotifyType.warning,
          })
        );
        return;
      }
    }

    props.action.setShowForm(false);
    setChanged(false);
  };
  const createNewPA = () => {
    const getIndex =
      plannedFormArray?.length > 0
        ? plannedFormArray?.map((x, index) =>
            x?.plannedActivityResource &&
            x?.plannedActivityResourceId &&
            dictionaryToArrayPlannedActivityResourceDto(
              x?.plannedActivityResource
            ).filter(
              (res) =>
                res.value.plannedActivityResourceId ===
                  x?.plannedActivityResourceId && res.value.ruleLinkedDc === 15
            ).length === 1
              ? index
              : null
          )
        : [];
    let index =
      getIndex.length > 0 ? getIndex?.filter((x) => x !== null)[0] : null;
    const existNoPA =
      plannedFormArray?.length > 0 &&
      plannedFormArray.filter(
        (x) =>
          x?.plannedActivityResource &&
          x?.plannedActivityResourceId &&
          dictionaryToArrayPlannedActivityResourceDto(
            x?.plannedActivityResource
          ).filter(
            (res) =>
              res.value.plannedActivityResourceId ===
                x?.plannedActivityResourceId && res.value.ruleLinkedDc === 15
          ).length === 1
      ).length > 0;
    existNoPA && index !== null ? confirmNoPADelete(index) : createForm();
  };

  //LOOKUP-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const ActivityStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.activityStatusResource)
      formData.activityStatusResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const DeliveryStatusResourceRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.deliveryStatusResource)
      formData.deliveryStatusResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const PlanningActivityStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.planningActivityStatusResource)
      formData.planningActivityStatusResource = obj as {
        [key: string]: string;
      };
    setFormData(formData);
  };
  const ResponsibilityPhaseRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.responsibilityPhaseResource)
      formData.responsibilityPhaseResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const PlannedActivityResource = async (value: {
    [key: string]: PlannedActivityResourceDto;
  }) => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    copy.plannedActivityResource = value;
    setFormData(copy);
    const obj = dictionaryToArrayPlannedActivityResourceDto(value).filter(
      (item) => item.key === copy.plannedActivityResourceId
    );
    if (
      obj.length &&
      formData &&
      formData.plannedActivityResourceId !== undefined
    ) {
      onChangePlannedActivityResourceId(obj[0].value, copy);
    }
  };

  const BudgetAvailabilityRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.budgetAvaibilityResource)
      formData.budgetAvaibilityResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const OperationalRiskRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.riskResource)
      formData.riskResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const PlanningRiskRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.planningRiskResource)
      formData.planningRiskResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const DriverRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.driverResource)
      formData.driverResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const ProgramRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.programResource)
      formData.programResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const BenefitsRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.benefitResource)
      formData.benefitResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <ActivityStatusContainer
            returnObject={ActivityStatusRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></ActivityStatusContainer>
        );

      case 3:
        return (
          <PlanningActivityStatusContainer
            returnObject={PlanningActivityStatusRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></PlanningActivityStatusContainer>
        );
      case 4:
        return (
          <ResponsibilityPhaseContainer
            returnObject={ResponsibilityPhaseRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></ResponsibilityPhaseContainer>
        );
      case 6:
        return (
          <PlannedActivityResourceContainer
            isDesignAspect={props.isDesignAspect}
            isForAddAsset={props.isForAddAsset}
            isForEditAsset={props.isForEditAsset}
            returnObject={PlannedActivityResource}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></PlannedActivityResourceContainer>
        );
      case 7:
        return (
          <BudgetAvailabilityContainer
            returnObject={BudgetAvailabilityRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></BudgetAvailabilityContainer>
        );
      case 8:
        return (
          <OperationalRiskContainer
            returnObject={OperationalRiskRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OperationalRiskContainer>
        );
      case 9:
        return (
          <Driver
            returnObject={DriverRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></Driver>
        );
      case 10:
        return (
          <Benefits
            returnObject={BenefitsRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></Benefits>
        );
      case 11:
        return (
          <PlanningRisk
            returnObject={PlanningRiskRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></PlanningRisk>
        );
      case 12:
        return (
          <Program
            returnObject={ProgramRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></Program>
        );
      default:
        return;
    }
  };

  //#endregion

  //ONCHANGE-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const onChangeLocalApproval = (e: any) => {
    removeValidation("localApproval");
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (e && e["key"] !== undefined && e["key"] !== null && e["key"] !== "") {
      copy.localApproval = e["key"];
    } else {
      copy.localApproval = undefined;
    }

    setFormData(copy);
  };

  const [startDate, setStartDate] = useState<Date | undefined>();
  const [disableActivityApproved, setDisableActivityApproved] =
    useState<boolean>(false);

  const onChangeYears = async (property, date: Date) => {
    var event = { currentTarget: {} } as React.ChangeEvent<HTMLInputElement>;
    if (date && date != null) {
      event.currentTarget.value = date.getFullYear().toString();
      onChange(property, event);
      setStartDate(date);
    } else {
      setStartDate(date);
      event.currentTarget.value = "";
      onChange(property, event);
    }
  };

  // useEffect(() => {
  // 	if (formData && formData.plannedImplementationYear != null && formData.plannedImplementationYear != 0 && formData.plannedImplementationYear != undefined) {
  // 		CheckFiscalYear(formData.plannedImplementationYear).then(x => {
  // 			let copy = { ...formData } as PlannedActivityDtoUpdate
  // 			if (x == false) {
  // 				if (copy.budgetAvaibilityResource != undefined) {
  // 					copy.budgetAvailabilityId = dictionaryToArray(copy.budgetAvaibilityResource).find(x => x.value.toLowerCase() == "no")?.key
  // 					setDisableActivityApproved(true)
  // 				} else {
  // 					setDisableActivityApproved(false)
  // 				}
  // 				setFormData(copy)
  // 			} else {
  // 				setDisableActivityApproved(false)
  // 				// copy.budgetAvailabilityId = undefined;
  // 				setFormData(copy)
  // 			}
  // 		})
  // 	}
  // }, [formData?.plannedImplementationYear]);

  useEffect(() => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (
      formData &&
      formData?.deliveryStatusResource &&
      formData?.deliveryStatusId &&
      formData?.deliveryStatusResource[formData?.deliveryStatusId!] !==
        "Mobilization" &&
      formData?.deliveryStatusResource[formData?.deliveryStatusId!] !==
        "Delivery Planning" &&
      formData?.deliveryStatusResource[formData?.deliveryStatusId!] !==
        "Budget Planning"
    ) {
      setDisabledInitialFunds(true);
      copy["localApproval"] = "YES";
    } else {
      setDisabledInitialFunds(false);
    }

    setFormData(copy);
  }, [formData?.deliveryStatusId]);

  const onChangeLinkedPlannedActivity = (e) => {
    removeValidation("linkedToPlannedActivityId");
    const copy = { ...formData } as PlannedActivityDtoUpdate;
    copy.linkedToPlannedActivityId = e.key;
    setFormData(copy);
  };

  //#endregion

  //MANAGE MIGRATION / UPGRADE STATUS-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const [isVisibleModalManage, setIsVisibleModalManage] =
    useState<boolean>(false);
  const [plannedActivityIdToManage, setPlannedActivityIdToManage] =
    useState<number>();

  const getManageMigrationData = async (id: number, idx) => {
    if (!props.changedLcm && !changed) {
      setPlannedActivityIdToManage(id);
      setIsVisibleModalManage(true);
    } else {
      props.action.setConfirm({
        title: "Confirm",
        message:
          "There are pending changes in the views of lcm and planned activity, they will be lost as you continue, are you sure you want to continue?",
        button: "Continue",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => props.action.setConfirm(stateConfirm),
          confirm: () => {
            setPlannedActivityIdToManage(id);
            setIsVisibleModalManage(true);
            props.action.setConfirm(stateConfirm);
          },
        },
      });
    }
  };

  const openModalStatus = (id: number) => {
    // debugger;
    if (!props.changedLcm && !changed) {
      setPlannedActivityIdToManage(id);
      setIsVisibleModalStatus(true);
    } else {
      props.action.setConfirm({
        title: "Confirm",
        message:
          "There are pending changes in the views of lcm and planned activity, they will be lost as you continue, are you sure you want to continue?",
        button: "Continue",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => props.action.setConfirm(stateConfirm),
          confirm: () => {
            setPlannedActivityIdToManage(id);
            setIsVisibleModalStatus(true);
            props.action.setConfirm(stateConfirm);
          },
        },
      });
    }
  };
  //#endregion

  //RTN-----------------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const rtnPlannedIdWithPadding = (id: number) => {
    let lenght = id.toString().length;
    let padding = 6 - lenght;
    let name = "AI";
    for (let i = 0; i < padding; i++) {
      name += "0";
    }
    name += id.toString();
    return name;
  };

  const formatGroupLabel = (data) => (
    <div>
      <span>{data.label}</span>
    </div>
  );

  function findWithAttr(array: any[], attr: string, value) {
    for (var i = 0; i < array.length; i += 1) {
      if (array[i][attr] === parseInt(value)) {
        return i;
      }
    }
    return -1;
  }

  //#endregion

  //LINKED LCM PLANNED ACTIVITY
  //#region
  const [isVisibleLinkedLCM, setIsVisibleLinkedLCM] = useState(false);
  const [
    linkedLCMPlannedActivityResource,
    setLinkedLCMPlannedActivityResource,
  ] = useState<Array<{ key: number; value: PlannedActivityToConnectData }>>();

  useEffect(() => {
    const plannedActivitySelected =
      formData?.plannedActivityResource &&
      dictionaryToArrayPlannedActivityResourceDto(
        formData?.plannedActivityResource
      ).find((x) => x.key === formData?.plannedActivityResourceId);
    if (
      (plannedActivitySelected?.value.ruleNetworkElement === 2 &&
        formData?.isReplacementExistingSolution) ||
      plannedActivitySelected?.value.ruleNetworkElement === 3
    ) {
      getLinkedLcmPlannedActivities();
    } else {
      setIsVisibleLinkedLCM(false);
    }
  }, [
    formData?.plannedActivityResourceId,
    formData?.isReplacementExistingSolution,
  ]);

  const getLinkedLcmPlannedActivities = () => {
    if (props?.designComponent?.key && props.opcoId) {
      GetLcmEngineeringPlannedActivity(
        props?.designComponent.key,
        props.opcoId
      ).then((result) => {
        setIsVisibleLinkedLCM(true);
        const listOfLinkedPlanned =
          result &&
          (dictionaryToArrayLinkedLCMPLannedActivities(result) as {
            key: number;
            value: PlannedActivityToConnectData;
          }[]);
        if (listOfLinkedPlanned?.length === 1) {
          //se trovi solo 1 linked planned selezionala in automatico
          const copy = { ...formData } as PlannedActivityDtoUpdate;
          copy.linkedToPlannedActivityId = listOfLinkedPlanned[0].key;
          setFormData(copy);
        }
        setLinkedLCMPlannedActivityResource(listOfLinkedPlanned);
      });
    }
  };
  //#endregion

  //USE-EFFECT FOR REMOVE VALIDATION--------------------------------------------------------------------------------------
  //#region
  useEffect(() => {
    removeValidation("activityDetails");
  }, [formData?.activityDetails]);

  useEffect(() => {
    removeValidation("activityStatusId");
  }, [formData?.activityStatusId]);

  const removeValidation = (property: string) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };
  //#endregion

  //DRIVER / BENEFIT / PLANNING RISK
  //#region
  const [driverOption, setDriverOption] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [benefitsOption, setBenefitsOption] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [planningRiskOption, setPlanningRiskOption] = useState<
    { key: number; value: string }[] | undefined
  >();

  useEffect(() => {
    let copy = { ...formData } as PlannedActivityDtoCreate;
    let optionsForBenefits = {} as { key: number; value: string }[] | undefined;
    if (props.isForAddAsset) {
      if (
        plannedResourceSelected?.benefitTextAddAsset &&
        plannedResourceSelected?.benefitTextAddAsset.length > 0
      ) {
        optionsForBenefits =
          formData?.benefitResource &&
          dictionaryToArray(formData?.benefitResource).filter((el) =>
            plannedResourceSelected?.benefitTextAddAsset?.includes(el.key)
          );
        if (optionsForBenefits?.length === 1) {
          copy.benefitId = optionsForBenefits[0].key;
        }
      } else {
        optionsForBenefits = [];
      }
    } else if (props.isForEditAsset) {
      if (
        plannedResourceSelected?.benefitTextEditAsset &&
        plannedResourceSelected?.benefitTextEditAsset.length > 0
      ) {
        optionsForBenefits =
          formData?.benefitResource &&
          dictionaryToArray(formData?.benefitResource).filter((el) =>
            plannedResourceSelected?.benefitTextEditAsset?.includes(el.key)
          );
        if (optionsForBenefits?.length === 1) {
          copy.benefitId = optionsForBenefits[0].key;
        }
      } else {
        optionsForBenefits = [];
      }
    } else if (props.isDesignAspect) {
      if (
        plannedResourceSelected?.benefitTextDesignAspect &&
        plannedResourceSelected?.benefitTextDesignAspect.length > 0
      ) {
        optionsForBenefits =
          formData?.benefitResource &&
          dictionaryToArray(formData?.benefitResource).filter((el) =>
            plannedResourceSelected?.benefitTextDesignAspect?.includes(el.key)
          );
        if (optionsForBenefits?.length === 1) {
          copy.benefitId = optionsForBenefits[0].key;
        }
      } else {
        optionsForBenefits = [];
        // optionsForBenefits = formData?.benefitResource && dictionaryToArray(formData?.benefitResource);
      }
    } else {
      if (
        plannedResourceSelected?.benefitTextLcm &&
        plannedResourceSelected?.benefitTextLcm.length > 0
      ) {
        optionsForBenefits =
          formData?.benefitResource &&
          dictionaryToArray(formData?.benefitResource).filter((el) =>
            plannedResourceSelected?.benefitTextLcm?.includes(el.key)
          );
        if (optionsForBenefits?.length === 1) {
          copy.benefitId = optionsForBenefits[0].key;
        }
      } else {
        optionsForBenefits = [];
        // optionsForBenefits = formData?.benefitResource && dictionaryToArray(formData?.benefitResource);
      }
    }
    setBenefitsOption(optionsForBenefits);

    let optionsForPlanningRisk = {} as
      | { key: number; value: string }[]
      | undefined;
    if (props.isForAddAsset) {
      if (
        plannedResourceSelected?.planningRisksAddAsset &&
        plannedResourceSelected?.planningRisksAddAsset.length > 0
      ) {
        optionsForPlanningRisk =
          formData?.planningRiskResource &&
          dictionaryToArray(formData?.planningRiskResource).filter((el) =>
            plannedResourceSelected?.planningRisksAddAsset?.includes(el.key)
          );
        if (optionsForPlanningRisk?.length === 1) {
          copy.planningRiskId = optionsForPlanningRisk[0].key;
        }
      } else {
        optionsForPlanningRisk = [];
      }
    } else if (props.isForEditAsset) {
      if (
        plannedResourceSelected?.planningRisksAEditAsset &&
        plannedResourceSelected?.planningRisksAEditAsset.length > 0
      ) {
        optionsForPlanningRisk =
          formData?.planningRiskResource &&
          dictionaryToArray(formData?.planningRiskResource).filter((el) =>
            plannedResourceSelected?.planningRisksAEditAsset?.includes(el.key)
          );
        if (optionsForPlanningRisk?.length === 1) {
          copy.planningRiskId = optionsForPlanningRisk[0].key;
        }
      } else {
        optionsForPlanningRisk = [];
        // optionsForPlanningRisk = formData?.planningRiskResource && dictionaryToArray(formData?.planningRiskResource);
      }
    } else if (props.isDesignAspect) {
      if (
        plannedResourceSelected?.planningRisksDesignAspect &&
        plannedResourceSelected?.planningRisksDesignAspect.length > 0
      ) {
        optionsForPlanningRisk =
          formData?.planningRiskResource &&
          dictionaryToArray(formData?.planningRiskResource).filter((el) =>
            plannedResourceSelected?.planningRisksDesignAspect?.includes(el.key)
          );
        if (optionsForPlanningRisk?.length === 1) {
          copy.planningRiskId = optionsForPlanningRisk[0].key;
        }
      } else {
        optionsForPlanningRisk = [];
        // optionsForPlanningRisk = formData?.planningRiskResource && dictionaryToArray(formData?.planningRiskResource);
      }
    } else {
      if (
        plannedResourceSelected?.planningRisksLcm &&
        plannedResourceSelected?.planningRisksLcm.length > 0
      ) {
        optionsForPlanningRisk =
          formData?.planningRiskResource &&
          dictionaryToArray(formData?.planningRiskResource).filter((el) =>
            plannedResourceSelected?.planningRisksLcm?.includes(el.key)
          );
        if (optionsForPlanningRisk?.length === 1) {
          copy.planningRiskId = optionsForPlanningRisk[0].key;
        }
      } else {
        optionsForPlanningRisk = [];
        // optionsForPlanningRisk = formData?.planningRiskResource && dictionaryToArray(formData?.planningRiskResource);
      }
    }
    setPlanningRiskOption(optionsForPlanningRisk);

    let optionsForDriver = {} as { key: number; value: string }[] | undefined;
    if (props.isDesignAspect) {
      if (
        plannedResourceSelected?.driverTextDesignAspect &&
        plannedResourceSelected?.driverTextDesignAspect.length > 0
      ) {
        optionsForDriver =
          formData?.driverResource &&
          dictionaryToArray(formData?.driverResource).filter((el) =>
            plannedResourceSelected?.driverTextDesignAspect?.includes(el.key)
          );
        if (optionsForDriver?.length === 1) {
          copy.driverId = optionsForDriver[0].key;
        }
      } else {
        optionsForDriver = [];
        // optionsForDriver = formData?.driverResource && dictionaryToArray(formData?.driverResource);
      }
    } else if (props.isForAddAsset) {
      if (
        plannedResourceSelected?.driverTextAddAsset &&
        plannedResourceSelected?.driverTextAddAsset.length > 0
      ) {
        optionsForDriver =
          formData?.driverResource &&
          dictionaryToArray(formData?.driverResource).filter((el) =>
            plannedResourceSelected?.driverTextAddAsset?.includes(el.key)
          );
        if (optionsForDriver?.length === 1) {
          copy.driverId = optionsForDriver[0].key;
        }
      } else {
        optionsForDriver = [];
      }
    } else if (props.isForEditAsset) {
      if (
        plannedResourceSelected?.driverTextEditAsset &&
        plannedResourceSelected?.driverTextEditAsset.length > 0
      ) {
        optionsForDriver =
          formData?.driverResource &&
          dictionaryToArray(formData?.driverResource).filter((el) =>
            plannedResourceSelected?.driverTextEditAsset?.includes(el.key)
          );
        if (optionsForDriver?.length === 1) {
          copy.driverId = optionsForDriver[0].key;
        }
      } else {
        optionsForDriver = [];
      }
    } else {
      if (
        plannedResourceSelected?.driverTextLcm &&
        plannedResourceSelected?.driverTextLcm.length > 0
      ) {
        optionsForDriver =
          formData?.driverResource &&
          dictionaryToArray(formData?.driverResource).filter((el) =>
            plannedResourceSelected?.driverTextLcm?.includes(el.key)
          );
        if (optionsForDriver?.length === 1) {
          copy.driverId = optionsForDriver[0].key;
        }
      } else {
        optionsForDriver = [];
        // optionsForDriver = formData?.driverResource && dictionaryToArray(formData?.driverResource);
      }
    }
    if (
      formData?.plannedActivityResourceId !== undefined &&
      plannedResourceSelected?.ruleLinkedDc === 34
    ) {
      InfraClusterPaLevelApiCall();
    }
    if (
      formData?.plannedActivityResourceId !== undefined &&
      plannedResourceSelected?.ruleLinkedDc === 36
    ) {
      InfraUpgradeClusterPaLevelApiCall();
    }
    if (
      formData?.plannedActivityResourceId !== undefined &&
      plannedResourceSelected?.ruleLinkedDc === 35
    ) {
      InfraClusterPaProgramLevelGetApiCall();
    }

    if (plannedResourceSelected?.ruleActicvityDetails === 5) {
      setActivityDetailsManual(true);
    } else {
      setActivityDetailsManual(false);
    }
    setDriverOption(optionsForDriver);
    setFormData(copy);
  }, [plannedResourceSelected || formData?.plannedActivityResourceId]);

  //#endregion

  //EXTERNAL FUNCTIONS
  //#region
  const ManageMigrationChangeNodes = (newNumberOfNode: number) => {
    if (props.action.changeNumberOfNodes) {
      props.action.changeNumberOfNodes(newNumberOfNode);
    }
  };

  const changeNodesFromActivityStatus = (NodesInProd: number, NodesInLab) => {
    if (props.action.changeNumberOfNodes) {
      props.action.changeNumberOfNodes(NodesInProd, NodesInLab);
    }
  };

  const updateActivityStatus = (id: number) => {
    const copy = { ...formData } as PlannedActivityDtoUpdate;
    copy.deliveryStatusId = id;
    setFormData(copy);
  };

  const updateDeliveryStatus = (
    plannedId: number,
    deliveryStatusId: number
  ) => {
    let copy = [...plannedFormArray] as PlannedActivityDtoUpdate[];
    let index = copy.findIndex((x) => x.plannedActivityId == plannedId);
    if (index != undefined && index != -1) {
      copy[index].deliveryStatusId = deliveryStatusId;
    }
    if (edit && formData?.plannedActivityId == plannedId) {
      let copyFormData = { ...formData } as PlannedActivityDtoUpdate;
      copyFormData.deliveryStatusId = deliveryStatusId;
      setFormData(copyFormData);
    }
    setPlannedFormArray(copy);
  };
  //#endregion
  useEffect(() => {
    mapPlannedResource();
  }, [props.lcmDeploymentStatusString, props.both]);

  const onSelectNode = (checked: boolean, obj: NetworkElementAssociated) => {
    const newArr = newNetworkAssocitate?.map((item) => {
      const keys1 = Object.keys(item);
      const keys2 = Object.keys(obj);

      // Check if both objects have the "id" field
      const hasIdField = keys1.includes("id") && keys2.includes("id");

      if (hasIdField && item.id === obj.id) {
        return { ...item, isFinalAsset: checked };
      } else if (
        !hasIdField &&
        keys1.length === keys2.length &&
        keys1.every((key) => item[key] === obj[key])
      ) {
        return { ...item, isFinalAsset: checked };
      } else {
        return item;
      }
    });

    setNewNetworkAssocitate(newArr);
    if (props.action.updateProdNodes && newArr) {
      props.action.updateProdNodes(newArr);
    }
  };

  const onDateCheck = (newRange) => {
    const dateRange =
      plannedFormArray &&
      plannedFormArray.map((el) => {
        if (el.startDate && el.plannedCompletion) {
          const startDateObj = new Date(el.startDate);
          const endDateObj = new Date(el.plannedCompletion);
          const formattedStartDate = `${startDateObj.getDate()}/${
            startDateObj.getMonth() + 1
          }/${startDateObj.getFullYear()}`;
          const formattedEndDate = `${endDateObj.getDate()}/${
            endDateObj.getMonth() + 1
          }/${endDateObj.getFullYear()}`;
          return { start: formattedStartDate, end: formattedEndDate };
        }
      });
    const newRangeList = dateRange.filter((el, i) => i !== index);
    if (newRangeList?.length > 0 && !isNoPA) {
      const isOverlapping = isDateRangeOverlapping(newRangeList, newRange);
      setIsDateRange(isOverlapping ? true : false);
      return isOverlapping ? true : false;
    }
  };
  const formateDateString = (dateStr) => {
    const [day, month, year] = dateStr.split("/");
    return `${year}-${month}-${day}`;
  };
  const validateGivenObject = (periods, givenObject) => {
    const parsedPeriods = periods.map((period) => ({
      start: formateDateString(period.start),
      end: formateDateString(period.end),
    }));

    const givenStart = new Date(formateDateString(givenObject.start));
    const givenEnd = new Date(formateDateString(givenObject.end));
    const currentIndex = index ?? -1;
    let isValid = true;
    // parsedPeriods.forEach((item)=>{
    //   if(givenStart > new Date(item.end)){
    //     isValid = true;
    //   }
    //   else if(givenEnd < new Date(item.start)) {
    //     isValid = true
    //   } else {
    //     return isValid = false
    //   }
    // })

    for (const item of parsedPeriods) {
      const itemStart = new Date(item.start);
      const itemEnd = new Date(item.end);

      if (givenStart > itemEnd || givenEnd < itemStart) {
        isValid = true;
        continue;
      } else {
        isValid = false;
        break;
      }
    }

    return isValid;
  };
  const validateStartEndDate = (newRange) => {
    const dateRange =
      plannedFormArray &&
      plannedFormArray
        .filter((item, i) => i !== index)
        .map((el) => {
          if (el.startDate && el.plannedCompletion) {
            const startDateObj =
              el?.startDate === null || el?.startDate === ""
                ? new Date()
                : new Date(el.startDate);
            const endDateObj = new Date(el.plannedCompletion);
            const formattedStartDate = `${startDateObj.getDate()}/${
              startDateObj.getMonth() + 1
            }/${startDateObj.getFullYear()}`;
            const formattedEndDate = `${endDateObj.getDate()}/${
              endDateObj.getMonth() + 1
            }/${endDateObj.getFullYear()}`;
            return { start: formattedStartDate, end: formattedEndDate };
          }
        });
    let isValid =
      dateRange.length > 0 ? validateGivenObject(dateRange, newRange) : true;
    return isValid;
  };

  const isPADisabled =
    (formData?.isPAReleaseDetailUnknown && ruleDCIdFlag) ||
    (formData?.plannedActivityResourceId === undefined &&
      formData?.plannedActivityResourceId === null) ||
    props.selectedDeploymentStatus?.readOnlyPlannedActivity ||
    props.formDisabed ||
    (props.lcmDeploymentStatusString?.toLocaleLowerCase().trim() ===
      "in-commissioning" &&
      edit) ||
    (props.lcmDeploymentStatusString?.toLocaleLowerCase().trim() ===
      "planned" &&
      edit) ||
    edit ||
    (formData?.plannedActivityResource &&
      dictionaryToArrayPlannedActivityResourceDto(
        formData?.plannedActivityResource
      ).filter(
        (x) =>
          x.value.plannedActivityResourceId ===
            formData?.plannedActivityResourceId &&
          (x.value.ruleLinkedDc === 8 ||
            x.value.ruleLinkedDc === 12 ||
            x.value.ruleLinkedDc === 13 ||
            x.value.ruleLinkedDc === 16 ||
            x.value.ruleLinkedDc === 34 ||
            x.value.ruleLinkedDc === 36 ||
            x.value.ruleLinkedDc === 35)
      ).length === 1);

  const removeUnknownState = () => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    copy.activityDetails = "";
    copy.activityDetailsText = "";
    copy.designComponentId = undefined;
    copy.buildBagId = undefined;
    copy.deliveryPlanAvailable = true;
    setFormData(copy);
    setUnknownDC(undefined);
  };

  const onChangePAState = (e) => {
    if (e) {
      if (e.target.checked === true) {
        setBuildBagRes(props?.buildBagResources);
      }
      setFormData((prev) => ({
        ...prev,
        buildBagId:
          e.target.checked === true ? props?.buildBagIds : prev?.buildBagId,
        deliveryPlanAvailable: e.target.checked === true ? false : true,
        activityDetails: e.target.checked ? prev?.activityDetails : "",
        activityDetailsText: e.target.checked ? prev?.activityDetailsText : "",
      }));
    }
  };

  useEffect(() => {
    if (
      plannedResourceSelected &&
      edit === false &&
      !props.isReleaseDetailUnKnown &&
      props.lcmDeploymentStatusString?.toLocaleLowerCase().trim() ===
        "in-service"
    ) {
      if (
        plannedResourceSelected?.ruleLinkedDc === 1 ||
        plannedResourceSelected?.ruleLinkedDc === 5 ||
        plannedResourceSelected?.ruleLinkedDc === 10
      ) {
        setFormData((prev) => ({ ...prev, isPAReleaseDetailUnknown: false }));
        setRuleDCIdFlag(true);
      } else {
        if (
          plannedResourceSelected?.ruleLinkedDc == 13 ||
          plannedResourceSelected?.ruleLinkedDc === 34 ||
          plannedResourceSelected?.ruleLinkedDc === 36 ||
          plannedResourceSelected?.ruleLinkedDc === 35
        ) {
          setRuleDCIdFlag(false);
        } else {
          setRuleDCIdFlag(false);
          removeUnknownState();
        }
      }
    }
  }, [plannedResourceSelected]);

  useEffect(() => {
    if (ruleDCIdFlag && formData && formData.isPAReleaseDetailUnknown) {
      let copy = { ...formData } as PlannedActivityDtoUpdate;
      copy.activityDetails = `Release Unknown - ${
        moment(formData.plannedCompletion).format("DD/MM/YYYY") ??
        "Not Specified"
      }`;
      copy.activityDetailsText = `Release Unknown - ${
        moment(formData.plannedCompletion).format("DD/MM/YYYY") ??
        "Not Specified"
      }`;
      setFormData(copy);
    }
  }, [formData?.plannedCompletion]);

  const getUnknownDC = async () => {
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    if (props.designComponent?.key != undefined) {
      await GetCreateUnkownDCPALevel(props.designComponent?.key).then(
        (c: any) => {
          if (c !== null && c !== undefined) {
            const list = dictionaryToArray(c);
            if (
              list &&
              ruleDCIdFlag &&
              formData &&
              formData.isPAReleaseDetailUnknown
            ) {
              copy.deliveryPlanAvailable = false;
              copy.designComponentId = list[0]?.key;
              copy.plannedDesignComponentName = list[0]?.value;
              copy.activityDetails = `Release Unknown - ${
                formData.plannedCompletion
                  ? formData.plannedCompletion
                  : "Not Specified"
              }`;
              copy.activityDetailsText = `Release Unknown - ${
                formData.plannedCompletion
                  ? formData.plannedCompletion
                  : "Not Specified"
              }`;
              setFormData(copy);
              setUnknownDC(list);
            }
          }
        }
      );
    }
  };

  useEffect(() => {
    if (
      formData &&
      formData?.isPAReleaseDetailUnknown &&
      edit === false &&
      !props.isReleaseDetailUnKnown
    ) {
      getUnknownDC();
    }
  }, [formData?.isPAReleaseDetailUnknown]);

  useEffect(() => {
    if (
      formData &&
      (formData.plannedActivityId === null ||
        formData.plannedActivityId === undefined)
    ) {
      setRuleDCIdFlag(false);
    }
  }, [formData?.plannedActivityId]);

  useEffect(() => {
    const isDcRule16 = plannedResourceSelected?.ruleLinkedDc === 16;
    if (
      props.buildBagIds ||
      isPADisabled ||
      isNoPA ||
      props.lcmDeploymentStatusString?.toLocaleLowerCase().trim() !==
        "in-service"
    ) {
      setFormData((prev) => ({
        ...prev,
        buildBagId: edit ? formData?.buildBagId : props.buildBagIds,
      }));
    } else {
      setFormData((prev) => ({
        ...prev,
        buildBagId: edit ? formData?.buildBagId : undefined,
      }));
    }
    if (isDcRule16 && !edit && props.isInLcm) {
      setFormData((prev) => ({
        ...prev,
        buildBagId: undefined, // reset dropdown value
      }));
    }

    if (!(isPADisabled || isNoPA) && props.isInLcm) {
      setFormData((prev) => ({
        ...prev,
        buildBagId: undefined,
      }));
      setBuildBagRes([]);
    }
  }, [
    props.buildBagIds,
    props.isInLcm,
    isPADisabled,
    isNoPA,
    plannedResourceSelected?.ruleLinkedDc,
  ]);

  const InfraClusterPaLevelApiCall = async () => {
    const payload = {
      ...infraClusterQuery,
      opCoId: props?.opcoId ? [props?.opcoId] : [],
      paId: edit ? formData?.plannedActivityId : 0,
    } as InfraClusterDtoUpdate;
    const result = await InfraClusterPaLevelGet(payload);
    setClusterDataResource(result?.InfraClusterGridResult?.items ?? []);
    setClusterData(result?.InfraClusterGridResult?.items ?? []);
    InfraClusterResource({
      opCoId: props?.opcoId,
      DcId: props?.designComponent?.key,
    });
    // console.log("Result", result);
  };

  const InfraUpgradeClusterPaLevelApiCall = async () => {
    const payload = {
      ...infraClusterQuery,
      opCoId: props?.opcoId ? [props?.opcoId] : [],
      paId: edit ? formData?.plannedActivityId : 0,
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
      opCoId: props?.opcoId,
      DcId: props?.designComponent?.key,
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
  const filteredClusters = formData?.plannedActivityId
    ? clusterUpgardeData
    : selectedSite
    ? clusterUpgardeData.filter((cluster: any) => cluster.site === selectedSite)
    : [];
  useEffect(() => {
    if (formData?.plannedActivityId && clusterUpgardeData?.length > 0) {
      const site = clusterUpgardeData[0]?.site;
      setSelectedSite(site);

      setFormData((prev: any) => ({
        ...prev,
        siteId: site,
      }));
    }
  }, [clusterUpgardeData]);

  //Cluster Program//
  const InfraClusterPaProgramLevelGetApiCall = async () => {
    const payload = {
      ...infraClusterQuery,
      opCoId: props?.opcoId ? [props?.opcoId] : [],
      paId: edit ? formData?.plannedActivityId : 0,
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
      opCoId: props?.opcoId,
      DcId: props?.designComponent?.key,
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
  //end cluster Programm//

  const removeClusterItem = (i: number) => {
    // let copy = { ...formData } as InfraClusterDtoUpdate;
    // copy.networkElementAssociateds?.splice(i, 1);
    // let updatedNetworkElementAssociatedid = networkElementAssociateds;
    // updatedNetworkElementAssociatedid =
    //   updatedNetworkElementAssociatedid.filter((item, index) => index !== i);
    // setNetworkElementAssociateds(updatedNetworkElementAssociatedid);
    // setFormData(copy);
  };
  const handleRowSelect = (index) => {
    setSelectedRows((prev: any) =>
      prev.includes(index) ? prev.filter((i) => i !== index) : [...prev, index]
    );
  };

  // const handleSelectAll = (checked) => {
  //   if (checked) {
  //     const allIndexes = clusterUpgardeData.map((_, i) => i);
  //     setSelectedRows(allIndexes);
  //   } else {
  //     setSelectedRows([]);
  //   }
  // };

  useEffect(() => {
    const designComponentIdToUse =
      formData?.designComponentId ?? props.designComponent?.key;
    if (designComponentIdToUse && props.opcoId && props.buildBagIds) {
      GetNetworkElementOpCo({
        dcId: designComponentIdToUse,
        opCoId: props.opcoId,
        lcmBagId: props.buildBagIds,
      }).then((x) => {
        if (x && Array.isArray(x)) {
          setBuildBagRes(x);
        } else {
          setBuildBagRes([]);
        }
      });
    } else {
      setBuildBagRes([]);
    }
  }, [
    formData?.designComponentId,
    props.opcoId,
    props.buildBagIds,
    props.designComponent?.key,
    isNoPA,
  ]);

  const excludeDates = () => {
    let newList = plannedFormArray.map((item) => {
      if (item && item.startDate && item.plannedCompletion) {
        return {
          start: moment(item.startDate).format("YYYY-MM-DD"),
          end: moment(item.plannedCompletion).format("YYYY-MM-DD"),
        };
      }
    });
    return newList ?? null;
  };

  const minDate = () => {
    if (!props.isReleaseDetailUnKnown) {
      return new Date();
    } else return null;
  };

  const maxDate = () => {
    let newDate = new Date();
    if (!props.isReleaseDetailUnKnown) {
      let maxDate = formData?.plannedImplementationYear;
      return newDate;
    }
    return null;
  };

  const currentYear = new Date().getFullYear();

  const handleDateChange = (newDate, e) => {
    e.preventDefault();
    setStartDate(newDate);
    updateStartEndDate();
    setStartEndFlag(true);
    onChangeYears("plannedImplementationYear", newDate);
    props.action.setChanged(true);
  };

  const updateStartEndDate = () => {
    if (formData && formData.plannedImplementationYear) {
      let copy = { ...formData } as PlannedActivityDtoUpdate;
      const year: any = Number(formData.plannedImplementationYear);
      const currentYear = new Date().getFullYear();
      copy.startDate = new Date();
      let endDate = new Date(`${year}/${3}/${1}`);
      copy.plannedCompletion = `${endDate.getFullYear()}/${
        endDate.getMonth() + 1
      }/${endDate.getDate()}` as any;
      if (currentYear !== year - 1) {
        copy.startDate = `${year - 1}/${4}/${1}`;
        setFormData(copy);
      }
      setFormData(copy);

      setStartEndFlag(false);
    }
  };

  const getFiscalYearFormat = (date) => {
    const fiscalStartYear = (date - 1) % 100;
    const fiscalEndYear = date % 100;
    return `FY${fiscalEndYear}: Apr '${fiscalStartYear} - Mar '${fiscalEndYear}`;
  };

  useEffect(() => {
    startEndFlag && updateStartEndDate();
  }, [startEndFlag]);

  useEffect(() => {
    if (formData && formData.plannedImplementationYear) {
      const formattedDate = getFiscalYearFormat(
        formData.plannedImplementationYear
      );
      setCustomDateFormat(formattedDate);
    }
  }, [formData?.plannedImplementationYear]);

  function getFinancialYear() {
    const date = new Date();
    const currentYear = date.getFullYear();
    const currentMonth = date.getMonth();

    const financialYearEnd = currentMonth >= 3 ? currentYear + 1 : currentYear;
    return new Date(String(financialYearEnd));
  }

  const BuildBagItemRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetPlannedActivityCreateResourceRefill({
        dcId: props?.designComponent?.["key"],
        isRefillData: true,
      });
      if (formData) {
        var obj: any = res?.PlannedActivityDtoCreate?.buildBagResources
          ? res?.PlannedActivityDtoCreate?.buildBagResources
          : [];

        setFormData({
          ...formData,
          buildBagResources: obj,
        });
      }
    } catch (error) {
      console.error("Error in BuildBagItemRefillData:", error);
    }
  };
  // console.log("Form", plannedFormArray);

  const onSaveCluster = (data) => {
    // console.log("Data", data, formData, plannedFormArray);

    setClusterData([...clusterData, data]);
  };

  const getInfraClusterAsPlannedId = () => {
    const cluster = clusterDropdown?.find((c) => c.key === selectedCluster);
    return cluster?.infraClusterAsPlannedId;
  };

  const onSaveClusterProgram = (data) => {
    const newCluster = {
      ...data,
      isNew: true,
      clusterName: selectedCluster,
      infraClusterAsPlannedId: getInfraClusterAsPlannedId(),
    };
    console.log("saveclusterData", newCluster);
    setClusterProgramData((prev) => [...prev, newCluster]);
  };
  const isInService = (statusValue: string) =>
    statusValue?.replace(/-/g, "").toLowerCase() === "inservice";

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => setIsVisibleModalLookup(0)}
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
      <Dialog
        open={isBuildBagItemFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setIsBuildBagItemFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <IconButton
          aria-label="close"
          onClick={() => setIsBuildBagItemFlag(false)}
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
          <BuildBagItemComponent
            redirect={"bagScreen"}
            returnObject={BuildBagItemRefillData}
            modal={{ isModal: true, setIsBuildBagItemFlag }}
          />
        </DialogContent>
      </Dialog>

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

      <Dialog
        open={viewBagFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setViewBagFlag(false);
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
          <div className="col-12 px-0 mt-1">
            <h4>View Software Component</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setViewBagFlag(false)}
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
          <ViewMappedComponent
            redirect={
              props.isForAddAsset || props.isForEditAsset
                ? "asset"
                : props.isInLcm
                ? "lcmPA"
                : ""
            }
            bagId={formData?.buildBagId}
            buildBagResources={formData?.buildBagResources}
            modal={{ isModal: true, setViewBagFlag }}
          />
        </DialogContent>
      </Dialog>
      <Modal
        show={isVisibleModalManage}
        backdrop="static"
        keyboard={false}
        size="xl"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Manage Migration</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <ManageMigration
            isFromPlannedActivityModal={false}
            lcmId={props.lcmId}
            action={{
              setIsVisibleModalManage,
              changeNodesFromModal: ManageMigrationChangeNodes,
              callBackForRefreshGetUpdate: props.action.editLcm,
            }}
            plannedActivityId={plannedActivityIdToManage}
          ></ManageMigration>
        </Modal.Body>
      </Modal>

      <ModalConfirm data={confirm} />

      <Dialog
        open={isVisibleModalStatus}
        onClose={() => setIsVisibleModalStatus(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Update Planned Activity Status</h4>
            </div>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalStatus(false)}
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
          <UpdatePlannedActivityStatusModal
            isFromPlannedActivityModal={false}
            lcmId={props.lcmId}
            action={{
              setIsVisibleModalStatus,
              updateActivityStatus,
              changeNodesFromActivityStatus,
              Update: updateDeliveryStatus,
              callBackForRefreshGetUpdate: props.action.editLcm,
            }}
            isReleaseDetailUnKnown={props.isReleaseDetailUnKnown}
            planningActivityDetailsResourceId={plannedActivityIdToManage}
            plannedActivityTypeForEnum={props.plannedActivityTypeForEnum}
            closeLCMModal={onCloseLCMModal}
          />
        </DialogContent>
      </Dialog>

      <div className="col-12 mx-0 px-0">
        <div className="col-12  d-flex justify-content-end my-2">
          {!props.selectedDeploymentStatus?.readOnlyPlannedActivity && (
            <button
              className={`  voda-bold btn btn-danger px-4 btnHeader ${
                props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                  ? "disabledCursor"
                  : ""
              }`}
              onClickCapture={() => createNewPA()}
              type="button"
              disabled={
                props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                  ? true
                  : props.formDisabed
              }
              data-toggle="tooltip"
              data-placement="top"
              title={
                props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                  ? `Saving is disabled due to redirection from LCM Export screen`
                  : ""
              }
            >
              Create New Planned Activity
            </button>
          )}
        </div>
        <div className="mx-0 px-0 py-3 flex-row">
          <table
            className="table table-borderless table-responsive"
            style={{ minHeight: "inherit" }}
          >
            <thead>
              <tr className="intestazione">
                <TH
                  propertyName="opCoId"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>

                {!props.isDesignAspect && (
                  <TH
                    propertyName="designComponent"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {(props.isDesignAspect || props.isReleaseDetailUnKnown) && (
                  <TH
                    propertyName="designComponentFamily"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {!props?.isFromNetworkElement && !props.isDesignAspect && (
                  <TH
                    propertyName="plannedDesignComponent"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {!props?.isFromNetworkElement && !props.isDesignAspect && (
                  <TH
                    propertyName="currentBuildBagDescription"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {!props?.isFromNetworkElement && !props.isDesignAspect && (
                  <TH
                    propertyName="plannedBuildBagDescription"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {!props?.isFromNetworkElement && !props.isDesignAspect && (
                  <TH
                    propertyName="plannedCompletion"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}
                <TH
                  propertyName="preBaseLineDate"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>
                <TH
                  propertyName="plannedAction"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>

                <TH
                  propertyName="deliveryStatus"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>

                {!props?.isFromNetworkElement && !props.isDesignAspect && (
                  <TH
                    propertyName="productImportanceId"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {(props.isForAddAsset || props.isForEditAsset) && (
                  <>
                    <TH
                      propertyName="forAddAsset"
                      action={thAction}
                      isVisibleFiltriString={isVisibleFiltri}
                      setupDuplicates={false}
                    ></TH>
                    <TH
                      propertyName="forEditAsset"
                      action={thAction}
                      isVisibleFiltriString={isVisibleFiltri}
                      setupDuplicates={false}
                    ></TH>
                  </>
                )}
                <TH
                  propertyName="activity Index"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>

                {!props.isDesignAspect && (
                  <TH
                    propertyName="eduspoc"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                {!props.isDesignAspect && (
                  <TH
                    propertyName="subDomainSpoc"
                    action={thAction}
                    isVisibleFiltriString={isVisibleFiltri}
                    setupDuplicates={false}
                  ></TH>
                )}

                <TH
                  propertyName="lastModified"
                  action={thAction}
                  isVisibleFiltriString={isVisibleFiltri}
                  setupDuplicates={false}
                ></TH>
                <th className="  customWidth"></th>
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => {
                document
                  .querySelectorAll<HTMLTableDataCellElement>("tbody > tr > td")
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

                const plannedActivity = props.formArray?.find(
                  (x) => x.plannedActivityId === item.plannedActivityId
                );
                let isPACheck = false;
                if (plannedActivity) {
                  const {
                    plannedActivityResource: paRes,
                    plannedActivityResourceId: paID,
                  } = plannedActivity;

                  isPACheck = paRes
                    ? dictionaryToArrayPlannedActivityResourceDto(paRes).some(
                        (res) =>
                          res.value.plannedActivityResourceId === paID &&
                          res.value.ruleLinkedDc === 15
                      )
                    : false;
                }
                return (
                  <tr
                    className="dati"
                    key={`${item.plannedActivityId}${index}`}
                  >
                    <td>{item.opco}</td>
                    {!props.isDesignAspect && (
                      <td className=" ">
                        <label
                          className="themeText w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html:
                              item.designComponent !== undefined
                                ? item.designComponent
                                    .replace(/ on /g, "\u00a0on\u00a0")
                                    .replace(/ with /g, "\u00a0with\u00a0")
                                : "",
                          }}
                        ></label>
                      </td>
                    )}

                    {(props.isDesignAspect || props.isReleaseDetailUnKnown) && (
                      <td className=" ">
                        <label
                          className="themeText w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html:
                              item.designComponentFamily !== undefined
                                ? item.designComponentFamily
                                    .replace(/ on /g, "\u00a0on\u00a0")
                                    .replace(/ with /g, "\u00a0with\u00a0")
                                : "",
                          }}
                        ></label>
                      </td>
                    )}

                    {!props?.isFromNetworkElement && !props.isDesignAspect && (
                      <td className=" ">
                        <label
                          className="themeText w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: item?.plannedDesignComponentName!,
                          }}
                        ></label>
                      </td>
                    )}

                    {!props?.isFromNetworkElement && !props.isDesignAspect && (
                      <td className=" ">
                        <label
                          className="themeText w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: item?.currentBuildBagDescription!
                              ? item?.currentBuildBagDescription!
                              : formData?.buildBagResources
                              ? resourceArrayRefactor(
                                  formData?.buildBagResources
                                ).filter(
                                  (res) => res.key === props.buildBagIds
                                )[0]?.value
                              : "",
                          }}
                        ></label>
                      </td>
                    )}

                    {!props?.isFromNetworkElement && !props.isDesignAspect && (
                      <td className=" ">
                        <label
                          className="themeText w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: item?.plannedBuildBagDescription ?? "",
                          }}
                        ></label>
                      </td>
                    )}

                    {!props?.isFromNetworkElement && !props.isDesignAspect && (
                      <td className=" ">{item?.plannedCompletionValue}</td>
                    )}
                    <td className=" ">{item?.preBaseLineDateValue}</td>

                    <td className=" ">{item.plannedAction ?? "---"}</td>

                    <td className=" ">
                      {item.deliveryStatusName
                        ? item.deliveryStatusName
                        : DeliveryStatusArr
                        ? DeliveryStatusArr.find(
                            (y) => y.key === item?.deliveryStatusId
                          )?.value
                        : "---"}
                    </td>

                    {!props?.isFromNetworkElement && !props.isDesignAspect && (
                      <td className=" ">{item.productImportance}</td>
                    )}

                    {(props.isForAddAsset || props.isForEditAsset) && (
                      <>
                        <td className=" ">
                          {item.forAddAsset === null
                            ? "---"
                            : item.forAddAsset?.toString()}
                        </td>
                        <td className=" ">
                          {item.forEditAsset === null
                            ? "---"
                            : item.forEditAsset?.toString()}
                        </td>
                      </>
                    )}
                    <td className=" ">
                      {item.plannedActivityId != 0 &&
                      item.plannedActivityId != undefined
                        ? rtnPlannedIdWithPadding(item.plannedActivityId)
                        : ""}
                    </td>
                    {!props.isDesignAspect && (
                      <td className=" ">
                        {item.eduSpoc &&
                          item.eduSpoc.map((x) => {
                            return (
                              <label className="w-100" key={x.key}>
                                {x.value}
                              </label>
                            );
                          })}
                      </td>
                    )}

                    {!props.isDesignAspect && (
                      <td className=" ">
                        {item.subDomainSpoc &&
                          item.subDomainSpoc.map((x) => {
                            return (
                              <label className="w-100" key={x.key}>
                                {x.value}
                              </label>
                            );
                          })}
                      </td>
                    )}

                    <td className="">
                      {formatDateWithTime(item.lastModified)}
                    </td>

                    <td className="actions">
                      <div className="d-flex flex-row">
                        <button
                          type="button"
                          title="Edit"
                          className="btn btn-link"
                          onClick={() =>
                            editForm(item.plannedActivityId, index, item)
                          }
                        >
                          {props.selectedDeploymentStatus
                            ?.readOnlyPlannedActivity ? (
                            <img
                              className="btnEdit"
                              src={require("../../img/search.png")}
                              alt="edit"
                            />
                          ) : (
                            <MdEdit color={`${darkMode ? "white" : "black"}`} />
                          )}
                        </button>
                        {item.plannedActivityId != undefined &&
                        props?.isFromNetworkElement === false &&
                        !props.isDesignAspect ? (
                          <button
                            disabled={
                              !(
                                item.deliveryStatusId === 3 ||
                                item.deliveryStatusId === 5 ||
                                item.deliveryStatusId === 8
                              )
                            }
                            type="button"
                            title="Manage Migration"
                            className="btn btn-link"
                            onClick={() =>
                              item.plannedActivityId &&
                              getManageMigrationData(
                                item.plannedActivityId,
                                index
                              )
                            }
                          >
                            <LuFolderSync
                              color={`${darkMode ? "white" : "black"}`}
                            />
                          </button>
                        ) : null}

                        {item.plannedActivityId !== undefined &&
                        !isPACheck &&
                        !props.isDesignAspect ? (
                          <button
                            type="button"
                            title="Update Planned Activity Status"
                            className="btn btn-link"
                            onClick={() =>
                              item.plannedActivityId &&
                              openModalStatus(item.plannedActivityId)
                            }
                          >
                            <IoIosRefresh
                              color={`${darkMode ? "white" : "black"}`}
                            />
                          </button>
                        ) : null}
                        <button
                          type="button"
                          title="Delete"
                          className="btn btn-link"
                          onClick={() => Delete(index)}
                        >
                          <MdDelete color={`${darkMode ? "white" : "black"}`} />
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
      <div className="col-12 mx-0 px-0 pl-0">
        {props.showForm ? (
          <form
            onChange={() => setChanged(true)}
            className={`mt-40 ${
              props.formDisabed ? "pointer-events-none" : ""
            }`}
          >
            <div className="py-4 w-100">
              <div className="row col-12 mt-2 mb-2 pl-0">
                <h5 className="titleSectionModal voda-bold col-12 mt-40">
                  {edit ? "Save Planned Activity" : "New Planned Activity"}
                </h5>
              </div>
              <div className="w-100">
                <div className="row col-md-12 mx-0 d-flex">
                  <div className="form-group col-md-6 pl-0">
                    <div className="col-md-12 pl-0">
                      <label className="labelForm voda-bold w-100">
                        OpCo
                        <input
                          type="text"
                          className="inputForm w-100"
                          value={props.opco}
                          disabled
                        />
                      </label>
                    </div>
                    {props.buildBagIds ? (
                      <>
                        <div className="col-md-12 pl-0">
                          <label
                            className="labelForm voda-bold w-100 mb-0"
                            title={
                              formData?.buildBagResources
                                ? resourceArrayRefactor(
                                    formData?.buildBagResources
                                  ).filter(
                                    (res) => res.key === props.buildBagIds
                                  )[0]?.value
                                : ""
                            }
                          >
                            {"Current Bag"}
                          </label>
                        </div>
                        <div className="form-group col-12 pl-4 customFakeInput disabled mt-0">
                          <label
                            className="labelForm   w-100 mb-0"
                            dangerouslySetInnerHTML={{
                              __html: formData?.buildBagResources
                                ? resourceArrayRefactor(
                                    formData?.buildBagResources
                                  ).filter(
                                    (res) => res.key === props.buildBagIds
                                  )[0]?.value
                                : "",
                            }}
                          ></label>
                        </div>
                      </>
                    ) : null}
                  </div>
                  <div className="form-group col-md-6 pr-0">
                    {props.designComponentFamily &&
                      !props.isReleaseDetailUnKnown && (
                        <div className="col-md-12 pr-0">
                          <div className="col-md-12 pl-0">
                            <label
                              className="labelForm voda-bold w-100 mb-0"
                              title={
                                props.designComponentFamily !== undefined
                                  ? props.designComponentFamily
                                      .replace('<b class="text-lowercase">', "")
                                      .replace("</b>", "")
                                  : ""
                              }
                            >
                              Design Component Family
                            </label>
                          </div>
                          <div className="form-group col-12 pl-4 customFakeInput disabled mt-0">
                            <label
                              className="labelForm   w-100 mb-0"
                              dangerouslySetInnerHTML={{
                                __html:
                                  props.designComponentFamily !== undefined
                                    ? props.designComponentFamily
                                        .replace(/ on /g, "\u00a0on\u00a0")
                                        .replace(/ with /g, "\u00a0with\u00a0")
                                    : "",
                              }}
                            ></label>
                          </div>
                        </div>
                      )}
                    <div className="col-md-12 pr-0">
                      <div className="col-md-12 pl-0">
                        <label
                          className="labelForm voda-bold w-100 mb-0"
                          title={
                            props.designComponent?.value != undefined
                              ? props.designComponent.value
                                  .replace('<b class="text-lowercase">', "")
                                  .replace("<b>", "")
                                  .replace("</b>", "")
                              : props.designComponentFamily != undefined
                              ? props.designComponentFamily
                                  .replace('<b class="text-lowercase">', "")
                                  .replace("<b>", "")
                                  .replace("</b>", "")
                              : ""
                          }
                        >
                          {props.isReleaseDetailUnKnown
                            ? "Initial Design Component"
                            : "Current Design Component"}
                        </label>
                      </div>
                      <div className="form-group col-12 pl-4 customFakeInput disabled mt-0">
                        <label
                          className="labelForm   w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html:
                              props.designComponent?.value != undefined
                                ? props.designComponent.value
                                    .replace(/ on /g, "\u00a0on\u00a0")
                                    .replace(/ with /g, "\u00a0with\u00a0")
                                : props.designComponentFamily != undefined
                                ? props.designComponentFamily
                                    .replace(/ on /g, "\u00a0on\u00a0")
                                    .replace(/ with /g, "\u00a0with\u00a0")
                                : "",
                          }}
                        ></label>
                      </div>
                    </div>
                    {!isNoPA && (
                      <div className="form-group col-md-12 pr-0">
                        <label className="labelForm voda-bold w-100">
                          Activity Status <span className="red">*</span>
                          <div
                            className={
                              ActivityStatusFromLogic?.length <= 1 ||
                              props.selectedDeploymentStatus
                                ?.readOnlyPlannedActivity
                                ? "not-allowed w-100"
                                : "w-100"
                            }
                          >
                            {formData?.activityStatusResource &&
                              dictionaryToArray(
                                formData?.activityStatusResource
                              ).map(
                                (item) =>
                                  ActivityStatusFromLogic.includes(
                                    item.key
                                  ) && (
                                    <Form.Check
                                      type="radio"
                                      className="radio"
                                      name="RequiredEditAsset"
                                      label={item.value}
                                      disabled={
                                        !ActivityStatusFromLogic.includes(
                                          item.key
                                        )
                                      }
                                      value={item.key}
                                      checked={true}
                                    />
                                  )
                              )}
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("activityStatusId") ? (
                            <label className="validation">
                              *Activity Status must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    )}
                  </div>
                </div>
                <div className="col-12 row mx-0">
                  <fieldset className="fieldset p-0">
                    <legend className="text-bb mt-4 pl-0">
                      Planned Activity Details
                    </legend>
                    <div className="row">
                      <div className="col-md-6 pl-0">
                        <div className="col-md-12">
                          <label className="labelForm voda-bold w-100">
                            Select the Planned Activity
                            <span className="red">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  isDisabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity ||
                                    props.formDisabed ||
                                    (props.lcmDeploymentStatusString
                                      ?.toLowerCase()
                                      .trim() == "in-commissioning" &&
                                      edit) ||
                                    (props.lcmDeploymentStatusString
                                      ?.toLowerCase()
                                      .trim() == "planned" &&
                                      !props?.edit) ||
                                    edit
                                  }
                                  options={
                                    props.lcmDeploymentStatusString
                                      ?.toLowerCase()
                                      .trim() == "in-decommissioning"
                                      ? [
                                          {
                                            label: "HW & SW",
                                            options:
                                              dictionaryToArrayPlannedActivityResourceDto(
                                                formData?.plannedActivityResource!
                                              ).filter(
                                                (x) =>
                                                  x.value.lcmHardware ===
                                                    true &&
                                                  x.value.lcmSoftware ===
                                                    true &&
                                                  x.value.forLcm === true &&
                                                  (x.value
                                                    .plannedActivityResourceDescription ==
                                                    "Replace Solution (Change Equipment Manufacturer)" ||
                                                    x.value
                                                      .plannedActivityResourceDescription ==
                                                      "Modernize System (incl. virtualization)")
                                              ),
                                          },
                                        ]
                                      : (formData?.buildBagResources
                                          ? resourceArrayRefactor(
                                              formData?.buildBagResources
                                            ).filter(
                                              (res) =>
                                                res.key === props.buildBagIds
                                            )[0]?.value
                                          : ""
                                        )?.includes("Empty")
                                      ? groupedPlannedResource &&
                                        groupedPlannedResource.map((group) => ({
                                          ...group,
                                          options: group.options.filter(
                                            (option) =>
                                              option.value.ruleLinkedDc !== 16
                                          ),
                                        }))
                                      : groupedPlannedResource
                                  }
                                  defaultValue={
                                    formData &&
                                    formData.plannedActivityResourceId !=
                                      undefined &&
                                    formData.plannedActivityResourceId != null
                                      ? formData?.plannedActivityResource &&
                                        groupedPlannedResource &&
                                        groupedPlannedResource
                                          ?.map((one) => one.options)
                                          ?.flat()
                                          ?.find(
                                            (x) =>
                                              x.key! ==
                                              formData.plannedActivityResourceId
                                          )
                                      : props.lcmDeploymentStatusString
                                          ?.toLowerCase()
                                          .trim() == "in-decommissioning"
                                      ? []
                                      : undefined
                                  }
                                  value={
                                    formData &&
                                    formData.plannedActivityResourceId !=
                                      undefined &&
                                    formData.plannedActivityResourceId != null
                                      ? formData?.plannedActivityResource &&
                                        groupedPlannedResource &&
                                        groupedPlannedResource
                                          ?.map((one) => one.options)
                                          ?.flat()
                                          ?.find(
                                            (x) =>
                                              x.key! ==
                                              formData.plannedActivityResourceId
                                          )
                                      : null
                                  }
                                  onChange={(e) => {
                                    if (e?.["value"]?.ruleLinkedDc == 15) {
                                      setIsNoPA(true);
                                      onChangePlannedActivityResourceId(
                                        e?.["value"]
                                      );
                                    } else {
                                      setIsNoPA(false);
                                      onChangePlannedActivityResourceId(
                                        e?.["value"]
                                      );
                                      onGetDeliveryStatus(e?.["key"]);
                                    }
                                    if (e?.["value"]?.ruleLinkedDc == 34) {
                                      InfraClusterPaLevelApiCall();
                                    }
                                    if (e?.["value"]?.ruleLinkedDc == 36) {
                                      InfraUpgradeClusterPaLevelApiCall();
                                    }
                                    if (e?.["value"]?.ruleLinkedDc == 35) {
                                      InfraClusterPaProgramLevelGetApiCall();
                                    }
                                    props.action.setChanged(true);
                                  }}
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  getOptionLabel={(option) =>
                                    option?.value
                                      ?.plannedActivityResourceDescription ?? ""
                                  }
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                  formatGroupLabel={formatGroupLabel}
                                ></Select>
                              </div>
                              {tipologicaPermesso && !props.formDisabed && (
                                <button
                                  disabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity
                                  }
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
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "plannedActivityResourceId"
                            ) ? (
                              <label className="validation">
                                *planned Activity must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                        {ruleDCIdFlag && !isNoPA && (
                          <div className="col-md-12">
                            <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                              <div className="switchContainer d-flex flex-row align-items-center">
                                <label
                                  className={`switch mr-2 ${
                                    edit ? "disabledCursor" : ""
                                  }`}
                                >
                                  <input
                                    type="checkbox"
                                    disabled={edit}
                                    // disabled={formData?.plannedActivityId ? true : false}
                                    checked={formData?.isPAReleaseDetailUnknown}
                                    onChange={(e) => {
                                      onChange("isPAReleaseDetailUnknown", e);
                                      onChangePAState(e);
                                    }}
                                  />
                                  <span
                                    className={`slider round ${
                                      edit ? "disabledCursor" : ""
                                    }`}
                                  ></span>
                                </label>
                                Release Details Unknown?
                              </div>
                            </label>
                          </div>
                        )}
                        {!isNoPA && (
                          <>
                            <div className="col-md-12">
                              <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                                <div className="switchContainer d-flex flex-row align-items-center">
                                  <label className="switch mr-2">
                                    <input
                                      type="checkbox"
                                      checked={formData?.deliveryPlanAvailable}
                                      onChange={(e) =>
                                        onChange("deliveryPlanAvailable", e)
                                      }
                                    />
                                    <span className="slider round"></span>
                                  </label>
                                  Delivery Plan Available
                                </div>
                              </label>
                            </div>
                            <Container
                              show={
                                plannedResourceSelected?.ruleNetworkElement == 2
                              }
                            >
                              <div className="col-12 mt-3 mb-3">
                                <label className="labelForm voda-bold d-flex align-items-center w-100">
                                  <input
                                    disabled={
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                    }
                                    type="radio"
                                    id="isNewService"
                                    checked={formData?.isNewServiceArchitecture}
                                    onChange={(e) =>
                                      isThisACheck(
                                        "isNewServiceArchitecture",
                                        e
                                      )
                                    }
                                    className=" mr-1"
                                  />
                                  Is this a New Service Architecture?
                                </label>
                                <label className="labelForm voda-bold pt-2 d-flex align-items-center w-100">
                                  <input
                                    disabled={
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                    }
                                    type="radio"
                                    id="isNewService"
                                    checked={
                                      formData?.isReplacementExistingSolution
                                    }
                                    onChange={(e) =>
                                      isThisACheck(
                                        "isReplacementExistingSolution",
                                        e
                                      )
                                    }
                                    className=" mr-1"
                                  />
                                  Is this a Replacement of Existing Solution?
                                </label>
                              </div>
                            </Container>
                            <Container show={isVisibleLinkedLCM}>
                              <div className="form-group col-md-12">
                                <label className="labelForm voda-bold w-100">
                                  Linked LCM Planned activity
                                  <span className="red">*</span>
                                  <div className="d-flex">
                                    <div className="w-100">
                                      <Select
                                        menuPosition={"fixed"}
                                        isDisabled={
                                          props.selectedDeploymentStatus
                                            ?.readOnlyPlannedActivity
                                        }
                                        options={
                                          linkedLCMPlannedActivityResource
                                        }
                                        value={
                                          formData &&
                                          formData.linkedToPlannedActivityId !=
                                            undefined
                                            ? linkedLCMPlannedActivityResource?.find(
                                                (x) =>
                                                  x.key ===
                                                  formData.linkedToPlannedActivityId
                                              )
                                            : null
                                        }
                                        onChange={(e) => {
                                          onChangeLinkedPlannedActivity(e);
                                          props.action.setChanged(true);
                                        }}
                                        onBlur={() => setInputValue("")}
                                        isSearchable
                                        getOptionLabel={(option) =>
                                          option.value.name ?? ""
                                        }
                                        getOptionValue={(option) =>
                                          option["key"].toString()
                                        }
                                        // formatGroupLabel={formatGroupLabel}
                                      />
                                    </div>
                                  </div>
                                  {validation &&
                                  validation.response === false &&
                                  validation.property?.includes(
                                    "linkedToPlannedActivityId"
                                  ) ? (
                                    <label className="validation">
                                      *Planned Activity must have a value
                                    </label>
                                  ) : null}
                                </label>
                              </div>
                            </Container>

                            <div className="col-md-12">
                              <label className="labelForm voda-bold w-100">
                                Activity Details{" "}
                                {props.lcmId ? (
                                  <span className="red">*</span>
                                ) : null}
                                {!activityDetailsManual ? (
                                  <div
                                    className="col-12 pl-4 customFakeInput disabled"
                                    style={{ marginTop: "4px" }}
                                  >
                                    <label
                                      className="labelForm w-100 mb-0"
                                      dangerouslySetInnerHTML={{
                                        __html: formData?.activityDetails ?? "",
                                      }}
                                    ></label>
                                  </div>
                                ) : (
                                  <input
                                    disabled={
                                      formData?.isPAReleaseDetailUnknown ||
                                      formData?.plannedActivityResourceId ==
                                        undefined ||
                                      !activityDetailsManual ||
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                    }
                                    type="text"
                                    onChange={(e) => {
                                      onChange("activityDetails", e);
                                      props.action.setChanged(true);
                                    }}
                                    onKeyUp={(e) => {
                                      onChange("activityDetails", e);
                                      props.action.setChanged(true);
                                    }}
                                    className="inputForm   w-100"
                                    value={formData?.activityDetails}
                                  />
                                )}
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "activityDetails"
                                ) ? (
                                  <label className="validation">
                                    *activity Details must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </>
                        )}
                      </div>
                      <div className="col-md-6 pr-0">
                        {props.isReleaseDetailUnKnown === true ? (
                          <div className="col-md-12 pr-0">
                            <div className="col-md-12 pl-0">
                              <label
                                className="labelForm voda-bold w-100 mb-0"
                                title={
                                  props.designComponentFamily !== undefined
                                    ? props.designComponentFamily
                                        .replace(
                                          '<b class="text-lowercase">',
                                          ""
                                        )
                                        .replace("</b>", "")
                                    : ""
                                }
                              >
                                Design Component Family
                              </label>
                            </div>
                            <div className="form-group col-12 pl-4 customFakeInput disabled mt-0">
                              <label
                                className="labelForm   w-100 mb-0"
                                dangerouslySetInnerHTML={{
                                  __html:
                                    props.designComponentFamily !== undefined
                                      ? props.designComponentFamily
                                          .replace(/ on /g, "\u00a0on\u00a0")
                                          .replace(
                                            / with /g,
                                            "\u00a0with\u00a0"
                                          )
                                      : "",
                                }}
                              ></label>
                            </div>
                          </div>
                        ) : props.isReleaseDetailUnKnown === false &&
                          props.isInLcm ? (
                          <div className="col-md-12">
                            <label className="labelForm w-100">
                              <label className="labelForm voda-bold mb-0">
                                Planned Design Component
                                {isConsigliati == true || !isNoPA ? (
                                  <span className="red">*</span>
                                ) : (
                                  ""
                                )}
                              </label>
                              <Select
                                menuPosition={"fixed"}
                                isDisabled={isNoPA ? true : isPADisabled}
                                options={
                                  formData &&
                                  formData.isPAReleaseDetailUnknown &&
                                  unknownDC !== undefined
                                    ? unknownDC.filter(
                                        (x) =>
                                          x.key == formData?.designComponentId
                                      )
                                    : resourceDesignComponent &&
                                      formData?.plannedActivityResource &&
                                      dictionaryToArrayPlannedActivityResourceDto(
                                        formData?.plannedActivityResource
                                      ).filter(
                                        (x) =>
                                          x.value.plannedActivityResourceId ===
                                            formData?.plannedActivityResourceId &&
                                          x.value.ruleLinkedDc === 13
                                      ).length === 1
                                    ? resourceDesignComponent &&
                                      resourceDesignComponent?.filter(
                                        (x) =>
                                          x.key == props.designComponent?.key
                                      )
                                    : props.lcmDeploymentStatusString
                                        ?.toLocaleLowerCase()
                                        .trim() != "in-commissioning"
                                    ? //   &&
                                      // props.lcmDeploymentStatusString
                                      //   ?.toLocaleLowerCase()
                                      //   .trim() != "planned"
                                      resourceDesignComponent &&
                                      resourceDesignComponent.filter(
                                        (x) =>
                                          x.key != props.designComponent?.key
                                      )
                                    : resourceDesignComponent
                                }
                                value={
                                  formData &&
                                  formData.isPAReleaseDetailUnknown &&
                                  unknownDC !== undefined
                                    ? unknownDC.filter(
                                        (x) =>
                                          x.key === formData?.designComponentId
                                      )
                                    : resourceDesignComponent &&
                                      formData?.plannedActivityResource &&
                                      dictionaryToArrayPlannedActivityResourceDto(
                                        formData?.plannedActivityResource
                                      ).filter(
                                        (x) =>
                                          x.value.plannedActivityResourceId ===
                                            formData?.plannedActivityResourceId &&
                                          (x.value.ruleLinkedDc === 13 ||
                                            x.value.ruleLinkedDc === 15 ||
                                            x.value.ruleLinkedDc === 8 ||
                                            x.value.ruleLinkedDc === 12 ||
                                            x.value.ruleLinkedDc === 16 ||
                                            x.value.ruleLinkedDc === 34 ||
                                            x.value.ruleLinkedDc === 36 ||
                                            x.value.ruleLinkedDc === 35)
                                      ).length === 1
                                    ? resourceDesignComponent &&
                                      resourceDesignComponent.filter(
                                        (x) =>
                                          x.key === props.designComponent?.key
                                      )
                                    : props.lcmDeploymentStatusString
                                        ?.toLocaleLowerCase()
                                        .trim() != "in-commissioning"
                                    ? //   &&
                                      // props.lcmDeploymentStatusString
                                      //   ?.toLocaleLowerCase()
                                      //   .trim() != "planned"
                                      resourceDesignComponent &&
                                      resourceDesignComponent.filter(
                                        (x) =>
                                          x.key === formData?.designComponentId
                                      )
                                    : resourceDesignComponent &&
                                      resourceDesignComponent.filter(
                                        (x) =>
                                          x.key === props.designComponent?.key
                                      )
                                }
                                onChange={(e) => {
                                  onChangeLinkedDesignComponentId(e);
                                  props.action.setChanged(true);
                                }}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                formatOptionLabel={function (data) {
                                  const isDecommision =
                                    formData?.plannedActivityResource &&
                                    props &&
                                    dictionaryToArrayPlannedActivityResourceDto(
                                      formData?.plannedActivityResource
                                    ).filter(
                                      (x) =>
                                        x.value.plannedActivityResourceId ===
                                          formData?.plannedActivityResourceId &&
                                        x.value.ruleLinkedDc === 13
                                    ).length === 1;

                                  const newValue = isDecommision
                                    ? `<b class="text-uppercase" >TO BE DECOMMISSIONED - </b> ${data.value}`
                                    : data.value;
                                  return (
                                    <span
                                      dangerouslySetInnerHTML={{
                                        __html: newValue,
                                      }}
                                    />
                                  );
                                }}
                                // styles={colourStyles}
                                styles={getCustomStyles(isPADisabled)}
                              ></Select>
                              {validation &&
                              validation.response === false &&
                              validation.property?.includes(
                                "designComponentId"
                              ) ? (
                                <label className="validation">
                                  *Planned Design Component must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                        ) : null}

                        {props.isInLcm ? (
                          <div className="col-md-12 pr-0">
                            <label className="labelForm voda-bold w-100">
                              Planned Bag<span className="red">*</span>
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition={"fixed"}
                                    isDisabled={
                                      isNoPA
                                        ? true
                                        : isPADisabled &&
                                          plannedResourceSelected?.ruleLinkedDc !==
                                            16
                                    }
                                    options={
                                      buildBagRes
                                        ? resourceArrayRefactor(buildBagRes)
                                        : []
                                    }
                                    value={
                                      formData?.buildBagId && buildBagRes
                                        ? resourceArrayRefactor(
                                            buildBagRes
                                          ).filter(
                                            (x) =>
                                              x.key === formData?.buildBagId
                                          )
                                        : null
                                    }
                                    onChange={(e) => {
                                      onChangeSelect("buildBagId", e);
                                    }}
                                    onBlur={() => setInputValue("")}
                                    isSearchable
                                    isClearable
                                    getOptionLabel={(option) => option.value}
                                    getOptionValue={(option) =>
                                      option["key"].toString()
                                    }
                                    styles={{
                                      option: (provided, state) => {
                                        // Inline logic to get isColour based on the key
                                        const isColour = buildBagRes.find(
                                          (item) => item.key === state.data.key
                                        )?.isColour;

                                        return {
                                          ...provided,
                                          color: isColour ? "blue" : undefined, // Apply blue color if isColour is true
                                        };
                                      },
                                    }}
                                    // isDisabled={isNoPA ? true : isPADisabled}
                                  ></Select>
                                </div>
                                {tipologicaPermesso &&
                                  !props.formDisabed &&
                                  formData?.buildBagId !== 0 && (
                                    <button
                                      type="button"
                                      className="btn btn-link"
                                      data-toggle="tooltip"
                                      data-placement="top"
                                      title="View Bag Component"
                                      style={{ color: "#6c757d !important" }}
                                      onClick={() => setViewBagFlag(true)}
                                    >
                                      <FaRegEye
                                        style={{ color: "gray" }}
                                        color={`${darkMode ? "white" : ""}`}
                                      />
                                    </button>
                                  )}
                                {tipologicaPermesso && !props.formDisabed && (
                                  <button
                                    className="btn btn-link"
                                    onClick={() => setIsBuildBagItemFlag(true)}
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
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes("buildBagId") ? (
                                <label className="validation h-16">
                                  *Planned Bag must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                        ) : null}
                      </div>
                    </div>
                    {plannedResourceSelected?.ruleLinkedDc === 36 && (
                      <div className="row">
                        <div className="col-md-6">
                          <div className="col-md-12 pl-0">
                            <DropdownInputComponent
                              label={"Site"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable
                              isClearable={false}
                              required
                              disabled={
                                formData?.plannedActivityId ? true : false
                              }
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
                                validation?.property?.includes("siteId") ??
                                false
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
                        <div className="col-md-6">
                          <div className="col-md-12 pr-0">
                            <DropdownInputComponent
                              label={"Hardware Type"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable
                              isClearable
                              required
                              value={
                                hardwareTypeOptions &&
                                hardwareTypeOptions.filter(
                                  (x: any) =>
                                    x.key ===
                                    ((formData as any)
                                      ?.infraClusterClusterUpgradeUpsertDto
                                      ?.plannedHardwareTypeId as any)
                                )
                              }
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
                      </div>
                    )}
                    {plannedResourceSelected?.ruleLinkedDc === 35 && (
                      <div className="row">
                        <div className="col-md-6">
                          <div className="col-md-12 pl-0">
                            <DropdownInputComponent
                              label={"Infra Cluster"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable
                              isClearable={false}
                              required
                              disabled={
                                formData?.plannedActivityId ? true : false
                              }
                              value={
                                clusterDropdown &&
                                clusterDropdown.filter(
                                  (x: any) =>
                                    x.key ===
                                    ((formData as any)?.clusetrId ||
                                      selectedCluster)
                                )
                              }
                              options={clusterDropdown}
                              isError={
                                validation?.property?.includes("clusetrId") ??
                                false
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
                      </div>
                    )}
                    {isNoPA && (
                      <div className="row">
                        <div className="form-group col-6 pl-0">
                          <div className="col-12">
                            <DropdownInputComponent
                              label={`${
                                LabelsDictionary["reasonForNoPlan"]?.Full ??
                                "reasonForNoPlan"
                              }`}
                              labelCSS="mb-0"
                              isError={
                                validation &&
                                validation.response === false &&
                                validation.property?.includes("reasonForNoPlan")
                                  ? true
                                  : false
                              }
                              error={"*Reason for no plan must have an value."}
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                formData &&
                                formData?.reasonForNoPlan != undefined
                                  ? reasonForNoPlan.find(
                                      (x) =>
                                        x.value.toLowerCase() ===
                                        formData?.reasonForNoPlan?.toLowerCase()
                                    )
                                  : null
                              }
                              options={reasonForNoPlan}
                              onChange={(e: any) =>
                                onChangeDropdown("reasonForNoPlan", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="form-group col-6 pr-0">
                          <div className="col-12">
                            <TextInputComponent
                              label={`${
                                LabelsDictionary["commentOnProjectStatus"]
                                  ?.Full ?? "commentOnProjectStatus"
                              }`}
                              labelCSS="mb-0"
                              required={true}
                              isError={
                                validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "commentOnProjectStatus"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Comment On Project Status must have an value."
                              }
                              inputCSS="labelForm voda-bold mb-0"
                              value={formData?.commentOnProjectStatus ?? ""}
                              onChange={(e: any) =>
                                onChange("commentOnProjectStatus", e)
                              }
                            />
                          </div>
                        </div>
                      </div>
                    )}
                    {newNetworkAssocitate !== null &&
                      props?.isInLcm &&
                      !isNoPA &&
                      (props.lcmDeploymentStatusString === "Planned" ||
                        props.lcmDeploymentStatusString ===
                          "In-Commissioning") && (
                        <div className="row">
                          <div className="col-12">
                            <label className="labelForm voda-bold w-100">
                              Select Network Elements
                              <span className="red">*</span>
                            </label>
                            <div className="row col-12 w-100 mx-0 px-0">
                              <div className="w-100">
                                <table className="minHeight-0 w-100">
                                  <thead>
                                    <tr className="mt-35 head">
                                      <th className="pl-2 ptb-12"></th>
                                      <th className="pl-2 ptb-12">
                                        Element Name
                                      </th>
                                      <th className="pl-2 ptb-12">
                                        Environment
                                      </th>
                                      <th className="pl-2 ptb-12">Location</th>
                                      <th className="pl-2 ptb-12">Status</th>
                                    </tr>
                                  </thead>
                                  <tbody>
                                    {newNetworkAssocitate?.map((item, i) => (
                                      <tr
                                        className={`dati ${
                                          numberIsNullOrZero(item.id) ? "" : ""
                                        }`}
                                        key={i}
                                      >
                                        <td>
                                          <input
                                            type="checkbox"
                                            checked={
                                              item.isFinalAsset ? true : false
                                            }
                                            onChange={(e) =>
                                              onSelectNode(
                                                e.target.checked,
                                                item
                                              )
                                            }
                                          ></input>
                                        </td>
                                        <td>{item.elementName}</td>
                                        <td>{item.enviroment}</td>
                                        <td>{item.location}</td>
                                        <td>{item.assetsStatus}</td>
                                      </tr>
                                    ))}
                                  </tbody>
                                </table>
                              </div>
                            </div>
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes("hasFinalAsset") ? (
                              <label
                                className="validation"
                                style={{ left: "10px" }}
                              >
                                *Select minimun of one network element
                              </label>
                            ) : null}
                          </div>
                        </div>
                      )}
                  </fieldset>
                  {plannedResourceSelected?.ruleLinkedDc === 34 &&
                    formData?.plannedActivityResourceId && (
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
                              Select Cluster Hardware
                              <span className="red">*</span>
                            </legend>
                            <button
                              className={`voda-bold btn btn-danger px-4 btnHeader w-100`}
                              style={{ maxWidth: "10rem" }}
                              onClick={() => setModalClusterFlag(true)}
                              type="button"
                            >
                              Add New Cluster
                            </button>
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "isAtleastOneCluster"
                            ) ? (
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
                                  <TH
                                    propertyName="locationId"
                                    overridePropertyName="Location"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="site"
                                    overridePropertyName="Site"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="platformId"
                                    overridePropertyName="Platform"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="clustertypeId"
                                    overridePropertyName="Cluster Type"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="clusterName"
                                    overridePropertyName="Cluster Name"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="plannedHardwareTypeId"
                                    overridePropertyName="Hardware Type"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="deploymentStatusId"
                                    overridePropertyName="Deployment Status"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="verticalResponsibleId"
                                    overridePropertyName="Vertical Responsible"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
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
                                                <MdDelete
                                                  color={`${
                                                    darkMode ? "white" : "black"
                                                  }`}
                                                />
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
                      </>
                    )}
                  {plannedResourceSelected?.ruleLinkedDc === 36 &&
                    formData?.plannedActivityResourceId && (
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
                            validation.property?.includes(
                              "isAtleastOneCluster"
                            ) ? (
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
                                  <TH
                                    propertyName="locationId"
                                    overridePropertyName="Location"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="site"
                                    overridePropertyName="Site"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="platformId"
                                    overridePropertyName="Platform"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="clustertypeId"
                                    overridePropertyName="Cluster Type"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="clusterName"
                                    overridePropertyName="Cluster Name"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="hardwaretypeId"
                                    overridePropertyName="Hardware Type"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="deploymentStatusId"
                                    overridePropertyName="Deployment Status"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="verticalResponsibleId"
                                    overridePropertyName="Vertical Responsible"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <th className="  customWidth"></th>
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
                                                <MdDelete
                                                  color={`${
                                                    darkMode ? "white" : "black"
                                                  }`}
                                                />
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
                      </>
                    )}
                  {plannedResourceSelected?.ruleLinkedDc === 35 &&
                    formData?.plannedActivityResourceId && (
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
                                  Remove App Cluster
                                </button>
                              </>
                            )}

                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "isAtleastOneCluster"
                            ) ? (
                              <label className="validation">
                                *Please select at least one app cluster to add /
                                remove.
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
                                  <TH
                                    propertyName="appClusterName"
                                    overridePropertyName="App Cluster Name"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>
                                  <TH
                                    propertyName="applicationName"
                                    overridePropertyName="Application Name"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>

                                  <TH
                                    propertyName="deploymentStatusId"
                                    overridePropertyName="Deployment Status"
                                    action={thAction}
                                    isVisibleFiltriString={isVisibleFiltri}
                                    setupDuplicates={false}
                                  ></TH>

                                  {isRemoveMode ? (
                                    <TH
                                      propertyName="Select"
                                      overridePropertyName="Select"
                                      action={thAction}
                                      isVisibleFiltriString={isVisibleFiltri}
                                      setupDuplicates={false}
                                    ></TH>
                                  ) : (
                                    <th className="  customWidth"></th>
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
                                                  setClusterProgramData(
                                                    (prev) =>
                                                      prev?.filter(
                                                        (_, idx) =>
                                                          idx !== index
                                                      )
                                                  )
                                                }
                                              >
                                                <MdDelete
                                                  color={`${
                                                    darkMode ? "white" : "black"
                                                  }`}
                                                />
                                              </button>
                                            ) : isRemoveMode &&
                                              isInService(
                                                item.deploymentStatusValue
                                              ) ? (
                                              <input
                                                type="checkbox"
                                                checked={item.selected || false}
                                                onChange={(e) => {
                                                  const checked =
                                                    e.target.checked;
                                                  setClusterProgramData(
                                                    (prev) =>
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
                                              isInService(
                                                item.deploymentStatusValue
                                              )
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
                      </>
                    )}
                  {!isNoPA && (
                    <>
                      <fieldset className="fieldset p-0">
                        <label className="text-bb mt-4 pl-0">
                          Implementation Details
                        </label>
                        <div className="row">
                          <div className="col-6 pl-0">
                            <div className="col-md-12">
                              <label
                                className={`labelForm voda-bold w-100 ${
                                  props.selectedDeploymentStatus
                                    ?.readOnlyPlannedActivity
                                    ? "disabledDate"
                                    : ""
                                }`}
                              >
                                Planned Implementation Year
                                <span className="red">*</span>
                                <div className="w-100">
                                  <DatePicker
                                    selected={startDate}
                                    onChange={handleDateChange}
                                    value={customDateFormat}
                                    showYearPicker
                                    className={`inputForm w-100 ${
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                        ? "disabledBackground"
                                        : ""
                                    }`}
                                    dateFormat="yyyy"
                                    yearItemNumber={8}
                                    minDate={new Date(currentYear - 10, 0, 1)}
                                    maxDate={new Date(currentYear + 10, 11, 31)}
                                    disabled={props.formDisabed}
                                    onFocus={(e) => {
                                      const financialYear = getFinancialYear();
                                      if (!startDate) {
                                        handleDateChange(financialYear, e);
                                      }
                                    }}
                                    onSelect={handleDateChange}
                                  />
                                </div>
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "plannedImplementationYear"
                                ) ? (
                                  <label className="validation">
                                    *Implementation Year is not valid{" "}
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>
                          <div className="col-md-6 pl-19">
                            <label className="labelForm voda-bold w-100">
                              Planning Status<span className="red">*</span>
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition={"fixed"}
                                    isDisabled={
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity ||
                                      props.formDisabed
                                    }
                                    options={
                                      formData?.planningActivityStatusResource &&
                                      dictionaryToArray(
                                        formData?.planningActivityStatusResource
                                      )
                                    }
                                    value={
                                      formData &&
                                      formData?.planningActivityStatusResource &&
                                      dictionaryToArray(
                                        formData?.planningActivityStatusResource
                                      ).filter(
                                        (x) =>
                                          x.key ===
                                          formData?.planningActivityStatusId
                                      )
                                    }
                                    onChange={(e) => {
                                      onChangeSelect(
                                        "planningActivityStatusId",
                                        e
                                      );
                                      props.action.setChanged(true);
                                    }}
                                    onBlur={() => setInputValue("")}
                                    isSearchable
                                    isClearable
                                    getOptionLabel={(option) => option.value}
                                    getOptionValue={(option) =>
                                      option["key"].toString()
                                    }
                                  ></Select>
                                </div>
                                {tipologicaPermesso && !props.formDisabed && (
                                  <button
                                    disabled={
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                    }
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
                              {validation &&
                              validation.response === false &&
                              validation.property?.includes(
                                "planningActivityStatusId"
                              ) ? (
                                <label className="validation">
                                  *Planning Status must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>

                          <div className="col-12">
                            <div className="row">
                              <div className="col-6 pl-0">
                                <div className="col-md-12">
                                  <label
                                    className={`labelForm voda-bold w-100 ${
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                        ? "disabledDate"
                                        : ""
                                    }`}
                                  >
                                    Planned Start Date
                                    {/* {(props.isReleaseDetailUnKnown || formData?.isPAReleaseDetailUnknown)&& (
                                  <span className="red">*</span>
                                )} */}
                                    <DatePicker
                                      // minDate={minDate()}
                                      selected={
                                        formData?.startDate
                                          ? new Date(formData?.startDate)
                                          : undefined
                                      }
                                      onChange={(startDate, e) => {
                                        e.preventDefault();
                                        onChangeDate("startDate", startDate);
                                        props.action.setChanged(true);
                                      }}
                                      className={`inputForm w-100 ${
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity
                                          ? "disabledBackground"
                                          : ""
                                      }`}
                                      minDate={new Date(1980, 0, 1)}
                                      maxDate={new Date(2999, 0, 1)}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText={"NOT SPECIFIED"}
                                      disabled={props.formDisabed}
                                    />
                                    {validation &&
                                    validation.response === false &&
                                    validation.property?.includes(
                                      "plannedStartDate"
                                    ) ? (
                                      <label className="validation">
                                        {formData?.startDate === "" ||
                                        formData?.startDate === null ||
                                        formData?.startDate === undefined
                                          ? "*planned start date must have a value"
                                          : "*planned start date must be less than planned completion"}
                                      </label>
                                    ) : null}
                                  </label>
                                </div>
                              </div>

                              <div className="col-6 pr-0">
                                <div className="col-md-12">
                                  <label
                                    className={`labelForm voda-bold w-100 ${
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                        ? "disabledDate"
                                        : ""
                                    }`}
                                  >
                                    Planned Completion Date
                                    <span className="red">*</span>
                                    <DatePicker
                                      selected={
                                        formData?.plannedCompletion &&
                                        new Date(formData?.plannedCompletion)
                                      }
                                      onChange={(newDate, e) => {
                                        e.preventDefault();
                                        onChangeDate(
                                          "plannedCompletion",
                                          newDate
                                        );
                                        props.action.setChanged(true);
                                        // onUnkownReleaseDate();
                                      }}
                                      className={`inputForm w-100 ${
                                        isDateRange && "custom-date-picker"
                                      } ${
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity
                                          ? "disabledBackground"
                                          : ""
                                      }`}
                                      minDate={new Date(1980, 0, 1)}
                                      maxDate={new Date(2999, 0, 1)}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText={"NOT SPECIFIED"}
                                      disabled={props.formDisabed}
                                    />
                                    {validation &&
                                    validation.response === false &&
                                    validation.property?.includes(
                                      "plannedCompletion"
                                    ) ? (
                                      <label className="validation">
                                        *planned Completion must have a value
                                      </label>
                                    ) : null}
                                  </label>
                                </div>
                              </div>
                            </div>
                          </div>

                          <div className="col-12">
                            <div className="row">
                              <div className="col-6 pl-0">
                                <div className="col-md-12">
                                  <label
                                    className={`labelForm voda-bold w-100`}
                                  >
                                    Pre Baseline Date
                                    <DatePicker
                                      selected={
                                        formData?.preBaseLineDate &&
                                        new Date(formData?.preBaseLineDate)
                                      }
                                      onChange={(startDate, e) => {
                                        e.preventDefault();
                                        onChangeDate(
                                          "preBaseLineDate",
                                          startDate
                                        );
                                        props.action.setChanged(true);
                                      }}
                                      className={`inputForm w-100 ${
                                        isDateRange && "custom-date-picker"
                                      }`}
                                      minDate={new Date(1980, 0, 1)}
                                      maxDate={new Date(2999, 0, 1)}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText={"NOT SPECIFIED"}
                                      disabled={props.formDisabed}
                                    />
                                  </label>
                                </div>
                              </div>
                              <div className="col-6 pr-0">
                                <div className="col-md-12">
                                  <label className="labelForm voda-bold w-100">
                                    Category
                                    <div className="d-flex mt-1">
                                      <div className="w-100">
                                        <Select
                                          menuPosition={"fixed"}
                                          options={
                                            formData?.plannedActivityCategoryResource &&
                                            dictionaryToArray(
                                              formData?.plannedActivityCategoryResource
                                            )
                                          }
                                          value={
                                            formData &&
                                            formData?.plannedActivityCategoryResource &&
                                            dictionaryToArray(
                                              formData?.plannedActivityCategoryResource
                                            ).filter(
                                              (x) =>
                                                x.key ===
                                                formData?.plannedActivityCategoryId
                                            )
                                          }
                                          onChange={(e) => {
                                            handlePlannedCategoryChange(
                                              e,
                                              "plannedActivityCategoryId",
                                              "plannedActivityCategoryName"
                                            );

                                            props.action.setChanged(true);
                                          }}
                                          onBlur={() => setInputValue("")}
                                          isSearchable
                                          isClearable
                                          getOptionLabel={(option) =>
                                            option.value
                                          }
                                          getOptionValue={(option) =>
                                            option["key"].toString()
                                          }
                                        ></Select>
                                      </div>
                                    </div>
                                  </label>
                                </div>
                              </div>
                              <div className="col-6 pl-0">
                                <div className="col-md-12">
                                  <label className="labelForm voda-bold w-100">
                                    LCM Category
                                    <div className="d-flex mt-1">
                                      <div className="w-100">
                                        <Select
                                          menuPosition={"fixed"}
                                          options={
                                            formData?.lcmCategoryResource &&
                                            dictionaryToArray(
                                              formData?.lcmCategoryResource
                                            )
                                          }
                                          value={
                                            formData &&
                                            formData?.lcmCategoryResource &&
                                            dictionaryToArray(
                                              formData?.lcmCategoryResource
                                            ).filter(
                                              (x) =>
                                                x.key == formData?.lcmCategories
                                            )
                                          }
                                          onChange={(e) => {
                                            handlePlannedCategoryChange(
                                              e,
                                              null,
                                              "lcmCategories"
                                            );

                                            props.action.setChanged(true);
                                          }}
                                          onBlur={() => setInputValue("")}
                                          isSearchable
                                          isClearable
                                          getOptionLabel={(option) =>
                                            option.value
                                          }
                                          getOptionValue={(option) =>
                                            option["key"].toString()
                                          }
                                        ></Select>
                                      </div>
                                    </div>
                                  </label>
                                </div>
                              </div>
                              <div className="col-6 pr-0">
                                <div className="col-md-12">
                                  <label className="labelForm voda-bold w-100">
                                    Priority
                                    <div className="d-flex mt-1">
                                      <div className="w-100">
                                        <Select
                                          menuPosition={"fixed"}
                                          options={
                                            formData?.priorityResource &&
                                            dictionaryToArray(
                                              formData?.priorityResource
                                            )
                                          }
                                          value={
                                            formData &&
                                            formData?.priorityResource &&
                                            dictionaryToArray(
                                              formData?.priorityResource
                                            ).filter(
                                              (x) =>
                                                x.value === formData?.priority
                                            )
                                          }
                                          onChange={(e) => {
                                            handlePlannedCategoryChange(
                                              e,
                                              null,
                                              "priority"
                                            );

                                            props.action.setChanged(true);
                                          }}
                                          onBlur={() => setInputValue("")}
                                          isSearchable
                                          isClearable
                                          getOptionLabel={(option) =>
                                            option.value
                                          }
                                          getOptionValue={(option) =>
                                            option["key"].toString()
                                          }
                                        ></Select>
                                      </div>
                                    </div>
                                  </label>
                                </div>
                              </div>
                              <div className="col-6 pl-0">
                                <div className="col-md-12">
                                  <TextInputComponent
                                    label="Team"
                                    labelCSS="mb-0"
                                    inputCSS="labelForm voda-bold mb-2"
                                    value={formData?.plannedActivityTeam}
                                    onChange={() => console.log("Term")}
                                    disabled={true}
                                  />
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </fieldset>

                      <fieldset className="fieldset p-0">
                        <label className="text-bb pl-0 mt-4">
                          Planning Details
                        </label>
                        <div className="row">
                          <div className="col-6 pl-0">
                            <div className="labelForm col-12">
                              <label className="voda-bold w-100 mb-0">
                                Driver
                                <div className="d-flex">
                                  <Select
                                    menuPosition={"fixed"}
                                    className="w-100"
                                    options={driverOption}
                                    value={
                                      driverOption &&
                                      formData &&
                                      driverOption.filter(
                                        (el) => el.key == formData.driverId
                                      )
                                    }
                                    onChange={(e) =>
                                      onChangeSelect("driverId", e)
                                    }
                                    // onKeyUp={(e) => onChangeSelect("driverId", e)}
                                    onBlur={() => setInputValue("")}
                                    isSearchable
                                    getOptionLabel={(option) =>
                                      option.value.toString()
                                    }
                                    getOptionValue={(option) =>
                                      option["key"].toString()
                                    }
                                    isDisabled={props.formDisabed}
                                  />
                                  {/* {tipologicaPermesso && (
															<button className="btn btn-link" onClick={() => setIsVisibleModalLookup(9)} type="button">
																<img style={{ height: 15 }} src={require("../../img/plus_B.png")} alt="+" />
															</button>
														)} */}
                                </div>
                              </label>
                            </div>
                          </div>
                          <div className="col-6 pr-0">
                            <div className="col-md-12">
                              <label className="labelForm voda-bold w-100">
                                Planning Risk<span className="red">*</span>
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      isDisabled={
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity ||
                                        props.formDisabed
                                      }
                                      options={planningRiskOption}
                                      value={
                                        planningRiskOption &&
                                        formData &&
                                        planningRiskOption.filter(
                                          (el) =>
                                            el.key == formData.planningRiskId
                                        )
                                      }
                                      onChange={(e) => {
                                        onChangeSelect("planningRiskId", e);
                                        props.action.setChanged(true);
                                      }}
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option["key"].toString()
                                      }
                                    ></Select>
                                  </div>
                                </div>
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "planningRisk"
                                ) ? (
                                  <label className="validation">
                                    *planning risk must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                            <Container show={!props?.isFromNetworkElement}>
                              <div className="col-12">
                                <label className="labelForm voda-bold w-100">
                                  Benefits (Driver details)
                                  <div className="d-flex">
                                    <Select
                                      menuPosition={"fixed"}
                                      className="w-100"
                                      options={benefitsOption}
                                      value={
                                        benefitsOption &&
                                        formData &&
                                        benefitsOption.filter(
                                          (el) => el.key == formData.benefitId
                                        )
                                      }
                                      onChange={(e) =>
                                        onChangeSelect("benefitId", e)
                                      }
                                      // onKeyUp={(e) =>
                                      //   onChangeSelect("benefitId", e)
                                      // }
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      getOptionLabel={(option) =>
                                        option.value.toString()
                                      }
                                      getOptionValue={(option) =>
                                        option["key"].toString()
                                      }
                                      isDisabled={props.formDisabed}
                                    />
                                  </div>
                                </label>
                              </div>
                            </Container>
                          </div>
                        </div>
                      </fieldset>

                      <fieldset className="fieldset p-0">
                        <label className="text-bb mt-4">
                          Financial Details
                        </label>
                        <div className="row">
                          <div className="col-md-6 pl-0">
                            <div className="col-12">
                              <label className="labelForm voda-bold w-100">
                                Is the activity part of the Detailed Budget?
                                <span className="red">*</span>
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      isDisabled={
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity ||
                                        disableActivityApproved ||
                                        props.formDisabed
                                      }
                                      options={
                                        formData?.budgetAvaibilityResource &&
                                        dictionaryToArray(
                                          formData?.budgetAvaibilityResource
                                        )
                                      }
                                      value={
                                        formData &&
                                        formData?.budgetAvaibilityResource &&
                                        dictionaryToArray(
                                          formData?.budgetAvaibilityResource
                                        ).filter(
                                          (x) =>
                                            x.key ===
                                            formData?.budgetAvailabilityId
                                        )
                                      }
                                      onChange={(e) => {
                                        onChangeSelect(
                                          "budgetAvailabilityId",
                                          e
                                        );
                                        props.action.setChanged(true);
                                      }}
                                      onBlur={() => setInputValue("")}
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
                                </div>
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "budgetAvailabilityId"
                                ) ? (
                                  <label className="validation">
                                    *This Field must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>

                          <div className="col-md-6">
                            <div className="col-md-12 pr-0">
                              <label className="labelForm voda-bold w-100">
                                Are the Initial Funds Loaded on SAP?
                                <span className="red">*</span>
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      isDisabled={
                                        disabledInitialFunds ||
                                        props.formDisabed
                                      }
                                      options={boolOptions}
                                      value={
                                        formData &&
                                        formData.localApproval != undefined
                                          ? boolOptions.find(
                                              (x) =>
                                                x.key ===
                                                formData?.localApproval
                                            )
                                          : null
                                      }
                                      onChange={(e) => {
                                        onChangeLocalApproval(e);
                                        props.action.setChanged(true);
                                      }}
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) => option["key"]}
                                    ></Select>
                                  </div>
                                </div>
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "localApproval"
                                ) ? (
                                  <label className="validation">
                                    *This Field must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>
                        </div>
                      </fieldset>
                      <fieldset className="fieldset p-0">
                        <label className="text-bb mt-4">Project Details</label>
                        <div className="row">
                          <div className="col-md-12 voda-bold mb-30">
                            <label className="  labelForm mb-0">
                              Responsibility Phase<span className="red">*</span>
                            </label>
                            <div className="flex w-100 mx-0 align-items-center row mx-0 py-2">
                              <div className="d-flex">
                                {formData?.responsibilityPhaseResource &&
                                formData?.responsibilityPhaseResource != null
                                  ? Object.keys(
                                      formData?.responsibilityPhaseResource
                                    ).map((name, index) => (
                                      <label
                                        className="labelForm voda-bold   mr-3 mb-0 d-flex align-items-center"
                                        key={name}
                                      >
                                        <input
                                          disabled={
                                            props.selectedDeploymentStatus
                                              ?.readOnlyPlannedActivity ||
                                            props.formDisabed
                                          }
                                          type="radio"
                                          onChange={(e) => {
                                            onChange(
                                              "responsibilityPhaseId",
                                              e
                                            );
                                            props.action.setChanged(true);
                                          }}
                                          name="responsibilityPhaseId"
                                          className=""
                                          id={name}
                                          value={name}
                                          checked={
                                            formData?.responsibilityPhaseId &&
                                            formData?.responsibilityPhaseId.toString() ===
                                              name
                                              ? true
                                              : false
                                          }
                                        />
                                        <label
                                          className="mb-0 ml-1"
                                          htmlFor={name}
                                        >
                                          {formData?.responsibilityPhaseResource &&
                                            formData
                                              ?.responsibilityPhaseResource[
                                              name
                                            ]}
                                        </label>
                                      </label>
                                    ))
                                  : null}
                              </div>

                              {tipologicaPermesso && !props.formDisabed && (
                                <button
                                  disabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity
                                  }
                                  className="btn btn-link add-bt"
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
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "responsibilityPhaseId"
                            ) ? (
                              <label className="validation">
                                *Responsibility phase must have a value
                              </label>
                            ) : null}
                          </div>

                          <div className="col-md-6">
                            <div className="form-group col-12 pl-0">
                              <label className="labelForm voda-bold w-100">
                                {formData && formData.localApproval === "YES"
                                  ? `Delivery Project Name-WBSCode`
                                  : "Delivery Project Name-WBSCode"}
                                {formData &&
                                  formData.localApproval === "YES" && (
                                    <span className="red">*</span>
                                  )}
                                <input
                                  disabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity ||
                                    props.formDisabed
                                  }
                                  type="text"
                                  onChange={(e) => {
                                    onChange("deliveryProjectName", e);
                                    props.action.setChanged(true);
                                  }}
                                  onKeyUp={(e) => {
                                    onChange("deliveryProjectName", e);
                                    props.action.setChanged(true);
                                  }}
                                  className="inputForm w-100"
                                  value={formData?.deliveryProjectName ?? ""}
                                />
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "deliveryProjectName"
                                ) ? (
                                  <label className="validation">
                                    *delivery Project Name must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                            <div className="col-12 pl-0">
                              <label className="labelForm voda-bold w-100">
                                Delivery Status<span className="red">*</span>
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      isDisabled={
                                        (edit && props.edit) ||
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity ||
                                        props.formDisabed ||
                                        plannedResourceSelected?.ruleLinkedDc ==
                                          14
                                      }
                                      options={
                                        DeliveryStatusArr
                                          ? DeliveryStatusArr
                                          : []
                                      }
                                      value={
                                        DeliveryStatusArr
                                          ? DeliveryStatusArr?.filter(
                                              (x) =>
                                                x.key ==
                                                formData?.deliveryStatusId
                                            )
                                          : null
                                      }
                                      onChange={(e) =>
                                        onChangeSelect("deliveryStatusId", e)
                                      }
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option["key"].toString()
                                      }
                                    ></Select>
                                    {/* {formData?.plannedActivityId !== undefined &&
                                !props.isDesignAspect ? (
                                  <button
                                    type="button"
                                    title="Update Planned Activity Status"
                                    className="btn btn-link"
                                    onClick={() =>
                                      formData?.plannedActivityId &&
                                      openModalStatus(
                                        formData?.plannedActivityId
                                      )
                                    }
                                  >
                                    <IoIosRefresh
                                      color={`${darkMode ? "white" : "black"}`}
                                    />
                                  </button>
                                ) : null} */}
                                  </div>
                                  {((edit && props.edit) ||
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity ||
                                    props.formDisabed) &&
                                  formData?.plannedActivityId !== undefined &&
                                  !props.isDesignAspect ? (
                                    <button
                                      type="button"
                                      title="Update Planned Activity Status"
                                      className="btn btn-link"
                                      onClick={() =>
                                        formData?.plannedActivityId &&
                                        openModalStatus(
                                          formData?.plannedActivityId
                                        )
                                      }
                                    >
                                      <IoIosRefresh
                                        color={`${
                                          darkMode ? "white" : "black"
                                        }`}
                                      />
                                    </button>
                                  ) : null}
                                  {/* {tipologicaPermesso &&
                                !props.formDisabed &&
                                !props.lcmId && (
                                  <button
                                    disabled={
                                      props.selectedDeploymentStatus
                                        ?.readOnlyPlannedActivity
                                    }
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
                                )} */}
                                </div>
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "deliveryStatusId"
                                ) ? (
                                  <label className="validation">
                                    *Delivery status must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>
                          <div className="col-md-6">
                            <div className="form-group col-12 pr-0">
                              <label className="labelForm voda-bold w-100">
                                Delivery Project Id (PPM ID)
                                <input
                                  disabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity ||
                                    props.formDisabed
                                  }
                                  type="text"
                                  onChange={(e) => {
                                    onChange("deliveryProjectId", e);
                                    props.action.setChanged(true);
                                  }}
                                  onKeyUp={(e) => {
                                    onChange("deliveryProjectId", e);
                                    props.action.setChanged(true);
                                  }}
                                  className="inputForm w-100"
                                  value={formData?.deliveryProjectId}
                                />
                              </label>
                            </div>
                          </div>
                        </div>
                      </fieldset>

                      <div className="col-12 pl-0 mt-4">
                        <fieldset className="fieldset p-0">
                          <label className="text-bb mb-40">
                            Engineering Risk Evaluation
                            <p
                              style={{
                                fontSize: "16px",
                                fontWeight: "initial",
                              }}
                            >
                              What is the risk to our customers and network if
                              the planned activity is not completed by the
                              completion date?
                            </p>
                          </label>

                          <div className="row">
                            <div className="col-md-12">
                              <div className="row">
                                <div className="col-6">
                                  <label className="labelForm voda-bold w-100">
                                    Risk Level
                                    {props?.isFromNetworkElement ? (
                                      ""
                                    ) : (
                                      <span className="red">*</span>
                                    )}
                                    <div className="d-flex">
                                      <div className="w-100">
                                        <Select
                                          menuPosition={"fixed"}
                                          isDisabled={
                                            props.selectedDeploymentStatus
                                              ?.readOnlyPlannedActivity ||
                                            props.formDisabed
                                          }
                                          options={
                                            formData?.riskResource &&
                                            dictionaryToArray(
                                              formData?.riskResource
                                            )
                                          }
                                          value={
                                            formData &&
                                            formData?.riskResource &&
                                            dictionaryToArray(
                                              formData?.riskResource
                                            ).filter(
                                              (x) =>
                                                x.key === formData?.riskEngId
                                            )
                                          }
                                          onChange={(e) =>
                                            onChangeSelect("riskEngId", e)
                                          }
                                          onBlur={() => setInputValue("")}
                                          isSearchable
                                          isClearable
                                          getOptionLabel={(option) =>
                                            option.value
                                          }
                                          getOptionValue={(option) =>
                                            option["key"].toString()
                                          }
                                        ></Select>
                                      </div>
                                      {tipologicaPermesso &&
                                        !props.formDisabed && (
                                          <button
                                            disabled={
                                              props.selectedDeploymentStatus
                                                ?.readOnlyPlannedActivity
                                            }
                                            className="btn btn-link"
                                            onClick={() =>
                                              setIsVisibleModalLookup(8)
                                            }
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
                                    {validation &&
                                    validation.response === false &&
                                    validation.property?.includes(
                                      "riskEngId"
                                    ) ? (
                                      <label className="validation">
                                        *Risk Engineering Evaluation must have a
                                        value
                                      </label>
                                    ) : null}
                                  </label>

                                  {/* {validation && validation.response === false && validation.property === "deliveryStatusId" ? <label className="validation">*Risk - Engineering Evaluation must have a value</label> : null} */}
                                </div>
                                <div className="col-6">
                                  <label className="labelForm voda-bold w-100 mb-0">
                                    Risk - Engineering Notes
                                    <input
                                      type="text"
                                      disabled={
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity ||
                                        props.formDisabed
                                      }
                                      onChange={(e) => {
                                        onChange("riskEngineeringNotes", e);
                                        props.action.setChanged(true);
                                      }}
                                      onKeyUp={(e) => {
                                        onChange("riskEngineeringNotes", e);
                                        props.action.setChanged(true);
                                      }}
                                      className="inputForm w-100"
                                      value={formData?.riskEngineeringNotes}
                                    />
                                  </label>
                                </div>
                              </div>
                            </div>
                          </div>
                        </fieldset>
                      </div>
                      <div className="col-12 pl-0 mt-4">
                        <fieldset className="fieldset p-0">
                          <label className="text-bb mt-4">
                            Operations Risk Evaluation
                            <p
                              style={{
                                fontSize: "16px",
                                fontWeight: "initial",
                              }}
                            >
                              What is the risk to our customers and network if
                              the planned activity is not completed by the
                              completion date?
                            </p>
                          </label>
                          <div className="row">
                            <div className="col-6">
                              <label className="labelForm voda-bold w-100">
                                Risk Level
                                {props?.isFromNetworkElement ? (
                                  ""
                                ) : (
                                  <span className="red">*</span>
                                )}
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      isDisabled={
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity ||
                                        props.formDisabed
                                      }
                                      options={
                                        formData?.riskResource &&
                                        dictionaryToArray(
                                          formData?.riskResource
                                        )
                                      }
                                      value={
                                        formData &&
                                        formData?.riskResource &&
                                        dictionaryToArray(
                                          formData?.riskResource
                                        ).filter(
                                          (x) => x.key === formData?.riskOpeId
                                        )
                                      }
                                      onChange={(e) =>
                                        onChangeSelect("riskOpeId", e)
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
                                  {tipologicaPermesso && !props.formDisabed && (
                                    <button
                                      disabled={
                                        props.selectedDeploymentStatus
                                          ?.readOnlyPlannedActivity
                                      }
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
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes("riskOpeId") ? (
                                  <label className="validation">
                                    *Risk Operational Evaluation must have a
                                    value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                            <div className="col-6">
                              <label className="labelForm voda-bold w-100 mb-0">
                                Risk Evaluation Notes
                                <input
                                  type="text"
                                  disabled={
                                    props.selectedDeploymentStatus
                                      ?.readOnlyPlannedActivity ||
                                    props.formDisabed
                                  }
                                  onChange={(e) => {
                                    onChange("riskOperationalNotes", e);
                                    props.action.setChanged(true);
                                  }}
                                  onKeyUp={(e) => {
                                    onChange("riskOperationalNotes", e);
                                    props.action.setChanged(true);
                                  }}
                                  className="inputForm w-100"
                                  value={formData?.riskOperationalNotes}
                                />
                              </label>
                            </div>
                          </div>
                        </fieldset>
                      </div>
                      <div className="col-12 pl-0 mt-20 mb-20">
                        <button
                          type="button"
                          className="further-btn"
                          disabled={props.formDisabed}
                          onClick={() => setShowFurther(!showfurther)}
                        >
                          Click for further details
                        </button>
                      </div>
                    </>
                  )}
                </div>

                {showfurther && !isNoPA && (
                  <>
                    <div className="col-md-12 row mx-0 d-flex align-content-start">
                      <div className="col-12 pl-0">
                        <label className="text-bb mt-4">
                          Financial Details
                        </label>
                      </div>
                      <div className="form-group col-6 pl-0">
                        <label className="labelForm voda-bold w-100 pr-0">
                          <div className="mb-3">Budget Value</div>
                          <input
                            disabled={
                              props.selectedDeploymentStatus
                                ?.readOnlyPlannedActivity
                            }
                            type="number"
                            min="0.00"
                            max="1000000000.00"
                            step="1000"
                            onChange={(e) => {
                              onChange("budgetValue", e);
                              props.action.setChanged(true);
                            }}
                            onKeyUp={(e) => {
                              onChange("budgetValue", e);
                              props.action.setChanged(true);
                            }}
                            className="inputForm w-100 mt-0"
                            value={formData?.budgetValue}
                          />
                        </label>
                      </div>

                      <div className="col-md-6">
                        <label className="labelForm voda-bold w-100 col-12 pl-0">
                          <div className="mb-3">Currency</div>
                          <Select
                            menuPosition={"fixed"}
                            isDisabled={
                              props.selectedDeploymentStatus
                                ?.readOnlyPlannedActivity
                            }
                            options={currencyOption}
                            className="ml-1 voda-bold mt-0"
                            placeholder="Currency"
                            value={
                              formData &&
                              currencyOption.filter(
                                (x) => x.value === formData?.currency
                              )
                            }
                            onChange={(e) => {
                              let copy = {
                                ...formData,
                              } as PlannedActivityDtoUpdate;
                              copy.currency = e?.["value"] ?? "";
                              props.action.setChanged(true);
                              setFormData(copy);
                            }}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) => option.value}
                          ></Select>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("currency") ? (
                            <label className="validation">
                              *Currency must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>

                      <div className="form-group col-6 pl-0 ">
                        <label className="labelForm voda-bold w-100">
                          Budget Tracking Id
                          <input
                            disabled={
                              props.selectedDeploymentStatus
                                ?.readOnlyPlannedActivity
                            }
                            type="text"
                            onChange={(e) => {
                              onChange("budgetTrackingId", e);
                              props.action.setChanged(true);
                            }}
                            onKeyUp={(e) => {
                              onChange("budgetTrackingId", e);
                              props.action.setChanged(true);
                            }}
                            className="inputForm w-100"
                            value={formData?.budgetTrackingId}
                          />
                        </label>
                      </div>

                      <div className="col-6">
                        <label className="labelForm voda-bold w-100">
                          Program
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.programResource &&
                                  dictionaryToArray(formData?.programResource)
                                }
                                value={
                                  formData &&
                                  formData?.programResource &&
                                  dictionaryToArray(
                                    formData?.programResource
                                  ).filter((x) => x.key === formData?.programId)
                                }
                                onChange={(e) => onChangeSelect("programId", e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              ></Select>
                            </div>
                            {tipologicaPermesso && !props.formDisabed && (
                              <button
                                disabled={
                                  props.selectedDeploymentStatus
                                    ?.readOnlyPlannedActivity
                                }
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
                            )}
                          </div>
                        </label>

                        {/* {validation && validation.response === false && validation.property === "deliveryStatusId" ? <label className="validation">*Risk - Engineering Evaluation must have a value</label> : null} */}
                      </div>
                      <div className="col-6 pl-0">
                        <label className="labelForm voda-bold w-100">
                          Project Owner
                          <input
                            type="text"
                            onChange={(e) => {
                              onChange("projectOwner", e);
                              props.action.setChanged(true);
                            }}
                            onKeyUp={(e) => {
                              onChange("projectOwner", e);
                              props.action.setChanged(true);
                            }}
                            className="inputForm w-100"
                            value={formData?.projectOwner}
                          />
                        </label>
                      </div>

                      {/* <div className="col-md-6">
                        <label className="labelForm voda-bold w-100 col-12 pl-0">
                          <div className="mb-3">Budget Owner</div>
                          <Select
                            menuPosition={"fixed"}
                            className="ml-1 voda-bold mt-0"
                            placeholder="Budget Owner"
                            options={
                              formData?.budgetOwnerResource &&
                              dictionaryToArray(formData?.budgetOwnerResource)
                            }
                            value={
                              formData &&
                              formData?.budgetOwnerResource &&
                              dictionaryToArray(
                                formData?.budgetOwnerResource
                              ).filter((x) => x.key === formData?.budgetOwnerId)
                            }
                            onChange={(e) => {
                              handlePlannedCategoryChange(
                                e,
                                "budgetOwnerId",
                                "budgetOwner"
                              );
                              props.action.setChanged(true);
                            }}
                            isSearchable
                            isClearable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </label>
                      </div> */}

                      <div className="col-md-12 p-0">
                        <label className="labelForm voda-bold w-100 col-12 pl-0 mt-4">
                          Notes
                          <textarea
                            disabled={
                              props.selectedDeploymentStatus
                                ?.readOnlyPlannedActivity
                            }
                            onChange={(e) => {
                              onChange("notes", e);
                              props.action.setChanged(true);
                            }}
                            onKeyUp={(e) => {
                              onChange("notes", e);
                              props.action.setChanged(true);
                            }}
                            className="inputForm w-100"
                            value={formData?.notes}
                          />
                        </label>
                      </div>
                      <div className="col-12 p-0">
                        {edit === true ? (
                          <div className="row">
                            <div className="form-group col-6 pl-0">
                              <label className="labelForm voda-bold w-100 col-12 pr-0">
                                Last Modified
                                <input
                                  readOnly={true}
                                  className="inputForm w-100 voda-regular"
                                  type="text"
                                  value={formatDateWithTime(
                                    formData?.lastModified
                                  )}
                                />
                              </label>
                            </div>
                            <div className="form-group col-6">
                              <label className="labelForm voda-bold w-100 col-12 pr-0">
                                Last Modified By
                                <input
                                  readOnly={true}
                                  className="inputForm w-100 voda-regular"
                                  type="text"
                                  value={formData?.lastModifiedBy}
                                />
                              </label>
                            </div>
                          </div>
                        ) : null}
                      </div>
                    </div>
                  </>
                )}

                <div className="form-group col-12 mb-4 mt-2 pl-0">
                  <div className="col-12 mb-4 mt-2 pl-0">
                    <button
                      className="  voda-bold btn btn-link px-4 btnHeader cancel"
                      onClick={() => props.action.setShowForm(false)}
                      type="button"
                    >
                      Cancel
                    </button>
                    <button
                      className={` voda-bold btn btn-danger mr-3 px-4 btnHeader ${
                        props.lcmRedirect ? "disabledCursor" : ""
                      }`}
                      type="button"
                      onClick={() => addOnList()}
                      disabled={
                        props.lcmRedirect === true
                          ? true
                          : props.selectedDeploymentStatus
                              ?.readOnlyPlannedActivity || props.formDisabed
                      }
                      data-toggle="tooltip"
                      data-placement="top"
                      title={
                        props.prevPage === "generatelcmdb" &&
                        props.lcmRedirect === true
                          ? `Saving is disabled due to redirection from LCM Export screen`
                          : ""
                      }
                    >
                      Save Planned Activity
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </form>
        ) : null}
      </div>
    </div>
  );
};

export default LcmEngPlannedActivity;
