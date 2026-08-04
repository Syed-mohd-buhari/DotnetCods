import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Modal, Form } from "react-bootstrap";
import {
  formatDateWithTime,
  AddMonth,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
  subtractMonths,
  lowerFirstLetter,
} from "../../Hook/Common";
import { useDispatch, useSelector } from "react-redux";
import Select from "react-select";
import {
  MajorSoftwareBuildDtoCreate,
  MajorSoftwareBuildDtoUpdate,
} from "../../Model/MajorSoftwareBuild";
import {
  CreatMajorSoftwareBuild,
  GetMajorSoftwareBuildCreateResource,
} from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCreateAction";
import { EditMajorSoftwareBuild } from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import OperatingSystemContainer from "../../Containers/Lookup/OperatingSystemContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  resourceArrayRefactor,
  resourceArrayRefactorMajorSW,
} from "../../Hook/Dictionary";
import DatePicker from "react-datepicker";
import { TipologicaGridDto } from "../../Model/LookUp/LookUpGenericModel";
import { SubNetworkBoundaryGridDto } from "../../Model/LookUp/SubnetworkBoundry";
import CriticalAssetType from "../../Containers/Lookup/CriticalAssetTypeContainer";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";
import {
  GetMajorSoftwareClonePreSubmit,
  GetSystemTypeForAddMajorSW,
} from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import {
  DataModalConfirm,
  rtnConfirmMessage,
  stateConfirm,
} from "../../Model/Common";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import TourGuide from "../../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../../Redux/Action/tourActions";
import { getMajorSwModalTourSteps } from "../../Constant/TourSteps";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: MajorSoftwareBuildDtoCreate,
      property: string
    );
    wizardBackFunction?(
      formData: MajorSoftwareBuildDtoCreate,
      property: string
    ): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  // data: MajorSoftwareBuildDtoUpdate | MajorSoftwareBuildDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
  softwareRedirect?: boolean;
  prevPage?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: MajorSoftwareBuildDtoCreate;
  onGetSwType?(type: string): void;
}

const MajorSoftwareModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("MajorSoftwareBuild");
  const [checkDeliveryMethod, setCheckDeliveryMethod] =
    useState<boolean>(false);
  const [existSoftwareVerionResource, setExistSoftwareVerionResource] =
    useState<any>();
  const [checkSWVersion, setCheckSWVersion] = useState<boolean>(true);
  const [datesGenerated, setDatesGenerated] = useState<boolean>(false);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);
  const [vulnerabilityError, setVulnerabilityError] = useState<boolean>(false);
  const [descriptionError, setDescriptionError] = useState<boolean>(false);
  const [dataConfirm, setDataConfirm] =
    useState<DataModalConfirm>(stateConfirm);
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
    onChangeMultipleSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<MajorSoftwareBuildDtoUpdate>(
    CreatMajorSoftwareBuild,
    EditMajorSoftwareBuild
  );
  const dtoEditResourceState = (state: RootState) =>
    state.majorSoftwareBuildEditReducer.MajorSoftwareBuildDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.majorSoftwareBuildCreateReducer.MajorSoftwareBuildDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const [lookupFlag, setLookupFlag] = useState<string>("");
  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const handleEndTour = () => dispatch(endGuideTour());
  const onHandelChangeAnnounced = (e: boolean) => {
    let copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    setChanged(true);

    if (e) {
      setDisabledDate(true);
      copy.eomStatus = 0;
      copy.endOfMaintenance = null;
      if (disabledEoSDate) {
        copy.endOfsupport = null;
      }
      setFormData(copy);
    } else {
      setDisabledDate(false);
      copy.eomStatus = 1;
      setFormData(copy);
    }
  };

  const onHandleCopyEoM = (e: boolean) => {
    let copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    if (e && copy) {
      setDisabledEoSDate(true);
      copy.endOfsupport = (copy?.endOfMaintenance as Date) ?? null;
      //copy.endOfMaintenance = null;
      setFormData(copy);
    } else {
      setDisabledEoSDate(false);
      //copy.endOfsupport = null;
      setFormData(copy);
    }
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit || forceEdit) {
      setforceEdit(false);
      if (editResource?.eomStatus === 0) {
        onHandelChangeAnnounced(true);
      }
      setFormData(editResource);
    } else if (!props.wizardMode) {
      setFormData(createResource);
    } else {
      const copy = { ...props.dataWizard } as MajorSoftwareBuildDtoCreate;

      copy.deliveryMethod = props.dataWizard?.deliveryMethod
        ? props.dataWizard.deliveryMethod
        : "Traditional";
      setFormData(copy);
      if (
        props.dataWizard?.lastTimeBuyNew !== undefined ||
        props.dataWizard?.operatingSystemId !== undefined ||
        props.dataWizard?.vulnerabilityStatus !== undefined
      ) {
        // setIsVisibleFurtherDetails(true);
      }

      if (props.dataWizard?.eomStatus === 0) {
        setDisabledDate(true);
      }
    }
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("MajorSoftwareBuild");
    } else {
      setKey(props.keyTab);
    }
    if (
      props.edit &&
      formData &&
      formData?.endOfsupport === formData?.endOfMaintenance
    ) {
      setDisabledEoSDate(true);
    }
  }, [formData?.endOfsupport]);

  useEffect(() => {
    if (
      formData &&
      props.wizardMode &&
      !numberIsNullOrZero(formData?.originalEquipmentManufacturerId)
    ) {
      props.action.setDataCheck &&
        props.action.setDataCheck(
          "swOemId",
          formData?.originalEquipmentManufacturerId
        );
    }
  }, [formData?.originalEquipmentManufacturerId]);

  useEffect(() => {
    if (
      formData &&
      props.wizardMode &&
      !stringIsNullOrEmpty(formData?.productName)
    ) {
      props.action.setDataCheck &&
        props.action.setDataCheck("productName", formData?.productName);

      if (props.onGetSwType) {
        props.onGetSwType(formData?.productName);
      }
    }
  }, [formData?.productName]);

  useEffect(() => {
    if (
      formData &&
      (formData.deliveryMethod === "CI/CD" ||
        formData.deliveryMethod === "One Track")
    ) {
      setCheckDeliveryMethod(true);
    } else {
      setCheckDeliveryMethod(false);
    }

    //To Auto Populated the dates
    if (formData?.deliveryMethod === "One Track") {
      if (formData.generaAvailableDate) {
        onChangeGeneralAvilabilityDate(formData.generaAvailableDate);
        return;
      }

      if (formData.endOfMaintenance) {
        onChangeEOM(new Date(formData.endOfMaintenance));
        return;
      }
    }
  }, [formData && formData.deliveryMethod]);

  useEffect(() => {
    if (formData && formData !== null) {
      rtnDeliveryMethodAndDates();
    }
  }, [
    checkDeliveryMethod,
    formData?.originalEquipmentManufacturerId,
    formData?.generaAvailableDate,
  ]);

  const { tipologicaPermesso } = useAuth();

  const validazioneClient = (copy: MajorSoftwareBuildDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.originalEquipmentManufacturerId == null ||
      copy?.originalEquipmentManufacturerId === undefined ||
      copy?.originalEquipmentManufacturerId === 0
    ) {
      addInvalidProperty("originalEquipmentManufacturerId");
    }

    if (
      (copy?.endOfMaintenance == null ||
        copy?.endOfMaintenance === undefined) &&
      !disabledDate
    ) {
      addInvalidProperty("endOfMaintenance");
    }
    if (
      (copy?.endOfsupport == null || copy?.endOfsupport === undefined) &&
      !disabledEoSDate
    ) {
      addInvalidProperty("endOfsupport");
    }
    if (
      copy?.softwareVersion == null ||
      copy?.softwareVersion === undefined ||
      copy?.softwareVersion.trim() == ""
    ) {
      addInvalidProperty("softwareVersion");
    }
    // if (
    //   formData &&
    //   formData.originalEquipmentManufacturerResource &&
    //   formData?.originalEquipmentManufacturerResource[
    //     formData?.originalEquipmentManufacturerId
    //   ] !== "VMWare"
    // ) {
    //   if (
    //     copy?.tcpSoftwareCompatibilityIdList == null ||
    //     copy?.tcpSoftwareCompatibilityIdList === undefined ||
    //     copy?.tcpSoftwareCompatibilityIdList.length === 0
    //   ) {
    //     addInvalidProperty("tcpSoftwareCompatibilityIdList");
    //   }
    //   if (
    //     copy?.tciSoftwareCompatibilityIdList == null ||
    //     copy?.tciSoftwareCompatibilityIdList === undefined ||
    //     copy?.tciSoftwareCompatibilityIdList.length === 0
    //   ) {
    //     addInvalidProperty("tciSoftwareCompatibilityIdList");
    //   }
    // }
    if (
      copy?.productNameId == null ||
      copy?.productNameId === undefined ||
      copy.productNameId === 0
    ) {
      addInvalidProperty("productNameId");
    }

    if (
      formData?.vulnerabilityStatus?.length !== 0 &&
      formData?.vulnerabilityStatus !== null &&
      (vulnerabilityError === true ||
        copy?.vulnerabilityStatus == null ||
        copy?.vulnerabilityStatus === undefined)
    ) {
      addInvalidProperty("vulnerabilityStatus");
    }
    if (
      formData?.description?.length !== 0 &&
      formData?.description !== null &&
      (descriptionError === true ||
        copy?.description == null ||
        copy?.description === undefined)
    ) {
      addInvalidProperty("description");
    }
    if (
      !props.edit &&
      !props.wizardMode &&
      checkSWVersion &&
      (copy?.existSystemTypeId === null ||
        copy?.existSystemTypeId === undefined)
    ) {
      addInvalidProperty("existSystemTypeId");
    }
    if (
      copy?.designContactIds === null ||
      copy?.designContactIds === undefined ||
      copy?.designContactIds?.length === 0
    ) {
      addInvalidProperty("designContactIds");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const changeSwApplicationName = (property: string, e: any) => {
    let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;

    if (e && e["value"]) {
      copy.productName = e["value"];
      copy.productNameId = e["key"];
      copy.isPlatform = e["isSelected"];
    } else {
      copy.productName = "";
      copy.productNameId = undefined;
      copy.isPlatform = false;
    }
    setFormData(copy);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const changeExistSoftwareVersion = (property: string, e: any) => {
    let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;

    if (e && e["value"]) {
      // copy.existSystemType = e["value"];
      copy.existSystemTypeId = e["key"];
    } else {
      // copy.existSystemType = "";
      copy.existSystemTypeId = undefined;
    }
    setFormData(copy);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] =
    useState<boolean>(false);

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            lookUpFlag={lookupFlag}
          ></OriginalEquipmentManufacturer>
        );
      case 2:
        return; // return <VulnerabilityStatusContainer returnObject={VulnerabilityStatusRefillData} modal={{ isModal: true, setIsVisibleModalLookup }}></VulnerabilityStatusContainer>;
      case 3:
        return (
          <OperatingSystemContainer
            returnObject={OperatingSystemRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OperatingSystemContainer>
        );
      case 4:
        return (
          <CriticalAssetType
            returnObject={CriticalAssetTypeRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup,
            }}
          />
        );
      case 5:
        return (
          <NetworkFunction
            returnObject={NetworkFunctionRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup,
            }}
          />
        );
      default:
        return;
    }
  };

  const onChangeEOM = (date: Date) => {
    let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;

    if (date !== null) {
      copy["eomStatus"] = 2;
      copy[lowerFirstLetter("endOfMaintenance")] = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()}`;
      //copy[lowerFirstLetter("endOfMaintenance")] = date as Date;
    } else {
      copy["eomStatus"] = 1;
    }
    if (copy.deliveryMethod === "One Track") {
      let generaAvailableDate = subtractMonths(date, 18);
      copy[lowerFirstLetter("generaAvailableDate")] = generaAvailableDate;
      let endOfsupport = AddMonth(date, 12);
      copy["endOfsupport"] = endOfsupport as Date;
    } else {
      if (disabledEoSDate) {
        copy.endOfsupport = copy.endOfMaintenance;
      }
    }
    setFormData(copy);
  };

  const onChangeGeneralAvilabilityDate = (date: Date) => {
    if (typeof date === "string") {
      date = new Date(date);
    }

    let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;

    if (!date) {
      copy[lowerFirstLetter("generaAvailableDate")] = undefined;
    } else {
      copy[lowerFirstLetter("generaAvailableDate")] = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()}`;
      if (copy.deliveryMethod === "One Track") {
        let endOfMaintenance = AddMonth(new Date(copy.generaAvailableDate), 18);
        //let endOfsupport = AddMonth(new Date(copy.generaAvailableDate), 30);
        copy[lowerFirstLetter("endOfMaintenance")] = endOfMaintenance;
        //copy[lowerFirstLetter("endOfsupport")] = endOfsupport;
      }
    }
    setFormData(copy);
  };

  const OriginalEquipmentManufacturerRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      if (lookupFlag === "equipment" && formData) {
        var obj: { [key: string]: string } =
          res?.originalEquipmentManufacturerResource
            ? res?.originalEquipmentManufacturerResource
            : value.reduce(
                (acc, item) => ({ ...acc, [item.id]: item.description }),
                {}
              ) ?? [];
        setFormData({
          ...formData,
          originalEquipmentManufacturerResource: obj,
        });
      }
      if (lookupFlag === "software" && formData) {
        // Declare the variable `obj` with the correct type: an array of objects
        let obj: { key: number; value: string; isSelected: boolean }[] = [];

        // If `res.productNamesResource` exists, use it directly
        if (res?.productNamesResource) {
          obj = res.productNamesResource.map((item) => ({
            key: item.key, // Assuming `key` is a number
            value: item.value, // Assuming `value` is the description/text
            isSelected: item.isSelected || false, // Default to `false` if `isSelected` is missing
          }));
        } else if (value && Array.isArray(value)) {
          // If `res.productNamesResource` is not available, fall back to using `value`
          obj = value.map((item) => ({
            key: item.id, // Assuming `id` is a number
            value: item.description, // Assuming `description` is the text you want to show
            isSelected: item.isPlatform || false, // Default to `false` as `isSelected` is not available in `value`
          }));
        }

        // Now set the state with the correctly structured `obj`
        setFormData({
          ...formData,
          productNamesResource: obj, // Now `obj` is correctly structured as an array
        });
      }
    } catch (error) {
      console.error("Error in OriginalEquipmentManufacturerRefillData:", error);
    }
  };

  useEffect(() => {
    if (
      formData?.productNameId != null &&
      Array.isArray(formData.productNamesResource)
    ) {
      const selectedItem = formData.productNamesResource.find(
        (p) => p.key === formData.productNameId
      );
      if (selectedItem && formData.isPlatform !== selectedItem.isSelected) {
        setFormData((prev) =>
          prev ? { ...prev, isPlatform: selectedItem.isSelected } : prev
        );
      }
    }
  }, [formData?.productNameId, formData?.productNamesResource]);

  const NetworkFunctionRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: {
        [key: string]: string;
      } = res?.networkFunctionsResource
        ? res?.networkFunctionsResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, networkFunctionsResource: obj });
    } catch (error) {
      console.error("Error in NetworkFunctionRefillData:", error);
    }
  };

  const OperatingSystemRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.operatingSystemResource
        ? res?.operatingSystemResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, operatingSystemResource: obj });
    } catch (error) {
      console.error("Error in OperatingSystemRefillData:", error);
    }
  };

  const CriticalAssetTypeRefillData = async (
    value: TipologicaGridDto[] | undefined
  ) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.criticalAssetTypeResource
        ? res?.criticalAssetTypeResource
        : value ?? [];
      formData &&
        setFormData({
          ...formData,
          criticalAssetTypeResource: obj.reduce((acc, item) => {
            if (item.id !== null && item.id !== undefined) {
              return {
                ...acc,
                [item.id]: item.description,
              } as TipologicaGridDto;
            } else {
              return {} as TipologicaGridDto;
            }
          }, {}),
        });
    } catch (error) {
      console.error("Error in CriticalAssetTypeRefillData:", error);
    }
  };

  const onChangeCheckDeliveryMethod = (e: any) => {
    let checked = e.target.checked;
    setCheckDeliveryMethod(checked);
    if (!checked) {
      let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
      copy.deliveryMethod = "Traditional";
      setDatesGenerated(false);
      setDisabledEoSDate(false);
      setFormData(copy);
    }
  };

  useEffect(() => {
    if (
      formData &&
      formData?.originalEquipmentManufacturerId !== undefined &&
      formData?.originalEquipmentManufacturerId !== null &&
      formData?.productNameId !== undefined &&
      formData?.productNameId !== null &&
      checkSWVersion &&
      !props.edit &&
      !props.wizardMode
    ) {
      let payload = {
        originalEquipmentManufacturerId:
          formData?.originalEquipmentManufacturerId,
        productNameId: formData?.productNameId,
        softwareVersion: 0,
      };
      callCheckSWVersion(payload);
    }
  }, [
    formData?.originalEquipmentManufacturerId,
    formData?.productNameId,
    checkSWVersion,
  ]);

  const callCheckSWVersion = async (body: any) => {
    const response: any = await GetSystemTypeForAddMajorSW(body);
    setExistSoftwareVerionResource(response?.data);
  };

  const RestoreOrphanDeleted = async (
    id: number | undefined,
    orphanDeletedValue?: boolean
  ) => {
    setOrphanDeleted(orphanDeletedValue ?? true);
    setforceEdit(true);
    if (props.action.Edit) await props.action.Edit(id);
    if (orphanDeletedValue === false) {
      // setDisableForm(true);
      setChanged(false);
    }
  };

  const rtnDeliveryMethodAndDates = () => {
    if (formData && formData !== null) {
      let deliveryString =
        formData?.originalEquipmentManufacturerResource &&
        dictionaryToArray(formData?.originalEquipmentManufacturerResource).find(
          (x) => x.key === formData?.originalEquipmentManufacturerId
        )?.value;
      if (
        formData?.originalEquipmentManufacturerId !== null &&
        deliveryString?.toLowerCase().includes("ericsson") &&
        checkDeliveryMethod
      ) {
        let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
        copy.deliveryMethod = "One Track";
        setDatesGenerated(true);
        if (copy && copy.generaAvailableDate) {
          copy.eomStatus = 2;
        }
        setFormData(copy);
      } else if (checkDeliveryMethod === true) {
        let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
        copy.deliveryMethod = "Traditional";
        setDatesGenerated(false);
        setFormData(copy);
      }
    }
  };

  const validateWizard = () => {
    let copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "majorSoftwareBuildDto"
      );
  };
  // console.log(formData,"karem major hard ware build ")

  useEffect(() => {
    if (
      formData &&
      formData?.vulnerabilityStatus &&
      formData?.vulnerabilityStatus !== undefined
    ) {
      if (
        formData?.vulnerabilityStatus.length !== 0 &&
        formData?.vulnerabilityStatus.length > 2000
      )
        setVulnerabilityError(true);
      else setVulnerabilityError(false);
    }
  }, [formData?.vulnerabilityStatus]);

  useEffect(() => {
    if (
      formData &&
      formData?.description &&
      formData?.description !== undefined
    ) {
      if (
        formData?.description.length !== 0 &&
        formData?.description.length > 2000
      )
        setDescriptionError(true);
      else setDescriptionError(false);
    }
  }, [formData?.description]);

  useEffect(() => {
    if (formData && formData.isPlatform) {
      let copy = { ...formData } as MajorSoftwareBuildDtoCreate;
      copy.tcpSoftwareCompatibilityIdList = [];
      copy.tciSoftwareCompatibilityIdList = [];
      setFormData(copy);
    }
  }, [formData?.isPlatform]);

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    if (e !== null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = null;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const setConfirmSubmit = async () => {
    let copy = { ...formData } as MajorSoftwareBuildDtoCreate;

    if (validazioneClient(copy).response == true) {
      if (copy && copy.existSystemTypeId) {
        await GetMajorSoftwareClonePreSubmit(
          copy?.existSystemTypeId,
          true
        ).then((x) => {
          if (x && x != undefined) {
            const SubmitConfirm = {
              title: "Confirm",
              button: "Confirm",
              message: rtnConfirmMessage(x?.info ?? ""),
              item: "",
              isOpen: true,
              actions: {
                cancel: () => setDataConfirm(stateConfirm),
                confirm: () => {
                  Save(
                    formData,
                    props.edit,
                    validazioneClient,
                    refresh,
                    RestoreOrphanDeleted,
                    orphanDeleted
                  );
                },
              },
            } as DataModalConfirm;
            setDataConfirm(SubmitConfirm);
          }
        });
      }
    }
  };

  return (
    <div className="col-12">
      <ModalConfirm data={dataConfirm} />
      <ModalConfirm data={confirmForm} />
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
          maxWidth={lookupFlag == "software" ? "lg" : "md"}
          scroll="body"
          fullWidth={true}
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            {/* <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
              <IconButton
                aria-label="close"
                onClick={() => {
                  setIsVisibleModalLookup(0);
                }}
              >
                <IoClose size={25} />
              </IconButton>
            </Box> */}
            {ReturnLookupContainer(isVisibleModalLookup)}
          </DialogContent>
        </Dialog>
      )}
      <form id="formSoftwareBuild" onChange={() => setChanged(true)}>
        <div className="row">
          <fieldset className="fieldset">
            <label className="text-bb">Software Description</label>
            <div className="row">
              <div className="col-6 pl-0">
                <div className="col-12">
                  <label
                    className="voda-bold w-100 mt-2"
                    id="majorSwAddModal_equipManufactur_tour"
                  >
                    Equipment Manufacturer <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.originalEquipmentManufacturerResource
                            )
                          }
                          value={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.originalEquipmentManufacturerResource
                            ).filter(
                              (x) =>
                                x.key ==
                                formData?.originalEquipmentManufacturerId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("originalEquipmentManufacturerId", e)
                          }
                          isSearchable
                          isClearable
                          isDisabled={props.edit ? true : false}
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => {
                            sessionStorage.setItem(
                              "lookUpType",
                              "/api/OriginalEquipmentManufacturer"
                            );
                            setLookupFlag("equipment");
                            setIsVisibleModalLookup(1);
                          }}
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
                      "originalEquipmentManufacturerId"
                    ) ? (
                      <label className="validation">
                        *Oem must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              {formData?.originalEquipmentManufacturerId === 13 && (
                <div className="col-6 pr-0">
                  <div className="col-12 mb-3 mt-4">
                    <label className="labelForm voda-bold text-uppercase mb-0 w-100 widthAuto">
                      <div className="switchContainer d-flex flex-row align-items-center">
                        <label className="switch mr-2">
                          <input
                            type="checkbox"
                            checked={checkDeliveryMethod}
                            onChange={(e) => onChangeCheckDeliveryMethod(e)}
                          />
                          <span className="slider round"></span>
                        </label>
                        Is This One Track?
                      </div>
                    </label>
                  </div>
                  <div className="form-group col-12 d-none">
                    <label className="labelForm voda-bold text-uppercase w-100">
                      Delivery Method
                      <input
                        type="text"
                        readOnly
                        onChange={(e) => onChange("deliveryMethod", e)}
                        onKeyUp={(e) => onChange("deliveryMethod", e)}
                        className="inputForm w-100"
                        value={
                          checkDeliveryMethod === false
                            ? "Traditional"
                            : formData?.deliveryMethod
                        }
                      />
                    </label>
                  </div>
                </div>
              )}

              <div
                className={`col-6 ${
                  formData?.originalEquipmentManufacturerId !== 13
                    ? "pr-0"
                    : "pl-0 pr-3"
                }`}
              >
                <div className="col-12">
                  <label
                    className=" voda-bold w-100 mt-2"
                    id="majorSwAddModal_productName_tour"
                  >
                    Product Name
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.productNamesResource &&
                            resourceArrayRefactorMajorSW(
                              formData?.productNamesResource
                            )
                          }
                          value={
                            formData?.productNamesResource &&
                            resourceArrayRefactorMajorSW(
                              formData?.productNamesResource
                            ).filter((x) => x.key == formData?.productNameId)
                          }
                          onChange={(e) => {
                            changeSwApplicationName("productNameId", e);
                          }}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>

                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => {
                            sessionStorage.setItem(
                              "lookUpType",
                              "/api/ProductName"
                            );
                            setLookupFlag("software");
                            setIsVisibleModalLookup(1);
                          }}
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
                    validation.property?.includes("productNameId") ? (
                      <label className="validation">
                        *Software Product must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>

              <div
                className={`col-6 ${
                  formData?.originalEquipmentManufacturerId !== 13
                    ? "pl-0"
                    : "pl-3"
                }`}
              >
                <div className="col-12">
                  <label
                    className="voda-bold w-100 mt-2"
                    id="majorSwAddModal_swVersion_tour"
                  >
                    Software Version
                    <span className="red">*</span>
                    <input
                      type="text"
                      readOnly={props.edit}
                      onChange={(e) => onChange("softwareVersion", e)}
                      onKeyUp={(e) => onChange("softwareVersion", e)}
                      className="inputForm w-100"
                      value={formData?.softwareVersion}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("softwareVersion") ? (
                      <label className="validation">
                        *Software Version must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              {formData &&
              formData?.originalEquipmentManufacturerId &&
              formData?.productNameId &&
              !props.edit &&
              !props.wizardMode ? (
                <>
                  <div
                    className={`col-6 ${
                      formData?.originalEquipmentManufacturerId === 13
                        ? "pr-0"
                        : "mr-0"
                    }`}
                  >
                    <div
                      className={`col-12 mb-3 mt-4 ${
                        formData?.originalEquipmentManufacturerId === 13 &&
                        "px-0"
                      }`}
                    >
                      <label className="labelForm voda-bold text-uppercase mb-0 w-100 widthAuto">
                        <div className="switchContainer d-flex flex-row align-items-center">
                          <label className="switch mr-2">
                            <input
                              type="checkbox"
                              checked={checkSWVersion}
                              onChange={(e) =>
                                setCheckSWVersion(e.target.checked)
                              }
                            />
                            <span className="slider round"></span>
                          </label>
                          Is there a SW reference?
                        </div>
                      </label>
                    </div>
                  </div>
                  {checkSWVersion && (
                    <div
                      className={`col-6 ${
                        formData?.originalEquipmentManufacturerId !== 13
                          ? "pl-0"
                          : "pl-3 pr-3"
                      }`}
                    >
                      <div className="col-12">
                        <label className="voda-bold w-100 mt-2 mb-0">
                          Existing Software Version
                          <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={existSoftwareVerionResource}
                                value={existSoftwareVerionResource?.filter(
                                  (x) => x.key == formData?.existSystemTypeId
                                )}
                                onChange={(e) => {
                                  changeExistSoftwareVersion(
                                    "existSystemTypeId",
                                    e
                                  );
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
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "existSystemTypeId"
                              ) ? (
                                <label className="validation">
                                  *Exist Software Version must have a value
                                </label>
                              ) : null}
                            </div>
                          </div>
                        </label>
                      </div>
                    </div>
                  )}
                </>
              ) : null}

              <div className={`col-6`}>
                <div
                  className={`col-12 ${
                    formData?.originalEquipmentManufacturerId === 13 &&
                    (checkSWVersion || formData.productNameId === null)
                      ? "px-0"
                      : "pr-0"
                  }`}
                >
                  <label
                    className="voda-bold w-100 mt-2 mb-0"
                    id="majorSwAddModal_designContact_tour"
                  >
                    Design Contact
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.designContacts &&
                            dictionaryToArray(formData.designContacts)
                          }
                          value={
                            formData?.designContacts &&
                            dictionaryToArray(formData?.designContacts).filter(
                              (x) => formData?.designContactIds?.includes(x.key)
                            )
                          }
                          onChange={(e) => {
                            OnChangeMultiSelect("designContactIds", e);
                          }}
                          isMulti
                          onBlur={() => setInputValue("")}
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
                        validation.property?.includes("designContactIds") ? (
                          <label className="validation">
                            *Design Contact must have a value
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                </div>
              </div>
              <div className={`col-6 pr-0`}>
                <div className="col-12 mb-3 mt-4">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100 widthAuto">
                    <div className="switchContainer d-flex flex-row align-items-center">
                      <label className="switch mr-2">
                        <input
                          type="checkbox"
                          checked={
                            formData?.productNamesResource?.find(
                              (p) => p.key === formData.productNameId
                            )?.isSelected ?? false
                          }
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
              {/* {props.wizardMode && (
                <div className="col-6 pl-3">
                  <div className="col-12 pl-0">
                    <div className="mt-2">
                      <div className="flex">
                        <span className="labelForm voda-bold sp w-100 flex-basis-95">
                          <span className="fz-16">
                            End of Maintenances
                            {!disabledDate && <span className="red">*</span>}
                          </span>
                          <DatePicker
                            selected={
                              formData?.endOfMaintenance &&
                              new Date(formData?.endOfMaintenance)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate("endOfMaintenance", newDate);
                              onChangeEOM(newDate);
                            }}
                            className="inputForm w-100"
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={
                              formData?.eomStatus === 0
                                ? "NOT ANNOUNCED"
                                : "NOT SPECIFIED"
                            }
                            disabled={disabledDate}
                          />
                          {!disabledDate && (
                            <label>
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "endOfMaintenance"
                              ) ? (
                                <label className="validation">
                                  *End Of Maintenance Date must have a value
                                </label>
                              ) : null}
                            </label>
                          )}
                        </span>

                        {formData?.deliveryMethod !== "One Track" ? (
                          <Form.Check
                            type="checkbox"
                            className="radio labelForm voda w-100 mt-35 flex-basis-43"
                            name="userLogin"
                            value="1"
                            label="Not Announced"
                            checked={formData?.eomStatus === 0 ? true : false}
                            onChange={(e: any) =>
                              onHandelChangeAnnounced(e.target.checked)
                            }
                          />
                        ) : (
                          ""
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              )} */}
            </div>
          </fieldset>
          <Container show={true}>
            <fieldset className="fieldset mt-4">
              <legend className="text-bb">
                Lifecycle Management Constraints
              </legend>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <div className="mt-2">
                      <div className="flex">
                        <span
                          className="labelForm voda-bold sp w-100 flex-basis-60"
                          id="majorSwAddModal_endOfMain_tour"
                        >
                          <span className="fz-16">
                            End of Maintenance
                            {<span className="red">*</span>}
                          </span>
                          <DatePicker
                            selected={
                              formData?.endOfMaintenance &&
                              new Date(formData?.endOfMaintenance)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate("endOfMaintenance", newDate);
                              onChangeEOM(newDate);
                            }}
                            className="inputForm w-100"
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={
                              formData?.eomStatus === 0
                                ? "NOT ANNOUNCED"
                                : "NOT SPECIFIED"
                            }
                            disabled={disabledDate}
                          />
                          {!disabledDate && (
                            <label>
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "endOfMaintenance"
                              ) ? (
                                <label className="validation">
                                  *End Of Maintenance Date must have a value
                                </label>
                              ) : null}
                            </label>
                          )}
                        </span>

                        {formData?.deliveryMethod !== "One Track" ? (
                          <Form.Check
                            type="checkbox"
                            className="radio labelForm voda w-100 mt-35 flex-basis-40"
                            name="userLogin"
                            value="1"
                            label="Not Announced"
                            checked={formData?.eomStatus === 0 ? true : false}
                            onChange={(e: any) =>
                              onHandelChangeAnnounced(e.target.checked)
                            }
                          />
                        ) : (
                          ""
                        )}
                      </div>
                    </div>
                  </div>
                  <div className="col-12">
                    <div className="mt-2">
                      <div className="flex">
                        <span
                          className="labelForm voda-bold sp w-100 flex-basis-60"
                          id="majorSwAddModal_endOfSupport_tour"
                        >
                          <span className="fz-16">
                            End of Support
                            {<span className="red">*</span>}
                          </span>
                          <DatePicker
                            selected={
                              formData?.endOfsupport &&
                              new Date(formData?.endOfsupport)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate("endOfsupport", newDate);
                            }}
                            // className={
                            //   datesGenerated
                            //     ? " disabledBackground inputForm w-100"
                            //     : "inputForm w-100 "
                            // }
                            className="inputForm w-100 "
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={
                              formData?.eomStatus === 0 && disabledEoSDate
                                ? "NOT ANNOUNCED"
                                : "NOT SPECIFIED"
                            }
                            disabled={disabledEoSDate}
                          />
                          {!disabledEoSDate && (
                            <label>
                              {validation &&
                              validation.response == false &&
                              validation.property?.includes("endOfsupport") ? (
                                <label className="validation">
                                  *End Of Support Date must have a value
                                </label>
                              ) : null}
                            </label>
                          )}
                        </span>
                        {formData?.deliveryMethod !== "One Track" ? (
                          <Form.Check
                            type="checkbox"
                            className="radio labelForm voda w-100 mt-35 flex-basis-40"
                            name="userLogin"
                            value="1"
                            label="Same as End Of Maintenance"
                            checked={disabledEoSDate}
                            onChange={(e: any) =>
                              onHandleCopyEoM(e.target.checked)
                            }
                          />
                        ) : null}
                      </div>
                    </div>
                  </div>

                  {/* <div className="col-12">
                    <label className="labelForm voda-bold w-100">
                      End of Support
                      <DatePicker
                        selected={
                          formData?.endOfsupport &&
                          new Date(formData?.endOfsupport)
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeDate("endOfsupport", newDate);
                        }}
                        // className={
                        //   datesGenerated
                        //     ? " disabledBackground inputForm w-100"
                        //     : "inputForm w-100 "
                        // }
                        className="inputForm w-100 "
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                      />
                    </label>
                      </div> */}
                </div>

                <div className="col-6 pr-0">
                  <div className="col-12">
                    <label
                      className="voda-bold w-100 mt-2"
                      id="majorSwAddModal_avialDate_tour"
                    >
                      General Availability Date
                      <DatePicker
                        selected={
                          formData?.generaAvailableDate &&
                          new Date(formData?.generaAvailableDate)
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          // onChangeDate("generaAvailableDate", newDate);
                          onChangeGeneralAvilabilityDate(newDate);
                        }}
                        className="inputForm w-100"
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                      />
                    </label>
                  </div>
                  <div className="col-12">
                    <label
                      className="labelForm voda-bold w-100 mt-2"
                      id="majorSwAddModal_lastTimeBuy_tour"
                    >
                      Last Time Buy
                      <DatePicker
                        selected={
                          formData?.lastTimeBuyNew &&
                          new Date(formData?.lastTimeBuyNew)
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeDate("lastTimeBuyNew", newDate);
                        }}
                        // className={
                        //   !props.edit
                        //     ? "disabledBackground inputForm w-100"
                        //     : "inputForm w-100"
                        // }
                        className="inputForm w-100"
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                      />
                    </label>
                  </div>
                </div>
              </div>
            </fieldset>
          </Container>
          {formData && !formData.isPlatform && (
            <fieldset className="fieldset">
              <legend className="text-bb">VmWare Compatibile Attribute</legend>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <label className="voda-bold w-100 mt-2">
                      TCI
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.tciBundleVersion &&
                              dictionaryToArray(formData?.tciBundleVersion)
                            }
                            value={
                              formData?.tciBundleVersion &&
                              dictionaryToArray(
                                formData?.tciBundleVersion
                              ).filter((el) =>
                                formData.tciSoftwareCompatibilityIdList?.includes(
                                  el.key
                                )
                              )
                            }
                            onChange={(e) =>
                              OnChangeMultiSelect(
                                "tciSoftwareCompatibilityIdList",
                                e
                              )
                            }
                            // onKeyUp={(e) => OnChangeMultiSelect("tciSoftwareCompatibilityIdList", e)}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isMulti
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </div>
                      </div>
                      {/* {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "tciSoftwareCompatibilityIdList"
                        ) ? (
                          <label className="validation h-16">
                            *TCP must have a value
                          </label>
                        ) : null} */}
                    </label>
                  </div>
                </div>
                <div className="col-6 pr-0">
                  <div className="col-12">
                    <label className="voda-bold w-100 mt-2">
                      TCP
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.tcpBundleVersion &&
                              dictionaryToArray(formData?.tcpBundleVersion)
                            }
                            value={
                              formData?.tcpBundleVersion &&
                              dictionaryToArray(
                                formData?.tcpBundleVersion
                              ).filter((el) =>
                                formData.tcpSoftwareCompatibilityIdList?.includes(
                                  el.key
                                )
                              )
                            }
                            onChange={(e) =>
                              OnChangeMultiSelect(
                                "tcpSoftwareCompatibilityIdList",
                                e
                              )
                            }
                            // onKeyUp={(e) => OnChangeMultiSelect("tcpSoftwareCompatibilityIdList", e)}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isMulti
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </div>
                      </div>
                      {/* {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "tcpSoftwareCompatibilityIdList"
                        ) ? (
                          <label className="validation h-16">
                            *TCP must have a value
                          </label>
                        ) : null} */}
                    </label>
                  </div>
                </div>
              </div>
            </fieldset>
          )}
          {/* CLASSIC */}

          <div className="col-12 mt-4">
            <button
              className="mb-20 mt-20 further-btn"
              type="button"
              onClick={() => setShowFurtherDetails(!showFurtherDetails)}
            >
              Click for further details
            </button>
          </div>

          <div className="col-12">
            <Container show={true}>
              <fieldset className="fieldset">
                {/* <legend className="red">Further Details:</legend> */}
                <div className="row">
                  {(showFurtherDetails || isVisibleFurtherDetails) && (
                    <>
                      <div className="col-6 mb-2 mt-2 pl-0">
                        <div className="form-group">
                          <label className="labelForm voda-bold mb-0 w-100">
                            Functional Entity
                            <div className="d-flex">
                              <Select
                                menuPosition={"fixed"}
                                className="w-100"
                                options={
                                  formData?.networkFunctionsResource &&
                                  dictionaryToArray(
                                    formData?.networkFunctionsResource
                                  )
                                }
                                value={
                                  formData?.networkFunctionsResource &&
                                  dictionaryToArray(
                                    formData?.networkFunctionsResource
                                  ).filter((el) =>
                                    formData?.networkFunctionsIds?.includes(
                                      el.key
                                    )
                                  )
                                }
                                onChange={(e) =>
                                  onChangeMultipleSelect(
                                    "networkFunctionsIds",
                                    e
                                  )
                                }
                                onBlur={() => setInputValue("")}
                                isMulti
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option.key.toString()
                                }
                              />
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
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "networkFunctionsIds"
                            ) ? (
                              <label className="validation">
                                *System Function must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                      <div className="col-12 col-md-6 pl-rem pr-0">
                        <label className="labelForm voda-bold mt-2 mb-0 w-100 pl-0">
                          Software Family
                          <div className="d-flex">
                            <Select
                              menuPosition={"fixed"}
                              className="w-100"
                              options={
                                formData?.criticalAssetTypeResource &&
                                dictionaryToArray(
                                  formData?.criticalAssetTypeResource
                                )
                              }
                              value={
                                formData?.criticalAssetTypeResource &&
                                dictionaryToArray(
                                  formData?.criticalAssetTypeResource
                                ).filter(
                                  (el) =>
                                    el.key === formData?.criticalAssetTypeId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("criticalAssetTypeId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) => option.key.toString()}
                            />
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
                      <div className="col-6 pl-0">
                        <div className="col-12 pl-0">
                          <label className="labelForm voda-bold mb-0">
                            Vulnerability Status
                          </label>
                          <div className="d-flex">
                            <label className="labelForm voda-bold w-100 mb-0">
                              <input
                                type="text"
                                onChange={(e) =>
                                  onChange("vulnerabilityStatus", e)
                                }
                                onKeyUp={(e) =>
                                  onChange("vulnerabilityStatus", e)
                                }
                                className="inputForm w-100"
                                value={formData?.vulnerabilityStatus}
                              />
                            </label>
                            {vulnerabilityError === true ||
                            (validation &&
                              validation.response === false &&
                              validation.property?.includes(
                                "vulnerabilityStatus"
                              )) ? (
                              <label className="validation">
                                *Maximum 2000 characters are only allowed.
                              </label>
                            ) : null}
                          </div>
                        </div>
                      </div>
                      <div className="col-6 pr-0 pl-rem">
                        <div className="row">
                          <div className="col-12">
                            <label className="labelForm voda-bold w-100">
                              Operating System
                              <div className="d-flex">
                                <div className="w-100">
                                  <Select
                                    menuPosition={"fixed"}
                                    options={
                                      formData?.operatingSystemResource &&
                                      dictionaryToArray(
                                        formData?.operatingSystemResource
                                      )
                                    }
                                    value={
                                      formData?.operatingSystemResource &&
                                      dictionaryToArray(
                                        formData?.operatingSystemResource
                                      ).filter(
                                        (x) =>
                                          x.key == formData?.operatingSystemId
                                      )
                                    }
                                    onChange={(e) =>
                                      onChangeSelect("operatingSystemId", e)
                                    }
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
                      <div className="col-12 pl-0">
                        <div className="form-group">
                          <label className="labelForm voda-bold mb-0 w-100">
                            Description
                          </label>
                          <div className="d-flex">
                            <label className="labelForm voda-bold mb-0 w-100">
                              <input
                                type="text"
                                onChange={(e) => onChange("description", e)}
                                onKeyUp={(e) => onChange("description", e)}
                                className="inputForm w-100"
                                value={formData?.description!}
                              />
                            </label>
                            {descriptionError === true ||
                            (validation &&
                              validation.response === false &&
                              validation.property?.includes("description")) ? (
                              <label className="validation">
                                *Maximum 2000 characters are only allowed.
                              </label>
                            ) : null}
                          </div>
                        </div>
                      </div>
                      {props.edit ? (
                        <div className="col-6 pl-0">
                          <label className="labelForm voda-bold w-100">
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
                      {props.edit ? (
                        <div className="col-6">
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
                    </>
                  )}
                </div>
              </fieldset>
            </Container>
          </div>

          {/* WIZARDMODE */}
          {/* <Container show={props.wizardMode}>
            <fieldset className="fieldset">
              <legend className="red">
                <button
                  type="button"
                  style={{ marginRight: 0, marginLeft: 0 }}
                  className="clearBtn mb-3 fz-16 Fbutton mt-4"
                  onClick={() =>
                    setIsVisibleFurtherDetails(!isVisibleFurtherDetails)
                  }
                >
                  Add Further Details?
                </button>
              </legend>
              <Container show={isVisibleFurtherDetails || showFurtherDetails}>
                <div className="row mt-35">
                  <div className="col-6 pl-0">
                    <div className="col-12">
                      <label className="voda-bold w-100">
                        General Availability Date
                        <DatePicker
                          selected={
                            formData?.generaAvailableDate &&
                            new Date(formData?.generaAvailableDate)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            // onChangeDate("generaAvailableDate", newDate);
                            onChangeGeneralAvilabilityDate(newDate);
                          }}
                          className="inputForm w-100"
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </label>
                    </div>
                    <div className="col-12">
                      <label className="voda-bold w-100 mt-2">
                        Last time buy
                        <DatePicker
                          selected={
                            formData?.lastTimeBuyNew &&
                            new Date(formData?.lastTimeBuyNew)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("lastTimeBuyNew", newDate);
                          }}
                          className="inputForm w-100"
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </label>
                    </div>
                    <div className="col-12 mb-2 ">
                      <div className="form-group">
                        <label className="voda-bold mb-0 w-100 mt-2">
                          Functional Entity
                          <div className="d-flex">
                            <Select
                              className="w-100"
                              options={
                                formData?.networkFunctionsResource &&
                                dictionaryToArray(
                                  formData?.networkFunctionsResource
                                )
                              }
                              value={
                                formData?.networkFunctionsResource &&
                                dictionaryToArray(
                                  formData?.networkFunctionsResource
                                ).filter((el) =>
                                  formData?.networkFunctionsIds?.includes(
                                    el.key
                                  )
                                )
                              }
                              onChange={(e) =>
                                onChangeMultipleSelect("networkFunctionsIds", e)
                              }
                              onBlur={() => setInputValue("")}
                              isMulti
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) => option.key.toString()}
                            />
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
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes(
                            "networkFunctionsIds"
                          ) ? (
                            <label className="validation">
                              *System Function must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </div>
                  <div className="col-6 pr-2">
                    <div className="col-12">
                      <label className="voda-bold w-100">
                        End of Support
                        <DatePicker
                          selected={
                            formData?.endOfsupport &&
                            new Date(formData?.endOfsupport)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("endOfsupport", newDate);
                          }}
                          onKeyUp={(e) => onChangeDate("endOfsupport", e)}
                          className="inputForm w-100 "
                          // className={
                          //   datesGenerated
                          //     ? " disabledBackground inputForm w-100"
                          //     : "inputForm w-100 "
                          // }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </label>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("endOfsupport") ? (
                        <label className="validation">
                          *End of Support must have a value
                        </label>
                      ) : null}
                    </div>
                    <div className="col-12">
                      <label className="voda-bold mt-2 w-100">
                        Operating System
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              options={
                                formData?.operatingSystemResource &&
                                dictionaryToArray(
                                  formData?.operatingSystemResource
                                )
                              }
                              value={
                                formData?.operatingSystemResource &&
                                dictionaryToArray(
                                  formData?.operatingSystemResource
                                ).filter(
                                  (x) => x.key == formData?.operatingSystemId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("operatingSystemId", e)
                              }
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
                    <div className="col-12">
                      <label className="labelForm voda-bold mt-2 mb-0 w-100 pl-0">
                        Software Family
                        <div className="d-flex">
                          <Select
                            className="w-100"
                            options={
                              formData?.criticalAssetTypeResource &&
                              dictionaryToArray(
                                formData?.criticalAssetTypeResource
                              )
                            }
                            value={
                              formData?.criticalAssetTypeResource &&
                              dictionaryToArray(
                                formData?.criticalAssetTypeResource
                              ).filter(
                                (el) => el.key === formData?.criticalAssetTypeId
                              )
                            }
                            onChange={(e) =>
                              onChangeSelect("criticalAssetTypeId", e)
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) => option.key.toString()}
                          />
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
                  <div className="col-12">
                    <label className="labelForm voda-bold mb-0 w-100">
                      Description
                    </label>
                    <div className="d-flex">
                      <label className="labelForm voda-bold w-100 mb-0">
                        <input
                          type="text"
                          onChange={(e) => onChange("description", e)}
                          onKeyUp={(e) => onChange("description", e)}
                          className="inputForm w-100"
                          value={formData?.description!}
                        />
                      </label>
                      {descriptionError === true ||
                      (validation &&
                        validation.response === false &&
                        validation.property?.includes("description")) ? (
                        <label className="validation pl-3">
                          *Maximum 2000 characters are only allowed.
                        </label>
                      ) : null}
                    </div>
                  </div>
                  <div className="col-12 my-4">
                    <label className="labelForm voda-bold mb-0">
                      Vulnerability Status
                    </label>
                    <div className="d-flex">
                      <label className="labelForm voda-bold w-100 mb-0">
                        <input
                          type="text"
                          onChange={(e) => onChange("vulnerabilityStatus", e)}
                          onKeyUp={(e) => onChange("vulnerabilityStatus", e)}
                          className="inputForm w-100"
                          value={formData?.vulnerabilityStatus}
                        />
                      </label>
                      {vulnerabilityError === true ||
                      (validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "vulnerabilityStatus"
                        )) ? (
                        <label className="validation pl-3">
                          *Maximum 2000 characters are only allowed.
                        </label>
                      ) : null}
                    </div>
                  </div>
                </div>
              </Container>
            </fieldset>
          </Container> */}
        </div>
        {/* <div className="row">
         
        </div> */}
      </form>
      {/* </Tab>
            </Tabs> */}
      <Container show={!props.wizardMode}>
        <div className="col-12 justify-content-end d-flex footerModal">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() =>
              props.action.closeModal && props.action.closeModal(changed)
            }
            type="button"
          >
            Cancel
          </button>
          <button
            className={` voda-bold btn btn-danger px-4 btnHeader ${
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? "disabledCursor"
                : ""
            }`}
            onClick={() => {
              if (formData?.existSystemTypeId && checkSWVersion) {
                setConfirmSubmit();
              } else {
                Save(
                  formData,
                  props.edit,
                  validazioneClient,
                  refresh,
                  RestoreOrphanDeleted,
                  orphanDeleted
                );
              }
            }}
            type="button"
            data-toggle="tooltip"
            data-placement="top"
            title={
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? `Saving is disabled due to redirection from LCM Export screen`
                : ""
            }
            disabled={
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? true
                : false
            }
            id="majorSwAddModal_submit_tour"
          >
            Submit
          </button>
        </div>
      </Container>
      <Container show={props.wizardMode}>
        <div className="col-12 d-flex justify-content-between py-4 mt-4">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={() =>
              props.action.setConfirmExitWizard &&
              props.action.setConfirmExitWizard()
            }
          >
            Exit
          </button>
          <div className="">
            <button
              disabled={!(props.wizardStep && props.wizardStep > 1)}
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              type="button"
              onClick={() =>
                props.action.wizardBackFunction &&
                formData &&
                props.action.wizardBackFunction(
                  formData,
                  "majorSoftwareBuildDto"
                )
              }
            >
              Back
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => validateWizard()}
            >
              Continue
            </button>
          </div>
        </div>
      </Container>
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          tourSteps={getMajorSwModalTourSteps}
          page={"majorSwAddModal"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default MajorSoftwareModal;
