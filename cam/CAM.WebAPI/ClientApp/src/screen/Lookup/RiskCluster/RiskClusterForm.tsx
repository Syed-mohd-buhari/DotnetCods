import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import Select from "react-select";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { EditProblemCategory } from "../../../Redux/Action/LookUp/ProblemCategory/ProblemCategoryEditAction";

import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  CreateRiskCluster,
  EditRiskCluster,
} from "../../../Redux/Action/LookUp/RiskCluster/RiskClusterGridAction";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const RiskClusterForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<TipologicaGridDto>(CreateRiskCluster, EditRiskCluster);

  const dtoEditResourceState = (state: RootState) =>
    state.riskClusterEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.riskClusterCreateReducer.LookUpDtoCreate;

  const riskLevel = [
    { key: 0, value: "Very High" },
    { key: 1, value: "High" },
    { key: 2, value: "Medium" },
    { key: 3, value: "Medium/Low" },
    { key: 4, value: "Low" },
  ];

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

  const validazioneClient = (copy: TipologicaGridDto) => {
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
    if (
      copy?.riskLevel === null ||
      copy?.riskLevel === undefined ||
      copy?.riskLevel.trim() === ""
    ) {
      addInvalidProperty("riskLevel");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as TipologicaGridDto;
    if (e && e["value"]) {
      copy[fieldSet] = e["value"];
      setFormData(copy);
    }
  };

  return (
    <div className="col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Description<span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100 mt-0"
                  defaultValue={formData?.description}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("description") ? (
                  <label className="validation">
                    *Risk Cluster Description is required
                  </label>
                ) : null}
              </label>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100 mb-0">
                Risk Level <span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                    menuPosition={"fixed"}
                      value={
                        formData && formData?.riskLevel != undefined
                          ? riskLevel.find(
                              (x) =>
                                x.value.toLowerCase() ===
                                formData?.riskLevel?.toLowerCase()
                            )
                          : null
                      }
                      options={riskLevel}
                      onChange={(e) => onChangeDropdown("riskLevel", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                </div>
                {validation &&
                validation.response == false &&
                validation.property?.includes("riskLevel") ? (
                  <label className="validation">
                    *Risk level must have a value
                  </label>
                ) : null}
              </label>
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

export default RiskClusterForm;
