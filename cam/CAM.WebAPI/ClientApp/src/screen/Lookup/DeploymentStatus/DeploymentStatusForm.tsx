import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime, numberIsNullOrZero } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatDeploymentStatus } from "../../../Redux/Action/LookUp/DeploymentStatus/DeploymentStatusCreateAction";
import { EditDeploymentStatus } from "../../../Redux/Action/LookUp/DeploymentStatus/DeploymentStatusEditAction";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Select from "react-select";
import { DeploymentStatusDto } from "../../../Model/LookUp/DeploymentStatus";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import { Form } from "react-bootstrap";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  rules: { key: number; value: string }[];
}

const DeploymentStatusForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");

  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    onChangeMultipleSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
  } = useFormTableCrud<DeploymentStatusDto>(
    CreatDeploymentStatus,
    EditDeploymentStatus
  );

  const dtoEditResourceState = (state: RootState) =>
    state.deploymentStatusEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.deploymentStatusCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (formData && formData.rule == 0) {
      let copy = { ...formData } as DeploymentStatusDto;
      copy.checkPlannedActivity = false;
      setFormData(copy);
    }
  }, [formData?.rule]);

  const validazioneClient = (copy: DeploymentStatusDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.deploymentStatusDescription === null ||
      copy?.deploymentStatusDescription === undefined ||
      copy?.deploymentStatusDescription.trim() === ""
    ) {
      addInvalidProperty("description");
    }
    if (copy?.rule === null || copy?.rule === undefined) {
      addInvalidProperty("rule");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeRadio = (type: string, checked: boolean | number) => {
    let copy = { ...formData } as DeploymentStatusDto;
    copy[type] = checked;
    setFormData(copy);
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as DeploymentStatusDto;
    if (e != null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
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
  };

  return (
    <div className="col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row col-12 px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Asset Deployment Status<span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("deploymentStatusDescription", e)}
                  onKeyUp={(e) => onChange("deploymentStatusDescription", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.deploymentStatusDescription}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("description") ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <label className="w-100 voda-bold">
              Is this a default Deployment Status?
            </label>
            <div className="w-100">
              <div className="radio-content flex-mode startFlex">
                <Form.Check
                  type="radio"
                  className="radio"
                  name="defaultValue"
                  label="Yes"
                  checked={formData?.defaultValue ? true : false}
                  value="true"
                  onChange={() => onChangeRadio("defaultValue", true)}
                />
                <Form.Check
                  type="radio"
                  className="radio"
                  name="defaultValue"
                  label="No"
                  value="false"
                  checked={!formData?.defaultValue ? true : false}
                  onChange={() => onChangeRadio("defaultValue", false)}
                />
              </div>
            </div>
            {/* <label className="labelForm voda-bold   w-100 my-0  d-flex align-items-center">
              <input
                type="checkbox"
                onChange={(e) => onChange("readOnlyPlannedActivity", e)}
                checked={formData?.readOnlyPlannedActivity ? true : false}
                className="inputForm checkboxList mb-1 mr-1"
              />
              Planned Activity read only
            </label> */}
          </div>
        </div>

        <div className="row col-12 px-0 mb-5">
          <fieldset className="w-100 row my-0 mt-2 mx-3 px-0">
            <legend className="text-bb mb-40">Planned Activity Settings</legend>
            <div className="col-12 p-0">
              <div className="row">
                <div className="col-6">
                  <label className="w-100 voda-bold">
                    Planned Activity is Read Only?
                  </label>
                  <div className="w-100">
                    <div className="radio-content flex-mode startFlex">
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="readonly"
                        label="Yes"
                        checked={
                          formData?.readOnlyPlannedActivity ? true : false
                        }
                        value="true"
                        onChange={() =>
                          onChangeRadio("readOnlyPlannedActivity", true)
                        }
                      />
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="readonly"
                        label="No"
                        value="false"
                        checked={
                          !formData?.readOnlyPlannedActivity ? true : false
                        }
                        onChange={() =>
                          onChangeRadio("readOnlyPlannedActivity", false)
                        }
                      />
                    </div>
                  </div>
                </div>
                <div className="col-6">
                  <label className="w-100 voda-bold">
                    Planned Activity is Required?
                  </label>
                  <div className="w-100">
                    <div className="radio-content flex-mode startFlex">
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="checkPlannedActivity"
                        label="Yes"
                        checked={formData?.checkPlannedActivity ? true : false}
                        value="true"
                        onChange={() =>
                          onChangeRadio("checkPlannedActivity", true)
                        }
                      />
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="checkPlannedActivity"
                        label="No"
                        value="false"
                        checked={!formData?.checkPlannedActivity ? true : false}
                        onChange={() =>
                          onChangeRadio("checkPlannedActivity", false)
                        }
                      />
                    </div>
                  </div>
                </div>

                <div className="col-6">
                  <label className="w-100 voda-bold mt-4">
                    Planned Action (Enable PA Tab)
                    <span className="red">*</span>
                  </label>
                  <div className="w-100">
                    <div className="radio-content flex-mode startFlex">
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="checkPlannedAction"
                        label="Yes"
                        checked={formData?.rule === 1 ? true : false}
                        value="true"
                        onChange={() => onChangeRadio("rule", 1)}
                        disabled={
                          formData?.deploymentStatusDescription?.toLowerCase() ===
                          "removed"
                        }
                      />
                      <Form.Check
                        type="radio"
                        className="radio"
                        name="checkPlannedAction"
                        label="No"
                        value="false"
                        checked={formData?.rule === 0 ? true : false}
                        onChange={() => onChangeRadio("rule", 0)}
                        disabled={
                          formData?.deploymentStatusDescription?.toLowerCase() ===
                          "removed"
                        }
                      />
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("rule") ? (
                      <label className="validation">
                        *Rule must have a value
                      </label>
                    ) : null}
                  </div>
                </div>

                {!formData?.readOnlyPlannedActivity ? (
                  <div className="col-6">
                    <div className="form-group mt-3">
                      <label className="labelForm voda-bold w-100 mb-0">
                        Allowed Planned Activity
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.plannedActivityResources &&
                                dictionaryToArray(
                                  formData?.plannedActivityResources
                                )
                              }
                              value={
                                formData?.plannedActivityResources &&
                                dictionaryToArray(
                                  formData?.plannedActivityResources
                                ).filter((x) => {
                                  return (
                                    formData &&
                                    formData?.plannedActivityResourceAllowedId?.indexOf(
                                      x.key
                                    ) != -1 &&
                                    formData?.plannedActivityResourceAllowedId?.indexOf(
                                      x.key
                                    ) != undefined
                                  );
                                })
                              }
                              onChange={(e) =>
                                OnChangeMultiSelect(
                                  "plannedActivityResourceAllowedId",
                                  e
                                )
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isMulti
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                            ></Select>
                          </div>
                        </div>
                      </label>
                      {/* {validation && validation.response == false && validation.property?.includes("rule") ? <label className="validation">*Rule must have a value</label> : null} */}
                    </div>
                  </div>
                ) : null}
              </div>
            </div>
          </fieldset>
          <div className="col-12">
            <div className="row">
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
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
                </div>
              ) : null}
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
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
                </div>
              ) : null}
            </div>
          </div>
        </div>
      </form>
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
          onClick={() => Save(formData, props.edit, validazioneClient, refresh)}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default DeploymentStatusForm;
