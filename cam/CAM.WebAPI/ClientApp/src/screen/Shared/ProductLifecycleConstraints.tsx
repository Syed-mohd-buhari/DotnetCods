import React, { useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import DatePicker from "react-datepicker";
import Select from "react-select";
import ModalConfirm from "../../Components/ModalConfirm";
import { paginationQuery } from "../../Containers/SystemTypeContainer";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/productLifecycleConstraints.css";
import {
  AddMonth,
  changeDate,
  changeText,
  subtractMonths,
} from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import { MajorHardwareBuildDtoUpdate } from "../../Model/MajorHardwareBuild";
import { MajorSoftwareBuildDtoUpdate } from "../../Model/MajorSoftwareBuild";
import { LifecycleConstraintDto } from "../../Model/SystemTypeModel";
import { GetRuleFromBuildCostruction } from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCommonAction";
import { GetMajorHardwareBuildEditResource } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildEditAction";
import { GetMajorSoftwareBuildEditResource } from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildEditAction";
import { setNotification } from "../../Redux/Action/NotificationAction";
import {
  GetProductLifecycleConstraintInfo,
  SaveProductLifecycleConstraint,
} from "../../Redux/Action/ProductLifecycleConstraint/ProductLifecycleConstraintCommonAction";
import { GetSystemTypeCreateResource } from "../../Redux/Action/SystemType/SystemTypeCreateAction";
import { GetSystemTypeEditResource } from "../../Redux/Action/SystemType/SystemTypeEditAction";
import { GetSystemTypeGrid } from "../../Redux/Action/SystemType/SystemTypeGridAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { rootStore } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    setIsVisibleModalProductLifecycle(val: boolean): any;
  };
}

const ProductLifecycleConstraints: React.FC<Props> = (props) => {
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [changed, setChanged] = useState<boolean>(false);
  const { readonly, isPermesso } = useAuth();
  const [validationSw, setValidationSw] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [validationHw, setValidationHw] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [enabledInput, setEnabledInput] = useState<string[]>([]);

  const [formData, setFormData] = useState<LifecycleConstraintDto>();
  const [formDataRestore, setFormDataRestore] = useState<string>();
  const [dataSaved, setDataSaved] = useState<boolean>(false);
  const [disabledDateSw, setDisabledDateSw] = useState<boolean>(false);
  const [disabledDateHw, setDisabledDateHw] = useState<boolean>(false);

  const toggleInputEnable = (input: string) => {
    let copy = [...enabledInput] as string[];

    if (
      formData?.majorSoftwareBuildDto?.eomStatus === 0 &&
      input === "endOfMaintenanceSw"
    ) {
      onHandelChangeAnnounced(true, "endOfMaintenanceSw");
    }
    if (copy.includes(input)) {
      let indexInput = copy.indexOf(input);
      copy.splice(indexInput, 1);
      setEnabledInput(copy);
    } else {
      copy.push(input);
      setEnabledInput(copy);
    }
  };

  const onHandelChangeAnnounced = (e: boolean, type: string) => {
    let copyForm = { ...formData } as LifecycleConstraintDto;

    if (e && type === "endOfMaintenanceSw") {
      setDisabledDateSw(true);
      if (copyForm.majorSoftwareBuildDto != undefined) {
        copyForm["majorSoftwareBuildDto"].eomStatus = 0;
        copyForm["majorSoftwareBuildDto"].endOfMaintenance = null;
        setFormData(copyForm);
      }
    }
    if (!e && type === "endOfMaintenanceSw") {
      setDisabledDateSw(false);
      if (copyForm.majorSoftwareBuildDto != undefined) {
        copyForm["majorSoftwareBuildDto"].eomStatus = 1;
        setFormData(copyForm);
      }
      setFormData(copyForm);
    }

    if (e && type === "endOfMaintenanceHww") {
      setDisabledDateHw(true);
      if (copyForm.majorHardwareBuildDto != undefined) {
        copyForm["majorHardwareBuildDto"].eomStatus = 0;
        copyForm["majorHardwareBuildDto"].endOfMaintenance = null;
        setFormData(copyForm);
      }
    }

    if (!e && type === "endOfMaintenanceHww") {
      setDisabledDateHw(false);
      if (copyForm.majorHardwareBuildDto != undefined) {
        copyForm["majorHardwareBuildDto"].eomStatus = 1;
        setFormData(copyForm);
      }
    }
  };

  //ONCHANGE FORM
  const onChangeSoftware = (property: string, date: Date | null) => {
    let copy = { ...formData } as LifecycleConstraintDto;
    if (copy.majorSoftwareBuildDto != undefined) {
      if (property === "endOfMaintenance") {
        if (date !== null) {
          copy.majorSoftwareBuildDto.eomStatus = 2;
        } else {
          copy.majorSoftwareBuildDto.eomStatus = 1;
        }
      }
      copy.majorSoftwareBuildDto[property] = date;
      setChanged(true);
      setFormData(copy);

      if (validationSw?.property?.includes(property)) {
        let copy = { ...validationSw, property: [...validationSw.property] };
        let idxOfProperty = copy.property.indexOf(property);
        copy.property.splice(idxOfProperty, 1);
        setValidationSw(copy);
      }
    }
  };

  const onChangeHardware = (property: string, date: Date | null) => {
    let copy = { ...formData } as LifecycleConstraintDto;

    if (copy.majorHardwareBuildDto != undefined) {
      if (property === "endOfMaintenance") {
        if (date !== null) {
          copy.majorHardwareBuildDto.eomStatus = 2;
        } else {
          copy.majorHardwareBuildDto.eomStatus = 1;
        }
      }
      // if (value != null && value != undefined && value != "") {
      copy.majorHardwareBuildDto[property] = date;
      setChanged(true);
      setFormData(copy);
      // setValidation({ response: true });
      // }
    }

    if (validationHw?.property?.includes(property)) {
      let copy = { ...validationHw, property: [...validationHw.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidationHw(copy);
    }
  };

  const onChangeSystemType = async (e: any, property?: string) => {
    let copy = { ...formData } as LifecycleConstraintDto;
    if (e && e["key"]) {
      await GetSystemTypeEditResource(e["key"]).then((x) => {
        if (x.SystemTypeDtoEdit) {
          copy.systemTypeDto = x.SystemTypeDtoEdit;
          if (
            copy.systemTypeDto != undefined &&
            copy.systemTypeDto.systemTypeId != undefined
          ) {
            let main = copy.systemTypeDto.majorHardwareBuildId.find(
              (x) => x.isMain == true
            );
            GetMajorResource(
              copy,
              copy.systemTypeDto.majorSoftwareBuildsId,
              main?.majorHardwareBuildId
            );
            return;
          }
        }
      });
    } else {
      await GetSystemTypeCreateResource().then((x) => {
        if (x) {
          copy.systemTypeDto = x;
          copy.majorHardwareBuildDto = undefined;
          copy.majorSoftwareBuildDto = undefined;
          setProprietaryHardware(false);
          setDatesGenerated(false);
        }
        setFormDataRestore("");
      });
    }
    setEnabledInput([]);
    setFormData(copy);

    if (property && validationSw?.property?.includes(property)) {
      let copyValidationSw = {
        ...validationSw,
        property: [...validationSw.property],
      };
      let idxOfProperty = copyValidationSw.property.indexOf(property);
      copyValidationSw.property.splice(idxOfProperty, 1);
      setValidationSw(copyValidationSw);
    }
  };

  //RISORSE MAJOR HW/SW
  const GetMajorResource = async (
    copy: LifecycleConstraintDto,
    sw?: number,
    hw?: number
  ) => {
    if (sw && sw != undefined) {
      await GetMajorSoftwareBuildEditResource(sw).then((x) => {
        if (x && x != undefined) {
          copy.majorSoftwareBuildDto = x;
        }
      });
    }
    if (hw && hw != undefined) {
      await GetMajorHardwareBuildEditResource(hw).then((x) => {
        if (x && x != undefined) {
          copy.majorHardwareBuildDto = x;
        }
      });
    }
    setFormData(copy);
    setFormDataRestore(JSON.stringify(copy));
  };

  //RESET DATE / INVALID DATE CONSTRAINT SCALING
  useEffect(() => {
    // if (
    //   formData &&
    //   formData.systemTypeDto &&
    //   formData.systemTypeDto.constraintScaling != undefined
    // ) {
    //   changeDate("constraintScaling", formData.systemTypeDto.constraintScaling);
    // } else {
    //   changeText("constraintScaling", undefined);
    // }
  }, [formData?.systemTypeDto?.constraintScaling]);

  //RISORSE SELECT SYSTEMTYPE
  const [resourceSystemType, setResourceSystemType] = useState<
    { key: number; value: string }[] | undefined
  >([]);

  useEffect(() => {
    if (resourceSystemType?.length == 0) {
    } else {
    }
  }, [resourceSystemType]);

  useEffect(() => {
    GetSystemTypeResource();
  }, []);

  const GetSystemTypeResource = async () => {
    await GetSystemTypeGrid(undefined, true).then((x) => {
      if (x && x != undefined) {
        let resource = x.map((s) => {
          return { key: s.systemTypeId, value: s.systemSolution ?? " " } as {
            key: number;
            value: string;
          };
        });
        setResourceSystemType(resource);
      }
    });
  };

  //LOGICA DATE SOFTWARE
  const [datesGenerated, setDatesGenerated] = useState<boolean>(false);
  const [checkDeliveryMethod, setCheckDeliveryMethod] =
    useState<boolean>(false);

  useEffect(() => {
    if (
      formData &&
      (formData.majorSoftwareBuildDto?.deliveryMethod === "CI/CD" ||
        formData.majorSoftwareBuildDto?.deliveryMethod === "OneTrack")
    ) {
      setCheckDeliveryMethod(true);
    } else {
      setCheckDeliveryMethod(false);
    }
  }, [formData && formData.majorSoftwareBuildDto?.deliveryMethod]);

  //     useEffect(() => {
  //     onChangeEOM();
  // }, [
  //   checkDeliveryMethod,
  //   formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId,
  //   formData?.majorSoftwareBuildDto?.endOfMaintenance,
  // ]);

  // const onChangeEOM = () => {
  //   if (formData && formData.majorSoftwareBuildDto !== null) {
  //     let deliveryString =
  //       formData?.majorSoftwareBuildDto
  //         ?.originalEquipmentManufacturerResource &&
  //       dictionaryToArray(
  //         formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerResource
  //       ).find(
  //         (x) =>
  //           x.key ===
  //           formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId
  //       )?.value;
  //     if (
  //       formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId !==
  //         null &&
  //       deliveryString?.toLowerCase().includes("ericsson") &&
  //       checkDeliveryMethod
  //     ) {
  //       let copy = { ...formData } as LifecycleConstraintDto;

  //       if (copy.majorSoftwareBuildDto)
  //         copy.majorSoftwareBuildDto.deliveryMethod = "OneTrack";

  //       setDatesGenerated(true);
  //       if (
  //         copy.majorSoftwareBuildDto &&
  //         copy.majorSoftwareBuildDto.endOfMaintenance
  //       ) {
  //     let generaAvailableDate = subtractMonths(new Date(copy.majorSoftwareBuildDto.endOfMaintenance), 18);
  //     let endOfsupport = AddMonth(new Date(copy.majorSoftwareBuildDto.endOfMaintenance), 12);
  //     copy.majorSoftwareBuildDto.generaAvailableDate = generaAvailableDate;
  //     copy.majorSoftwareBuildDto.endOfsupport = endOfsupport;
  //       }
  //       setFormData(copy);
  //     } else if (checkDeliveryMethod === true) {
  //       let copy = { ...formData } as LifecycleConstraintDto;
  //       if (copy.majorSoftwareBuildDto)
  //         copy.majorSoftwareBuildDto.deliveryMethod = "CI/CD";

  //       setDatesGenerated(false);
  //       setFormData(copy);
  //     }
  //   }
  // };

  useEffect(() => {
    if (formData && formData.majorSoftwareBuildDto != null) {
      rtnDeliveryMethodAndDates();
    }
  }, [
    checkDeliveryMethod,
    formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId,
    formData?.majorSoftwareBuildDto?.generaAvailableDate,
  ]);

  const rtnDeliveryMethodAndDates = () => {
    if (formData && formData.majorSoftwareBuildDto !== null) {
      let deliveryString =
        formData?.majorSoftwareBuildDto
          ?.originalEquipmentManufacturerResource &&
        dictionaryToArray(
          formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerResource
        ).find(
          (x) =>
            x.key ===
            formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId
        )?.value;
      if (
        formData?.majorSoftwareBuildDto?.originalEquipmentManufacturerId !==
          null &&
        deliveryString?.toLowerCase().includes("ericsson") &&
        checkDeliveryMethod
      ) {
        let copy = { ...formData } as LifecycleConstraintDto;

        if (copy.majorSoftwareBuildDto)
          copy.majorSoftwareBuildDto.deliveryMethod = "OneTrack";

        setDatesGenerated(true);
        if (
          copy.majorSoftwareBuildDto &&
          copy.majorSoftwareBuildDto.generaAvailableDate
        ) {
          let endOfMaintenance = AddMonth(
            new Date(copy.majorSoftwareBuildDto.generaAvailableDate),
            18
          );
          let endOfsupport = AddMonth(
            new Date(copy.majorSoftwareBuildDto.generaAvailableDate),
            30
          );
          copy.majorSoftwareBuildDto.endOfMaintenance = new Date(
            endOfMaintenance
          );
          copy.majorSoftwareBuildDto.endOfsupport = new Date(endOfsupport);
        }
        setFormData(copy);
      } else if (checkDeliveryMethod === true) {
        let copy = { ...formData } as LifecycleConstraintDto;
        if (copy.majorSoftwareBuildDto)
          copy.majorSoftwareBuildDto.deliveryMethod = "CI/CD";

        setDatesGenerated(false);
        setFormData(copy);
      }
    }
  };

  // //RESET DATE / INVALID DATE SOFTWARE
  useEffect(() => {
    if (
      formData &&
      formData.majorSoftwareBuildDto &&
      formData.majorSoftwareBuildDto.lastTimeBuyNew != undefined
    ) {
      changeDate(
        "lastTimeBuyNewSw",
        formData.majorSoftwareBuildDto.lastTimeBuyNew
      );
    } else {
      changeText("lastTimeBuyNewSw", undefined);
    }

    if (
      formData &&
      formData.majorSoftwareBuildDto &&
      formData.majorSoftwareBuildDto.endOfsupport != undefined
    ) {
      changeDate("endOfsupportSw", formData.majorSoftwareBuildDto.endOfsupport);
    } else {
      changeText("endOfsupportSw", undefined);
    }

    if (
      formData &&
      formData.majorSoftwareBuildDto &&
      formData.majorSoftwareBuildDto.endOfMaintenance != undefined
    ) {
      changeDate(
        "endOfMaintenanceSw",
        formData.majorSoftwareBuildDto.endOfMaintenance
      );
    } else {
      changeText("endOfMaintenanceSw", undefined);
    }

    if (
      formData &&
      formData.majorSoftwareBuildDto &&
      formData.majorSoftwareBuildDto.generaAvailableDate != undefined
    ) {
      changeDate(
        "generaAvailableDateSw",
        formData.majorSoftwareBuildDto.generaAvailableDate
      );
    } else {
      changeText("generaAvailableDateSw", undefined);
    }
  }, [formData?.majorSoftwareBuildDto]);

  //LOGICA DATE HARDWARE
  const [proprietaryHardware, setProprietaryHardware] =
    useState<boolean>(false);

  useEffect(() => {
    onChangeBuildConstruction();
  }, [formData?.majorHardwareBuildDto?.buildConstructionId]);

  const onChangeBuildConstruction = async () => {
    await GetRuleFromBuildCostruction(
      formData?.majorHardwareBuildDto?.buildConstructionId ?? 0
    ).then((r) => {
      switch (r) {
        case 0:
        case 2:
        case 3:
          //NO RULES
          //COTS or OTHER
          //NFVI
          setProprietaryHardware(false);
          break;
        case 1:
          //PROPRIETARY
          setProprietaryHardware(true);
          break;

        default:
          break;
      }
    });
  };

  //RESET DATE / INVALID DATE HARDWARE
  useEffect(() => {
    if (
      formData &&
      formData.majorHardwareBuildDto &&
      formData.majorHardwareBuildDto?.lastTimeBuyNew != undefined
    ) {
      changeDate(
        "lastTimeBuyNewHw",
        formData?.majorHardwareBuildDto?.lastTimeBuyNew
      );
    } else {
      changeText("lastTimeBuyNewHw", undefined);
    }

    if (
      formData &&
      formData.majorHardwareBuildDto &&
      formData.majorHardwareBuildDto?.lastTimeBuyExpansions != undefined
    ) {
      changeDate(
        "lastTimeBuyExpansionsHw",
        formData?.majorHardwareBuildDto?.lastTimeBuyExpansions
      );
    } else {
      changeText("lastTimeBuyExpansionsHw", undefined);
    }

    if (
      formData &&
      formData.majorHardwareBuildDto &&
      formData.majorHardwareBuildDto?.lastTimeBuyUpgrades != undefined
    ) {
      changeDate(
        "lastTimeBuyUpgradesHw",
        formData?.majorHardwareBuildDto?.lastTimeBuyUpgrades
      );
    } else {
      changeText("lastTimeBuyUpgradesHw", undefined);
    }

    if (
      formData &&
      formData.majorHardwareBuildDto &&
      formData.majorHardwareBuildDto?.endOfsupport != undefined
    ) {
      changeDate(
        "endOfsupportHw",
        formData?.majorHardwareBuildDto?.endOfsupport
      );
    } else {
      changeText("endOfsupportHw", undefined);
    }

    if (
      formData &&
      formData.majorHardwareBuildDto &&
      formData.majorHardwareBuildDto?.endOfMaintenance != undefined
    ) {
      changeDate(
        "endOfMaintenanceHww",
        formData?.majorHardwareBuildDto?.endOfMaintenance
      );
    } else {
      changeText("endOfMaintenanceHww", undefined);
    }
  }, [formData?.majorHardwareBuildDto, proprietaryHardware]);

  //FUNZIONI FONDO PAGINA

  const cancelConfirm = async () => {
    const cancel = {
      title: "Warning",
      button: "Do Nothing",
      message: "Are you sure you want to quit? Unsaved changes will be lost",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => refrehSystemTypeGrid(),
        confirm: () => setConfirm(stateConfirm),
      },
    };
    if (changed) {
      setConfirm(cancel);
    } else {
      refrehSystemTypeGrid();
    }
  };

  const refrehSystemTypeGrid = async () => {
    await GetSystemTypeGrid(paginationQuery).then((x) => {
      props.action.setIsVisibleModalProductLifecycle(false);
    });
  };

  const Submit = async (data?: LifecycleConstraintDto) => {
    // if (data?.systemTypeDto?.constraintScaling) {
    //   data.systemTypeDto.constraintScaling = new Date(
    //     data?.systemTypeDto?.constraintScaling
    //   );
    // }

    if (!data) {
      setValidationSw({ response: false, property: ["systemTypeId"] });
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
    }

    if (data && data.majorSoftwareBuildDto && data.majorHardwareBuildDto) {
      if (
        validazioneClient(
          data.majorSoftwareBuildDto,
          data.majorHardwareBuildDto
        ).response
      ) {
        await SaveProductLifecycleConstraint(data).then((x) => {
          if (x && !x.warning) {
            setDataSaved(true);
            setFormData(data);
            AfterSaveConfirm();
            rootStore.dispatch({ type: "REFRESH", payload: true });
            rootStore.dispatch({ type: "REFRESH", payload: false });
          }
        });
      } else {
        rootStore.dispatch(
          setNotification({
            message: "Check the fields entered",
            notifyType: NotifyType.warning,
          })
        );
      }
    }
  };

  const GetConstraintFromNewDate = async () => {
    if (formData && formData != undefined) {
      await GetProductLifecycleConstraintInfo(formData).then((x) => {
        let copy = { ...formData } as LifecycleConstraintDto;
        if (x && x != undefined) {
          if (copy.systemTypeDto != undefined) {
            copy.systemTypeDto.lcmStatus = x.lcmStatus;
            copy.systemTypeDto.constraintScaling = x.constraintScaling;
            copy.systemTypeDto.constraintLcm = x.constraintLcm;
            setFormData(copy);
            rootStore.dispatch(
              setNotification({
                message: "Constraints calculated",
                notifyType: NotifyType.success,
              })
            );
          }
        }
      });
    }
  };

  const AfterSaveConfirm = () => {
    const afterSave: DataModalConfirm = {
      title: "",
      button: "Yes",
      cancelText: "No",
      message: "Do you want to update another product?",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => {
          setConfirm(stateConfirm);
          props.action.setIsVisibleModalProductLifecycle(false);
        },
        confirm: () => RefreshModal(),
      },
    };

    setConfirm(afterSave);
  };

  const RestoreDataConfirm = () => {
    const restore: DataModalConfirm = {
      title: "Restore",
      button: "Restore",
      message:
        "Do you want to restore data to previous version? All changes will be lost",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => RestoreData(),
      },
    };
    setConfirm(restore);
  };

  const RestoreData = async () => {
    if (dataSaved) {
      formDataRestore && Submit(JSON.parse(formDataRestore));
      setDataSaved(false);
    } else {
      formDataRestore && setFormData(JSON.parse(formDataRestore));
      setEnabledInput([]);
      setDataSaved(false);
      setConfirm(stateConfirm);
      setValidationSw({ response: true });
      setValidationHw({ response: true });
    }
  };

  const RefreshModal = async () => {
    let copy = { ...formData } as LifecycleConstraintDto;
    await GetSystemTypeCreateResource().then((x) => {
      if (x) {
        copy.systemTypeDto = x;
        copy.majorHardwareBuildDto = undefined;
        copy.majorSoftwareBuildDto = undefined;
        setProprietaryHardware(false);
        setDatesGenerated(false);
      }
      setConfirm(stateConfirm);
      setFormData(copy);
      setDataSaved(false);
      setChanged(false);
      setFormDataRestore("");
    });
  };

  //VALIDAZIONEEEE
  const validazioneClient = (
    copySoftw: MajorSoftwareBuildDtoUpdate,
    copyHardw: MajorHardwareBuildDtoUpdate
  ) => {
    let copyValidationSw = { response: true, property: [] } as CommonValidation;
    let copyValidationHw = { response: true, property: [] } as CommonValidation;
    const addInvalidPropertySw = (property: string) => {
      copyValidationSw?.property?.push(property);
      copyValidationSw.response = false;
    };
    const addInvalidPropertyHw = (property: string) => {
      copyValidationHw?.property?.push(property);
      copyValidationHw.response = false;
    };
    if (
      formData?.systemTypeDto?.systemTypeId === undefined ||
      formData?.systemTypeDto?.systemTypeId === null ||
      formData?.systemTypeDto?.systemTypeId === 0
    ) {
      addInvalidPropertySw("systemTypeId");
    }
    // if (
    //   (copySoftw?.generaAvailableDate == null ||
    //     copySoftw?.generaAvailableDate === undefined ||
    //     copySoftw?.generaAvailableDate.toString() == "") &&
    //   datesGenerated
    // ) {
    //   addInvalidPropertySw("generaAvailableDate");
    // }
    // if (
    //   copySoftw?.generaAvailableDate == null ||
    //   copySoftw?.generaAvailableDate === undefined ||
    //   copySoftw?.generaAvailableDate.toString() == ""
    // ) {
    //   if (!copyValidationSw?.property?.includes("generaAvailableDate")) {
    //     addInvalidPropertySw("generaAvailableDate");
    //   }
    // }
    // if (
    //   copySoftw?.endOfMaintenance == null ||
    //   copySoftw?.endOfMaintenance === undefined ||
    //   copySoftw?.endOfMaintenance.toString() == ""
    // ) {
    //   addInvalidPropertySw("endOfMaintenance");
    // }
    // if (
    //   copyHardw?.endOfMaintenance == null ||
    //   copyHardw?.endOfMaintenance === undefined ||
    //   copyHardw?.endOfMaintenance.toString() == ""
    // ) {
    //   addInvalidPropertyHw("endOfMaintenance");
    // }
    setValidationSw(copyValidationSw);
    setValidationHw(copyValidationHw);
    return { response: copyValidationSw.response && copyValidationHw.response };
  };

  return (
    <div className="">
      <ModalConfirm data={confirm} />
      <div className="col-12">
        <div className="form-group col-6 pl-0">
          <label className="  w-100">
            <label className="mb-0 voda-bold">Please Select the Product</label>
            <div className="d-flex">
              <div className="w-100">
                <Select
                menuPosition={"fixed"}
                  options={resourceSystemType}
                  value={
                    formData &&
                    resourceSystemType &&
                    resourceSystemType?.filter(
                      (x) => x.key == formData?.systemTypeDto?.systemTypeId
                    )
                  }
                  onChange={(e) => onChangeSystemType(e, "systemTypeId")}
                  isSearchable
                  isClearable
                  // isDisabled={resourceSystemType?.length == 0}
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                  formatOptionLabel={function (data) {
                    return (
                      <span dangerouslySetInnerHTML={{ __html: data.value }} />
                    );
                  }}
                ></Select>
                {validationSw &&
                validationSw.response == false &&
                validationSw.property?.includes("systemTypeId") ? (
                  <label className="validation">
                    *you must select a product
                  </label>
                ) : null}
              </div>
            </div>
          </label>
        </div>
      </div>

      {/*-------------- Software Lifecycle Management Constraints ------------*/}

      <form id="formSoftwareBuild">
        <div className="row col-12 px-0 mx-0">
          <fieldset className="fieldset">
            <label className="text-bb">
              Software Lifecycle Management Constraints
            </label>
            <div className="row">
              <div className="col-12 px-2 pl-0">
                <div className="form-group col-6">
                  <label className="voda-bold w-100">
                    Equipment Manufacturer<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData &&
                            formData?.majorSoftwareBuildDto &&
                            formData?.majorSoftwareBuildDto
                              ?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.majorSoftwareBuildDto
                                ?.originalEquipmentManufacturerResource
                            )
                          }
                          value={
                            formData?.majorSoftwareBuildDto
                              ?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.majorSoftwareBuildDto
                                ?.originalEquipmentManufacturerResource
                            ).find(
                              (x) =>
                                x.key ==
                                formData.majorSoftwareBuildDto
                                  ?.originalEquipmentManufacturerId
                            )
                          }
                          // onChange={(e) => onChangeSoftware("originalEquipmentManufacturerId", e)}
                          isSearchable
                          isClearable
                          isDisabled
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                          formatOptionLabel={function (data) {
                            return (
                              <span
                                dangerouslySetInnerHTML={{ __html: data.value }}
                              />
                            );
                          }}
                        ></Select>
                      </div>
                    </div>
                  </label>
                </div>
              </div>
              <div className="col-6 px-2 pl-0">
                <div className="form-group col-12">
                  <span className="flex">
                    <span className="voda-bold fs-16">End Of Maintenance</span>
                    <Form.Check
                      type="checkbox"
                      className="radio  voda-bold  mr fs-15"
                      name="userLogin"
                      value="100"
                      label="Not Announced"
                      checked={
                        formData?.majorSoftwareBuildDto?.eomStatus === 0
                          ? true
                          : false
                      }
                      onChange={(e: any) =>
                        onHandelChangeAnnounced(
                          e.target.checked,
                          "endOfMaintenanceSw"
                        )
                      }
                      disabled={!enabledInput.includes("endOfMaintenanceSw")}
                    />
                  </span>
                  <label
                    className=" voda-bold  w-100"
                    onClick={(e) => e.preventDefault()}
                  >
                    <div className="d-flex">
                      <div
                        className={
                          !enabledInput.includes("endOfMaintenanceSw") ||
                          formData?.majorSoftwareBuildDto?.eomStatus === 0
                            ? "disabledDate w-100"
                            : "w-100"
                        }
                      >
                        <DatePicker
                          selected={
                            formData?.majorSoftwareBuildDto?.endOfMaintenance &&
                            new Date(
                              formData?.majorSoftwareBuildDto?.endOfMaintenance
                            )
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeSoftware("endOfMaintenance", newDate);
                          }}
                          className={
                            !enabledInput.includes("endOfMaintenanceSw") ||
                            datesGenerated ||
                            disabledDateSw
                              ? " disabledBackground inputForm w-100"
                              : "inputForm w-100 "
                          }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={
                            formData?.majorSoftwareBuildDto?.eomStatus === 0
                              ? "NOT ANNOUNCED"
                              : "NOT SPECIFIED"
                          }
                        />
                      </div>
                      {!readonly && (
                        <button
                          disabled={
                            datesGenerated ||
                            formData?.systemTypeDto?.systemTypeId == undefined
                          }
                          title={
                            datesGenerated
                              ? "The value will be auto-generated, edit the General Availability date"
                              : ""
                          }
                          type="button"
                          className=" voda-bold btn btn-link btn-icon  "
                          onClick={() =>
                            toggleInputEnable("endOfMaintenanceSw")
                          }
                        >
                          <img
                            className="btnEdit"
                            src={require("../../img/squareArrow.png")}
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {/* {validationSw &&
                  validationSw.response == false &&
                  validationSw.property?.includes("endOfMaintenance") ? (
                    <label className="validation">
                      *End of Maintenance must have a value
                    </label>
                  ) : null} */}
                </div>
                <div
                  className="form-group col-12"
                  onClick={(e) => e.preventDefault()}
                >
                  <label className="voda-bold w-100">
                    General Availability
                    <div className="d-flex">
                      <div
                        className={
                          !enabledInput.includes("generaAvailableDateSw")
                            ? "disabledDate w-100"
                            : "w-100"
                        }
                      >
                        <DatePicker
                          selected={
                            formData?.majorSoftwareBuildDto
                              ?.generaAvailableDate &&
                            new Date(
                              formData?.majorSoftwareBuildDto?.generaAvailableDate
                            )
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeSoftware("generaAvailableDate", newDate);
                          }}
                          className={
                            !enabledInput.includes("generaAvailableDateSw")
                              ? " disabledBackground inputForm w-100"
                              : "inputForm w-100 "
                          }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </div>
                      {!readonly && (
                        <button
                          disabled={
                            formData?.systemTypeDto?.systemTypeId == undefined
                          }
                          type="button"
                          className=" voda-bold btn btn-link btn-icon"
                          onClick={() =>
                            toggleInputEnable("generaAvailableDateSw")
                          }
                        >
                          <img
                            className="btnEdit"
                            src={require("../../img/squareArrow.png")}
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {/* {validationSw &&
                  validationSw.response == false &&
                  validationSw.property?.includes("generaAvailableDate") ? (
                    <label className="validation">
                      *General Availability date must have a value
                    </label>
                  ) : null} */}
                </div>
              </div>
              <div className="col-6 px-2 row mx-0">
                <div
                  className="form-group col-12 pr-0"
                  onClick={(e) => e.preventDefault()}
                >
                  <label className=" voda-bold w-100">
                    End Of Support
                    <div className="d-flex">
                      <div
                        className={
                          !enabledInput.includes("endOfsupportSw")
                            ? "disabledDate w-100"
                            : "w-100"
                        }
                      >
                        <DatePicker
                          selected={
                            formData?.majorSoftwareBuildDto?.endOfsupport &&
                            new Date(
                              formData?.majorSoftwareBuildDto?.endOfsupport
                            )
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeSoftware("endOfsupport", newDate);
                          }}
                          className={
                            !enabledInput.includes("endOfsupportSw") ||
                            datesGenerated
                              ? " disabledBackground inputForm w-100"
                              : "inputForm w-100 "
                          }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                          // readOnly={!enabledInput.includes("endOfsupportSw") || datesGenerated}
                        />
                      </div>
                      {!readonly && (
                        <button
                          disabled={
                            datesGenerated ||
                            formData?.systemTypeDto?.systemTypeId == undefined
                          }
                          title={
                            datesGenerated
                              ? "The value will be auto-generated, edit the General Availability date"
                              : ""
                          }
                          type="button"
                          className=" voda-bold btn btn-link btn-icon "
                          onClick={() => toggleInputEnable("endOfsupportSw")}
                        >
                          <img
                            className="btnEdit"
                            src={require("../../img/squareArrow.png")}
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {/* {validationSw && validationSw.response == false && validationSw.property?.includes("endOfsupport") ? <label className="validation">*End of Support must have a value</label> : null} */}
                </div>
                <div
                  className="form-group col-12 d-flex align-items-center pr-0"
                  onClick={(e) => e.preventDefault()}
                >
                  {/* <div className="form-group col-12 d-flex align-items-center"> */}
                  <label className=" voda-bold  w-100">
                    <div
                      className={
                        !enabledInput.includes("lastTimeBuyNewSw") ||
                        formData?.majorSoftwareBuildDto?.eomStatus === 0
                          ? "disabledDate w-100"
                          : "w-100"
                      }
                    >
                      Last Time Buy
                      <DatePicker
                        selected={
                          formData?.majorSoftwareBuildDto?.lastTimeBuyNew &&
                          new Date(
                            formData?.majorSoftwareBuildDto?.lastTimeBuyNew
                          )
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeSoftware("lastTimeBuyNew", newDate);
                        }}
                        className={
                          !enabledInput.includes("lastTimeBuyNewSw")
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                        // readOnly={!enabledInput.includes("lastTimeBuyNewSw")}
                      />
                    </div>
                  </label>
                  {!readonly && (
                    <button
                      disabled={
                        formData?.systemTypeDto?.systemTypeId == undefined
                      }
                      type="button"
                      className=" voda-bold btn btn-link btn-icon  "
                      onClick={() => toggleInputEnable("lastTimeBuyNewSw")}
                    >
                      <img
                        className="btnEdit"
                        src={require("../../img/squareArrow.png")}
                      />
                    </button>
                  )}
                </div>
              </div>
            </div>
          </fieldset>
        </div>
      </form>

      {/*-------------- Hardware Lifecycle Management Constraints ------------*/}

      <form id="formSoftwareBuild">
        <div className="row col-12 px-0 mx-0">
          <fieldset className="fieldset">
            <label className="text-bb">
              Hardware Lifecycle Management Constraints
            </label>
            <div className="row">
              <div className="col-6 row mx-0">
                <div className="form-group col-12 pl-0">
                  <label className=" voda-bold w-100">
                    Equipment Manufacturer<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.majorHardwareBuildDto
                              ?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.majorHardwareBuildDto
                                ?.originalEquipmentManufacturerResource
                            )
                          }
                          value={
                            formData?.majorHardwareBuildDto
                              ?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.majorHardwareBuildDto
                                ?.originalEquipmentManufacturerResource
                            ).find(
                              (x) =>
                                x.key ==
                                formData.majorHardwareBuildDto
                                  ?.originalEquipmentManufacturerId
                            )
                          }
                          // onChange={(e) => onChangeSoftware("originalEquipmentManufacturerId", e)}
                          isSearchable
                          isClearable
                          isDisabled
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                          formatOptionLabel={function (data) {
                            return (
                              <span
                                dangerouslySetInnerHTML={{ __html: data.value }}
                              />
                            );
                          }}
                        ></Select>
                      </div>
                    </div>
                  </label>
                </div>
                <div
                  className="form-group col-12 pl-0"
                  onClick={(e) => e.preventDefault()}
                >
                  <label className="labelForm voda-bold text-uppercase w-100">
                    General Availability
                    <div className="d-flex">
                      <div
                        className={
                          !enabledInput.includes("generaAvailableDateHw")
                            ? "disabledDate w-100"
                            : "w-100"
                        }
                      >
                        <DatePicker
                          selected={
                            formData?.majorHardwareBuildDto
                              ?.generaAvailableDate &&
                            new Date(
                              formData?.majorHardwareBuildDto?.generaAvailableDate
                            )
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeHardware("generaAvailableDate", newDate);
                          }}
                          className={
                            !enabledInput.includes("generaAvailableDateHw")
                              ? " disabledBackground inputForm w-100"
                              : "inputForm w-100 "
                          }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </div>
                      {!readonly && (
                        <button
                          disabled={
                            formData?.systemTypeDto?.systemTypeId == undefined
                          }
                          type="button"
                          className="text-uppercase voda-bold btn btn-link btn-icon"
                          onClick={() =>
                            toggleInputEnable("generaAvailableDateHw")
                          }
                        >
                          <img
                            className="btnEdit"
                            src={require("../../img/squareArrow.png")}
                          />
                        </button>
                      )}
                    </div>
                  </label>
                </div>
                <div className="form-group col-12 pl-0">
                  <span className="flex">
                    <span className="voda-bold fs-16 ">End Of Maintenance</span>
                    <Form.Check
                      type="checkbox"
                      className="radio voda-bold  mr fs-15"
                      name="userLogin"
                      value="100"
                      label="Not Announced"
                      checked={
                        formData?.majorHardwareBuildDto?.eomStatus === 0
                          ? true
                          : false
                      }
                      onChange={(e: any) =>
                        onHandelChangeAnnounced(
                          e.target.checked,
                          "endOfMaintenanceHww"
                        )
                      }
                      disabled={!enabledInput.includes("endOfMaintenanceHww")}
                    />
                  </span>
                  <label
                    className=" voda-bold  w-100"
                    onClick={(e) => e.preventDefault()}
                  >
                    <div className="d-flex">
                      <div
                        className={
                          !enabledInput.includes("endOfMaintenanceHww") ||
                          formData?.majorHardwareBuildDto?.eomStatus === 0
                            ? "disabledDate w-100"
                            : "w-100"
                        }
                      >
                        <DatePicker
                          selected={
                            formData?.majorHardwareBuildDto?.endOfMaintenance &&
                            new Date(
                              formData?.majorHardwareBuildDto?.endOfMaintenance
                            )
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeHardware("endOfMaintenance", newDate);
                          }}
                          className={
                            !enabledInput.includes("endOfMaintenanceHww") ||
                            disabledDateHw ||
                            datesGenerated
                              ? " disabledBackground inputForm w-100"
                              : "inputForm w-100 "
                          }
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={
                            formData?.majorHardwareBuildDto?.eomStatus === 0
                              ? "NOT ANNOUNCED"
                              : "NOT SPECIFIED"
                          }
                          // readOnly={!enabledInput.includes("endOfMaintenanceSw") || datesGenerated}
                        />
                      </div>
                      {!readonly && (
                        <button
                          disabled={
                            datesGenerated ||
                            formData?.systemTypeDto?.systemTypeId == undefined
                          }
                          title={
                            datesGenerated
                              ? "The value will be auto-generated, edit the General Availability date"
                              : ""
                          }
                          type="button"
                          className=" voda-bold btn btn-link btn-icon  "
                          onClick={() =>
                            toggleInputEnable("endOfMaintenanceHww")
                          }
                        >
                          <img
                            className="btnEdit"
                            src={require("../../img/squareArrow.png")}
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {/* {validationSw &&
                  validationSw.response == false &&
                  validationSw.property?.includes("endOfMaintenance") ? (
                    <label className="validation">
                      *End of Maintenance must have a value
                    </label>
                  ) : null} */}
                </div>

                {proprietaryHardware && (
                  <div
                    className="form-group col-12 d-flex align-items-center pl-0"
                    onClick={(e) => e.preventDefault()}
                  >
                    <label
                      className={
                        !enabledInput.includes("endOfsupportHw")
                          ? "disabledDate voda-bold w-100"
                          : " voda-bold w-100"
                      }
                    >
                      End of Support
                      <DatePicker
                        selected={
                          formData?.majorHardwareBuildDto?.endOfsupport &&
                          new Date(
                            formData?.majorHardwareBuildDto?.endOfsupport
                          )
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeHardware("endOfsupport", newDate);
                        }}
                        className={
                          !enabledInput.includes("endOfsupportHw")
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                        // readOnly={!enabledInput.includes("endOfsupportHw")}
                      />
                    </label>
                    {!readonly && (
                      <button
                        disabled={
                          formData?.systemTypeDto?.systemTypeId == undefined
                        }
                        type="button"
                        className=" voda-bold btn btn-link btn-icon "
                        onClick={() => toggleInputEnable("endOfsupportHw")}
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/squareArrow.png")}
                        />
                      </button>
                    )}
                  </div>
                )}
              </div>
              {proprietaryHardware && (
                <div className="col-6 px-2 mx-0">
                  <div className="form-group col-12 d-flex">
                    <label
                      className={
                        !enabledInput.includes("lastTimeBuyNewHw")
                          ? "disabledDate  voda-bold  w-100"
                          : " voda-bold  w-100"
                      }
                    >
                      Last Time Buy - New
                      <DatePicker
                        selected={
                          formData?.majorHardwareBuildDto?.lastTimeBuyNew &&
                          new Date(
                            formData?.majorHardwareBuildDto?.lastTimeBuyNew
                          )
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeHardware("lastTimeBuyNew", newDate);
                        }}
                        className={
                          !enabledInput.includes("lastTimeBuyNewHw")
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                        // readOnly={!enabledInput.includes("lastTimeBuyNewHw")}
                      />
                    </label>
                    {!readonly && (
                      <button
                        disabled={
                          formData?.systemTypeDto?.systemTypeId == undefined
                        }
                        type="button"
                        className="text-uppercase voda-bold btn btn-link btn-icon"
                        onClick={() => toggleInputEnable("lastTimeBuyNewHw")}
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/squareArrow.png")}
                        />
                      </button>
                    )}
                  </div>

                  <div className="form-group col-12 d-flex">
                    <label
                      className={
                        !enabledInput.includes("lastTimeBuyUpgradesHw")
                          ? "disabledDate  voda-bold  w-100"
                          : " voda-bold  w-100"
                      }
                    >
                      Last Time Buy - Upgrade
                      <DatePicker
                        selected={
                          formData?.majorHardwareBuildDto
                            ?.lastTimeBuyUpgrades &&
                          new Date(
                            formData?.majorHardwareBuildDto?.lastTimeBuyUpgrades
                          )
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeHardware("lastTimeBuyUpgrades", newDate);
                        }}
                        className={
                          !enabledInput.includes("lastTimeBuyUpgradesHw")
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                        // readOnly={!enabledInput.includes("lastTimeBuyUpgradesHw")}
                      />
                    </label>
                    {!readonly && (
                      <button
                        disabled={
                          formData?.systemTypeDto?.systemTypeId == undefined
                        }
                        type="button"
                        className="text-uppercase voda-bold btn btn-link btn-icon"
                        onClick={() =>
                          toggleInputEnable("lastTimeBuyUpgradesHw")
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/squareArrow.png")}
                        />
                      </button>
                    )}
                  </div>

                  <div className="form-group col-12 d-flex">
                    <label
                      className={
                        !enabledInput.includes("lastTimeBuyExpansionsHw")
                          ? "disabledDate  voda-bold  w-100"
                          : " voda-bold  w-100"
                      }
                    >
                      Last Time Buy - Exspansion
                      <DatePicker
                        selected={
                          formData?.majorHardwareBuildDto
                            ?.lastTimeBuyExpansions &&
                          new Date(
                            formData?.majorHardwareBuildDto?.lastTimeBuyExpansions
                          )
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeHardware("lastTimeBuyExpansions", newDate);
                        }}
                        className={
                          !enabledInput.includes("lastTimeBuyExpansionsHw")
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={"NOT SPECIFIED"}
                        // readOnly={!enabledInput.includes("lastTimeBuyExpansionsHw")}
                      />
                    </label>
                    {!readonly && (
                      <button
                        disabled={
                          formData?.systemTypeDto?.systemTypeId == undefined
                        }
                        type="button"
                        className=" voda-bold btn btn-link btn-icon"
                        onClick={() =>
                          toggleInputEnable("lastTimeBuyExpansionsHw")
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/squareArrow.png")}
                        />
                      </button>
                    )}
                  </div>
                </div>
              )}
            </div>
          </fieldset>
        </div>
      </form>

      <fieldset className="fieldset">
        {!readonly && (
          <>
            <legend className=" red">
              <button
                type="button"
                disabled={formData?.systemTypeDto?.systemTypeId == undefined}
                className=" voda-bold btn btn-danger mb-4"
                onClick={() => GetConstraintFromNewDate()}
              >
                Calculate Constraints
              </button>
            </legend>
          </>
        )}

        <div className="row">
          <div className="col-6 row mx-0">
            <div className="form-group col-12 pl-0">
              <label className="labelForm voda-bold w-100">
                Constraint (Scaling)
                <input
                  type="text"
                  className="inputForm w-100"
                  readOnly
                  value={formData?.systemTypeDto?.constraintScaling}
                />
              </label>
            </div>
          </div>
          <div className="col-6 px-2 row mx-0">
            <div className="form-group col-12">
              <label className="labelForm voda-bold  w-100">
                Constraint (lcm)
                <input
                  type="text"
                  className="inputForm w-100"
                  readOnly
                  value={formData?.systemTypeDto?.constraintLcm ?? ""}
                />
              </label>
            </div>
          </div>
        </div>
      </fieldset>

      <div className="col-12">
        <div className="form-group row">
          <div className="col-6 p-0">
            {!readonly && (
              <>
                <button
                  type="button"
                  disabled={!dataSaved && !changed}
                  className=" voda-bold btn mr-2 cancel br-20"
                  onClick={() => RestoreDataConfirm()}
                >
                  Restore
                </button>
              </>
            )}
          </div>
          <div className="col-6 p-0" style={{ textAlign: "right" }}>
            <button
              type="button"
              className=" voda-bold btn mr-2 cancel"
              onClick={() => cancelConfirm()}
            >
              Cancel
            </button>
            {!readonly && (
              <button
                type="button"
                className=" voda-bold btn btn-danger mr-2"
                onClick={() => Submit(formData)}
              >
                Save
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductLifecycleConstraints;
