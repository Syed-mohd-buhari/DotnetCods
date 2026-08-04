import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime, lowerFirstLetter } from "../../../Hook/Common";
import {
  dictionaryToArray,
  dictionaryToArrayActivityDetail,
} from "../../../Hook/Dictionary";

import { Form, Modal, Tab, Tabs } from "react-bootstrap";
import Select from "react-select";
import BenefitsContainer from "../../../Containers/Lookup/BenefitsContainer";
import DriverContainer from "../../../Containers/Lookup/DriverContainer";
import PlanningRisk from "../../../Containers/Lookup/PlanningRiskContainer";
import { useAuth } from "../../../Hook/useAuth";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import {
  PlannedActivityResourceDto,
  TipologicaGridDtoForVirtualized,
} from "../../../Model/LookUp/PlannedActivityResource";
import { CreatPlannedActivityResource } from "../../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceCreateAction";
import { EditPlannedActivityResource } from "../../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceEditAction";
import { RootState } from "../../../Redux/Store/rootStore";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import ActivityDetailsContainer from "../../../Containers/Lookup/ActivityDetailsContainer";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  rules: { key: number; value: string }[];
  rulesNetworkElement: { key: number; value: string }[];
  rulesLinkedDesignComponent: { key: number; value: string }[];
  rulesActivityDetailsNetworkElement: { key: number; value: string }[];
  addRemoveActivityDetailsDropdownOptions: { key: number; value: string }[];
}

const PlannedActivityResourceForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");
  const [errorSchema, setErrorSchema] = useState<MessageError>();
  const [started, setStarted] = useState<boolean>(false);

  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
  } = useFormTableCrud<PlannedActivityResourceDto>(
    CreatPlannedActivityResource,
    EditPlannedActivityResource
  );

  const preSave = () => {
    let copy = { ...formData } as PlannedActivityResourceDto;

    if (!copy.forLcm && !copy.forserviceplan) {
      copy.ruleActicvityDetails = undefined;
      copy.activityDetailsLcm = undefined;
    }

    if (!copy.forLcm) {
      copy.lcmHardware = undefined;
      copy.lcmSoftware = undefined;
      copy.lcmLabelHardware = undefined;
      copy.lcmLabelSoftware = undefined;
      copy.driverTextLcm = undefined;
      copy.benefitTextLcm = undefined;
      copy.planningRisksLcm = undefined;
    }

    if (!copy.forserviceplan) {
      copy.activityDetailsService = undefined;
      copy.planningRiskServicePlan = undefined;
      copy.driverTextServicePlan = undefined;
      copy.benefitTextServicePlan = undefined;
      copy.serviceExportable = undefined;
      // Don't clear ruleActicvityDetails here
    }

    if (!copy.forNetworkElement) {
      copy.activityDetailsForVirtualizedNetworkElement = undefined;
      copy.activityDetailsNetworkElement = undefined;
      copy.ruleActicvityDetailsNetworkElement = undefined;
      copy.ruleNetworkElement = undefined;
      copy.benefitTextNetworkElement = undefined;
      copy.driverTextNetworkElement = undefined;
      copy.planningRisksNetworkElement = undefined;
      copy.onBareMetalNetworkElement = undefined;
      copy.onVirtualizedNetworkElement = undefined;
      copy.forCreateNetworkElement = undefined;
      copy.forEditNetworkElement = undefined;
      copy.networkElementHardware = undefined;
      copy.networkElementSoftware = undefined;
      copy.networkElementLabelSoftware = undefined;
      copy.networkElementLabelHardware = undefined;
    }

    Save(copy, props.edit, validazioneClient, refresh);
  };
  const dtoEditResourceState = (state: RootState) =>
    state.plannedActivityResourceEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.plannedActivityResourceCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const Grid = (state: RootState) =>
    state.activityDetailsGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      setStarted(false);
      if (editResource?.forLcm) setKey("LCM");
      else if (editResource?.forNetworkElement) setKey("NE");
      else if (editResource?.forDesignAspect) setKey("DA");
      else if (editResource?.forserviceplan) setKey("forserviceplan");
      else setKey("");
    } else {
      setStarted(false);
      setKey("");

      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (formData && formData != null) {
      if (!props.edit && !started) {
        let copy = { ...formData } as PlannedActivityResourceDto;
        copy.exportable = true;
        setFormData(copy);
        setStarted(true);
      }
    }
  }, [formData?.exportable]);

  const validazioneClient = (copy: PlannedActivityResourceDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.plannedActivityResourceDescription === null ||
      copy?.plannedActivityResourceDescription === undefined ||
      copy?.plannedActivityResourceDescription.trim() === ""
    ) {
      addInvalidProperty("plannedActivityResourceDescription");
    }
    if (
      copy?.forLcm &&
      (copy.ruleActicvityDetails == undefined ||
        copy.ruleActicvityDetails == null ||
        copy.ruleActicvityDetails == 0)
    ) {
      addInvalidProperty("ruleActicvityDetails");
    }
    if (
      copy?.forserviceplan &&
      (copy.ruleActicvityDetails == undefined ||
        copy.ruleActicvityDetails == null ||
        copy.ruleActicvityDetails == 0)
    ) {
      addInvalidProperty("ruleActicvityDetails");
    }

    // if (
    //   copy?.activityDetailsLcm === undefined ||
    //   copy?.activityDetailsLcm === null ||
    //   copy?.activityDetailsLcm.trim() === ""
    // ) {
    //   addInvalidProperty("activityDetailsLcm");
    // }
    if (
      copy.forLcm == true &&
      (copy.lcmSoftware == false || copy.lcmSoftware == undefined) &&
      (copy.lcmHardware == false || copy.lcmHardware == undefined)
    ) {
      addInvalidProperty("onHwSwLCM");
    }
    if (
      copy.forLcm === false &&
      copy.forNetworkElement === false &&
      copy.forDesignAspect === false &&
      copy.forAddAsset &&
      copy.forEditAsset
    ) {
      addInvalidProperty("for");
    }

    if (
      copy.forAddAsset &&
      (copy.ruleAddAsset === undefined || copy.ruleAddAsset === null)
    ) {
      addInvalidProperty("ruleAddAsset");
    }

    if (
      copy.forEditAsset &&
      (copy.ruleEditAsset === undefined || copy.ruleEditAsset === null)
    ) {
      addInvalidProperty("ruleEditAsset");
    }

    if (
      copy.forAddAsset &&
      (copy.ruleActicvityDetailsAddAsset === undefined ||
        copy.ruleActicvityDetailsAddAsset === null ||
        copy.ruleActicvityDetailsAddAsset === 0)
    ) {
      addInvalidProperty("ruleActicvityDetailsAddAsset");
    }

    if (
      copy.forEditAsset &&
      (copy.ruleActicvityDetailsEditAsset === undefined ||
        copy.ruleActicvityDetailsEditAsset === null ||
        copy.ruleActicvityDetailsEditAsset === 0)
    ) {
      addInvalidProperty("ruleActicvityDetailsEditAsset");
    }

    if (
      copy.forAddAsset &&
      (copy.onVirtualizedAddAsset === undefined ||
        copy.onVirtualizedAddAsset === null ||
        copy.onVirtualizedAddAsset === false) &&
      (copy.onBareMetalAddAsset === false ||
        copy.onBareMetalAddAsset === null ||
        copy.onBareMetalAddAsset === undefined)
    ) {
      addInvalidProperty("onVirtualizedAddAsset");
    }

    if (
      copy.forEditAsset &&
      (copy.onVirtualizedEditAsset === undefined ||
        copy.onVirtualizedEditAsset === null ||
        copy.onVirtualizedEditAsset === false) &&
      (copy.onBareMetalEditAsset === false ||
        copy.onBareMetalEditAsset === null ||
        copy.onBareMetalEditAsset === undefined)
    ) {
      addInvalidProperty("onVirtualizedEditAsset");
    }

    if (
      copy.forNetworkElement == true &&
      (copy.networkElementSoftware == false ||
        copy.networkElementSoftware == undefined) &&
      (copy.networkElementHardware == false ||
        copy.networkElementHardware == undefined)
    ) {
      addInvalidProperty("onHwSwNE");
    }

    if (
      copy.forNetworkElement == true &&
      (copy.onBareMetalNetworkElement == false ||
        copy.onBareMetalNetworkElement == undefined) &&
      (copy.onVirtualizedNetworkElement == false ||
        copy.onVirtualizedNetworkElement == undefined)
    ) {
      addInvalidProperty("onBareVirtualized");
    }

    if (
      copy.forEditAsset == true &&
      (copy.onBareMetalEditAsset == false ||
        copy.onBareMetalEditAsset == undefined) &&
      (copy.onVirtualizedEditAsset == false ||
        copy.onVirtualizedEditAsset == undefined)
    ) {
      addInvalidProperty("onBareVirtualized");
    }

    if (
      copy.forAddAsset &&
      (copy.addAssetSoftware === false ||
        copy.addAssetSoftware === undefined) &&
      (copy.addAssetHardware === false || copy.addAssetHardware === undefined)
    ) {
      addInvalidProperty("onHwSwAddAsset");
    }

    if (
      copy.forEditAsset &&
      (copy.editAssetSoftware === false ||
        copy.editAssetSoftware === undefined) &&
      (copy.editAssetHardware === false || copy.editAssetHardware === undefined)
    ) {
      addInvalidProperty("onHwSwEditAsset");
    }

    if (
      copy?.forNetworkElement &&
      (copy.ruleActicvityDetailsNetworkElement == undefined ||
        copy.ruleActicvityDetailsNetworkElement == null ||
        copy.ruleActicvityDetailsNetworkElement == 0)
    ) {
      addInvalidProperty("ruleActicvityDetailsNetworkElement");
    }
    if (
      copy?.forNetworkElement &&
      (copy.ruleNetworkElement == undefined || copy.ruleNetworkElement == null)
    ) {
      addInvalidProperty("ruleNetworkElement");
    }

    if (
      copy.forAddAsset &&
      (copy.ruleActicvityDetailsAddAsset === undefined ||
        copy.ruleActicvityDetailsAddAsset === null)
    ) {
      addInvalidProperty("ruleActicvityDetailsAddAsset");
    }

    if (
      copy.forEditAsset &&
      (copy.ruleActicvityDetailsEditAsset === undefined ||
        copy.ruleActicvityDetailsEditAsset === null)
    ) {
      addInvalidProperty("ruleActicvityDetailsEditAsset");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const removeValidation = (property: string) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const multipleRemoveValidation = (listOfProperty: string[]) => {
    //Rimuovi Validazione
    if (validation && validation?.property != undefined) {
      let copy = { ...validation, property: [...validation.property] };

      listOfProperty.forEach((property) => {
        if (validation?.property?.includes(property)) {
          let idxOfProperty = copy.property.indexOf(property);
          copy.property.splice(idxOfProperty, 1);
        }
      });

      setValidation(copy);
    }
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeExportable = (checked: boolean, type: string) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    if (type === "LCM") copy.exportable = checked;
    else if (type === "DA") copy.designAspectExportable = checked;
    else if (type === "Service") copy.serviceExportable = checked;
    setFormData(copy);
  };

  const onChangeRequiredPlanned = (type: string, checked: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy[type] = checked;
    setFormData(copy);
  };

  const onChangeCheckbox = (property: string, checked: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy[property] = checked;
    if (property == "lcmHardware") {
      removeValidation("onHwSwLCM");
      copy.lcmLabelHardware = undefined;
    } else if (property == "lcmSoftware") {
      removeValidation("onHwSwLCM");
      copy.lcmLabelSoftware = undefined;
    } else if (
      property === "networkElementHardware" ||
      property === "networkElementSoftware"
    ) {
      removeValidation("onHwSwNE");
    } else if (
      property === "forCreateNetworkElement" ||
      property === "forEditNetworkElement"
    ) {
      removeValidation("fornet");
    }

    setFormData(copy);
  };

  const [isVisibleCustomJson, setIsVisibleCustomJson] =
    useState<boolean>(false);

  const onChangeJson = (e: any) => {
    let jsonString = JSON.stringify(e.updated_src);
    let copy = { ...formData } as PlannedActivityResourceDto;
    if (jsonString != "{}") {
      copy.jsonForm = jsonString;
    } else {
      copy.jsonForm = undefined;
    }
    setFormData(copy);
    validateJsonSchema(jsonString);
  };

  const onChangeJsonProvvisorio = (e: any, property?: string) => {
    let jsonString = e.target.value;
    let copy = { ...formData } as PlannedActivityResourceDto;
    if (jsonString != "{}" && jsonString != null && jsonString != "") {
      let test = jsonString.replace(/\n/g, "").replace(/&quot;/g, '"');
      copy.jsonForm = test;
    } else {
      copy.jsonForm = undefined;
    }
    setFormData(copy);

    if (property && validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  interface JsonSchema {
    type: string;
    title?: string | number;
    required?: string[] | number[];
    properties: {};
  }

  interface PropertyJsonSchema {
    title: string;
    type: string;
    inputType: string;
  }

  interface SelectJsonSchema extends PropertyJsonSchema {
    enum: [];
    enumNames?: string[];
  }

  interface DateJsonSchema extends PropertyJsonSchema {
    format: string;
  }

  interface MessageError {
    response: boolean;
    message: string;
  }

  const validateJsonSchema = (value: string) => {
    let rtn = { response: true, message: "" } as MessageError;
    if (value !== undefined && value !== "" && value != null) {
      try {
        JSON.parse(value);
      } catch {
        rtn.message = "json entered is not valid, check the field.";
        rtn.response = false;
        setErrorSchema(rtn);
        return rtn;
      }

      let x = JSON.parse(value) as JsonSchema;
      if (x.type === undefined || x.type === "") {
        rtn.message = "type must have a value";
        rtn.response = false;
        setErrorSchema(rtn);
        return rtn;
      } else if (x.type != undefined && x.type !== "object") {
        rtn.message = 'type value must be "object"';
        rtn.response = false;
        setErrorSchema(rtn);
        return rtn;
      }

      if (x.required !== undefined) {
        if (Array.isArray(x.required) === false) {
          rtn.message =
            "Required must be a list of properties names required in the form";
          rtn.response = false;
          setErrorSchema(rtn);
          return rtn;
        }
      }

      if (x.properties === undefined) {
        rtn.message = "Properties must have a value";
        rtn.response = false;
        setErrorSchema(rtn);
        return rtn;
      } else if (
        typeof x.properties !== "object" ||
        Array.isArray(x.properties) === true ||
        Object.keys(x.properties).length === 0
      ) {
        rtn.message = "Properties value is not valid";
        rtn.response = false;
        setErrorSchema(rtn);
        return rtn;
      } else {
        for (let prop in x.properties) {
          if (
            typeof x.properties[prop] !== "object" ||
            Array.isArray(x.properties[prop]) === true ||
            Object.keys(x.properties[prop]).length === 0
          ) {
            rtn.message = `${prop} must have a value`;
            rtn.response = false;
            setErrorSchema(rtn);
            return rtn;
          } else {
            let p = x.properties[prop] as PropertyJsonSchema;
            if (p.type === undefined || p.type === "") {
              rtn.message = ` "type" of property "${prop}" must have a value`;
              rtn.response = false;
              setErrorSchema(rtn);
              return rtn;
            } else if (
              p.type !== "string" &&
              p.type !== "number" &&
              p.type !== "boolean"
            ) {
              rtn.message = ` "type" value of a property can be \"string\" or \"number\" or \"boolean\" `;
              rtn.response = false;
              setErrorSchema(rtn);
              return rtn;
            } else if (p.title === undefined || p.title === "") {
              rtn.message = ` "title" of property "${prop}" must have a value`;
              rtn.response = false;
              setErrorSchema(rtn);
              return rtn;
            } else if (p.inputType === undefined || p.inputType === "") {
              rtn.message = ` "inputType" of property "${prop}" must have a value`;
              rtn.response = false;
              setErrorSchema(rtn);
              return rtn;
            } else if (
              p.inputType !== "text" &&
              p.inputType !== "date" &&
              p.inputType !== "select" &&
              p.inputType !== "checkbox"
            ) {
              rtn.message = ` "inputType" value of property "${prop}" can only be \"text\" or \"date\" or \"select\" or \"checkbox\"`;
              rtn.response = false;
              setErrorSchema(rtn);
              return rtn;
            }

            if (p.inputType === "select") {
              if (p.type === "boolean") {
                rtn.message = `"inputType" value of a property type boolean can not be "select"`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              }

              let s = x.properties[prop] as SelectJsonSchema;
              if (s.enum === undefined) {
                rtn.message = `"enum" of property "${prop}" must have a value`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              } else if (Array.isArray(s.enum) === false) {
                rtn.message = `"enum" value of property "${prop}" must be a list of ${
                  p.type === "string" ? "strings" : "numbers"
                }`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              } else if (s.enum.length === 0) {
                rtn.message = `"enum" list of property "${prop}" can not be empty`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              } else if (p.type === "number") {
                for (let i = 0; i < s.enum.length; i++) {
                  if (typeof s.enum[i] !== "number") {
                    rtn.message = `"enum" value of property "${prop}" must be a list of numbers`;
                    rtn.response = false;
                    setErrorSchema(rtn);
                    return rtn;
                  }
                }
                if (s.enumNames === undefined) {
                  rtn.message = `A property with inputType "select" and type "number" must have a "enumNames" as list of strings for display labels in the select`;
                  rtn.response = false;
                  setErrorSchema(rtn);
                  return rtn;
                } else {
                  if (Array.isArray(s.enumNames) === false) {
                    rtn.message = `"enumNames" value of property "${prop}" must be a list of strings`;
                    rtn.response = false;
                    setErrorSchema(rtn);
                    return rtn;
                  } else if (s.enumNames.length === 0) {
                    rtn.message = `"enumNames" list of property "${prop}" can not be empty`;
                    rtn.response = false;
                    setErrorSchema(rtn);
                    return rtn;
                  } else if (s.enum.length !== s.enumNames.length) {
                    rtn.message = `"enum" and "enumNames" properties of "${prop}" have different lengths`;
                    rtn.response = false;
                    setErrorSchema(rtn);
                    return rtn;
                  } else {
                    for (let i = 0; i < s.enumNames.length; i++) {
                      if (typeof s.enumNames[i] !== "string") {
                        rtn.message = `"enumNames" value of property "${prop}" must be a list of strings`;
                        rtn.response = false;
                        setErrorSchema(rtn);
                        return rtn;
                      }
                    }
                  }
                }
              } else if (p.type === "string") {
                for (let i = 0; i < s.enum.length; i++) {
                  if (typeof s.enum[i] !== "string") {
                    rtn.message = `"enum" value of property "${prop}" must be a list of strings`;
                    rtn.response = false;
                    setErrorSchema(rtn);
                    return rtn;
                  }
                }
              }
            } else if (p.inputType === "date") {
              if (p.type === "boolean") {
                rtn.message = `"inputType" of a property type boolean can not be "date"`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              } else if (p.type === "number") {
                rtn.message = `"inputType" of a property type number can not be "date"`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              }

              let d = x.properties[prop] as DateJsonSchema;
              if (d.format === undefined || d.format === "") {
                rtn.message = `"format" of "${prop}" must have a value`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              } else if (d.format !== "date") {
                rtn.message = `"format" value of property "${prop}" must be date`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              }
            } else if (p.inputType === "checkbox") {
              if (p.type !== "boolean") {
                rtn.message = `properties with inputType "checkbox" must be type "boolean"`;
                rtn.response = false;
                setErrorSchema(rtn);
                return rtn;
              }
            }
          }
        }
      }

      rtn.message = "";
      rtn.response = true;
      setErrorSchema(rtn);
      setIsVisibleCustomJson(false);
      return rtn;
    } else {
      rtn.message = "";
      rtn.response = true;
      setErrorSchema(rtn);
      setIsVisibleCustomJson(false);
      return rtn;
    }
  };

  const onChangeForNetworkElement = (val: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy.forNetworkElement = val;
    const listOfProperty = [
      "fornet",
      "for",
      "benefitTextNetworkElement",
      "driverTextNetworkElement",
      "activityDetailsNetworkElement",
      "ruleActicvityDetailsNetworkElement",
      "onHwSwNE",
    ];
    if (keyTabs === "" && val) setKey("NE");
    if (!val) setKey(copy.forLcm ? "LCM" : copy.forDesignAspect ? "DA" : "");
    multipleRemoveValidation(listOfProperty);
    setFormData(copy);
  };

  const onChangeForLCM = (val: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy.forLcm = val;
    const listOfProperty = [
      "forLcm",
      "for",
      "lcmHardware",
      "lcmSoftware",
      "ruleActicvityDetails",
      "onHwSwLCM",
    ];
    if (keyTabs === "" && val) setKey("LCM");
    if (!val)
      setKey(copy.forNetworkElement ? "NE" : copy.forDesignAspect ? "DA" : "");
    multipleRemoveValidation(listOfProperty);
    setFormData(copy);
  };

  const onChangeForDesignAspect = (val: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy.forDesignAspect = val;
    const listOfProperty = [
      "forDesignAspect",
      "for",
      "designAspectHardware",
      "designAspectSoftware",
      "ruleActicvityDetails",
      "onHwSwDesignAspect",
    ];
    if (keyTabs === "" && val) setKey("DA");
    if (!val) setKey(copy.forNetworkElement ? "NE" : copy.forLcm ? "LCM" : "");
    multipleRemoveValidation(listOfProperty);
    setFormData(copy);
  };

  const onChangeAssets = (type: string, val: boolean) => {
    let copy = { ...formData } as PlannedActivityResourceDto;

    const listOfProperty = [
      "forLcm",
      "for",
      "lcmHardware",
      "lcmSoftware",
      "ruleActicvityDetails",
      "onHwSwLCM",
    ];

    if (type === "addAssets") {
      copy.forAddAsset = val;
      if (keyTabs === "" && val) setKey("forCreateAssets");
      if (!val)
        setKey(
          copy.forNetworkElement
            ? "NE"
            : copy.forDesignAspect
            ? "DA"
            : copy.forEditAsset
            ? "forEditAsset"
            : copy.forserviceplan
            ? "forserviceplan"
            : ""
        );
      multipleRemoveValidation(listOfProperty);
    }

    if (type === "editAssets") {
      copy.forEditAsset = val;
      if (keyTabs === "" && val) setKey("forEditAsset");
      if (!val)
        setKey(
          copy.forNetworkElement
            ? "NE"
            : copy.forDesignAspect
            ? "DA"
            : copy.forAddAsset
            ? "forCreateAssets"
            : copy.forserviceplan
            ? "forserviceplan"
            : ""
        );
      multipleRemoveValidation(listOfProperty);
    }

    if (type === "service") {
      copy.forserviceplan = val;
      if (keyTabs === "" && val) setKey("forserviceplan");
      if (!val)
        setKey(
          copy.forNetworkElement
            ? "NE"
            : copy.forDesignAspect
            ? "DA"
            : copy.forAddAsset
            ? "forCreateAssets"
            : copy.forEditAsset
            ? "forEditAsset"
            : ""
        );
      multipleRemoveValidation(listOfProperty);
    }

    setFormData(copy);
  };

  const [isValidActivityforVirtualized, setIsValidActivityforVirtualized] =
    useState<boolean>(true);

  useEffect(() => {
    if (
      formData?.activityDetailsForVirtualizedNetworkElement ===
      formData?.activityDetailsNetworkElement
    ) {
      setIsValidActivityforVirtualized(false);
    } else {
      setIsValidActivityforVirtualized(true);
    }
  }, [
    formData?.activityDetailsNetworkElement,
    formData?.activityDetailsForVirtualizedNetworkElement,
  ]);

  const customChangeSelect = (property: string, e: any) => {
    const val: string = e.value;
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy[lowerFirstLetter(property)] = val;
    setFormData(copy);
    removeValidation(property);
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as PlannedActivityResourceDto;
    if (e != null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    removeValidation(property);
  };

  const customChangeSelectForActivityDetails = (property: string, e: any) => {
    const val: string = e.value.description;
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy[lowerFirstLetter(property)] = val;
    console.log("copy => ", copy);
    setFormData(copy);
    removeValidation(property);
  };

  const onChangeSelectString = (property: string, e: any) => {
    const val: string = e.value;
    let copy = { ...formData } as PlannedActivityResourceDto;
    copy[lowerFirstLetter(property)] = val;
    setFormData(copy);
    removeValidation(property);
  };

  const [disabledSelect, setDisabledSelect] = useState<Array<string>>([]);

  const DriverRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.driverResource)
      formData.driverResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const ActivityDetailsRefillData = (value: Array<any>) => {
    console.log("value => ", value);
    let list = {} as {
      [key: string]: TipologicaGridDtoForVirtualized;
    };
    value.forEach((item) => {
      list = {
        ...list,
        [String(item.id)]: item,
      };
    });
    let copy = { ...formData } as PlannedActivityResourceDto;
    console.log("list => ", list);
    copy.activityDetailsResource = list;
    setFormData(copy);
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
  const PlanningRiskRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.planningRiskResource)
      formData.planningRiskResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <ActivityDetailsContainer
            returnObject={ActivityDetailsRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );

      case 2:
        return (
          <BenefitsContainer
            returnObject={BenefitsRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 3:
        return (
          <DriverContainer
            returnObject={DriverRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 4:
        return (
          <PlanningRisk
            returnObject={PlanningRiskRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return;
    }
  };

  const [ruleActivityDetails, setRuleActivityDetails] = useState<number>(0);

  // useEffect(() => {
  //   if (formData?.ruleActicvityDetails != undefined) {
  //     let copy = { ...formData };
  //     copy.activityDetailsLcm = undefined;
  //     setFormData(copy);
  //   }
  // }, [formData?.ruleActicvityDetails]);

  // useEffect(() => {
  //   if (GridDto?.items && isVisibleModalLookup === 0) {
  //     const data = GridDto.items as TipologicaGridDtoForVirtualized[];
  //     let list = {} as {
  //       [key: string]: TipologicaGridDtoForVirtualized;
  //     };
  //     data.forEach((item) => {
  //       list = {
  //         ...list,
  //         [String(item.id)]: item,
  //       };
  //     });
  //     let copy = { ...formData } as PlannedActivityResourceDto;
  //     copy.activityDetailsResource = list;
  //     setFormData(copy);
  //   }
  // }, [isVisibleModalLookup]);

  useEffect(() => {
    if (formData?.ruleActicvityDetailsNetworkElement != undefined) {
      let copy = { ...formData };
      //copy.activityDetailsNetworkElement = undefined;
      copy.activityDetailsForVirtualizedNetworkElement = undefined;
      setFormData(copy);
    }
  }, [formData?.ruleActicvityDetailsNetworkElement]);

  return (
    <div className="col-12">
      <Modal
        show={isVisibleModalLookup > 0}
        // backdrop="static"
        backdropClassName="secondBackdropLookup"
        dialogClassName="dialogGrid"
        className="secondModalLookup"
        keyboard={false}
        size="lg"
        centered
        onHide={() => setIsVisibleModalLookup(0)}
      >
        <Modal.Header closeButton className="mb-2">
          {/* <div className="col-12 px-0">
            <div className="col-12"> */}
          {/* <h4 className="mb-0">Lookup Tables</h4> */}
          {/* </div> */}
          {/* <ErrorNotification OnModal={true} /> */}
          {/* </div> */}
        </Modal.Header>
        <Modal.Body>{ReturnLookupContainer(isVisibleModalLookup)}</Modal.Body>
      </Modal>

      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row col-12 px-0 mx-0">
          <div className="col-6 pl-0">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Planned Activity<span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) =>
                    onChange("plannedActivityResourceDescription", e)
                  }
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100"
                  value={formData?.plannedActivityResourceDescription}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes(
                  "plannedActivityResourceDescription"
                ) ? (
                  <label className="validation">*Description is required</label>
                ) : null}
              </label>
            </div>
          </div>
          <div className="col-6 row mx-0 form-group">
            <div className="form-group w-100">
              <label className="labelForm voda-bold w-100 mb-0">
                Planned Activity Type<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={props.rulesLinkedDesignComponent.map((x) => {
                        return { key: x.key, value: x.value };
                      })}
                      value={props.rulesLinkedDesignComponent
                        .filter((x) => x.key === formData?.ruleLinkedDc)
                        .map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                      onChange={(e) => onChangeSelect("ruleLinkedDc", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("ruleLinkedDc") ? (
                <label className="validation">
                  *Planned Activity Type must have a value
                </label>
              ) : null}
            </div>
          </div>
          <div className="mr-4 w-100 col-12 row">
            <label className="text-bb mt-5">Enable Planned Activity for</label>
            <div className="col-12 pl-0">
              <div className="row">
                <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) => onChangeForLCM(e.target.checked)}
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forLcm}
                    />
                    <span className="labelForm m-0">LCM Engineering</span>
                  </label>
                </div>
                <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) =>
                        onChangeAssets("addAssets", e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forAddAsset}
                    />
                    <span className="labelForm m-0">Add Assets</span>
                  </label>
                </div>
                <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) =>
                        onChangeAssets("editAssets", e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forEditAsset}
                    />
                    <span className="labelForm m-0">Edit Assets</span>
                  </label>
                </div>

                {/* <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) =>
                        onChangeForNetworkElement(e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forNetworkElement}
                    />
                    <span className="labelForm m-0">Network Element</span>
                  </label>
                </div> */}
                <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) =>
                        onChangeForDesignAspect(e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forDesignAspect}
                    />
                    <span className="labelForm m-0">Design Aspect</span>
                  </label>
                </div>
                <div className="col-3">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      type="checkbox"
                      onChange={(e) =>
                        onChangeAssets("service", e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.forserviceplan}
                    />
                    <span className="labelForm m-0">Service</span>
                  </label>
                </div>
                {validation &&
                validation.response == false &&
                validation.property?.includes("for") ? (
                  <label className="validation">
                    *For Design Aspect or For Network Element must be true
                  </label>
                ) : null}
              </div>
            </div>
          </div>
        </div>
      </form>

      <Tabs
        defaultActiveKey={keyTabs}
        id="uncontrolled-tab-example"
        activeKey={keyTabs}
        onSelect={(x) => setKey(x || "")}
        className="mt-5"
      >
        <Tab
          eventKey="LCM"
          title="LCM Engineering Function"
          className="col-12"
          disabled={!formData?.forLcm}
        >
          {/* <div className="col-12 px-0 row mt-4"> */}
          <div className="col-12 row px-0 mt-4">
            <div className="col-12 p-0">
              <label className="text-bb">Planned Activity Settings</label>
              <div className="row col-12 mb-3">
                <label className="labelForm voda-bold w-100 mb-0 mt-4">
                  Activity is applicable for<span className="red">*</span>
                </label>
                <div className="mr-4">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      disabled={!formData?.forLcm}
                      type="checkbox"
                      onChange={(e) =>
                        onChangeCheckbox("lcmHardware", e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.lcmHardware}
                    />
                    Hardware
                  </label>
                </div>
                <div className="">
                  <label className="labelForm w-100 h-100 d-flex align-items-center">
                    <input
                      disabled={!formData?.forLcm}
                      type="checkbox"
                      onChange={(e) =>
                        onChangeCheckbox("lcmSoftware", e.target.checked)
                      }
                      style={{ height: "15px" }}
                      className="inputForm mb-1 mr-1"
                      checked={formData?.lcmSoftware}
                    />
                    Software
                  </label>
                </div>
                {validation &&
                validation.response == false &&
                validation.property?.includes("onHwSwLCM") ? (
                  <label className="validation col-12">
                    *You must select at least one of these
                  </label>
                ) : null}
              </div>
            </div>
            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold w-100 mb-0">
                  Rule For Activity Details<span className="red">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={props.rules.map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                        value={props.rules
                          .filter(
                            (x) => x.key === formData?.ruleActicvityDetails
                          )
                          .map((x) => {
                            return { key: x.key, value: x.value };
                          })}
                        onChange={(e) =>
                          onChangeSelect("ruleActicvityDetails", e)
                        }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value.toString()}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forLcm}
                      ></Select>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("ruleActicvityDetails") ? (
                        <label className="validation">
                          *Rule must have a value
                        </label>
                      ) : null}
                    </div>
                  </div>
                </label>
              </div>
              {/* SE RULE LCM è ADD/REMOVE HW COMPONENTS */}
              {formData?.ruleActicvityDetails === 8 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={props.addRemoveActivityDetailsDropdownOptions}
                        value={props.addRemoveActivityDetailsDropdownOptions.filter(
                          (x) => x.value === formData?.activityDetailsLcm
                        )}
                        onChange={(e) =>
                          onChangeSelectString("activityDetailsLcm", e)
                        }
                        // onKeyUp={(e) =>
                        //   onChangeSelectString("activityDetailsLcm", e)
                        // }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forLcm}
                      />
                    </div>
                  </label>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("activityDetailsLcm") ? (
                    <label className="validation">
                      *activity Details Lcm cannot be empty!
                    </label>
                  ) : null}
                </div>
              ) : null}
              {/* SE RULE LCM è FIXED TEXT FROM ADMIN O Upgrade HW Components*/}
              {formData?.ruleActicvityDetails === 6 ||
              formData?.ruleActicvityDetails === 7 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData?.activityDetailsResource
                          )
                        }
                        value={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData?.activityDetailsResource
                          ).filter((x) => {
                            return (
                              x.value.description?.trim() ===
                              formData?.activityDetailsLcm?.trim()
                            );
                          })[0]
                        }
                        onChange={(e) =>
                          customChangeSelectForActivityDetails(
                            "activityDetailsLcm",
                            e
                          )
                        }
                        // onKeyUp={(e) =>
                        //   customChangeSelectForActivityDetails(
                        //     "activityDetailsLcm",
                        //     e
                        //   )
                        // }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) =>
                          option.value?.description != undefined
                            ? option.value?.description
                            : ""
                        }
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forLcm}
                      />
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(1)}
                          type="button"
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../../img/plus_icon.png")}
                            alt="plus"
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("activityDetailsLcm") ? (
                    <label className="validation">
                      *activity Details Lcm cannot be empty!
                    </label>
                  ) : null}
                </div>
              ) : null}
            </div>
          </div>

          <div className="col-12 row px-0">
            <label className="text-bb">Planning Settings</label>

            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Planning Risks
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData?.planningRiskResource)
                      }
                      value={
                        formData?.planningRiskResource &&
                        dictionaryToArray(
                          formData?.planningRiskResource
                        ).filter((el) =>
                          formData.planningRisksLcm?.includes(el.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("planningRisksLcm", e)
                      }
                      // onKeyUp={(e) => OnChangeMultiSelect("planningRisksNetworkElement", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forLcm}
                    />
                    {tipologicaPermesso && formData?.forLcm && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(4)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("planningRisksLCM") ? (
                <label className="validation">
                  *benefit Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className=" col-12 form-group p-0">
              <label className="labelForm voda-bold mb-0 w-100">
                Drivers
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData?.driverResource &&
                      dictionaryToArray(formData?.driverResource)
                    }
                    value={
                      formData?.driverResource &&
                      dictionaryToArray(formData?.driverResource).filter((el) =>
                        formData.driverTextLcm?.includes(el.key)
                      )
                    }
                    onChange={(e) => OnChangeMultiSelect("driverTextLcm", e)}
                    // onKeyUp={(e) => customChangeSelect("driverTextNetworkElement", e)}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isMulti
                    getOptionLabel={(option) => option.value.toString()}
                    getOptionValue={(option) => option["key"].toString()}
                    isDisabled={
                      !formData?.forLcm ||
                      (formData?.forLcm && disabledSelect.includes("driverLCM"))
                    }
                  />
                  {tipologicaPermesso && formData?.forLcm && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(3)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("driverLCM") ? (
                <label className="validation">
                  *driver Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Benefits
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource)
                      }
                      value={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource).filter(
                          (x) => formData?.benefitTextLcm?.includes(x.key)
                        )
                      }
                      onChange={(e) => OnChangeMultiSelect("benefitTextLcm", e)}
                      // onKeyUp={(e) => customChangeSelect("benefitTextNetworkElement", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forLcm ||
                        (formData?.forLcm &&
                          disabledSelect.includes("benefitLCM"))
                      }
                    />
                    {tipologicaPermesso && formData?.forLcm && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(2)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("benefitTextNetworkElement") ? (
                <label className="validation">
                  *benefit Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className="col-12 from-group p-0">
              <label className="labelForm voda-bold mb-0 mt-50">
                Exportable to LCM DB?
              </label>
              <div className="w-100">
                <div className="radio-content flex-mode startFlex">
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="exportable"
                    label="Yes"
                    checked={formData?.exportable ? true : false}
                    onChange={() => onChangeExportable(true, "LCM")}
                  />
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="exportable"
                    label="No"
                    checked={!formData?.exportable ? true : false}
                    onChange={() => onChangeExportable(false, "LCM")}
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 row px-0">
            <label className="text-bb mb-40 mt-4 mt-50">
              LCM Label Settings
            </label>
            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Label for LCM/Hardware
                  <input
                    type="text"
                    disabled={!formData?.lcmHardware}
                    onChange={(e) => onChange("lcmLabelHardware", e)}
                    onKeyUp={(e) => onChange("lcmLabelHardware", e)}
                    className="inputForm w-100"
                    value={
                      formData?.lcmHardware == true
                        ? formData?.lcmLabelHardware
                        : ""
                    }
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("lcmLabelHw") ? (
                  <label className="validation">
                    *Lcm Label HW is required
                  </label>
                ) : null}
              </div>
            </div>

            <div className="col-6 pr-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Label for LCM/Software
                  <input
                    type="text"
                    disabled={!formData?.lcmSoftware}
                    onChange={(e) => onChange("lcmLabelSoftware", e)}
                    onKeyUp={(e) => onChange("lcmLabelSoftware", e)}
                    className="inputForm w-100"
                    value={
                      formData?.lcmSoftware == true
                        ? formData?.lcmLabelSoftware
                        : ""
                    }
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("lcmLabelSw") ? (
                  <label className="validation">
                    *Lcm Label SW is required
                  </label>
                ) : null}
              </div>
            </div>
            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Activity Details
                  <input
                    type="text"
                    onChange={(e) => onChange("activityDetailsLcm", e)}
                    onKeyUp={(e) => onChange("activityDetailsLcm", e)}
                    className="inputForm w-100"
                    value={formData?.activityDetailsLcm}
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("activityDetailsLcm") ? (
                  <label className="validation">
                    *Activity Details cannot be empty.
                  </label>
                ) : null}
              </div>
            </div>
          </div>
        </Tab>
        <Tab
          eventKey="forCreateAssets"
          disabled={!formData?.forAddAsset}
          title="For Add Assets"
          className="col-12"
        >
          <div className="col-12 px-0 row mt-4">
            <label className="text-bb">Planned Design Component Settings</label>
            <div className="col-12 p-0">
              <label>Requires a Planned Design Component?</label>
              <div className="w-100">
                <div className="radio-content flex-mode startFlex">
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="RequiredAddAsset"
                    label="Yes"
                    checked={
                      formData?.plannedDesignComponentRequiredAddAsset
                        ? true
                        : false
                    }
                    value="true"
                    onChange={() =>
                      onChangeRequiredPlanned(
                        "plannedDesignComponentRequiredAddAsset",
                        true
                      )
                    }
                  />
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="RequiredAddAsset"
                    label="No"
                    value="false"
                    checked={
                      !formData?.plannedDesignComponentRequiredAddAsset
                        ? true
                        : false
                    }
                    onChange={() =>
                      onChangeRequiredPlanned(
                        "plannedDesignComponentRequiredAddAsset",
                        false
                      )
                    }
                  />
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">Add Asset Settings</label>
              <div className="row">
                <div className="col-6">
                  <div className="form-group w-100">
                    <label className="labelForm voda-bold w-100 mb-0">
                      Rule For Add Asset<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={props.rulesNetworkElement.map((x) => {
                              return { key: x.key, value: x.value };
                            })}
                            value={props.rulesNetworkElement
                              .filter((x) => x.key === formData?.ruleAddAsset)
                              .map((x) => {
                                return { key: x.key, value: x.value };
                              })}
                            onChange={(e) => onChangeSelect("ruleAddAsset", e)}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={!formData?.forAddAsset}
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("ruleAddAsset") ? (
                        <label className="validation">
                          *Rule Add Asset must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  <label className="w-100 mb-0 voda-bold">
                    Element Type<span className="red">*</span>
                  </label>
                  <label className="labelForm voda-bold w-100 mb-0">
                    <div className="row mx-0 w-100 form-group mb-0">
                      <div className="mr-4">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forAddAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "onBareMetalAddAsset",
                                e.target.checked
                              )
                            }
                            checked={formData?.onBareMetalAddAsset}
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                          />
                          Native
                        </label>
                      </div>
                      <div className="">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forAddAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "onVirtualizedAddAsset",
                                e.target.checked
                              )
                            }
                            checked={formData?.onVirtualizedAddAsset}
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                          />
                          Virtual
                        </label>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("onVirtualizedAddAsset") ? (
                        <label
                          className="validation"
                          style={{ bottom: "-14px" }}
                        >
                          *On Bare Metal or on Virtualized must be true
                        </label>
                      ) : null}
                    </div>
                  </label>

                  <label className="w-100 mb-0 voda-bold mt-30">
                    Activity is applicable for<span className="red">*</span>
                  </label>
                  <div className="row mx-0 w-100 form-group">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <div className="mr-4">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forAddAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "addAssetHardware",
                                e.target.checked
                              )
                            }
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                            checked={formData?.addAssetHardware}
                          />
                          Hardware
                        </label>
                      </div>
                      <div className="">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forAddAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "addAssetSoftware",
                                e.target.checked
                              )
                            }
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                            checked={formData?.addAssetSoftware}
                          />
                          Software
                        </label>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("onHwSwAddAsset") ? (
                        <label
                          className="validation w-100"
                          style={{ bottom: "-5px" }}
                        >
                          *On Hardware or on software must be true
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">
                Planned Activity Settings<span className="red">*</span>
              </label>
              <div className="row">
                <div className="col-6">
                  <div className="form-group w-100">
                    <label className="labelForm voda-bold w-100 mb-0">
                      Rule for Activity Details<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={props.rulesActivityDetailsNetworkElement.map(
                              (x) => {
                                return { key: x.key, value: x.value };
                              }
                            )}
                            value={props.rulesActivityDetailsNetworkElement
                              .filter(
                                (x) =>
                                  x.key ===
                                  formData?.ruleActicvityDetailsAddAsset
                              )
                              .map((x) => {
                                return { key: x.key, value: x.value };
                              })}
                            onChange={(e) =>
                              onChangeSelect("ruleActicvityDetailsAddAsset", e)
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={!formData?.forAddAsset}
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes(
                        "ruleActicvityDetailsAddAsset"
                      ) ? (
                        <label className="validation w-100">
                          *Rule for Activity Details must has value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  {/* SE RULE ADNE è ADD/REMOVE HW COMPONENTS */}
                  {formData?.ruleActicvityDetailsAddAsset === 6 ? (
                    <div className="w-100 form-group">
                      <label className="labelForm voda-bold mb-0 w-100">
                        Activity Details Text
                        <div className="d-flex">
                          <Select
                            menuPosition={"fixed"}
                            className="w-100"
                            options={
                              props.addRemoveActivityDetailsDropdownOptions
                            }
                            value={props.addRemoveActivityDetailsDropdownOptions.filter(
                              (x) =>
                                x.value === formData?.activityDetailsAddAsset
                            )}
                            onChange={(e) =>
                              onChangeSelectString("activityDetailsAddAsset", e)
                            }
                            // onKeyUp={(e) =>
                            //   onChangeSelectString("activityDetailsAddAsset", e)
                            // }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          />
                        </div>
                      </label>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes(
                        "activityDetailsAddAsset"
                      ) ? (
                        <label className="validation">
                          *activity Details Add Asset cannot be empty!
                        </label>
                      ) : null}
                    </div>
                  ) : null}
                  {/* SE RULE ADNE è VIRTUALIZE SYSTEM O FIXED TEXT FROM ADMIN*/}

                  {formData?.ruleActicvityDetailsAddAsset === 1 ||
                  formData?.ruleActicvityDetailsAddAsset === 3 ? (
                    <div className="w-100 form-group">
                      <label className="labelForm voda-bold mb-0 w-100">
                        Activity Details Text
                        <div className="d-flex">
                          <Select
                            menuPosition={"fixed"}
                            className="w-100"
                            options={
                              formData?.activityDetailsResource &&
                              dictionaryToArrayActivityDetail(
                                formData?.activityDetailsResource
                              ).filter((x) =>
                                formData?.ruleActicvityDetailsAddAsset === 1
                                  ? x.value.forVirtualized === true
                                  : x.value.forVirtualized === false
                              )
                            }
                            value={
                              formData?.activityDetailsResource &&
                              dictionaryToArrayActivityDetail(
                                formData?.activityDetailsResource
                              ).filter((x) => {
                                return (
                                  x.value.description ===
                                  formData?.activityDetailsAddAsset
                                );
                              })
                            }
                            onChange={(e) =>
                              customChangeSelectForActivityDetails(
                                "activityDetailsAddAsset",
                                e
                              )
                            }
                            // onKeyUp={(e) =>
                            //   customChangeSelectForActivityDetails(
                            //     "activityDetailsAddAsset",
                            //     e
                            //   )
                            // }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) =>
                              option.value?.description != undefined
                                ? option.value?.description
                                : ""
                            }
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={
                              !formData?.forAddAsset ||
                              (formData?.forAddAsset &&
                                disabledSelect.includes("activityDetails"))
                            }
                          />
                          {tipologicaPermesso && formData?.forAddAsset && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsVisibleModalLookup(1)}
                              type="button"
                            >
                              <img
                                style={{ height: 15 }}
                                src={require("../../../img/plus_icon.png")}
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes(
                          "activityDetailsAddAsset"
                        ) ? (
                          <label className="validation">
                            *activity Details Add Asset cannot be empty!
                          </label>
                        ) : null}
                      </label>
                    </div>
                  ) : null}

                  {/* SE RULE ADNE è VIRTUALIZE SYSTEM (testo in piu nel caso il dc sia virtualized) */}
                  {formData?.ruleActicvityDetailsAddAsset === 1 ? (
                    <div className="row mx-0 w-100 form-group">
                      <div className="w-100">
                        <label className="labelForm voda-bold   mb-0 w-100">
                          Activity Details Text For Virtualized
                          <div className="d-flex">
                            <Select
                              menuPosition={"fixed"}
                              className="w-100"
                              options={
                                formData?.activityDetailsResource &&
                                dictionaryToArrayActivityDetail(
                                  formData?.activityDetailsResource
                                ).filter((x) => x.value.forVirtualized === true)
                              }
                              value={
                                formData?.activityDetailsResource &&
                                dictionaryToArrayActivityDetail(
                                  formData?.activityDetailsResource
                                ).filter(
                                  (x) =>
                                    x.value.description ===
                                    formData?.activityDetailsForVirtualizedAddAsset
                                )
                              }
                              onChange={(e) =>
                                customChangeSelectForActivityDetails(
                                  "activityDetailsForVirtualizedAddAsset",
                                  e
                                )
                              }
                              // onKeyUp={(e) =>
                              //   customChangeSelectForActivityDetails(
                              //     "activityDetailsForVirtualizedAddAsset",
                              //     e
                              //   )
                              // }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              getOptionLabel={(option) =>
                                option.value?.description != undefined
                                  ? option.value?.description
                                  : ""
                              }
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              isDisabled={
                                !formData?.forAddAsset ||
                                (formData?.forAddAsset &&
                                  disabledSelect.includes("activityDetails"))
                              }
                            />
                            {tipologicaPermesso && formData?.forAddAsset && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(1)}
                                type="button"
                              >
                                <img
                                  style={{ height: 15 }}
                                  src={require("../../../img/plus_icon.png")}
                                  alt="plus"
                                />
                              </button>
                            )}
                          </div>
                          {/* {formData?.activityDetailsForVirtualizedAddAsset &&
                          !isValidActivityforVirtualized ? (
                            <label className="validation mt-2">
                              *This field cannot be the same of Activity Details
                              Text
                            </label>
                          ) : null} */}
                        </label>
                      </div>
                    </div>
                  ) : null}
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">Planning Settings</label>
              <div className="w-100 form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  Benefits Text
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource)
                      }
                      value={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource).filter(
                          (x) => formData?.benefitTextAddAsset?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("benefitTextAddAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("benefitTextAddAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forAddAsset ||
                        (formData?.forAddAsset &&
                          disabledSelect.includes("benefit"))
                      }
                    />
                    {tipologicaPermesso && formData?.forAddAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(2)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("benefitTextAddAsset") ? (
                    <label className="validation">
                      *benefit Text Add Asset cannot be empty!
                    </label>
                  ) : null}
                </label>
              </div>

              <div className="w-100 form-group">
                <label className="labelForm voda-bold   mb-0 w-100">
                  Driver
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.driverResource &&
                        dictionaryToArray(formData?.driverResource)
                      }
                      value={
                        formData?.driverResource &&
                        dictionaryToArray(formData?.driverResource).filter(
                          (x) => formData?.driverTextAddAsset?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("driverTextAddAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("driverTextAddAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forAddAsset ||
                        (formData?.forAddAsset &&
                          disabledSelect.includes("driver"))
                      }
                    />
                    {tipologicaPermesso && formData?.forAddAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(3)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("driverTextAddAsset") ? (
                <label className="validation">
                  *driver Text Add Asset cannot be empty!
                </label>
              ) : null}
              <div className="w-100 form-group">
                <label className="labelForm voda-bold   mb-0 w-100">
                  Planning Risk
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData?.planningRiskResource)
                      }
                      value={
                        formData?.planningRiskResource &&
                        dictionaryToArray(
                          formData?.planningRiskResource
                        ).filter((el) =>
                          formData.planningRisksAddAsset?.includes(el.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("planningRisksAddAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("planningRisksAddAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forAddAsset}
                    />
                    {tipologicaPermesso && formData?.forAddAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(4)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("planningRisksAddAsset") ? (
                    <label className="validation">
                      *planning risk for add asset cannot be empty!
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
          </div>
        </Tab>
        <Tab
          eventKey="forEditAsset"
          disabled={!formData?.forEditAsset}
          title="For Edit Assets"
          className="col-12"
        >
          <div className="col-12 px-0 row mt-4">
            <label className="text-bb">Planned Design Component Settings</label>
            <div className="col-12 p-0">
              <label>Requires a Planned Design Component?</label>
              <div className="w-100">
                <div className="radio-content flex-mode startFlex">
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="RequiredEditAsset"
                    label="Yes"
                    checked={
                      formData?.plannedDesignComponentRequiredEditAsset
                        ? true
                        : false
                    }
                    value="true"
                    onChange={() =>
                      onChangeRequiredPlanned(
                        "plannedDesignComponentRequiredEditAsset",
                        true
                      )
                    }
                  />
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="RequiredEditAsset"
                    label="No"
                    value="false"
                    checked={
                      !formData?.plannedDesignComponentRequiredEditAsset
                        ? true
                        : false
                    }
                    onChange={() =>
                      onChangeRequiredPlanned(
                        "plannedDesignComponentRequiredEditAsset",
                        false
                      )
                    }
                  />
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">Edit Asset Settings</label>
              <div className="row">
                <div className="col-6">
                  <div className="form-group w-100">
                    <label className="labelForm voda-bold w-100 mb-0">
                      Rule For Edit Asset<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={props.rulesNetworkElement.map((x) => {
                              return { key: x.key, value: x.value };
                            })}
                            value={props.rulesNetworkElement
                              .filter((x) => x.key === formData?.ruleEditAsset)
                              .map((x) => {
                                return { key: x.key, value: x.value };
                              })}
                            onChange={(e) => onChangeSelect("ruleEditAsset", e)}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={!formData?.forEditAsset}
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("ruleEditAsset") ? (
                        <label className="validation">
                          *Rule Edit Asset must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold w-100 mb-0">
                    <label className="w-100 mb-0 voda-bold">
                      Element Type<span className="red">*</span>
                    </label>
                    <div className="row mx-0 w-100 form-group">
                      <div className="mr-4">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forEditAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "onBareMetalEditAsset",
                                e.target.checked
                              )
                            }
                            checked={formData?.onBareMetalEditAsset}
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                          />
                          Native
                        </label>
                      </div>
                      <div className="">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forEditAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "onVirtualizedEditAsset",
                                e.target.checked
                              )
                            }
                            checked={formData?.onVirtualizedEditAsset}
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                          />
                          Virtual
                        </label>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("onBareVirtualized") ? (
                        <label className="validation" style={{ bottom: "0" }}>
                          *On Bare Metal or on Virtualized must be true
                        </label>
                      ) : null}
                    </div>
                  </label>

                  <label className="w-100 mb-0 voda-bold">
                    Activity is applicable for<span className="red">*</span>
                  </label>
                  <label className="labelForm w-100 align-items-center">
                    <div className="row mx-0 w-100 form-group">
                      <div className="mr-4">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forEditAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "editAssetHardware",
                                e.target.checked
                              )
                            }
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                            checked={formData?.editAssetHardware}
                          />
                          Hardware
                        </label>
                      </div>
                      <div className="">
                        <label className="labelForm w-100 h-100 d-flex align-items-center">
                          <input
                            disabled={!formData?.forEditAsset}
                            type="checkbox"
                            onChange={(e) =>
                              onChangeCheckbox(
                                "editAssetSoftware",
                                e.target.checked
                              )
                            }
                            style={{ height: "15px" }}
                            className="inputForm mb-1 mr-1"
                            checked={formData?.editAssetSoftware}
                          />
                          Software
                        </label>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("onHwSwEditAsset") ? (
                        <label
                          className="validation w-100"
                          style={{ bottom: "0px" }}
                        >
                          *On Hardware or on software must be true
                        </label>
                      ) : null}
                    </div>
                  </label>
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">
                Planned Activity Settings<span className="red">*</span>
              </label>
              <div className="row">
                <div className="col-6">
                  <div className="form-group w-100">
                    <label className="labelForm voda-bold w-100 mb-0">
                      Rule for Activity Details<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={props.rulesActivityDetailsNetworkElement.map(
                              (x) => {
                                return { key: x.key, value: x.value };
                              }
                            )}
                            value={props.rulesActivityDetailsNetworkElement
                              .filter(
                                (x) =>
                                  x.key ===
                                  formData?.ruleActicvityDetailsEditAsset
                              )
                              .map((x) => {
                                return { key: x.key, value: x.value };
                              })}
                            onChange={(e) =>
                              onChangeSelect("ruleActicvityDetailsEditAsset", e)
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={!formData?.forEditAsset}
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes(
                        "ruleActicvityDetailsEditAsset"
                      ) ? (
                        <label className="validation">
                          *Rule must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  {/* SE RULE ADNE è ADD/REMOVE HW COMPONENTS */}
                  {formData?.ruleActicvityDetailsEditAsset === 6 ? (
                    <div className="w-100 form-group">
                      <label className="labelForm voda-bold mb-0 w-100">
                        Activity Details Text
                        <div className="d-flex">
                          <Select
                            menuPosition={"fixed"}
                            className="w-100"
                            options={
                              props.addRemoveActivityDetailsDropdownOptions
                            }
                            value={props.addRemoveActivityDetailsDropdownOptions.filter(
                              (x) =>
                                x.value === formData?.activityDetailsEditAsset
                            )}
                            onChange={(e) =>
                              onChangeSelectString(
                                "activityDetailsEditAsset",
                                e
                              )
                            }
                            // onKeyUp={(e) =>
                            //   onChangeSelectString(
                            //     "activityDetailsEditAsset",
                            //     e
                            //   )
                            // }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          />
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes(
                          "activityDetailsEditAsset"
                        ) ? (
                          <label className="validation">
                            *activity Details Network Element cannot be empty!
                          </label>
                        ) : null}
                      </label>
                    </div>
                  ) : null}
                  {/* SE RULE ADNE è VIRTUALIZE SYSTEM O FIXED TEXT FROM ADMIN*/}

                  {formData?.ruleActicvityDetailsEditAsset === 1 ||
                  formData?.ruleActicvityDetailsEditAsset === 3 ? (
                    <div className="w-100 form-group">
                      <label className="labelForm voda-bold mb-0 w-100">
                        Activity Details Text
                        <div className="d-flex">
                          <Select
                            menuPosition={"fixed"}
                            className="w-100"
                            options={
                              formData?.activityDetailsResource &&
                              dictionaryToArrayActivityDetail(
                                formData?.activityDetailsResource
                              ).filter((x) =>
                                formData?.ruleActicvityDetailsEditAsset === 1
                                  ? x.value.forVirtualized === true
                                  : x.value.forVirtualized === false
                              )
                            }
                            value={
                              formData?.activityDetailsResource &&
                              dictionaryToArrayActivityDetail(
                                formData?.activityDetailsResource
                              ).filter((x) => {
                                return (
                                  x.value.description ===
                                  formData?.activityDetailsEditAsset
                                );
                              })
                            }
                            onChange={(e) =>
                              customChangeSelectForActivityDetails(
                                "activityDetailsEditAsset",
                                e
                              )
                            }
                            // onKeyUp={(e) =>
                            //   customChangeSelectForActivityDetails(
                            //     "activityDetailsEditAsset",
                            //     e
                            //   )
                            // }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            getOptionLabel={(option) =>
                              option.value?.description != undefined
                                ? option.value?.description
                                : ""
                            }
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            isDisabled={
                              !formData?.forEditAsset ||
                              (formData?.forEditAsset &&
                                disabledSelect.includes("activityDetails"))
                            }
                          />
                          {tipologicaPermesso && formData?.forEditAsset && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsVisibleModalLookup(1)}
                              type="button"
                            >
                              <img
                                style={{ height: 15 }}
                                src={require("../../../img/plus_icon.png")}
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                      </label>
                    </div>
                  ) : null}

                  {/* SE RULE ADNE è VIRTUALIZE SYSTEM (testo in piu nel caso il dc sia virtualized) */}
                  {formData?.ruleActicvityDetailsEditAsset === 1 ? (
                    <div className="row mx-0 w-100 form-group">
                      <div className="w-100">
                        <label className="labelForm voda-bold   mb-0 w-100">
                          Activity Details Text For Virtualized
                          <div className="d-flex">
                            <Select
                              menuPosition={"fixed"}
                              className="w-100"
                              options={
                                formData?.activityDetailsResource &&
                                dictionaryToArrayActivityDetail(
                                  formData?.activityDetailsResource
                                ).filter((x) => x.value.forVirtualized === true)
                              }
                              value={
                                formData?.activityDetailsResource &&
                                dictionaryToArrayActivityDetail(
                                  formData?.activityDetailsResource
                                ).filter(
                                  (x) =>
                                    x.value.description ===
                                    formData?.activityDetailsForVirtualizedEditAsset
                                )
                              }
                              onChange={(e) =>
                                customChangeSelectForActivityDetails(
                                  "activityDetailsForVirtualizedEditAsset",
                                  e
                                )
                              }
                              // onKeyUp={(e) =>
                              //   customChangeSelectForActivityDetails(
                              //     "activityDetailsForVirtualizedEditAsset",
                              //     e
                              //   )
                              // }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              getOptionLabel={(option) =>
                                option.value?.description != undefined
                                  ? option.value?.description
                                  : ""
                              }
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              isDisabled={
                                !formData?.forEditAsset ||
                                (formData?.forEditAsset &&
                                  disabledSelect.includes("activityDetails"))
                              }
                            />
                            {tipologicaPermesso && formData?.forEditAsset && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(1)}
                                type="button"
                              >
                                <img
                                  style={{ height: 15 }}
                                  src={require("../../../img/plus_icon.png")}
                                  alt="plus"
                                />
                              </button>
                            )}
                          </div>
                          {/* {formData?.activityDetailsForVirtualizedEditAsset &&
                      !isValidActivityforVirtualized ? (
                        <label className="validation mt-2">
                          *This field cannot be the same of Activity Details
                          Text{" "}
                        </label>
                      ) : null} */}
                        </label>
                      </div>
                    </div>
                  ) : null}
                </div>
              </div>
            </div>

            <div className="col-12 p-0">
              <label className="text-bb mb-40 mt-50">Planning Settings</label>
              <div className="w-100 form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  Benefits Text
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource)
                      }
                      value={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource).filter(
                          (x) => formData?.benefitTextEditAsset?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("benefitTextEditAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("benefitTextEditAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forEditAsset ||
                        (formData?.forEditAsset &&
                          disabledSelect.includes("benefit"))
                      }
                    />
                    {tipologicaPermesso && formData?.benefitTextEditAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(2)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("benefitTextEditAsset") ? (
                <label className="validation">
                  *benefit Text Edit Asset cannot be empty!
                </label>
              ) : null}
              <div className="w-100 form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  Driver
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.driverResource &&
                        dictionaryToArray(formData?.driverResource)
                      }
                      value={
                        formData?.driverResource &&
                        dictionaryToArray(formData?.driverResource).filter(
                          (x) => formData?.driverTextEditAsset?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("driverTextEditAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("driverTextEditAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forEditAsset ||
                        (formData?.forEditAsset &&
                          disabledSelect.includes("benefit"))
                      }
                    />
                    {tipologicaPermesso && formData?.forEditAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(3)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("driverTextEditAsset") ? (
                <label className="validation">
                  *driver Text Edit Asset cannot be empty!
                </label>
              ) : null}
              <div className="w-100 form-group">
                <label className="labelForm voda-bold   mb-0 w-100">
                  Planning Risk
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData?.planningRiskResource)
                      }
                      value={
                        formData?.planningRiskResource &&
                        dictionaryToArray(
                          formData?.planningRiskResource
                        ).filter((el) =>
                          formData.planningRisksAEditAsset?.includes(el.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("planningRisksAEditAsset", e)
                      }
                      // onKeyUp={(e) =>
                      //   OnChangeMultiSelect("planningRisksAEditAsset", e)
                      // }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forEditAsset}
                    />
                    {tipologicaPermesso && formData?.forEditAsset && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(4)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("planningRisksAEditAsset") ? (
                <label className="validation">
                  *Planning Risks Edit Asset cannot be empty!
                </label>
              ) : null}
            </div>
          </div>
        </Tab>
        <Tab
          eventKey="forserviceplan"
          disabled={!formData?.forserviceplan}
          title="For Service"
          className="col-12"
        >
          <div className="col-12 row px-0 mt-4">
            <div className="col-12 p-0">
              <label className="text-bb">Planned Activity Settings</label>
            </div>

            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold w-100 mb-0">
                  Rule For Activity Details<span className="red">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={props.rules.map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                        value={props.rules
                          .filter(
                            (x) => x.key === formData?.ruleActicvityDetails
                          )
                          .map((x) => {
                            return { key: x.key, value: x.value };
                          })}
                        onChange={(e) =>
                          onChangeSelect("ruleActicvityDetails", e)
                        }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value.toString()}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forserviceplan}
                      ></Select>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("ruleActicvityDetails") ? (
                        <label className="validation">
                          *Rule must have a value
                        </label>
                      ) : null}
                    </div>
                  </div>
                </label>
              </div>

              {formData?.ruleActicvityDetails === 8 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={props.addRemoveActivityDetailsDropdownOptions}
                        value={props.addRemoveActivityDetailsDropdownOptions.filter(
                          (x) => x.value === formData?.activityDetailsService
                        )}
                        onChange={(e) =>
                          onChangeSelectString("activityDetailsService", e)
                        }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forserviceplan}
                      />
                    </div>
                  </label>
                </div>
              ) : null}

              {formData?.ruleActicvityDetails === 6 ||
              formData?.ruleActicvityDetails === 7 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData.activityDetailsResource
                          )
                        }
                        value={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData.activityDetailsResource
                          ).filter(
                            (x) =>
                              x.value.description ===
                              formData?.activityDetailsService
                          )
                        }
                        onChange={(e) =>
                          customChangeSelectForActivityDetails(
                            "activityDetailsService",
                            e
                          )
                        }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) =>
                          option.value?.description ?? ""
                        }
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forserviceplan}
                      />
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(1)}
                          type="button"
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../../img/plus_icon.png")}
                            alt="plus"
                          />
                        </button>
                      )}
                    </div>
                  </label>
                </div>
              ) : null}
            </div>
          </div>

          <div className="col-12 row px-0">
            <label className="text-bb">Planning Settings</label>

            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Planning Risks
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData.planningRiskResource)
                      }
                      value={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData.planningRiskResource).filter(
                          (el) =>
                            formData.planningRiskServicePlan?.includes(el.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("planningRiskServicePlan", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forserviceplan}
                    />
                    {tipologicaPermesso && formData?.forserviceplan && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(4)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
            </div>

            <div className="col-12 form-group p-0">
              <label className="labelForm voda-bold mb-0 w-100">
                Drivers
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData?.driverResource &&
                      dictionaryToArray(formData.driverResource)
                    }
                    value={
                      formData?.driverResource &&
                      dictionaryToArray(formData.driverResource).filter((el) =>
                        formData.driverTextServicePlan?.includes(el.key)
                      )
                    }
                    onChange={(e) =>
                      OnChangeMultiSelect("driverTextServicePlan", e)
                    }
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isMulti
                    getOptionLabel={(option) => option.value.toString()}
                    getOptionValue={(option) => option["key"].toString()}
                    isDisabled={!formData?.forserviceplan}
                  />
                  {tipologicaPermesso && formData?.forserviceplan && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(3)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
              </label>
            </div>

            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Benefits
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.benefitResource &&
                        dictionaryToArray(formData.benefitResource)
                      }
                      value={
                        formData?.benefitResource &&
                        dictionaryToArray(formData.benefitResource).filter(
                          (x) =>
                            formData?.benefitTextServicePlan?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("benefitTextServicePlan", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forserviceplan}
                    />
                    {tipologicaPermesso && formData?.forserviceplan && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(2)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
            </div>
          </div>
        </Tab>

        <Tab
          eventKey="DA"
          title="Design Aspect"
          className="col-12"
          disabled={!formData?.forDesignAspect}
        >
          {/* <div className="col-12 px-0 row mt-4"> */}
          <div className="col-12 row px-0 mt-4">
            <div className="col-12 p-0">
              <label className="text-bb">Planned Activity Settings</label>
              <div className="row">
                <div className=" col-6 mb-3">
                  <label className="labelForm voda-bold w-100 mb-0 mt-4">
                    Activity is applicable for<span className="red">*</span>
                  </label>
                  <div className="flex justify-content-start mt-2">
                    <div className="mr-4">
                      <label className="labelForm w-100 h-100 d-flex align-items-center">
                        <input
                          disabled={!formData?.forDesignAspect}
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox(
                              "designAspectHardware",
                              e.target.checked
                            )
                          }
                          style={{ height: "15px" }}
                          className="inputForm mb-1 mr-1"
                          checked={formData?.designAspectHardware}
                        />
                        Hardware
                      </label>
                    </div>
                    <div className="">
                      <label className="labelForm w-100 h-100 d-flex align-items-center">
                        <input
                          disabled={!formData?.forDesignAspect}
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox(
                              "designAspectSoftware",
                              e.target.checked
                            )
                          }
                          style={{ height: "15px" }}
                          className="inputForm mb-1 mr-1"
                          checked={formData?.designAspectSoftware}
                        />
                        Software
                      </label>
                    </div>
                  </div>

                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("onHwSwDesignAspect") ? (
                    <label className="validation col-12">
                      *You must select at least one of these
                    </label>
                  ) : null}
                </div>

                <div className="col-6">
                  <label className="labelForm voda-bold w-100 mb-0 mt-4">
                    Element Type<span className="red">*</span>
                  </label>
                  <div className="row mx-0 w-100 form-group">
                    <div className="mr-4">
                      <label className="labelForm w-100 h-100 d-flex align-items-center">
                        <input
                          //disabled={!formData?.forNetworkElement}
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox(
                              "onBareMetalNetworkElement",
                              e.target.checked
                            )
                          }
                          //checked={formData?.onBareMetalNetworkElement}
                          style={{ height: "15px" }}
                          className="inputForm mb-1 mr-1"
                        />
                        Native
                      </label>
                    </div>
                    <div className="">
                      <label className="labelForm w-100 h-100 d-flex align-items-center">
                        <input
                          //disabled={!formData?.forNetworkElement}
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox(
                              "onVirtualizedNetworkElement",
                              e.target.checked
                            )
                          }
                          //checked={formData?.onVirtualizedNetworkElement}
                          style={{ height: "15px" }}
                          className="inputForm mb-1 mr-1"
                        />
                        Virtual
                      </label>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("onBareVirtualized") ? (
                      <label className="validation">
                        *On Bare Metal or on Virtualized must be true
                      </label>
                    ) : null}
                  </div>
                </div>
              </div>
            </div>
            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold w-100 mb-0">
                  Rule For Activity Details<span className="red">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={props.rules.map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                        value={props.rules
                          .filter((x) => x.key === formData?.ruleDesignAspect)
                          .map((x) => {
                            return { key: x.key, value: x.value };
                          })}
                        onChange={(e) => onChangeSelect("ruleDesignAspect", e)}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value.toString()}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forDesignAspect}
                      ></Select>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("ruleDesignAspect") ? (
                        <label className="validation">
                          *Rule must have a value
                        </label>
                      ) : null}
                    </div>
                  </div>
                </label>
              </div>
              {/* SE RULE LCM è ADD/REMOVE HW COMPONENTS */}
              {formData?.ruleDesignAspect === 8 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={props.addRemoveActivityDetailsDropdownOptions}
                        value={props.addRemoveActivityDetailsDropdownOptions.filter(
                          (x) =>
                            x.value === formData?.activityDetailsDesignAspect
                        )}
                        onChange={(e) =>
                          onChangeSelectString("activityDetailsDesignAspect", e)
                        }
                        // onKeyUp={(e) =>
                        //   onChangeSelectString("activityDetailsDesignAspect", e)
                        // }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forLcm}
                      />
                    </div>
                  </label>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes(
                    "activityDetailsDesignAspect"
                  ) ? (
                    <label className="validation">
                      *activity Details Design Aspect cannot be empty!
                    </label>
                  ) : null}
                </div>
              ) : null}
              {/* SE RULE LCM è FIXED TEXT FROM ADMIN O Upgrade HW Components*/}
              {formData?.ruleDesignAspect === 6 ||
              formData?.ruleDesignAspect === 7 ? (
                <div className="w-100 form-group">
                  <label className="labelForm voda-bold text-uppercase mb-0 w-100">
                    Activity Details Text
                    <div className="d-flex">
                      <Select
                        menuPosition={"fixed"}
                        className="w-100"
                        options={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData?.activityDetailsResource
                          )
                        }
                        value={
                          formData?.activityDetailsResource &&
                          dictionaryToArrayActivityDetail(
                            formData?.activityDetailsResource
                          ).filter((x) => {
                            return (
                              x.value.description ===
                              formData?.activityDetailsDesignAspect
                            );
                          })
                        }
                        onChange={(e) =>
                          customChangeSelectForActivityDetails(
                            "activityDetailsDesignAspect",
                            e
                          )
                        }
                        // onKeyUp={(e) =>
                        //   customChangeSelectForActivityDetails(
                        //     "activityDetailsDesignAspect",
                        //     e
                        //   )
                        // }
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) =>
                          option.value?.description != undefined
                            ? option.value?.description
                            : ""
                        }
                        getOptionValue={(option) => option["key"].toString()}
                        isDisabled={!formData?.forDesignAspect}
                      />
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(1)}
                          type="button"
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../../img/plus_icon.png")}
                            alt="plus"
                          />
                        </button>
                      )}
                    </div>
                  </label>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes(
                    "activityDetailsDesignAspect"
                  ) ? (
                    <label className="validation">
                      *activity Details Design Aspect cannot be empty!
                    </label>
                  ) : null}
                </div>
              ) : null}
            </div>
          </div>

          <div className="col-12 row px-0">
            <label className="text-bb">Planning Settings</label>

            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Planning Risks
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.planningRiskResource &&
                        dictionaryToArray(formData?.planningRiskResource)
                      }
                      value={
                        formData?.planningRiskResource &&
                        dictionaryToArray(
                          formData?.planningRiskResource
                        ).filter((el) =>
                          formData.planningRisksDesignAspect?.includes(el.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("planningRisksDesignAspect", e)
                      }
                      // onKeyUp={(e) => OnChangeMultiSelect("planningRisksNetworkElement", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={!formData?.forDesignAspect}
                    />
                    {tipologicaPermesso && formData?.forDesignAspect && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(4)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("planningRisksDesignAspect") ? (
                <label className="validation">
                  *benefit Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className=" col-12 form-group p-0">
              <label className="labelForm voda-bold mb-0 w-100">
                Drivers
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData?.driverResource &&
                      dictionaryToArray(formData?.driverResource)
                    }
                    value={
                      formData?.driverResource &&
                      dictionaryToArray(formData?.driverResource).filter((el) =>
                        formData.driverTextDesignAspect?.includes(el.key)
                      )
                    }
                    onChange={(e) =>
                      OnChangeMultiSelect("driverTextDesignAspect", e)
                    }
                    // onKeyUp={(e) => customChangeSelect("driverTextNetworkElement", e)}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isMulti
                    getOptionLabel={(option) => option.value.toString()}
                    getOptionValue={(option) => option["key"].toString()}
                    isDisabled={
                      !formData?.forDesignAspect ||
                      (formData?.forDesignAspect &&
                        disabledSelect.includes("driverDesignAspect"))
                    }
                  />
                  {tipologicaPermesso && formData?.forDesignAspect && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(3)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("driverDesignAspect") ? (
                <label className="validation">
                  *driver Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className="col-12 form-group p-0">
              <div className="w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Benefits
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource)
                      }
                      value={
                        formData?.benefitResource &&
                        dictionaryToArray(formData?.benefitResource).filter(
                          (x) =>
                            formData?.benefitTextDesignAspect?.includes(x.key)
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect("benefitTextDesignAspect", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                      isDisabled={
                        !formData?.forDesignAspect ||
                        (formData?.forDesignAspect &&
                          disabledSelect.includes("benefitDesignAspect"))
                      }
                    />
                    {tipologicaPermesso && formData?.forDesignAspect && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(2)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                </label>
              </div>
              {validation &&
              validation.response == false &&
              validation.property?.includes("benefitTextNetworkElement") ? (
                <label className="validation">
                  *benefit Text Network Element cannot be empty!
                </label>
              ) : null}
            </div>
            <div className="col-12 from-group p-0">
              <label className="labelForm voda-bold mb-0 mt-50">
                Exportable to LCM DB?
              </label>
              <div className="w-100">
                <div className="radio-content flex-mode startFlex">
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="designAspectExportable"
                    label="Yes"
                    checked={formData?.designAspectExportable ? true : false}
                    onChange={() => onChangeExportable(true, "DA")}
                  />
                  <Form.Check
                    type="radio"
                    className="radio"
                    name="designAspectExportable"
                    label="No"
                    checked={!formData?.designAspectExportable ? true : false}
                    onChange={() => onChangeExportable(false, "DA")}
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 row px-0">
            <label className="text-bb mb-40 mt-4 mt-50">
              Design Aspect Label Settings
            </label>
            <div className="col-6 pl-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Label for LCM/Hardware
                  <input
                    type="text"
                    disabled={!formData?.designAspectHardware}
                    onChange={(e) => onChange("designAspectLabelHardware", e)}
                    onKeyUp={(e) => onChange("designAspectLabelHardware", e)}
                    className="inputForm w-100"
                    value={
                      formData?.designAspectHardware == true
                        ? formData?.designAspectLabelHardware
                        : ""
                    }
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("designAspectLabelHw") ? (
                  <label className="validation">
                    *Lcm Label HW is required
                  </label>
                ) : null}
              </div>
            </div>

            <div className="col-6 pr-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Label for LCM/Software
                  <input
                    type="text"
                    disabled={!formData?.designAspectSoftware}
                    onChange={(e) => onChange("DesignAspectLabelSoftware", e)}
                    onKeyUp={(e) => onChange("DesignAspectLabelSoftware", e)}
                    className="inputForm w-100"
                    value={
                      formData?.designAspectSoftware == true
                        ? formData?.designAspectLabelSoftware
                        : ""
                    }
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("designAspectLabelSw") ? (
                  <label className="validation">
                    *DesignAspect Label SW is required
                  </label>
                ) : null}
              </div>
            </div>
          </div>
        </Tab>
      </Tabs>

      {props.edit === true ? (
        <div className="col-12 row mt-3 form-group p-0">
          <div className=" col-6 mt-3 form-group">
            <div className="w-100">
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
          </div>
          <div className=" col-6 mt-3 form-group">
            <div className="w-100">
              <label className="labelForm voda-bold   w-100">
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
        </div>
      ) : null}

      <div className="col-12 justify-content-end d-flex ">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => preSave()}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default PlannedActivityResourceForm;
