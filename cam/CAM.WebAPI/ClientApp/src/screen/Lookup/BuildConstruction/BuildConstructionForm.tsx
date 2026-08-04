import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import {
  GetBuildConstructionCreateResource,
  CreatBuildConstruction,
} from "../../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCreateAction";
import { EditBuildConstruction } from "../../../Redux/Action/LookUp/BuildConstruction/BuildConstructionEditAction";

import { TipologicaGridDtoRule } from "../../../Model/LookUp/LookUpGenericModel";
import Select from "react-select";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  rules: { key: number; value: string }[];
  cloudTypeBuild: { key: number; value: string }[];
}

const BuildConstructionForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");
  const [showCloudType, setShowCloudType] = useState(false);

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
  } = useFormTableCrud<TipologicaGridDtoRule>(
    CreatBuildConstruction,
    EditBuildConstruction
  );

  const dtoEditResourceState = (state: RootState) =>
    state.buildConstructionEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.buildConstructionCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const isProprietaryHW = props.rules.some(
    (rule) => rule.value == "As Proprietary HW" && rule.key === formData?.rule
  );
  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      const isCloud = !!editResource?.isCloudHostedAssetBool;
      setShowCloudType(isCloud);
    } else {
      setFormData(createResource);
      setShowCloudType(false);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: TipologicaGridDtoRule) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.description === null ||
      copy?.description === undefined ||
      copy?.description.trim() === ""
    ) {
      addInvalidProperty("description");
    }
    if (showCloudType && !copy?.cloudTypeBuild?.trim()) {
      addInvalidProperty("cloudTypeBuild");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const onChangeCloudTypeBuild = (obj: any) => {
    let copy = { ...formData } as TipologicaGridDtoRule;
    if (obj && obj["value"]) {
      copy.cloudTypeBuild = obj["value"];
    } else {
      copy.cloudTypeBuild = undefined;
    }

    setFormData(copy);
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeSelectRule = (selectedOption: any) => {
    setFormData((prev) => ({
      ...prev,
      rule: selectedOption.key,
      isCloudHostedAssetBool:
        selectedOption.value === "As Proprietary HW"
          ? false
          : prev?.isCloudHostedAssetBool,
    }));

    if (selectedOption.value === "As Proprietary HW") {
      setShowCloudType(false);
    }
  };

  return (
    <div className="col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Build Construction<span className="red fz-20">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.description}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("description") ? (
                <label
                  className="validation"
                  style={{
                    position: "relative",
                    top: "-1px",
                    display: "block",
                  }}
                >
                  *Description is required
                </label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold w-100 mb-0">
                Rule<span className="red fz-20">*</span>
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
                      onChange={onChangeSelectRule}
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
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold w-100 mb-0">
                <input
                  type="checkbox"
                  checked={showCloudType}
                  onChange={(e) => {
                    const checked = e.target.checked;
                    setShowCloudType(checked);

                    setFormData((prev) => ({
                      ...prev,
                      isCloudHostedAssetBool: checked,
                      cloudTypeBuild: checked ? formData?.cloudTypeBuild : "",
                    }));
                  }}
                  disabled={isProprietaryHW}
                  className="mr-2"
                />
                Is Cloud Hosted Asset?
              </label>
            </div>
          </div>
          {showCloudType && (
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold w-100 mb-0">
                  Cloud Type <span className="red fz-20">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={props.cloudTypeBuild?.map((x) => {
                          return { key: x.key, value: x.value };
                        })}
                        value={
                          props.cloudTypeBuild
                            ? props.cloudTypeBuild
                                .filter(
                                  (x) => x.value === formData?.cloudTypeBuild
                                )
                                .map((x) => {
                                  return { key: x.key, value: x.value };
                                })
                            : null
                        }
                        onChange={(e) => onChangeCloudTypeBuild(e)}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        getOptionLabel={(option) => option.value.toString()}
                        getOptionValue={(option) => option["key"].toString()}
                      />
                    </div>
                  </div>
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("cloudTypeBuild") ? (
                  <label className="validation">*Cloud Type is required</label>
                ) : null}
              </div>
            </div>
          )}
          {props.edit === true ? (
            <div className="fixed-bottom-wrapper" style={{ width: "100%" }}>
              {" "}
              {/* New fixed container */}
              <div className="row">
                <div className="col-6 ms-n2">
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
              </div>
            </div>
          ) : null}
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

export default BuildConstructionForm;
