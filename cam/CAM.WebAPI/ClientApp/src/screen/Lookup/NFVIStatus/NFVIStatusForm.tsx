import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { colourStyles, formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { dictionaryToArray } from "../../../Hook/Dictionary";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatNFVIStatus } from "../../../Redux/Action/LookUp/NFVIStatus/NFVIStatusCreateAction";
import { EditNFVIStatus } from "../../../Redux/Action/LookUp/NFVIStatus/NFVIStatusEditAction";
import { NFVIStatusDtoGrid } from "../../../Model/LookUp/NFVIStatus";
import Select from "react-select";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

export const colors = [
  { key: "red", value: "Red" },
  { key: "blue", value: "Blue" },
  { key: "gold", value: "Yellow" },
  { key: "orange", value: "Amber" },
  { key: "black", value: "White" },
  { key: "grey", value: "Grey" },
  { key: "green", value: "Green" },
];

const NFVIStatusForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<NFVIStatusDtoGrid>(CreatNFVIStatus, EditNFVIStatus);

  const dtoEditResourceState = (state: RootState) =>
    state.nFVIStatusEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.nFVIStatusCreateReducer.LookUpDtoCreate;

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

  const validazioneClient = (copy: NFVIStatusDtoGrid) => {
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
      copy?.color === null ||
      copy?.color === undefined ||
      copy?.color === ""
    ) {
      addInvalidProperty("color");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const OnChangeColor = (e: any, property: string) => {
    let copy = { ...formData } as NFVIStatusDtoGrid;
    copy.color = e && e["key"];
    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(property);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
  };

  return (
    <div className="mt-4 col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                NFVI Status<span className="red">*</span>
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
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Color<span className="red">*</span>
                <Select
                menuPosition={"fixed"}
                  options={colors
                    .map((x) => {
                      return { key: x.key, value: x.value, color: x.key };
                    })
                    .sort((a, b) =>
                      a.value.toLowerCase() < b.value.toLowerCase() ? -1 : 1
                    )}
                  value={colors
                    .filter((x) => x.key === formData?.color)
                    .map((x) => {
                      return { key: x.key, value: x.value, color: x.key };
                    })}
                  onChange={(e) => OnChangeColor(e, "color")}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"]}
                  styles={colourStyles}
                ></Select>
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("color") ? (
                <label className="validation">*Color is required</label>
              ) : null}
            </div>
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
          Submit
        </button>
      </div>
    </div>
  );
};

export default NFVIStatusForm;
