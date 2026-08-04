import React, { useState, useEffect, useCallback } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { dictionaryToArray } from "../../Hook/Dictionary";
import {
  formatDateWithTime,
  boolOptions,
  convertEnumToArray,
  safeNumber,
} from "../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  PlannedActivityTypeForEnum,
  SettingsUpdatePlannedActivityDtoCreate,
  SettingsUpdatePlannedActivityDtoUpdate,
} from "../../Model/SettingsUpdatePlannedActivity";
import Select from "react-select";
import {
  CreatSettingsUpdatePlannedActivity,
  GetSettingsUpdatePlannedActivityCreateResource,
} from "../../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityCreateAction";
import { EditSettingsUpdatePlannedActivity } from "../../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityEditAction";
import { RelatedResource } from "../../Model/CommonModels";
import { useAuth } from "../../Hook/useAuth";
import SharedLookUp from "../../Containers/Lookup/SharedLookUpContainer";
import { Modal } from "react-bootstrap";
import DeliveryStatusContainer from "../../Containers/Lookup/DeliveryStatusContainer";
import {
  GetCrossSettings,
  GetPATypeAndDeploymentStatus,
} from "../../Redux/Action/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityCommonAction";
import DeploymentStatus from "../../Containers/Lookup/DeploymentStatusContainer";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import {
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
  };
  edit: boolean;
  keyTab?: string;
  rulesResource: { key: number; value: string }[];
  rulesResourceElementCount: { key: number; value: string }[];
}

export interface CommonValidation {
  response: boolean;
  property: string[];
}

const SettingsUpdatePlannedActivityModal: React.FC<Props> = (props) => {
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
    confirmForm,
  } = useFormTableCrud<SettingsUpdatePlannedActivityDtoUpdate>(
    CreatSettingsUpdatePlannedActivity,
    EditSettingsUpdatePlannedActivity
  );
  const dtoEditResourceState = (state: RootState) =>
    state.SettingsUpdatePlannedActivityEditReducer
      .SettingsUpdatePlannedActivityDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.SettingsUpdatePlannedActivityCreateReducer
      .SettingsUpdatePlannedActivityDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [durationInWeeks, setDurationInWeeks] = useState(false);

  const [forceEdit, setForceEdit] = useState<boolean>(false);
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [successorPlannedActiviType, setSuccessorPlannedActivityType] =
    useState<boolean>(true);

  const milestoneOptions = [
    { key: 1, value: "MS1" },
    { key: 2, value: "MS2" },
    { key: 3, value: "MS3" },
    { key: 4, value: "MS4" },
  ];

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit || forceEdit) {
      setFormData({
        ...editResource,
        isMileStone: editResource?.isMileStone === "Yes" ? true : false,
      });
      setForceEdit(false);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (formData?.ruleforSuccessorPlannedActivityCreation) {
      setSuccessorPlannedActivityType(true);
    } else {
      setSuccessorPlannedActivityType(false);
    }
  }, [formData?.ruleforSuccessorPlannedActivityCreation]);

  useEffect(() => {
    const copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;

    if (
      formData?.plannedActivityTypeFor !== undefined &&
      formData?.plannedActivityTypeFor !== null
    ) {
      if (formData?.rule === 0 && formData?.plannedActivityTypeFor === 0) {
        copy.ruleforSuccessorPlannedActivityCreation = false;
      }
      GetPATypeAndDeploymentStatus(formData?.plannedActivityTypeFor)
        .then((res) => {
          setDeploymentStatus(res?.data["deploymentStatus"]);
          setAssetDeploymentStatus(res?.data["assetDeploymentStatus"]);
          copy.planningActivityResource = res?.data["plannedActivityTypes"];
          setFormData(copy);
        })
        .catch((error) => {
          console.log(error);
          setFormData(copy);
        });
    }
  }, [formData?.plannedActivityTypeFor]);

  useEffect(() => {
    if (
      formData?.plannedActivityTypeFor !== null &&
      formData?.plannedActivityTypeFor !== undefined &&
      formData?.planningActivityResourceId &&
      formData?.deliveryStatusId
    ) {
      getCrossSettingsResource();
    }
  }, [
    formData?.plannedActivityTypeFor,
    formData?.planningActivityResourceId,
    formData?.deliveryStatusId,
  ]);

  const getCrossSettingsResource = async () => {
    const result = await GetCrossSettings(
      formData?.plannedActivityTypeFor!,
      formData?.planningActivityResourceId!,
      formData?.deliveryStatusId!
    );

    const copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    copy.crossSettingscResource = result;

    setFormData(copy);
  };

  const [validazioneCustom, setValidazioneCustom] = useState<{
    response: boolean;
    property?: string;
    message?: string;
  }>();

  const [deploymentStatus, setDeploymentStatus] = useState();
  const [assetDeploymentStatus, setAssetDeploymentStatus] = useState();

  const handleDurationType = (check) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (!check) {
      copy.msStatusDuration = formData?.msStatusDuration
        ? Math.round(formData?.msStatusDuration * 4)
        : 0;
    } else {
      const months = formData?.msStatusDuration
        ? formData?.msStatusDuration / 4
        : 0 / 4;
      copy.msStatusDuration = parseFloat(months.toFixed(1));
    }
    setFormData(copy);
  };

  const getMsStatusDuration = () => {
    let displayDuration;
    if (durationInWeeks) {
      displayDuration = formData?.msStatusDuration
        ? Math.round(formData?.msStatusDuration * 4)
        : 0;
    } else {
      const months = formData?.msStatusDuration
        ? formData?.msStatusDuration / 4
        : 0 / 4;
      displayDuration = parseFloat(months.toFixed(1));
    }
    return displayDuration;
  };

  const onChangeMaxOrder = (property: string, e: any) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoCreate;
    let value = e.target.value;

    if (value < 1) {
      setValidazioneCustom({ property, response: false });
      copy[property] = 1;
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

  const validazioneClient = (copy: SettingsUpdatePlannedActivityDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy.settingsUpdatePlannedActivityDescription === null ||
      copy.settingsUpdatePlannedActivityDescription === undefined ||
      copy.settingsUpdatePlannedActivityDescription.trim() === ""
    ) {
      addInvalidProperty("settingsUpdatePlannedActivityDescription");
    }

    if (
      copy.planningActivityStatusId === null ||
      copy.planningActivityStatusId === undefined ||
      copy.planningActivityStatusId === 0
    ) {
      addInvalidProperty("planningActivityStatusId");
    }

    if (
      formData?.planningActivityResource &&
      dictionaryToArray(formData?.planningActivityResource).filter(
        (x) =>
          x.key == formData?.planningActivityResourceId &&
          x.value == "Decommission Service Nodes"
      ).length > 0 &&
      (copy.assetDeploymentStatusIds === null ||
        copy.assetDeploymentStatusIds === undefined ||
        copy.assetDeploymentStatusIds[0] === 0)
    ) {
      addInvalidProperty("assetDeploymentStatusIds");
    }

    if (
      copy.plannedActivityTypeFor === null ||
      copy.plannedActivityTypeFor === undefined
    ) {
      addInvalidProperty("plannedActivityTypeFor");
    }

    if (
      (copy.ruleforSuccessorPlannedActivityCreation === null ||
        copy.ruleforSuccessorPlannedActivityCreation === undefined) &&
      copy.plannedActivityTypeFor === 0
    ) {
      addInvalidProperty("ruleforSuccessorPlannedActivityCreation");
    }

    if (
      (copy.specifyDC === null || copy.specifyDC === undefined) &&
      copy.plannedActivityTypeFor === 0
    ) {
      addInvalidProperty("specifyDC");
    }

    if (
      (copy.needPlannedAsset === null || copy.needPlannedAsset === undefined) &&
      copy.plannedActivityTypeFor === 0
    ) {
      addInvalidProperty("needPlannedAsset");
    }
    if (
      (copy.isRollback === null || copy.isRollback === undefined) &&
      copy.plannedActivityTypeFor === 0
    ) {
      addInvalidProperty("isRollback");
    }

    if (
      (copy.lcmDeploymentStatusIds === null ||
        copy.lcmDeploymentStatusIds === undefined ||
        copy.lcmDeploymentStatusIds.length === 0) &&
      (copy.plannedActivityTypeFor === 0 ||
        copy.plannedActivityTypeFor === null ||
        copy.plannedActivityTypeFor === undefined)
    ) {
      addInvalidProperty("lcmDeploymentStatusIds");
    }

    if (
      (copy.assetDeploymentStatusIds === null ||
        copy.assetDeploymentStatusIds === undefined ||
        copy.assetDeploymentStatusIds.length === 0) &&
      (copy.plannedActivityTypeFor === 1 ||
        copy.plannedActivityTypeFor === 2 ||
        copy.plannedActivityTypeFor === null ||
        copy.plannedActivityTypeFor === undefined)
    ) {
      addInvalidProperty("assetDeploymentStatusIds");
    }
    if (
      copy.budgetAvailabilityId === null ||
      copy.budgetAvailabilityId === undefined ||
      copy.budgetAvailabilityId === 0
    ) {
      addInvalidProperty("budgetAvailabilityId");
    }
    if (
      copy.deliveryStatusId === null ||
      copy.deliveryStatusId === undefined ||
      copy.deliveryStatusId === 0
    ) {
      addInvalidProperty("deliveryStatusId");
    }

    if (copy.ruleElementCount === null || copy.ruleElementCount === undefined) {
      addInvalidProperty("ruleElementCount");
    }

    if (
      (copy.successorPlannedActivityId === null ||
        copy.successorPlannedActivityId === undefined ||
        copy.successorPlannedActivityId === 0) &&
      copy.plannedActivityTypeFor === 0 &&
      successorPlannedActiviType
    ) {
      addInvalidProperty("successorPlannedActivityId");
    }

    if (
      copy.localApproval === null ||
      copy.localApproval === undefined ||
      copy.localApproval === ""
    ) {
      addInvalidProperty("localApproval");
    }

    if (
      copy.planningActivityResourceId === null ||
      copy.planningActivityResourceId === undefined ||
      copy.planningActivityResourceId === 0
    ) {
      addInvalidProperty("planningActivityResourceId");
    }
    if (
      copy.maxOrder === null ||
      copy.maxOrder === undefined ||
      copy?.maxOrder < 1 ||
      copy.maxOrder.toString() === ""
    ) {
      addInvalidProperty("maxOrder");
    }

    if (
      (copy.rule === null ||
        copy.rule === undefined ||
        copy.rule.toString() === "") &&
      copy?.plannedActivityTypeFor === 0
    ) {
      addInvalidProperty("rule");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    // let array = [] as Array<number>;
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoCreate;
    if (e && e["key"]) {
      // array.push(e[i].key);
      copy[property] = [e.key];
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

    setTimeout(() => {
      console.log(copy);
    }, 3000);
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const onChangeLocalApproval = (e: any) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (e && e["key"]) {
      copy.localApproval = e["key"];
    } else {
      copy.localApproval = "";
    }
    setFormData(copy);

    if (validation?.property?.includes("localApproval")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("localApproval");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangePAFor = (e) => {
    const copy = {
      ...createResource,
      plannedActivityTypeFor: e["key"],
      settingsUpdatePlannedActivityDescription: "",
    } as SettingsUpdatePlannedActivityDtoUpdate;

    setFormData(copy);
  };

  let ruleCross = [
    { key: 0, value: "Transfert all Nodes" },
    { key: 1, value: "Ask to user" },
  ];

  const OnChangeCrossSettings = (prop: string, index: number, e: any) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (copy.crossSettingsOutIds != undefined) {
      if (e && e.key != undefined) {
        copy.crossSettingsOutIds[index][prop] = e.key.toString();
      }
    } else {
      let arr = [] as RelatedResource[];
      if (e && e.key != undefined) {
        switch (prop) {
          case "value":
            arr.push({ id: "0", value: e.key.toString() });
            break;
          case "id":
            arr.push({ id: e.key.toString(), value: "0" });
            break;

          default:
            break;
        }
      }
      copy.crossSettingsOutIds = arr;
    }
    setFormData(copy);
  };

  const addCross = () => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (copy.crossSettingsOutIds != undefined) {
      copy.crossSettingsOutIds.push({ id: "0", value: "0" });
    } else {
      let arr = [] as RelatedResource[];
      arr.push({ id: "0", value: "0" });
      copy.crossSettingsOutIds = arr;
    }
    setFormData(copy);
  };

  const removeCross = (index: number) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (copy.crossSettingsOutIds != undefined) {
      if (copy.crossSettingsOutIds.length > 1) {
        copy.crossSettingsOutIds.splice(index, 1);
      } else {
        copy.crossSettingsOutIds = undefined;
      }
    }
    setFormData(copy);
  };

  const Restore = (id: number) => {
    setForceEdit(true);
    props.action.Edit && props.action.Edit(id);
  };

  const LcmDeploymentStatusRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetSettingsUpdatePlannedActivityCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.lcmDeploymentStatusResource
        ? res?.lcmDeploymentStatusResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData &&
        setFormData({ ...formData, lcmDeploymentStatusResource: obj });
    } catch (error) {
      console.error("Error in OperatingSystemRefillData:", error);
    }
  };

  const DeliveryStatusResourceRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetSettingsUpdatePlannedActivityCreateResource({
        isRefillData: true,
      });
      var obj: { [key: string]: string } = res?.deliveryStatusResource
        ? res?.deliveryStatusResource
        : value.reduce(
            (acc, item) => ({ ...acc, [item.id]: item.description }),
            {}
          ) ?? [];
      formData && setFormData({ ...formData, deliveryStatusResource: obj });
    } catch (error) {
      console.error("Error in OperatingSystemRefillData:", error);
    }
  };

  const DeploymentStatusRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.deploymentStatusId]: item.deploymentStatusDescription,
      }),
      {}
    );
    setDeploymentStatus(obj);
  };

  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 1:
          return (
            <DeploymentStatus
              returnObject={DeploymentStatusRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            />
          );

        case 2:
          return (
            <DeliveryStatusContainer
              returnObject={DeliveryStatusResourceRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></DeliveryStatusContainer>
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

  const onChangeSelecRule = (e: any): void => {
    const copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;

    if (e && e["value"]) {
      copy.rule = e["key"];
      if (copy.rule === 0) {
        copy.ruleforSuccessorPlannedActivityCreation = false;
      }
      setFormData(copy);
    }
  };

  const onChangeSpecifyDC = (e) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (e) {
      copy.specifyDC = e["key"] === 1 ? true : false;
      setFormData(copy);
    }
  };

  const onChangeNeedPlannedAsset = (e) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (e) {
      copy.needPlannedAsset = e["key"] === 1 ? true : false;
      setFormData(copy);
    }
  };

  const onChangeIsRollback = (e) => {
    let copy = { ...formData } as SettingsUpdatePlannedActivityDtoUpdate;
    if (e) {
      copy.isRollback = e["key"] === 1 ? true : false;
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
      <form id="formDesignComponent" onChange={() => setChanged(true)}>
        <div className="row col-12">
          <label
            className="text-bb"
            style={{ marginRight: "15px", marginLeft: "15px" }}
          >
            Description
          </label>
          <div className="col-md-12">
            <div className="form-group col-6 pl-0">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Planned Activity For<span className="red">*</span>
                </label>

                <Select
                  menuPosition={"fixed"}
                  options={convertEnumToArray(PlannedActivityTypeForEnum)}
                  value={convertEnumToArray(PlannedActivityTypeForEnum).filter(
                    (x) => x.key === formData?.plannedActivityTypeFor
                  )}
                  onChange={(e) => {
                    onChangeSelect("plannedActivityTypeFor", e);
                    onChangePAFor(e);
                  }}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  isDisabled={props?.edit}
                  getOptionLabel={(option) =>
                    option.value === "LcmEngineering"
                      ? "Lcm Engineering"
                      : option.value === "AddAsset"
                      ? "Add Asset"
                      : option.value === "EditAsset"
                      ? "Edit Asset"
                      : option.value === "DesignAspect"
                      ? "Design Aspect"
                      : option.value === "ServicePlan"
                      ? "Service Plan"
                      : option.value
                  }
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
                {validation &&
                validation.response === false &&
                validation.property?.includes("plannedActivityTypeFor") ? (
                  <label className="validation">
                    *Planned Activity For must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>
          <div className="col-md-6">
            <div className="form-group">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Planned Activity Type<span className="red">*</span>
                </label>
                <Select
                  menuPosition={"fixed"}
                  options={
                    formData?.planningActivityResource &&
                    dictionaryToArray(formData?.planningActivityResource)
                  }
                  value={
                    formData &&
                    formData?.planningActivityResource &&
                    dictionaryToArray(
                      formData?.planningActivityResource
                    ).filter(
                      (x) => x.key === formData?.planningActivityResourceId
                    )
                  }
                  onChange={(e) =>
                    onChangeSelect("planningActivityResourceId", e)
                  }
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  isDisabled={props.edit}
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
                {validation &&
                validation.response === false &&
                validation.property?.includes("planningActivityResourceId") ? (
                  <label className="validation">
                    *Planned Activity Type must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          <div className="col-md-6"></div>
          <div className="col-md-6">
            <div className="form-group">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Delivery Status<span className="red">*</span>
                </label>

                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={
                        formData?.deliveryStatusResource &&
                        dictionaryToArray(formData?.deliveryStatusResource)
                      }
                      value={
                        formData &&
                        formData?.deliveryStatusResource &&
                        dictionaryToArray(
                          formData?.deliveryStatusResource
                        ).filter((x) => x.key === formData?.deliveryStatusId)
                      }
                      onChange={(e) => onChangeSelect("deliveryStatusId", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  {tipologicaPermesso && (
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
                validation.response === false &&
                validation.property?.includes("deliveryStatusId") ? (
                  <label className="validation">
                    *Delivery Status must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          {formData?.plannedActivityTypeFor === 0 && (
            <div className="col-md-6">
              <label className="labelForm voda-bold w-100">
                LCM Deployment Status<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={dictionaryToArray(deploymentStatus!)}
                      value={
                        formData &&
                        dictionaryToArray(deploymentStatus!).filter((x) =>
                          formData?.plannedActivityTypeFor === 0
                            ? formData?.lcmDeploymentStatusIds?.[0] === x.key
                            : formData?.plannedActivityTypeFor === 1 ||
                              formData?.plannedActivityTypeFor === 2
                            ? formData.assetDeploymentStatusIds?.[0] === x.key
                            : null
                        )
                      }
                      onChange={(e) =>
                        OnChangeMultiSelect(
                          formData?.plannedActivityTypeFor === 0
                            ? "lcmDeploymentStatusIds"
                            : formData?.plannedActivityTypeFor === 1 ||
                              formData?.plannedActivityTypeFor === 2
                            ? "assetDeploymentStatusIds"
                            : "",
                          e
                        )
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      // isMulti
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(11)}
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
                validation.property?.includes("lcmDeploymentStatusIds") ? (
                  <label className="validation">
                    *Deployment Status must have a value
                  </label>
                ) : null}
              </label>
            </div>
          )}

          {formData?.planningActivityResource && (
            <div className="col-md-6">
              <label className="labelForm voda-bold w-100">
                Asset Status
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={dictionaryToArray(assetDeploymentStatus!)}
                      value={dictionaryToArray(assetDeploymentStatus!).filter(
                        (x) =>
                          formData?.assetDeploymentStatusIds?.[0] === x.key ??
                          null
                      )}
                      onChange={(e) =>
                        OnChangeMultiSelect("assetDeploymentStatusIds" ?? "", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      // isMulti
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  {tipologicaPermesso && (
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
                {/* {validation &&
                validation.response === false &&
                validation.property?.includes("assetDeploymentStatusIds") ? (
                  <label className="validation">
                    *Asset Status must have a value
                  </label>
                ) : null} */}
              </label>
            </div>
          )}
          <div className="col-6">
            <label className="labelForm voda-bold w-100">
              Description<span className="red">*</span>
              <input
                type="text"
                onChange={(e) =>
                  onChange("settingsUpdatePlannedActivityDescription", e)
                }
                onKeyUp={(e) =>
                  onChange("settingsUpdatePlannedActivityDescription", e)
                }
                value={formData?.settingsUpdatePlannedActivityDescription}
                className="inputForm w-100"
              />
              {validation &&
              validation.response === false &&
              validation.property?.includes(
                "settingsUpdatePlannedActivityDescription"
              ) ? (
                <label className="validation">
                  *Description must have a value
                </label>
              ) : null}
            </label>
          </div>

          <div className="form-group col-md-6">
            <label className="labelForm voda-bold w-100">
              Max Order<span className="red">*</span>
              <input
                min={1}
                minLength={1}
                type="number"
                onChange={(e) => onChangeMaxOrder("maxOrder", e)}
                onKeyUp={(e) => onChangeMaxOrder("maxOrder", e)}
                value={formData?.maxOrder}
                className="inputForm w-100"
              />
              {(validation &&
                validation.response === false &&
                validation.property?.includes("maxOrder")) ||
              (validazioneCustom?.response === false &&
                validation?.property?.includes("maxOrder")) ? (
                <label className="validation">
                  *Max order must have a valid number
                </label>
              ) : null}
            </label>
          </div>

          <label
            className="text-bb mb-40"
            style={{ marginRight: "15px", marginLeft: "15px" }}
          >
            Planned Activity Settings
          </label>

          <div className="col-md-6">
            <div className="form-group">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Budget Availability<span className="red">*</span>
                </label>
                <Select
                  menuPosition={"fixed"}
                  options={
                    formData?.budgetAvaibilityResource &&
                    dictionaryToArray(formData?.budgetAvaibilityResource)
                  }
                  value={
                    formData &&
                    formData?.budgetAvaibilityResource &&
                    dictionaryToArray(
                      formData?.budgetAvaibilityResource
                    ).filter((x) => x.key === formData?.budgetAvailabilityId)
                  }
                  onChange={(e) => onChangeSelect("budgetAvailabilityId", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
                {validation &&
                validation.response === false &&
                validation.property?.includes("budgetAvailabilityId") ? (
                  <label className="validation">
                    *Budget Availability must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          <div className="form-group col-md-6">
            <label className="labelForm voda-bold w-100">
              Local Approval<span className="red">*</span>
              <div className="d-flex">
                <div className="w-100">
                  <Select
                    menuPosition={"fixed"}
                    options={boolOptions}
                    value={
                      formData && formData.localApproval != undefined
                        ? boolOptions.find(
                            (x) => x.key === formData?.localApproval
                          )
                        : null
                    }
                    onChange={(e) => onChangeLocalApproval(e)}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option["key"]}
                  ></Select>
                </div>
              </div>
              {validation &&
              validation.response === false &&
              validation.property?.includes("localApproval") ? (
                <label className="validation">
                  *local Approval must have a value
                </label>
              ) : null}
            </label>
          </div>

          <div className="col-md-6">
            <div className="form-group">
              <label className="labelForm mb-0 w-100">
                <label className="labelForm mb-0 voda-bold">
                  Planning Status<span className="red">*</span>
                </label>
                <Select
                  menuPosition={"fixed"}
                  options={
                    formData?.planningActivityStatusResource &&
                    dictionaryToArray(formData?.planningActivityStatusResource)
                  }
                  value={
                    formData &&
                    formData?.planningActivityStatusResource &&
                    dictionaryToArray(
                      formData?.planningActivityStatusResource
                    ).filter(
                      (x) => x.key === formData?.planningActivityStatusId
                    )
                  }
                  onChange={(e) =>
                    onChangeSelect("planningActivityStatusId", e)
                  }
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
                {validation &&
                validation.response === false &&
                validation.property?.includes("planningActivityStatusId") ? (
                  <label className="validation">
                    *planning Activity Status must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          <label
            className="text-bb"
            style={{ marginRight: "15px", marginLeft: "15px" }}
          >
            Managing Rules
          </label>

          <div className="form-group col-12 mx-0">
            <div className="row">
              {formData?.plannedActivityTypeFor === 0 && (
                <>
                  <label className="form-group labelForm voda-bold w-100 col-6">
                    Rule<span className="red">*</span>
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={props.rulesResource}
                        value={
                          formData && formData.rule != undefined
                            ? props.rulesResource.find(
                                (x) => x.key == formData?.rule
                              )
                            : null
                        }
                        onChange={(e) => onChangeSelecRule(e)}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                      ></Select>
                      {validation &&
                      validation.response === false &&
                      validation.property?.includes("rule") ? (
                        <label className="validation">
                          *Rule must have a value
                        </label>
                      ) : null}
                    </div>
                  </label>
                </>
              )}

              <label className="labelForm voda-bold w-100 col-6">
                Rule Element Count<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={props.rulesResourceElementCount}
                      value={
                        formData && formData.ruleElementCount != undefined
                          ? props.rulesResourceElementCount.find(
                              (x) => x.key == formData?.ruleElementCount
                            )
                          : null
                      }
                      onChange={(e) => onChangeSelect("ruleElementCount", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("ruleElementCount") ? (
                      <label className="validation ml-17">
                        *ruleElementCount element count must have a value
                      </label>
                    ) : null}
                  </div>
                </div>
              </label>
              {formData?.plannedActivityTypeFor === 0 && (
                <>
                  <label className="labelForm voda-bold w-100 col-6">
                    Rule for Successor Planned Activity Creation
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          className="w-100"
                          options={[
                            { key: 1, value: "Yes" },
                            { key: 0, value: "No" },
                          ]}
                          value={
                            formData &&
                            [
                              { key: 1, value: "Yes" },
                              { key: 0, value: "No" },
                            ].filter(
                              (el) =>
                                (el.key === 1 ? true : false) ===
                                formData?.ruleforSuccessorPlannedActivityCreation
                            )
                          }
                          onChange={(e) => {
                            onChangeSelect(
                              "ruleforSuccessorPlannedActivityCreation",
                              e
                            );
                          }}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          isDisabled={formData?.rule === 0}
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key.toString()}
                        />
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "ruleforSuccessorPlannedActivityCreation"
                        ) ? (
                          <label className="validation ml-17">
                            *Rule for Successor Planned Activity Creation
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                  <div className="col-md-6">
                    <div className="form-group">
                      <label className="labelForm mb-0 w-100">
                        <label className="labelForm mb-0 voda-bold">
                          Successor Planned Activity Type
                          {successorPlannedActiviType && (
                            <span className="red">*</span>
                          )}
                        </label>
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.successorPlannedActivityTypeResource &&
                            dictionaryToArray(
                              formData?.successorPlannedActivityTypeResource
                            )
                          }
                          value={
                            formData &&
                            formData?.successorPlannedActivityTypeResource &&
                            dictionaryToArray(
                              formData?.successorPlannedActivityTypeResource
                            ).filter(
                              (x) =>
                                x.key === formData?.successorPlannedActivityId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("SuccessorPlannedActivityId", e)
                          }
                          isDisabled={
                            formData &&
                            !formData?.ruleforSuccessorPlannedActivityCreation
                          }
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                        {validation &&
                        validation.response === false &&
                        successorPlannedActiviType &&
                        validation.property?.includes(
                          "successorPlannedActivityId"
                        ) ? (
                          <label className="validation">
                            *Successor Planned Activity Type must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                  <label className="labelForm voda-bold w-100 col-6">
                    Specify Design Component
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          className="w-100"
                          options={[
                            { key: 1, value: "Yes" },
                            { key: 0, value: "No" },
                          ]}
                          value={
                            formData &&
                            [
                              { key: 1, value: "Yes" },
                              { key: 0, value: "No" },
                            ].filter((el) =>
                              formData?.specifyDC === true
                                ? el.key === 1
                                : el.key === 0
                            )
                          }
                          onChange={(e) => {
                            onChangeSpecifyDC(e);
                          }}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          // isDisabled={formData?.rule === 0}
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key.toString()}
                        />
                        {/* <Select
                          options={
                            formData?.budgetAvaibilityResource &&
                            dictionaryToArray(
                              formData?.budgetAvaibilityResource
                            )
                          }
                          value={
                            formData?.budgetAvaibilityResource &&
                            dictionaryToArray(
                              formData?.budgetAvaibilityResource
                            ).filter(
                              (x) =>
                                (x.key === 1 ? true : false) ===
                                formData?.specifyDC
                            )
                          }
                          onChange={(e) => onChangeSelect("specifyDC", e)}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select> */}
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes("specifyDC") ? (
                          <label className="validation ml-17">
                            *Rule for Specify Design Component
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                  <label className="labelForm voda-bold w-100 col-6">
                    Planned Asset Required
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          className="w-100"
                          options={[
                            { key: 1, value: "Yes" },
                            { key: 0, value: "No" },
                          ]}
                          value={
                            formData &&
                            [
                              { key: 1, value: "Yes" },
                              { key: 0, value: "No" },
                            ].filter((el) =>
                              formData?.needPlannedAsset === true
                                ? el.key === 1
                                : el.key === 0
                            )
                          }
                          onChange={(e) => {
                            onChangeNeedPlannedAsset(e);
                          }}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          // isDisabled={formData?.rule === 0}
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key.toString()}
                        />

                        {validation &&
                        validation.response === false &&
                        validation.property?.includes("needPlannedAsset") ? (
                          <label className="validation ml-17">
                            *Rule for Planned Asset Required
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                  <label className="labelForm voda-bold w-100 col-6">
                    Is Rollback Allowed
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          className="w-100"
                          options={[
                            { key: 1, value: "Yes" },
                            { key: 0, value: "No" },
                          ]}
                          value={
                            formData &&
                            [
                              { key: 1, value: "Yes" },
                              { key: 0, value: "No" },
                            ].filter((el) =>
                              formData?.isRollback === true
                                ? el.key === 1
                                : el.key === 0
                            )
                          }
                          onChange={(e) => {
                            onChangeIsRollback(e);
                          }}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          // isDisabled={formData?.rule === 0}
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key.toString()}
                        />

                        {validation &&
                        validation.response === false &&
                        validation.property?.includes("isRollback") ? (
                          <label className="validation ml-17">
                            *Rule for Is Rollback is required
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                </>
              )}
            </div>
          </div>
          <div className="col-12 px-0" style={{ display: "grid" }}>
            <label
              className="text-bb"
              style={{ marginRight: "15px", marginLeft: "15px" }}
            >
              Milestone Settings
            </label>
          </div>
          <div className="col-12 d-flex">
            <div className="col-6 d-flex pl-0">
              <label className="labelForm voda-bold w-100 col-6 pl-0">
                Please select the Milestone
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={milestoneOptions}
                      value={
                        formData &&
                        milestoneOptions.filter(
                          (el) => formData?.msStatus == el?.key?.toString()
                        )
                      }
                      onChange={(e) => onChangeSelect("msStatus", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      // isDisabled={formData?.rule === 0}
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option.key.toString()}
                    />
                  </div>
                </div>
              </label>
              <div className="col-6 pr-0">
                <div className="form-group">
                  <ToggleInputComponent
                    label={"Is it Milestone?"}
                    labelCSS="mb-2"
                    value={formData?.isMileStone ?? false}
                    required={false}
                    onChange={(e: any) => onChange("isMileStone", e)}
                  />
                </div>
              </div>
            </div>
            <div className="col-6 d-flex pr-0">
              <div className="col-6 pl-0">
                <TextInputComponent
                  label={`Duration in ${!durationInWeeks ? "Weeks" : "Months"}`}
                  labelCSS="mb-0"
                  inputCSS="labelForm voda-bold mb-2"
                  value={formData?.msStatusDuration}
                  onChange={(e: any) => onChange("msStatusDuration", e)}
                />
              </div>
              <div className="col-6 pr-0" style={{ display: "grid" }}>
                <label className="labelForm voda-bold mb-0">
                  Change Duration Type
                </label>
                <label className="labelForm voda-bold mb-4">
                  <div className="switchSmall ml-2">
                    <input
                      type="checkbox"
                      checked={durationInWeeks}
                      onChange={() => {
                        setDurationInWeeks(!durationInWeeks);
                        handleDurationType(!durationInWeeks);
                      }}
                      className="mr-1"
                    />
                    <span className="sliderSmall round"></span>
                  </div>
                </label>
              </div>
            </div>
          </div>
          <label
            className="text-bb"
            style={{ marginRight: "15px", marginLeft: "15px" }}
          >
            Delivery Status Actions
          </label>
          <div className="form-group col-12 pr-0">
            <div className="row w-100 p-0 mb-40">
              <div className="col-9">
                <label
                  className="labelForm voda-bold mb-0"
                  title="When you change status from the current one to one of those listed below, a new Lcm Engineering will be created with the same number of nodes as the original one"
                >
                  Setup Actions for Planned Activitiy Delivery Status
                </label>
              </div>
              <div className="col-3 pr-0">
                <button
                  type="button"
                  className="btn btn-link fr plus-b"
                  onClick={() => addCross()}
                >
                  <img
                    style={{ height: 20 }}
                    src={require("../../img/plus_icon.png")}
                    alt="+"
                  />
                </button>
              </div>
            </div>
            {formData?.crossSettingsOutIds?.map((c, i) => {
              return (
                <div className="d-flex mb-40" key={i}>
                  <div
                    className="w-50 pr-1 prr-2"
                    title="When you change status from the current one to one of those listed below, a new Lcm Engineering will be created with the same number of nodes as the original one"
                  >
                    <label className="labelForm mb-0 voda-bold">
                      For<span className="red">*</span>
                    </label>
                    <Select
                      menuPosition={"fixed"}
                      options={
                        formData?.crossSettingscResource &&
                        dictionaryToArray(formData?.crossSettingscResource)
                      }
                      value={
                        formData?.crossSettingscResource &&
                        dictionaryToArray(
                          formData?.crossSettingscResource
                        ).find((x) => x.key.toString() == c.id)
                      }
                      onChange={(e) => OnChangeCrossSettings("id", i, e)}
                      onBlur={() => setInputValue("")}
                      // isSearchable
                      // isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  <div
                    className="w-50 pl-1 plr-2"
                    title="When you change status from the current one to one of those listed below, a new Lcm Engineering will be created with the same number of nodes as the original one"
                  >
                    <label className="labelForm mb-0 voda-bold">
                      Do<span className="red">*</span>
                    </label>
                    <Select
                      menuPosition={"fixed"}
                      options={ruleCross}
                      value={ruleCross.find((x) => x.key.toString() == c.value)}
                      onChange={(e) => OnChangeCrossSettings("value", i, e)}
                      onBlur={() => setInputValue("")}
                      // isSearchable
                      // isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  <button
                    type="button"
                    style={{ paddingTop: "30px" }}
                    className="btn btn-link"
                    onClick={() => removeCross(i)}
                  >
                    <img
                      className="btnEdit"
                      src={require("../../img/delete.png")}
                    />
                  </button>
                </div>
              );
            })}
            {/* <div className="d-flex">
								<div className="w-100" title="When you change status from the current one to one of those listed below, a new Lcm Engineering will be created with the same number of nodes as the original one">
									<Select
										options={formData?.crossSettingscResource && dictionaryToArray(formData?.crossSettingscResource)}
										value={
											formData?.crossSettingscResource &&
											dictionaryToArray(formData?.crossSettingscResource).filter((x) => {
												return formData && formData?.crossSettingsOutIds?.indexOf(x.key) != -1 && formData?.crossSettingsOutIds?.indexOf(x.key) != undefined;
											})
										}
										onChange={(e) => OnChangeMultiSelect("crossSettingsOutIds", e)}
										onBlur={() => setInputValue("")}
										isMulti
										isSearchable
										isClearable
										getOptionLabel={(option) => option.value}
										getOptionValue={(option) => option["key"].toString()}
									></Select>
								</div>
							</div> */}
            {/* {validation &&
            validation.response === false &&
            validation.property?.includes("rule") ? (
              <label className="validation">*rule must have a value</label>
            ) : null} */}
          </div>

          {props.edit == true ? (
            <div className="row col-12 mx-0 px-0 pt-3">
              <div className="col-md-6 ">
                <div className="form-group">
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

              <div className="col-md-6">
                <div className="form-group">
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
        </div>
      </form>

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
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => {
            Save(
              {
                ...formData,
                isMileStone: formData?.isMileStone === true ? "Yes" : "No",
                msStatusDuration: !durationInWeeks
                  ? formData?.msStatusDuration
                  : formData?.msStatusDuration
                  ? Math.round(formData?.msStatusDuration * 4)
                  : 0,
              },
              props.edit,
              validazioneClient,
              refresh,
              Restore
            );
          }}
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default SettingsUpdatePlannedActivityModal;
