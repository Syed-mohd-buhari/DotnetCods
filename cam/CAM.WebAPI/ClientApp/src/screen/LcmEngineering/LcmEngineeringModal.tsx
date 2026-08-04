import React, { useState, useEffect, useCallback } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  formatDateWithTime,
  changeDate,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
  getMaxDate,
} from "../../Hook/Common";
import { useDispatch, useSelector } from "react-redux";
import Select from "react-select";
import {
  LcmEngineeringCreate,
  LcmEngineeringDtoUpdate,
  NetworkElementAssociated,
} from "../../Model/LcmEngineering";
import { EditLcmEngineering } from "../../Redux/Action/LcmEngineering/LcmEngineeringEditAction";
import {
  CreatLcmEngineering,
  GetLCMResourceKey,
  GetLcmEngineeringCreateResource,
} from "../../Redux/Action/LcmEngineering/LcmEngineeringCreateAction";
import setLoader from "../../Redux/Action/LoaderAction";
import { GetLcmEngineeringGrid } from "../../Redux/Action/LcmEngineering/LcmEngineeringGridAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { Tabs, Tab, Modal } from "react-bootstrap";
// import ModalPlannedActivities from "../PlannedActivities/PlannedActivitiesModal"; //previously used planned activity reused component
import LcmEngPlannedActivity from "./LcmEngPlannedActivity";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { GetPlannedActivityCreateResource } from "../../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { deletePlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityDeleteAction";
import { GetPlannedActivityEditResource } from "../../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import { GetPlannedActivityGrid } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityDtoGrid,
  PlannedActivityQueryObjectGrid,
} from "../../Model/PlannedActivity";
import ProductImportanceContainer from "../../Containers/Lookup/ProductImportanceContainer";
import SupportedResourceContainer from "../../Containers/Lookup/SupportedResourceContainer";
import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import ReasonCheckboxContainer from "../../Containers/Lookup/ReasonCheckboxContainer";
import FullOrPartialResourceContainer from "../../Containers/Lookup/FullOrPartialResourceContainer";
import SubdomainSpocContainer from "../../Containers/Lookup/SubdomainSpocContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import { ReasonCheckboxDto } from "../../Model/LookUp/ReasonCheckbox";
import { TipologicaGridDtoRule } from "../../Model/LookUp/LookUpGenericModel";
import {
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  getResourceObject,
  stateConfirm,
} from "../../Model/Common";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import DatePicker from "react-datepicker";
import {
  GetMajorSoftwareBuildEoS,
  GetMajorHardwareBuildEoS,
  GetCreatedDCID,
} from "../../Redux/Action/LcmEngineering/LcmEngineeringCommonAction";
import {
  dictionaryToArray,
  dictionaryToArrayGridDtoRule,
  dictionaryToArrayPlannedActivityResourceDto,
  dictionaryToArrayReasonCheckbox,
  dictionaryToArrayLocationDto,
  dictionaryToArrayDeploymentStatusDto,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import { NetworkElementAsPlannedDtoCreate } from "../../Model/NetworkElementAsPlanned";
import { LocationDto } from "../../Model/LookUp/Location";
import { GetNetworkElementAsPlannedCreateResource } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCreateAction";
import Location from "../../Containers/Lookup/LocationContainer";
import EnvironmentContainer from "../../Containers/Lookup/EnvironmentContainer";
import {
  GetNetworkElementAssociateds,
  GetNetworkElementOpCo,
} from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCommonAction";
import SharedLookUp from "../../Containers/Lookup/SharedLookUpContainer";
import {
  GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource,
  SaveUpdatedAssetDetails,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import { copyFile } from "fs";
import { IsDCHasNfxiBuildConstruction } from "../../Redux/Action/DesignComponent/DesignComponentCommonAction";
import { DeploymentStatusDto } from "../../Model/LookUp/DeploymentStatus";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions, DialogProps } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import PlannedActivityResource from "../../Containers/Lookup/PlannedActivityResourceContainer";
import BuildBagItemComponent from "../../Containers/BuildBagComponent";
import { FaRegEye } from "react-icons/fa";
import ViewMappedComponent from "../../Containers/ViewMappedComponent";
import { useNavigate } from "react-router";
import TourGuide from "../../Components/TourGuide";
import { startGuideTour, endGuideTour } from "../../Redux/Action/tourActions";
import { getLcmModalTourSteps } from "../../Constant/TourSteps";

interface ExtEos {
  data: Date | null;
  verificata: boolean;
}

let paginationQueryPlanning: PlannedActivityQueryObjectGrid = {
  plannedImplementationYear: [],
  activityStatusId: [],
  buildBagId: [],
  planningActivityStatusId: [],
  designComponentId: [],
  designComponentFamilyid: [],
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
  lcmEngineeringId: [],
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
  // data: LcmEngineeringDtoUpdate | LcmEngineeringDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
  idDetail?: number | string | undefined | null;
  formDisabed?: boolean;
  lcmRedirect?: boolean;
  prevPage?: string;
  resetLocalState?(type: boolean): any;
}

const LcmEngineeringModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("operational");
  const {
    formData,
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
    checkIsExist,
  } = useFormTableCrud<LcmEngineeringDtoUpdate>(
    CreatLcmEngineering,
    EditLcmEngineering
  );
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const handleEndTour = () => dispatch(endGuideTour());
  const dtoEditResourceState = (state: RootState) =>
    state.lcmEngineeringEditReducer.LcmEngineeringDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.lcmEngineeringCreateReducer.LcmEngineeringDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const Grid = (state: RootState) => state.locationGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);

  const GridEnvironments = (state: RootState) =>
    state.environmentGridReducer.LookUpGridResult;
  const GridDtoEnvironemnts = useSelector(GridEnvironments);

  const GridAllSharedLookUp = (state: RootState) =>
    state.sharedLookUpGridReducer.LookUpGridResultAll;
  const GridDtoAllSharedLookUp = useSelector(GridAllSharedLookUp);

  const dtoNewResourceStateNetwork = (state: RootState) =>
    state.networkElementAsPlannedCreateReducer.NetworkElementAsPlannedDtoCreate;
  let createResourceNetwork = useSelector(dtoNewResourceStateNetwork);

  const [swInwarranty, setSwInwarranty] = useState("");
  const [warranty, setWarranty] = useState<boolean>(false);
  const [
    checkChangeReasonCheckboxSoftware,
    setCheckChangeReasonCheckboxSoftware,
  ] = useState<boolean>(false);

  const [
    checkChangeReasonCheckboxHardware,
    setCheckChangeReasonCheckboxHardware,
  ] = useState<boolean>(false);
  const [onHardware, setOnHardware] = useState<boolean>(false);
  const [both, setBoth] = useState<boolean>(false);
  const [isBuildBagItemFlag, setIsBuildBagItemFlag] = useState<boolean>(false);
  const [viewBagFlag, setViewBagFlag] = useState<boolean>(false);
  const [onSoftware, setOnSoftware] = useState<boolean>(false);
  const [checkAddedNetwork, setAddedNetwork] = useState<boolean>(false);
  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [defaultLCMStatus, setDefaultLCMStatus] = useState<any>();
  const [verificaData, setverificaData] = useState({
    data: null,
    verificata: false,
  } as ExtEos);

  const [networkElementAssociateds, setNetworkElementAssociateds] = useState<
    Array<NetworkElementAssociated>
  >([]);
  const [buildBagRes, setBuildBagRes] = useState<
    Array<{ key: number; text: string }>
  >([]);

  const { NetworkElementAsPlannedDtoCreate } = useSelector(
    (state: any) => state?.networkElementAsPlannedCreateReducer
  );
  const { deploymentStatusReosurce } = NetworkElementAsPlannedDtoCreate || {};
  const [verificaDataHW, setverificaDataHW] = useState({
    data: null,
    verificata: false,
  } as ExtEos);

  const [showSectionFurther, setShowSectionFurther] = useState<boolean>(false);

  const [networkElementToAdd, setNetworkElementToAdd] =
    useState<NetworkElementAsPlannedDtoCreate | null>();

  const [toggleResource, setToggleResource] = useState<boolean>(false);
  const [lcmDeploymentStatusString, setlcmDeploymentStatusString] =
    useState<string>();
  const [designComponentFamilyName, setDesignComponentFamilyName] =
    useState<string>();
  const [validationError, setValidationError] = useState<boolean>(false);
  const [editKeyName, setEditKeyName] = useState<boolean>(false);
  const [resourceLifeCycle, setResourceLifeCycle] = useState<string>();
  const [resourceKeyError, setResourceKeyError] = useState<boolean>(false);
  const [prevResourceKeyError, setPrevResourceKeyError] =
    useState<boolean>(false);
  const [editResourceKey, setEditResourceKey] = useState<boolean>(false);
  const [editPrevResourceKey, setEditPrevResourceKey] =
    useState<boolean>(false);
  const [assetsStatus, setAssetsStatus] = useState<any>(
    deploymentStatusReosurce
  );
  const [environmentSelect, setEnvironmentSelect] = useState<boolean>(false);
  const [isUnlocked, setIsUnlocked] = useState(false);

  const [mergedDesignComponents, setMergedDesignComponents] = useState<
    { key: number; value: string; color?: string }[] | undefined
  >([]);

  useEffect(() => {
    if (environmentSelect) {
      const defaultLocation =
        networkElementToAdd?.locationReosurce &&
        dictionaryToArrayLocationDto(networkElementToAdd?.locationReosurce)
          .map((x) => {
            return {
              key: x.key,
              value: x.value,
            };
          })
          .filter((x) => {
            return x.value.opcoId == formData?.opCoId && x.value.defaultValue;
          });
      if (defaultLocation?.length) {
        onChangeLocation(defaultLocation[0]);
      }
    }
  }, [environmentSelect]);

  useEffect(() => {
    if (formData && formData.designComponentId) {
      GetMajorSoftwareBuildEoS(formData.designComponentId).then((x) =>
        setverificaData({
          data: x != null ? new Date(x) : null,
          verificata: true,
        })
      );

      GetMajorHardwareBuildEoS(formData.designComponentId).then((x) => {
        setverificaDataHW({
          data: x != null ? new Date(x) : null,
          verificata: true,
        });
      });
    } else {
      setverificaData({ data: null, verificata: false });
      setverificaDataHW({ data: null, verificata: false });
    }
  }, [formData?.designComponentId]);

  useEffect(() => {
    if (!formData?.designComponentResource) {
      setMergedDesignComponents([]);
      return;
    }

    const designComponentArray = dictionaryToArray(
      formData.designComponentResource
    );
    const transientDC = formData.transientDesignComponentResource;

    const transientDCKeys = transientDC
      ? Object.keys(transientDC)
          .filter((key) => !isNaN(parseInt(key, 10)))
          .map((key) => parseInt(key, 10))
      : [];

    const mergedArray = designComponentArray.map((dc) => ({
      key: dc.key,
      value: dc.value,
      color: transientDCKeys.includes(dc.key) ? "#80400B" : "#000000",
    }));

    setMergedDesignComponents(mergedArray);
  }, [
    formData?.designComponentResource,
    formData?.transientDesignComponentResource,
  ]);

  useEffect(() => {
    if (checkIsExist) {
      setFormData(props.edit ? editResource : createResource);
    }
  }, [checkIsExist]);

  useEffect(() => {
    if (formData && formData.designComponentId && formData.opCoId) {
      // const selectResource = {
      //   opCoId: formData?.opCoId.toString(),
      //   designComponentId: formData?.designComponentId.toString(),
      // };
      GetNetworkElementAssociateds(
        formData.designComponentId,
        formData.opCoId
      ).then((x) => {
        let copy = { ...formData } as LcmEngineeringDtoUpdate;
        // console.log("formData", copy);
        let arr = [] as NetworkElementAssociated[];
        copy.networkElementAssociateds?.map((item, i) => {
          if (numberIsNullOrZero(item.id)) {
            arr.push(item);
          }
        });
        if (x && x != undefined) {
          arr.push(...x);
        }

        copy.networkElementAssociateds = arr;
        setNetworkElementAssociateds(arr);

        //API call to get Resource Key value

        // !props?.edit &&
        //   GetLCMResourceKey(selectResource).then((x) => {
        //     if (x) {
        //       setFormData({ ...copy, resourceKey: x?.data });
        //     }
        //   });
        // setFormData(copy);
      });
    }
  }, [formData?.designComponentId, formData?.opCoId]);

  useEffect(() => {
    if (formData && formData.designComponentId && formData.opCoId) {
      GetNetworkElementOpCo({
        dcId: formData.designComponentId,
        opCoId: formData.opCoId,
        lcmBagId: 0,
      }).then((x) => {
        if (x && Array.isArray(x)) {
          setBuildBagRes(x); // Store original array of {key, text} objects
        } else {
          setBuildBagRes([]);
        }
      });
    }
  }, [formData?.designComponentId, formData?.opCoId]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (networkElementToAdd == undefined) {
      GetNetworkElementAsPlannedCreateResource().then((x) => {
        setNetworkElementToAdd(x);
      });
    }

    if (props.edit) {
      const deploymentstatus: string = dictionaryToArray(
        editResource?.lcmDeploymentStatusResource!
      ).find((x) => x.key === editResource?.lcmDeploymentStatusId)?.value!;
      setlcmDeploymentStatusString(deploymentstatus);
      setFormData(editResource);
      calculateSoftwareInWarranty(formData || ({} as LcmEngineeringDtoUpdate));
      setautoResetHW(false);
      setautoResetSW(false);
    } else {
      setFormData(createResource);
      setDefaultLCMStatus(createResource?.lcmDeploymentStatusResource);
      setautoResetHW(true);
      setautoResetSW(true);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (formData?.plannedActivityDto && formData?.plannedActivityDto != null) {
      let dataArray = formData?.plannedActivityDto;
      let copy = { ...formData } as LcmEngineeringDtoUpdate;

      if (
        dataArray
          .filter(
            (x) =>
              x.plannedActivityResource != undefined &&
              x.plannedActivityResourceId != undefined &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x.plannedActivityResourceId)?.value
                .lcmHardware === true
          )
          .filter(
            (x) =>
              x.plannedActivityResource != undefined &&
              x.plannedActivityResourceId != undefined &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x.plannedActivityResourceId)?.value
                .exportable === true
          ).length > 0
      ) {
        setOnHardware(true);
        copy.onHardware = true;
      } else {
        setOnHardware(false);
        copy.onHardware = false;
      }

      if (
        dataArray
          .filter(
            (x) =>
              x.plannedActivityResource != undefined &&
              x.plannedActivityResourceId != undefined &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x.plannedActivityResourceId)?.value
                .lcmSoftware === true
          )
          .filter(
            (x) =>
              x.plannedActivityResource != undefined &&
              x.plannedActivityResourceId != undefined &&
              dictionaryToArrayPlannedActivityResourceDto(
                x.plannedActivityResource
              ).find((y) => y.key === x.plannedActivityResourceId)?.value
                .exportable === true
          ).length > 0
      ) {
        setOnSoftware(true);
        copy.onSoftware = true;
      } else {
        setOnSoftware(false);
        copy.onSoftware = false;
      }
      setFormData(copy);
    }
  }, [formData?.plannedActivityDto]);

  useEffect(() => {
    if (props.keyTab === "" || props.keyTab == null || !props.edit) {
      setKey("operational");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  useEffect(() => {
    if (
      formData !== undefined &&
      !props.edit &&
      formData?.lcmDeploymentStatusId !== undefined
    ) {
      if (formData?.lcmDeploymentStatusId === 4) {
        setShowForm(false);
      } else {
        setShowForm(true);
      }
    }
  }, [formData?.lcmDeploymentStatusId]);

  useEffect(() => {
    calculateSoftwareInWarranty(formData || ({} as LcmEngineeringDtoUpdate));
    if (formData?.warranty == true) {
      setWarranty(true);
    } else {
      setWarranty(false);
    }

    if (
      props.edit &&
      formData !== undefined &&
      formData?.resourceKey !== undefined &&
      formData?.resourceKey !== null &&
      resourceLifeCycle === undefined
    ) {
      const resArr = formData?.resourceKey?.split("_");
      setResourceLifeCycle(resArr[1]);
    }
  }, [formData]);

  // useEffect(() => {
  //   console.log("Working");
  //   if (formData && formData.designComponentFamilyid) {
  //     GetCreatedDCID(formData.designComponentFamilyid).then((x) => {
  //       let copy = { ...formData } as LcmEngineeringDtoUpdate;
  //       copy.designComponentId = x;
  //       setFormData(copy);
  //     });
  //   }
  // }, [formData?.designComponentFamilyid]);

  const onChangeCheckbox = (property: string, event: any) => {
    setChanged(true);
    setValidation(null);
    const checked = event.target.checked;
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy[property] = checked;
    if (property === "sparesProvisioned" && checked) {
      copy.checkboxResourceLcmEngineeringHardwares = undefined;
      setCheckChangeReasonCheckboxHardware(false);
    }

    setFormData(copy);
  };

  const onChangeReleaseDetails = (property: string, event: any) => {
    const checked = event.target.checked;
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy[property] = checked;
    setFormData(copy);
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    if (property == "subDomainSpocIds" && e != null && e !== undefined) {
      e = [e];
    }
    let array = [] as Array<number>;
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    if (e != null && e.length > 0 && e !== undefined) {
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

  const onChangeWarranty = (checked: boolean) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy.warranty = checked;

    if (checked == true) {
      setWarranty(checked);
    } else {
      setWarranty(checked);
      copy.softwareEndOfWarrantyDate = undefined;
      copy.isExtendedSupportOfferedByVendor = false;
      copy.hwIsExtendedSupportOfferedByVendor = false;
      calculateSoftwareInWarranty(copy || {});
    }
    setRuleSW(0);
    copy.softwareSupportedId = undefined;
    copy.checkboxResourceLcmEngineeringSoftwares = undefined;
    copy.softwareSupportProvider = undefined;
    copy.softwareEndOfSupportContract = undefined;
    copy.fullorPartialSupportId = undefined;
    setFormData(copy);
  };

  const onChangeCheckSliderInput = (type: string, e: any) => {
    let checked = e.target.checked;
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    type === "SW"
      ? (copy.isExtendedSupportOfferedByVendor = checked)
      : (copy.hwIsExtendedSupportOfferedByVendor = checked);
    setFormData(copy);
  };

  //VALIDAZIONE PRE Save
  const validazioneClient = (copy: LcmEngineeringDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opCoId == null ||
      copy?.opCoId === undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }
    if (
      copy?.buildBagId == null ||
      copy?.buildBagId === undefined ||
      copy?.buildBagId === 0
    ) {
      addInvalidProperty("buildBagId");
    }
    if (
      copy?.isReleaseDetailUnKnown == false &&
      (copy?.designComponentId == null ||
        copy?.designComponentId === undefined ||
        copy?.designComponentId === 0)
    ) {
      addInvalidProperty("designComponentId");
    }
    if (
      copy?.isReleaseDetailUnKnown == true &&
      (copy?.designComponentFamilyid == null ||
        copy?.designComponentFamilyid === undefined ||
        copy?.designComponentFamilyid === 0)
    ) {
      addInvalidProperty("designComponentFamilyid");
    }
    if (
      copy?.lcmDeploymentStatusId == null ||
      copy?.lcmDeploymentStatusId === undefined ||
      copy?.lcmDeploymentStatusId === 0
    ) {
      addInvalidProperty("lcmDeploymentStatusId");
    }
    if (
      (copy?.numberOfNodes == null ||
        copy?.numberOfNodes === undefined ||
        copy.numberOfNodes.toString() === "") &&
      !copy.elementCount
    ) {
      addInvalidProperty("numberOfNodes");
    }
    if (
      (copy?.numberOfNodesInLab == null ||
        copy?.numberOfNodesInLab === undefined ||
        copy.numberOfNodesInLab.toString() === "") &&
      !copy.elementCount
    ) {
      addInvalidProperty("numberOfNodesInLab");
    }
    if (
      copy?.productImportanceId == null ||
      copy?.productImportanceId === undefined ||
      copy?.productImportanceId === 0
    ) {
      addInvalidProperty("productImportanceId");
    }
    if (
      (copy?.softwareEndOfWarrantyDate == null ||
        copy?.softwareEndOfWarrantyDate === undefined) &&
      warranty
    ) {
      addInvalidProperty("softwareEndOfWarrantyDate");
    }
    if (
      (copy?.softwareEndOfSupportContract == null ||
        copy?.softwareEndOfSupportContract == undefined) &&
      (ruleSW == 1 || ruleSW == 2)
    ) {
      addInvalidProperty("softwareEndOfSupportContract");
    }
    if (
      (copy?.hardwareEndOfSupportContract == null ||
        copy?.hardwareEndOfSupportContract == undefined) &&
      (ruleHW == 1 || ruleHW == 2)
    ) {
      addInvalidProperty("hardwareEndOfSupportContract");
    }

    const reg =
      /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    const isAValidEmail =
      copy?.operationalContact && reg.test(copy?.operationalContact);

    if (
      copy?.eduSpocIds == null ||
      copy?.eduSpocIds === undefined ||
      copy?.eduSpocIds.length === 0
    ) {
      addInvalidProperty("eduSpocIds");
    }
    if (
      copy?.subDomainSpocIds == null ||
      copy?.subDomainSpocIds === undefined ||
      copy?.subDomainSpocIds.length === 0
    ) {
      addInvalidProperty("subDomainSpocIds");
    }
    if (
      props?.edit &&
      (resourceKeyError === true ||
        copy?.resourceKey === null ||
        copy?.resourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("resourceKey");
    }
    if (
      props?.edit &&
      formData?.previousResourceKey !== null &&
      (prevResourceKeyError === true ||
        copy?.previousResourceKey === null ||
        copy?.previousResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("previousResourceKey");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const onChangeOPco = (e: any) => {
    if (networkElementToAdd) {
      networkElementToAdd.locationId = undefined;
      setNetworkElementToAdd(networkElementToAdd);
    }
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const calculateSoftwareInWarranty = (copy: LcmEngineeringDtoUpdate) => {
    if (copy.softwareEndOfWarrantyDate !== undefined) {
      if (new Date(copy.softwareEndOfWarrantyDate) >= new Date()) {
        setSwInwarranty("YES");
      } else {
        setSwInwarranty("NO");
      }
    } else {
      setSwInwarranty("");
    }
  };

  //Modifiche Planning
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQueryPlanning,
    isPermesso ? GetPlannedActivityGrid : undefined
  );
  const [dataPlanning, setDataPlanning] = useState<
    PlannedActivityDtoGrid[] | undefined
  >([]);
  const [designComponentState, setDesignComponentState] = useState<
    any | undefined
  >({});
  const { New, Edit, Delete } = useOperationTableCrud<
    PlannedActivityDtoUpdate,
    PlannedActivityDtoCreate
  >(
    GetPlannedActivityCreateResource,
    GetPlannedActivityEditResource,
    deletePlannedActivity,
    refresh
  );

  const AddOnList = async (item: PlannedActivityDtoUpdate[]) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    const dates = item.map((ele) => ele.plannedCompletion);
    const maxDateObj = getMaxDate(dates);

    if (
      copy.plannedActivityDto === null ||
      copy.plannedActivityDto === undefined
    ) {
      copy.plannedActivityDto = [];
    }
    item?.length > 0 &&
      item.map((x) => {
        const { reasonForNoPlan, commentOnProjectStatus, ...rest } =
          x as PlannedActivityDtoUpdate;
        if (
          x?.plannedActivityResource &&
          dictionaryToArrayPlannedActivityResourceDto(
            x?.plannedActivityResource
          ).filter(
            (res) =>
              res.value.plannedActivityResourceId ==
                x?.plannedActivityResourceId && res.value.ruleLinkedDc == 15
          ).length > 0
        ) {
          copy.reasonForNoPlan = reasonForNoPlan;
          copy.commentOnProjectStatus = commentOnProjectStatus;
        }
      });
    copy.plannedActivityDto = item;
    const paRes: any = item[maxDateObj.index].plannedActivityResource;
    const paID: any = item[maxDateObj.index].plannedActivityResourceId;
    const isPACheck =
      dictionaryToArrayPlannedActivityResourceDto(paRes).filter(
        (res) =>
          res.value.plannedActivityResourceId == paID &&
          res.value.ruleLinkedDc == 15
      ).length > 0
        ? true
        : false;
    // console.log("item", paRes, paID, isPACheck);
    const selectedPA = dictionaryToArrayPlannedActivityResourceDto(paRes).find(
      (x) => x.key == paID
    );

    if (
      item &&
      item[maxDateObj.index].plannedActivityResourceId &&
      !isPACheck
    ) {
      let result =
        await GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
          item[maxDateObj.index].plannedActivityResourceId,
          item[maxDateObj.index].deliveryStatusId
        );
      if (lcmDeploymentStatusString === "In-Commissioning") {
        if (dictionaryToArray(result?.data!)[0].value !== "Planned") {
          copy.lcmDeploymentStatusResource = result?.data;
        }
      } else {
        copy.lcmDeploymentStatusResource = result?.data;
      }
      if (
        dictionaryToArray(result?.data!).length === 1 &&
        (lcmDeploymentStatusString !== "Planned" ||
          selectedPA?.value.ruleLinkedDc === 8 ||
          selectedPA?.value.ruleLinkedDc == 12)
      ) {
        if (lcmDeploymentStatusString !== "In-Commissioning") {
          copy.lcmDeploymentStatusId = dictionaryToArray(result?.data!)[0].key;
          const lcmstatus = dictionaryToArray(result?.data!)[0].value;
          // setlcmDeploymentStatusString(lcmstatus)
        }
      }

      if (props.edit === true && lcmDeploymentStatusString === "Planned") {
        let res = await SaveUpdatedAssetDetails({
          designComponentFamilyid: copy.designComponentFamilyid
            ? copy.designComponentFamilyid
            : null,
          designComponentId: copy.designComponentId
            ? copy.designComponentId
            : null,
          isReleaseDetailUnKnown: copy.isReleaseDetailUnKnown,
          subDomainSpocIds: copy.subDomainSpocIds,
          eduSpocIds: copy.eduSpocIds,
          opCoId: copy.opCoId,
          networkElementAssociateds: networkElementAssociateds,
        });
      }
    }

    setFormData(copy);
  };

  const GetElementFromList = (index: number) => {
    return formData?.plannedActivityDto && formData?.plannedActivityDto[index];
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const OpCoRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.LcmEngineeringDtoCreate
        ?.opCoResource
        ? res?.LcmEngineeringDtoCreate?.opCoResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      setFormData({ ...formData, opCoResource: obj });
    } catch (error) {
      console.error("Error in OpCoRefillData:", error);
    }
  };

  const ProductImportanceRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.LcmEngineeringDtoCreate
        ?.productImportanceResource
        ? res?.LcmEngineeringDtoCreate?.productImportanceResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      setFormData({ ...formData, productImportanceResource: obj });
    } catch (error) {
      console.error("Error in ProductImportanceRefillData:", error);
    }
  };

  const FullOrPartialResourceRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.LcmEngineeringDtoCreate
        ?.fullorPartialSupportResource
        ? res?.LcmEngineeringDtoCreate?.fullorPartialSupportResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      setFormData({ ...formData, fullorPartialSupportResource: obj });
    } catch (error) {
      console.error("Error in FullOrPartialResourceRefillData:", error);
    }
  };
  const CheckboxReasonTypeRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.LcmEngineeringDtoCreate?.checkboxResourceResource
        ? res?.LcmEngineeringDtoCreate?.checkboxResourceResource
        : value ?? [];
      setFormData({
        ...formData,
        checkboxResourceResource: obj.reduce(
          (acc, item) => ({ ...acc, [item.id]: item }),
          {}
        ),
      });
    } catch (error) {
      console.error("Error in CheckboxReasonTypeRefillData:", error);
    }
  };

  const SupportedResourceRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.LcmEngineeringDtoCreate?.supportedResource
        ? res?.LcmEngineeringDtoCreate?.supportedResource
        : value ?? [];
      setFormData({
        ...formData,
        supportedResource: obj.reduce(
          (acc, item) => ({ ...acc, [item.id]: item }),
          {}
        ),
      });
    } catch (error) {
      console.error("Error in SupportedResourceRefillData:", error);
    }
  };

  const SubDomainSpocRefillData = (value: Array<any>) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    const EduSpocObject = {} as { [key: string]: string };
    const SubDomainObject = {} as { [key: string]: string };
    if (copy && copy.eduSpocResource) {
      let newCopy = value
        .filter((itm) => itm.isEdu === true)
        .reduce(
          (acc, item) => ({ ...acc, [item.id]: item.description }),
          {}
        ) as {
        [key: string]: string;
      };
      let inputObject = { ...copy.eduSpocResource, ...newCopy };
      const uniqueValues = new Set(Object.values(inputObject));
      for (const value of uniqueValues) {
        const key: any = Object.keys(inputObject).find(
          (k) => inputObject[k] === value
        );
        EduSpocObject[key] = value;
      }
    }

    if (copy && copy.subDomainSpocResource) {
      let newCopy = value
        .filter((itm) => itm.isSubDomain === true)
        .reduce(
          (acc, item) => ({ ...acc, [item.id]: item.description }),
          {}
        ) as {
        [key: string]: string;
      };
      let inputObject = { ...copy.subDomainSpocResource, ...newCopy };
      const uniqueValues = new Set(Object.values(inputObject));
      for (const value of uniqueValues) {
        const key: any = Object.keys(inputObject).find(
          (k) => inputObject[k] === value
        );
        SubDomainObject[key] = value;
      }
    }

    setFormData({
      ...copy,
      eduSpocResource: EduSpocObject,
      subDomainSpocResource: SubDomainObject,
    });
  };

  const RemoveSubDomainSpocRefillData = (id: any) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    const EduSpocObject = { ...copy.eduSpocResource } as {
      [key: string]: string;
    };
    const SubDomainObject = { ...copy.subDomainSpocResource } as {
      [key: string]: string;
    };
    if (
      copy.eduSpocResource &&
      EduSpocObject &&
      EduSpocObject.hasOwnProperty(id)
    ) {
      delete EduSpocObject[id];
    }
    if (
      copy.subDomainSpocResource &&
      SubDomainObject &&
      SubDomainObject.hasOwnProperty(id)
    ) {
      delete SubDomainObject[id];
    }
    setFormData({
      ...copy,
      eduSpocResource: EduSpocObject,
      subDomainSpocResource: SubDomainObject,
    });
  };

  const OperationalContractRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.LcmEngineeringDtoCreate?.operationalContractResource
        ? res?.LcmEngineeringDtoCreate?.operationalContractResource
        : value ?? [];
      setFormData({
        ...formData,
        operationalContractResource: obj.reduce(
          (acc, item) => ({ ...acc, [item.id]: item.description }),
          {}
        ),
      });
    } catch (error) {
      console.error("Error in OperationalContractRefillData:", error);
    }
  };

  const LcmDeploymentStatusRefillData = async (value: Array<any>) => {
    try {
      const res: LcmEngineeringCreate = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.LcmEngineeringDtoCreate
        ?.operationalContractResource
        ? res?.LcmEngineeringDtoCreate?.operationalContractResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      setFormData({ ...formData, operationalContractResource: obj });
    } catch (error) {
      console.error("Error in LcmDeploymentStatusRefillData:", error);
    }
  };

  //LOOKUP NETWORK ELEMENT

  const EnvironmentContainerRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetNetworkElementAsPlannedCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.environmentReosurce
        ? res?.environmentReosurce
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];

      setNetworkElementToAdd({
        ...networkElementToAdd,
        environmentReosurce: obj,
      });
    } catch (error) {
      console.error("Error in EnvironmentContainerRefillData:", error);
    }
  };

  const returnSourceData = (value: Array<any>): any => {
    var obj = value.reduce((acc, item) => ({ ...acc, [item.id]: item }), {});
    return obj;
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

  const ReturnLookupContainer = useCallback(
    (value: number) => {
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
            <ProductImportanceContainer
              returnObject={ProductImportanceRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></ProductImportanceContainer>
          );
        case 3:
          return (
            <SupportedResourceContainer
              returnObject={SupportedResourceRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></SupportedResourceContainer>
          );
        case 4:
          return (
            <ReasonCheckboxContainer
              returnObject={CheckboxReasonTypeRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></ReasonCheckboxContainer>
          );
        case 5:
          return (
            <FullOrPartialResourceContainer
              returnObject={FullOrPartialResourceRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></FullOrPartialResourceContainer>
          );
        case 8:
          return (
            <SubdomainSpocContainer
              subDomain={false}
              returnObject={SubDomainSpocRefillData}
              removedObject={RemoveSubDomainSpocRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></SubdomainSpocContainer>
          );
        case 9:
          return (
            <SubdomainSpocContainer
              subDomain={true}
              returnObject={SubDomainSpocRefillData}
              removedObject={RemoveSubDomainSpocRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></SubdomainSpocContainer>
          );
        case 7:
          return (
            <Location
              returnObject={LocationRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            />
          );
        case 6:
          return (
            <EnvironmentContainer
              returnObject={EnvironmentContainerRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            />
          );
        case 10:
          return (
            <SharedLookUp
              returnObject={OperationalContractRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
              apiType="OperationalContract"
            />
          );
        case 11:
          return (
            <SharedLookUp
              returnObject={LcmDeploymentStatusRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
              apiType="LcmDeploymentStatus"
            />
          );

        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );

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

  const [opco, setOpco] = useState<string>();
  const [designComponent, setDesignComponent] = useState<
    { key: number; value: string } | undefined
  >();
  const [designComponentFamily, setDesignComponentFamily] = useState<
    { key: number; value: string } | undefined
  >();
  const [productImportance, setProductImportance] = useState<
    { key: number; value: string } | undefined
  >();
  const [eduSpoc, setEduSpoc] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [subdomainSpoc, setSubdomainSpoc] = useState<
    { key: number; value: string }[] | undefined
  >();

  //PROPS PLANNED ACTIVITY
  useEffect(() => {
    if (formData?.opCoId != undefined && formData.opCoResource != undefined) {
      let opcoString = dictionaryToArray(formData.opCoResource).find(
        (x) => x.key == formData.opCoId
      )?.value;
      setOpco(opcoString);
    } else {
      setOpco("");
    }

    if (
      formData?.designComponentId != undefined &&
      formData.designComponentResource != undefined
    ) {
      let designComponent = dictionaryToArray(
        formData.designComponentResource
      ).find((x) => x.key == formData.designComponentId);
      if (designComponent?.key && designComponent.value)
        setDesignComponent({
          key: designComponent?.key,
          value: designComponent?.value,
        });
    } else {
      setDesignComponent(undefined);
    }
    if (
      formData?.designComponentFamilyid != undefined &&
      formData.designComponentFamilyResource != undefined
    ) {
      let designComponentFamily = dictionaryToArray(
        formData.designComponentFamilyResource
      ).find((x) => x.key == formData.designComponentFamilyid);
      if (designComponentFamily?.key && designComponentFamily.value)
        setDesignComponentFamily({
          key: designComponentFamily?.key,
          value: designComponentFamily?.value,
        });
    } else {
      setDesignComponentFamily(undefined);
    }

    if (
      formData?.productImportanceId != undefined &&
      formData.productImportanceResource != undefined
    ) {
      let productImportance = dictionaryToArray(
        formData.productImportanceResource
      ).find((x) => x.key == formData.productImportanceId);
      if (productImportance?.key && productImportance.value)
        setProductImportance({
          key: productImportance?.key,
          value: productImportance?.value,
        });
    } else {
      setProductImportance(undefined);
    }

    let eduspocSelected =
      formData?.subDomainSpocResource &&
      dictionaryToArray(formData?.subDomainSpocResource).filter((x) => {
        return (
          formData &&
          formData?.eduSpocIds?.indexOf(x.key) != -1 &&
          formData?.eduSpocIds?.indexOf(x.key) != undefined
        );
      });
    setEduSpoc(eduspocSelected);

    let subDomainSelected =
      formData?.subDomainSpocResource &&
      dictionaryToArray(formData?.subDomainSpocResource).filter((x) => {
        return (
          formData &&
          formData?.subDomainSpocIds?.indexOf(x.key) != -1 &&
          formData?.subDomainSpocIds?.indexOf(x.key) != undefined
        );
      });
    setSubdomainSpoc(subDomainSelected);
  }, [
    formData?.opCoId,
    formData?.designComponentId,
    formData?.productImportanceId,
    formData?.eduSpocIds,
    formData?.subDomainSpocIds,
  ]);

  const onChangeReasonCheckbox = (property: string, e: any) => {
    let checked = e.target.checked;
    let value = parseInt(e.target.value);
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    if (property === "checkboxResourceLcmEngineeringHardwares") {
      if (checked) {
        copy.checkboxResourceLcmEngineeringHardwares = [];
        copy.checkboxResourceLcmEngineeringHardwares.push(value);

        setCheckChangeReasonCheckboxHardware(false);
      } else {
        let index = copy.checkboxResourceLcmEngineeringHardwares?.findIndex(
          (x) => x === value
        );
        if (index != -1 && index != undefined) {
          copy.checkboxResourceLcmEngineeringHardwares &&
            copy.checkboxResourceLcmEngineeringHardwares.splice(index, 1);
        }
        setCheckChangeReasonCheckboxHardware(true);
      }
    } else if (property === "checkboxResourceLcmEngineeringSoftwares") {
      if (checked) {
        copy.checkboxResourceLcmEngineeringSoftwares = [];
        copy.checkboxResourceLcmEngineeringSoftwares.push(value);

        setCheckChangeReasonCheckboxSoftware(false);
      } else {
        let index = copy.checkboxResourceLcmEngineeringSoftwares?.findIndex(
          (x) => x === value
        );
        if (index != -1 && index != undefined) {
          copy.checkboxResourceLcmEngineeringSoftwares &&
            copy.checkboxResourceLcmEngineeringSoftwares.splice(index, 1);
        }
        setCheckChangeReasonCheckboxSoftware(true);
      }
    }
    setFormData(copy);
  };

  const [ruleSW, setRuleSW] = useState<number>();
  const [ruleHW, setRuleHW] = useState<number>();
  const [autoResetSW, setautoResetSW] = useState<boolean>(false);
  const [autoResetHW, setautoResetHW] = useState<boolean>(false);

  useEffect(() => {
    if (formData?.supportedResource != undefined) {
      let copy = { ...formData } as LcmEngineeringDtoUpdate;
      if (formData?.softwareSupportedId != undefined) {
        let ruleSW = dictionaryToArrayGridDtoRule(
          formData?.supportedResource
        ).find((x) => x.key == formData?.softwareSupportedId)?.value.rule;
        setRuleSW(ruleSW);
        if (autoResetSW) {
          copy.checkboxResourceLcmEngineeringSoftwares = undefined;
          copy.softwareSupportProvider = undefined;
          copy.softwareEndOfSupportContract = undefined;
          copy.fullorPartialSupportId = undefined;
          setFormData(copy);
        } else {
          setautoResetSW(true);
        }
      }
    }
  }, [formData?.softwareSupportedId]);

  useEffect(() => {
    if (formData?.supportedResource != undefined) {
      let copy = { ...formData } as LcmEngineeringDtoUpdate;
      if (formData?.hardwareSupportedId != undefined) {
        let ruleHW = dictionaryToArrayGridDtoRule(
          formData?.supportedResource
        ).find((x) => x.key == formData?.hardwareSupportedId)?.value.rule;
        setRuleHW(ruleHW);
        if (autoResetHW) {
          copy.checkboxResourceLcmEngineeringHardwares = undefined;
          copy.sparesProvisioned = undefined;
          copy.hardwareSupportProvider = undefined;
          copy.hardwareEndOfSupportContract = undefined;
          copy.renewalInProgress = undefined;
          setFormData(copy);
        } else {
          setautoResetHW(true);
        }
      }
    }
  }, [formData?.hardwareSupportedId]);

  useEffect(() => {
    if (formData && formData.softwareEndOfSupportContract) {
      changeDate(
        "softwareEndOfSupportContract1",
        formData?.softwareEndOfSupportContract
      );
      changeDate(
        "softwareEndOfSupportContract2",
        formData?.softwareEndOfSupportContract
      );
    }

    if (formData && formData.softwareEndOfWarrantyDate)
      changeDate(
        "softwareEndOfWarrantyDate",
        formData?.softwareEndOfWarrantyDate
      );

    if (formData && formData.hardwareEndOfSupportContract) {
      changeDate(
        "hardwareEndOfSupportContract",
        formData?.hardwareEndOfSupportContract
      );
      changeDate(
        "hardwareEndOfSupportContract1",
        formData?.hardwareEndOfSupportContract
      );
    }
  }, [
    formData?.softwareEndOfSupportContract,
    formData?.softwareEndOfWarrantyDate,
    formData?.hardwareEndOfSupportContract,
  ]);

  // useEffect(()=>{
  //   getResourceKey();
  // },[selectedOpco,selectedDesignId])

  const resetSW = (copy: LcmEngineeringDtoUpdate) => {
    setRuleSW(0);
    copy.softwareSupportedId = undefined;
    copy.checkboxResourceLcmEngineeringSoftwares = undefined;
    copy.softwareSupportProvider = undefined;
    copy.softwareEndOfSupportContract = undefined;
    copy.fullorPartialSupportId = undefined;
    setFormData(copy);
  };

  const [validazioneCustom, setValidazioneCustom] = useState<{
    response: boolean;
    property?: string;
    message?: string;
  }>();

  const onChangeNodes = (property: string, e: any) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    let value = e.target.value;

    if (value < 0) {
      setValidazioneCustom({ property: property, response: false });
      copy[property] = 0;
    } else {
      setValidazioneCustom({ response: true });
      copy[property] = value;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const [showForm, setShowForm] = useState<boolean>(false);
  const [confirmPlanned, setConfirmPlanned] =
    useState<DataModalConfirm>(stateConfirm);

  const SaveOrConfirmPlanned = () => {
    const updatedFormData = {
      ...formData,
      networkElementAssociateds: networkElementAssociateds,
      opCoResource: {},
      designComponentResource: {},
      productImportanceResource: {},
      eduSpocResource: {},
      subDomainSpocResource: {},
      supportedResource: {},
      operationalContractResource: {},
      plannedActivityDto: formData?.plannedActivityDto?.map((item) => {
        const { reasonForNoPlan, commentOnProjectStatus, ...res } = item;
        return {
          ...res,
          opCoResource: {},
          designComponentResource: [],
          designComponentIsVirtualizedResource: {},
          plannedActivityResource: {},
          driverResource: {},
          planningRiskResource: {},
          benefitResource: {},
        };
      }),
    } as LcmEngineeringDtoUpdate;

    // Appending the resource key with splited life cycle value
    if (
      props.edit &&
      resourceLifeCycle !== undefined &&
      formData?.resourceKey !== null
    ) {
      updatedFormData.resourceKey =
        formData?.resourceKey?.split("_")[0] + "_" + resourceLifeCycle;
    }
    //

    const confirmPlannedState = {
      title: "Continue without saving planned activities?",
      button: "Continue",
      message:
        "Are you sure you wanto to continue? The New Planned Activity is not saved, Please click on Save Planned Activity Button, If you want to discard the planned activity changes then click on Continue Button.",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setConfirmPlanned(stateConfirm),
        confirm: () =>
          Save(
            updatedFormData,
            props.edit,
            validazioneClient,
            refresh,
            RestoreOrphanDeleted,
            orphanDeleted
          ),
      },
    };

    if (
      ruleSW === 3 &&
      !updatedFormData?.checkboxResourceLcmEngineeringSoftwares?.length
    ) {
      setCheckChangeReasonCheckboxSoftware(true);
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Operational",
          notifyType: NotifyType.warning,
        })
      );

      return;
    }

    if (
      ruleHW === 3 &&
      !updatedFormData?.checkboxResourceLcmEngineeringHardwares?.length &&
      !updatedFormData?.renewalInProgress &&
      !updatedFormData?.sparesProvisioned
    ) {
      setCheckChangeReasonCheckboxHardware(true);
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Operational",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    if (showForm) {
      let copy = { ...updatedFormData } as LcmEngineeringDtoUpdate;
      if (validazioneClient(copy).response == true) {
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
      let copy = { ...updatedFormData } as LcmEngineeringDtoUpdate;
      // if (
      //   updatedFormData &&
      //   validazioneClient(updatedFormData).response == true
      // ) {
      //   let arr = [] as NetworkElementAssociated[];
      //   copy.networkElementAssociateds?.map((x) => {
      //     if (numberIsNullOrZero(x.id)) {
      //       arr.push(x);
      //     }
      //   });
      //   copy.networkElementAssociateds = arr;
      //   setFormData(copy);
      // }
      Save(
        copy,
        props.edit,
        validazioneClient,
        refresh,
        RestoreOrphanDeleted,
        orphanDeleted
      );
    }
  };

  const changeNumberOfNodes = (
    newNodesValueProd: number,
    newNodesValueLab?: number
  ) => {
    const copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy.numberOfNodes = newNodesValueProd;
    if (newNodesValueLab !== null && newNodesValueLab !== undefined) {
      copy.numberOfNodesInLab = newNodesValueLab;
    }
    setFormData(copy);
  };

  const onChangeSupportedId = (property: string, e: any) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;

    if (property === "softwareSupportedId") {
      copy.isExtendedSupportOfferedByVendor = false;
      if (!e.target.checked) {
        copy.softwareSupportedId = undefined;
        setRuleSW(0);
      } else {
        copy.softwareSupportedId = +e.target.value;
        setRuleSW(+e.target.value);
      }
    }

    if (property === "hardwareSupportedId") {
      copy.hwIsExtendedSupportOfferedByVendor = false;
      if (!e.target.checked) {
        copy.hardwareSupportedId = undefined;
        setRuleHW(0);
      } else {
        copy.hardwareSupportedId = +e.target.value;
        setRuleHW(+e.target.value);
      }
    }
    setFormData(copy);
  };

  const onChangeRenewall = (e) => {
    const val = e.currentTarget.checked;
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy.renewalInProgress = val;
    if (val) {
      copy.sparesProvisioned = undefined;
      copy.checkboxResourceLcmEngineeringHardwares = undefined;
      setCheckChangeReasonCheckboxHardware(false);
    }
    setFormData(copy);
  };

  const onChangeNodeCountApproach = (val: boolean) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy.elementCount = val;
    setFormData(copy);
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

  const onChangeAssetStatus = (obj: any) => {
    let copy = { ...networkElementToAdd } as NetworkElementAsPlannedDtoCreate;
    if (obj && obj["key"]) {
      copy.assetsStatusId = obj["key"];
      copy.assetsStatus = obj["value"];
    } else {
      copy.assetsStatusId = undefined;
      copy.assetsStatus = undefined;
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

  const clearFormNetworkElement = () => {
    setNetworkElementToAdd(createResourceNetwork);
  };

  const checkNodeStatus = (lcmStatus, toAdd) => {
    if (lcmStatus && lcmStatus?.toLowerCase() === "in-service") {
      if (
        toAdd?.assetsStatus !== "IN-SERVICE" &&
        toAdd?.assetsStatus !== "PLANNED" &&
        toAdd?.assetsStatus !== "IN COMMISSIONING"
      ) {
        rootStore.dispatch(
          setNotification({
            message: `Please check the asset status.`,
            notifyType: NotifyType.warning,
          })
        );
        return false;
      } else return true;
    } else if (
      lcmStatus &&
      lcmStatus?.toLowerCase() !== "in-service" &&
      lcmStatus?.replaceAll("-", " ")?.toLowerCase() !==
        toAdd?.assetsStatus?.replaceAll("-", " ")?.toLowerCase()
    ) {
      rootStore.dispatch(
        setNotification({
          message: `Please check the asset status.`,
          notifyType: NotifyType.warning,
        })
      );
      return false;
    } else return true;
  };

  const addNetworkElement = async () => {
    //setAddedNetwork(true);
    let obj = networkElementToAdd;
    let toAdd: NetworkElementAssociated = {
      elementName: obj?.elementName,

      enviroment:
        networkElementToAdd?.environmentReosurce &&
        dictionaryToArray(networkElementToAdd?.environmentReosurce).find(
          (x) => x.key === obj?.environmentId
        )?.value,

      location:
        networkElementToAdd?.locationReosurce &&
        dictionaryToArrayLocationDto(networkElementToAdd?.locationReosurce)
          .map((x) => {
            return { key: x.key, value: x.value };
          })
          .find((x) => x.key === obj?.locationId)?.value.description,

      locationId: obj?.locationId,
      enviromentId: obj?.environmentId,
      assetsStatusId: obj?.assetsStatusId,
      assetsStatus:
        deploymentStatusReosurce &&
        dictionaryToArrayDeploymentStatusDto(assetsStatus)
          .map((x) => {
            return { key: x.key, value: x.value };
          })
          .find((x) => x.key == obj?.assetsStatusId)?.value
          .deploymentStatusDescription,
      isFinalAsset: false,
    };

    let copy = { ...formData } as LcmEngineeringDtoUpdate;

    const nodeIsExist = networkElementAssociateds?.filter(
      (item) => item.elementName === toAdd.elementName
    );

    if (nodeIsExist?.length) {
      rootStore.dispatch(
        setNotification({
          message: "Node is already exist !",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    const lcmDeploymentStatusString =
      copy.lcmDeploymentStatusResource !== undefined &&
      copy?.lcmDeploymentStatusId &&
      copy?.lcmDeploymentStatusResource[copy?.lcmDeploymentStatusId];

    if (!toAdd.locationId) {
      rootStore.dispatch(
        setNotification({
          message: `Please select a location.`,
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    const statusSuccess = checkNodeStatus(lcmDeploymentStatusString, toAdd);

    if (statusSuccess) {
      setNetworkElementAssociateds((value) => [...value, toAdd]);
      setNetworkElementToAdd({
        ...obj,
        locationId: undefined,
        environmentId: undefined,
        elementName: "",
        assetsStatusId: undefined,
      });
    }
  };

  const validazioneNetworkElement = () => {
    return (
      networkElementToAdd != undefined &&
      !numberIsNullOrZero(networkElementToAdd.locationId) &&
      !stringIsNullOrEmpty(networkElementToAdd.elementName) &&
      !numberIsNullOrZero(networkElementToAdd.environmentId) &&
      formData != undefined &&
      !numberIsNullOrZero(formData?.opCoId) &&
      !numberIsNullOrZero(formData?.designComponentId)
    );
  };

  const removeNetworkElement = (i: number) => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    copy.networkElementAssociateds?.splice(i, 1);
    let updatedNetworkElementAssociatedid = networkElementAssociateds;
    updatedNetworkElementAssociatedid =
      updatedNetworkElementAssociatedid.filter((item, index) => index !== i);
    setNetworkElementAssociateds(updatedNetworkElementAssociatedid);
    setFormData(copy);
  };

  const callBackGetUpdateLcm = (id: number) => {
    setKey("operational");
    props.action.Edit(id);
    setShowForm(false);
  };

  const onHideFunc = () => {
    if (isVisibleModalLookup === 10) {
      let dataCopy = [...(GridDtoAllSharedLookUp?.items ?? [])];
      OperationalContractRefillData(dataCopy);
    }
  };
  const SavePlan = (func) => {
    func();
  };
  useEffect(() => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;

    if (
      formData?.designComponentId &&
      lcmDeploymentStatusString != "In-Service" &&
      lcmDeploymentStatusString
    ) {
      IsDCHasNfxiBuildConstruction(formData?.designComponentId!).then((res) => {
        // copy.AssetsResource = res?.data;
        if (res?.data) {
          setOnSoftware(res?.data);
          copy.onSoftware = res?.data;
          setOnHardware(false);
          copy.onHardware = false;
          setBoth(false);
          setFormData(copy);
        } else {
          setOnSoftware(true);
          copy.onSoftware = true;
          setOnHardware(true);
          copy.onHardware = true;
          setBoth(true);
          setFormData(copy);
        }

        // setFormData(copy);
      });
    } else {
      setOnHardware(false);
      copy.onHardware = false;
      setOnSoftware(false);
      copy.onSoftware = false;
    }
  }, [formData?.designComponentId, lcmDeploymentStatusString]);

  // useEffect(() => {
  //   if (formData && networkElementAssociateds) {
  //     let copy = { ...formData } as LcmEngineeringDtoUpdate;
  //     let noOfNodes = 0;
  //     let noOfNodesInLab = 0;
  //     networkElementAssociateds?.map((item) =>
  //       item.enviroment === "PRODUCTION" ? noOfNodes++ : noOfNodesInLab++
  //     );
  //     setFormData({
  //       ...copy,
  //       numberOfNodes: noOfNodes,
  //       numberOfNodesInLab: noOfNodesInLab,
  //     });
  //   }
  // }, [networkElementAssociateds]);

  const validateResourceKeys = (key, event) => {
    if (key === "resourceKey") {
      onChange("resourceKey", event);
      if (/^3[0-9A-Za-z]{7}$/.test(event.target.value))
        setResourceKeyError(false);
      else setResourceKeyError(true);
    }
    if (key === "previousResourceKey") {
      onChange("previousResourceKey", event);
      if (event.target.value.length <= 255) setPrevResourceKeyError(false);
      else setPrevResourceKeyError(true);
    }
  };

  const GetDCId = (e) => {
    if (formData) {
      GetCreatedDCID(e?.key).then((data) => {
        if (data) {
          let designComponent = dictionaryToArray(data);
          let copy = { ...formData } as LcmEngineeringDtoUpdate;
          if (designComponent[0]?.key && designComponent[0]?.value) {
            copy.designComponentId = designComponent[0]?.key;
            copy.designComponentFamilyid = e?.key;
            setDesignComponent({
              key: designComponent[0]?.key,
              value: designComponent[0]?.value,
            });
            setFormData(copy);
          }
        }
        // if (id) {
        //   let copy = { ...formData } as LcmEngineeringDtoUpdate;
        //   copy.designComponentId = id;
        //   copy.designComponentFamilyid = e?.key;
        //   let designComponent =
        //     formData.designComponentResource &&
        //     dictionaryToArray(formData.designComponentResource).find(
        //       (x) => x.key === id
        //     );
        //   if (designComponent?.key && designComponent.value) {
        //     setDesignComponent({
        //       key: designComponent?.key,
        //       value: designComponent?.value,
        //     });
        //   }
        //   setFormData(copy);
        // }
      });
    }
  };

  useEffect(() => {
    if (deploymentStatusReosurce) {
      setAssetsStatus(deploymentStatusReosurce);
    }
  }, [deploymentStatusReosurce]);

  useEffect(() => {
    let copy = { ...formData } as LcmEngineeringDtoUpdate;
    if (formData !== null && formData !== undefined && !props.edit) {
      if (formData.isReleaseDetailUnKnown === true) {
        formData.designComponentId = undefined;
        formData.lcmDeploymentStatusResource &&
          dictionaryToArray(formData.lcmDeploymentStatusResource).filter(
            (x) => {
              if (x.value.toLowerCase() === "planned") {
                copy.lcmDeploymentStatusId = x.key;
                setFormData(copy);
                setlcmDeploymentStatusString("Planned");
              }
            }
          );
        copy.lcmDeploymentStatusResource = { 1: "Planned" };
        setFormData(copy);
        setShowForm(true);
        setDesignComponent(undefined);
      } else {
        setShowForm(true);
        copy.lcmDeploymentStatusResource = defaultLCMStatus;
        copy.designComponentFamilyid = undefined;
        setDesignComponentFamily(undefined);
        setFormData(copy);
      }
    }
  }, [formData?.isReleaseDetailUnKnown]);

  const updateProdNodes = (obj: NetworkElementAssociated[]) => {
    setNetworkElementAssociateds(obj);
    setFormData({
      ...formData,
      networkElementAssociateds: obj,
    });
  };

  const BuildBagItemRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetLcmEngineeringCreateResource({
        isRefillData: true,
      });
      // console.log("res", res);
      if (formData) {
        var obj: any = res?.LcmEngineeringDtoCreate?.buildBagResources
          ? res?.LcmEngineeringDtoCreate?.buildBagResources
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
  const [run, setRun] = useState(false);
  useEffect(() => {
    if (tourStarted) {
      const timeout = setTimeout(() => {
        setRun(true);
      }, 900);
      return () => clearTimeout(timeout);
    }
  }, [tourStarted]);

  useEffect(() => {
    if (!props.edit) return;

    const value = resourceArrayRefactor(buildBagRes)?.find(
      (x) => x.key === formData?.buildBagId
    )?.value;

    if (value === "Empty-1.0") {
      setIsUnlocked(true);
    }
  }, [props.edit, formData?.buildBagId, buildBagRes]);

  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />
      <ModalConfirm data={confirmPlanned} />
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
        maxWidth={React.useState<DialogProps["maxWidth"]>(false)[0]}
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
            redirect={"lcm"}
            bagId={formData?.buildBagId}
            buildBagResources={formData?.buildBagResources}
            modal={{ isModal: true, setViewBagFlag }}
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
      <Tabs
        id="uncontrolled-tab-example"
        activeKey={keyTabs ?? "operational"}
        onSelect={(x) => setKey(x || "")}
      >
        <Tab eventKey="operational" title="LCM Engineering">
          <form
            onChange={() => setChanged(true)}
            className={`mt-4 ${props.formDisabed ? "pointer-events-none" : ""}`}
          >
            <div className="row col-12 px-0 mx-0">
              <div className="col-12 p-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mb-4">Implementation Details</label>
                  <div className="row">
                    <div className="col-12">
                      <label className="labelForm voda-bold mb-2">
                        Release Details Unknown?
                      </label>
                      <label className="labelForm voda-bold mb-2">
                        <div className="switchSmall ml-2">
                          <input
                            type="checkbox"
                            onChange={(e) => {
                              onChangeReleaseDetails(
                                "isReleaseDetailUnKnown",
                                e
                              );
                            }}
                            disabled={props.edit}
                            checked={formData?.isReleaseDetailUnKnown}
                            className="mr-1"
                          />
                          <span className="sliderSmall round"></span>
                        </div>
                      </label>
                    </div>
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label
                          className="labelForm voda-bold w-100"
                          id="lcmModal_opco_tour"
                        >
                          OpCo <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-90">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.opCoResource &&
                                  dictionaryToArray(formData?.opCoResource)
                                }
                                value={
                                  formData?.opCoResource &&
                                  dictionaryToArray(
                                    formData?.opCoResource
                                  ).filter((x) => x.key === formData?.opCoId)
                                }
                                onChange={(e) => {
                                  onChangeOPco(e);
                                  onChangeSelect("opCoId", e);
                                }}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                isDisabled={
                                  props.edit
                                    ? true
                                    : formData?.networkElementAssociateds &&
                                      formData?.networkElementAssociateds
                                        ?.length > 0
                                    ? true
                                    : false
                                }
                              ></Select>
                            </div>
                            {tipologicaPermesso && !props.formDisabed && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(1)}
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
                          validation.property?.includes("opCoId") ? (
                            <label className="validation h-16">
                              *OpCo must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          Current Bag <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-90">
                              <Select
                                menuPosition={"fixed"}
                                isDisabled={props.edit && !isUnlocked}
                                id="lcmModal_currentBag_tour"
                                options={
                                  buildBagRes &&
                                  resourceArrayRefactor(buildBagRes)
                                }
                                value={
                                  buildBagRes &&
                                  resourceArrayRefactor(buildBagRes).filter(
                                    (x) => x.key === formData?.buildBagId
                                  )
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
                                // isDisabled={props.edit ? true : false}
                              ></Select>
                            </div>
                            {tipologicaPermesso &&
                            !props.formDisabed &&
                            formData?.buildBagId ? (
                              <button
                                type="button"
                                className="btn btn-link"
                                data-toggle="tooltip"
                                data-placement="top"
                                title="View Bag Component"
                                onClick={() => setViewBagFlag(true)}
                              >
                                <FaRegEye style={{ color: "gray" }} />
                              </button>
                            ) : (
                              ""
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
                              *Current Bag must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          Product Importance<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-90">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.productImportanceResource &&
                                  dictionaryToArray(
                                    formData?.productImportanceResource
                                  )
                                }
                                value={
                                  formData?.productImportanceResource &&
                                  dictionaryToArray(
                                    formData?.productImportanceResource
                                  ).filter(
                                    (x) =>
                                      x.key === formData?.productImportanceId
                                  )
                                }
                                onChange={(e) =>
                                  onChangeSelect("productImportanceId", e)
                                }
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                isDisabled={props.formDisabed}
                              ></Select>
                            </div>
                            {tipologicaPermesso && !props.formDisabed && (
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
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes(
                            "productImportanceId"
                          ) ? (
                            <label className="validation h-16">
                              *product Importance Id must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 pr-0">
                        {formData?.isReleaseDetailUnKnown ? (
                          <label className="labelForm w-100">
                            <label
                              className="labelForm voda-bold mb-0"
                              title={
                                formData?.designComponentFamilyResource &&
                                dictionaryToArray(
                                  formData?.designComponentFamilyResource
                                )
                                  .find(
                                    (x) =>
                                      x.key ===
                                      formData?.designComponentFamilyid
                                  )
                                  ?.value.replace(
                                    '<b class="text-lowercase">',
                                    ""
                                  )
                                  .replace("</b>", "")
                              }
                            >
                              Current Design Component Family
                              <span className="red">*</span>
                            </label>
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.designComponentFamilyResource &&
                                dictionaryToArray(
                                  formData?.designComponentFamilyResource
                                ).sort((a, b) =>
                                  a.value.toLowerCase().trim() <
                                  b.value.toLowerCase().trim()
                                    ? -1
                                    : 1
                                )
                              }
                              value={
                                formData?.designComponentFamilyResource &&
                                dictionaryToArray(
                                  formData?.designComponentFamilyResource
                                ).filter(
                                  (x) =>
                                    x.key === formData?.designComponentFamilyid
                                )
                              }
                              onChange={(e) => {
                                // onChangeSelect("designComponentFamilyid", e);
                                // setDesignComponentFamilyName(e?.["value"]);
                                GetDCId(e);
                              }}
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              isDisabled={props.edit}
                              formatOptionLabel={function (data) {
                                return (
                                  <span
                                    dangerouslySetInnerHTML={{
                                      __html: data.value,
                                    }}
                                  />
                                );
                              }}
                              // isDisabled={
                              //   props.edit
                              //     ? true
                              //     : formData?.networkElementAssociateds &&
                              //       formData?.networkElementAssociateds?.length >
                              //         0
                              //     ? true
                              //     : false
                              // }
                            ></Select>
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes(
                              "designComponentFamilyid"
                            ) ? (
                              <label className="validation h-16">
                                *Design Component Family must have a value
                              </label>
                            ) : null}
                          </label>
                        ) : (
                          <label
                            className="labelForm w-100"
                            id="lcmModal_currentDC_tour"
                          >
                            <label
                              className="labelForm voda-bold mb-0"
                              title={
                                formData?.designComponentResource &&
                                dictionaryToArray(
                                  formData?.designComponentResource
                                )
                                  .find(
                                    (x) => x.key === formData?.designComponentId
                                  )
                                  ?.value.replace(
                                    '<b class="text-lowercase">',
                                    ""
                                  )
                                  .replace("</b>", "")
                              }
                            >
                              Current Design Component
                              <span className="red">*</span>
                            </label>
                            <Select
                              menuPosition={"fixed"}
                              options={mergedDesignComponents?.sort((a, b) =>
                                a.value.toLowerCase().trim() <
                                b.value.toLowerCase().trim()
                                  ? -1
                                  : 1
                              )}
                              value={
                                mergedDesignComponents &&
                                mergedDesignComponents.filter(
                                  (x) => x.key === formData?.designComponentId
                                )
                              }
                              onChange={(e) => {
                                onChangeSelect("designComponentId", e);
                                setDesignComponentFamilyName(e?.["value"]);
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
                              styles={{
                                option: (provided, state) => ({
                                  ...provided,
                                  color: state.data.color || "#000000",
                                }),
                              }}
                              isDisabled={
                                props.edit
                                  ? true
                                  : formData?.networkElementAssociateds &&
                                    formData?.networkElementAssociateds
                                      ?.length > 0
                                  ? true
                                  : false
                              }
                            ></Select>
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes(
                              "designComponentId"
                            ) ? (
                              <label className="validation h-16">
                                *Design Component must have a value
                              </label>
                            ) : null}
                          </label>
                        )}
                      </div>
                      <div className="col-12 pr-0">
                        <label
                          className="labelForm voda-bold w-100"
                          id="lcmModal_deploymentStatus_tour"
                        >
                          Deployment Status<span className="red">*</span>
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.lcmDeploymentStatusResource &&
                                dictionaryToArray(
                                  formData?.lcmDeploymentStatusResource
                                )
                              }
                              value={
                                formData?.isReleaseDetailUnKnown &&
                                formData?.lcmDeploymentStatusResource
                                  ? dictionaryToArray(
                                      formData?.lcmDeploymentStatusResource
                                    ).filter(
                                      (x) =>
                                        x.value === "Planned" ||
                                        x.value === "In-Commissioning"
                                    )
                                  : formData?.lcmDeploymentStatusResource &&
                                    dictionaryToArray(
                                      formData?.lcmDeploymentStatusResource
                                    ).filter(
                                      (x) =>
                                        x.key ===
                                        formData?.lcmDeploymentStatusId
                                    )
                              }
                              onChange={(e) => {
                                onChangeSelect("lcmDeploymentStatusId", e);
                                setlcmDeploymentStatusString(e?.["value"]);
                              }}
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              isDisabled={
                                props?.edit || formData?.isReleaseDetailUnKnown
                              }
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                            ></Select>
                          </div>
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes(
                            "lcmDeploymentStatusId"
                          ) ? (
                            <label className="validation h-16">
                              *Deployment Status must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    {props?.edit && (
                      <div className="col-12 my-2">
                        <div className="row col-12 ">
                          {" "}
                          <label className="labelForm voda-bold mb-2">
                            View Resource Keys
                          </label>
                          <label className="labelForm voda-bold mb-2">
                            <div className="switchSmall ml-2">
                              <input
                                type="checkbox"
                                onChange={(e) => {
                                  setToggleResource(e.target.checked);
                                }}
                                className="mr-1"
                                checked={toggleResource}
                              />
                              <span className="sliderSmall round"></span>
                            </div>
                          </label>
                        </div>
                        <div className="row">
                          {toggleResource && (
                            <>
                              {" "}
                              <div className="col-6">
                                <div className="col-12 pl-0">
                                  <label className="labelForm voda-bold mb-0">
                                    Resource Key
                                  </label>
                                  <div className="d-flex">
                                    <label className="labelForm voda-bold mb-0 w-90 fontFamily">
                                      <div className="input-group">
                                        <input
                                          onChange={(e) =>
                                            validateResourceKeys(
                                              "resourceKey",
                                              e
                                            )
                                          }
                                          onKeyUp={(e) =>
                                            validateResourceKeys(
                                              "resourceKey",
                                              e
                                            )
                                          }
                                          type="text"
                                          className={`form-control ${
                                            !editResourceKey &&
                                            " inputBackground "
                                          } groupInput w-100`}
                                          value={
                                            formData?.resourceKey?.split(
                                              "_"
                                            )[0] || ""
                                          }
                                          disabled={
                                            !editResourceKey && props.edit
                                          }
                                        />
                                        <div className="input-group-append ">
                                          <span className="input-group-text inputBackground">{`_${resourceLifeCycle}`}</span>
                                        </div>
                                      </div>
                                    </label>
                                    {props.edit && (
                                      <button
                                        type="button"
                                        title={"Edit Resource Key"}
                                        className="btn btn-link"
                                        onClick={() =>
                                          setEditResourceKey(!editResourceKey)
                                        }
                                      >
                                        <img
                                          className="btnEdit op-55"
                                          src={require("../../img/edit.png")}
                                        />
                                      </button>
                                    )}
                                  </div>

                                  {resourceKeyError === true ||
                                  (validation &&
                                    validation.response == false &&
                                    validation.property?.includes(
                                      "resourceKey"
                                    )) ? (
                                    <label className="validationFeild h-16 mb-1 w-100">
                                      Resource Key must start with '3', followed
                                      by 7 alphanumeric characters.
                                    </label>
                                  ) : null}
                                </div>
                              </div>
                              <div className="col-6">
                                <div className="col-12 pr-0">
                                  <label className="labelForm voda-bold mb-0">
                                    Previous Resource Key
                                  </label>
                                  <div className="d-flex">
                                    <label className="labelForm voda-bold mb-0 w-100 fontFamily">
                                      <input
                                        onChange={(e) =>
                                          validateResourceKeys(
                                            "previousResourceKey",
                                            e
                                          )
                                        }
                                        onKeyUp={(e) =>
                                          validateResourceKeys(
                                            "previousResourceKey",
                                            e
                                          )
                                        }
                                        type="text"
                                        className="inputForm w-100"
                                        value={
                                          formData?.previousResourceKey || ""
                                        }
                                        disabled={
                                          !editPrevResourceKey && props.edit
                                        }
                                      />
                                    </label>
                                    {props.edit && (
                                      <button
                                        type="button"
                                        title={"Edit Resource Key"}
                                        className="btn btn-link"
                                        onClick={() =>
                                          setEditPrevResourceKey(
                                            !editPrevResourceKey
                                          )
                                        }
                                      >
                                        <img
                                          className="btnEdit op-55"
                                          src={require("../../img/edit.png")}
                                        />
                                      </button>
                                    )}
                                  </div>
                                </div>
                                <div className="col-12 pr-0">
                                  {prevResourceKeyError === true ||
                                  (validation &&
                                    validation.response == false &&
                                    validation.property?.includes(
                                      "previousResourceKey"
                                    )) ? (
                                    <label className="validationFeild h-16 mb-1 w-100">
                                      Previous Resource Key must be within 255
                                      characters.
                                    </label>
                                  ) : null}
                                </div>
                              </div>
                            </>
                          )}
                        </div>
                      </div>
                    )}
                    <div className="col-12">
                      <div className="row mx-0 mt-2 col-12 pl-0">
                        <label className="voda-bold w-100">
                          Planned Activities Status
                        </label>
                        <div className="flex flex-gab-15">
                          <label className="labelForm">
                            <input
                              type="checkbox"
                              checked={!onHardware && !onSoftware}
                              readOnly
                              className=" mr-1"
                            />
                            No Activity
                          </label>
                          <label className="labelForm">
                            <input
                              type="checkbox"
                              checked={onHardware}
                              readOnly
                              className=" mr-1"
                            />
                            Activity on HW
                          </label>
                          <label className="labelForm">
                            <input
                              type="checkbox"
                              checked={onSoftware}
                              readOnly
                              className=" mr-1"
                            />
                            Activity on SW
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>

              <div className="col-12 p-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mb-4 mt-4">Nodes Management</label>
                  <div className="row">
                    <div className="col-12">
                      <div className="form-group col-12 pl-0">
                        <label className="labelForm voda-bold w-100 d-flex align-items-center mb-3">
                          Node Count Approach <span className="red">*</span>
                        </label>
                        <label
                          className="mb-0 w-100 widthAuto mr-2"
                          onClick={() => onChangeNodeCountApproach(false)}
                        >
                          <div className="d-flex align-items-center mr-3">
                            <input
                              type="radio"
                              className={`mr-1`}
                              onChange={(e) => onChangeNodeCountApproach(false)}
                              checked={formData?.elementCount === false}
                              disabled={props.formDisabed}
                            />
                            <label className="mb-0 labelForm">
                              Manual Direct Input
                            </label>
                          </div>
                        </label>
                        <label
                          className=" mb-0 w-100 widthAuto"
                          onClick={() => onChangeNodeCountApproach(true)}
                        >
                          <div className="d-flex align-items-center  ">
                            <input
                              type="radio"
                              className="mr-1"
                              onChange={(e) => onChangeNodeCountApproach(true)}
                              checked={formData?.elementCount === true}
                              disabled={props.formDisabed}
                            />
                            <label className="mb-0 labelForm">
                              Element Count
                            </label>
                          </div>
                        </label>
                      </div>
                      {!formData?.elementCount ? (
                        <fieldset className="fieldset">
                          <div className="row">
                            <div className="col-6 pl-0 pr-5">
                              <label className="labelForm voda-bold w-100 mb-0">
                                No.of Production Nodes
                                <span className="red">*</span>
                                <input
                                  type="number"
                                  onChange={(e) =>
                                    onChangeNodes("numberOfNodes", e)
                                  }
                                  onKeyUp={(e) =>
                                    onChangeNodes("numberOfNodes", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.numberOfNodes!}
                                  disabled={props.formDisabed}
                                  //disabled
                                />
                                {validation &&
                                validation.response == false &&
                                validation.property?.includes(
                                  "numberOfNodes"
                                ) ? (
                                  <label className="validation h-16">
                                    *Number of Nodes in Service Production must
                                    have a valid number
                                  </label>
                                ) : null}
                                {validazioneCustom &&
                                validazioneCustom.response == false &&
                                validazioneCustom.property ==
                                  "numberOfNodes" ? (
                                  <label className="validation h-16">
                                    *Number of Nodes in Service Production can
                                    not be negative
                                  </label>
                                ) : null}
                              </label>
                            </div>
                            <div className="col-6 pr-0 pl-4">
                              <label className="labelForm voda-bold w-100 mb-0">
                                No.of Lab Nodes
                                <span className="red">*</span>
                                <input
                                  type="number"
                                  onChange={(e) =>
                                    onChangeNodes("numberOfNodesInLab", e)
                                  }
                                  onKeyUp={(e) =>
                                    onChangeNodes("numberOfNodesInLab", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.numberOfNodesInLab}
                                  disabled={props.formDisabed}
                                  //disabled
                                />
                                {validation &&
                                validation.response == false &&
                                validation.property?.includes(
                                  "numberOfNodesInLab"
                                ) ? (
                                  <label className="validation h-16">
                                    *Number of Nodes in Service Lab must have a
                                    valid number
                                  </label>
                                ) : null}
                                {validazioneCustom &&
                                validazioneCustom.response == false &&
                                validazioneCustom.property ==
                                  "numberOfNodesInLab" ? (
                                  <label className="validation h-16">
                                    *Number of Nodes in Service Lab can not be
                                    is negative
                                  </label>
                                ) : null}
                              </label>
                            </div>
                          </div>
                        </fieldset>
                      ) : (
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
                                  disabled={props.formDisabed}
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
                                        setEnvironmentSelect(true);
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
                                      isDisabled={props.formDisabed}
                                    ></Select>
                                  </div>
                                  {tipologicaPermesso && !props.formDisabed && (
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
                                {validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "environmentId"
                                ) ? (
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
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      isClearable
                                      isDisabled={
                                        numberIsNullOrZero(formData?.opCoId) ||
                                        props.formDisabed
                                      }
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
                                  {tipologicaPermesso && !props.formDisabed && (
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
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      options={dictionaryToArrayDeploymentStatusDto(
                                        deploymentStatusReosurce
                                      )
                                        .map((x) => {
                                          return {
                                            key: x.key,
                                            value: x.value,
                                          };
                                        })
                                        .filter((x) => {
                                          if (
                                            lcmDeploymentStatusString
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "planned" &&
                                            x.value.deploymentStatusDescription
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "planned" &&
                                            formData?.isReleaseDetailUnKnown
                                          ) {
                                            return x;
                                          } else if (
                                            lcmDeploymentStatusString
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "in service" &&
                                            (x.value.deploymentStatusDescription
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "planned" ||
                                              x.value.deploymentStatusDescription
                                                ?.toLowerCase()
                                                .replaceAll("-", " ") ===
                                                "in service" ||
                                              x.value.deploymentStatusDescription
                                                ?.toLowerCase()
                                                .replaceAll("-", " ") ===
                                                "in commissioning")
                                          ) {
                                            return x;
                                          } else if (
                                            lcmDeploymentStatusString
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "in decommissioning" &&
                                            x.value.deploymentStatusDescription
                                              ?.toLowerCase()
                                              .replaceAll("-", " ") ===
                                              "decommissioning"
                                          ) {
                                            return x;
                                          } else
                                            return (
                                              x.value.deploymentStatusDescription
                                                ?.toLowerCase()
                                                .replaceAll("-", " ") ===
                                              lcmDeploymentStatusString
                                                ?.toLowerCase()
                                                .replaceAll("-", " ")
                                            );
                                        })}
                                      value={
                                        networkElementToAdd?.assetsStatusId !=
                                        undefined
                                          ? networkElementToAdd?.assetsStatus &&
                                            dictionaryToArrayDeploymentStatusDto(
                                              networkElementToAdd?.assetsStatus
                                            )
                                              .map((x) => {
                                                return {
                                                  key: x.key,
                                                  value: x.value,
                                                };
                                              })
                                              .find(
                                                (x) =>
                                                  x.key ==
                                                  networkElementToAdd?.deploymentStatusId
                                              )
                                          : null
                                      }
                                      onChange={(e) => onChangeAssetStatus(e)}
                                      // onBlur={() => setInputValue("")}
                                      isSearchable
                                      isClearable
                                      getOptionLabel={(option) =>
                                        option.value
                                          ?.deploymentStatusDescription ?? ""
                                      }
                                      getOptionValue={(option) =>
                                        option["key"].toString()
                                      }
                                    ></Select>
                                  </div>
                                </div>
                              </label>
                            </div>
                            <div className="form-group col-12 px-0 d-flex justify-content-center">
                              <button
                                className="clearBtn"
                                onClick={() => clearFormNetworkElement()}
                                type="button"
                                disabled={props.formDisabed}
                              >
                                Clear
                              </button>
                              <button
                                className={`br-0 voda-bold btn btn-danger px-4 btnHeader ${
                                  checkAddedNetwork ? "spinner" : ""
                                }`}
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
                      )}
                    </div>
                    <div className="col-12 pr-0">
                      {formData?.elementCount ? (
                        <fieldset className="fieldset p-0">
                          <legend className="voda-bold mb-4 fz-18">
                            Network Element Details
                          </legend>
                          <div className="row">
                            <div className="w-100">
                              <table className="w-80">
                                <thead>
                                  <tr className="mt-4 head">
                                    <th className="pl-2 ptb-12">
                                      Element Name
                                    </th>
                                    <th className="pl-2 ptb-12">Environment</th>
                                    <th className="pl-2 ptb-12">Location</th>
                                    <th className="pl-2 ptb-12">Status</th>
                                    <th className="pl-2 ptb-12"></th>
                                  </tr>
                                </thead>
                                <tbody>
                                  {networkElementAssociateds?.map((item, i) => (
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
                                      <td>
                                        {numberIsNullOrZero(item.id) ? (
                                          <img
                                            onClick={() =>
                                              removeNetworkElement(i)
                                            }
                                            className="btnEdit op-55"
                                            src={require("../../img/delete.png")}
                                          />
                                        ) : null}
                                      </td>
                                    </tr>
                                  ))}
                                </tbody>
                              </table>
                            </div>
                          </div>
                        </fieldset>
                      ) : null}
                    </div>
                  </div>
                </fieldset>
              </div>

              <div className="col-12 p-0 mt-4">
                <fieldset className="fieldset p-0">
                  <legend className="text-bb mb-4">Contacts</legend>
                  <div className="row">
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          Edu Spoc<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.eduSpocResource &&
                                  dictionaryToArray(formData?.eduSpocResource)
                                }
                                value={
                                  formData?.eduSpocResource &&
                                  dictionaryToArray(
                                    formData?.eduSpocResource
                                  ).filter((x) => {
                                    return (
                                      formData &&
                                      formData?.eduSpocIds?.indexOf(x.key) !=
                                        -1 &&
                                      formData?.eduSpocIds?.indexOf(x.key) !=
                                        undefined
                                    );
                                  })
                                }
                                onChange={(e) =>
                                  OnChangeMultiSelect("eduSpocIds", e)
                                }
                                onBlur={() => setInputValue("")}
                                isMulti
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                isDisabled={props.formDisabed}
                              ></Select>
                            </div>
                            {/* {tipologicaPermesso && !props.formDisabed && (
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
                            )} */}
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("eduSpocIds") ? (
                            <label className="validation h-16">
                              *Edu-spoc must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 pr-0">
                        <label className="labelForm voda-bold   w-100">
                          Sub-Domain Spoc<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.subDomainSpocResource &&
                                  dictionaryToArray(
                                    formData?.subDomainSpocResource
                                  )
                                }
                                value={
                                  formData?.subDomainSpocResource &&
                                  dictionaryToArray(
                                    formData?.subDomainSpocResource
                                  ).filter((x) => {
                                    return (
                                      formData &&
                                      formData?.subDomainSpocIds?.indexOf(
                                        x.key
                                      ) != -1 &&
                                      formData?.subDomainSpocIds?.indexOf(
                                        x.key
                                      ) != undefined
                                    );
                                  })
                                }
                                onChange={(e) =>
                                  OnChangeMultiSelect("subDomainSpocIds", e)
                                }
                                onBlur={() => setInputValue("")}
                                isMulti={false}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                isDisabled={props.formDisabed}
                              ></Select>
                            </div>
                            {/* {tipologicaPermesso && !props.formDisabed && (
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
                            )} */}
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("subDomainSpocIds") ? (
                            <label className="validation h-16">
                              *Sub-Domain Spoc must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>

              <button
                className="further-btn mt-20"
                type="button"
                onClick={() => setShowSectionFurther(!showSectionFurther)}
                disabled={props.formDisabed}
              >
                Click for Operations Details
              </button>

              {showSectionFurther && (
                <>
                  <div className="col-12 p-0">
                    <fieldset className="fieldset p-0">
                      <div className="row mt-4">
                        <div className="col-12 pl-0">
                          <fieldset>
                            <label className="text-bb mt-4">
                              Software Contract Terms
                            </label>
                            <div className="form-group">
                              <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                                <div className="switchContainer d-fe">
                                  Is SW Under Warranty?
                                  <label className="switch">
                                    <input
                                      type="checkbox"
                                      onChange={(e) =>
                                        onChangeWarranty(e.target.checked)
                                      }
                                      className="mr-1"
                                      checked={formData?.warranty}
                                    />
                                    <span className="slider round"></span>
                                  </label>
                                </div>
                              </label>
                            </div>
                            {formData?.warranty && (
                              <div className="">
                                <label className="labelForm voda-bold   w-100">
                                  SW End Of Warranty Date
                                  <span className="red">*</span>
                                  <DatePicker
                                    selected={
                                      formData?.softwareEndOfWarrantyDate &&
                                      new Date(
                                        formData?.softwareEndOfWarrantyDate
                                      )
                                    }
                                    onChange={(newDate, e) => {
                                      e.preventDefault();
                                      onChangeDate(
                                        "softwareEndOfWarrantyDate",
                                        newDate,
                                        {
                                          operation:
                                            calculateSoftwareInWarranty,
                                        }
                                      );
                                    }}
                                    className="inputForm w-100"
                                    minDate={new Date(1980, 0, 1)}
                                    maxDate={new Date(2999, 0, 1)}
                                    dateFormat="dd/MM/yyyy"
                                    placeholderText={"NOT SPECIFIED"}
                                  />
                                  {validation &&
                                  validation.response === false &&
                                  validation.property?.includes(
                                    "softwareEndOfWarrantyDate"
                                  ) &&
                                  warranty ? (
                                    <label className="validation h-16">
                                      *SW End of Warranty Date must have a value
                                    </label>
                                  ) : null}
                                </label>
                              </div>
                            )}
                            {!formData?.warranty && (
                              <div className="w-100">
                                <div className="form-group">
                                  <label className="labelForm voda-bold mb-0">
                                    Is Software Supported?
                                  </label>
                                  <div className="flex w-100 mx-0 row mx-0 py-2">
                                    <div className="d-flex flex-row flex-gab-30">
                                      {formData?.supportedResource &&
                                        dictionaryToArrayGridDtoRule(
                                          formData.supportedResource
                                        ).map((x) => {
                                          return (
                                            <div
                                              key={x.key}
                                              className="d-flex align-items-center"
                                            >
                                              <input
                                                type="checkbox"
                                                className="mr-1"
                                                onChange={(e) =>
                                                  onChangeSupportedId(
                                                    "softwareSupportedId",
                                                    e
                                                  )
                                                }
                                                value={x.key}
                                                id={`${x.value.description}${x.value.id}SW`}
                                                checked={
                                                  x.key ===
                                                  +formData.softwareSupportedId!
                                                    ? true
                                                    : false
                                                }
                                              />
                                              <label
                                                className="mb-0 labelForm"
                                                htmlFor={`${x.value.description}${x.value.id}SW`}
                                              >
                                                {x.value.description}
                                              </label>
                                            </div>
                                          );
                                        })}
                                    </div>
                                    {tipologicaPermesso && (
                                      <button
                                        className="btn btn-link ml-3 add-bt"
                                        onClick={() =>
                                          setIsVisibleModalLookup(3)
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
                                </div>
                                {/* IF EQUIPMENT */}
                                {ruleSW == 1 ? (
                                  <div className="col-md-6 pl-0">
                                    <div className="form-group">
                                      <label className="labelForm voda-bold w-100">
                                        Software End of Support Contract
                                        <span className="red">*</span>
                                        <DatePicker
                                          selected={
                                            formData?.softwareEndOfSupportContract &&
                                            new Date(
                                              formData?.softwareEndOfSupportContract
                                            )
                                          }
                                          onChange={(newDate, e) => {
                                            e.preventDefault();
                                            onChangeDate(
                                              "softwareEndOfSupportContract",
                                              newDate
                                            );
                                          }}
                                          className="inputForm w-100"
                                          minDate={new Date(1980, 0, 1)}
                                          maxDate={new Date(2999, 0, 1)}
                                          dateFormat="dd/MM/yyyy"
                                          placeholderText={"NOT SPECIFIED"}
                                        />
                                        {validation &&
                                        validation.response === false &&
                                        validation.property?.includes(
                                          "softwareEndOfSupportContract"
                                        ) ? (
                                          <label className="validation h-16">
                                            *SW End of Support Contract must
                                            have a value
                                          </label>
                                        ) : null}
                                      </label>
                                    </div>
                                    {/* IF EOS HAS PASSED */}

                                    {formData &&
                                      formData.softwareEndOfSupportContract &&
                                      verificaData.data! <
                                        new Date(
                                          formData.softwareEndOfSupportContract
                                        ) &&
                                      (formData.softwareEndOfSupportContract ||
                                        verificaData.data === null) && (
                                        <div className="form-group">
                                          <label className="  voda-bold labelForm mb-0">
                                            Is the contract Full (Patches
                                            available) or Partial (Restore
                                            Operation only)
                                          </label>
                                          <div className="d-flex w-100 mx-0 align-items-center row mx-0 py-2">
                                            {formData?.fullorPartialSupportResource &&
                                            formData?.fullorPartialSupportResource !=
                                              null
                                              ? Object.keys(
                                                  formData?.fullorPartialSupportResource
                                                ).map((name, index) => (
                                                  <label
                                                    className="labelForm   mr-3 mb-0 d-flex align-items-center"
                                                    key={name}
                                                  >
                                                    <input
                                                      type="radio"
                                                      onChange={(e) =>
                                                        onChange(
                                                          "fullorPartialSupportId",
                                                          e
                                                        )
                                                      }
                                                      name="fullorPartialSupportId"
                                                      className=""
                                                      id={name}
                                                      value={name}
                                                      checked={
                                                        formData?.fullorPartialSupportId &&
                                                        formData?.fullorPartialSupportId.toString() ===
                                                          name
                                                          ? true
                                                          : false
                                                      }
                                                    />
                                                    <label
                                                      className="mb-0 ml-1"
                                                      htmlFor={name}
                                                    >
                                                      {formData?.fullorPartialSupportResource &&
                                                        formData
                                                          ?.fullorPartialSupportResource[
                                                          name
                                                        ]}
                                                    </label>
                                                  </label>
                                                ))
                                              : null}
                                            {tipologicaPermesso && (
                                              <button
                                                className="btn ml-3 add-bt"
                                                onClick={() =>
                                                  setIsVisibleModalLookup(5)
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

                                          {/* {validation && validation.response === false && validation.property === "softwareSupportedId" && warranty ? <label className="validation h-16">*SW End of Warranty Date must have a value</label> : null} */}
                                        </div>
                                      )}
                                  </div>
                                ) : null}
                                {/* IF THIRD PARTY */}
                                {ruleSW == 2 ? (
                                  <div className="row">
                                    <div className="col-md-6">
                                      <label className="labelForm voda-bold   w-100">
                                        Who is the Support Provider?
                                        <input
                                          type="text"
                                          onChange={(e) =>
                                            onChange(
                                              "softwareSupportProvider",
                                              e
                                            )
                                          }
                                          className="inputForm w-100"
                                          value={
                                            formData?.softwareSupportProvider
                                          }
                                        />
                                      </label>
                                    </div>
                                    <div className="col-md-6">
                                      <label className="labelForm voda-bold w-100">
                                        Software End of Support Contract
                                        <span className="red">*</span>
                                        <DatePicker
                                          selected={
                                            formData?.softwareEndOfSupportContract &&
                                            new Date(
                                              formData?.softwareEndOfSupportContract
                                            )
                                          }
                                          onChange={(newDate, e) => {
                                            e.preventDefault();
                                            onChangeDate(
                                              "softwareEndOfSupportContract",
                                              newDate
                                            );
                                          }}
                                          className="inputForm w-100"
                                          minDate={new Date(1980, 0, 1)}
                                          maxDate={new Date(2999, 0, 1)}
                                          dateFormat="dd/MM/yyyy"
                                          placeholderText={"NOT SPECIFIED"}
                                        />
                                      </label>
                                      {validation &&
                                      validation.response === false &&
                                      validation.property?.includes(
                                        "softwareEndOfSupportContract"
                                      ) ? (
                                        <label className="validation h-16">
                                          *SW End of Support Contract must have
                                          a value
                                        </label>
                                      ) : null}
                                    </div>
                                    <div className="col-md-6">
                                      <label className="  voda-bold labelForm mb-0">
                                        Is the contract Full (Patches available)
                                        or Partial (Restore Operation only)
                                      </label>
                                      <div className="d-flex w-100 mx-0 align-items-center row mx-0 py-2">
                                        {formData?.fullorPartialSupportResource &&
                                        formData?.fullorPartialSupportResource !=
                                          null
                                          ? Object.keys(
                                              formData?.fullorPartialSupportResource
                                            ).map((name, index) => (
                                              <label
                                                className="labelForm   mr-3 mb-0 d-flex align-items-center"
                                                key={name}
                                              >
                                                <input
                                                  type="radio"
                                                  onChange={(e) =>
                                                    onChange(
                                                      "fullorPartialSupportId",
                                                      e
                                                    )
                                                  }
                                                  name="fullorPartialSupportId"
                                                  className=""
                                                  id={name}
                                                  value={name}
                                                  checked={
                                                    formData?.fullorPartialSupportId &&
                                                    formData?.fullorPartialSupportId.toString() ===
                                                      name
                                                      ? true
                                                      : false
                                                  }
                                                />
                                                <label
                                                  className="mb-0 ml-1"
                                                  htmlFor={name}
                                                >
                                                  {formData?.fullorPartialSupportResource &&
                                                    formData
                                                      ?.fullorPartialSupportResource[
                                                      name
                                                    ]}
                                                </label>
                                              </label>
                                            ))
                                          : null}
                                        {tipologicaPermesso && (
                                          <button
                                            className="btn btn-link"
                                            onClick={() =>
                                              setIsVisibleModalLookup(5)
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
                                    </div>
                                  </div>
                                ) : null}

                                {/* IF NONE */}
                                {ruleSW == 3 ? (
                                  <>
                                    <div className="form-group">
                                      <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                                        <div className="switchContainer d-fe">
                                          Is the Extended Support Offered by
                                          Vendor?
                                          <label className="switch">
                                            <input
                                              type="checkbox"
                                              onChange={(e) =>
                                                onChangeCheckSliderInput(
                                                  "SW",
                                                  e
                                                )
                                              }
                                              className="mr-1"
                                              checked={
                                                formData?.isExtendedSupportOfferedByVendor
                                              }
                                            />
                                            <span className="slider round"></span>
                                          </label>
                                        </div>
                                      </label>
                                    </div>
                                    <div className="w-100">
                                      <div className="form-group">
                                        <label className="  labelForm voda-bold mb-0">
                                          Please select the reason for no
                                          support
                                          <span className="red">*</span>
                                        </label>
                                        <div className="flex w-100 mx-0 align-items-start row mx-0 py-2">
                                          <div className="d-flex flex-column w-75">
                                            {formData?.checkboxResourceResource &&
                                              dictionaryToArrayReasonCheckbox(
                                                formData.checkboxResourceResource
                                              )
                                                .filter(
                                                  (x) => x.value.isSoftware
                                                )
                                                .map((x) => {
                                                  return (
                                                    <div
                                                      key={x.key}
                                                      className="d-flex align-items-start"
                                                    >
                                                      <input
                                                        type="checkbox"
                                                        className="mr-1 mt-1"
                                                        onChange={(e) =>
                                                          onChangeReasonCheckbox(
                                                            "checkboxResourceLcmEngineeringSoftwares",
                                                            e
                                                          )
                                                        }
                                                        value={x.key}
                                                        id={`${x.value.description}${x.value.id}SWCheckbox`}
                                                        checked={formData.checkboxResourceLcmEngineeringSoftwares?.includes(
                                                          x.key
                                                        )}
                                                      />
                                                      <label
                                                        className="mb-2 labelForm"
                                                        htmlFor={`${x.value.description}${x.value.id}SWCheckbox`}
                                                      >
                                                        {x.value.description}
                                                      </label>
                                                    </div>
                                                  );
                                                })}
                                          </div>
                                          {tipologicaPermesso && (
                                            <button
                                              className="btn btn-link ml-3 add-bt"
                                              onClick={() =>
                                                setIsVisibleModalLookup(4)
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
                                        {checkChangeReasonCheckboxSoftware && (
                                          <label className="validation">
                                            *Software Reason Must Have Value !
                                          </label>
                                        )}
                                      </div>
                                    </div>
                                  </>
                                ) : null}
                              </div>
                            )}
                          </fieldset>
                        </div>
                        <div className="col-12 pl-0">
                          <fieldset className="pr-0">
                            <label className="text-bb mt-4">
                              Hardware Contract Terms
                            </label>
                            <div className="row col-12 pr-0">
                              <div className="form-group w-100">
                                <label className="labelForm voda-bold mb-0">
                                  Is Hardware Supported?
                                </label>
                                <div className="flex w-100 py-2">
                                  <div className="d-flex flex-row flex-gab-30">
                                    {formData?.supportedResource &&
                                      dictionaryToArrayGridDtoRule(
                                        formData.supportedResource
                                      ).map((x) => {
                                        return (
                                          <div
                                            key={x.key}
                                            className="d-flex align-items-center"
                                          >
                                            <input
                                              type="checkbox"
                                              className="mr-1"
                                              name="HardwareSupportedResource"
                                              onChange={(e) =>
                                                onChangeSupportedId(
                                                  "hardwareSupportedId",
                                                  e
                                                )
                                              }
                                              value={x.key}
                                              id={`${x.value.description}${x.value.id}HW`}
                                              checked={
                                                x.key ===
                                                +formData.hardwareSupportedId!
                                                  ? true
                                                  : false
                                              }
                                            />
                                            <label
                                              className="mb-0 labelForm"
                                              htmlFor={`${x.value.description}${x.value.id}HW`}
                                            >
                                              {x.value.description}
                                            </label>
                                          </div>
                                        );
                                      })}
                                  </div>
                                  {tipologicaPermesso && (
                                    <button
                                      className="btn ml-3 add-bt"
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
                              </div>
                              {/* IF EQUIPMENT */}
                              {ruleHW == 1 ? (
                                <div className="col-md-6 pl-0">
                                  <label className="labelForm voda-bold   w-100">
                                    Hardware End of Support Contract
                                    <span className="red">*</span>
                                    <DatePicker
                                      selected={
                                        formData?.hardwareEndOfSupportContract &&
                                        new Date(
                                          formData?.hardwareEndOfSupportContract
                                        )
                                      }
                                      onChange={(newDate, e) => {
                                        e.preventDefault();
                                        onChangeDate(
                                          "hardwareEndOfSupportContract",
                                          newDate
                                        );
                                      }}
                                      className="inputForm w-100"
                                      minDate={new Date(1980, 0, 1)}
                                      maxDate={new Date(2999, 0, 1)}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText={"NOT SPECIFIED"}
                                    />
                                    {validation &&
                                    validation.response === false &&
                                    validation.property?.includes(
                                      "hardwareEndOfSupportContract"
                                    ) ? (
                                      <label className="validation h-16">
                                        *HW End of Support Contract must have a
                                        value
                                      </label>
                                    ) : null}
                                  </label>

                                  {formData &&
                                    formData.hardwareEndOfSupportContract &&
                                    verificaDataHW.data! <
                                      new Date(
                                        formData.hardwareEndOfSupportContract
                                      ) &&
                                    (formData.hardwareEndOfSupportContract ||
                                      verificaDataHW.data === null) && (
                                      <div className="form-group">
                                        <label className="  voda-bold labelForm mb-0">
                                          Is the contract Full (Spares
                                          available) or Partial
                                        </label>
                                        <div className="d-flex w-100 mx-0 align-items-center row mx-0 py-2">
                                          {formData?.fullorPartialSupportHWResource &&
                                          formData?.fullorPartialSupportHWResource !==
                                            null
                                            ? Object.keys(
                                                formData?.fullorPartialSupportHWResource
                                              ).map((name, index) => (
                                                <label
                                                  className="labelForm mr-3 mb-0 d-flex align-items-center"
                                                  key={name}
                                                >
                                                  <input
                                                    type="radio"
                                                    onChange={(e) =>
                                                      onChange(
                                                        "fullorPartialSupportHWId",
                                                        e
                                                      )
                                                    }
                                                    name="fullorPartialSupportHWId"
                                                    className=""
                                                    id={name}
                                                    value={name}
                                                    checked={
                                                      formData?.fullorPartialSupportHWId &&
                                                      formData?.fullorPartialSupportHWId.toString() ===
                                                        name
                                                        ? true
                                                        : false
                                                    }
                                                  />
                                                  <label
                                                    className="mb-0 ml-1"
                                                    htmlFor="fullorPartialSupportHWId"
                                                  >
                                                    {formData?.fullorPartialSupportHWResource &&
                                                      formData
                                                        ?.fullorPartialSupportHWResource[
                                                        name
                                                      ]}
                                                  </label>
                                                </label>
                                              ))
                                            : null}
                                          {tipologicaPermesso && (
                                            <button
                                              className="btn ml-3 add-bt"
                                              onClick={() =>
                                                setIsVisibleModalLookup(5)
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

                                        {/* {validation && validation.response === false && validation.property === "softwareSupportedId" && warranty ? <label className="validation h-16">*SW End of Warranty Date must have a value</label> : null} */}
                                      </div>
                                    )}
                                </div>
                              ) : null}

                              {/* IF THIRD PARTY */}
                              {ruleHW == 2 ? (
                                <div className="w-100">
                                  <div className="row">
                                    <div className="col-md-6 pl-4">
                                      <label className="labelForm voda-bold w-100">
                                        Who is the Support Provider?
                                        <input
                                          type="text"
                                          onChange={(e) =>
                                            onChange(
                                              "hardwareSupportProvider",
                                              e
                                            )
                                          }
                                          className="inputForm w-100"
                                          value={
                                            formData?.hardwareSupportProvider
                                          }
                                        />
                                      </label>
                                    </div>
                                    <div className="col-md-6 pr-4">
                                      <label className="labelForm voda-bold w-100">
                                        Hardware End of Support Contract
                                        <span className="red">*</span>
                                        <DatePicker
                                          selected={
                                            formData?.hardwareEndOfSupportContract &&
                                            new Date(
                                              formData?.hardwareEndOfSupportContract
                                            )
                                          }
                                          onChange={(newDate, e) => {
                                            e.preventDefault();
                                            onChangeDate(
                                              "hardwareEndOfSupportContract",
                                              newDate
                                            );
                                          }}
                                          className="inputForm w-100"
                                          minDate={new Date(1980, 0, 1)}
                                          maxDate={new Date(2999, 0, 1)}
                                          dateFormat="dd/MM/yyyy"
                                          placeholderText={"NOT SPECIFIED"}
                                        />
                                        {validation &&
                                        validation.response === false &&
                                        validation.property?.includes(
                                          "hardwareEndOfSupportContract"
                                        ) ? (
                                          <label className="validation h-16">
                                            *HW End of Support Contract must
                                            have a value
                                          </label>
                                        ) : null}
                                      </label>
                                    </div>
                                    <div className="">
                                      <label className="labelForm voda-bold mb-0 w-100 widthAuto">
                                        <div className="switchContainer d-fe pl-4">
                                          Spare Parts Available?
                                          <label className="switch">
                                            <input
                                              type="checkbox"
                                              onChange={(e) =>
                                                onChangeCheckbox(
                                                  "sparesProvisioned",
                                                  e
                                                )
                                              }
                                              checked={
                                                formData?.sparesProvisioned
                                              }
                                            />
                                            <span className="slider round"></span>
                                          </label>
                                        </div>
                                      </label>
                                    </div>
                                  </div>
                                </div>
                              ) : null}

                              {/* IF NONE */}
                              {ruleHW == 3 ? (
                                <div className="w-100">
                                  <div className="form-group pl-3">
                                    <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                                      <div className="switchContainer d-fe">
                                        Is Renewal in Progress?
                                        <label className="switch">
                                          <input
                                            type="checkbox"
                                            checked={
                                              formData?.renewalInProgress
                                            }
                                            onChange={(e) =>
                                              onChangeRenewall(e)
                                            }
                                          />
                                          <span className="slider round"></span>
                                        </label>
                                      </div>
                                    </label>
                                  </div>

                                  <div className="form-group pl-3">
                                    <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                                      <div className="switchContainer d-fe">
                                        Is the Extended Support Offered by
                                        Vendor?
                                        <label className="switch">
                                          <input
                                            type="checkbox"
                                            checked={
                                              formData?.hwIsExtendedSupportOfferedByVendor
                                            }
                                            onChange={(e) =>
                                              onChangeCheckSliderInput("HW", e)
                                            }
                                          />
                                          <span className="slider round"></span>
                                        </label>
                                      </div>
                                    </label>
                                  </div>
                                  {/* IF NO RENEVAL IN PROGRESS */}

                                  {!formData?.renewalInProgress && (
                                    <div className="form-group pl-0 mb-0">
                                      <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                                        <div className="switchContainer d-fe pl-3">
                                          Spare Parts Available?
                                          <label className="switch">
                                            <input
                                              type="checkbox"
                                              onChange={(e) =>
                                                onChangeCheckbox(
                                                  "sparesProvisioned",
                                                  e
                                                )
                                              }
                                              checked={
                                                formData?.sparesProvisioned
                                              }
                                            />
                                            <span className="slider round"></span>
                                          </label>
                                        </div>
                                      </label>

                                      {!formData?.sparesProvisioned &&
                                      ruleHW == 3 ? (
                                        <div className="flex w-100 align-items-start row pl-4 py-2">
                                          <div className="d-flex flex-column">
                                            {formData?.checkboxResourceResource &&
                                              dictionaryToArrayReasonCheckbox(
                                                formData.checkboxResourceResource
                                              )
                                                .filter(
                                                  (x) => x.value.isHardware
                                                )
                                                .map((x) => {
                                                  return (
                                                    <div
                                                      key={x.key}
                                                      className="d-flex align-items-center  "
                                                    >
                                                      <input
                                                        type="checkbox"
                                                        className="mr-1"
                                                        onChange={(e) =>
                                                          onChangeReasonCheckbox(
                                                            "checkboxResourceLcmEngineeringHardwares",
                                                            e
                                                          )
                                                        }
                                                        id={`${x.value.description}${x.value.id}HWCheckbox2`}
                                                        value={x.key}
                                                        checked={formData.checkboxResourceLcmEngineeringHardwares?.includes(
                                                          x.key
                                                        )}
                                                      />
                                                      <label
                                                        className="mb-0 labelForm"
                                                        htmlFor={`${x.value.description}${x.value.id}HWCheckbox2`}
                                                      >
                                                        {x.value.description}
                                                      </label>
                                                    </div>
                                                  );
                                                })}
                                            {checkChangeReasonCheckboxHardware && (
                                              <label className="validation">
                                                *Hardware Reason Must Have Value
                                                !
                                              </label>
                                            )}
                                          </div>

                                          {tipologicaPermesso && (
                                            <button
                                              className="btn btn-link ml-3 add-bt"
                                              onClick={() =>
                                                setIsVisibleModalLookup(4)
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
                                      ) : null}
                                    </div>
                                  )}
                                </div>
                              ) : null}
                            </div>
                          </fieldset>
                        </div>

                        <div className="col-12 mt-4 pl-2">
                          <div className="row">
                            <div className="col-6">
                              <label className="labelForm voda-bold w-100">
                                Operational Contact
                                <div className="d-flex">
                                  <div className="w-100">
                                    <Select
                                      menuPosition={"fixed"}
                                      options={
                                        formData?.operationalContractResource &&
                                        dictionaryToArray(
                                          formData?.operationalContractResource
                                        )
                                      }
                                      value={
                                        formData?.operationalContractResource &&
                                        dictionaryToArray(
                                          formData?.operationalContractResource
                                        ).filter((x) => {
                                          return (
                                            formData &&
                                            formData?.operationContractsIds?.indexOf(
                                              x.key
                                            ) != -1 &&
                                            formData?.operationContractsIds?.indexOf(
                                              x.key
                                            ) != undefined
                                          );
                                        })
                                      }
                                      onChange={(e) =>
                                        OnChangeMultiSelect(
                                          "operationContractsIds",
                                          e
                                        )
                                      }
                                      onBlur={() => setInputValue("")}
                                      isSearchable
                                      isClearable
                                      isMulti
                                      getOptionLabel={(option) => option.value}
                                      getOptionValue={(option) =>
                                        option["key"].toString()
                                      }
                                    ></Select>
                                  </div>
                                  {tipologicaPermesso && (
                                    <button
                                      className="btn btn-link"
                                      onClick={() =>
                                        setIsVisibleModalLookup(10)
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
                              </label>
                            </div>
                            {/* <div className="col-6">
                              <div className="col-12">
                                <label className="labelForm voda-bold w-100">
                                  Resource Key
                                  <input
                                    onChange={(e) => onChange("resourceKey", e)}
                                    onKeyUp={(e) => onChange("resourceKey", e)}
                                    type="text"
                                    className="inputForm w-100"
                                    value={formData?.resourceKey}
                                  />
                                </label>
                              </div>
                            </div>
                            <div className="col-6">
                              <div className="col-12 pl-0">
                                <label className="labelForm voda-bold w-100">
                                  Previous Resource Key
                                  <input
                                    onChange={(e) =>
                                      onChange("previousResourceKey", e)
                                    }
                                    onKeyUp={(e) =>
                                      onChange("previousResourceKey", e)
                                    }
                                    type="text"
                                    className="inputForm w-100"
                                    value={formData?.previousResourceKey}
                                  />
                                </label>
                              </div>
                            </div> */}
                          </div>
                        </div>
                      </div>
                    </fieldset>
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
          {run && (
            <TourGuide
              start={run}
              tourSteps={getLcmModalTourSteps}
              page={"lcmModal"}
              setStartTour={(val: boolean) =>
                dispatch(val ? startGuideTour() : endGuideTour())
              }
              onTourEnd={handleEndTour}
            />
          )}
        </Tab>
        <Tab
          eventKey="plannedActivities"
          title="Planned Activities"
          className=""
          disabled={
            formData?.lcmDeploymentStatusId == undefined ||
            formData?.lcmDeploymentStatusId == null ||
            formData?.opCoId == undefined ||
            formData?.opCoId == null ||
            formData?.opCoId == 0 ||
            formData?.buildBagId == undefined ||
            formData?.buildBagId == null ||
            formData?.buildBagId == 0 ||
            (formData?.isReleaseDetailUnKnown === true &&
              (formData?.designComponentFamilyid == undefined ||
                formData?.designComponentFamilyid == null ||
                formData?.designComponentFamilyid == 0)) ||
            (formData?.isReleaseDetailUnKnown === false &&
              (formData?.designComponentId == undefined ||
                formData?.designComponentId == null ||
                formData?.designComponentId == 0))
          }
        >
          <LcmEngPlannedActivity
            designComponentFamily={
              formData?.designComponentFamilyResource &&
              dictionaryToArray(formData?.designComponentFamilyResource).filter(
                (x) => x.key === formData?.designComponentFamilyid
              )[0]?.value
            }
            designComponentFamilyId={formData?.designComponentFamilyid}
            both={both}
            lcmDeploymentStatusString={lcmDeploymentStatusString}
            designComponent={designComponent}
            idDetail={props.idDetail}
            formDisabed={props.formDisabed}
            networkElementAssociateds={networkElementAssociateds}
            pagination={query}
            onSoftware={onSoftware}
            onHardware={onHardware}
            action={{
              Delete,
              Edit,
              New,
              setChanged,
              Filter: setQuery,
              AddOnList,
              GetElementFromList,
              setShowForm,
              changeNumberOfNodes,
              updateProdNodes,
              setConfirm: setConfirmPlanned,
              editLcm: callBackGetUpdateLcm,
              closeModal: props.action.closeModal,
            }}
            showForm={showForm}
            principalId={editResource?.lcmEngineeringId ?? 0}
            formArray={formData?.plannedActivityDto}
            opco={opco}
            opcoId={formData?.opCoId}
            edit={props.edit}
            changedLcm={changed}
            productImportance={productImportance}
            numberOfNodesInProd={formData?.numberOfNodes ?? 0}
            numberOfNodesInLab={formData?.numberOfNodesInLab ?? 0}
            eduSpoc={eduSpoc}
            subdomainSpoc={subdomainSpoc}
            isFromNetworkElement={false}
            isInLcm={true}
            buildBagIds={formData?.buildBagId}
            buildBagResources={formData?.buildBagResources}
            lcmRedirect={props.lcmRedirect ?? false}
            prevPage={props.prevPage}
            isReleaseDetailUnKnown={formData?.isReleaseDetailUnKnown}
            lcmId={formData?.lcmEngineeringId}
            plannedActivityTypeForEnum={
              PlannedActivityTypeForEnum["LcmEngineering"]
            }
            reasonForNoPlan={formData?.reasonForNoPlan}
            commentOnProjectStatus={formData?.commentOnProjectStatus}
          />
        </Tab>
      </Tabs>

      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => {
            props.resetLocalState && props.resetLocalState(false);
            props.action.closeModal(changed);
          }}
          type="button"
        >
          Cancel
        </button>
        {lcmDeploymentStatusString != "In-Service" &&
        !formData?.plannedActivityDto?.length ? (
          <button
            disabled={
              keyTabs == "plannedActivities" ||
              formData?.lcmDeploymentStatusId == undefined ||
              formData?.lcmDeploymentStatusId == null ||
              formData?.opCoId == undefined ||
              formData?.opCoId == null ||
              formData?.opCoId == 0 ||
              (formData?.isReleaseDetailUnKnown === true &&
                (formData?.designComponentFamilyid == undefined ||
                  formData?.designComponentFamilyid == null ||
                  formData?.designComponentFamilyid == 0)) ||
              (formData?.isReleaseDetailUnKnown === false &&
                (formData?.designComponentId == undefined ||
                  formData?.designComponentId == null ||
                  formData?.designComponentId == 0))
            }
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={() => setKey("plannedActivities")}
            type="button"
          >
            Next
          </button>
        ) : (
          <button
            className={` voda-bold btn btn-danger px-4 btnHeader ${
              props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                ? "disabledCursor"
                : ""
            }`}
            onClick={() => SaveOrConfirmPlanned()}
            type="button"
            data-toggle="tooltip"
            data-placement="top"
            title={
              props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                ? `Saving is disabled due to redirection from LCM Export screen`
                : ""
            }
            disabled={
              props.prevPage === "generatelcmdb" && props.lcmRedirect === true
                ? true
                : props.formDisabed
            }
          >
            Save LCM Engineering
          </button>
        )}
      </div>
    </div>
  );
};

export default LcmEngineeringModal;
