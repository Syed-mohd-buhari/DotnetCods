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
  safeNumber,
} from "../../Hook/Common";
import { useSelector } from "react-redux";
import Select from "react-select";
import {
  ComponentSwBuildDtoCreate,
  ComponentSwBuildDtoUpdate,
} from "../../Model/ComponentSwBuild";
import {
  CreatComponentSwBuild,
  GetComponentSwBuildCreateResource,
} from "../../Redux/Action/ComponentSwBuild/ComponentSwBuildCreateAction";
import { EditComponentSwBuild } from "../../Redux/Action/ComponentSwBuild/ComponentSwBuildEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import ComponentManufacturer from "../../Containers/Lookup/ComponentManufactureContainer";
import OperatingSystemContainer from "../../Containers/Lookup/OperatingSystemContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import DatePicker from "react-datepicker";
import { TipologicaGridDto } from "../../Model/LookUp/LookUpGenericModel";
import { SubNetworkBoundaryGridDto } from "../../Model/LookUp/SubnetworkBoundry";
import CriticalAssetType from "../../Containers/Lookup/CriticalAssetTypeContainer";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";
import {
  GetComponentSwClonePreSubmit,
  GetSystemTypeForAddMajorSW,
} from "../../Redux/Action/ComponentSwBuild/ComponentSwBuildCommonAction";
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
import {
  DropdownInputComponent,
  TextInputComponent,
} from "../../Components/FormField";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: ComponentSwBuildDtoCreate,
      property: string
    );
    wizardBackFunction?(
      formData: ComponentSwBuildDtoCreate,
      property: string
    ): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  // data: ComponentSwBuildDtoUpdate | ComponentSwBuildDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
  softwareRedirect?: boolean;
  prevPage?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: ComponentSwBuildDtoCreate;
  onGetSwType?(type: string): void;
}

const ComponentSwModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("ComponentSwBuild");
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
  } = useFormTableCrud<ComponentSwBuildDtoUpdate>(
    CreatComponentSwBuild,
    EditComponentSwBuild
  );
  const dtoEditResourceState = (state: RootState) =>
    state.componentSwBuildEditReducer.ComponentSwBuildDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.componentSwBuildCreateReducer.ComponentSwBuildDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const [lookupFlag, setLookupFlag] = useState<string>("");
  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);

  const onHandelChangeAnnounced = (e: boolean) => {
    let copy = { ...formData } as ComponentSwBuildDtoCreate;
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
    let copy = { ...formData } as ComponentSwBuildDtoCreate;
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
      const copy = { ...props.dataWizard } as ComponentSwBuildDtoCreate;

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
      setKey("ComponentSwBuild");
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
      !numberIsNullOrZero(formData?.componentManufacturerId)
    ) {
      props.action.setDataCheck &&
        props.action.setDataCheck("swOemId", formData?.componentManufacturerId);
    }
  }, [formData?.componentManufacturerId]);

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
    formData?.componentManufacturerId,
    formData?.generaAvailableDate,
  ]);

  const { tipologicaPermesso } = useAuth();

  const validazioneClient = (copy: ComponentSwBuildDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.componentManufacturerId == null ||
      copy?.componentManufacturerId === undefined ||
      copy?.componentManufacturerId === 0
    ) {
      addInvalidProperty("componentManufacturerId");
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
    let copy = { ...formData } as ComponentSwBuildDtoUpdate;

    if (e && e["value"]) {
      copy.productName = e["value"];
      copy.productNameId = e["key"];
    } else {
      copy.productName = "";
      copy.productNameId = undefined;
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
    let copy = { ...formData } as ComponentSwBuildDtoUpdate;
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
          <ComponentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            lookUpFlag={lookupFlag}
          ></ComponentManufacturer>
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
      default:
        return;
    }
  };

  const onChangeEOM = (date: Date) => {
    let copy = { ...formData } as ComponentSwBuildDtoUpdate;

    if (date !== null) {
      copy["eomStatus"] = 2;
      copy[lowerFirstLetter("endOfMaintenance")] = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()}`;
      //copy[lowerFirstLetter("endOfMaintenance")] = date;
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

    let copy = { ...formData } as ComponentSwBuildDtoUpdate;

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
      const res: any = await GetComponentSwBuildCreateResource({
        isRefillData: true,
      });
      if (formData) {
        var obj: any = res?.componentManufacturerResource
          ? res?.componentManufacturerResource
          : [];
        setFormData({
          ...formData,
          componentManufacturerResource: obj,
        });
      }
    } catch (error) {
      console.error("Error in OriginalEquipmentManufacturerRefillData:", error);
    }
  };

  const OperatingSystemRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetComponentSwBuildCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.operatingSystemResource
        ? res?.operatingSystemResource
        : [];
      formData && setFormData({ ...formData, operatingSystemResource: obj });
    } catch (error) {
      console.error("Error in OperatingSystemRefillData:", error);
    }
  };

  const CriticalAssetTypeRefillData = async (
    value: TipologicaGridDto[] | undefined
  ) => {
    try {
      const res: any = await GetComponentSwBuildCreateResource({
        isRefillData: true,
      });
      var obj: any = res?.criticalAssetTypeResource
        ? res?.criticalAssetTypeResource
        : value ?? [];
      formData &&
        setFormData({
          ...formData,
          criticalAssetTypeResource: obj,
        });
    } catch (error) {
      console.error("Error in CriticalAssetTypeRefillData:", error);
    }
  };

  const onChangeCheckDeliveryMethod = (e: any) => {
    let checked = e.target.checked;
    setCheckDeliveryMethod(checked);
    if (!checked) {
      let copy = { ...formData } as ComponentSwBuildDtoUpdate;
      copy.deliveryMethod = "Traditional";
      setDatesGenerated(false);
      setDisabledEoSDate(false);
      setFormData(copy);
    }
  };

  // useEffect(() => {
  //   if (
  //     formData &&
  //     formData?.componentManufacturerId !== undefined &&
  //     formData?.componentManufacturerId !== null &&
  //     formData?.productNameId !== undefined &&
  //     formData?.productNameId !== null &&
  //     checkSWVersion &&
  //     !props.edit &&
  //     !props.wizardMode
  //   ) {
  //     let payload = {
  //       componentManufacturerId:
  //         formData?.componentManufacturerId,
  //       productNameId: formData?.productNameId,
  //       softwareVersion: 0,
  //     };
  //     callCheckSWVersion(payload);
  //   }
  // }, [
  //   formData?.componentManufacturerId,
  //   formData?.productNameId,
  //   checkSWVersion,
  // ]);

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
        resourceArrayRefactor(
          formData?.originalEquipmentManufacturerResource
        ).find((x) => x.key === formData?.componentManufacturerId)?.value;
      if (
        formData?.componentManufacturerId !== null &&
        deliveryString?.toLowerCase().includes("ericsson") &&
        checkDeliveryMethod
      ) {
        let copy = { ...formData } as ComponentSwBuildDtoUpdate;
        copy.deliveryMethod = "One Track";
        setDatesGenerated(true);
        if (copy && copy.generaAvailableDate) {
          copy.eomStatus = 2;
        }
        setFormData(copy);
      } else if (checkDeliveryMethod === true) {
        let copy = { ...formData } as ComponentSwBuildDtoUpdate;
        copy.deliveryMethod = "Traditional";
        setDatesGenerated(false);
        setFormData(copy);
      }
    }
  };

  const validateWizard = () => {
    let copy = { ...formData } as ComponentSwBuildDtoCreate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "componentSwBuildDto"
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

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as ComponentSwBuildDtoUpdate;
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

  console.log("formData", formData);
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
          maxWidth="md"
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
            <label className="text-bb">Component Software Description</label>
            <div className="row">
              <div className="col-6 pl-0">
                <div className="col-12">
                  <label className="voda-bold w-100 mt-2">
                    Component <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.componentManufacturerResource &&
                            resourceArrayRefactor(
                              formData?.componentManufacturerResource
                            )
                          }
                          value={
                            formData?.componentManufacturerResource &&
                            resourceArrayRefactor(
                              formData?.componentManufacturerResource
                            ).filter(
                              (x) => x.key == formData?.componentManufacturerId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("componentManufacturerId", e)
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
                              "/api/ComponentManufacturers"
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
                    validation.property?.includes("componentManufacturerId") ? (
                      <label className="validation">
                        *Oem must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>

              <div className="col-6 pl-3">
                <div className="col-12">
                  <label className="voda-bold w-100 mt-2">
                    Component Version
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
                        *Component Software Version must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>

              <div className="col-6 pl-0">
                <div className="col-12">
                  <label className="voda-bold w-100 mt-2 mb-0">
                    Design Contact
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.designContacts &&
                            resourceArrayRefactor(formData.designContacts)
                          }
                          value={
                            formData?.designContacts &&
                            resourceArrayRefactor(
                              formData?.designContacts
                            ).filter((x) =>
                              formData?.designContactIds?.includes(
                                safeNumber(x.key)
                              )
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
            </div>
          </fieldset>
          <Container show={true}>
            <fieldset className="fieldset mt-4">
              <legend className="text-bb">Lifecycle Management</legend>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <div className="mt-2">
                      <div className="flex">
                        <span className="labelForm voda-bold sp w-100 flex-basis-60">
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
                        <span className="labelForm voda-bold sp w-100 flex-basis-60">
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
                    <label className="voda-bold w-100 mt-2">
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
                    <label className="labelForm voda-bold w-100 mt-2">
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
                      <div className="form-group col-6 pl-0">
                        <div className="col-12 pl-0">
                          <DropdownInputComponent
                            label={"Software Family"}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-0"
                            isSearchable={true}
                            isClearable={true}
                            isAdd={tipologicaPermesso}
                            onAddClicked={() => setIsVisibleModalLookup(4)}
                            value={
                              formData &&
                              formData?.criticalAssetTypeResource != undefined
                                ? resourceArrayRefactor(
                                    formData.criticalAssetTypeResource
                                  ).filter(
                                    (x) =>
                                      x.key === formData?.criticalAssetTypeId
                                  )
                                : null
                            }
                            options={
                              formData?.criticalAssetTypeResource &&
                              resourceArrayRefactor(
                                formData?.criticalAssetTypeResource
                              )
                            }
                            onChange={(e: any) =>
                              onChangeSelect("criticalAssetTypeId", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-6 pr-0">
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label={`Vulnerability Status`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-0"
                            value={formData?.vulnerabilityStatus ?? ""}
                            required={false}
                            isError={
                              vulnerabilityError === true ||
                              (validation &&
                                validation.response === false &&
                                validation.property?.includes(
                                  "vulnerabilityStatus"
                                ))
                                ? true
                                : false
                            }
                            error={"*Maximum 2000 characters are only allowed."}
                            onChange={(e: any) =>
                              onChange("vulnerabilityStatus", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-6 pl-0">
                        <div className="col-12 pl-0">
                          <DropdownInputComponent
                            label={"Operating System"}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-0"
                            isSearchable={true}
                            isClearable={true}
                            isAdd={tipologicaPermesso}
                            onAddClicked={() => setIsVisibleModalLookup(3)}
                            value={
                              formData &&
                              formData?.operatingSystemResource != undefined
                                ? resourceArrayRefactor(
                                    formData.operatingSystemResource
                                  ).find(
                                    (x) => x.key === formData?.operatingSystemId
                                  )
                                : null
                            }
                            options={
                              formData?.operatingSystemResource &&
                              resourceArrayRefactor(
                                formData?.operatingSystemResource
                              )
                            }
                            onChange={(e: any) =>
                              onChangeSelect("operatingSystemId", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-12 px-0">
                        <div className="col-12 px-0">
                          <TextInputComponent
                            label={`Description`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.description ?? ""}
                            required={false}
                            isError={
                              descriptionError === true ||
                              (validation &&
                                validation.response === false &&
                                validation.property?.includes("description"))
                                ? true
                                : false
                            }
                            error={"*Maximum 2000 characters are only allowed."}
                            onChange={(e: any) => onChange("description", e)}
                          />
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
              Save(
                formData,
                props.edit,
                validazioneClient,
                refresh,
                RestoreOrphanDeleted,
                orphanDeleted
              );
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
                props.action.wizardBackFunction(formData, "componentSwBuildDto")
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
    </div>
  );
};

export default ComponentSwModal;
