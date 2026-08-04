import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Modal, Form } from "react-bootstrap";
import {
  formatDateWithTime,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
} from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useSelector } from "react-redux";
import Select from "react-select";
import { DeliveryTrackingDtoUpdate } from "../../Model/DeliveryTracking";
import { CreatDeliveryTracking } from "../../Redux/Action/DeliveryTracking/DeliveryTrackingCreateAction";
import { EditDeliveryTracking } from "../../Redux/Action/DeliveryTracking/DeliveryTrackingEditAction";
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

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: DeliveryTrackingDtoUpdate,
      property: string
    );
    wizardBackFunction?(
      formData: DeliveryTrackingDtoUpdate,
      property: string
    ): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  edit: boolean;
  keyTab?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: DeliveryTrackingDtoUpdate;
}

const DeliveryTrackingModal: React.FC<Props> = (props) => {
  const { tipologicaPermesso, VerifyIsInRole } = useAuth();
  const [keyTabs, setKey] = useState("DeliveryTracking");
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
  } = useFormTableCrud<DeliveryTrackingDtoUpdate>(
    CreatDeliveryTracking,
    EditDeliveryTracking
  );
  const dtoEditResourceState = (state: RootState) =>
    state.deliveryTrackingEditReducer.DeliveryTrackingDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.deliveryTrackingCreateReducer.DeliveryTrackingDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disableWhat, setDisableWhat] = useState<boolean>(true);
  const [disablePlatform, setDisablePlatform] = useState<boolean>(true);
  const [disableHwType, setDisableHwType] = useState<boolean>(true);
  const [required, setRequired] = useState<boolean>(true);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);

  useEffect(() => {
    if (props.edit || forceEdit) {
      setforceEdit(false);
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: DeliveryTrackingDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    // const addInvalidProperty = (property: string) => {
    //   copyValidation?.property?.push(property);
    //   copyValidation.response = false;
    // };

    // if (
    //   copy?.originalEquipmentManufacturerId == null ||
    //   copy?.originalEquipmentManufacturerId === undefined ||
    //   copy?.originalEquipmentManufacturerId === 0
    // ) {
    //   addInvalidProperty("originalEquipmentManufacturerId");
    // }
    // if (
    //   copy?.buildConstructionId == null ||
    //   copy?.buildConstructionId === undefined ||
    //   copy?.buildConstructionId === 0
    // ) {
    //   addInvalidProperty("buildConstructionId");
    // }
    // if (
    //   (copy?.platformId == null ||
    //     copy?.platformId === undefined ||
    //     copy?.platformId === 0) &&
    //   required &&
    //   !noRules
    // ) {
    //   addInvalidProperty("platformId");
    // }
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
    // if (
    //   (copy?.hardwareType == null ||
    //     copy?.hardwareType === undefined ||
    //     copy?.hardwareType.trim() === "") &&
    //   required &&
    //   !noRules
    // ) {
    //   addInvalidProperty("hardwareType");
    //   setValidazioneCustom(undefined);
    // }
    // if (
    //   (copy?.hardwareSolution == null ||
    //     copy?.hardwareSolution === undefined ||
    //     copy?.hardwareSolution.trim() === "") &&
    //   required &&
    //   !noRules
    // ) {
    //   addInvalidProperty("hardwareSolution");
    //   setValidazioneCustom(undefined);
    // }
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

  const [autoFilled, setAutoFilled] = useState<boolean>(false);

  //const [processor, setProcessor] = useState<string>("");
  //const [operatingSystem, setOperatingSystem] = useState<string>("");

  const validateWizard = () => {
    let copy = { ...formData } as DeliveryTrackingDtoUpdate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "DeliveryTrackingDto"
      );
  };

  // const onHandelChangeAnnounced = (e: boolean) => {
  //   let copy = { ...formData } as DeliveryTrackingDtoUpdate;
  //   if (e) {
  //     setDisabledDate(true);
  //     copy.eomStatus = 0;
  //     copy.endOfMaintenance = null;
  //     setFormData(copy);
  //   } else {
  //     setDisabledDate(false);
  //     copy.eomStatus = 1;
  //     setFormData(copy);
  //   }
  // };

  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />

      <form id="formHardwareBuild" onChange={() => setChanged(true)}>
        <div>
          <fieldset className="fieldset">
            <div className="row">
              <div className="col-12">
                <label className="labelForm voda-bold w-100 pr-2">
                  Activity
                  {/* <input
                    disabled
                    type="text"
                    onChange={(e) => onChange("activity", e)}
                    onKeyUp={(e) => onChange("activity", e)}
                    className="inputForm w-100"
                    value={formData?.activity ? formData?.activity : ""}                   
                  /> */}
                  {/* <label
                    className="labelForm w-100"
                    dangerouslySetInnerHTML={{
                      __html: formData?.activity
                        ? stringIsNullOrEmpty(formData?.activity)
                          ? "---"
                          : formData?.activity
                        : "---",
                    }}
                  ></label> */}
                  <div
                    className="col-12 customFakeInput disabled"
                    style={{ marginTop: "4px" }}
                  >
                    <label
                      className="labelForm  w-100 mb-0"
                      dangerouslySetInnerHTML={{
                        __html: formData?.activity ?? "",
                      }}
                    ></label>
                  </div>
                </label>
              </div>
              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS1 Event Type
                  <textarea
                    disabled
                    rows={2}
                    onChange={(e) => onChange("ms1EventType", e)}
                    onKeyUp={(e) => onChange("ms1EventType", e)}
                    className="inputForm w-100"
                    value={formData?.ms1EventType ? formData?.ms1EventType : ""}
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.mS1EventTypeResource &&
                      dictionaryToArray(formData?.mS1EventTypeResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS1 Status"
                    value={
                      formData?.mS1EventTypeResource != undefined
                        ? formData?.mS1EventTypeResource &&
                          dictionaryToArray(
                            formData?.mS1EventTypeResource
                          ).filter((x) => x.key == formData?.ms1EventType)
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms1EventType", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms1EventType") ? (
                    <label className="validation">
                      *MS1 Event Type must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS1 Status
                  <input
                    disabled
                    type="text"
                    onChange={(e) => onChange("ms1status", e)}
                    onKeyUp={(e) => onChange("ms1status", e)}
                    className="inputForm w-100"
                    value={
                      formData?.msStatusResource &&
                      formData?.ms1status &&
                      dictionaryToArray(formData?.msStatusResource).find(
                        (x) => x.key === formData?.ms1status
                      )?.value
                    }
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.ms1StatusResource &&
                      dictionaryToArray(formData?.ms1StatusResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS1 Status"
                    value={
                      formData?.ms1Status != undefined
                        ? formData?.ms1StatusResource &&
                          dictionaryToArray(formData?.ms1StatusResource).filter(
                            (x) => x.key == formData?.ms1Status
                          )
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms1Status", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms1Status") ? (
                    <label className="validation">
                      *MS1 Status have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS1 Baseline Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms1BaseLineDate &&
                      new Date(formData?.ms1BaseLineDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms1BaseLineDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS1 Baseline Date"
                  />
                </span>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS1 Latest Planning Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms1LatestPlanningDate &&
                      new Date(formData?.ms1LatestPlanningDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms1LatestPlanningDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS1 Latest Planning Dat"
                  />
                </span>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS2 Event Type
                  <textarea
                    disabled
                    rows={2}
                    onChange={(e) => onChange("ms2EventType", e)}
                    onKeyUp={(e) => onChange("ms2EventType", e)}
                    className="inputForm w-100"
                    value={formData?.ms2EventType ? formData?.ms2EventType : ""}
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.mS2EventTypeResource &&
                      dictionaryToArray(formData?.mS2EventTypeResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS2 Event Type"
                    value={
                      formData?.ms2EventType != undefined
                        ? formData?.mS2EventTypeResource &&
                          dictionaryToArray(
                            formData?.mS2EventTypeResource
                          ).filter((x) => x.key == formData?.ms2EventType)
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms2EventType", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms2EventType") ? (
                    <label className="validation">
                      *MS2 Event Type must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS2 Status
                  <input
                    disabled
                    type="text"
                    onChange={(e) => onChange("ms2status", e)}
                    onKeyUp={(e) => onChange("ms2status", e)}
                    className="inputForm w-100"
                    value={
                      formData?.msStatusResource &&
                      formData?.ms2status &&
                      dictionaryToArray(formData?.msStatusResource).find(
                        (x) => x.key === formData?.ms2status
                      )?.value
                    }
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.ms2StatusResource &&
                      dictionaryToArray(formData?.ms2StatusResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS2 Status"
                    value={
                      formData?.ms2StatusResource != undefined
                        ? formData?.ms2StatusResource &&
                          dictionaryToArray(formData?.ms2StatusResource).filter(
                            (x) => x.key == formData?.ms2status
                          )
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms2status", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms2status") ? (
                    <label className="validation">
                      *MS2 Status must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS2 Baseline Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms2BaseLineDate &&
                      new Date(formData?.ms2BaseLineDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms2BaseLineDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS2 Baseline Date"
                  />
                </span>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS2 Latest Planning Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms2LatestPlanningDate &&
                      new Date(formData?.ms2LatestPlanningDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms2LatestPlanningDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS2 Latest Planning Date"
                  />
                </span>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS3 Event Type
                  <textarea
                    disabled
                    rows={2}
                    onChange={(e) => onChange("ms3EventType", e)}
                    onKeyUp={(e) => onChange("ms3EventType", e)}
                    className="inputForm w-100"
                    value={formData?.ms3EventType ? formData?.ms3EventType : ""}
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.mS3EventTypeResource &&
                      dictionaryToArray(formData?.mS3EventTypeResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS3 Event Type"
                    value={
                      formData?.ms3EventType != undefined
                        ? formData?.mS3EventTypeResource &&
                          dictionaryToArray(
                            formData?.mS3EventTypeResource
                          ).filter((x) => x.key == formData?.ms3EventType)
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms3EventType", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms3EventType") ? (
                    <label className="validation">
                      *MS# Event Type must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS3 Status
                  <input
                    disabled
                    type="text"
                    onChange={(e) => onChange("ms3status", e)}
                    onKeyUp={(e) => onChange("ms3status", e)}
                    className="inputForm w-100"
                    value={
                      formData?.msStatusResource &&
                      formData?.ms3status &&
                      dictionaryToArray(formData?.msStatusResource).find(
                        (x) => x.key === formData?.ms3status
                      )?.value
                    }
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.ms3StatusResource &&
                      dictionaryToArray(formData?.ms3StatusResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS3 Status"
                    value={
                      formData?.ms3status != undefined
                        ? formData?.ms3StatusResource &&
                          dictionaryToArray(formData?.ms3StatusResource).filter(
                            (x) => x.key == formData?.ms3status
                          )
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms3status", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms3status") ? (
                    <label className="validation">
                      *MS3 Status must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS3 Baseline Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms3BaseLineDate &&
                      new Date(formData?.ms3BaseLineDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms3BaseLineDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS3 Baseline Date"
                  />
                </span>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS3 Latest Planning Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms3LatestPlanningDate &&
                      new Date(formData?.ms3LatestPlanningDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms3LatestPlanningDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS3 Latest Planning Date"
                  />
                </span>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS4 Event Type
                  <textarea
                    disabled
                    rows={2}
                    onChange={(e) => onChange("ms4EventType", e)}
                    onKeyUp={(e) => onChange("ms4EventType", e)}
                    className="inputForm w-100"
                    value={formData?.ms4EventType ? formData?.ms4EventType : ""}
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.mS4EventTypeResource &&
                      dictionaryToArray(formData?.mS4EventTypeResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS4 Event Type"
                    value={
                      formData?.ms4EventType != undefined
                        ? formData?.mS4EventTypeResource &&
                          dictionaryToArray(
                            formData?.mS4EventTypeResource
                          ).filter((x) => x.key == formData?.ms4EventType)
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms4EventType", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms4EventType") ? (
                    <label className="validation">
                      *MS4 Event Type must have a value
                    </label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  MS4 Status
                  <input
                    disabled
                    type="text"
                    onChange={(e) => onChange("ms4status", e)}
                    onKeyUp={(e) => onChange("ms4status", e)}
                    className="inputForm w-100"
                    value={
                      formData?.msStatusResource &&
                      formData?.ms4status &&
                      dictionaryToArray(formData?.msStatusResource).find(
                        (x) => x.key === formData?.ms4status
                      )?.value
                    }
                  />
                  {/* <Select
                    isDisabled={true}
                    options={
                      formData?.ms4StatusResource &&
                      dictionaryToArray(formData?.ms4StatusResource).sort(
                        (a, b) =>
                          a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                      )
                    }
                    placeholder="MS4 Status"
                    value={
                      formData?.ms4status != undefined
                        ? formData?.ms4StatusResource &&
                          dictionaryToArray(formData?.ms4StatusResource).filter(
                            (x) => x.key == formData?.ms4status
                          )
                        : null
                    }
                    onChange={(e) => onChangeSelect("ms4status", e)}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"].toString()}
                  ></Select> */}
                  {/* {validation &&
                  validation.response == false &&
                  validation.property?.includes("ms4status") ? (
                    <label className="validation">*Oem must have a value</label>
                  ) : null} */}
                </label>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS4 Baseline Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms4BaseLineDate &&
                      new Date(formData?.ms4BaseLineDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms4BaseLineDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS4 Baseline Date"
                  />
                </span>
              </div>
              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">MS4 Latest Planning Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ms4LatestPlanningDate &&
                      new Date(formData?.ms4LatestPlanningDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ms4LatestPlanningDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="MS4 Latest Planning Date"
                  />
                </span>
              </div>
              <div className="col-6">
                <label className="labelForm voda-bold w-100 mb-20">
                  PPM Id
                  <input
                    type="text"
                    disabled={true}
                    className="inputForm w-100"
                    value={formData?.ppmID ?? ""}
                    placeholder="PPM Id"
                  />
                </label>
              </div>

              <div className="col-6">
                <span className="labelForm voda-bold sp w-100">
                  <span className="fz-16">PPM Import Date</span>
                  <DatePicker
                    disabled={true}
                    selected={
                      formData?.ppmImportDate &&
                      new Date(formData?.ppmImportDate)
                    }
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate("ppmImportDate", newDate);
                    }}
                    className="inputForm w-100 "
                    minDate={new Date(1980, 0, 1)}
                    maxDate={new Date(2999, 0, 1)}
                    dateFormat="dd/MM/yyyy"
                    placeholderText="PPM Import Date"
                  />
                </span>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 pr-2">
                  Notes 1
                  <input
                    type="text"
                    disabled={true}
                    onChange={(e) => onChange("notes1", e)}
                    onKeyUp={(e) => onChange("notes1", e)}
                    className="inputForm w-100"
                    value={formData?.notes1 ?? ""}
                    placeholder="Note 1"
                  />
                </label>
              </div>

              <div className="col-6">
                <label className="labelForm voda-bold w-100 pr-2">
                  Notes 2
                  <input
                    type="text"
                    disabled={true}
                    onChange={(e) => onChange("notes2", e)}
                    onKeyUp={(e) => onChange("notes2", e)}
                    className="inputForm w-100"
                    placeholder="Note 2"
                    value={formData?.notes2 ?? ""}
                  />
                </label>
              </div>
            </div>
          </fieldset>

          {/* CLASSIC */}
          <Container show={!props.wizardMode && showFurtherDetails}>
            <fieldset className="fieldset">
              <div className="row">
                <div className="col-12 p-0">
                  <div className="row mt-50">
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
        </div>
      </form>
      <Container show={!props.wizardMode}>
        <div className="col-12 justify-content-end mt-4 pr-4 d-flex footerModal">
          <button
            className="voda-bold btn btn-danger px-4 btnHeader"
            onClick={() => {
              props.action.closeModal && props.action.closeModal(changed);
            }}
            type="button"
          >
            Close
          </button>
        </div>
      </Container>
    </div>
  );
};

export default DeliveryTrackingModal;
