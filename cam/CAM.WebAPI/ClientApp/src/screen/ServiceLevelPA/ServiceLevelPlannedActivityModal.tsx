import React, {
  useState,
  useEffect,
  useLayoutEffect,
  useRef,
  SetStateAction,
} from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import ModalConfirm from "../../Components/ModalConfirm";
import Select from "react-select";
import {
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  QueryObjectGrid,
  stateConfirm,
} from "../../Model/Common";
import {
  Dialog,
  DialogContent,
  Box,
  IconButton,
  DialogTitle,
} from "@mui/material";
import { useTheme } from "../../Context/ThemeContext";

import { IoClose } from "react-icons/io5";
import { ServiceLevelPlanDtoUpdate } from "../../Model/ServicePlan";
import { CreateServicePlan } from "../../Redux/Action/ServicePlan/ServicePlanCreateAction";
import { EditServicePlan } from "../../Redux/Action/ServicePlan/ServicePlanEditAction";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityQueryObjectGrid,
} from "../../Model/PlannedActivity";
import Container from "../../Components/Container";
import { useAuth } from "../../Hook/useAuth";
import { useSelector } from "react-redux";
import DatePicker from "react-datepicker";
import {
  DropdownInputComponent,
  MultiSelectComponent,
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";
import { IoIosRefresh } from "react-icons/io";
import {
  dictionaryToArray,
  dictionaryToArrayPlannedActivityResourceDto,
} from "../../Hook/Dictionary";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { PlannedActivityResourceDto } from "../../Model/LookUp/PlannedActivityResource";
import {
  boolOptions,
  currencyOption,
  formatDateWithTime,
  lowerFirstLetter,
  numberIsNullOrZero,
} from "../../Hook/Common";
import { Form, Tab, Tabs } from "react-bootstrap";
import UpdatePlannedActivityStatusModal from "../PlannedActivities/UpdatePlannedActivityStatusModal";
import UpdateServiceLevelPAStatusModal from "../PlannedActivities/UpdateServiceLevelPAStatusModal";
import ServiceMaster from "../../Containers/Lookup/ServiceMasterContainer";
import {
  ActivityStatusLogics,
  GetPlannedActivityRelatedToDeliveryStatus,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import Program from "../../Containers/Lookup/ProgramContainer";
import PlanningRisk from "../../Containers/Lookup/PlanningRiskContainer";
import Benefits from "../../Containers/Lookup/BenefitsContainer";
import Driver from "../../Containers/Lookup/DriverContainer";
import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import ActivityStatusContainer from "../../Containers/Lookup/ActivityStatusContainer";
import PlanningActivityStatusContainer from "../../Containers/Lookup/PlanningActivityStatusContainer";
import ResponsibilityPhaseContainer from "../../Containers/Lookup/ResponsibilityPhaseContainer";
import PlannedActivityResourceContainer from "../../Containers/Lookup/PlannedActivityResourceContainer";
import BudgetAvailabilityContainer from "../../Containers/Lookup/BudgetAvailabilityContainer";
import OperationalRiskContainer from "../../Containers/Lookup/OperationalRiskContainer";
import AssetsDetailsContainer from "../../Containers/Lookup/AssetsDetailsContainer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { NetworkElementAssociated } from "../../Model/DesignAspects";
import { DeploymentStatusDto } from "../../Model/LookUp/DeploymentStatus";
import { TH } from "country-flag-icons/react/3x2";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { GetFilterColumPlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { MdEdit } from "react-icons/md";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const ServiceLevelPlannedActivityModal = (props: Props) => {
  const [keyTabs, setKey] = useState("operational");
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [edit, setEdit] = useState<boolean>(false);
  const [plannedActivityData, setPlannedActivityData] =
    useState<PlannedActivityDtoUpdate | null>(null);
  const [ActivityStatusFromLogic, setActivityStatusFromLogic] = useState<
    number[]
  >([]);
  const [activityDetailsManual, setActivityDetailsManual] =
    useState<boolean>(false);
  const [isConsigliati, setIsConsigliati] = useState<boolean>(false);
  const { darkMode } = useTheme();
  const [groupedPlannedResource, setGroupedPlannedResource] = useState<
    | {
        label: string;
        options: { key: number; value: PlannedActivityResourceDto }[];
      }[]
    | undefined
  >([]);

  const [plannedResourceSelected, setPlannedResourceSelected] =
    useState<PlannedActivityResourceDto>();
  const [driverOption, setDriverOption] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [benefitsOption, setBenefitsOption] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [planningRiskOption, setPlanningRiskOption] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [index, setIndex] = useState<number | undefined>(undefined);

  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);
  const [startEndFlag, setStartEndFlag] = useState<boolean>(false);
  const [customDateFormat, setCustomDateFormat] = useState<any>(null);
  const [isDateRange, setIsDateRange] = useState<boolean>(false);
  const [disabledInitialFunds, setDisabledInitialFunds] = useState<any>(false);
  const [isVisibleModalManage, setIsVisibleModalManage] =
    useState<boolean>(false);
  const [plannedActivityIdToManage, setPlannedActivityIdToManage] =
    useState<number>();
  const [startDate, setStartDate] = useState<Date | undefined>();
  const [disableActivityApproved, setDisableActivityApproved] =
    useState<boolean>(false);
  const [DeliveryStatusArr, setDeliveryStatusArr] = useState<any>();
  const [showfurther, setShowFurther] = useState<boolean>(false);

  const onChangeYears = async (property, date: Date) => {
    var event = { currentTarget: {} } as React.ChangeEvent<HTMLInputElement>;
    if (date && date != null) {
      event.currentTarget.value = date.getFullYear().toString();
      onChangePAText(property, event);
      setStartDate(date);
    } else {
      setStartDate(date);
      event.currentTarget.value = "";
      onChangePAText(property, event);
    }
  };
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const {
    formData,
    setFormData,
    changed,
    validation,
    setValidation,
    setChanged,
    setInputValue,
  } = useFormTableCrud<ServiceLevelPlanDtoUpdate>(
    CreateServicePlan,
    EditServicePlan
  );
  const dtoNewResourceState = (state: RootState) =>
    state.servicePlanCreateReducer.ServicePlanDtoCreate;
  let createResource = useSelector(dtoNewResourceState);

  const dtoEditResourceState = (state: RootState) =>
    state.servicePlanEditReducer.ServicePlanDtoEdit;
  let editResource = useSelector(dtoEditResourceState);

  useEffect(() => {
    if (props.edit) {
      let editCopy = {
        ...editResource,
      } as PlannedActivityDtoUpdate;
      let copy = {
        ...createResource?.plannedActivityCreateDto,
      } as PlannedActivityDtoUpdate;
      copy.plannedActivityResourceId = 0;
      copy.responsibilityPhaseId =
        copy?.responsibilityPhaseResource &&
        dictionaryToArray(copy.responsibilityPhaseResource).find(
          (x) => x.value.toLowerCase() == "engineering"
        )?.key;
      setFormData({
        ...editCopy,
        opCoId: editCopy?.servicePlanDetails?.opCoId ?? 0,
        serviceMasterId: editCopy?.servicePlanDetails?.serviceMasterId ?? 0,
      });
      if (editResource?.plannedActivityUpdateDto === null) {
        setPlannedActivityData(copy);
      } else {
        setPlannedActivityData(editResource?.plannedActivityUpdateDto ?? null);
      }
    } else {
      let copy = {
        ...createResource?.plannedActivityCreateDto,
      } as PlannedActivityDtoUpdate;
      copy.plannedActivityResourceId = 0;
      copy.responsibilityPhaseId =
        copy?.responsibilityPhaseResource &&
        dictionaryToArray(copy.responsibilityPhaseResource).find(
          (x) => x.value.toLowerCase() == "engineering"
        )?.key;
      setFormData(createResource);
      setPlannedActivityData(copy ?? null);
    }
  }, [createResource, editResource, props.edit]);
  const [validationForm, setValidationForm] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const formValidationClient = (copy: ServiceLevelPlanDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };
    if (
      copy?.opCoId == null ||
      copy?.opCoId == undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }

    if (
      copy?.serviceMasterId === 0 ||
      copy?.serviceMasterId == null ||
      copy?.serviceMasterId === undefined
    ) {
      addInvalidProperty("serviceMasterId");
    }
    if (
      copy?.dcfIdList?.length === 0 ||
      copy?.dcfIdList == null ||
      copy?.dcfIdList === undefined
    ) {
      addInvalidProperty("dcfIdList");
    }
    setValidationForm(copyValidation);
    return copyValidation;
  };
  const validazioneClient = (copy: PlannedActivityDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.plannedActivityResourceId == null ||
      copy?.plannedActivityResourceId == undefined ||
      copy?.plannedActivityResourceId === 0
    ) {
      addInvalidProperty("plannedActivityResourceId");
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
      copy?.deliveryProjectName === null ||
      copy?.deliveryProjectName === undefined ||
      copy?.deliveryProjectName === ""
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
    if (!copy?.riskEngId) {
      addInvalidProperty("riskEngId");
    }
    if (!copy?.riskOpeId) {
      addInvalidProperty("riskOpeId");
    }
    if (
      new Date(copy.startDate!).getTime() >
      new Date(copy.plannedCompletion!).getTime()
    ) {
      addInvalidProperty("plannedStartDate");
    }
    setValidation(copyValidation);
    return copyValidation;
  };
  useEffect(() => {
    if (ActivityStatusFromLogic?.length == 1) {
      // debugger
      let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
      copy.activityStatusId = ActivityStatusFromLogic?.[0];
      setPlannedActivityData(copy);
    }
  }, [ActivityStatusFromLogic]);

  useEffect(() => {
    if (
      plannedActivityData?.budgetAvailabilityId !== null ||
      (plannedActivityData?.responsibilityPhaseId !== 1 &&
        plannedActivityData?.responsibilityPhaseId !== null) ||
      plannedActivityData?.deliveryStatusId !== null ||
      plannedActivityData?.localApproval !== null
    ) {
      launchActivityStatusLogic();
    }
  }, [
    plannedActivityData?.budgetAvailabilityId,
    plannedActivityData?.responsibilityPhaseId,
    plannedActivityData?.deliveryStatusId,
    plannedActivityData?.localApproval,
  ]);

  const launchActivityStatusLogic = async () => {
    await ActivityStatusLogics(
      plannedActivityData?.deliveryStatusId,
      plannedActivityData?.budgetAvailabilityId,
      plannedActivityData?.responsibilityPhaseId,
      plannedActivityData?.localApproval
    ).then((x) => {
      setActivityStatusFromLogic(x);
    });
  };

  const refresh = (changed?: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const stripHtml = (html) => {
    const temp = document.createElement("div");
    temp.innerHTML = html;
    return temp.textContent || temp.innerText || "";
  };

  //   useEffect(() => {
  //     if (plannedActivityData) {
  //       setFormData({
  //         ...formData,
  //         plannedActivityCreateDto: plannedActivityData,
  //       });
  //     }
  //   }, [plannedActivityData]);

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as ServiceLevelPlanDtoUpdate;
    if (validationForm?.property?.includes(fieldSet)) {
      let copy = { ...validationForm, property: [...validationForm.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidationForm(copy);
    }
    copy[fieldSet] = e?.["key"] ?? null;
    setFormData({
      ...copy,
      plannedActivityCreateDto: plannedActivityData ?? undefined,
    });
  };

  const onChangePADropdown = (fieldSet: string, e: any) => {
    const copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    if (validation?.property?.includes(fieldSet)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
    copy[fieldSet] = e?.["key"] ?? null;
    setPlannedActivityData(copy);
  };

  const handleDcfMultiSelect = (
    selected: Array<{ label: string; value: number }>
  ) => {
    const selectedIds = selected.map((item) => item.value);
    let copy = { ...formData } as ServiceLevelPlanDtoUpdate;
    copy.dcfIdList = selectedIds;
    setFormData(copy);
    if (validationForm?.property?.includes("dcfIdList")) {
      let copy = { ...validationForm, property: [...validationForm.property] };
      let idx = copy.property.indexOf("dcfIdList");
      copy.property.splice(idx, 1);
      setValidationForm(copy);
    }
  };

  const onChangePAText = (fieldSet: string, e: any) => {
    const copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    if (validation?.property?.includes(fieldSet)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }

    if (e.currentTarget.type === "checkbox") {
      copy[fieldSet] = e.currentTarget.checked;
      setPlannedActivityData(copy);
    } else {
      copy[fieldSet] = e?.currentTarget?.value;
      setPlannedActivityData(copy);
    }
  };
  const onChangeText = (fieldSet: string, e: any) => {
    const copy = { ...formData } as ServiceLevelPlanDtoUpdate;
    if (validation?.property?.includes(fieldSet)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidationForm(copy);
    }
    if (e && e?.target?.value) {
      copy[fieldSet] = e?.target?.value;
      setFormData(copy);
    }
  };
  const onChangePaDate = (property: string, newDate: Date | null) => {
    let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    if (newDate === null) {
      copy[lowerFirstLetter(property)] = null;
    } else {
      const DateString = `${newDate.getFullYear()}/${
        newDate.getMonth() + 1
      }/${newDate.getDate()}`;
      copy[lowerFirstLetter(property)] = DateString;
    }

    setPlannedActivityData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };
  useEffect(() => {
    if (plannedResourceSelected) {
      let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
      let optionsForBenefits = {} as
        | { key: number; value: string }[]
        | undefined;
      if (
        plannedResourceSelected?.benefitTextServicePlan &&
        plannedResourceSelected?.benefitTextServicePlan.length > 0
      ) {
        optionsForBenefits =
          plannedActivityData?.benefitResource &&
          dictionaryToArray(plannedActivityData?.benefitResource).filter(
            (el: any) =>
              plannedResourceSelected?.benefitTextServicePlan?.includes(el.key)
          );
        if (optionsForBenefits?.length === 1) {
          copy.benefitId = optionsForBenefits[0].key;
        }
      } else {
        optionsForBenefits = [];
      }
      setBenefitsOption(optionsForBenefits ?? []);

      let optionsForPlanningRisk = {} as
        | { key: number; value: string }[]
        | undefined;

      if (
        plannedResourceSelected?.planningRiskServicePlan &&
        plannedResourceSelected?.planningRiskServicePlan.length > 0
      ) {
        optionsForPlanningRisk =
          plannedActivityData?.planningRiskResource &&
          dictionaryToArray(plannedActivityData?.planningRiskResource).filter(
            (el) =>
              plannedResourceSelected?.planningRiskServicePlan?.includes(el.key)
          );
        if (optionsForPlanningRisk?.length === 1) {
          copy.planningRiskId = optionsForPlanningRisk[0].key;
        }
      } else {
        optionsForPlanningRisk = [];
      }
      setPlanningRiskOption(optionsForPlanningRisk ?? []);

      let optionsForDriver = {} as { key: number; value: string }[] | undefined;
      if (
        plannedResourceSelected?.driverTextServicePlan &&
        plannedResourceSelected?.driverTextServicePlan.length > 0
      ) {
        optionsForDriver =
          plannedActivityData?.driverResource &&
          dictionaryToArray(plannedActivityData?.driverResource).filter((el) =>
            plannedResourceSelected?.driverTextServicePlan?.includes(el.key)
          );
        if (optionsForDriver?.length === 1) {
          copy.driverId = optionsForDriver[0].key;
        }
      } else {
        optionsForDriver = [];
      }
      setDriverOption(optionsForDriver ?? []);
      //   setPlannedActivityData(copy);
    }
  }, [plannedResourceSelected]);

  useEffect(() => {
    if (
      plannedActivityData &&
      plannedActivityData?.plannedActivityResourceId &&
      Object.keys(plannedActivityData).length !== 0
    ) {
      if (
        plannedActivityData !== undefined &&
        plannedActivityData?.plannedActivityResource &&
        plannedActivityData?.plannedActivityResourceId !== undefined &&
        !numberIsNullOrZero(plannedActivityData?.plannedActivityResourceId) &&
        dictionaryToArrayPlannedActivityResourceDto(
          plannedActivityData.plannedActivityResource
        ).find((y) => y.key == plannedActivityData.plannedActivityResourceId)
          ?.value.exportable == true
      ) {
        setIsConsigliati(true);
      } else {
        setIsConsigliati(false);
      }
      if (
        plannedActivityData !== undefined &&
        plannedActivityData?.plannedActivityResource &&
        plannedActivityData?.plannedActivityResourceId !== null
      ) {
        let selected = dictionaryToArrayPlannedActivityResourceDto(
          plannedActivityData?.plannedActivityResource
        ).find((x) => x.key == plannedActivityData?.plannedActivityResourceId);

        setPlannedResourceSelected(selected?.value);
      }
      //MAPPING DELLE PLANNED RESOURCE
      // if (plannedActivityData && plannedActivityData.plannedActivityResource != undefined) {
      //   mapPlannedResource();
      // }
    }
  }, [plannedActivityData?.plannedActivityResourceId]);

  useEffect(() => {
    if (
      plannedActivityData?.plannedActivityResourceId !== undefined &&
      plannedActivityData?.deliveryStatusResource
    ) {
      onGetDeliveryStatus();
    }
  }, [plannedActivityData?.plannedActivityResourceId]);

  const onGetDeliveryStatus = async (e?: any) => {
    let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;

    if ((copy && copy.plannedActivityResourceId !== undefined) || e) {
      copy.plannedActivityResourceId = e ? e : copy.plannedActivityResourceId;
      if (copy.plannedActivityResourceId) {
        const deliveryStatusResponse: any =
          await GetPlannedActivityRelatedToDeliveryStatus(
            e ? e : copy.plannedActivityResourceId!,
            4
          );

        if (deliveryStatusResponse?.length > 0) {
          copy.deliveryStatusId = deliveryStatusResponse[0]?.key;
          setPlannedActivityData((prev) => ({
            ...prev,
            deliveryStatusId: deliveryStatusResponse[0]?.key ?? null,
          }));
        }
        setDeliveryStatusArr(deliveryStatusResponse ?? []);
      }
    }
  };
  const currentYear = new Date().getFullYear();

  const handleDateChange = (newDate, e) => {
    e.preventDefault();
    setStartDate(newDate);
    updateStartEndDate();
    setStartEndFlag(true);
    onChangeYears("plannedImplementationYear", newDate);
    // props.action.setChanged(true);
  };

  const updateStartEndDate = () => {
    if (plannedActivityData && plannedActivityData.plannedImplementationYear) {
      let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
      const year: any = Number(plannedActivityData.plannedImplementationYear);
      const currentYear = new Date().getFullYear();
      copy.startDate = new Date();
      let endDate = new Date(`${year}/${3}/${1}`);
      copy.plannedCompletion = `${endDate.getFullYear()}/${
        endDate.getMonth() + 1
      }/${endDate.getDate()}` as any;
      if (currentYear !== year - 1) {
        copy.startDate = `${year - 1}/${4}/${1}`;
        setPlannedActivityData(copy);
      }
      setPlannedActivityData(copy);

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
    if (plannedActivityData && plannedActivityData.plannedImplementationYear) {
      const formattedDate = getFiscalYearFormat(
        plannedActivityData.plannedImplementationYear
      );
      setCustomDateFormat(formattedDate);
    }
  }, [plannedActivityData?.plannedImplementationYear]);

  function getFinancialYear() {
    const date = new Date();
    const currentYear = date.getFullYear();
    const currentMonth = date.getMonth();

    const financialYearEnd = currentMonth >= 3 ? currentYear + 1 : currentYear;
    return new Date(String(financialYearEnd));
  }

  const removeValidation = (property: string) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const handlePlannedCategoryChange = (obj, propertyId, propertyName) => {
    let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    if (propertyId !== null) {
      copy[propertyId] = obj && obj["key"];
    }
    if (propertyName === "lcmCategories") {
      copy[propertyName] = obj && obj["key"];
    } else {
      copy[propertyName] = obj && obj["value"];
    }
    setPlannedActivityData(copy);
  };
  const onChangeLocalApproval = (e: any) => {
    removeValidation("localApproval");
    let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    if (e && e["key"] !== undefined && e["key"] !== null && e["key"] !== "") {
      copy.localApproval = e["key"];
    } else {
      copy.localApproval = undefined;
    }
    setPlannedActivityData(copy);
  };

  const openModalStatus = (id: number) => {
    // debugger;
    if (!changed) {
      setPlannedActivityIdToManage(id);
      setIsVisibleModalStatus(true);
    } else {
      setConfirm({
        title: "Confirm",
        message:
          "There are pending changes in the views of lcm and planned activity, they will be lost as you continue, are you sure you want to continue?",
        button: "Continue",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => {
            setPlannedActivityIdToManage(id);
            setIsVisibleModalStatus(true);
            setConfirm(stateConfirm);
          },
        },
      });
    }
  };
  const OpCoRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.opCoResource)
      plannedActivityData.opCoResource = obj as { [key: string]: string };
    setPlannedActivityData(plannedActivityData);
  };
  const ActivityStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.activityStatusResource)
      plannedActivityData.activityStatusResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };
  const DeliveryStatusResourceRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.deliveryStatusResource)
      plannedActivityData.deliveryStatusResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };
  const PlanningActivityStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (
      plannedActivityData &&
      plannedActivityData?.planningActivityStatusResource
    )
      plannedActivityData.planningActivityStatusResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };
  const ResponsibilityPhaseRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.responsibilityPhaseResource)
      plannedActivityData.responsibilityPhaseResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };

  const PlannedActivityResource = async (value: {
    [key: string]: PlannedActivityResourceDto;
  }) => {
    let copy = { ...plannedActivityData } as PlannedActivityDtoUpdate;
    copy.plannedActivityResource = value;
    setPlannedActivityData(copy);
  };

  const BudgetAvailabilityRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.budgetAvaibilityResource)
      plannedActivityData.budgetAvaibilityResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };
  const OperationalRiskRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.riskResource)
      plannedActivityData.riskResource = obj as { [key: string]: string };
    setPlannedActivityData(plannedActivityData);
  };
  const PlanningRiskRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.planningRiskResource)
      plannedActivityData.planningRiskResource = obj as {
        [key: string]: string;
      };
    setPlannedActivityData(plannedActivityData);
  };
  const DriverRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.driverResource)
      plannedActivityData.driverResource = obj as { [key: string]: string };
    setPlannedActivityData(plannedActivityData);
  };
  const ProgramRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.programResource)
      plannedActivityData.programResource = obj as { [key: string]: string };
    setPlannedActivityData(plannedActivityData);
  };
  const BenefitsRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (plannedActivityData && plannedActivityData?.benefitResource)
      plannedActivityData.benefitResource = obj as { [key: string]: string };
    setPlannedActivityData(plannedActivityData);
  };
  const ServiceRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.serviceMasterIndex]: item.description }),
      {}
    );
    if (formData && formData?.serviceMasterResource)
      formData.serviceMasterResource = obj as { [key: string]: string };
    setFormData(formData);
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
      case 13:
        return (
          <ServiceMaster
            returnObject={ServiceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );

      default:
        return;
    }
  };
  return (
    <div className="col-12">
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
          {/* <UpdatePlannedActivityStatusModal
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
                  isDAPA={true}
                  dcfId={props.designComponentFamilyId}
                  opCoId={props.opcoId}
                  plannedDcfId={
                    plannedDcfId !== 0
                      ? plannedDcfId
                      : formData?.plannedDesignComponentFamilyId ?? 0
                  }
                  onOpenLookup={handleModalLookup}
                  addOnList={addOnList}
                  onRequestParentSave={props.onRequestParentSave}
                  daAssetMigrationDtoGrid={
                    formData?.plaftformMigrationDcfResources?.daAssetMigrationDtoGrid
                  }
                /> */}

          <UpdateServiceLevelPAStatusModal
            PAID={Number(formData?.plannedActivityUpdateDto?.plannedActivityId)}
            plannedActivitypeForId={4}
            action={{
              setIsVisibleModalStatus,
              refresh,
            }}
          />
        </DialogContent>
      </Dialog>
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

      <Tabs
        defaultActiveKey={keyTabs}
        id="uncontrolled-tab-example"
        activeKey={keyTabs}
        onSelect={(x) => setKey(x || "")}
      >
        <Tab eventKey="operational" title="Service Level">
          <form onChange={() => setChanged(true)}>
            <div className="row col-12 px-0 mx-0">
              <div className="col-12 p-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mt-3">Implementation Details</label>
                  <div className="row col-12 px-0">
                    <div className="col-6">
                      <div className="form-group">
                        <DropdownInputComponent
                          label={"OpCo"}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          disabled={props?.edit}
                          value={
                            plannedActivityData &&
                            plannedActivityData?.opCoResource != undefined
                              ? dictionaryToArray(
                                  plannedActivityData.opCoResource
                                ).find((x) => x.key === formData?.opCoId)
                              : null
                          }
                          options={
                            plannedActivityData?.opCoResource
                              ? dictionaryToArray(
                                  plannedActivityData?.opCoResource
                                )
                              : null
                          }
                          onChange={(e: any) => {
                            onChangePADropdown("opCoId", e);
                            onChangeDropdown("opCoId", e);
                          }}
                          isError={
                            validationForm &&
                            validationForm.response === false &&
                            validationForm.property?.includes("opCoId")
                              ? true
                              : false
                          }
                          error="OpCo must have a value."
                          position="fixed"
                        />
                      </div>
                    </div>
                    <div className="col-6 pr-0">
                      <div className="form-group">
                        <MultiSelectComponent
                          label="Planned Design Component Family"
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          required={true}
                          value={(
                            plannedActivityData?.designComponentFamilyResource ??
                            []
                          )
                            ?.filter(
                              (item) =>
                                Array.isArray(formData?.dcfIdList) &&
                                formData?.dcfIdList.includes(item.key)
                            )
                            ?.map((item) => ({
                              label: stripHtml(item.value),
                              value: item.key,
                            }))}
                          options={(
                            plannedActivityData?.designComponentFamilyResource ??
                            []
                          ).map((item) => ({
                            label: stripHtml(item.value),
                            value: item.key,
                          }))}
                          onChange={handleDcfMultiSelect}
                          isError={
                            validationForm &&
                            validationForm.response === false &&
                            validationForm.property?.includes("dcfIdList")
                              ? true
                              : false
                          }
                          error="Planned DCF must have a value."
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 px-0">
                        <div className="form-group">
                          <DropdownInputComponent
                            label={"Service"}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            disabled={
                              formData?.plannedActivityId !== 0 ? true : false
                            }
                            value={
                              formData &&
                              formData?.serviceMasterResource != undefined
                                ? dictionaryToArray(
                                    formData.serviceMasterResource
                                  ).find(
                                    (x) => x.key === formData?.serviceMasterId
                                  )
                                : null
                            }
                            options={
                              formData?.serviceMasterResource
                                ? dictionaryToArray(
                                    formData?.serviceMasterResource
                                  )
                                : null
                            }
                            onChange={(e: any) =>
                              onChangeDropdown("serviceMasterId", e)
                            }
                            isError={
                              validationForm &&
                              validationForm.response === false &&
                              validationForm.property?.includes(
                                "serviceMasterId"
                              )
                                ? true
                                : false
                            }
                            error="Service must have a value."
                            isAdd={tipologicaPermesso}
                            onAddClicked={() => setIsVisibleModalLookup(13)}
                          />
                        </div>
                      </div>
                    </div>
                    <div className="col-6 d-flex pr-0">
                      <table className="w-100" style={{ minHeight: "0px" }}>
                        <thead>
                          <tr className="intestazione">
                            <th>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>Selected Design Component Family</label>
                              </div>
                            </th>
                          </tr>
                        </thead>
                        <tbody
                          style={{
                            maxHeight: "12rem",
                            overflowY: "scroll",
                            display: "grid",
                          }}
                        >
                          {(
                            plannedActivityData?.designComponentFamilyResource ??
                            []
                          )
                            .filter(
                              (item) =>
                                Array.isArray(formData?.dcfIdList) &&
                                formData?.dcfIdList.includes(item.key)
                            )
                            .map((item) => (
                              <tr key={item.key} className="dati">
                                <td
                                  className="heightLimitTable"
                                  style={{
                                    maxWidth: "100% !important",
                                    whiteSpace: "unset !important",
                                  }}
                                >
                                  <div
                                    dangerouslySetInnerHTML={{
                                      __html: item.value,
                                    }}
                                  />
                                </td>
                              </tr>
                            ))}
                        </tbody>
                      </table>
                      {props.edit &&
                      plannedActivityData?.plannedActivityId !== undefined ? (
                        <button
                          type="button"
                          title="Update Planned Activity Status"
                          className="btn btn-link"
                          style={{ height: "1rem" }}
                          onClick={() =>
                            plannedActivityData?.plannedActivityId &&
                            openModalStatus(
                              plannedActivityData?.plannedActivityId
                            )
                          }
                        >
                          <IoIosRefresh
                            color={`${darkMode ? "white" : "black"}`}
                          />
                        </button>
                      ) : null}
                    </div>
                  </div>
                </fieldset>
              </div>
            </div>
          </form>
        </Tab>
        <Tab
          eventKey="plannedActivities"
          title="Planned Activities"
          // disabled={!props.edit}
        >
          <div className="col-12 mx-0 px-0">
            <div className="col-12 mx-0 px-0 pl-0">
              <form onChange={() => setChanged(true)}>
                <div className="w-100">
                  <div className="row col-12 mt-2 mb-2 pl-0">
                    <h5 className="titleSectionModal voda-bold col-12 mt-40">
                      {edit ? "Save Planned Activity" : "New Planned Activity"}
                    </h5>
                  </div>
                  <div className="row col-12 px-0">
                    <div className="col-6">
                      <div className="form-group">
                        <DropdownInputComponent
                          label={"OpCo"}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          disabled={true}
                          required={true}
                          value={
                            plannedActivityData &&
                            plannedActivityData?.opCoResource != undefined
                              ? dictionaryToArray(
                                  plannedActivityData.opCoResource
                                ).find((x) => x.key === formData?.opCoId)
                              : null
                          }
                          options={
                            plannedActivityData?.opCoResource
                              ? dictionaryToArray(
                                  plannedActivityData?.opCoResource
                                )
                              : null
                          }
                          onChange={(e: any) => onChangePADropdown("opCoId", e)}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("opCoId")
                              ? true
                              : false
                          }
                          error="OpCo must have a value."
                          position="fixed"
                        />
                      </div>
                    </div>
                    <div className="col-6 pr-0">
                      <div className="form-group">
                        <DropdownInputComponent
                          label={"Service"}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          disabled={true}
                          value={
                            formData &&
                            formData?.serviceMasterResource != undefined
                              ? dictionaryToArray(
                                  formData.serviceMasterResource
                                ).find(
                                  (x) => x.key === formData?.serviceMasterId
                                )
                              : null
                          }
                          options={
                            formData?.serviceMasterResource
                              ? dictionaryToArray(
                                  formData?.serviceMasterResource
                                )
                              : null
                          }
                          onChange={(e: any) =>
                            onChangeDropdown("serviceMasterId", e)
                          }
                          isError={
                            validationForm &&
                            validationForm.response === false &&
                            validationForm.property?.includes("serviceMasterId")
                              ? true
                              : false
                          }
                          error="Service must have a value."
                          isAdd={tipologicaPermesso}
                          onAddClicked={() => setIsVisibleModalLookup(13)}
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="form-group">
                        <TextInputComponent
                          label={`Program Name`}
                          labelCSS="mb-0"
                          inputCSS="labelForm voda-bold mb-2"
                          required={true}
                          disabled={false}
                          value={plannedActivityData?.deliveryProjectName ?? ""}
                          onChange={(e: any) =>
                            onChangePAText("deliveryProjectName", e)
                          }
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("deliveryProjectName")
                              ? true
                              : false
                          }
                          error="Program Name must have a value."
                        />
                      </div>
                    </div>
                    <div className="col-6 d-flex pr-0">
                      <table className="w-100" style={{ minHeight: "0px" }}>
                        <thead>
                          <tr className="intestazione">
                            <th>
                              <div className="h-100 d-flex align-items-center divFilter">
                                <label>Selected Design Component Family</label>
                              </div>
                            </th>
                          </tr>
                        </thead>
                        <tbody
                          style={{
                            maxHeight: "12rem",
                            overflowY: "scroll",
                            display: "grid",
                          }}
                        >
                          {(
                            plannedActivityData?.designComponentFamilyResource ??
                            []
                          )
                            .filter(
                              (item) =>
                                Array.isArray(formData?.dcfIdList) &&
                                formData?.dcfIdList.includes(item.key)
                            )
                            .map((item) => (
                              <tr key={item.key} className="dati">
                                <td
                                  className="heightLimitTable"
                                  style={{
                                    maxWidth: "100% !important",
                                    whiteSpace: "unset !important",
                                  }}
                                >
                                  <div
                                    dangerouslySetInnerHTML={{
                                      __html: item.value,
                                    }}
                                  />
                                </td>
                              </tr>
                            ))}
                        </tbody>
                      </table>
                      {props.edit &&
                      plannedActivityData?.plannedActivityId !== undefined ? (
                        <button
                          type="button"
                          title="Update Planned Activity Status"
                          className="btn btn-link"
                          style={{ height: "1rem" }}
                          onClick={() =>
                            plannedActivityData?.plannedActivityId &&
                            openModalStatus(
                              plannedActivityData?.plannedActivityId
                            )
                          }
                        >
                          <IoIosRefresh
                            color={`${darkMode ? "white" : "black"}`}
                          />
                        </button>
                      ) : null}
                    </div>
                  </div>
                </div>

                <div className="w-100">
                  <fieldset className="fieldset p-0">
                    <legend className="text-bb mt-4 mb-3 pl-0">
                      Planned Activity Details
                    </legend>

                    <div className="row col-12 px-0">
                      <div className="col-6">
                        <div className="form-group">
                          <DropdownInputComponent
                            label={"Select the Planned Activity"}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            value={
                              plannedActivityData?.plannedActivityResource &&
                              plannedActivityData?.plannedActivityResourceId !==
                                0 &&
                              plannedActivityData?.plannedActivityResourceId !==
                                null
                                ? dictionaryToArrayPlannedActivityResourceDto(
                                    plannedActivityData?.plannedActivityResource
                                  )
                                    ?.filter(
                                      (x) =>
                                        x.key ==
                                        plannedActivityData.plannedActivityResourceId
                                    )
                                    ?.map((res) => ({
                                      key: res.key,
                                      value:
                                        res.value
                                          ?.plannedActivityResourceDescription,
                                    }))
                                : null
                            }
                            options={
                              plannedActivityData?.plannedActivityResource
                                ? dictionaryToArrayPlannedActivityResourceDto(
                                    plannedActivityData?.plannedActivityResource
                                  )?.map((res) => ({
                                    key: res?.key,
                                    value:
                                      res?.value
                                        ?.plannedActivityResourceDescription,
                                  }))
                                : []
                            }
                            onChange={(e: any) => {
                              onChangePADropdown(
                                "plannedActivityResourceId",
                                e
                              );
                              onGetDeliveryStatus(e?.["key"]);
                            }}
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes(
                                "plannedActivityResourceId"
                              )
                                ? true
                                : false
                            }
                            error="planned Activity must have a value."
                            position="fixed"
                            disabled={
                              formData?.plannedActivityId !== 0 ? true : false
                            }
                            isAdd={tipologicaPermesso}
                            onAddClicked={() => setIsVisibleModalLookup(6)}
                          />
                        </div>
                      </div>
                      <div className="col-12">
                        <div className="form-group col-6 pl-0">
                          <div className="col-12 pl-0">
                            <ToggleInputComponent
                              label={"Delivery Plan Available"}
                              value={
                                plannedActivityData?.deliveryPlanAvailable ??
                                false
                              }
                              required={false}
                              onChange={(e: any) =>
                                onChangePAText("deliveryPlanAvailable", e)
                              }
                            />
                          </div>
                        </div>
                      </div>
                    </div>
                  </fieldset>

                  <fieldset className="fieldset p-0">
                    <label className="text-bb mt-4 mb-3 pl-0">
                      Implementation Details
                    </label>
                    <div className="row col-12 px-0">
                      <div className="col-6 pl-0">
                        <div className="col-md-12">
                          <label className={`labelForm voda-bold w-100 `}>
                            Planned Implementation Year
                            <span className="red">*</span>
                            <div className="w-100">
                              <DatePicker
                                selected={startDate}
                                onChange={handleDateChange}
                                value={customDateFormat}
                                showYearPicker
                                className={`inputForm w-100`}
                                dateFormat="yyyy"
                                yearItemNumber={8}
                                minDate={new Date(currentYear - 10, 0, 1)}
                                maxDate={new Date(currentYear + 10, 11, 31)}
                                //   disabled={props.formDisabed}
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
                                options={
                                  plannedActivityData?.planningActivityStatusResource &&
                                  dictionaryToArray(
                                    plannedActivityData?.planningActivityStatusResource
                                  )
                                }
                                value={
                                  plannedActivityData &&
                                  plannedActivityData?.planningActivityStatusResource &&
                                  dictionaryToArray(
                                    plannedActivityData?.planningActivityStatusResource
                                  ).filter(
                                    (x) =>
                                      x.key ===
                                      plannedActivityData?.planningActivityStatusId
                                  )
                                }
                                onChange={(e) => {
                                  onChangePADropdown(
                                    "planningActivityStatusId",
                                    e
                                  );
                                  // props.action.setChanged(true);
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
                              <label className={`labelForm voda-bold w-100`}>
                                Planned Start Date
                                <DatePicker
                                  selected={
                                    plannedActivityData?.startDate
                                      ? new Date(plannedActivityData?.startDate)
                                      : undefined
                                  }
                                  onChange={(startDate, e) => {
                                    e.preventDefault();
                                    onChangePaDate("startDate", startDate);
                                    //   props.action.setChanged(true);
                                  }}
                                  className={`inputForm w-100`}
                                  minDate={new Date(1980, 0, 1)}
                                  maxDate={new Date(2999, 0, 1)}
                                  dateFormat="dd/MM/yyyy"
                                  placeholderText={"NOT SPECIFIED"}
                                  // disabled={props.formDisabed}
                                />
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "plannedStartDate"
                                ) ? (
                                  <label className="validation">
                                    {plannedActivityData?.startDate === "" ||
                                    plannedActivityData?.startDate === null ||
                                    plannedActivityData?.startDate === undefined
                                      ? "*planned start date must have a value"
                                      : "*planned start date must be less than planned completion"}
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>

                          <div className="col-6 pr-0">
                            <div className="col-md-12">
                              <label className={`labelForm voda-bold w-100`}>
                                Planned Completion Date
                                <span className="red">*</span>
                                <DatePicker
                                  selected={
                                    plannedActivityData?.plannedCompletion &&
                                    new Date(
                                      plannedActivityData?.plannedCompletion
                                    )
                                  }
                                  onChange={(newDate, e) => {
                                    e.preventDefault();
                                    onChangePaDate(
                                      "plannedCompletion",
                                      newDate
                                    );
                                    //   props.action.setChanged(true);
                                    // onUnkownReleaseDate();
                                  }}
                                  className={`inputForm w-100 ${
                                    isDateRange && "custom-date-picker"
                                  } `}
                                  minDate={new Date(1980, 0, 1)}
                                  maxDate={new Date(2999, 0, 1)}
                                  dateFormat="dd/MM/yyyy"
                                  placeholderText={"NOT SPECIFIED"}
                                  // disabled={props.formDisabed}
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
                              <label className={`labelForm voda-bold w-100`}>
                                Pre Baseline Date
                                <DatePicker
                                  selected={
                                    plannedActivityData?.preBaseLineDate &&
                                    new Date(
                                      plannedActivityData?.preBaseLineDate
                                    )
                                  }
                                  onChange={(newDate, e) => {
                                    e.preventDefault();
                                    onChangePaDate("preBaseLineDate", newDate);
                                    //   props.action.setChanged(true);
                                  }}
                                  className={`inputForm w-100 ${
                                    isDateRange && "custom-date-picker"
                                  }`}
                                  minDate={new Date(1980, 0, 1)}
                                  maxDate={new Date(2999, 0, 1)}
                                  dateFormat="dd/MM/yyyy"
                                  placeholderText={"NOT SPECIFIED"}
                                  // disabled={props.formDisabed}
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
                                        plannedActivityData?.plannedActivityCategoryResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.plannedActivityCategoryResource
                                        )
                                      }
                                      value={
                                        plannedActivityData &&
                                        plannedActivityData?.plannedActivityCategoryResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.plannedActivityCategoryResource
                                        ).filter(
                                          (x) =>
                                            x.key ===
                                            plannedActivityData?.plannedActivityCategoryId
                                        )
                                      }
                                      onChange={(e) => {
                                        handlePlannedCategoryChange(
                                          e,
                                          "plannedActivityCategoryId",
                                          "plannedActivityCategoryName"
                                        );

                                        //   props.action.setChanged(true);
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
                                        plannedActivityData?.lcmCategoryResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.lcmCategoryResource
                                        )
                                      }
                                      value={
                                        plannedActivityData &&
                                        plannedActivityData?.lcmCategoryResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.lcmCategoryResource
                                        ).filter(
                                          (x) =>
                                            x.key ==
                                            plannedActivityData?.lcmCategories
                                        )
                                      }
                                      onChange={(e) => {
                                        handlePlannedCategoryChange(
                                          e,
                                          null,
                                          "lcmCategories"
                                        );

                                        //   props.action.setChanged(true);
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
                                        plannedActivityData?.priorityResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.priorityResource
                                        )
                                      }
                                      value={
                                        plannedActivityData &&
                                        plannedActivityData?.priorityResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.priorityResource
                                        ).filter(
                                          (x) =>
                                            x.value ===
                                            plannedActivityData?.priority
                                        )
                                      }
                                      onChange={(e) => {
                                        handlePlannedCategoryChange(
                                          e,
                                          null,
                                          "priority"
                                        );

                                        //   props.action.setChanged(true);
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
                                value={plannedActivityData?.plannedActivityTeam}
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
                    <div className="row col-12 mx-0 px-0">
                      <div className="col-6 pl-0">
                        <div className="labelForm col-12 pl-0">
                          <label className="voda-bold w-100 mb-0">
                            Driver
                            <div className="d-flex">
                              <Select
                                menuPosition={"fixed"}
                                className="w-100"
                                options={driverOption}
                                value={
                                  driverOption &&
                                  plannedActivityData &&
                                  driverOption.filter(
                                    (el) =>
                                      el.key == plannedActivityData.driverId
                                  )
                                }
                                onChange={(e) =>
                                  onChangePADropdown("driverId", e)
                                }
                                // onKeyUp={(e) => onChangePADropdown("driverId", e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                getOptionLabel={(option) =>
                                  option.value.toString()
                                }
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                //   isDisabled={props.formDisabed}
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
                                  options={planningRiskOption}
                                  value={
                                    planningRiskOption &&
                                    plannedActivityData &&
                                    planningRiskOption.filter(
                                      (el) =>
                                        el.key ==
                                        plannedActivityData.planningRiskId
                                    )
                                  }
                                  onChange={(e) => {
                                    onChangePADropdown("planningRiskId", e);
                                    //   props.action.setChanged(true);
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
                            validation.property?.includes("planningRisk") ? (
                              <label className="validation">
                                *planning risk must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                        <Container show={true}>
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
                                    plannedActivityData &&
                                    benefitsOption.filter(
                                      (el) =>
                                        el.key == plannedActivityData.benefitId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangePADropdown("benefitId", e)
                                  }
                                  // onKeyUp={(e) =>
                                  //   onChangePADropdown("benefitId", e)
                                  // }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  getOptionLabel={(option) =>
                                    option.value.toString()
                                  }
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                  // isDisabled={props.formDisabed}
                                />
                              </div>
                            </label>
                          </div>
                        </Container>
                      </div>
                    </div>
                  </fieldset>
                  <fieldset className="fieldset p-0">
                    <label className="text-bb mt-4">Financial Details</label>
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
                                  isDisabled={disableActivityApproved}
                                  options={
                                    plannedActivityData?.budgetAvaibilityResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.budgetAvaibilityResource
                                    )
                                  }
                                  value={
                                    plannedActivityData &&
                                    plannedActivityData?.budgetAvaibilityResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.budgetAvaibilityResource
                                    ).filter(
                                      (x) =>
                                        x.key ===
                                        plannedActivityData?.budgetAvailabilityId
                                    )
                                  }
                                  onChange={(e) => {
                                    onChangePADropdown(
                                      "budgetAvailabilityId",
                                      e
                                    );
                                    //   props.action.setChanged(true);
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
                                  isDisabled={disabledInitialFunds}
                                  options={boolOptions}
                                  value={
                                    plannedActivityData &&
                                    plannedActivityData.localApproval !=
                                      undefined
                                      ? boolOptions?.find(
                                          (x) =>
                                            x.key ===
                                            plannedActivityData?.localApproval
                                        )
                                      : null
                                  }
                                  onChange={(e) => {
                                    onChangeLocalApproval(e);
                                    //   props.action.setChanged(true);
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
                            validation.property?.includes("localApproval") ? (
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
                            {plannedActivityData?.responsibilityPhaseResource &&
                            plannedActivityData?.responsibilityPhaseResource !=
                              null
                              ? Object.keys(
                                  plannedActivityData?.responsibilityPhaseResource
                                ).map((name, index) => (
                                  <label
                                    className="labelForm voda-bold   mr-3 mb-0 d-flex align-items-center"
                                    key={name}
                                  >
                                    <input
                                      type="radio"
                                      onChange={(e) => {
                                        onChangePAText(
                                          "responsibilityPhaseId",
                                          e
                                        );
                                        //   props.action.setChanged(true);
                                      }}
                                      name="responsibilityPhaseId"
                                      className=""
                                      id={name}
                                      value={name}
                                      checked={
                                        plannedActivityData?.responsibilityPhaseId &&
                                        plannedActivityData?.responsibilityPhaseId.toString() ===
                                          name
                                          ? true
                                          : false
                                      }
                                    />
                                    <label className="mb-0 ml-1" htmlFor={name}>
                                      {plannedActivityData?.responsibilityPhaseResource &&
                                        plannedActivityData
                                          ?.responsibilityPhaseResource[name]}
                                    </label>
                                  </label>
                                ))
                              : null}
                          </div>

                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link add-bt"
                              // onClick={() => setIsVisibleModalLookup(4)}
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
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold w-100">
                            Delivery Status<span className="red">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  isDisabled={true}
                                  options={
                                    DeliveryStatusArr ? DeliveryStatusArr : []
                                  }
                                  value={
                                    DeliveryStatusArr
                                      ? DeliveryStatusArr?.filter(
                                          (x) =>
                                            x.key ==
                                            plannedActivityData?.deliveryStatusId
                                        )
                                      : null
                                  }
                                  onChange={(e) =>
                                    onChangePADropdown("deliveryStatusId", e)
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
                              {props.edit &&
                              plannedActivityData?.plannedActivityId !==
                                undefined ? (
                                <button
                                  type="button"
                                  title="Update Planned Activity Status"
                                  className="btn btn-link"
                                  onClick={() =>
                                    plannedActivityData?.plannedActivityId &&
                                    openModalStatus(
                                      plannedActivityData?.plannedActivityId
                                    )
                                  }
                                >
                                  <IoIosRefresh
                                    color={`${darkMode ? "white" : "black"}`}
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
                                type="text"
                                onChange={(e) => {
                                  onChangePAText("deliveryProjectId", e);
                                  // props.action.setChanged(true);
                                }}
                                onKeyUp={(e) => {
                                  onChangePAText("deliveryProjectId", e);
                                  // props.action.setChanged(true);
                                }}
                                className="inputForm w-100"
                                value={plannedActivityData?.deliveryProjectId}
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
                          What is the risk to our customers and network if the
                          planned activity is not completed by the completion
                          date?
                        </p>
                      </label>

                      <div className="row">
                        <div className="col-md-12">
                          <div className="row">
                            <div className="col-6">
                              <label className="labelForm voda-bold w-100">
                                Risk Level
                                <span className="red">*</span>
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      options={
                                        plannedActivityData?.riskResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.riskResource
                                        )
                                      }
                                      value={
                                        plannedActivityData &&
                                        plannedActivityData?.riskResource &&
                                        dictionaryToArray(
                                          plannedActivityData?.riskResource
                                        ).filter(
                                          (x) =>
                                            x.key ===
                                            plannedActivityData?.riskEngId
                                        )
                                      }
                                      onChange={(e) =>
                                        onChangePADropdown("riskEngId", e)
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
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes("riskEngId") ? (
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
                                  onChange={(e) => {
                                    onChangePAText("riskEngineeringNotes", e);
                                    //   props.action.setChanged(true);
                                  }}
                                  onKeyUp={(e) => {
                                    onChangePAText("riskEngineeringNotes", e);
                                    //   props.action.setChanged(true);
                                  }}
                                  className="inputForm w-100"
                                  value={
                                    plannedActivityData?.riskEngineeringNotes
                                  }
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
                          What is the risk to our customers and network if the
                          planned activity is not completed by the completion
                          date?
                        </p>
                      </label>
                      <div className="row">
                        <div className="col-6">
                          <label className="labelForm voda-bold w-100">
                            Risk Level
                            <span className="red">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    plannedActivityData?.riskResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.riskResource
                                    )
                                  }
                                  value={
                                    plannedActivityData &&
                                    plannedActivityData?.riskResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.riskResource
                                    ).filter(
                                      (x) =>
                                        x.key === plannedActivityData?.riskOpeId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangePADropdown("riskOpeId", e)
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
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes("riskOpeId") ? (
                              <label className="validation">
                                *Risk Operational Evaluation must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                        <div className="col-6">
                          <label className="labelForm voda-bold w-100 mb-0">
                            Risk Evaluation Notes
                            <input
                              type="text"
                              onChange={(e) => {
                                onChangePAText("riskOperationalNotes", e);
                                //   props.action.setChanged(true);
                              }}
                              onKeyUp={(e) => {
                                onChangePAText("riskOperationalNotes", e);
                                //   props.action.setChanged(true);
                              }}
                              className="inputForm w-100"
                              value={plannedActivityData?.riskOperationalNotes}
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
                      onClick={() => setShowFurther(!showfurther)}
                    >
                      Click for further details
                    </button>
                  </div>

                  {showfurther && (
                    <>
                      <div className="col-md-12 row mx-0 px-0 d-flex align-content-start">
                        <div className="col-12 pl-0">
                          <label className="text-bb mt-4">
                            Financial Details
                          </label>
                        </div>
                        <div className="form-group col-6 pl-0">
                          <label className="labelForm voda-bold w-100 pr-0">
                            <div className="mb-3">Budget Value</div>
                            <input
                              type="number"
                              min="0.00"
                              max="1000000000.00"
                              step="1000"
                              onChange={(e) => {
                                onChangePAText("budgetValue", e);
                                //   props.action.setChanged(true);
                              }}
                              onKeyUp={(e) => {
                                onChangePAText("budgetValue", e);
                                //   props.action.setChanged(true);
                              }}
                              className="inputForm w-100 mt-0"
                              value={plannedActivityData?.budgetValue}
                            />
                          </label>
                        </div>

                        <div className="col-md-6 pr-0">
                          <label className="labelForm voda-bold w-100 col-12 pl-0">
                            <div className="mb-3">Currency</div>
                            <Select
                              menuPosition={"fixed"}
                              options={currencyOption}
                              className="ml-1 voda-bold mt-0"
                              placeholder="Currency"
                              value={
                                plannedActivityData &&
                                currencyOption.filter(
                                  (x) =>
                                    x.value === plannedActivityData?.currency
                                )
                              }
                              onChange={(e) => {
                                let copy = {
                                  ...plannedActivityData,
                                } as PlannedActivityDtoUpdate;
                                copy.currency = e?.["value"] ?? "";
                                //   props.action.setChanged(true);
                                setPlannedActivityData(copy);
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
                              type="text"
                              onChange={(e) => {
                                onChangePAText("budgetTrackingId", e);
                                //   props.action.setChanged(true);
                              }}
                              onKeyUp={(e) => {
                                onChangePAText("budgetTrackingId", e);
                                //   props.action.setChanged(true);
                              }}
                              className="inputForm w-100"
                              value={plannedActivityData?.budgetTrackingId}
                            />
                          </label>
                        </div>
                        <div className="col-6 pr-0">
                          <label className="labelForm voda-bold w-100">
                            Program
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    plannedActivityData?.programResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.programResource
                                    )
                                  }
                                  value={
                                    plannedActivityData &&
                                    plannedActivityData?.programResource &&
                                    dictionaryToArray(
                                      plannedActivityData?.programResource
                                    ).filter(
                                      (x) =>
                                        x.key === plannedActivityData?.programId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangePADropdown("programId", e)
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
                                onChangePAText("projectOwner", e);
                                //   props.action.setChanged(true);
                              }}
                              onKeyUp={(e) => {
                                onChangePAText("projectOwner", e);
                                //   props.action.setChanged(true);
                              }}
                              className="inputForm w-100"
                              value={plannedActivityData?.projectOwner}
                            />
                          </label>
                        </div>

                        <div className="col-md-12 p-0">
                          <label className="labelForm voda-bold w-100 col-12 pl-0 mt-4">
                            Notes
                            <textarea
                              onChange={(e) => {
                                onChangePAText("notes", e);
                                //   props.action.setChanged(true);
                              }}
                              onKeyUp={(e) => {
                                onChangePAText("notes", e);
                                //   props.action.setChanged(true);
                              }}
                              className="inputForm w-100"
                              value={plannedActivityData?.notes}
                            />
                          </label>
                        </div>
                        <div className="col-12 p-0">
                          {props?.edit === true ? (
                            <div className="row">
                              <div className="form-group col-6 pl-0">
                                <label className="labelForm voda-bold w-100 col-12 pr-0">
                                  Last Modified
                                  <input
                                    readOnly={true}
                                    className="inputForm w-100 voda-regular"
                                    type="text"
                                    value={formatDateWithTime(
                                      plannedActivityData?.lastModified
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
                </div>
              </form>
            </div>
          </div>
        </Tab>
      </Tabs>
      <div className="col-12 justify-content-end d-flex my-4 pt-5">
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={async () => {
            const isPAforEdit =
              plannedActivityData?.plannedActivityResourceId ?? null;
            const createPayload = {
              ...formData,
              opCoId: 0,
              serviceMasterId: 0,
              servicePlanDetails: {
                opCoId: formData?.opCoId,
                serviceMasterId: formData?.serviceMasterId,
                plannedActivityId: 0,
              },
              plannedActivityCreateDto:
                isPAforEdit !== null && isPAforEdit !== 0
                  ? {
                      ...plannedActivityData,
                      opCoId: formData?.opCoId,
                      isServicePlan: true,
                    }
                  : null,
            } as ServiceLevelPlanDtoUpdate;

            const editPayload = {
              ...formData,
              servicePlanDetails: {
                opCoId: formData?.opCoId,
                serviceMasterId: formData?.serviceMasterId,
                servicePlanId: formData?.servicePlanId,
              },
              plannedActivityId: formData?.plannedActivityId ?? 0,
              plannedActivityCreateDto:
                isPAforEdit !== null &&
                isPAforEdit !== 0 &&
                formData?.plannedActivityId === 0
                  ? {
                      ...plannedActivityData,
                      opCoId: formData?.opCoId,
                      isServicePlan: true,
                    }
                  : null,
              plannedActivityUpdateDto:
                isPAforEdit !== null &&
                isPAforEdit !== 0 &&
                formData?.plannedActivityId !== 0
                  ? {
                      ...plannedActivityData,
                      opCoId: formData?.opCoId,
                      isServicePlan: true,
                    }
                  : null,
            } as ServiceLevelPlanDtoUpdate;

            const isFormValid = !!(
              formData && formValidationClient(formData)?.response === true
            );
            const isPAFormValid = !!(
              plannedActivityData &&
              validazioneClient(plannedActivityData)?.response === true
            );
            if (isPAforEdit !== null && isPAforEdit !== 0) {
              setKey("plannedActivities");
            }
            if (
              (isPAforEdit !== null && isPAforEdit !== 0
                ? isPAFormValid
                : true) &&
              isFormValid
            ) {
              if (props?.edit) {
                const response = await EditServicePlan(editPayload);
                if (response?.ResultDtoEdit?.warning === false) {
                  refresh();
                }
              } else {
                const response = await CreateServicePlan(createPayload);
                if (response?.ResultDtoCreate?.warning === false) {
                  refresh();
                }
              }
            } else {
              rootStore.dispatch(
                setNotification({
                  message: "Check the fields entered",
                  notifyType: NotifyType.warning,
                })
              );
              return;
            }
          }}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default ServiceLevelPlannedActivityModal;
