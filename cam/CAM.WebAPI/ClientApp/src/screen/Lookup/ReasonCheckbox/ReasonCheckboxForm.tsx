import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatReasonCheckbox } from "../../../Redux/Action/LookUp/ReasonCheckbox/ReasonCheckboxCreateAction";
import { EditReasonCheckbox } from "../../../Redux/Action/LookUp/ReasonCheckbox/ReasonCheckboxEditAction";

import { ReasonCheckboxDto } from "../../../Model/LookUp/ReasonCheckbox";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const ReasonCheckboxForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<ReasonCheckboxDto>(
    CreatReasonCheckbox,
    EditReasonCheckbox
  );

  const dtoEditResourceState = (state: RootState) =>
    state.reasonCheckboxEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.reasonCheckboxCreateReducer.LookUpDtoCreate;

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

  const validazioneClient = (copy: ReasonCheckboxDto) => {
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
    setValidation(copyValidation);
    return copyValidation;
  };

  const onChangeCheckbox = (property: string, checked: boolean) => {
    let copy = { ...formData } as ReasonCheckboxDto;
    copy[property] = checked;
    setFormData(copy);
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
                Reason Description<span className="red">*</span>
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
          <div className="col-12">
            <div className="row">
              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                    <div className="switchContainer d-fe">
                      Enable for Hardware
                      <label className="switch">
                        <input
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox("isHardware", e.target.checked)
                          }
                          className="mr-1"
                          checked={formData?.isHardware}
                        />
                        <span className="slider round"></span>
                      </label>
                    </div>
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold   mb-0 w-100 widthAuto">
                    <div className="switchContainer d-fe">
                      Enable for Software
                      <label className="switch">
                        <input
                          type="checkbox"
                          onChange={(e) =>
                            onChangeCheckbox("isSoftware", e.target.checked)
                          }
                          className="mr-1"
                          checked={formData?.isSoftware}
                        />
                        <span className="slider round"></span>
                      </label>
                    </div>
                  </label>
                </div>
              </div>
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

export default ReasonCheckboxForm;
