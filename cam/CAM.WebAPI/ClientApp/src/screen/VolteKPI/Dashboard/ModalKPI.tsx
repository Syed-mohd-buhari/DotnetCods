import { stringify } from "querystring";
import React, { useEffect, useReducer, useState } from "react";
import DatePicker from "react-datepicker";
import { useSelector } from "react-redux";
import Select from "react-select";
import ModalConfirm from "../../../Components/ModalConfirm";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import "../../../Css/VolteKPI.css";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import { useAuth } from "../../../Hook/useAuth";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import {
  VolteKPIDtoCreate,
  VolteKPIDtoUpdate,
  VolteKPIQueryObjectGrid,
} from "../../../Model/VolteKpi/VolteKPI";
import {
  CreatVolteKPI,
  GetVolteKPICreateResource,
} from "../../../Redux/Action/VolteKPI/VolteKPICreateAction";
import { EditVolteKPI } from "../../../Redux/Action/VolteKPI/VolteKPIEditAction";
import { RootState } from "../../../Redux/Store/rootStore";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
interface Props {
  action: {
    closeModal(): any;
    onSave(changeProposal: boolean, data: VolteKPIDtoCreate): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
  };
  edit: boolean;
  kpiTypeForEdit?: number;
  data?: VolteKPIDtoUpdate | null;
  keyTab?: string;
}

export interface FilteredKPIData {
  volteKPIType: number; //con una sola KPI ovvero quella selezionata
  opCoId: number; // con l'id dell'opco selezionato
  eoyTarget: number | undefined;
  ActualNumberOfRegisteredSubscribers: number | undefined;
  ActualNumberOfProvisionedSubscriber: number | undefined;
  actualValue: number | undefined;
  monthlyTarget: number | undefined;
  comment: string | undefined;
  month: number | undefined;
  year: number | undefined;
  targetValueChangeProposal: number | undefined;
  targetMonth: number | undefined;
  targetYear: number | undefined;
}

const ModalKPI: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    changed,
    validation,
    setValidation,
    setChanged,
    setInputValue,
    confirmForm,
  } = useFormTableCrud<VolteKPIDtoCreate>(CreatVolteKPI, EditVolteKPI);

  const dtoEditResourceState = (state: RootState) =>
    state.volteKPIEditReducer.VolteKPIDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.volteKPICreateReducer.VolteKPIDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //AGGIUNTI DA ME
  const [date, setDate] = useState<Date | null>(new Date());
  const [targetDate, setTargetDate] = useState<Date | null>(new Date());
  const [volteKPITypesOption, setVolteKPITypesOption] = useState<
    { key: number; value: string }[]
  >([]);
  const [opcoOption, setOpcoOption] = useState<any>();
  const [filteredData, setFilteredData] = useState<FilteredKPIData | null>(
    null
  );
  const [originalData, setOriginalData] = useState<VolteKPIDtoCreate | null>(
    null
  );

  const [volteKPITypesSelected, setVolteKPITypesSelected] =
    useState<any>();
  const [opcoSelected, setOpcoSelected] = useState<any>();
  const [changeProposal, setChangeProposal] = useState<boolean>(false);
  const [edit, setEdit] = useState<boolean>(false);
  const [editCall, setEditCall] = useState<boolean>(false); // per NON far avvenire la chiamata(quando cambio opco e data ) la prima volta

  const { KPIAdmin } = useAuth();

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    const listOfVolteKPITypesOption:
      | { key: number; value: string }[]
      | undefined = createResource?.volteKPITypeResource
      ? dictionaryToArray(createResource?.volteKPITypeResource)
      : undefined;
    const listOfOpco: any | undefined =
      createResource?.opCoResource
        ? dictionaryToArray(createResource?.opCoResource)
        : undefined;
    listOfVolteKPITypesOption &&
      setVolteKPITypesOption(listOfVolteKPITypesOption);
    setOpcoOption(listOfOpco);
    setFormData(createResource);
    setOriginalData(createResource);
  }, [createResource]);

  useEffect(() => {
    setEdit(props.edit);
    if (props.edit) {
      const listOfVolteKPITypesOption:
        | { key: number; value: string }[]
        | undefined = createResource?.volteKPITypeResource
        ? dictionaryToArray(createResource?.volteKPITypeResource)
        : undefined;
      const listOfOpco: any = createResource?.opCoResource
        ? dictionaryToArray(createResource?.opCoResource)
        : [{ key: 0, value: "" }];

      listOfVolteKPITypesOption &&
        setVolteKPITypesOption(listOfVolteKPITypesOption);
      setOpcoOption(listOfOpco);
      changeFilteredData();
      setFormData(createResource);
      const [selectedOpco] = listOfOpco?.filter(
        (x) => x.key == createResource?.opCoId
      ) as any;
      setOpcoSelected(selectedOpco);
      const [selectedVolteKpi] = listOfVolteKPITypesOption?.filter(
        (x) => x.key == props.kpiTypeForEdit
      ) as any;
      setVolteKPITypesSelected(selectedVolteKpi);
    } else {
      const listOfVolteKPITypesOption:
        | { key: number; value: string }[]
        | undefined = createResource?.volteKPITypeResource
        ? dictionaryToArray(createResource?.volteKPITypeResource)
        : undefined;
      const listOfOpco: any = createResource?.opCoResource
        ? dictionaryToArray(createResource?.opCoResource)
        : [{ key: 0, value: "" }];

      listOfVolteKPITypesOption &&
        setVolteKPITypesOption(listOfVolteKPITypesOption);
      setOpcoOption(listOfOpco);
    }

    if (props.edit || edit) {
      const newDate =
        createResource?.year &&
        createResource?.month &&
        new Date(createResource?.year, createResource?.month - 1);
      if (newDate && volteKPITypesSelected?.key !== 4) {
        setDate(newDate);
      }
      // else if (newDate && volteKPITypesSelected?.key === 4) {
      // 	setTargetDate(newDate);
      // }
    }
  }, []);

  useEffect(() => {
    setEdit(props.edit);
  }, [props.edit]);

  const validazioneClient = () => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      opcoSelected === null ||
      opcoSelected === undefined ||
      opcoSelected.length === 0
    ) {
      addInvalidProperty("opCo");
    }
    if (
      volteKPITypesSelected === null ||
      volteKPITypesSelected === undefined ||
      volteKPITypesSelected.length === 0
    ) {
      addInvalidProperty("volteKPITypes");
    }
    if (date === null || date === undefined) {
      addInvalidProperty("date");
    }
    // if (volteKPITypesSelected?.key == 2 ? false : edit && (filteredData?.eoyTarget === null || filteredData?.eoyTarget === undefined)) {
    // 	addInvalidProperty("eoyTarget");
    // }
    if (
      volteKPITypesSelected?.key === 3 || volteKPITypesSelected?.key === 4
        ? (filteredData?.eoyTarget ?? 0) > 100
        : (filteredData?.eoyTarget ?? 0) >= 1000000
    ) {
      addInvalidProperty("eoyTarget");
    }
    // if (volteKPITypesSelected?.key == 2 ? false : (!copyValidation.property.includes("volteKPITypes") && filteredData?.actualValue === null) || filteredData?.actualValue === undefined) {
    // 	addInvalidProperty("actualValue");
    // }
    // if (volteKPITypesSelected?.key == 2 ? false : !copyValidation.property.includes("volteKPITypes") && filteredData?.actualValue != undefined && filteredData?.actualValue >= 1000000) {
    // 	addInvalidProperty("actualValue");
    // }
    if (
      volteKPITypesSelected?.key === 3 || volteKPITypesSelected?.key === 4
        ? (filteredData?.actualValue ?? 0) > 100
        : (filteredData?.actualValue ?? 0) >= 1000000
    ) {
      addInvalidProperty("actualValue");
    }
    // if (volteKPITypesSelected?.key == 2 ? false : edit && (filteredData?.monthlyTarget === null || filteredData?.monthlyTarget === undefined) && KPIAdmin) {
    // 	addInvalidProperty("monthlyTarget");
    // }
    // if (volteKPITypesSelected?.key == 2 ? false : edit && filteredData?.monthlyTarget != undefined && filteredData?.monthlyTarget >= 1000000 && KPIAdmin) {
    // 	addInvalidProperty("monthlyTarget");
    // }
    if (
      volteKPITypesSelected?.key === 3 || volteKPITypesSelected?.key === 4
        ? (filteredData?.monthlyTarget ?? 0) > 100
        : (filteredData?.monthlyTarget ?? 0) >= 1000000
    ) {
      addInvalidProperty("monthlyTarget");
    }
    if (
      (changeProposal &&
        (filteredData?.targetValueChangeProposal === null ||
          filteredData?.targetValueChangeProposal === undefined)) ||
      (volteKPITypesSelected?.key === 3 || volteKPITypesSelected?.key === 4
        ? (filteredData?.targetValueChangeProposal ?? 0) > 100
        : (filteredData?.targetValueChangeProposal ?? 0) >= 1000000)
    ) {
      addInvalidProperty("targetValueChangeProposal");
    }
    if (
      (KPIAdmin ? changed : edit ? changed : changeProposal) &&
      (filteredData?.comment === null ||
        filteredData?.comment === undefined ||
        filteredData?.comment.trim() === "")
    ) {
      addInvalidProperty("comment");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  const onChangeDate = (newDate, property?: string) => {
    if (property === "targetDate") {
      removeValidation("targetDate");
      setTargetDate(newDate);
    } else {
      removeValidation("date");
      setDate(newDate);
    }
  };

  const changeSelected = (property: string, value: any) => {
    removeValidation(property);
    // Cambio volteKPITypes o opCo
    setFormData(originalData);
    property === "volteKPITypes"
      ? setVolteKPITypesSelected(value)
      : property === "opCo" && setOpcoSelected(value);
  };

  useEffect(() => {
    if (formData != undefined) {
      const copy = { ...formData } as VolteKPIDtoUpdate;
      copy.month = date?.getMonth() != null ? date?.getMonth() + 1 : undefined;
      copy.year = date?.getFullYear();
      copy.volteKPIType = volteKPITypesSelected?.key;
      copy.opCoId = opcoSelected?.key;
      changeFilteredData();
      setFormData(copy);
      multipleRemoveValidation(["comment", "targetValueChangeProposal"]);
      if (
        volteKPITypesSelected?.key === null ||
        volteKPITypesSelected?.key === undefined ||
        opcoSelected?.key === null ||
        opcoSelected?.key === undefined
      ) {
        resetFilteredData();
      }
    }
  }, [date, volteKPITypesSelected, opcoSelected]);

  useEffect(() => {
    multipleRemoveValidation(["comment", "targetValueChangeProposal"]);
    onSwitchProposalOff();
  }, [changeProposal]);

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

  const removeValidation = (property: string) => {
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
    if (property === "all" && validation?.property) {
      let copy = { ...validation, property: [...validation.property] };
      validation.property.map((el, idx) => copy.property.splice(idx, 1));
      setValidation(copy);
    }
  };

  const onSwitchProposalOff = () => {
    const newData = { ...filteredData } as FilteredKPIData;
    const copy = { ...formData } as VolteKPIDtoCreate;
    newData.targetValueChangeProposal = undefined;
    newData.comment = undefined;
    switch (volteKPITypesSelected?.key) {
      case 1:
        copy.kpiOneTargetValueChangeProposal = undefined;
        copy.kpiOneComment = undefined;
        break;
      case 3:
        copy.kpiThreeTargetValueChangeProposal = undefined;
        copy.kpiThreeComment = undefined;
        break;
      case 4:
        copy.kpiFourTargetValueChangeProposal = undefined;
        copy.kpiFourComment = undefined;
        break;
    }
    setFilteredData(newData);
    setFormData(copy);
  };

  const change = (property: string, value: string | undefined) => {
    if (edit) setChanged(true);
    removeValidation(property);
    const newData = { ...filteredData } as FilteredKPIData;
    const copy = { ...formData } as VolteKPIDtoCreate;
    // Set Dati da mostrare in modale
    if (property === "comment") {
      newData.comment = value != undefined && value != "" ? value : undefined;
    } else {
      newData[property] =
        value != undefined && value != "" && !isNaN(+value)
          ? +parseFloat(value).toFixed(3)
          : undefined;
    }

    newData.volteKPIType = volteKPITypesSelected?.key; //con una sola KPI ovvero quella selezionata
    newData.opCoId = opcoSelected?.key; // con l'id dell'opco selezionato
    newData.month =
      date?.getMonth() != undefined && date.getMonth() != null
        ? date?.getMonth() + 1
        : undefined;
    newData.year = date?.getFullYear();
    setFilteredData(newData);
    // -----------------Set formData per invio dati-----------------------

    // Riempio solo quelli interessati
    switch (volteKPITypesSelected?.key) {
      case 1:
        copy.kpiOneEoYTarget = newData.eoyTarget;
        copy.kpiOneActualValue = newData.actualValue;
        copy.kpiOneMonthlyTarget = newData.monthlyTarget;
        copy.kpiOneComment = newData.comment;
        copy.kpiOneTargetValueChangeProposal =
          newData.targetValueChangeProposal;
        break;
      case 2:
        copy.kpiTwoActualNumberOfRegisteredSubscribers =
          newData.ActualNumberOfRegisteredSubscribers;
        // copy.kpiTwoActualValue = newData.actualValue; come da task 473
        copy.kpiTwoActualNumberOfProvisionedSubscriber =
          newData.ActualNumberOfProvisionedSubscriber;
        copy.kpiTwoComment = newData.comment;

        break;
      case 3:
        copy.kpiThreeEoYTarget = newData.eoyTarget && newData.eoyTarget / 100;
        copy.kpiThreeActualValue =
          newData.actualValue && newData.actualValue / 100;
        copy.kpiThreeMonthlyTarget =
          newData.monthlyTarget && newData.monthlyTarget / 100;
        copy.kpiThreeComment = newData.comment;
        copy.kpiThreeTargetValueChangeProposal =
          newData.targetValueChangeProposal;

        break;
      case 4:
        copy.kpiFourEoYTarget = newData.eoyTarget;
        copy.kpiFourActualValue = newData.actualValue;
        copy.kpiFourMonthlyTarget = newData.monthlyTarget;
        copy.kpiFourComment = newData.comment;
        copy.kpiFourTargetValueChangeProposal =
          newData.targetValueChangeProposal;

        break;

      default:
        return;
    }
    copy.volteKPIType = volteKPITypesSelected.key; //una sola KPI ovvero quella selezionata
    copy.opCoId = opcoSelected?.key; // l'id dell'opco selezionato
    copy.month = date?.getMonth() != null ? date?.getMonth() + 1 : undefined;
    copy.year = date?.getFullYear();
    setFormData(copy);
  };

  useEffect(() => {
    changeFilteredData();
    setChanged(false);
  }, [volteKPITypesSelected, opcoSelected]);

  useEffect(() => {
    // ogni volta che formData cambia, filtro i dati per la KPI selezionata (se selezionata)
    changeFilteredData();
    if (
      formData?.volteKPIId != 0 &&
      formData?.volteKPIId != undefined &&
      formData?.volteKPIId != null
    ) {
      //se ce il KPI ID vuol dire che esiste gia
      switch (volteKPITypesSelected?.key) {
        case 1:
          if (
            originalData?.kpiOneActualValue != null &&
            originalData?.kpiOneActualValue != undefined
          ) {
            // setChangeProposal(false);
            !edit && setEdit(true); //Per cambiare il comportamento della modale solo se il dato esiste gia
          }
          break;
        case 2:
          if (
            originalData?.kpiTwoActualNumberOfRegisteredSubscribers != null &&
            originalData?.kpiTwoActualNumberOfRegisteredSubscribers != undefined
          ) {
            // setChangeProposal(false);
            !edit && setEdit(true); //Per cambiare il comportamento della modale solo se il dato esiste gia
          }
          break;
        case 3:
          if (
            originalData?.kpiThreeActualValue != null &&
            originalData?.kpiThreeActualValue != undefined
          ) {
            // setChangeProposal(false);
            !edit && setEdit(true); //Per cambiare il comportamento della modale solo se il dato esiste gia
          }
          break;
        case 4:
          if (
            originalData?.kpiFourActualValue != null &&
            originalData?.kpiFourActualValue != undefined
          ) {
            // setChangeProposal(false);
            !edit && setEdit(true); //Per cambiare il comportamento della modale solo se il dato esiste gia
          }
          break;
        default:
          break;
      }
    } else {
      setEdit(false);
    }
  }, [formData]);

  useEffect(() => {
    if (date != null && opcoSelected?.length != 0 && opcoSelected != null) {
      const dataForCall = {} as VolteKPIQueryObjectGrid;
      dataForCall.opCo = [opcoSelected?.key];
      dataForCall.month = date != null ? date?.getMonth() + 1 : undefined;
      dataForCall.year = date != null ? date?.getFullYear() : undefined;
      if (props.edit || edit) {
        // if (editCall) {
        // 	GetVolteKPICreateResource(dataForCall);
        // } else {
        // 	setEditCall(true);
        // }
        GetVolteKPICreateResource(dataForCall);
      } else {
        GetVolteKPICreateResource(dataForCall);
      }
    }
  }, [opcoSelected, date]);

  const onConfirmModal = () => {
    const isValid = validazioneClient().response;
    if (isValid) {
      if (formData != undefined && formData != null) {
        switch (volteKPITypesSelected?.key) {
          case 1:
            formData.kpiOneTargetValueChangeProposal =
              formData.kpiOneTargetValueChangeProposal !== null &&
              formData.kpiOneTargetValueChangeProposal !== undefined
                ? formData.kpiOneTargetValueChangeProposal
                : formData.kpiOneTargetValueChangeProposal;
            break;
          case 2:
            formData.kpiTwoActualNumberOfProvisionedSubscriber =
              formData.kpiTwoActualNumberOfProvisionedSubscriber ?? 0;
            formData.kpiTwoActualNumberOfRegisteredSubscribers =
              formData.kpiTwoActualNumberOfRegisteredSubscribers ?? 0;
            break;
          case 3:
            formData.kpiThreeTargetValueChangeProposal =
              formData.kpiThreeTargetValueChangeProposal !== null &&
              formData.kpiThreeTargetValueChangeProposal !== undefined
                ? formData.kpiThreeTargetValueChangeProposal / 100
                : formData.kpiThreeTargetValueChangeProposal;
            break;
          case 4:
            formData.kpiFourTargetValueChangeProposal =
              formData.kpiFourTargetValueChangeProposal !== null &&
              formData.kpiFourTargetValueChangeProposal !== undefined
                ? formData.kpiFourTargetValueChangeProposal / 100
                : formData.kpiFourTargetValueChangeProposal;
            break;
          default:
            break;
        }
        props.action.onSave(changeProposal, formData);
      }
      // !changeProposal && props.action.closeModal();
      props.action.closeModal();
    }
  };

  const changeFilteredData = () => {
    removeValidation("actualValue");
    let newData = {} as FilteredKPIData;
    if (
      volteKPITypesSelected != null &&
      volteKPITypesSelected != undefined &&
      (opcoSelected != null || opcoSelected != undefined)
    ) {
      switch (volteKPITypesSelected?.key) {
        case 1:
          newData.eoyTarget =
            formData?.kpiOneEoYTarget && +formData?.kpiOneEoYTarget?.toFixed(3);
          newData.actualValue =
            formData?.kpiOneActualValue &&
            +formData?.kpiOneActualValue.toFixed(3);
          newData.monthlyTarget =
            formData?.kpiOneMonthlyTarget &&
            +formData?.kpiOneMonthlyTarget.toFixed(3);
          newData.comment = formData?.kpiOneComment;
          newData.targetValueChangeProposal =
            formData?.kpiOneTargetValueChangeProposal;
          break;
        case 2:
          newData.ActualNumberOfRegisteredSubscribers =
            formData?.kpiTwoActualNumberOfRegisteredSubscribers &&
            +formData?.kpiTwoActualNumberOfRegisteredSubscribers.toFixed(3);
          // newData.actualValue = formData?.kpiTwoActualValue && +formData?.kpiTwoActualValue.toFixed(3);
          newData.ActualNumberOfProvisionedSubscriber =
            formData?.kpiTwoActualNumberOfProvisionedSubscriber &&
            +formData?.kpiTwoActualNumberOfProvisionedSubscriber.toFixed(3);
          newData.comment = formData?.kpiTwoComment;
          break;
        case 3:
          newData.eoyTarget =
            formData?.kpiThreeEoYTarget &&
            +(formData?.kpiThreeEoYTarget * 100).toFixed(3);
          newData.actualValue =
            formData?.kpiThreeActualValue &&
            +(formData?.kpiThreeActualValue * 100).toFixed(3);
          newData.monthlyTarget =
            formData?.kpiThreeMonthlyTarget &&
            +(formData?.kpiThreeMonthlyTarget * 100).toFixed(3);
          newData.comment = formData?.kpiThreeComment;
          newData.targetValueChangeProposal =
            formData?.kpiThreeTargetValueChangeProposal;
          break;
        case 4:
          newData.eoyTarget =
            formData?.kpiFourEoYTarget &&
            +formData?.kpiFourEoYTarget.toFixed(3);
          newData.actualValue =
            formData?.kpiFourActualValue &&
            +formData?.kpiFourActualValue.toFixed(3);
          newData.monthlyTarget =
            formData?.kpiFourMonthlyTarget &&
            +formData?.kpiFourMonthlyTarget.toFixed(3);
          newData.comment = formData?.kpiFourComment;
          newData.targetValueChangeProposal =
            formData?.kpiFourTargetValueChangeProposal;
          break;
        default:
          return;
      }
      newData.month = formData?.month;
      newData.year = formData?.year;
      setFilteredData(newData);
    } else {
      resetFilteredData();
    }
    // (newData.actualValue != undefined && newData.actualValue != null) && setEdit(true);
  };

  const resetFilteredData = () => {
    let data = {} as FilteredKPIData;
    data.eoyTarget = undefined;
    data.ActualNumberOfProvisionedSubscriber = undefined;
    data.ActualNumberOfRegisteredSubscribers = undefined;
    data.actualValue = undefined;
    data.monthlyTarget = undefined;
    data.comment = undefined;
    setFilteredData(data);
  };

  const returnLabel = (property: string) => {
    let label = "";
    if (property === "monthlyTarget") {
      switch (volteKPITypesSelected?.key) {
        case 1:
          label = "Provisioned Monthly Target [K SUBS]";
          break;
        case 2:
          label = "Actual Number Of Provisioned Subscriber [K-Subs] ";
          break;
        case 3:
          label = "Target Monthly Value (%)";
          break;
        case 4:
          label = "Target Monthly Value (%)";
          break;
        default:
          label = "Target Monthly Value";
      }
    }

    return label;
  };

  return (
    <div className="mt-2  py-4 col-12">
      <ModalConfirm data={confirmForm} />
      <form id="formVolteKPI" onChange={() => edit && setChanged(true)}>
        <div className="row col-12 px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  KPI<span className="red">*</span>
                </label>
                <Select
                menuPosition={"fixed"}
                  // isDisabled={edit && volteKPITypesSelected ? true : false}
                  options={volteKPITypesOption}
                  value={volteKPITypesSelected}
                  onChange={(e) => changeSelected("volteKPITypes", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("volteKPITypes") ? (
                  <label className="validation">
                    *You must select the KPI!
                  </label>
                ) : null}
              </label>
            </div>
          </div>
          {/* <div className={edit ? "disabledDate form-group col-6" : "form-group col-6"}> */}
          <div className="form-group col-6">
            <label className="labelForm voda-bold   w-100">
              Month<span className="red">*</span>
              <div className="d-flex">
                <div className="w-100">
                  <DatePicker
                    selected={date}
                    onChange={(newDate, e) => {
                      e.preventDefault();
                      onChangeDate(newDate);
                    }}
                    dateFormat="MMMM yyyy"
                    showMonthYearPicker
                    // className={edit ? " disabledBackground inputForm w-100" : "inputForm w-100 "}
                    className={"inputForm w-100 "}
                    maxDate={new Date()}
                  />

                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("date") ? (
                    <label className="validation">
                      *You must select a Date
                    </label>
                  ) : null}
                </div>
              </div>
            </label>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm    mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Opco<span className="red">*</span>
                </label>
                <Select
                menuPosition={"fixed"}
                  // isDisabled={edit}
                  options={opcoOption}
                  value={opcoSelected}
                  onChange={(e) => changeSelected("opCo", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option.key}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("opCo") ? (
                  <label className="validation">
                    *You must select the opco
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          {volteKPITypesSelected?.key === 4 && (
            <>
              <div
                className={
                  edit ? "disabledDate form-group col-6" : "form-group col-6"
                }
              >
                <label className="labelForm voda-bold   w-100">
                  Target Date:
                  <div className="d-flex">
                    <div className="w-100">
                      <DatePicker
                        selected={date}
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeDate(newDate, "targetDate");
                        }}
                        dateFormat="MMMM yyyy"
                        showMonthYearPicker
                        className={
                          edit
                            ? " disabledBackground inputForm w-100"
                            : "inputForm w-100 "
                        }
                        maxDate={new Date()}
                      />

                      {validation &&
                      validation.response === false &&
                      validation.property?.includes("date") ? (
                        <label className="validation">
                          *You must select a Date
                        </label>
                      ) : null}
                    </div>
                  </div>
                </label>
              </div>

              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold   mb-0 w-100">
                    Final target %
                    <input
                      type="number"
                      onChange={(e) => change("finalTarget", e.target.value)}
                      onKeyUp={(e) =>
                        change("finalTarget", e.currentTarget.value)
                      }
                      className="inputForm w-100"
                      // value={filteredData?.finalTarget}
                    />
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("finalTarget") ? (
                      <label className="validation">
                        {/* {filteredData?.finalTarget && filteredData?.finalTarget >= 100 ? "*This number must be smaller than 100 " : "*This Field cannot be empty"} */}
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            </>
          )}

          {/* {((volteKPITypesSelected?.key != 2 && edit && !KPIAdmin) || (volteKPITypesSelected?.key != 2 && !edit)) && ( */}
          {!KPIAdmin
            ? volteKPITypesSelected?.key != 2 && (
                <>
                  <div className="col-12 mt-3">
                    <div className="form-group">
                      <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                        <div className="switchContainer">
                          KPI Target Value Change Proposal
                          <label className="switch">
                            <input
                              disabled={
                                !volteKPITypesSelected?.key ||
                                !opcoSelected?.key
                              }
                              type="checkbox"
                              checked={changeProposal}
                              onChange={(e) =>
                                setChangeProposal(!changeProposal)
                              }
                            />
                            <span className="slider round"></span>
                          </label>
                        </div>
                      </label>
                    </div>
                  </div>

                  <div className="col-6">
                    <div className="form-group">
                      <label className="labelForm voda-bold   mb-0 w-100">
                        {`Target Monthly Value Proposal ${
                          volteKPITypesSelected?.key === 3 ||
                          volteKPITypesSelected?.key === 4
                            ? "(%)"
                            : ""
                        }`}
                        <input
                          type="number"
                          disabled={!changeProposal}
                          onChange={(e) =>
                            change("targetValueChangeProposal", e.target.value)
                          }
                          onKeyUp={(e) =>
                            change(
                              "targetValueChangeProposal",
                              e.currentTarget.value
                            )
                          }
                          onKeyPress={(e) => {
                            e = e || window.event;
                            var charCode =
                              typeof e.which == "undefined"
                                ? e.keyCode
                                : e.which;
                            var charStr = String.fromCharCode(charCode);
                            if (!charStr.match(/^[0-9]+$/)) e.preventDefault();
                          }}
                          className="inputForm w-100"
                          value={filteredData?.targetValueChangeProposal ?? ""}
                        />
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "targetValueChangeProposal"
                        ) ? (
                          <label className="validation">
                            {filteredData?.targetValueChangeProposal === null ||
                            filteredData?.targetValueChangeProposal ===
                              undefined
                              ? "*This Field cannot be empty"
                              : volteKPITypesSelected?.key === 3 &&
                                filteredData?.targetValueChangeProposal > 100
                              ? "*This number must be maximum 100 "
                              : filteredData?.targetValueChangeProposal &&
                                filteredData?.targetValueChangeProposal >=
                                  1000000
                              ? "*This number must be smaller than 1.000.000 "
                              : null}
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                </>
              )
            : null}
        </div>

        <div className="col-12 p-0">
          <label className="text-bb mb-40 px-0 mt-25">
            EoY Target Value [K Subs]
          </label>
          <div className="row">
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  {volteKPITypesSelected?.key === 3
                    ? "EoY Target (%)"
                    : volteKPITypesSelected?.key === 2
                    ? "Actual Number Of Registered Subscribers [K-Subs]"
                    : volteKPITypesSelected?.key === 1
                    ? "EoY Target [K SUBS]"
                    : "EoY Target"}
                  <input
                    type="number"
                    max={volteKPITypesSelected?.key === 3 ? 100 : undefined}
                    min={0}
                    disabled={
                      KPIAdmin ? false : volteKPITypesSelected?.key !== 2
                    }
                    onChange={(e) =>
                      change(
                        volteKPITypesSelected?.key === 2
                          ? "ActualNumberOfRegisteredSubscribers"
                          : "eoyTarget",
                        e.target.value
                      )
                    }
                    onKeyUp={(e) =>
                      change(
                        volteKPITypesSelected?.key === 2
                          ? "ActualNumberOfRegisteredSubscribers"
                          : "eoyTarget",
                        e.currentTarget.value
                      )
                    }
                    className="inputForm w-100"
                    value={
                      (volteKPITypesSelected?.key === 2
                        ? filteredData?.ActualNumberOfRegisteredSubscribers
                        : filteredData?.eoyTarget) ?? ""
                    }
                  />
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("eoyTarget") ? (
                    <label className="validation">
                      {filteredData?.eoyTarget === null ||
                      filteredData?.eoyTarget === undefined
                        ? "*This Field cannot be empty"
                        : volteKPITypesSelected?.key === 3 &&
                          filteredData?.eoyTarget > 100
                        ? "*This number must be maximum 100 "
                        : filteredData?.eoyTarget &&
                          filteredData?.eoyTarget >= 1000000
                        ? "*This number must be smaller than 1.000.000 "
                        : null}
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold   mb-0 w-100">
                  {returnLabel("monthlyTarget")}
                  <input
                    type="number"
                    pattern="[^b]+"
                    disabled={
                      KPIAdmin ? false : volteKPITypesSelected?.key !== 2
                    }
                    onChange={(e) =>
                      change(
                        volteKPITypesSelected?.key === 2
                          ? "ActualNumberOfProvisionedSubscriber"
                          : "monthlyTarget",
                        volteKPITypesSelected?.key === 3
                          ? Math.floor(parseFloat(e.target.value)).toString()
                          : e.target.value
                      )
                    }
                    onKeyUp={(e) =>
                      change(
                        volteKPITypesSelected?.key === 2
                          ? "ActualNumberOfProvisionedSubscriber"
                          : "monthlyTarget",
                        volteKPITypesSelected?.key === 3
                          ? Math.floor(
                              parseFloat(e.currentTarget.value)
                            ).toString()
                          : e.currentTarget.value
                      )
                    }
                    className="inputForm w-100"
                    value={
                      (volteKPITypesSelected?.key === 2
                        ? filteredData?.ActualNumberOfProvisionedSubscriber
                        : Math.floor(filteredData?.monthlyTarget ?? 0)) ?? ""
                    }
                  />
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("monthlyTarget") ? (
                    <label className="validation">
                      {filteredData?.monthlyTarget === null ||
                      filteredData?.monthlyTarget === undefined
                        ? "*This Field cannot be empty"
                        : volteKPITypesSelected?.key === 3 &&
                          filteredData?.monthlyTarget > 100
                        ? "*This number must be maximum 100 "
                        : (filteredData?.monthlyTarget &&
                            filteredData?.monthlyTarget) >= 1000000
                        ? "*This number must be smaller than 1.000.000 "
                        : null}
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
          </div>
        </div>

        <div className="col-12 p-0">
          {/* <label className="text-bb mb-40 mt-25">Monthly Result</label> */}
          <div className="row">
            {volteKPITypesSelected?.key != 2 && (
              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold mb-0 w-100">
                    {`Actual Monthly Value ${
                      volteKPITypesSelected?.key === 1 ? "[K SUBS]" : ""
                    }  ${volteKPITypesSelected?.key === 3 ? "(%)" : ""} :`}
                    <input
                      type="number"
                      onChange={(e) => change("actualValue", e.target.value)}
                      onKeyUp={(e) =>
                        change("actualValue", e.currentTarget.value)
                      }
                      className="inputForm w-100"
                      value={filteredData?.actualValue ?? ""}
                    />
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("actualValue") ? (
                      <label className="validation">
                        {filteredData?.actualValue === null ||
                        filteredData?.actualValue === undefined
                          ? "*This Field cannot be empty"
                          : volteKPITypesSelected?.key === 3 &&
                            filteredData?.actualValue > 100
                          ? "*This number must be maximum 100 "
                          : filteredData?.actualValue &&
                            filteredData?.actualValue >= 1000000
                          ? "*This number must be smaller than 1.000.000 "
                          : null}
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            )}
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  Comments
                  <input
                    type="text"
                    readOnly={
                      KPIAdmin ? !changed : edit ? !changed : !changeProposal
                    }
                    onChange={(e) => change("comment", e.target.value)}
                    onKeyUp={(e) => change("comment", e.currentTarget.value)}
                    className="inputForm w-100"
                    value={filteredData?.comment ?? ""}
                  />
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("comment") ? (
                    <label className="validation">
                      *Comments must have a value
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
          </div>
        </div>
      </form>

      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal()}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={onConfirmModal}
          type="button"
        >
          {edit ? "EDIT" : "Save"}
        </button>
      </div>
    </div>
  );
};

export default ModalKPI;
