import React, { useEffect, useRef, useState } from "react";
import { Box, Divider, Typography } from "@mui/material";
import { FiChevronDown, FiChevronUp, FiX } from "react-icons/fi";
import { createPortal } from "react-dom";
import Select from "react-select";
import { useDispatch, useSelector } from "react-redux";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import {
  MajorHardwareBuildDtoCreate,
  MajorHardwareBuildDtoUpdate,
} from "../../../Model/MajorHardwareBuild";
import {
  CreatMajorHardwareBuild,
  GetMajorHardwareBuildCreateResource,
} from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildCreateAction";
import {
  EditMajorHardwareBuild,
  GetMajorHardwareBuildEditResource,
} from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildEditAction";
import { RootState } from "../../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { useAuth } from "../../../Hook/useAuth";
import ModalConfirm from "../../../Components/ModalConfirm";
import { CommonValidation } from "../../../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import {
  formatDateWithTime,
  lowerFirstLetter,
  numberIsNullOrZero,
} from "../../../Hook/Common";
import { GetRuleFromBuildCostruction } from "../../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCommonAction";
import OriginalEquipmentManufacturer from "../../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import PlatformContainer from "../../../Containers/Lookup/PlatformContainer";
import HardwareSolutionResourceContainer from "../../../Containers/Lookup/HardwareSolutionResourceContainer";
import BuildConstructionContainer from "../../../Containers/Lookup/BuildConstructionContainer";
import {
  CloseButton,
  DateField,
  FieldBox,
  HeaderRow,
  INPUT_STYLE,
  LABEL_STYLE,
  ModalCard,
  ModalFooter,
  ModalHeader,
  Overlay,
  REQ,
  ScrollBody,
  SectionBox,
  SELECT_STYLES,
  T,
  ToggleSwitch,
  ValidationLabel,
} from "../../utils/styled";

interface ProductHardwareModalProps {
  open: boolean;
  onClose: () => void;
  mode?: "new" | "edit";
  editId?: number;
  wizardMode?: boolean;
  wizardStep?: number;
  dataWizard?: MajorHardwareBuildDtoCreate;
  hardwareRedirect?: boolean;
  prevPage?: string;
  keyTab?: string;
  action?: {
    closeModal?: (changed?: boolean) => any;
    refresh?: () => any;
    Edit?: (id: number | undefined) => any;
    validateFormWizard?: (
      response: boolean,
      formData: MajorHardwareBuildDtoCreate,
      property: string
    ) => void;
    wizardBackFunction?: (
      formData: MajorHardwareBuildDtoCreate,
      property: string
    ) => any;
    setConfirmExitWizard?: () => any;
    setDataCheck?: (prop: string, val: number | string) => any;
  };
}

const ProductHardwareModal: React.FC<ProductHardwareModalProps> = ({
  open,
  onClose,
  mode = "new",
  editId,
  wizardMode = false,
  wizardStep,
  dataWizard,
  hardwareRedirect,
  prevPage,
  keyTab,
  action,
}) => {
  const [showFurtherDetails, setShowFurtherDetails] = useState(false);
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] = useState(false);
  const [disabledDate, setDisabledDate] = useState(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState(false);
  const [disableHwType, setDisableHwType] = useState(true);
  const [required, setRequired] = useState(true);
  const [orphanDeleted, setOrphanDeleted] = useState(false);
  const [forceEdit, setForceEdit] = useState(false);
  const [autoFilled, setAutoFilled] = useState(false);

  const [proprietaryHardware, setProprietaryHardware] = useState(false);
  const [visualizeHardware, setVisualizeHardware] = useState(false);
  const [cotsOrOther, setCotsOrOther] = useState(false);
  const [noRules, setNoRules] = useState(false);
  const [manDC, setManDC] = useState(false);

  const [lookupFlag, setLookupFlag] = useState("");
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState(0);
  const [isCreateReady, setIsCreateReady] = useState(false);
  const [isEditLoaded, setIsEditLoaded] = useState(false);

  const overlayRef = useRef<HTMLDivElement>(null);

  const createResource = useSelector(
    (state: RootState) =>
      state.majorHardwareBuildCreateReducer.MajorHardwareBuildDtoCreate
  );
  const editResource = useSelector(
    (state: RootState) =>
      state.majorHardwareBuildEditReducer.MajorHardwareBuildDtoEdit
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
    setChanged,
    inputValue,
    setInputValue,
    confirmForm,
  } = useFormTableCrud<MajorHardwareBuildDtoUpdate>(
    CreatMajorHardwareBuild,
    EditMajorHardwareBuild
  );

  const { tipologicaPermesso } = useAuth();

  useEffect(() => {
    if (open && mode === "new" && !isCreateReady) {
      setIsCreateReady(true);
      GetMajorHardwareBuildCreateResource();
    }
    if (open && mode === "edit" && editId && !isEditLoaded) {
      setIsEditLoaded(true);
      GetMajorHardwareBuildEditResource(editId);
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
      setFormData(dataWizard);
      if (
        dataWizard?.vulnerabilityStatus !== undefined &&
        dataWizard?.vulnerabilityStatus !== ""
      ) {
        setIsVisibleFurtherDetails(true);
      }
    }
  }, [createResource, editResource, mode, dataWizard]);

  useEffect(() => {
    if (!open) {
      setIsCreateReady(false);
      setIsEditLoaded(false);
      return;
    }
    const handler = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };
    document.addEventListener("keydown", handler);
    return () => document.removeEventListener("keydown", handler);
  }, [open, onClose]);

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
      formData &&
      wizardMode &&
      !numberIsNullOrZero(formData?.originalEquipmentManufacturerId)
    ) {
      action?.setDataCheck?.(
        "mhOemId",
        formData.originalEquipmentManufacturerId
      );
    }
    if (formData && wizardMode && !numberIsNullOrZero(formData?.platformId)) {
      action?.setDataCheck?.("platform", formData.platformId);
    }
  }, [formData?.originalEquipmentManufacturerId, formData?.platformId]);

  useEffect(() => {
    if ((mode === "edit" && !autoFilled) || (wizardMode && !autoFilled)) {
      if (formData?.buildConstructionId) {
        onChangeBuildConstruction(formData.buildConstructionId, false).then(
          () => setAutoFilled(true)
        );
      } else {
        if (wizardMode) {
          setCotsOrOther(false);
          setProprietaryHardware(false);
          setVisualizeHardware(false);
        }
        setAutoFilled(false);
      }
    }
  }, [formData?.buildConstructionId]);

  const resetDefaultValues = (property: string) => {
    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    if (
      formData &&
      (formData[property] === "" ||
        formData[property] === undefined ||
        formData[property] === null)
    ) {
      if (property === "hardwareType") {
        copy.hardwareType = "Various";
      }
      setFormData(copy);
    }
  };

  const onHandelChangeAnnounced = (e: boolean) => {
    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    setChanged(true);
    if (e) {
      setDisabledDate(true);
      copy.eomStatus = 0;
      copy.endOfMaintenance = null;
      if (disabledEoSDate) copy.endOfsupport = undefined;
      setFormData(copy);
    } else {
      setDisabledDate(false);
      copy.eomStatus = 1;
      setFormData(copy);
    }
  };

  const onHandleCopyEoM = (e: boolean) => {
    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;
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
    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    if (date !== null) {
      copy["eomStatus"] = 2;
      copy[lowerFirstLetter("endOfMaintenance")] = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()}`;
    } else {
      copy["eomStatus"] = 1;
    }
    if (disabledEoSDate) {
      copy.endOfsupport = copy?.endOfMaintenance as Date;
    }
    setFormData(copy);
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;
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

  const onChangeBuildConstruction = async (
    key: number | undefined,
    autoReset: boolean,
    property?: string
  ) => {
    if (property && validation?.property?.includes(property)) {
      const v = { ...validation, property: [...validation.property] };
      v.property.splice(v.property.indexOf(property), 1);
      setValidation(v);
    }

    const copy = { ...formData } as MajorHardwareBuildDtoUpdate;

    if (key) {
      copy.buildConstructionId = key;

      if (mode === "edit") {
        if ([3, 4, 5, 6].includes(key)) {
          setVisualizeHardware(true);
          setProprietaryHardware(false);
          setCotsOrOther(false);
          setRequired(false);
          setNoRules(false);
          setManDC(false);
          if (autoReset) {
            copy.hardwareType = "Various";
            copy.hardwareSolution = "Multiple Applications";
            copy.lastTimeBuyNew = undefined;
            copy.lastTimeBuyExpansions = undefined;
            copy.lastTimeBuyUpgrades = undefined;
            copy.endOfsupport = undefined;
          }
        }

        if (key === 1 || key === 7) {
          setProprietaryHardware(true);
          setVisualizeHardware(false);
          setCotsOrOther(false);
          setNoRules(false);
          setManDC(true);
          setRequired(true);
          if (autoReset) {
            copy.hardwareSolution = editResource?.hardwareSolution ?? "";
            copy.hardwareType = editResource?.hardwareType ?? "";
            copy.platformId = editResource?.platformId ?? 0;
            const arr = (copy.hardwareType || "").split(
              '<b class="text-lowercase"> with </b>'
            );
            copy.hardwareType =
              arr[1] && arr.length > 1 ? `${arr[0]} with ${arr[1]}` : arr[0];
          }
        }

        if (key === 2) {
          setProprietaryHardware(false);
          setVisualizeHardware(false);
          setCotsOrOther(true);
          setNoRules(false);
          setManDC(true);
          setRequired(true);
          if (autoReset) {
            copy.hardwareType = editResource?.hardwareType ?? "";
            copy.hardwareSolution = editResource?.hardwareSolution ?? "";
            copy.platformId = editResource?.platformId ?? 0;
            copy.lastTimeBuyNew = undefined;
            copy.lastTimeBuyExpansions = undefined;
            copy.lastTimeBuyUpgrades = undefined;
            copy.endOfsupport = undefined;
          }
          const arr = (copy.hardwareType || "").split(
            '<b class="text-lowercase"> with </b>'
          );
          copy.hardwareType =
            arr[1] && arr.length > 1 ? `${arr[0]} with ${arr[1]}` : arr[0];
        }

        if (editResource?.buildConstructionId === key) {
          copy.otherHardwareInfo = editResource.otherHardwareInfo;
        } else {
          copy.otherHardwareInfo = "";
        }
      }

      if (mode !== "edit") {
        await GetRuleFromBuildCostruction(copy.buildConstructionId ?? 0).then(
          (r) => {
            switch (r) {
              case 0:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVisualizeHardware(false);
                setRequired(true);
                setManDC(true);
                setNoRules(true);
                if (autoReset) {
                  copy.hardwareType = "";
                  copy.platformId = 0;
                  copy.lastTimeBuyNew = undefined;
                  copy.lastTimeBuyExpansions = undefined;
                  copy.lastTimeBuyUpgrades = undefined;
                  copy.endOfsupport = undefined;
                  copy.hardwareSolution = "";
                }
                break;
              case 1:
                setProprietaryHardware(true);
                setCotsOrOther(false);
                setNoRules(false);
                setManDC(true);
                setRequired(true);
                setVisualizeHardware(false);
                if (autoReset) {
                  copy.hardwareSolution = "";
                  copy.hardwareType = "";
                  copy.platformId = 0;
                }
                break;
              case 2:
                setCotsOrOther(true);
                setProprietaryHardware(false);
                setVisualizeHardware(false);
                setNoRules(false);
                setManDC(true);
                setRequired(true);
                if (autoReset) {
                  copy.hardwareType = "";
                  copy.hardwareSolution = "";
                  copy.platformId = 0;
                  copy.lastTimeBuyNew = undefined;
                  copy.lastTimeBuyExpansions = undefined;
                  copy.lastTimeBuyUpgrades = undefined;
                  copy.endOfsupport = undefined;
                }
                break;
              case 3:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVisualizeHardware(true);
                setRequired(false);
                setNoRules(false);
                setManDC(false);
                if (autoReset) {
                  copy.hardwareType = "Various";
                  copy.hardwareSolution = "Multiple Applications";
                  copy.lastTimeBuyNew = undefined;
                  copy.lastTimeBuyExpansions = undefined;
                  copy.lastTimeBuyUpgrades = undefined;
                  copy.endOfsupport = undefined;
                }
                break;
              case 4:
              case 5:
              case 6:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVisualizeHardware(true);
                setRequired(false);
                setNoRules(false);
                setManDC(false);
                copy.hardwareType = "";
                copy.hardwareSolution = "";
                copy.platformId = 0;
                copy.lastTimeBuyNew = undefined;
                copy.lastTimeBuyExpansions = undefined;
                copy.lastTimeBuyUpgrades = undefined;
                copy.endOfsupport = undefined;
                break;
              default:
                break;
            }
          }
        );
      }
    } else {
      copy.buildConstructionId = undefined;
      setCotsOrOther(false);
      setProprietaryHardware(false);
      setVisualizeHardware(false);
      if (autoReset) {
        copy.hardwareType = "";
        copy.platformId = 0;
      }
    }
    setFormData(copy);
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

  const validazioneClient = (copy: MajorHardwareBuildDtoUpdate) => {
    const v = { response: true, property: [] } as CommonValidation;
    const fail = (p: string) => {
      v.property?.push(p);
      v.response = false;
    };

    if (
      !copy?.originalEquipmentManufacturerId ||
      copy.originalEquipmentManufacturerId === 0
    )
      fail("originalEquipmentManufacturerId");

    if (!copy?.buildConstructionId || copy.buildConstructionId === 0)
      fail("buildConstructionId");

    if (!copy?.platformId || copy.platformId === 0) fail("platformId");

    if (
      manDC &&
      (!copy?.designContactIds || copy.designContactIds.length === 0)
    )
      fail("designContactIds");

    if (
      (!copy?.hardwareType || copy.hardwareType.trim() === "") &&
      required &&
      !noRules
    )
      fail("hardwareType");

    if (
      (!copy?.hardwareSolution || copy.hardwareSolution.trim() === "") &&
      required &&
      !noRules
    )
      fail("hardwareSolution");

    if (!copy?.endOfsupport && !disabledEoSDate) fail("endOfsupport");

    if (!copy?.endOfMaintenance && !disabledDate) fail("endOfMaintenance");

    setValidation(v);
    return v;
  };

  const refresh = (changedVal: boolean) => {
    action?.closeModal?.(changedVal);
    action?.refresh?.();
    onClose();
  };

  const handleSave = () => {
    Save(
      formData,
      mode === "edit",
      validazioneClient,
      refresh,
      RestoreOrphanDeleted,
      orphanDeleted
    );
  };

  const validateWizard = () => {
    const copy = { ...formData } as MajorHardwareBuildDtoCreate;
    action?.validateFormWizard?.(
      validazioneClient(copy).response,
      copy,
      "majorHardwareBuildDto"
    );
  };

  const OriginalEquipmentManufacturerRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.originalEquipmentManufacturerResource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData)
        setFormData({
          ...formData,
          originalEquipmentManufacturerResource: obj,
        });
    } catch (err) {
      console.error("OriginalEquipmentManufacturerRefillData error:", err);
    }
  };

  const PlatformRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.platformResource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData) setFormData({ ...formData, platformResource: obj });
    } catch (err) {
      console.error("PlatformRefillData error:", err);
    }
  };

  const HardwareSolutionResourceRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.hardwareSolutionReource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData) setFormData({ ...formData, hardwareSolutionReource: obj });
    } catch (err) {
      console.error("HardwareSolutionResourceRefillData error:", err);
    }
  };

  const BuildConstructionResourceRefillData = async (value: any[]) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      const obj =
        res?.buildConstructionResource ??
        value.reduce((acc, i) => ({ ...acc, [i.id]: i.description }), {});
      if (formData)
        setFormData({ ...formData, buildConstructionResource: obj });
    } catch (err) {
      console.error("BuildConstructionResourceRefillData error:", err);
    }
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            lookUpFlag="equipment"
          />
        );
      case 2:
        return (
          <PlatformContainer
            returnObject={PlatformRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 3:
        return (
          <HardwareSolutionResourceContainer
            returnObject={HardwareSolutionResourceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 4:
        return (
          <BuildConstructionContainer
            returnObject={BuildConstructionResourceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return null;
    }
  };

  if (!open) return null;

  return createPortal(
    <>
      <ModalConfirm data={confirmForm} />

      {isVisibleModalLookup > 0 && (
        <Dialog
          open={isVisibleModalLookup > 0}
          onClose={(_, reason) => {
            if (reason === "backdropClick" || reason === "escapeKeyDown")
              return;
            setIsVisibleModalLookup(0);
          }}
          maxWidth="lg"
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
          if (e.target === overlayRef.current) onClose();
        }}
      >
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
                {mode === "new" ? "New Hardware" : "Edit Hardware"}
              </Typography>
              <CloseButton onClick={onClose}>
                <FiX style={{ fontSize: "20px", color: "#000" }} />
              </CloseButton>
            </HeaderRow>
            <Divider />
          </ModalHeader>

          <ScrollBody onChange={() => setChanged(true)}>
            <SectionBox>
              <Typography
                sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
              >
                Hardware Description
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
                  <label style={LABEL_STYLE}>
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
                          ).sort((a: any, b: any) =>
                            a.value.toLowerCase() < b.value.toLowerCase()
                              ? -1
                              : 1
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
                      <ValidationLabel>*OEM must have a value</ValidationLabel>
                    )}
                </FieldBox>

                <FieldBox width={323}>
                  <label style={LABEL_STYLE}>Build construction {REQ}</label>
                  <Box sx={{ display: "flex", gap: "4px" }}>
                    <Box sx={{ flex: 1 }}>
                      <Select
                        menuPortalTarget={document.body}
                        styles={SELECT_STYLES}
                        options={
                          formData?.buildConstructionResource &&
                          dictionaryToArray(formData.buildConstructionResource)
                        }
                        value={
                          formData?.buildConstructionResource &&
                          dictionaryToArray(
                            formData.buildConstructionResource
                          ).filter(
                            (x: any) => x.key === formData?.buildConstructionId
                          )
                        }
                        onChange={(e) =>
                          e &&
                          onChangeBuildConstruction(
                            (e as any)["key"],
                            true,
                            "buildConstructionId"
                          )
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
                  {validation?.response === false &&
                    validation.property?.includes("buildConstructionId") && (
                      <ValidationLabel>
                        *Build Construction must have a value
                      </ValidationLabel>
                    )}
                </FieldBox>

                <FieldBox width={323}>
                  <label style={LABEL_STYLE}>Platform {REQ}</label>
                  <Select
                    menuPortalTarget={document.body}
                    styles={SELECT_STYLES}
                    options={
                      formData?.platformResource &&
                      dictionaryToArray(formData.platformResource)
                    }
                    value={
                      formData?.platformResource &&
                      dictionaryToArray(formData.platformResource).filter(
                        (x: any) => x.key === formData?.platformId
                      )
                    }
                    onChange={(e) => onChangeSelect("platformId", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(o: any) => o.value}
                    getOptionValue={(o: any) => o.key.toString()}
                  />
                  {validation?.response === false &&
                    validation.property?.includes("platformId") && (
                      <ValidationLabel>
                        *Platform must have a value
                      </ValidationLabel>
                    )}
                </FieldBox>

                <FieldBox width={323}>
                  <label style={LABEL_STYLE}>HW type {REQ}</label>
                  <Box sx={{ display: "flex", gap: "4px" }}>
                    <input
                      style={INPUT_STYLE}
                      readOnly={mode === "edit" || disableHwType}
                      disabled={disableHwType}
                      value={formData?.hardwareType ?? ""}
                      onChange={(e) => onChange("hardwareType", e)}
                      onKeyUp={(e) => onChange("hardwareType", e)}
                    />
                    {tipologicaPermesso && (
                      <button
                        className="btn btn-link"
                        type="button"
                        onClick={() => {
                          setDisableHwType(!disableHwType);
                          resetDefaultValues("hardwareType");
                        }}
                        style={{ padding: "0 4px", flexShrink: 0 }}
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/edit.png")}
                          alt="edit"
                        />
                      </button>
                    )}
                  </Box>
                  {validation?.response === false &&
                    validation.property?.includes("hardwareType") && (
                      <ValidationLabel>
                        *Hardware Type must have a value
                      </ValidationLabel>
                    )}
                </FieldBox>

                <FieldBox width={323}>
                  <label style={LABEL_STYLE}>
                    What application does this hardware host? {REQ}
                  </label>
                  <input
                    style={INPUT_STYLE}
                    readOnly={mode === "edit"}
                    value={formData?.hardwareSolution ?? ""}
                    onChange={(e) => onChange("hardwareSolution", e)}
                    onKeyUp={(e) => onChange("hardwareSolution", e)}
                  />
                  {validation?.response === false &&
                    validation.property?.includes("hardwareSolution") && (
                      <ValidationLabel>
                        *Hardware Solution must have a value
                      </ValidationLabel>
                    )}
                </FieldBox>

                <FieldBox width={323}>
                  <label style={LABEL_STYLE}>
                    Design contact {manDC && REQ}
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
                        (x: any) => formData?.designContactIds?.includes(x.key)
                      )
                    }
                    onChange={(e) => OnChangeMultiSelect("designContactIds", e)}
                    isMulti
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isClearable
                    getOptionLabel={(o: any) => o.value}
                    getOptionValue={(o: any) => o.key.toString()}
                    formatOptionLabel={(data: any) => (
                      <span dangerouslySetInnerHTML={{ __html: data.value }} />
                    )}
                  />
                  {validation?.response === false &&
                    validation.property?.includes("designContactIds") && (
                      <ValidationLabel>
                        *Design Contact must have a value
                      </ValidationLabel>
                    )}
                </FieldBox>
              </Box>
            </SectionBox>

            <SectionBox>
              <Typography
                sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
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
                  sx={{ display: "flex", flexDirection: "column", gap: "4px" }}
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
                  />
                  {!disabledDate &&
                    validation?.response === false &&
                    validation.property?.includes("endOfMaintenance") && (
                      <ValidationLabel>
                        *End Of Maintenance Date must have a value
                      </ValidationLabel>
                    )}
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
                </Box>

                <Box
                  sx={{ display: "flex", flexDirection: "column", gap: "4px" }}
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
                  />
                  {!disabledEoSDate &&
                    validation?.response === false &&
                    validation.property?.includes("endOfsupport") && (
                      <ValidationLabel>
                        *End Of Support Date must have a value
                      </ValidationLabel>
                    )}
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
                </Box>

                <DateField
                  label="General availability date"
                  value={
                    formData?.generaAvailableDate
                      ? new Date(formData.generaAvailableDate)
                      : null
                  }
                  onChange={(d) => onChangeDate("generaAvailableDate", d)}
                />

                {proprietaryHardware && (
                  <>
                    <DateField
                      label="Last time buy - new"
                      value={
                        formData?.lastTimeBuyNew
                          ? new Date(formData.lastTimeBuyNew)
                          : null
                      }
                      onChange={(d) => onChangeDate("lastTimeBuyNew", d)}
                    />
                    <DateField
                      label="Last time buy - expansions"
                      value={
                        formData?.lastTimeBuyExpansions
                          ? new Date(formData.lastTimeBuyExpansions)
                          : null
                      }
                      onChange={(d) => onChangeDate("lastTimeBuyExpansions", d)}
                    />
                    <DateField
                      label="Last time buy - upgrades"
                      value={
                        formData?.lastTimeBuyUpgrades
                          ? new Date(formData.lastTimeBuyUpgrades)
                          : null
                      }
                      onChange={(d) => onChangeDate("lastTimeBuyUpgrades", d)}
                    />
                  </>
                )}
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
                onClick={() => setShowFurtherDetails((v) => !v)}
              >
                <Typography
                  sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
                >
                  Futher Details
                </Typography>
                {showFurtherDetails ? (
                  <FiChevronUp size={20} />
                ) : (
                  <FiChevronDown size={20} />
                )}
              </Box>

              {showFurtherDetails && (
                <Box
                  sx={{
                    display: "flex",
                    flexWrap: "wrap",
                    gap: "16px",
                    mt: "8px",
                  }}
                >
                  <FieldBox width={323}>
                    <label style={LABEL_STYLE}>Vulnerability status</label>
                    <input
                      style={INPUT_STYLE}
                      value={formData?.vulnerabilityStatus ?? ""}
                      onChange={(e) => onChange("vulnerabilityStatus", e)}
                      onKeyUp={(e) => onChange("vulnerabilityStatus", e)}
                    />
                  </FieldBox>

                  {(proprietaryHardware || cotsOrOther) && (
                    <FieldBox width={323}>
                      <label style={LABEL_STYLE}>Other HW info</label>
                      <input
                        style={INPUT_STYLE}
                        value={formData?.otherHardwareInfo ?? ""}
                        onChange={(e) => onChange("otherHardwareInfo", e)}
                        onKeyUp={(e) => onChange("otherHardwareInfo", e)}
                      />
                    </FieldBox>
                  )}

                  <FieldBox width={625}>
                    <label style={LABEL_STYLE}>Description</label>
                    <input
                      style={INPUT_STYLE}
                      maxLength={2000}
                      value={formData?.description ?? ""}
                      onChange={(e) => onChange("description", e)}
                      onKeyUp={(e) => onChange("description", e)}
                    />
                  </FieldBox>

                  {mode === "edit" && (
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
                  )}
                </Box>
              )}
            </SectionBox>
          </ScrollBody>

          {!wizardMode ? (
            <ModalFooter>
              <Box
                onClick={() => {
                  action?.closeModal?.(changed);
                  onChangeBuildConstruction(
                    undefined,
                    true,
                    "buildConstructionId"
                  );
                  onClose();
                }}
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
                    prevPage === "generatelcmdb" && hardwareRedirect
                      ? "not-allowed"
                      : "pointer",
                  minWidth: "100px",
                  height: "40px",
                  opacity:
                    prevPage === "generatelcmdb" && hardwareRedirect ? 0.6 : 1,
                  "&:hover": {
                    background:
                      prevPage === "generatelcmdb" && hardwareRedirect
                        ? T.red
                        : "#C40000",
                  },
                }}
                title={
                  prevPage === "generatelcmdb" && hardwareRedirect
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
                            formData as MajorHardwareBuildDtoCreate,
                            "majorHardwareBuildDto"
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
                      wizardStep && wizardStep > 1 ? "pointer" : "not-allowed",
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
      </Overlay>
    </>,
    document.body
  );
};

export default ProductHardwareModal;
