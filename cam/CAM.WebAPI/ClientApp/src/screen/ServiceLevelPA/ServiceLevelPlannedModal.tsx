import React, {
  SetStateAction,
  forwardRef,
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
} from "react";
import { Form, Modal } from "react-bootstrap";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useSelector } from "react-redux";
import Select from "react-select";
import Container from "../../Components/Container";
import ModalConfirm from "../../Components/ModalConfirm";
import Paginate from "../../Components/PaginationComponent";
import TH from "../../Components/TableCrud/TableCrudTH";
import ActivityStatusContainer from "../../Containers/Lookup/ActivityStatusContainer";
import Benefits from "../../Containers/Lookup/BenefitsContainer";
import BudgetAvailabilityContainer from "../../Containers/Lookup/BudgetAvailabilityContainer";
import DeliveryStatusContainer from "../../Containers/Lookup/DeliveryStatusContainer";
import Driver from "../../Containers/Lookup/DriverContainer";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";
import OperationalRiskContainer from "../../Containers/Lookup/OperationalRiskContainer";
import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import PlannedActivityResourceContainer from "../../Containers/Lookup/PlannedActivityResourceContainer";
import PlanningActivityStatusContainer from "../../Containers/Lookup/PlanningActivityStatusContainer";
import PlanningRisk from "../../Containers/Lookup/PlanningRiskContainer";
import Program from "../../Containers/Lookup/ProgramContainer";
import ResponsibilityPhaseContainer from "../../Containers/Lookup/ResponsibilityPhaseContainer";
import SecurityTireZone from "../../Containers/Lookup/SecurityTireZoneContainer";
import ServiceMaster from "../../Containers/Lookup/ServiceMasterContainer";
import SharedLookUp from "../../Containers/Lookup/SharedLookUpContainer";
import SupportedService from "../../Containers/Lookup/SupportedServiceContainer";
import AssetsDetailsContainer from "../../Containers/Lookup/AssetsDetailsContainer";
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
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  QueryObjectGrid,
  stateConfirm,
} from "../../Model/Common";
import { DesignAspectDtoUpdate } from "../../Model/DesignAspects";
import { DeploymentStatusDto } from "../../Model/LookUp/DeploymentStatus";
import { LocationDto } from "../../Model/LookUp/Location";
import { PlannedActivityResourceDto } from "../../Model/LookUp/PlannedActivityResource";
import {
  DaAssetMigrationDto,
  DaAssetMigrationGrid,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../Model/LookUp/AssetMigrationModels";
import { NetworkElementAssociated } from "../../Model/LcmEngineering";
import { NetworkElementAsPlannedDtoCreate } from "../../Model/NetworkElementAsPlanned";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityDtoGrid,
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
import { GetPlannedActivityCreateResource } from "../../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { deletePlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityDeleteAction";
import {
  EditPlannedActivity,
  GetPlannedActivityEditResource,
} from "../../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import {
  GetFilterColumPlannedActivity,
  GetPlannedActivityGrid,
} from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import {
  CreateServicePlan,
  GetServicePlanCreateResource,
} from "../../Redux/Action/ServicePlan/ServicePlanCreateAction";
import { GetAssetsPlatformMigrationGrid } from "../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import {
  EditDesignAspect,
  GetUpdateDAMigration,
} from "../../Redux/Action/DesignAspect/DesignAspectEditAction";
import { CreatDesignAspect } from "../../Redux/Action/DesignAspect/DesignAspectCreateAction";
import {
  GetDesignComponentFamily,
  GetServicesAndNetworks,
} from "../../Redux/Action/DesignAspect/DesignAspectBuildCommonAction";
import {
  GetNetworkElementOpCo,
  GetPlatformMigarteGetDesignComponentList,
} from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCommonAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import setLoader from "../../Redux/Action/LoaderAction";
import { ApiCallWithErrorHandling } from "../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../Business/DesignAspectsBusiness";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import ManageMigration from "../PlannedActivities/ManageMigrationModal";
import UpdatePlannedActivityStatusModal from "../PlannedActivities/UpdatePlannedActivityStatusModal";
import AssetsDetailsGrid from "../Lookup/AssetPlatform/AssetsDetailsGrid";
import { useNavigate, useLocation } from "react-router-dom";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions, DialogProps } from "@mui/material";
import { Box } from "@mui/material";
import {
  DropdownInputComponent,
  MultiSelectComponent,
  TextInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import BuildBagItemComponent from "../../Containers/BuildBagComponent";
import ViewMappedComponent from "../../Containers/ViewMappedComponent";
import { useTheme } from "../../Context/ThemeContext";
import { BiCopy } from "react-icons/bi";
import { FaRegEye } from "react-icons/fa";
import { IoClose } from "react-icons/io5";
import { IoIosRefresh } from "react-icons/io";
import { LuFolderSync } from "react-icons/lu";
import { MdDelete, MdEdit } from "react-icons/md";
import moment from "moment";

interface ServiceLevelPAProps {
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
  onChangeOpco?: (e: any) => void;
  onChangeDCF?: (e: any) => void;
  opcosResource?: { [key: string]: string };
  dcFsResource?: { [key: string]: string };
  opcoIdValue?: number;
  dcfIdValue?: number;
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
  dcfResource?: Record<number, string>;
  onRequestParentSave?: () => Promise<void>;
  isAssetDetailContainer?: boolean;
}

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

const ServiceLevelPlannedActivity: React.FC<ServiceLevelPAProps> = (props) => {
  const [isVisibleFiltri, setIsVisibleFiltri] = useState("");
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isBuildBagItemFlag, setIsBuildBagItemFlag] = useState<boolean>(false);
  const [viewBagFlag, setViewBagFlag] = useState<boolean>(false);
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
  const [serviceDropdownOptions, setServiceDropdownOptions] = useState<
    Array<any>
  >([]);
  const [resourceDesignComponent, setResourceDesignComponent] = useState<
    { key: number; value: string; color?: string | "#000000" }[] | undefined
  >([]);
  const [unknownDC, setUnknownDC] = useState<
    { key: number; value: string }[] | undefined
  >([]);
  const [designComponentSingleArray, setDesignComponentSingleArray] = useState<
    { key: number; value: string }[]
  >([]);
  const [transientDC, setTransientDC] = useState<any>();
  const [isConsigliati, setIsConsigliati] = useState<boolean>(false);
  const [startEndFlag, setStartEndFlag] = useState<boolean>(false);
  const { tipologicaPermesso } = useAuth();
  const [newNetworkAssocitate, setNewNetworkAssocitate] =
    useState<NetworkElementAssociated[]>();
  const [isServiceCrudModalOpen, setIsServiceCrudModalOpen] = useState(false);
  const [index, setIndex] = useState<number | undefined>(undefined);
  const [buildBagRes, setBuildBagRes] = useState<
    Array<{ key: number; text: string; isColour: boolean }>
  >([]);
  const [dcfSelectedIds, setDcfSelectedIds] = useState<number[]>([]);
  const [deliveryProjectNamePart, setDeliveryProjectNamePart] = useState("");
  const [wbsCodePart, setWbsCodePart] = useState("");
  const [hasFetchedAssets, setHasFetchedAssets] = useState(false);
  const [plannedDcfId, setPlannedDcfId] = useState<number>(0);
  const handleDcfMultiSelect = (
    selected: Array<{ label: string; value: number }>
  ) => {
    const selectedIds = selected.map((item) => item.value);
    const copy = { ...formData };
    copy.designComponentFamilyIdList = selectedIds;
    setFormData(copy);
  };

  function stripHtml(html: string) {
    const tmp = document.createElement("DIV");
    tmp.innerHTML = html;
    return tmp.textContent || tmp.innerText || "";
  }

  const getFiltersData = (state: RootState) =>
    state.plannedActivityGridReducer.filter;
  let filterData = useSelector(getFiltersData);

  const { resetFilter, checkFilterinValue } =
    useFilterTableCrud<PlannedActivityQueryObjectGrid>(
      props.action.Filter,
      //   GetFilterColumPlannedActivity,
      undefined,
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
    CreateServicePlan,
    EditPlannedActivity
  );

  const dtoNewResourceState = (state: RootState) => {
    const root = state.servicePlanCreateReducer.ServicePlanDtoCreate;
    return root;
  };
  let createResource = useSelector(dtoNewResourceState);

  const [plannedFormArray, setPlannedFormArray] = useState(
    props.formArray ?? []
  );
  const [assetsData, setAssetsData] = useState<DaAssetMigrationDto[]>([]);
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

  const handleModalLookup = (value: number) => {
    setIsVisibleModalLookup(value); // store the value
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
      if (plannedResourceSelected?.ruleLinkedDc === 17) {
        if (
          copy?.projectDescription === null ||
          copy?.projectDescription === undefined ||
          copy?.projectDescription === ""
        ) {
          addInvalidProperty("projectDescription");
        }

        const deliveryProject = copy?.deliveryProjectName ?? "";
        const parts = deliveryProject.split("-");

        const projectName = parts[0]?.trim();
        const wbCode = parts[1]?.trim();

        if (!projectName) {
          addInvalidProperty("deliveryProjectNamePart");
        }

        if (!wbCode) {
          addInvalidProperty("deliveryProjectName-WB");
        }
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
      if (
        (plannedResourceSelected?.ruleLinkedDc === 18 ||
          plannedResourceSelected?.ruleLinkedDc === 19) &&
        (copy?.plannedDesignComponentFamilyId === null ||
          copy?.plannedDesignComponentFamilyId === undefined ||
          copy?.plannedDesignComponentFamilyId === 0)
      ) {
        addInvalidProperty("plannedDesignComponentFamilyId");
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
    // console.log("PA Validation", copyValidation);
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
      console.log(createResource);
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
        copy.plannedActivityResourceId = obj?.plannedActivityResourceId;
      } else {
        copy.plannedActivityResourceId = undefined;
        copy.buildBagId = undefined;
      }
      if (!props.isInLcm) {
        copy.buildBagId = props.buildBagIds;
        copy.designComponentId = props.designComponent?.key;
      }
      setPlannedResourceSelected(undefined);
      setNoPAFormData(copy);
      setFormData(copy);
    }
  }, [createResource, edit]);

  const designComponentArray = Array.isArray(
    formData?.designComponentFamilyResource
  )
    ? formData?.designComponentFamilyResource
    : [];
  const plannnedesignComponentArray = Array.isArray(
    formData?.plannedDcfResource
  )
    ? formData?.plannedDcfResource
    : [];
  useEffect(() => {
    if (formData?.deliveryProjectName) {
      const [name = "", code = ""] = formData.deliveryProjectName.split("-");
      setDeliveryProjectNamePart(name);
      setWbsCodePart(code);
    }
  }, [formData?.deliveryProjectName]);

  useEffect(() => {
    setPlannedFormArray(props.formArray ?? []);

    if (props.lcmDeploymentStatusString?.toLowerCase().trim() == "planned") {
      const obj = getPlannedPAId(createResource);
      if (!props.isInLcm) {
        obj.buildBagId = props.buildBagIds;
        obj.designComponentId = props.designComponent?.key;
      }
      setFormData(obj);
    } else {
      //   setFormData({
      //     ...createResource,
      //     buildBagId: props.isInLcm ? undefined : props.buildBagIds,
      //   });
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
      target?.ruleLinkedDc === 16
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
        plannedDesignComponentFamilyId: x?.plannedDesignComponentFamilyId,
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

  const addOnList = async (skipValidation: boolean = false) => {
    if (formData !== null && formData !== undefined) {
      const selectedPA =
        formData?.plannedActivityResource &&
        dictionaryToArrayPlannedActivityResourceDto(
          formData?.plannedActivityResource
        ).find((x) => x.key == formData?.plannedActivityResourceId);

      const copy = {
        ...formData,
        opCoId: props.opcoId,
        serviceMasterId: formData?.serviceMasterId ?? 0,
        currency:
          formData?.budgetValue == null ||
          formData?.budgetValue == undefined ||
          formData?.budgetValue == ""
            ? undefined
            : formData?.currency,
        designComponentFamilyId: props.designComponentFamilyId,
        plannedDesignComponentFamilyId:
          formData?.plannedActivityResource &&
          dictionaryToArrayPlannedActivityResourceDto(
            formData?.plannedActivityResource
          ).filter(
            (x) =>
              x.value.plannedActivityResourceId ===
                formData?.plannedActivityResourceId &&
              x.value.ruleLinkedDc !== 18 &&
              x.value.ruleLinkedDc !== 19
          ).length === 1
            ? props.designComponentFamilyId
            : formData?.plannedDesignComponentFamilyId,
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

        plaftformMigrationDcfResources: {
          daAssetMigrationDtoGrid: assetsData.map((asset) => {
            const { uniqueId, ...assetWithoutUniqueId } = asset;
            return {
              ...assetWithoutUniqueId,
              plannedActivityId: formData?.plannedActivityId || 0,
              opcoId: props.opcoId || 401,
            };
          }),
        },
      } as PlannedActivityDtoUpdate;

      setFormData(copy);

      if (skipValidation || validazioneClient(copy)?.response === true) {
        const copyFormArray: PlannedActivityDtoUpdate[] = props.formArray
          ? props.formArray.map((el) => {
              return {
                ...el,
                opCoId: props.opcoId,
                designComponentFamilyId: props.designComponentFamilyId,
                plannedDesignComponentFamilyId:
                  formData?.plannedActivityResource &&
                  dictionaryToArrayPlannedActivityResourceDto(
                    formData?.plannedActivityResource
                  ).filter(
                    (x) =>
                      x.value.plannedActivityResourceId ===
                        formData?.plannedActivityResourceId &&
                      x.value.ruleLinkedDc !== 18 &&
                      x.value.ruleLinkedDc !== 19
                  ).length === 1
                    ? props.designComponentFamilyId
                    : formData?.plannedDesignComponentFamilyId,
              };
            })
          : [];

        if (edit && index !== undefined) {
          if (isNoPA) {
            copyFormArray[index] = {
              ...copy,
              opCoId: props.opcoId,
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
              designComponentFamilyId: props.designComponentFamilyId ?? 0,
              plannedDesignComponentFamilyId:
                formData?.plannedActivityResource &&
                dictionaryToArrayPlannedActivityResourceDto(
                  formData?.plannedActivityResource
                ).filter(
                  (x) =>
                    x.value.plannedActivityResourceId ===
                      formData?.plannedActivityResourceId &&
                    x.value.ruleLinkedDc !== 18 &&
                    x.value.ruleLinkedDc !== 19
                ).length === 1
                  ? props.designComponentFamilyId
                  : formData?.plannedDesignComponentFamilyId,
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

              plaftformMigrationDcfResources:
                copy.plaftformMigrationDcfResources,
            };
          } else {
            if (props.isDesignAspect || props?.isFromNetworkElement) {
              copyFormArray[index] = {
                ...copy,
                plannedCompletionValue: formatTimeLocal(
                  copy.plannedCompletion!
                ),
                preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),

                plaftformMigrationDcfResources:
                  copy.plaftformMigrationDcfResources,
              };
            } else if (props.isInLcm) {
              copyFormArray[index] = {
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

                plaftformMigrationDcfResources:
                  copy.plaftformMigrationDcfResources,
              };
            } else {
              copyFormArray[index] = {
                ...copy,
                plannedCompletionValue: formatTimeLocal(
                  copy.plannedCompletion!
                ),
                preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),

                plaftformMigrationDcfResources:
                  copy.plaftformMigrationDcfResources,
              };
            }
          }
          props.action.AddOnList(copyFormArray);
        } else {
          // For new planned activities
          if (isNoPA) {
            const newFormArray = {
              ...formData,
              opCoId: props.opcoId,
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
              // ✅ PRESERVE ASSETS DATA
              plaftformMigrationDcfResources:
                copy.plaftformMigrationDcfResources,
            } as PlannedActivityDtoUpdate;
            copyFormArray.push(newFormArray);
          } else {
            const newItem = props.isDesignAspect
              ? {
                  ...copy,
                  plannedCompletionValue: formatTimeLocal(
                    copy.plannedCompletion!
                  ),
                  preBaseLineDateValue: formatTimeLocal(copy.preBaseLineDate!),
                  // ✅ PRESERVE ASSETS DATA
                  plaftformMigrationDcfResources:
                    copy.plaftformMigrationDcfResources,
                }
              : {
                  ...copy,
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

                  plaftformMigrationDcfResources:
                    copy.plaftformMigrationDcfResources,
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
          }
        }

        props.action.AddOnList(copyFormArray);
      } else {
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

  const handleAssetsChange = (assets: DaAssetMigrationDto[]) => {
    setAssetsData(assets);
    const copy = { ...formData } as PlannedActivityDtoUpdate;

    if (!copy.plaftformMigrationDcfResources) {
      copy.plaftformMigrationDcfResources = {
        daAssetMigrationDtoGrid: [],
      };
    }

    copy.plaftformMigrationDcfResources.daAssetMigrationDtoGrid = assets.map(
      (asset) => {
        const { uniqueId, ...assetWithoutUniqueId } = asset;
        return {
          ...assetWithoutUniqueId,
          plannedActivityId: formData?.plannedActivityId || 0,
          opcoId: props.opcoId || 401,
        };
      }
    );
    setFormData(copy);
  };

  // const OpCoRefillData = (value: Array<any>) => {
  //   //   var data = value.map(x => x.id, x.description )
  //   var obj = value.reduce(
  //     (acc, item) => ({ ...acc, [item.id]: item.description }),
  //     {}
  //   );
  //   if (formData && formData?.opcosResource)
  //     formData.opcosResource = obj as { [key: string]: string };
  //   setFormData(formData);
  // };

  // const ReturnLookupContainer = (value: number) => {
  //   switch (value) {
  //     case 1:
  //       return (
  //         <OpcoContainer
  //           returnObject={OpCoRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></OpcoContainer>
  //       );
  //     case 2:
  //       return (
  //         <ActivityStatusContainer
  //           returnObject={ActivityStatusRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></ActivityStatusContainer>
  //       );

  //     case 3:
  //       return (
  //         <PlanningActivityStatusContainer
  //           returnObject={PlanningActivityStatusRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlanningActivityStatusContainer>
  //       );
  //     case 4:
  //       return (
  //         <ResponsibilityPhaseContainer
  //           returnObject={ResponsibilityPhaseRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></ResponsibilityPhaseContainer>
  //       );
  //     case 6:
  //       return (
  //         <PlannedActivityResourceContainer
  //           isDesignAspect={props.isDesignAspect}
  //           isForAddAsset={props.isForAddAsset}
  //           isForEditAsset={props.isForEditAsset}
  //           returnObject={PlannedActivityResource}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlannedActivityResourceContainer>
  //       );
  //     case 7:
  //       return (
  //         <BudgetAvailabilityContainer
  //           returnObject={BudgetAvailabilityRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></BudgetAvailabilityContainer>
  //       );
  //     case 8:
  //       return (
  //         <OperationalRiskContainer
  //           returnObject={OperationalRiskRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></OperationalRiskContainer>
  //       );
  //     case 9:
  //       return (
  //         <Driver
  //           returnObject={DriverRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Driver>
  //       );
  //     case 10:
  //       return (
  //         <Benefits
  //           returnObject={BenefitsRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Benefits>
  //       );
  //     case 11:
  //       return (
  //         <PlanningRisk
  //           returnObject={PlanningRiskRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlanningRisk>
  //       );
  //     case 12:
  //       return (
  //         <Program
  //           returnObject={ProgramRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Program>
  //       );
  //     case 13:
  //       return (
  //         <AssetsDetailsContainer
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //           opcoId={props.opcoId}
  //           dcfId={props.designComponentFamilyId}
  //           plannedDcfId={formData?.plannedDesignComponentFamilyId ?? 0}
  //           paId={formData?.plannedActivityId ?? 0}
  //           initialAssets={assetsData}
  //           onAssetsChange={handleAssetsChange}
  //           isAssetDetailContainer={props?.isAssetDetailContainer}
  //         />
  //       );

  //     default:
  //       return;
  //   }
  // };

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
            x.value.ruleLinkedDc === 16)
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
      setFormData((prev) => ({
        ...prev,
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
        if (plannedResourceSelected?.ruleLinkedDc == 13) {
          setRuleDCIdFlag(false);
        } else {
          setRuleDCIdFlag(false);
          removeUnknownState();
        }
      }
    }
    if (plannedResourceSelected?.ruleLinkedDc !== 17) {
      setFormData((prev) => ({
        ...prev,
        designComponentFamilyIdList: [],
      }));
    }
  }, [plannedResourceSelected]);

  useEffect(() => {
    const fetchDesignComponentSingleArray = async () => {
      if (
        plannedResourceSelected?.ruleLinkedDc !== 19 &&
        plannedResourceSelected?.ruleLinkedDc !== 18
      ) {
        return;
      }

      try {
        const data = await GetPlatformMigarteGetDesignComponentList(
          (props?.designComponentFamily ?? "").trim(), // remove extra spaces
          props?.dcfResource ?? {} // new dictionary format
        );
        setDesignComponentSingleArray(data || []);
      } catch (error) {
        console.error("Error fetching design components:", error);
        setDesignComponentSingleArray([]);
      }
    };

    fetchDesignComponentSingleArray();
  }, [plannedResourceSelected?.ruleLinkedDc]);

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
      copy.plannedCompletion = new Date(`${year}/${3}/${1}`);
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
      const res: any = await GetServicePlanCreateResource({
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
  // const ReturnLookupContainer = (value: number) => {
  //   switch (value) {
  //     case 1:
  //       return (
  //         <ActivityStatusContainer
  //           returnObject={ActivityStatusRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></ActivityStatusContainer>
  //       );

  //     case 3:
  //       return (
  //         <PlanningActivityStatusContainer
  //           returnObject={PlanningActivityStatusRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlanningActivityStatusContainer>
  //       );
  //     case 4:
  //       return (
  //         <ResponsibilityPhaseContainer
  //           returnObject={ResponsibilityPhaseRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></ResponsibilityPhaseContainer>
  //       );
  //     case 6:
  //       return (
  //         <PlannedActivityResourceContainer
  //           isDesignAspect={props.isDesignAspect}
  //           isForAddAsset={props.isForAddAsset}
  //           isForEditAsset={props.isForEditAsset}
  //           returnObject={PlannedActivityResource}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlannedActivityResourceContainer>
  //       );
  //     case 7:
  //       return (
  //         <BudgetAvailabilityContainer
  //           returnObject={BudgetAvailabilityRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></BudgetAvailabilityContainer>
  //       );
  //     case 8:
  //       return (
  //         <OperationalRiskContainer
  //           returnObject={OperationalRiskRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></OperationalRiskContainer>
  //       );
  //     case 9:
  //       return (
  //         <Driver
  //           returnObject={DriverRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Driver>
  //       );
  //     case 10:
  //       return (
  //         <Benefits
  //           returnObject={BenefitsRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Benefits>
  //       );
  //     case 11:
  //       return (
  //         <PlanningRisk
  //           returnObject={PlanningRiskRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></PlanningRisk>
  //       );
  //     case 12:
  //       return (
  //         <Program
  //           returnObject={ProgramRefillData}
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //         ></Program>
  //       );
  //     case 13:
  //       return (
  //         <AssetsDetailsContainer
  //           modal={{ isModal: true, setIsVisibleModalLookup }}
  //           opcoId={props.opcoId}
  //           dcfId={props.designComponentFamilyId}
  //           plannedDcfId={formData?.plannedDesignComponentFamilyId ?? 0}
  //           paId={formData?.plannedActivityId ?? 0}
  //           initialAssets={assetsData}
  //           onAssetsChange={handleAssetsChange}
  //           isAssetDetailContainer={props?.isAssetDetailContainer}
  //         />
  //       );

  //     default:
  //       return;
  //   }
  // };
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
          {/* {ReturnLookupContainer(isVisibleModalLookup)} */}
        </DialogContent>
      </Dialog>
      {isServiceCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsServiceCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogTitle className="d-flex justify-content-center"></DialogTitle>
          <IconButton
            aria-label="close"
            onClick={() => setIsServiceCrudModalOpen(false)}
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
            <ServiceMaster
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () => setIsServiceCrudModalOpen(false),
              }}
              returnObject={(value) => {
                console.log("Service data:", value);
                setIsServiceCrudModalOpen(false);
              }}
            />
          </DialogContent>
        </Dialog>
      )}
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

      <div className="col-12 mx-0 px-0 pl-0">
        {props.showForm ? (
          <form onChange={() => setChanged(true)}>
            <div className="pb-4 w-100">
              <div className="row col-12 mt-2 mb-2 pl-0">
                <div className="row col-12 px-0 mx-0 mt-3">
                  <div className="col-6">
                    <label className="labelForm voda-bold w-100">
                      OpCo <span className="red">*</span>
                      <Select
                        menuPosition={"fixed"}
                        options={
                          props.opcosResource &&
                          dictionaryToArray(props.opcosResource)
                        }
                        value={
                          props.opcosResource &&
                          dictionaryToArray(props.opcosResource).filter(
                            (x) => x.key === props.opcoIdValue
                          )
                        }
                        onChange={(e) =>
                          props.onChangeOpco && props.onChangeOpco(e)
                        }
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={props.edit}
                      />
                    </label>
                  </div>
                  <div className="col-6">
                    <label className="labelForm voda-bold w-100">
                      Design Component Family <span className="red">*</span>
                      <Select
                        menuPosition={"fixed"}
                        options={
                          props.dcfResource &&
                          dictionaryToArray(props.dcfResource)
                        }
                        value={
                          props.dcfResource
                            ? dictionaryToArray(props.dcfResource).filter(
                                (x) => x.key === props.dcfIdValue
                              )
                            : null
                        }
                        onChange={(e) =>
                          props.onChangeDCF && props.onChangeDCF(e)
                        }
                        isSearchable
                        isClearable
                        isDisabled={props.edit}
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
                      />
                    </label>
                  </div>
                  <div className="col-6">
                    <label className="labelForm voda-bold w-100">
                      Service
                      <div className="d-flex align-items-center">
                        <div className="flex-grow-1">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              (formData?.serviceMasterResource &&
                                dictionaryToArray(
                                  formData?.serviceMasterResource
                                )) ??
                              []
                            }
                            value={
                              formData?.serviceMasterResource &&
                              formData?.serviceMasterId
                                ? dictionaryToArray(
                                    formData.serviceMasterResource
                                  ).find(
                                    (opt) =>
                                      opt.key === formData.serviceMasterId
                                  )
                                : null
                            }
                            onChange={(e) => {
                              let copy = { ...formData };
                              copy.serviceMasterId = e?.key ?? 0;
                              setFormData(copy);
                              props.action.setChanged(true);
                            }}
                            isSearchable
                            isClearable
                            placeholder="Select Service..."
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) => option.key.toString()}
                          />
                        </div>
                        <div className="pb-1 pl-1">
                          <button
                            className="btn btn-link"
                            onClick={() => setIsServiceCrudModalOpen(true)}
                            type="button"
                          >
                            <img
                              style={{ height: 15 }}
                              src={require("../../img/plus_icon.png")}
                              alt="+"
                            />
                          </button>
                        </div>
                      </div>
                    </label>
                  </div>
                </div>
              </div>
              <div className="w-100">
                <div className="row col-md-12 mx-0 d-flex">
                  <div className="form-group col-md-6 pr-0">
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
                                    // props.lcmDeploymentStatusString
                                    //   ?.toLowerCase()
                                    //   .trim() == "planned" ||
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
                                    if (
                                      (plannedResourceSelected?.ruleLinkedDc ===
                                        18 &&
                                        e?.["value"]?.ruleLinkedDc === 19) ||
                                      (plannedResourceSelected?.ruleLinkedDc ===
                                        19 &&
                                        e?.["value"]?.ruleLinkedDc === 18)
                                    ) {
                                      setFormData((prev) => ({
                                        ...prev,
                                        plannedDesignComponentFamilyId:
                                          undefined,
                                      }));
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
                          </>
                        )}
                      </div>
                    </div>
                  </fieldset>
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
                                        *Planned Completion must have a value
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
                                      onChange={(newDate, e) => {
                                        e.preventDefault();
                                        onChangeDate(
                                          "preBaseLineDate",
                                          newDate
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
                            {plannedResourceSelected?.ruleLinkedDc !== 17 && (
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
                            )}
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
                                  formData?.plannedActivityId !== undefined ? (
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
                          {plannedResourceSelected?.ruleLinkedDc !== 17 && (
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
                          )}
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

                <div className="col-12 justify-content-end d-flex footerModal">
                  <button
                    className="voda-bold btn btn-danger px-4 btnHeader"
                    onClick={() => addOnList()}
                    type="button"
                  >
                    Save
                  </button>
                </div>
              </div>
            </div>
          </form>
        ) : null}
      </div>
    </div>
  );
};

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

interface ServiceLevelPAModalProps {
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

const ServiceLevelPlannedModal: React.FC<ServiceLevelPAModalProps> = (
  props
) => {
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
  } = useFormTableCrud<any>(CreateServicePlan, EditDesignAspect);

  const dtoEditResourceState = (state: RootState) =>
    state.designAspectEditReducer.DesignAspectDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.servicePlanCreateReducer.ServicePlanDtoCreate;
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
  //   const GridAssetSelector = (state: RootState) =>
  //     state.daassetsMigrationGridReducer.DAAssetsMigrationGridResult;

  //   const GridAssetDto: QueryResultDtoOfDAAssetMigrationDtoGrid | null =
  //     useSelector(GridAssetSelector);

  const { tipologicaPermesso, isPermesso, pageSize } = useAuth();

  const [networkElementToAdd, setNetworkElementToAdd] =
    useState<NetworkElementAsPlannedDtoCreate | null>();

  const [opco, setOpco] = useState<string>();

  const [supportedServiceDCF, setSupportedServiceDCF] = useState<Array<any>>(
    []
  );

  const [networkFunctionDCF, setNetworkFunctionDCF] = useState<Array<any>>([]);
  //   const [isAssetDetailContainer, setIsAssetDetailsCpntainer] =
  //     useState<boolean>(false);
  //   const [assetData, setAssetData] = useState<any[] | undefined>([]);
  //   const [renderGridState, setRenderGridState] = useState<
  //     CustomGridRender | undefined
  //   >();
  //   const [isFiltriAttivati, setIsFiltriAttivati] = useState(false);

  //   useEffect(() => {
  //     paginationQueryAssets.pageSize = pageSize;
  //   }, [pageSize]);
  //UPDATE ON CHANGE DTO

  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);
  console.log("FormData", formData);
  //   useEffect(() => {
  //     if (props.fromDesignAspect) {
  //       setFormData(createResource);
  //       return;
  //     }
  //     if (props.edit) {
  //       setFormData(editResource);
  //     } else {
  //       setFormData(createResource);
  //     }
  //   }, [createResource, editResource, props.edit]);

  //   useEffect(() => {
  //     let copy = { ...formData } as DesignAspectDtoUpdate;

  //     if (formData && formData.designComponentFamilyId && !props.edit) {
  //       GetServicesAndNetworks(formData.designComponentFamilyId).then(
  //         (response) => {
  //           copy.supportedServicesResource = response.services;
  //           copy.usedNetworkFunctionsResource = response.networkFunctions;
  //           copy.isSupportedAllServices = response.isSupportedAllServices;
  //           copy.platformSoftware = response.platformSoftware;
  //           copy.designComponentFamilyName = dictionaryToArray(
  //             formData.dcFsResource!
  //           ).find((x) => x.key == formData.designComponentFamilyId)?.value!;
  //           copy.subNetworkBoundary = response.subNetworkBoundary;
  //           copy.supportedServicesIds = dictionaryToArray(response?.services).map(
  //             (x) => x.key
  //           );
  //           copy.usedNetworkFunctionsIds = dictionaryToArray(
  //             response?.networkFunctions
  //           ).map((x) => x.key);

  //           console.log("copy in useEffect 1 => ", copy);

  //           setFormData(copy);
  //         }
  //       );
  //     }
  //   }, [formData?.designComponentFamilyId]);

  //   useEffect(() => {
  //     console.log("props => ", props);
  //     if (formData && formData.opCoId && !props.dcfId) {
  //       let copy = { ...formData } as DesignAspectDtoUpdate;
  //       GetDesignComponentFamily(formData.opCoId).then((res) => {
  //         copy.dcFsResource = res;
  //         setFormData(copy);
  //       });
  //     }
  //   }, [formData?.opCoId]);

  //   const plannedActivity402 = formData?.plannedActivityDto?.find(
  //     (a) => a.plannedActivityResourceId === 402
  //   );

  //   const isQueryReady =
  //     !!formData?.opCoId &&
  //     !!formData?.designComponentFamilyId &&
  //     !!plannedActivity402?.plannedActivityId;

  //   const {
  //     query: query1,
  //     next: next1,
  //     back: back1,
  //     setQuery: setQuery1,
  //   } = useResourceTableCrud(
  //     paginationQueryAssets,
  //     isQueryReady && isPermesso ? GetAssetsPlatformMigrationGrid : undefined
  //   );

  //   useEffect(() => {
  //     setAssetData([]);
  //     setRenderGridState(undefined);
  //   }, [
  //     isQueryReady,
  //     formData?.opCoId,
  //     formData?.designComponentFamilyId,
  //     formData?.plannedActivityDto,
  //   ]);

  //   const onChangeOpco = (e: any) => {
  //     let copy = { ...formData } as DesignAspectDtoUpdate;
  //     if (e && e["key"]) {
  //       copy.opCoId = e["key"];
  //       if (!props.dcfId) {
  //         copy.subNetworkBoundary = "";
  //         copy.supportedServicesIds = undefined;
  //         copy.usedNetworkFunctionsIds = undefined;
  //         copy.designComponentFamilyId = 0;
  //       }
  //     } else {
  //       copy.opCoId = 0;
  //       if (!props.dcfId) {
  //         copy.dcFsResource = undefined;
  //         copy.subNetworkBoundary = "";
  //         copy.supportedServicesIds = undefined;
  //         copy.usedNetworkFunctionsIds = undefined;
  //         copy.designComponentFamilyId = 0;
  //       }
  //     }
  //     setFormData(copy);
  //   };

  const onChangeOpco = (e: any) => {
    let copy = { ...formData };
    copy.opCoId = e?.["key"] ?? 0;
    setFormData(copy);
  };

  //   const onChangeDCF = (e: any) => {
  //     let copy = { ...formData } as DesignAspectDtoUpdate;
  //     if (e && e["key"]) {
  //       copy.designComponentFamilyId = e["key"];
  //     } else {
  //       copy.subNetworkBoundary = "";
  //       copy.supportedServicesIds = undefined;
  //       copy.usedNetworkFunctionsIds = undefined;
  //       copy.designComponentFamilyId = 0;
  //     }

  //     setFormData(copy);
  //   };

  const onChangeDCF = (e: any) => {
    let copy = { ...formData };
    copy.designComponentFamilyId = e?.["key"] ?? 0;
    setFormData(copy);
  };

  //   useEffect(() => {
  //     if (props.keyTab === "" || props.keyTab == null || !props.edit) {
  //       setKey("operational");
  //     } else {
  //       setKey(props.keyTab);
  //     }
  //   }, []);

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

  //   const SaveOrConfirmPlanned = async () => {
  //     const plannedActivityId =
  //       formData?.plannedActivityDto?.[0]?.plannedActivityId;
  //     const updatedFormData = {
  //       ...formData,
  //       opcosResource: {},
  //       authenicationTypesResource: {},
  //       instanseResiliencesResource: {},
  //       licenseModelsResource: {},
  //       securityManagersResource: {},
  //       securityTireZoneResource: {},
  //       siteResilienceMethodsResource: {},
  //       siteResiliencesResource: {},
  //       swDeliveryLifeCyclesResource: {},
  //       thirdPartyAccessResource: {},
  //     } as DesignAspectDtoUpdate;

  //     const handleAfterSave = async (saveResult: any) => {
  //       if (saveResult?.ResultDtoEdit && !saveResult?.ResultDtoEdit.warning) {
  //         try {
  //           await GetUpdateDAMigration(
  //             plannedActivityId ?? 0,
  //             formData?.designComponentFamilyId ?? 0,
  //             props?.dcfId ?? 0
  //           );
  //         } catch (err) {
  //           console.error("Failed to call GetUpdateDAMigration:", err);
  //         }
  //       }
  //     };

  //     const saveResult = await Save(
  //       updatedFormData,
  //       props.edit,
  //       () => ({ response: true, property: [] }),
  //       refresh,
  //       RestoreOrphanDeleted,
  //       orphanDeleted
  //     );
  //     await handleAfterSave(saveResult);
  //     // setIsAssetDetailsCpntainer(true);
  //   };

  const SaveOrConfirmPlanned = async () => {
    const validation = validazioneClient(formData);
    if (!validation.response) {
      rootStore.dispatch(
        setNotification({
          message: "Please fill in all required fields",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

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

    try {
      // Call CreateServicePlan directly as async function
      const saveResult = await CreateServicePlan(updatedFormData);

      if (saveResult?.ResultDtoCreate && !saveResult.ResultDtoCreate.warning) {
        try {
          await GetUpdateDAMigration(
            plannedActivityId ?? 0,
            formData?.designComponentFamilyId ?? 0,
            props?.dcfId ?? 0
          );
        } catch (err) {
          console.error("Failed to call GetUpdateDAMigration:", err);
        }

        // Close modal and refresh
        refresh(true);
      }
    } catch (error) {
      console.error("Error saving service plan:", error);
      rootStore.dispatch(
        setNotification({
          message: "Error saving service plan",
          notifyType: NotifyType.error,
        })
      );
    }
  };

  //   const SaveOrConfirmPlanned = async () => {
  //     const plannedActivityId =
  //       formData?.plannedActivityDto?.[0]?.plannedActivityId;
  //     const updatedFormData = {
  //       ...formData,
  //       opcosResource: {},
  //       authenicationTypesResource: {},
  //       instanseResiliencesResource: {},
  //       licenseModelsResource: {},
  //       securityManagersResource: {},
  //       securityTireZoneResource: {},
  //       siteResilienceMethodsResource: {},
  //       siteResiliencesResource: {},
  //       swDeliveryLifeCyclesResource: {},
  //       thirdPartyAccessResource: {},
  //     } as DesignAspectDtoUpdate;
  //     const handleAfterSave = async (saveResult: any) => {
  //       if (saveResult?.ResultDtoEdit && !saveResult?.ResultDtoEdit.warning) {
  //         try {
  //           const migrationResponse = await GetUpdateDAMigration(
  //             plannedActivityId ?? 0,
  //             formData?.designComponentFamilyId ?? 0,
  //             props?.dcfId ?? 0
  //           );
  //         } catch (err) {
  //           console.error("Failed to call GetUpdateDAMigration:", err);
  //         }
  //       }
  //     };

  //     const confirmPlannedState = {
  //       title: "Continue without saving planned activities?",
  //       button: "Continue",
  //       message:
  //         "Are you sure you want to continue? All changes in Planned activities will be lost",
  //       item: "",
  //       isOpen: true,
  //       actions: {
  //         cancel: () => setConfirmPlanned(stateConfirm),
  //         confirm: async () => {
  //           const saveResult = await Save(
  //             updatedFormData,
  //             props.edit,
  //             validazioneClient,
  //             refresh,
  //             RestoreOrphanDeleted,
  //             orphanDeleted
  //           );

  //           await handleAfterSave(saveResult);
  //           setIsAssetDetailsCpntainer(true);
  //         },
  //       },
  //     };

  //     if (showForm) {
  //       let copy = { ...updatedFormData } as DesignAspectDtoUpdate;
  //       if (validazioneClient(copy).response === true) {
  //         setConfirmPlanned(confirmPlannedState);
  //       } else {
  //         rootStore.dispatch(
  //           setNotification({
  //             message: "Check the fields entered in Operational",
  //             notifyType: NotifyType.warning,
  //           })
  //         );
  //       }
  //     } else {
  //       let copy = { ...updatedFormData } as DesignAspectDtoUpdate;
  //       const saveResult = await Save(
  //         copy,
  //         props.edit,
  //         validazioneClient,
  //         refresh,
  //         RestoreOrphanDeleted,
  //         orphanDeleted
  //       );

  //       await handleAfterSave(saveResult);
  //       setIsAssetDetailsCpntainer(true);
  //     }
  //   };
  //   useEffect(() => {
  //     if (!isQueryReady) return;

  //     const plannedActivity402 = formData!.plannedActivityDto!.find(
  //       (a) => a.plannedActivityResourceId === 402
  //     );

  //     if (!plannedActivity402?.plannedActivityId) return;

  //     let copy = {} as DaAssetMigrationGrid;

  //     copy.opcoId = [formData!.opCoId];
  //     copy.currentDcfId = [formData!.designComponentFamilyId];
  //     copy.plannedActivityId = [plannedActivity402.plannedActivityId];

  //     setQuery1(copy);
  //   }, [
  //     isQueryReady,
  //     formData?.opCoId,
  //     formData?.designComponentFamilyId,
  //     formData?.plannedActivityDto,
  //   ]);

  //   useEffect(() => {
  //     if (GridAssetDto !== undefined || GridAssetDto !== null) {
  //       setAssetData(GridAssetDto?.items);
  //       let copy = { ...GridAssetDto?.gridRender } as
  //         | CustomGridRender
  //         | undefined;
  //       setRenderGridState(copy);
  //       setLoader("REMOVE", "GetReconciliationGrid");
  //     }
  //   }, [GridAssetDto]);
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
    undefined
  );
  const [dataPlanning, setDataPlanning] = useState<
    PlannedActivityDtoGrid[] | undefined
  >([]);

  const { New, Edit, Delete } = useOperationTableCrud<
    PlannedActivityDtoUpdate,
    PlannedActivityDtoCreate
  >(
    GetServicePlanCreateResource,
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
    props.action.Edit(id);
    setShowForm(false);
  };

  //PROPS PLANNED ACTIVITY

  useEffect(() => {
    if (formData?.opCoId != undefined && formData?.opCoResource != undefined) {
      let opcoString = dictionaryToArray(formData.opCoResource).find(
        (x) => x.key == formData.opCoId
      )?.value;
      setOpco(opcoString);
    } else {
      setOpco("");
    }
  }, [formData?.opCoId]);

  //   useEffect(() => {
  //     if (formData?.opCoId != undefined && formData.opcosResource != undefined) {
  //       let opcoString = dictionaryToArray(formData.opcosResource).find(
  //         (x) => x.key == formData.opCoId
  //       )?.value;
  //       setOpco(opcoString);
  //     } else {
  //       setOpco("");
  //     }
  //   }, [formData?.opCoId]);

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
          {/* {ReturnLookupContainer(isVisibleModalLookup)} */}
        </DialogContent>
      </Dialog>
      <ServiceLevelPlannedActivity
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
        showForm={true}
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
        edit={false}
        subdomainSpoc={undefined}
        isFromNetworkElement={false}
        isDesignAspect={true}
        plannedActivityTypeForEnum={PlannedActivityTypeForEnum["DesignAspect"]}
        buildBagIds={0}
        dcfResource={formData?.dcFsResource}
        onRequestParentSave={SaveOrConfirmPlanned}
        // isAssetDetailContainer={isAssetDetailContainer}
        onChangeOpco={onChangeOpco}
        onChangeDCF={onChangeDCF}
        opcosResource={formData?.opCoResource}
        dcFsResource={
          Array.isArray(formData?.designComponentFamilyResource)
            ? formData?.designComponentFamilyResource?.reduce(
                (acc, item) => ({ ...acc, [item.key]: item.value }),
                {}
              )
            : formData?.designComponentFamilyResource
        }
        opcoIdValue={formData?.opCoId}
        dcfIdValue={formData?.designComponentFamilyId}
      />
    </div>
  );
};

export default ServiceLevelPlannedModal;
export { ServiceLevelPlannedActivity };
