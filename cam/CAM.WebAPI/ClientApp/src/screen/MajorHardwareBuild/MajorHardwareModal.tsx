import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Modal, Form } from "react-bootstrap";
import {
  formatDateWithTime,
  numberIsNullOrZero,
  lowerFirstLetter,
} from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useDispatch, useSelector } from "react-redux";
import Select from "react-select";
import { MajorHardwareBuildDtoUpdate } from "../../Model/MajorHardwareBuild";
import {
  CreatMajorHardwareBuild,
  GetMajorHardwareBuildCreateResource,
} from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildCreateAction";
import { EditMajorHardwareBuild } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import HardwareSolutionResourceContainer from "../../Containers/Lookup/HardwareSolutionResourceContainer";
import { useAuth } from "../../Hook/useAuth";
import BuildConstructionContainer from "../../Containers/Lookup/BuildConstructionContainer";
import PlatformContainer from "../../Containers/Lookup/PlatformContainer";
import ModalConfirm from "../../Components/ModalConfirm";
import { GetRuleFromBuildCostruction } from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCommonAction";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import DatePicker from "react-datepicker";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import TourGuide from "../../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../../Redux/Action/tourActions";
import { getMajorHwModalTourSteps } from "../../Constant/TourSteps";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: MajorHardwareBuildDtoUpdate,
      property: string
    );
    wizardBackFunction?(
      formData: MajorHardwareBuildDtoUpdate,
      property: string
    ): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  edit: boolean;
  keyTab?: string;
  hardwareRedirect?: boolean;
  prevPage?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: MajorHardwareBuildDtoUpdate;
}

const MajorHardwareModal: React.FC<Props> = (props) => {
  const { tipologicaPermesso, VerifyIsInRole } = useAuth();
  const [keyTabs, setKey] = useState("MajorHardwareBuild");
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    onChangeDate,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<MajorHardwareBuildDtoUpdate>(
    CreatMajorHardwareBuild,
    EditMajorHardwareBuild
  );
  const dtoEditResourceState = (state: RootState) =>
    state.majorHardwareBuildEditReducer.MajorHardwareBuildDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.majorHardwareBuildCreateReducer.MajorHardwareBuildDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disableWhat, setDisableWhat] = useState<boolean>(true);
  const [disablePlatform, setDisablePlatform] = useState<boolean>(true);
  const [disableHwType, setDisableHwType] = useState<boolean>(true);
  const [required, setRequired] = useState<boolean>(true);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const handleEndTour = () => dispatch(endGuideTour());

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
      setFormData(props.dataWizard);
      if (
        props.dataWizard?.vulnerabilityStatus != undefined &&
        props.dataWizard?.vulnerabilityStatus != ""
      ) {
        setIsVisibleFurtherDetails(true);
      }
    }

    console.log("create form data => ", createResource);
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("MajorHardwareBuild");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  useEffect(() => {
    if (
      formData &&
      props.wizardMode &&
      !numberIsNullOrZero(formData?.originalEquipmentManufacturerId)
    ) {
      props.action.setDataCheck &&
        props.action.setDataCheck(
          "mhOemId",
          formData?.originalEquipmentManufacturerId
        );
    }
    if (
      formData &&
      props.wizardMode &&
      !numberIsNullOrZero(formData?.platformId)
    ) {
      props.action.setDataCheck &&
        props.action.setDataCheck("platform", formData?.platformId);
    }
  }, [formData?.originalEquipmentManufacturerId, formData?.platformId]);

  const resetDefaultValues = (property: string) => {
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    if (
      (formData && formData[property] === "") ||
      (formData && formData[property] === undefined) ||
      (formData && formData[property] === null)
    ) {
      if (property === "hardwareSolution") {
        copy.hardwareSolution = "Multiple Applications";
      }

      if (property === "hardwareType") {
        copy.hardwareType = "Various";
      }

      setFormData(copy);
    }
  };
  const onHandleCopyEoM = (e: boolean) => {
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
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
  const validazioneClient = (copy: MajorHardwareBuildDtoUpdate) => {
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
      copy?.buildConstructionId == null ||
      copy?.buildConstructionId === undefined ||
      copy?.buildConstructionId === 0
    ) {
      addInvalidProperty("buildConstructionId");
    }
    if (
      copy?.platformId == null ||
      copy?.platformId === undefined ||
      copy?.platformId === 0
    ) {
      addInvalidProperty("platformId");
    }
    if (
      manDC &&
      (copy?.designContactIds === null ||
        copy?.designContactIds === undefined ||
        copy?.designContactIds?.length === 0)
    ) {
      addInvalidProperty("designContactIds");
    }
    // if (cotsOrOther && (processor.trim() == "" || processor == undefined)) {
    //   setValidazioneCustom({ response: false, property: "processor" });
    //   addInvalidProperty("processor");
    // }
    // if (
    //   cotsOrOther &&
    //   (operatingSystem?.trim() == "" || operatingSystem == undefined)
    // ) {
    //   setValidazioneCustom({ response: false, property: "operatingSystem" });
    //   addInvalidProperty("operatingSystem");
    // }
    if (
      (copy?.hardwareType == null ||
        copy?.hardwareType === undefined ||
        copy?.hardwareType.trim() === "") &&
      required &&
      !noRules
    ) {
      addInvalidProperty("hardwareType");
      setValidazioneCustom(undefined);
    }
    if (
      (copy?.hardwareSolution == null ||
        copy?.hardwareSolution === undefined ||
        copy?.hardwareSolution.trim() === "") &&
      required &&
      !noRules
    ) {
      addInvalidProperty("hardwareSolution");
      setValidazioneCustom(undefined);
    }
    if (
      (copy?.endOfsupport == null || copy?.endOfsupport === undefined) &&
      !disabledEoSDate
    ) {
      addInvalidProperty("endOfsupport");
    }
    if (
      (copy?.endOfMaintenance == null ||
        copy?.endOfMaintenance === undefined) &&
      !disabledDate
    ) {
      addInvalidProperty("endOfMaintenance");
    }
    // if (
    //   (copy?.endOfMaintenance == null ||
    //     copy?.endOfMaintenance === undefined ||
    //     copy?.endOfMaintenance.toString() == "") &&
    // ) {
    //   addInvalidProperty("endOfMaintenance");
    //   setValidazioneCustom(undefined);
    // }

    // setValidazioneCustom(undefined);
    setValidation(copyValidation);
    return copyValidation;

    // return { response: true }
  };

  const [validazioneCustom, setValidazioneCustom] = useState<
    { response: boolean; property: string } | undefined
  >();

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            lookUpFlag="equipment"
          ></OriginalEquipmentManufacturer>
        );
      case 2:
        return (
          <PlatformContainer
            returnObject={PlatformRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></PlatformContainer>
        );
      case 3:
        return (
          <HardwareSolutionResourceContainer
            returnObject={HardwareSolutionResourceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></HardwareSolutionResourceContainer>
        );
      case 4:
        return (
          <BuildConstructionContainer
            returnObject={BuildConstructionResourceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></BuildConstructionContainer>
        );
      // case 5:
      //     return <HardwareTypeContainer returnObject={HardwareTypeRefillData} modal={{ isModal: true, setIsVisibleModalLookup }}></HardwareTypeContainer>;
      default:
        return;
    }
  };

  useEffect(() => {
    if (
      props.edit &&
      formData &&
      formData?.endOfsupport === formData?.endOfMaintenance
    ) {
      setDisabledEoSDate(true);
    }
  }, [formData?.endOfsupport]);

  const OriginalEquipmentManufacturerRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } =
        res?.originalEquipmentManufacturerResource
          ? res?.originalEquipmentManufacturerResource
          : value.reduce(
              (acc, item) => ({ ...acc, [item.id]: item.description }),
              {}
            ) ?? [];
      formData &&
        setFormData({
          ...formData,
          originalEquipmentManufacturerResource: obj,
        });
    } catch (error) {
      console.error("Error in OriginalEquipmentManufacturerRefillData:", error);
    }
  };

  const PlatformRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.platformResource
        ? res?.platformResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, platformResource: obj });
    } catch (error) {
      console.error("Error in PlatformRefillData:", error);
    }
  };

  const HardwareSolutionResourceRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.hardwareSolutionReource
        ? res?.hardwareSolutionReource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, hardwareSolutionReource: obj });
    } catch (error) {
      console.error("Error in HardwareSolutionResourceRefillData:", error);
    }
  };

  const BuildConstructionResourceRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetMajorHardwareBuildCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.buildConstructionResource
        ? res?.buildConstructionResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, buildConstructionResource: obj });
    } catch (error) {
      console.error("Error in BuildConstructionResourceRefillData:", error);
    }
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] =
    useState<boolean>(false);

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

  // useEffect(() => {
  // 	if (formData && formData.lastTimeBuyNew) changeDate("lastTimeBuyNew", formData?.lastTimeBuyNew);

  // 	if (formData && formData.lastTimeBuyExpansions) changeDate("lastTimeBuyExpansions", formData?.lastTimeBuyExpansions);

  // 	if (formData && formData.lastTimeBuyUpgrades) changeDate("lastTimeBuyUpgrades", formData?.lastTimeBuyUpgrades);
  // }, [formData?.lastTimeBuyNew, formData?.lastTimeBuyExpansions, formData?.lastTimeBuyUpgrades]);

  //NUOVE REGOLE
  const [proprietaryHardware, setProprietaryHardware] =
    useState<boolean>(false);

  const [visualizeHardware, setVirsualizeHardware] = useState<boolean>(false);
  const [cotsOrOther, setCotsOrOther] = useState<boolean>(false);
  const [noRules, setNoRules] = useState<boolean>(false);
  const [manDC, setManDC] = useState<boolean>(false);

  const [autoFilled, setAutoFilled] = useState<boolean>(false);

  //const [processor, setProcessor] = useState<string>("");
  //const [operatingSystem, setOperatingSystem] = useState<string>("");

  useEffect(() => {
    if ((props.edit && !autoFilled) || (props.wizardMode && !autoFilled)) {
      if (
        formData?.buildConstructionId &&
        formData.buildConstructionId != undefined
      ) {
        onChangeBuildConstruction(formData.buildConstructionId, false).then(
          (x) => {
            setAutoFilled(true);
          }
        );
      } else {
        if (props.wizardMode) {
          setCotsOrOther(false);
          setProprietaryHardware(false);
          setVirsualizeHardware(false);
          //setProcessor("");
          //setOperatingSystem("");
          // setAutoFilled(true)
        }
        setAutoFilled(false);
      }
    }
  }, [formData?.buildConstructionId]);

  const onChangeEOM = (date: Date) => {
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;

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

  const onChangeBuildConstruction = async (
    key: number | undefined,
    autoReset: boolean,
    property?: string
  ) => {
    // REMOVE VALIDATION
    if (property && validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    if (key && key !== undefined) {
      copy.buildConstructionId = key;
      if (props.edit) {
        if (key === 3 || key === 4 || key === 5 || key === 6) {
          setVirsualizeHardware(true);
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
            //setProcessor("");
            // setOperatingSystem("");
          }
        }

        if (key === 1 || key === 7) {
          setProprietaryHardware(true);
          setVirsualizeHardware(false);
          setCotsOrOther(false);
          setNoRules(false);
          setManDC(true);
          setRequired(true);
          if (autoReset) {
            copy.hardwareSolution =
              props.edit && editResource?.hardwareSolution
                ? editResource?.hardwareSolution
                : "";
            copy.hardwareType =
              props.edit && editResource?.hardwareType
                ? editResource?.hardwareType
                : "";
            copy.platformId =
              props.edit && editResource?.platformId
                ? editResource?.platformId
                : 0;

            //setProcessor("");
            //setOperatingSystem("");
            let arr = copy.hardwareType.split(
              '<b class="text-lowercase"> with </b>'
            );
            if (arr[1] !== "" && arr.length > 1) {
              copy.hardwareType = arr[0] + " with " + arr[1];
            } else {
              copy.hardwareType = arr[0];
            }
          }
        }

        if (key === 2) {
          console.log("enter on case 2");
          setProprietaryHardware(false);
          setVirsualizeHardware(false);
          setCotsOrOther(true);
          setNoRules(false);
          setManDC(true);
          setRequired(true);
          if (autoReset) {
            copy.hardwareType =
              props.edit && editResource && editResource.hardwareType
                ? editResource.hardwareType
                : "";
            copy.hardwareSolution =
              props.edit && editResource?.hardwareSolution
                ? editResource?.hardwareSolution
                : "";
            copy.platformId =
              props.edit && editResource?.platformId
                ? editResource?.platformId
                : 0;
            copy.lastTimeBuyNew = undefined;
            copy.lastTimeBuyExpansions = undefined;
            copy.lastTimeBuyUpgrades = undefined;
            copy.endOfsupport = undefined;
            //setOperatingSystem("");
          }
          let arr = copy.hardwareType.split(
            '<b class="text-lowercase"> with </b>'
          );
          if (arr[1] !== "" && arr.length > 1) {
            copy.hardwareType = arr[0] + " with " + arr[1];
          } else {
            copy.hardwareType = arr[0];
          }
        }

        if (editResource?.buildConstructionId === key) {
          copy.otherHardwareInfo = editResource.otherHardwareInfo;
        } else {
          copy.otherHardwareInfo = "";
        }
      }

      if (!props.edit) {
        console.log("enter on not equal =>");
        await GetRuleFromBuildCostruction(copy.buildConstructionId ?? 0).then(
          (r) => {
            switch (r) {
              case 0:
                //NO RULES
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVirsualizeHardware(false);
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
                  copy.hardwareSolution = "";
                  copy.hardwareType = "";
                  copy.platformId = 0;
                }
                break;

              case 1:
                //PROPRIETARY
                setProprietaryHardware(true);
                setCotsOrOther(false);
                setNoRules(false);
                setManDC(true);
                setRequired(true);
                setVirsualizeHardware(false);
                if (autoReset) {
                  copy.hardwareSolution = "";
                  copy.hardwareType = "";
                  copy.platformId = 0;
                }
                break;

              case 2:
                //COTS or OTHER
                setCotsOrOther(true);
                setProprietaryHardware(false);
                setVirsualizeHardware(false);
                setNoRules(false);
                setManDC(true);
                setRequired(true);
                console.log("case 2");
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
                //NFVI
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVirsualizeHardware(true);
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
                  //setProcessor("");
                  // setOperatingSystem("");
                }
                break;

              case 4:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVirsualizeHardware(true);
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

              case 5:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVirsualizeHardware(true);
                setNoRules(false);
                setManDC(false);
                copy.hardwareType = "";
                copy.hardwareSolution = "";
                setRequired(false);
                copy.platformId = 0;
                copy.lastTimeBuyNew = undefined;
                copy.lastTimeBuyExpansions = undefined;
                copy.lastTimeBuyUpgrades = undefined;
                copy.endOfsupport = undefined;
                break;

              case 6:
                setCotsOrOther(false);
                setProprietaryHardware(false);
                setVirsualizeHardware(true);
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
      setProprietaryHardware(false);
      if (autoReset) {
        copy.hardwareType = "";
        copy.platformId = 0;
        //setProcessor("");
        // setOperatingSystem("");
      }
    }
    setFormData(copy);
  };
  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
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
  const validateWizard = () => {
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "majorHardwareBuildDto"
      );
  };

  const onHandelChangeAnnounced = (e: boolean) => {
    let copy = { ...formData } as MajorHardwareBuildDtoUpdate;
    if (e) {
      setDisabledDate(true);
      copy.eomStatus = 0;
      copy.endOfMaintenance = null;
      if (disabledEoSDate) {
        copy.endOfsupport = undefined;
      }
      setFormData(copy);
    } else {
      setDisabledDate(false);
      copy.eomStatus = 1;
      setFormData(copy);
    }
  };
  return (
    <div className="col-12">
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
          maxWidth="lg"
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
      <form id="formHardwareBuild" onChange={() => setChanged(true)}>
        <div className="row">
          <fieldset className="fieldset">
            <label className="text-bb">Hardware Description</label>
            <div className="row">
              <div className="col-6">
                <div className="col-12 pl-0">
                  <label
                    className="labelForm voda-bold w-100 mb-20"
                    id="majorHwAddModal_equipManufactur_tour"
                  >
                    Equipment Manufacturer<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.originalEquipmentManufacturerResource
                            ).sort((a, b) =>
                              a.value.toLowerCase() < b.value.toLowerCase()
                                ? -1
                                : 1
                            )
                          }
                          placeholder="Select Manufacturer"
                          value={
                            formData?.originalEquipmentManufacturerId !=
                            undefined
                              ? formData?.originalEquipmentManufacturerResource &&
                                dictionaryToArray(
                                  formData?.originalEquipmentManufacturerResource
                                ).filter(
                                  (x) =>
                                    x.key ==
                                    formData?.originalEquipmentManufacturerId
                                )
                              : null
                          }
                          onChange={(e) =>
                            onChangeSelect("originalEquipmentManufacturerId", e)
                          }
                          isDisabled={props.edit}
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
                              "/api/OriginalEquipmentManufacturer"
                            );
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
              <div className="col-6">
                <div className="col-12 pr-0">
                  <label
                    className="labelForm voda-bold w-100 mb-20"
                    id="majorHwAddModal_BuildConstruction_tour"
                  >
                    Build Construction <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.buildConstructionResource &&
                            dictionaryToArray(
                              formData?.buildConstructionResource
                            )
                          }
                          value={
                            formData?.buildConstructionResource &&
                            dictionaryToArray(
                              formData?.buildConstructionResource
                            ).filter(
                              (x) => x.key == formData?.buildConstructionId
                            )
                          }
                          onChange={(e) =>
                            e &&
                            onChangeBuildConstruction(
                              e && e["key"],
                              true,
                              "buildConstructionId"
                            )
                          }
                          placeholder="Select Build Construction"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
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
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("buildConstructionId") ? (
                      <label className="validation">
                        *build Construction must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>

              <div className="col-12 px-0">
                <fieldset className="fieldset">
                  <div className="row">
                    <div className="col-6 pl-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold w-100">
                          Platform <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.platformResource &&
                                  dictionaryToArray(formData?.platformResource)
                                }
                                value={
                                  formData?.platformResource &&
                                  dictionaryToArray(
                                    formData?.platformResource
                                  ).filter(
                                    (x) => x.key === formData?.platformId
                                  )
                                }
                                onChange={(e) =>
                                  onChangeSelect("platformId", e)
                                }
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              ></Select>
                            </div>
                          </div>
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes("platformId") ? (
                            <label className="validation">
                              *platform must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6 pr-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold w-100">
                          HW Type <span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <input
                                readOnly={props.edit}
                                type="text"
                                disabled={disableHwType}
                                onChange={(e) => onChange("hardwareType", e)}
                                onKeyUp={(e) => onChange("hardwareType", e)}
                                className="inputForm w-100"
                                value={formData?.hardwareType ?? ""}
                              />
                            </div>
                            {tipologicaPermesso && (
                              <button
                                className="btn btn-link"
                                onClick={() => {
                                  setDisableHwType(!disableHwType);
                                  resetDefaultValues("hardwareType");
                                }}
                                type="button"
                              >
                                <img
                                  style={{ height: 15 }}
                                  src={require("../../img/edit.png")}
                                  alt="plus"
                                />
                              </button>
                            )}
                          </div>
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes("hardwareType") ? (
                            <label className="validation">
                              *hardware Type must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </div>

                  <div className="row">
                    <div className="col-6 pl-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold w-100">
                          What Application does this Hardware Host?
                          <span className="red">*</span>
                          <input
                            type="text"
                            readOnly={props.edit}
                            onChange={(e) => onChange("hardwareSolution", e)}
                            onKeyUp={(e) => onChange("hardwareSolution", e)}
                            className="inputForm w-100"
                            value={formData?.hardwareSolution ?? ""}
                          />
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes("hardwareSolution") ? (
                            <label className="validation">
                              *hardwareSolution must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6 pr-0">
                      <div className="col-12">
                        <label
                          className="labelForm voda-bold w-100"
                          id="majorHwAddModal_DesignContact_tour"
                        >
                          Design Contact
                          {manDC && <span className="red">*</span>}
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
                                  dictionaryToArray(
                                    formData?.designContacts
                                  ).filter((x) =>
                                    formData?.designContactIds?.includes(x.key)
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
                                "designContactIds"
                              ) ? (
                                <label className="validation">
                                  *Design Contact must have a value
                                </label>
                              ) : null}
                            </div>
                          </div>
                        </label>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>

              <fieldset className="fieldset">
                <div className="row">
                  <div className="col-6 pl-0">
                    <div className={`col-12`}>
                      <div className="mt-2">
                        <div className="flex">
                          <span
                            className="labelForm voda-bold sp w-100"
                            id="majorHwAddModal_endOfMain_tour"
                          >
                            <span className="fz-16">
                              End of Maintenance{<span className="red">*</span>}
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
                              className="inputForm w-100 "
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
                          </span>
                          <Form.Check
                            type="checkbox"
                            className={`radio labelForm voda w-100 mt-20 ${
                              props.wizardMode
                                ? "flex-basis-60 mt-3"
                                : "flex-basis-36"
                            }`}
                            name="userLogin"
                            value="1"
                            label="Not Announced"
                            checked={formData?.eomStatus === 0 ? true : false}
                            onChange={(e: any) =>
                              onHandelChangeAnnounced(e.target.checked)
                            }
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
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="row">
                  <div className="col-9 pl-0">
                    <div className="col-12">
                      <div className="mt-2">
                        <div className="flex">
                          <span
                            className="labelForm voda-bold sp w-100"
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
                              className="inputForm w-94"
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
                                validation.property?.includes(
                                  "endOfsupport"
                                ) ? (
                                  <label className="validation">
                                    *End Of Support Date must have a value
                                  </label>
                                ) : null}
                              </label>
                            )}
                          </span>
                          <Form.Check
                            style={{ paddingLeft: "0px" }}
                            type="checkbox"
                            className={`radio labelForm voda w-100 mt-20 `}
                            name="userLogin"
                            value="1"
                            label="Same as End Of Maintenance"
                            checked={disabledEoSDate}
                            onChange={(e: any) =>
                              onHandleCopyEoM(e.target.checked)
                            }
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </fieldset>
            </div>
          </fieldset>

          {!props.wizardMode && (
            <div className="col-12">
              <button
                type="button"
                className="mb-40 further-btn"
                onClick={() => setShowFurtherDetails(!showFurtherDetails)}
              >
                Click for further details
              </button>
            </div>
          )}

          {showFurtherDetails ? (
            <>
              <fieldset className="fieldset">
                <div className="row">
                  <div className="col-6 pl-0 pr-4">
                    <div className="col-12">
                      <label className="labelForm voda-bold w-100">
                        General Availability Date
                        <DatePicker
                          selected={
                            formData?.generaAvailableDate &&
                            new Date(formData?.generaAvailableDate)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("generaAvailableDate", newDate);
                          }}
                          className="inputForm w-100"
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </label>
                    </div>
                  </div>
                  {/* <div className="col-6">
                    <div className="col-12 pr-0">
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
                          className="inputForm w-100 "
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("endOfsupport") ? (
                          <label className="validation">
                            *End of Support must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div> */}

                  {proprietaryHardware && (
                    <>
                      <div className="col-6 pl-0 pr-4">
                        <div className="col-12">
                          <label className="labelForm voda-bold w-100">
                            Last Time Buy - New
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyNew &&
                                new Date(formData?.lastTimeBuyNew)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyNew", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>

                        <div className="col-12">
                          <label className="labelForm voda-bold w-100">
                            Last Time Buy - Expansions
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyExpansions &&
                                new Date(formData?.lastTimeBuyExpansions)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyExpansions", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>
                      </div>

                      <div className="col-6 pr-0">
                        <div className="col-12">
                          <label className="labelForm voda-bold w-100">
                            Last Time Buy - Upgrades
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyUpgrades &&
                                new Date(formData?.lastTimeBuyUpgrades)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyUpgrades", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>
                      </div>
                    </>
                  )}
                </div>
              </fieldset>
            </>
          ) : null}

          <Container show={!props.wizardMode && showFurtherDetails}>
            <fieldset className="fieldset">
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <label className="labelForm voda-bold w-100 pr-2">
                      Vulnerability Status
                      <input
                        type="text"
                        onChange={(e) => onChange("vulnerabilityStatus", e)}
                        onKeyUp={(e) => onChange("vulnerabilityStatus", e)}
                        className="inputForm w-100"
                        value={formData?.vulnerabilityStatus ?? ""}
                      />
                    </label>
                  </div>
                </div>

                <Container
                  show={
                    !props.wizardMode && (proprietaryHardware || cotsOrOther)
                  }
                >
                  <div className="col-6 pl-4">
                    <label className="voda-bold w-100">
                      Other HW Info
                      <input
                        type="text"
                        onChange={(e) => onChange("otherHardwareInfo", e)}
                        onKeyUp={(e) => onChange("otherHardwareInfo", e)}
                        className="inputForm w-100"
                        value={formData?.otherHardwareInfo ?? ""}
                      />
                    </label>
                  </div>
                </Container>
                <div className="col-12">
                  <div className="form-group">
                    <label className="labelForm voda-bold mb-0 w-100">
                      Description
                      <div className="d-flex">
                        <input
                          type="text"
                          maxLength={2000}
                          onChange={(e) => onChange("description", e)}
                          onKeyUp={(e) => onChange("description", e)}
                          className="inputForm w-100"
                          value={formData?.description!}
                        />
                      </div>
                    </label>
                  </div>
                </div>
                <div className="col-12 p-0">
                  <div className="row">
                    <div className="col-6 pr-0">
                      {props.edit ? (
                        <div className="col-12">
                          <label className="labelForm voda-bold  w-100">
                            Last Modified
                            <input
                              readOnly={true}
                              className="inputForm w-100 voda-regular"
                              type="text"
                              value={formatDateWithTime(formData?.lastModified)}
                            />
                          </label>
                        </div>
                      ) : null}
                    </div>
                    <div className="col-6 pl-0">
                      {props.edit ? (
                        <div className="col-12">
                          <label className="labelForm voda-bold  w-100 pr-2">
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
              </div>
            </fieldset>
          </Container>
          {/* //WIZARD */}
          <Container show={props.wizardMode}>
            <fieldset className="fieldset">
              <div className="row">
                <button
                  type="button"
                  className="further-btn mb-20"
                  onClick={() =>
                    setIsVisibleFurtherDetails(!isVisibleFurtherDetails)
                  }
                >
                  Add Further Details?
                </button>
                {isVisibleFurtherDetails && (
                  <div className="col-12 d-flex row">
                    <div className="col-6 pl-0">
                      <div className="col-12 pl-0 pr-0">
                        <label className="labelForm voda-bold w-100">
                          Vulnerability Status
                          <input
                            type="text"
                            onChange={(e) => onChange("vulnerabilityStatus", e)}
                            onKeyUp={(e) => onChange("vulnerabilityStatus", e)}
                            className="inputForm w-100"
                            value={
                              formData?.vulnerabilityStatus &&
                              formData?.vulnerabilityStatus != undefined
                                ? formData?.vulnerabilityStatus
                                : ""
                            }
                          />
                        </label>
                      </div>

                      <Container show={props.wizardMode}>
                        <div className="row">
                          {proprietaryHardware ||
                            (cotsOrOther && (
                              <div className="col-6 form-group">
                                <label className="voda-bold w-100 pr-2">
                                  Other HW Info
                                  <input
                                    type="text"
                                    onChange={(e) =>
                                      onChange("otherHardwareInfo", e)
                                    }
                                    onKeyUp={(e) =>
                                      onChange("otherHardwareInfo", e)
                                    }
                                    className="inputForm w-100"
                                    value={formData?.otherHardwareInfo ?? ""}
                                  />
                                </label>
                              </div>
                            ))}

                          {proprietaryHardware && (
                            <div className="col-12">
                              <label className="labelForm voda-bold  w-100">
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
                                  className="inputForm w-100 "
                                  minDate={new Date(1980, 0, 1)}
                                  maxDate={new Date(2999, 0, 1)}
                                  dateFormat="dd/MM/yyyy"
                                  placeholderText={"NOT SPECIFIED"}
                                />
                              </label>
                            </div>
                          )}
                        </div>
                      </Container>
                    </div>
                    <div className="col-6 pl-0 pr-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold w-100">
                          General Availability Date
                          <DatePicker
                            selected={
                              formData?.generaAvailableDate &&
                              new Date(formData?.generaAvailableDate)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate("generaAvailableDate", newDate);
                            }}
                            className="inputForm w-100"
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                        </label>
                      </div>
                    </div>

                    {proprietaryHardware && (
                      <div className="row">
                        <div className="col-6">
                          <label className=" labelForm voda-bold  w-100">
                            Last time buy - new
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyNew &&
                                new Date(formData?.lastTimeBuyNew)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyNew", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>
                        <div className="col-6">
                          <label className="labelForm voda-bold  w-100">
                            Last time buy - upgrades
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyUpgrades &&
                                new Date(formData?.lastTimeBuyUpgrades)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyUpgrades", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>
                        <div className="col-6">
                          <label className="labelForm voda-bold  w-100">
                            Last time buy - expansions
                            <DatePicker
                              selected={
                                formData?.lastTimeBuyExpansions &&
                                new Date(formData?.lastTimeBuyExpansions)
                              }
                              onChange={(newDate, e) => {
                                e.preventDefault();
                                onChangeDate("lastTimeBuyExpansions", newDate);
                              }}
                              className="inputForm w-100 "
                              minDate={new Date(1980, 0, 1)}
                              maxDate={new Date(2999, 0, 1)}
                              dateFormat="dd/MM/yyyy"
                              placeholderText={"NOT SPECIFIED"}
                            />
                          </label>
                        </div>
                      </div>
                    )}
                    <div className="col-12 pl-0 pr-0">
                      <div className="form-group">
                        <label className="labelForm voda-bold mb-0 w-100">
                          Description
                          <div className="d-flex">
                            <input
                              type="text"
                              onChange={(e) => onChange("description", e)}
                              onKeyUp={(e) => onChange("description", e)}
                              className="inputForm w-100"
                              value={formData?.description!}
                            />
                          </div>
                        </label>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            </fieldset>
          </Container>
        </div>
      </form>
      <Container show={!props.wizardMode}>
        <div className="col-12 justify-content-end  pr-4 d-flex footerModal">
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => {
              props.action.closeModal && props.action.closeModal(changed);
              onChangeBuildConstruction(undefined, true, "buildConstructionId");
            }}
            type="button"
          >
            Cancel
          </button>
          <button
            className={` voda-bold btn btn-danger px-4 btnHeader ${
              props.prevPage === "generatelcmdb" &&
              props.hardwareRedirect === true
                ? "disabledCursor"
                : ""
            }`}
            onClick={() =>
              Save(
                formData,
                props.edit,
                validazioneClient,
                refresh,
                RestoreOrphanDeleted,
                orphanDeleted
              )
            }
            type="button"
            data-toggle="tooltip"
            data-placement="top"
            title={
              props.prevPage === "generatelcmdb" &&
              props.hardwareRedirect === true
                ? `Saving is disabled due to redirection from LCM Export screen`
                : ""
            }
            disabled={
              props.prevPage === "generatelcmdb" &&
              props.hardwareRedirect === true
                ? true
                : false
            }
            id="majorHwAddModal_submit_tour"
          >
            Submit
          </button>
        </div>
      </Container>
      <Container show={props.wizardMode}>
        <div className="d-flex justify-content-between">
          <button
            className=" voda-bold btn btn-link px-4 btnHeader cancel"
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
              className="voda-bold btn btn-link px-4 btnHeader cancel"
              type="button"
              onClick={() =>
                props.action.wizardBackFunction &&
                formData &&
                props.action.wizardBackFunction(
                  formData,
                  "majorHardwareBuildDto"
                )
              }
            >
              Back
            </button>
            <button
              className="voda-bold btn btn-danger px-4 btnHeader"
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
          tourSteps={getMajorHwModalTourSteps}
          page={"majorHwAddModal"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default MajorHardwareModal;
