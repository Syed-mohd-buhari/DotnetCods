import React, { useEffect, useRef, useState } from "react";
import { Box, Divider, Typography } from "@mui/material";
import { FiCheck, FiChevronDown, FiChevronUp, FiX } from "react-icons/fi";
import { createPortal } from "react-dom";
import Select from "react-select";
import { useDispatch, useSelector } from "react-redux";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import {
  MajorSoftwareBuildDtoCreate,
  MajorSoftwareBuildDtoUpdate,
} from "../../../Model/MajorSoftwareBuild";
import {
  CreatMajorSoftwareBuild,
  GetMajorSoftwareBuildCreateResource,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCreateAction";
import {
  EditMajorSoftwareBuild,
  GetMajorSoftwareBuildEditResource,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildEditAction";
import { RootState } from "../../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { useAuth } from "../../../Hook/useAuth";
import ModalConfirm from "../../../Components/ModalConfirm";
import { CommonValidation } from "../../../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  resourceArrayRefactorMajorSW,
} from "../../../Hook/Dictionary";
import {
  AddMonth,
  formatDateWithTime,
  lowerFirstLetter,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
  subtractMonths,
} from "../../../Hook/Common";
import {
  DataModalConfirm,
  rtnConfirmMessage,
  stateConfirm,
} from "../../../Model/Common";
import {
  GetMajorSoftwareClonePreSubmit,
  GetSystemTypeForAddMajorSW,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import OriginalEquipmentManufacturer from "../../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import OperatingSystemContainer from "../../../Containers/Lookup/OperatingSystemContainer";
import CriticalAssetType from "../../../Containers/Lookup/CriticalAssetTypeContainer";
import NetworkFunction from "../../../Containers/Lookup/NetworkFunctionContainer";
import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import {
  CloseButton,
  DateField,
  DeleteModalCard,
  FieldBox,
  HeaderRow,
  INPUT_STYLE,
  LABEL_STYLE,
  ModalCard,
  ModalFooter,
  ModalHeader,
  Overlay,
  REQ,
  RedBtn,
  ScrollBody,
  SectionBox,
  SELECT_STYLES,
  T,
  ToggleSwitch,
  ValidationLabel,
} from "../../utils/styled";

interface ProductSoftwareModalProps {
  open: boolean;
  onClose: () => void;
  mode?: "new" | "edit";
  editId?: number;
  wizardMode?: boolean;
  wizardStep?: number;
  dataWizard?: MajorSoftwareBuildDtoCreate;
  softwareRedirect?: boolean;
  prevPage?: string;
  keyTab?: string;
  onGetSwType?: (type: string) => void;
  onCreateSuccess?: (id: number | string | undefined) => void;
  onOpenHardwareModal?: () => void;
  action?: {
    closeModal?: (changed?: boolean) => any;
    refresh?: () => any;
    Edit?: (id: number | undefined) => any;
    validateFormWizard?: (
      response: boolean,
      formData: MajorSoftwareBuildDtoCreate,
      property: string
    ) => void;
    wizardBackFunction?: (
      formData: MajorSoftwareBuildDtoCreate,
      property: string
    ) => any;
    setConfirmExitWizard?: () => any;
    setDataCheck?: (prop: string, val: number | string) => any;
  };
}

const ProductSoftwareModal: React.FC<ProductSoftwareModalProps> = ({
  open,
  onClose,
  mode = "new",
  editId,
  wizardMode = false,
  wizardStep,
  dataWizard,
  softwareRedirect,
  prevPage,
  keyTab,
  onGetSwType,
  onCreateSuccess,
  onOpenHardwareModal,
  action,
}) => {
  const [additionalExpanded, setAdditionalExpanded] = useState(false);
  const [checkDeliveryMethod, setCheckDeliveryMethod] = useState(false);
  const [existSoftwareVersionResource, setExistSoftwareVersionResource] =
    useState<any[]>([]);
  const [checkSWVersion, setCheckSWVersion] = useState(true);
  const [datesGenerated, setDatesGenerated] = useState(false);
  const [vulnerabilityError, setVulnerabilityError] = useState(false);
  const [descriptionError, setDescriptionError] = useState(false);
  const [disabledDate, setDisabledDate] = useState(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState(false);
  const [orphanDeleted, setOrphanDeleted] = useState(false);
  const [forceEdit, setForceEdit] = useState(false);
  const [lookupFlag, setLookupFlag] = useState("");
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState(0);
  const [isCreateReady, setIsCreateReady] = useState(false);
  const [isEditLoaded, setIsEditLoaded] = useState(false);
  const [dataConfirm, setDataConfirm] =
    useState<DataModalConfirm>(stateConfirm);
  const [showSuccessScreen, setShowSuccessScreen] = useState(false);
  const [createdRecordId, setCreatedRecordId] = useState<
    number | string | undefined
  >(undefined);
  const overlayRef = useRef<HTMLDivElement>(null);
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const createResource = useSelector(
    (state: RootState) =>
      state.majorSoftwareBuildCreateReducer.MajorSoftwareBuildDtoCreate
  );
  const editResource = useSelector(
    (state: RootState) =>
      state.majorSoftwareBuildEditReducer.MajorSoftwareBuildDtoEdit
  );
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
    confirmForm,
  } = useFormTableCrud<MajorSoftwareBuildDtoUpdate>(
    CreatMajorSoftwareBuild,
    EditMajorSoftwareBuild
  );
  const { tipologicaPermesso } = useAuth();

  useEffect(() => {
    if (open && mode === "new" && !isCreateReady) {
      setIsCreateReady(true);
      GetMajorSoftwareBuildCreateResource();
    }
    if (open && mode === "edit" && editId && !isEditLoaded) {
      setIsEditLoaded(true);
      GetMajorSoftwareBuildEditResource(editId);
    }
  }, [open, mode, editId]);

  useEffect(() => {
    if (mode === "edit" || forceEdit) {
      setForceEdit(false);
      if (editResource?.eomStatus === 0) onHandelChangeAnnounced(true);
      setFormData(editResource);
    } else if (!wizardMode) {
      setFormData(createResource);
    } else {
      const copy = {
        ...dataWizard,
        deliveryMethod: dataWizard?.deliveryMethod ?? "Traditional",
      } as MajorSoftwareBuildDtoCreate;
      setFormData(copy);
      if (dataWizard?.eomStatus === 0) setDisabledDate(true);
    }
  }, [createResource, editResource, mode, dataWizard]);

  useEffect(() => {
    if (!open) {
      setIsCreateReady(false);
      setIsEditLoaded(false);
      setShowSuccessScreen(false);
      setCreatedRecordId(undefined);
      return;
    }
    const handler = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        if (showSuccessScreen) {
          handleCloseConfirmation();
        } else {
          attemptClose();
        }
      }
    };
    document.addEventListener("keydown", handler);
    return () => document.removeEventListener("keydown", handler);
  }, [open, onClose, showSuccessScreen, changed]);

  useEffect(() => {
    if (
      mode === "edit" &&
      formData &&
      formData?.endOfsupport === formData?.endOfMaintenance
    ) {
      setDisabledEoSDate(true);
    }
  }, [formData?.endOfsupport]);
  useEffect(() => {
    if (
      mode !== "new" ||
      wizardMode ||
      !userInfo?.account.name ||
      !formData?.designContacts ||
      (formData?.designContactIds && formData.designContactIds.length > 0)
    ) {
      return;
    }
    const contacts = dictionaryToArray(formData.designContacts);
    const match = contacts.find(
      (c: any) =>
        typeof c.value === "string" &&
        c.value.trim().toLowerCase() ===
          userInfo?.account.name.trim().toLowerCase()
    );
    if (match) {
      setFormData((prev) =>
        prev ? { ...prev, designContactIds: [match.key] } : prev
      );
    }
  }, [formData?.designContacts, mode, wizardMode, userInfo]);

  useEffect(() => {
    if (
      formData &&
      wizardMode &&
      !numberIsNullOrZero(formData?.originalEquipmentManufacturerId)
    ) {
      action?.setDataCheck?.(
        "swOemId",
        formData.originalEquipmentManufacturerId
      );
    }
  }, [formData?.originalEquipmentManufacturerId]);

  useEffect(() => {
    if (formData && wizardMode && !stringIsNullOrEmpty(formData?.productName)) {
      action?.setDataCheck?.("productName", formData.productName);
      onGetSwType?.(formData.productName);
    }
  }, [formData?.productName]);

  useEffect(() => {
    if (
      formData?.productNameId &&
      Array.isArray(formData.productNamesResource)
    ) {
      const selected = formData.productNamesResource.find(
        (p) => p.key === formData.productNameId
      );
      if (selected && formData.isPlatform !== selected.isSelected) {
        setFormData((prev) =>
          prev ? { ...prev, isPlatform: selected.isSelected } : prev
        );
      }
    }
  }, [formData?.productNameId, formData?.productNamesResource]);

  useEffect(() => {
    if (formData?.isPlatform) {
      setFormData((prev) =>
        prev
          ? ({
              ...prev,
              tcpSoftwareCompatibilityIdList: [],
              tciSoftwareCompatibilityIdList: [],
            } as MajorSoftwareBuildDtoCreate)
          : prev
      );
    }
  }, [formData?.isPlatform]);

  useEffect(() => {
    if (
      formData?.deliveryMethod === "CI/CD" ||
      formData?.deliveryMethod === "One Track"
    ) {
      setCheckDeliveryMethod(true);
    } else {
      setCheckDeliveryMethod(false);
    }
    if (formData?.deliveryMethod === "One Track") {
      if (formData.generaAvailableDate) {
        onChangeGeneralAvailabilityDate(new Date(formData.generaAvailableDate));
        return;
      }
      if (formData.endOfMaintenance) {
        onChangeEOM(new Date(formData.endOfMaintenance));
        return;
      }
    }
  }, [formData?.deliveryMethod]);

  useEffect(() => {
    if (formData) rtnDeliveryMethodAndDates();
  }, [
    checkDeliveryMethod,
    formData?.originalEquipmentManufacturerId,
    formData?.generaAvailableDate,
  ]);

  useEffect(() => {
    if (formData?.vulnerabilityStatus) {
      setVulnerabilityError(formData.vulnerabilityStatus.length > 2000);
    }
  }, [formData?.vulnerabilityStatus]);

  useEffect(() => {
    if (formData?.description) {
      setDescriptionError(formData.description.length > 2000);
    }
  }, [formData?.description]);

  useEffect(() => {
    if (
      formData?.originalEquipmentManufacturerId &&
      formData?.productNameId &&
      checkSWVersion &&
      !mode.includes("edit") &&
      !wizardMode
    ) {
      callCheckSWVersion({
        originalEquipmentManufacturerId:
          formData.originalEquipmentManufacturerId,
        productNameId: formData.productNameId,
        softwareVersion: 0,
      });
    }
  }, [
    formData?.originalEquipmentManufacturerId,
    formData?.productNameId,
    checkSWVersion,
  ]);

  const onHandelChangeAnnounced = (e: boolean) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    setChanged(true);
    if (e) {
      setDisabledDate(true);
      copy.eomStatus = 0;
      copy.endOfMaintenance = null;
      if (disabledEoSDate) copy.endOfsupport = null;
      setFormData(copy);
    } else {
      setDisabledDate(false);
      copy.eomStatus = 1;
      setFormData(copy);
    }
  };

  const onHandleCopyEoM = (e: boolean) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    if (e) {
      setDisabledEoSDate(true);
      copy.endOfsupport = (copy?.endOfMaintenance as Date) ?? null;
      setFormData(copy);
    } else {
      setDisabledEoSDate(false);
      setFormData(copy);
    }
  };

  const onChangeEOM = (date: Date | null) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    if (date !== null) {
      copy["eomStatus"] = 2;
      copy[lowerFirstLetter("endOfMaintenance")] = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()}`;
    } else {
      copy["eomStatus"] = 1;
    }
    if (copy.deliveryMethod === "One Track" && date) {
      copy[lowerFirstLetter("generaAvailableDate")] = subtractMonths(date, 18);
      copy["endOfsupport"] = AddMonth(date, 12) as Date;
    } else {
      if (disabledEoSDate) copy.endOfsupport = copy.endOfMaintenance;
    }
    setFormData(copy);
  };

  const onChangeGeneralAvailabilityDate = (date: Date | null) => {
    let d = typeof date === "string" ? new Date(date) : date;
    const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    if (!d) {
      copy[lowerFirstLetter("generaAvailableDate")] = undefined;
    } else {
      copy[lowerFirstLetter("generaAvailableDate")] = `${d.getFullYear()}/${
        d.getMonth() + 1
      }/${d.getDate()}`;
      if (copy.deliveryMethod === "One Track") {
        copy[lowerFirstLetter("endOfMaintenance")] = AddMonth(
          new Date(copy.generaAvailableDate),
          18
        );
      }
    }
    setFormData(copy);
  };

  const onChangeCheckDeliveryMethod = (checked: boolean) => {
    setCheckDeliveryMethod(checked);
    if (!checked) {
      const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
      copy.deliveryMethod = "Traditional";
      setDatesGenerated(false);
      setDisabledEoSDate(false);
      setFormData(copy);
    }
  };

  const rtnDeliveryMethodAndDates = () => {
    if (!formData) return;
    const deliveryString =
      formData?.originalEquipmentManufacturerResource &&
      dictionaryToArray(formData.originalEquipmentManufacturerResource).find(
        (x) => x.key === formData?.originalEquipmentManufacturerId
      )?.value;
    if (
      formData?.originalEquipmentManufacturerId !== null &&
      deliveryString?.toLowerCase().includes("ericsson") &&
      checkDeliveryMethod
    ) {
      const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
      copy.deliveryMethod = "One Track";
      setDatesGenerated(true);
      if (copy?.generaAvailableDate) copy.eomStatus = 2;
      setFormData(copy);
    } else if (checkDeliveryMethod) {
      const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
      copy.deliveryMethod = "Traditional";
      setDatesGenerated(false);
      setFormData(copy);
    }
  };

  const callCheckSWVersion = async (body: any) => {
    const response: any = await GetSystemTypeForAddMajorSW(body);
    setExistSoftwareVersionResource(response?.data ?? []);
  };

  const changeSwApplicationName = (property: string, e: any) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    if (e?.value) {
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
      const v = { ...validation, property: [...validation.property] };
      v.property.splice(v.property.indexOf(property), 1);
      setValidation(v);
    }
  };

  const changeExistSoftwareVersion = (property: string, e: any) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    copy.existSystemTypeId = e?.key ?? undefined;
    setFormData(copy);
    if (validation?.property?.includes(property)) {
      const v = { ...validation, property: [...validation.property] };
      v.property.splice(v.property.indexOf(property), 1);
      setValidation(v);
    }
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    const copy = { ...formData } as MajorSoftwareBuildDtoUpdate;
    if (e?.length > 0) {
      copy[property] = e.map((item: any) => item.key);
    } else {
      copy[property] = null;
    }
    setFormData(copy);
    if (validation?.property?.includes(property)) {
      const v = { ...validation, property: [...validation.property] };
      v.property.splice(v.property.indexOf(property), 1);
      setValidation(v);
    }
  };

  const RestoreOrphanDeleted = async (
    id: number | undefined,
    orphanDeletedValue?: boolean
  ) => {
    setOrphanDeleted(orphanDeletedValue ?? true);
    setForceEdit(true);
    if (action?.Edit) await action.Edit(id);
    if (orphanDeletedValue === false) setChanged(false);
  };

  const validazioneClient = (copy: MajorSoftwareBuildDtoUpdate) => {
    let v = { response: true, property: [] } as CommonValidation;
    const fail = (p: string) => {
      v.property?.push(p);
      v.response = false;
    };

    if (
      !copy?.originalEquipmentManufacturerId ||
      copy.originalEquipmentManufacturerId === 0
    )
      fail("originalEquipmentManufacturerId");

    if (!copy?.endOfMaintenance && !disabledDate) fail("endOfMaintenance");
    if (!copy?.endOfsupport && !disabledEoSDate) fail("endOfsupport");

    if (!copy?.softwareVersion?.trim()) fail("softwareVersion");

    if (!copy?.productNameId || copy.productNameId === 0) fail("productNameId");

    // if (
    //   formData?.vulnerabilityStatus?.length !== 0 &&
    //   formData?.vulnerabilityStatus !== null &&
    //   (vulnerabilityError || !copy?.vulnerabilityStatus)
    // )
    //   fail("vulnerabilityStatus");

    // if (
    //   formData?.description?.length !== 0 &&
    //   formData?.description !== null &&
    //   (descriptionError || !copy?.description)
    // )
    //   fail("description");

    if (
      !mode.includes("edit") &&
      !wizardMode &&
      checkSWVersion &&
      !copy?.existSystemTypeId
    )
      fail("existSystemTypeId");

    if (!copy?.designContactIds?.length) fail("designContactIds");

    setValidation(v);
    return v;
  };
  const resetFormState = () => {
    setFormData(undefined as any);
    setChanged(false);
    setValidation({ response: true, property: [] });
    setCheckDeliveryMethod(false);
    setDisabledDate(false);
    setDisabledEoSDate(false);
    setAdditionalExpanded(false);
    setOrphanDeleted(false);
  };
  const refresh = (changedVal: boolean) => {
    if (mode === "new" && !wizardMode) {
      const newId =
        (formData as any)?.majorSoftwareBuildId ?? (formData as any)?.id;
      setCreatedRecordId(newId);
      setShowSuccessScreen(true);
      action?.refresh?.();
      return;
    }
    resetFormState();
    action?.closeModal?.(changedVal);
    action?.refresh?.();
    onClose();
  };

  const setConfirmSubmit = async () => {
    const copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    if (validazioneClient(copy).response && copy?.existSystemTypeId) {
      await GetMajorSoftwareClonePreSubmit(copy.existSystemTypeId, true).then(
        (x) => {
          if (x) {
            const SubmitConfirm: DataModalConfirm = {
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
                    mode === "edit",
                    validazioneClient,
                    refresh,
                    RestoreOrphanDeleted,
                    orphanDeleted
                  );
                },
              },
            };
            setDataConfirm(SubmitConfirm);
          }
        }
      );
    }
  };

  const handleSave = () => {
    if (formData?.existSystemTypeId && checkSWVersion && mode !== "edit") {
      setConfirmSubmit();
    } else {
      Save(
        formData,
        mode === "edit",
        validazioneClient,
        refresh,
        RestoreOrphanDeleted,
        orphanDeleted
      );
    }
  };

  const validateWizard = () => {
    const copy = { ...formData } as MajorSoftwareBuildDtoCreate;
    action?.validateFormWizard?.(
      validazioneClient(copy).response,
      copy,
      "majorSoftwareBuildDto"
    );
  };
  const handleCreateAnother = () => {
    setShowSuccessScreen(false);
    setCreatedRecordId(undefined);
    setChanged(false);
    setValidation({ response: true, property: [] });
    setCheckDeliveryMethod(false);
    setDisabledDate(false);
    setDisabledEoSDate(false);
    setAdditionalExpanded(false);
    setOrphanDeleted(false);
    setIsCreateReady(true);
    GetMajorSoftwareBuildCreateResource();
  };
  const handleCreateHardware = () => {
    setShowSuccessScreen(false);
    action?.closeModal?.(true);
    onCreateSuccess?.(createdRecordId);
    onClose();
    onOpenHardwareModal?.();
  };
  const handleCloseConfirmation = () => {
    setShowSuccessScreen(false);
    action?.closeModal?.(true);
    onCreateSuccess?.(createdRecordId);
    onClose();
  };
  const attemptClose = () => {
    if (!changed) {
      onClose();
      return;
    }
    setDataConfirm({
      title: "Close without saving?",
      button: "Yes, Close",
      message:
        "You have unsaved changes. Closing now will not save them. Are you sure you want to close?",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setDataConfirm(stateConfirm),
        confirm: () => {
          setDataConfirm(stateConfirm);
          onClose();
        },
      },
    });
  };

  const OriginalEquipmentManufacturerRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      if (lookupFlag === "equipment" && formData) {
        const obj =
          res?.originalEquipmentManufacturerResource ??
          value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
        setFormData({
          ...formData,
          originalEquipmentManufacturerResource: obj,
        });
      }
      if (lookupFlag === "software" && formData) {
        const obj = res?.productNamesResource
          ? res.productNamesResource.map((i: any) => ({
              key: i.key,
              value: i.value,
              isSelected: i.isSelected ?? false,
            }))
          : value.map((i: any) => ({
              key: i.id,
              value: i.description,
              isSelected: i.isPlatform ?? false,
            }));
        setFormData({ ...formData, productNamesResource: obj });
      }
    } catch (err) {
      console.error("OriginalEquipmentManufacturerRefillData error:", err);
    }
  };

  const NetworkFunctionRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.networkFunctionsResource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData) setFormData({ ...formData, networkFunctionsResource: obj });
    } catch (err) {
      console.error("NetworkFunctionRefillData error:", err);
    }
  };

  const OperatingSystemRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.operatingSystemResource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData) setFormData({ ...formData, operatingSystemResource: obj });
    } catch (err) {
      console.error("OperatingSystemRefillData error:", err);
    }
  };

  const CriticalAssetTypeRefillData = async (
    value: TipologicaGridDto[] | undefined
  ) => {
    try {
      const res: any = await GetMajorSoftwareBuildCreateResource({
        isRefillData: true,
      });
      const raw = res?.criticalAssetTypeResource ?? value ?? [];
      const obj = raw.reduce((acc: any, item: any) => {
        if (item.id != null) return { ...acc, [item.id]: item.description };
        return acc;
      }, {});
      if (formData)
        setFormData({ ...formData, criticalAssetTypeResource: obj });
    } catch (err) {
      console.error("CriticalAssetTypeRefillData error:", err);
    }
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            lookUpFlag={lookupFlag}
          />
        );
      case 3:
        return (
          <OperatingSystemContainer
            returnObject={OperatingSystemRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 4:
        return (
          <CriticalAssetType
            returnObject={CriticalAssetTypeRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 5:
        return (
          <NetworkFunction
            returnObject={NetworkFunctionRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return null;
    }
  };

  if (!open) return null;

  const isOneTrack = formData?.deliveryMethod === "One Track";
  const isEricsson = formData?.originalEquipmentManufacturerId === 13;
  const oemName = formData?.originalEquipmentManufacturerResource
    ? dictionaryToArray(
        formData?.originalEquipmentManufacturerResource ?? []
      ).filter(
        (x: any) => x.key === formData?.originalEquipmentManufacturerId
      )?.[0]?.value
    : "";

  const productName = formData?.productNamesResource
    ? resourceArrayRefactorMajorSW(formData.productNamesResource).filter(
        (x: any) => x.key === formData?.productNameId
      )?.[0]?.value
    : "";

  const title =
    mode === "new"
      ? "New Software"
      : `Edit Software - ${oemName} ${productName} ${
          formData?.softwareVersion ?? ""
        }`;
  return createPortal(
    <>
      <ModalConfirm data={dataConfirm} />
      <ModalConfirm data={confirmForm} />

      {isVisibleModalLookup > 0 && (
        <Dialog
          open={isVisibleModalLookup > 0}
          onClose={(_, reason) => {
            if (reason === "backdropClick" || reason === "escapeKeyDown")
              return;
            setIsVisibleModalLookup(0);
          }}
          maxWidth={lookupFlag === "software" ? "lg" : "md"}
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            {ReturnLookupContainer(isVisibleModalLookup)}
          </DialogContent>
        </Dialog>
      )}

      <Overlay
        ref={overlayRef}
        onClick={(e) => {
          if (e.target !== overlayRef.current) return;
          if (showSuccessScreen) {
            handleCloseConfirmation();
          } else {
            attemptClose();
          }
        }}
      >
        {showSuccessScreen ? (
          <DeleteModalCard
            sx={{
              width: "560px",
              maxWidth: "90vw",
              padding: "48px 40px 40px",
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              textAlign: "center",
              position: "relative",
            }}
          >
            <CloseButton
              onClick={handleCloseConfirmation}
              sx={{ position: "absolute", top: "16px", right: "16px" }}
            >
              <FiX style={{ fontSize: "20px", color: "#000" }} />
            </CloseButton>

            <FiCheck
              style={{
                fontSize: "72px",
                color: T.red,
                strokeWidth: 3,
                marginBottom: "16px",
              }}
            />

            <Typography
              sx={{ fontSize: "26px", color: T.black, lineHeight: "34px" }}
            >
              Thanks for adding a
            </Typography>
            <Typography
              sx={{
                fontSize: "26px",
                fontWeight: 500,
                color: T.black,
                lineHeight: "34px",
                mb: "20px",
              }}
            >
              Software Product
            </Typography>

            <Typography
              sx={{
                fontSize: "16px",
                color: T.black,
                lineHeight: "24px",
                mb: "32px",
                maxWidth: "420px",
              }}
            >
              This has successfully been added to the product library and ready
              to use in TEMS
            </Typography>
            <Box sx={{ width: "100%", mb: "20px" }}>
              <RedBtn onClick={handleCreateAnother} sx={{ width: "100%" }}>
                Would you like to create another software product?
              </RedBtn>
            </Box>

            {/* <Typography
              onClick={handleCreateHardware}
              sx={{
                fontSize: "15px",
                color: T.black,
                textDecoration: "underline",
                cursor: "pointer",
              }}
            >
              Would you like to create a hardware product?
            </Typography> */}
          </DeleteModalCard>
        ) : (
          <ModalCard>
            <ModalHeader>
              <HeaderRow>
                <Typography
                  sx={{
                    fontWeight: 700,
                    fontSize: "18px",
                    lineHeight: "28px",
                    color: T.black,
                  }}
                >
                  {title}
                </Typography>
                <CloseButton onClick={attemptClose}>
                  <FiX style={{ fontSize: "20px", color: "#000" }} />
                </CloseButton>
              </HeaderRow>
              <Divider />
            </ModalHeader>

            <ScrollBody onChange={() => setChanged(true)}>
              <SectionBox>
                <Typography
                  sx={{
                    fontWeight: 700,
                    fontSize: "18px",
                    color: T.black,
                  }}
                >
                  Software Description
                </Typography>

                <Box
                  sx={{
                    display: "flex",
                    flexWrap: "wrap",
                    gap: "16px",
                    alignItems: "flex-start",
                  }}
                >
                  <FieldBox width={323}>
                    <label
                      style={LABEL_STYLE}
                      id="majorSwAddModal_equipManufactur_tour"
                    >
                      Equipment manufacturer {REQ}
                    </label>
                    <Box sx={{ display: "flex", gap: "4px" }}>
                      <Box sx={{ flex: 1 }}>
                        <Select
                          menuPortalTarget={document.body}
                          styles={SELECT_STYLES}
                          options={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData.originalEquipmentManufacturerResource
                            )
                          }
                          value={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData.originalEquipmentManufacturerResource
                            ).filter(
                              (x: any) =>
                                x.key ===
                                formData?.originalEquipmentManufacturerId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("originalEquipmentManufacturerId", e)
                          }
                          isSearchable
                          isClearable
                          isDisabled={mode === "edit"}
                          getOptionLabel={(o: any) => o.value}
                          getOptionValue={(o: any) => o.key.toString()}
                        />
                      </Box>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          type="button"
                          onClick={() => {
                            sessionStorage.setItem(
                              "lookUpType",
                              "/api/OriginalEquipmentManufacturer"
                            );
                            setLookupFlag("equipment");
                            setIsVisibleModalLookup(1);
                          }}
                          style={{ padding: "0 4px", flexShrink: 0 }}
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../../img/plus_icon.png")}
                            alt="plus"
                          />
                        </button>
                      )}
                    </Box>
                    {validation?.response === false &&
                      validation.property?.includes(
                        "originalEquipmentManufacturerId"
                      ) && (
                        <ValidationLabel>
                          *OEM must have a value
                        </ValidationLabel>
                      )}
                  </FieldBox>

                  {isEricsson && (
                    <FieldBox width="auto">
                      <Box sx={{ height: "36px" }} />
                      <ToggleSwitch
                        checked={checkDeliveryMethod}
                        onChange={onChangeCheckDeliveryMethod}
                        label="Is This One Track?"
                      />
                    </FieldBox>
                  )}

                  <FieldBox width={323}>
                    <label
                      style={LABEL_STYLE}
                      id="majorSwAddModal_productName_tour"
                    >
                      Product name {REQ}
                    </label>
                    <Box sx={{ display: "flex", gap: "4px" }}>
                      <Box sx={{ flex: 1 }}>
                        <Select
                          menuPortalTarget={document.body}
                          styles={SELECT_STYLES}
                          options={
                            formData?.productNamesResource &&
                            resourceArrayRefactorMajorSW(
                              formData.productNamesResource
                            )
                          }
                          value={
                            formData?.productNamesResource &&
                            resourceArrayRefactorMajorSW(
                              formData.productNamesResource
                            ).filter(
                              (x: any) => x.key === formData?.productNameId
                            )
                          }
                          onChange={(e) =>
                            changeSwApplicationName("productNameId", e)
                          }
                          isDisabled={mode === "edit"}
                          isSearchable
                          isClearable
                          getOptionLabel={(o: any) => o.value}
                          getOptionValue={(o: any) => o.key.toString()}
                        />
                      </Box>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          type="button"
                          onClick={() => {
                            sessionStorage.setItem(
                              "lookUpType",
                              "/api/ProductName"
                            );
                            setLookupFlag("software");
                            setIsVisibleModalLookup(1);
                          }}
                          style={{ padding: "0 4px", flexShrink: 0 }}
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../../img/plus_icon.png")}
                            alt="plus"
                          />
                        </button>
                      )}
                    </Box>
                    {validation?.response === false &&
                      validation.property?.includes("productNameId") && (
                        <ValidationLabel>
                          *Software Product must have a value
                        </ValidationLabel>
                      )}
                  </FieldBox>

                  <FieldBox width={250}>
                    <label
                      style={LABEL_STYLE}
                      id="majorSwAddModal_swVersion_tour"
                    >
                      Software version {REQ}
                    </label>
                    <Box
                      sx={{
                        display: "flex",
                        alignItems: "center",
                        gap: "9px",
                      }}
                    >
                      <input
                        style={{
                          ...INPUT_STYLE,
                          background:
                            mode === "edit"
                              ? "rgb(245, 245, 245)"
                              : "rgb(255, 255, 255)",
                        }}
                        readOnly={mode === "edit"}
                        value={formData?.softwareVersion ?? ""}
                        onChange={(e) => {
                          const copy = {
                            ...formData,
                          } as MajorSoftwareBuildDtoUpdate;
                          copy.softwareVersion = e.target.value;
                          setFormData(copy);
                        }}
                      />
                    </Box>
                    {validation?.response === false &&
                      validation.property?.includes("softwareVersion") && (
                        <ValidationLabel>
                          *Software Version must have a value
                        </ValidationLabel>
                      )}
                  </FieldBox>

                  {formData?.originalEquipmentManufacturerId &&
                  formData?.productNameId &&
                  mode !== "edit" &&
                  !wizardMode ? (
                    <>
                      <FieldBox width="auto">
                        <Box sx={{ height: "42px" }} />
                        <ToggleSwitch
                          checked={checkSWVersion}
                          onChange={(v) => setCheckSWVersion(v)}
                          label="Is there a SW reference?"
                        />
                      </FieldBox>

                      {checkSWVersion && (
                        <FieldBox width={323}>
                          <label style={LABEL_STYLE}>
                            Existing Software Version {REQ}
                          </label>
                          <Select
                            menuPortalTarget={document.body}
                            styles={SELECT_STYLES}
                            options={existSoftwareVersionResource}
                            value={existSoftwareVersionResource?.filter(
                              (x: any) => x.key === formData?.existSystemTypeId
                            )}
                            onChange={(e) =>
                              changeExistSoftwareVersion("existSystemTypeId", e)
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            getOptionLabel={(o: any) => o.value}
                            getOptionValue={(o: any) => o.key.toString()}
                            formatOptionLabel={(data: any) => (
                              <span
                                dangerouslySetInnerHTML={{
                                  __html: data.value,
                                }}
                              />
                            )}
                          />
                          {validation?.response === false &&
                            validation.property?.includes(
                              "existSystemTypeId"
                            ) && (
                              <ValidationLabel>
                                *Existing Software Version must have a value
                              </ValidationLabel>
                            )}
                        </FieldBox>
                      )}
                    </>
                  ) : null}

                  <FieldBox width={323}>
                    <label
                      style={LABEL_STYLE}
                      id="majorSwAddModal_designContact_tour"
                    >
                      Software Product Owner {REQ}
                    </label>
                    <Select
                      menuPortalTarget={document.body}
                      styles={SELECT_STYLES}
                      options={
                        formData?.designContacts &&
                        dictionaryToArray(formData.designContacts)
                      }
                      value={
                        formData?.designContacts &&
                        dictionaryToArray(formData.designContacts).filter(
                          (x: any) =>
                            formData?.designContactIds?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("designContactIds", e)
                      }
                      isMulti
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      getOptionLabel={(o: any) => o.value}
                      getOptionValue={(o: any) => o.key.toString()}
                      formatOptionLabel={(data: any) => (
                        <span
                          dangerouslySetInnerHTML={{ __html: data.value }}
                        />
                      )}
                    />
                    {validation?.response === false &&
                      validation.property?.includes("designContactIds") && (
                        <ValidationLabel>
                          *Software Product Owner must have a value
                        </ValidationLabel>
                      )}
                  </FieldBox>
                </Box>
                <Box
                  sx={{
                    display: "flex",
                    flexWrap: "wrap",
                    gap: "16px",
                    alignItems: "flex-start",
                  }}
                >
                  <FieldBox width="auto">
                    <ToggleSwitch
                      checked={
                        formData?.productNamesResource?.find(
                          (p: any) => p.key === formData.productNameId
                        )?.isSelected ?? false
                      }
                      disabled
                      label={`This is ${
                        formData?.productNamesResource?.find(
                          (p: any) => p.key === formData.productNameId
                        )?.isSelected
                          ? ""
                          : "not"
                      } a Platform Software (e.g. Broadcom)`}
                      showToggle={false}
                    />
                  </FieldBox>
                </Box>
              </SectionBox>

              <SectionBox>
                <Typography
                  sx={{
                    fontWeight: 700,
                    fontSize: "18px",
                    color: T.black,
                  }}
                >
                  Lifecycle Management
                </Typography>

                <Box
                  sx={{
                    display: "flex",
                    flexWrap: "wrap",
                    gap: "20px",
                    alignItems: "flex-start",
                  }}
                >
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: "4px",
                    }}
                  >
                    <DateField
                      label={<>End of Maintenance {!disabledDate && REQ}</>}
                      value={
                        formData?.endOfMaintenance
                          ? new Date(formData.endOfMaintenance)
                          : null
                      }
                      onChange={(d) => {
                        onChangeDate("endOfMaintenance", d);
                        onChangeEOM(d);
                      }}
                      disabled={disabledDate}
                      placeholder={
                        formData?.eomStatus === 0
                          ? "NOT ANNOUNCED"
                          : "NOT SPECIFIED"
                      }
                      id="majorSwAddModal_endOfMain_tour"
                    />
                    {!disabledDate &&
                      validation?.response === false &&
                      validation.property?.includes("endOfMaintenance") && (
                        <ValidationLabel>
                          *End Of Maintenance Date must have a value
                        </ValidationLabel>
                      )}
                    {!isOneTrack && (
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: "6px",
                          mt: "4px",
                        }}
                      >
                        <input
                          type="checkbox"
                          checked={formData?.eomStatus === 0}
                          onChange={(e) =>
                            onHandelChangeAnnounced(e.target.checked)
                          }
                          style={{
                            width: "16px",
                            height: "16px",
                            cursor: "pointer",
                          }}
                        />
                        <label
                          style={{
                            ...LABEL_STYLE,
                            cursor: "pointer",
                            marginBottom: 0,
                          }}
                        >
                          Not Announced
                        </label>
                      </Box>
                    )}
                  </Box>

                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: "4px",
                    }}
                  >
                    <DateField
                      label={<>End of support {!disabledEoSDate && REQ}</>}
                      value={
                        formData?.endOfsupport
                          ? new Date(formData.endOfsupport)
                          : null
                      }
                      onChange={(d) => onChangeDate("endOfsupport", d)}
                      disabled={disabledEoSDate}
                      placeholder={
                        formData?.eomStatus === 0 && disabledEoSDate
                          ? "NOT ANNOUNCED"
                          : "NOT SPECIFIED"
                      }
                      id="majorSwAddModal_endOfSupport_tour"
                    />
                    {!disabledEoSDate &&
                      validation?.response === false &&
                      validation.property?.includes("endOfsupport") && (
                        <ValidationLabel>
                          *End Of Support Date must have a value
                        </ValidationLabel>
                      )}
                    {!isOneTrack && (
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: "6px",
                          mt: "4px",
                        }}
                      >
                        <input
                          type="checkbox"
                          checked={disabledEoSDate}
                          onChange={(e) => onHandleCopyEoM(e.target.checked)}
                          style={{
                            width: "16px",
                            height: "16px",
                            cursor: "pointer",
                          }}
                        />
                        <label
                          style={{
                            ...LABEL_STYLE,
                            cursor: "pointer",
                            marginBottom: 0,
                          }}
                        >
                          Same as End of Maintenance
                        </label>
                      </Box>
                    )}
                  </Box>

                  <DateField
                    label="General availability date"
                    value={
                      formData?.generaAvailableDate
                        ? new Date(formData.generaAvailableDate)
                        : null
                    }
                    onChange={onChangeGeneralAvailabilityDate}
                    id="majorSwAddModal_avialDate_tour"
                  />
                </Box>
              </SectionBox>

              <SectionBox>
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    cursor: "pointer",
                  }}
                  onClick={() => setAdditionalExpanded((v) => !v)}
                >
                  <Typography
                    sx={{
                      fontWeight: 700,
                      fontSize: "18px",
                      color: T.black,
                    }}
                  >
                    Futher Details
                  </Typography>
                  {additionalExpanded ? (
                    <FiChevronUp size={20} />
                  ) : (
                    <FiChevronDown size={20} />
                  )}
                </Box>

                {additionalExpanded && (
                  <Box
                    sx={{
                      display: "flex",
                      flexWrap: "wrap",
                      gap: "16px",
                      mt: "8px",
                    }}
                  >
                    <DateField
                      label="Last time buy"
                      value={
                        formData?.lastTimeBuyNew
                          ? new Date(formData.lastTimeBuyNew)
                          : null
                      }
                      onChange={(d) => onChangeDate("lastTimeBuyNew", d)}
                      id="majorSwAddModal_lastTimeBuy_tour"
                    />

                    <FieldBox width={253}>
                      <label style={LABEL_STYLE}>Software Type</label>
                      <Box sx={{ display: "flex", gap: "4px" }}>
                        <Box sx={{ flex: 1 }}>
                          <Select
                            menuPortalTarget={document.body}
                            styles={SELECT_STYLES}
                            options={
                              formData?.criticalAssetTypeResource &&
                              dictionaryToArray(
                                formData.criticalAssetTypeResource
                              )
                            }
                            value={
                              formData?.criticalAssetTypeResource &&
                              dictionaryToArray(
                                formData.criticalAssetTypeResource
                              ).filter(
                                (el: any) =>
                                  el.key === formData?.criticalAssetTypeId
                              )
                            }
                            onChange={(e) =>
                              onChangeSelect("criticalAssetTypeId", e)
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            getOptionLabel={(o: any) => o.value}
                            getOptionValue={(o: any) => o.key.toString()}
                          />
                        </Box>
                        {tipologicaPermesso && (
                          <button
                            className="btn btn-link"
                            type="button"
                            onClick={() => setIsVisibleModalLookup(4)}
                            style={{ padding: "0 4px", flexShrink: 0 }}
                          >
                            <img
                              style={{ height: 15 }}
                              src={require("../../../img/plus_icon.png")}
                              alt="plus"
                            />
                          </button>
                        )}
                      </Box>
                    </FieldBox>

                    <FieldBox width={253}>
                      <label style={LABEL_STYLE}>Operating system</label>
                      <Box sx={{ display: "flex", gap: "4px" }}>
                        <Box sx={{ flex: 1 }}>
                          <Select
                            menuPortalTarget={document.body}
                            styles={SELECT_STYLES}
                            options={
                              formData?.operatingSystemResource &&
                              dictionaryToArray(
                                formData.operatingSystemResource
                              )
                            }
                            value={
                              formData?.operatingSystemResource &&
                              dictionaryToArray(
                                formData.operatingSystemResource
                              ).filter(
                                (x: any) =>
                                  x.key === formData?.operatingSystemId
                              )
                            }
                            onChange={(e) =>
                              onChangeSelect("operatingSystemId", e)
                            }
                            isSearchable
                            isClearable
                            getOptionLabel={(o: any) => o.value}
                            getOptionValue={(o: any) => o.key.toString()}
                          />
                        </Box>
                        {tipologicaPermesso && (
                          <button
                            className="btn btn-link"
                            type="button"
                            onClick={() => setIsVisibleModalLookup(3)}
                            style={{ padding: "0 4px", flexShrink: 0 }}
                          >
                            <img
                              style={{ height: 15 }}
                              src={require("../../../img/plus_icon.png")}
                              alt="plus"
                            />
                          </button>
                        )}
                      </Box>
                    </FieldBox>

                    {/* {mode === "edit" && (
                      <>
                        <FieldBox width={253}>
                          <label style={LABEL_STYLE}>Last Modified</label>
                          <input
                            style={{ ...INPUT_STYLE, background: "#f5f5f5" }}
                            value={
                              formatDateWithTime(
                                formData?.lastModified
                              )?.toUpperCase() ?? ""
                            }
                            readOnly
                          />
                        </FieldBox>
                        <FieldBox width={253}>
                          <label style={LABEL_STYLE}>Last Modified By</label>
                          <input
                            style={{ ...INPUT_STYLE, background: "#f5f5f5" }}
                            value={formData?.lastModifiedBy ?? ""}
                            readOnly
                          />
                        </FieldBox>
                      </>
                    )} */}
                  </Box>
                )}
              </SectionBox>
            </ScrollBody>

            {!wizardMode ? (
              <ModalFooter>
                <Box
                  onClick={attemptClose}
                  sx={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    px: "24px",
                    py: "8px",
                    border: "1px solid #000",
                    borderRadius: "6px",
                    cursor: "pointer",
                    minWidth: "100px",
                    height: "40px",
                    "&:hover": { background: "#F5F5F5" },
                  }}
                >
                  <Typography sx={{ fontSize: "16px", color: "#000" }}>
                    Cancel
                  </Typography>
                </Box>
                <Box
                  onClick={handleSave}
                  sx={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    px: "24px",
                    py: "8px",
                    background: T.red,
                    borderRadius: "6px",
                    cursor:
                      prevPage === "generatelcmdb" && softwareRedirect
                        ? "not-allowed"
                        : "pointer",
                    minWidth: "100px",
                    height: "40px",
                    opacity:
                      prevPage === "generatelcmdb" && softwareRedirect
                        ? 0.6
                        : 1,
                    "&:hover": {
                      background:
                        prevPage === "generatelcmdb" && softwareRedirect
                          ? T.red
                          : "#C40000",
                    },
                  }}
                  title={
                    prevPage === "generatelcmdb" && softwareRedirect
                      ? "Saving is disabled due to redirection from LCM Export screen"
                      : ""
                  }
                >
                  <Typography sx={{ fontSize: "16px", color: "#FFF" }}>
                    {mode === "edit" ? "Update" : "Save"}
                  </Typography>
                </Box>
              </ModalFooter>
            ) : (
              <ModalFooter sx={{ justifyContent: "space-between" }}>
                <Box
                  onClick={() => action?.setConfirmExitWizard?.()}
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    px: "24px",
                    py: "8px",
                    border: "1px solid #000",
                    borderRadius: "6px",
                    cursor: "pointer",
                    height: "40px",
                    "&:hover": { background: "#F5F5F5" },
                  }}
                >
                  <Typography sx={{ fontSize: "16px", color: "#000" }}>
                    Exit
                  </Typography>
                </Box>
                <Box sx={{ display: "flex", gap: "12px" }}>
                  <Box
                    onClick={
                      wizardStep && wizardStep > 1
                        ? () =>
                            action?.wizardBackFunction?.(
                              formData as MajorSoftwareBuildDtoCreate,
                              "majorSoftwareBuildDto"
                            )
                        : undefined
                    }
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      px: "24px",
                      py: "8px",
                      border: "1px solid #000",
                      borderRadius: "6px",
                      cursor:
                        wizardStep && wizardStep > 1
                          ? "pointer"
                          : "not-allowed",
                      height: "40px",
                      opacity: wizardStep && wizardStep > 1 ? 1 : 0.5,
                      "&:hover": {
                        background:
                          wizardStep && wizardStep > 1
                            ? "#F5F5F5"
                            : "transparent",
                      },
                    }}
                  >
                    <Typography sx={{ fontSize: "16px", color: "#000" }}>
                      Back
                    </Typography>
                  </Box>
                  <Box
                    onClick={validateWizard}
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      px: "24px",
                      py: "8px",
                      background: T.red,
                      borderRadius: "6px",
                      cursor: "pointer",
                      height: "40px",
                      "&:hover": { background: "#C40000" },
                    }}
                  >
                    <Typography sx={{ fontSize: "16px", color: "#FFF" }}>
                      Continue
                    </Typography>
                  </Box>
                </Box>
              </ModalFooter>
            )}
          </ModalCard>
        )}
      </Overlay>
    </>,
    document.body
  );
};

export default ProductSoftwareModal;
