import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatPlannedActivityNetworkElement } from "../../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementCreateAction";
import { EditPlannedActivityNetworkElement } from "../../../Redux/Action/LookUp/PlannedActivityNetworkElement/PlannedActivityNetworkElementEditAction";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { PlannedActivityNetworkElementDtoUpdate } from "../../../Model/LookUp/PlannedActivityNetworkElement";
import Select from "react-select";
import { dictionaryToArrayPlannedActivityResourceDto } from "../../../Hook/Dictionary";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  rules: {
    key: number;
    value: string;
  }[];
}

const PlannedActivityNetworkElementForm: React.FC<Props> = (props) => {
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
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
  } = useFormTableCrud<PlannedActivityNetworkElementDtoUpdate>(
    CreatPlannedActivityNetworkElement,
    EditPlannedActivityNetworkElement
  );

  const dtoEditResourceState = (state: RootState) =>
    state.plannedActivityNetworkElementEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.plannedActivityNetworkElementCreateReducer.LookUpDtoCreate;

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

  const validazioneClient = (copy: PlannedActivityNetworkElementDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.plannedActivityNetworkElementDescription === null ||
      copy?.plannedActivityNetworkElementDescription === undefined ||
      copy?.plannedActivityNetworkElementDescription.trim() === ""
    ) {
      addInvalidProperty("description");
    }
    // if (copy?.driverText === null || copy?.driverText === undefined || copy?.driverText.trim() === "") {
    // 	addInvalidProperty("driverText");
    // }
    if (copy?.forCreate == false && copy?.forEdit == false) {
      addInvalidProperty("for");
    }
    // if (copy?.benefitText === null || copy?.benefitText === undefined || copy?.benefitText.trim() === "") {
    // 	addInvalidProperty("benefitText");
    // }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  return (
    <div className="mt-4 col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row col-12 px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Planned Activity Network Element*
                <input
                  type="text"
                  onChange={(e) =>
                    onChange("plannedActivityNetworkElementDescription", e)
                  }
                  onKeyUp={(e) =>
                    onChange("plannedActivityNetworkElementDescription", e)
                  }
                  className="inputForm w-100"
                  value={formData?.plannedActivityNetworkElementDescription}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes(
                "plannedActivityNetworkElementDescription"
              ) ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100 mb-0">
                Planned Activity Linked*
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                    menuPosition={"fixed"}
                      options={
                        formData?.plannedActivityResource &&
                        dictionaryToArrayPlannedActivityResourceDto(
                          formData?.plannedActivityResource
                        ).map((x) => {
                          return {
                            key: x.key,
                            value:
                              x.value.plannedActivityResourceDescription ?? "",
                          };
                        })
                      }
                      value={
                        formData?.plannedActivityResource &&
                        dictionaryToArrayPlannedActivityResourceDto(
                          formData?.plannedActivityResource
                        )
                          .filter(
                            (x) => x.key == formData?.plannedActivityResourceId
                          )
                          .map((x) => {
                            return {
                              key: x.key,
                              value:
                                x.value.plannedActivityResourceDescription ??
                                "",
                            };
                          })
                      }
                      onChange={(e) =>
                        onChangeSelect("plannedActivityResourceId", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("plannedActivityResourceId") ? (
                <label className="validation">
                  *planned Activity linked must have a value
                </label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100 mb-0">
                Rule*
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                    menuPosition={"fixed"}
                      options={props.rules.map((x) => {
                        return { key: x.key, value: x.value };
                      })}
                      value={props.rules
                        .filter((x) => x.key === formData?.rule)
                        .map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                      onChange={(e) => onChangeSelect("rule", e)}
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
              validation.property?.includes("rule") ? (
                <label className="validation">*Rule must have a value</label>
              ) : null}
            </div>
          </div>
          <div className="col-6 pr-0 my-3 row mx-0">
            <label className="labelForm voda-bold    d-flex align-items-center w-50">
              <input
                type="checkbox"
                checked={formData?.forCreate}
                onChange={(e) => onChange("forCreate", e)}
                className=" mr-1"
              />
              Is for Create?
            </label>
            <label className="labelForm voda-bold pt-2   d-flex align-items-center w-50">
              <input
                type="checkbox"
                checked={formData?.forEdit}
                onChange={(e) => onChange("forEdit", e)}
                className=" mr-1"
              />
              Is for Edit?
            </label>
            {validation &&
            validation.response == false &&
            validation.property?.includes("for") ? (
              <label className="validation">
                *is For create or is For edit must be true
              </label>
            ) : null}
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                driver Text*
                <input
                  type="text"
                  onChange={(e) => onChange("driverText", e)}
                  onKeyUp={(e) => onChange("driverText", e)}
                  className="inputForm w-100"
                  value={formData?.driverText}
                />
              </label>
            </div>
            {validation &&
            validation.response == false &&
            validation.property?.includes("driverText") ? (
              <label className="validation">
                *Driver Text must have a value
              </label>
            ) : null}
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                benefits text*
                <input
                  type="text"
                  onChange={(e) => onChange("benefitText", e)}
                  onKeyUp={(e) => onChange("benefitText", e)}
                  className="inputForm w-100"
                  value={formData?.benefitText}
                />
              </label>
            </div>
            {validation &&
            validation.response == false &&
            validation.property?.includes("benefitText") ? (
              <label className="validation">
                *Benefits Text must have a value
              </label>
            ) : null}
          </div>
          {props.edit === true ? (
            <div className="col-6">
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
          ) : null}
          {props.edit === true ? (
            <div className="col-6">
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
          ) : null}
        </div>
      </form>
      <div className="col-12 justify-content-end mt-3 d-flex ">
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
          {props.edit ? "Save CHANGES" : "Create Lookup"}
        </button>
      </div>
    </div>
  );
};

export default PlannedActivityNetworkElementForm;
