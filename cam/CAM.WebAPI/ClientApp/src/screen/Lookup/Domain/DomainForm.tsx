import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreateSystemName } from "../../../Redux/Action/LookUp/Domain/DomainCreateAction";
import { EditSystemName } from "../../../Redux/Action/LookUp/Domain/DomainEditAction";
import { SystemNamesDto } from "../../../Model/LookUp/Domain";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const SystemNamesForm: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    setChanged,
    inputValue,
  } = useFormTableCrud<SystemNamesDto>(CreateSystemName, EditSystemName);

  // Fix reducer name consistency - ensure it matches your actual reducer name
  const dtoEditResourceState = (state: RootState) =>
    state.systemNamesEditReducer.LookUpDtoEdit; // Changed from systemNamesEditReducer
  const dtoNewResourceState = (state: RootState) =>
    state.systemNameCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  // Enhanced useEffect to properly handle initialization
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validateForm = (copy: SystemNamesDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (props.edit && (!copy.systemNameId || copy.systemNameId <= 0)) {
      addInvalidProperty("systemNameId");
    }

    if (
      !copy?.systemNameDescription ||
      copy.systemNameDescription.trim() === ""
    ) {
      addInvalidProperty("systemNameDescription");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const handleSubmit = () => {
    if (!formData) return;

    const validationResult = validateForm(formData);
    if (!validationResult.response) return;

    // Ensure we're passing the correct mode
    Save(formData, props.edit, validateForm, (changed: boolean) => {
      props.action.closeModal(changed);
      props.action.refresh();
    });
  };

  return (
    <div className="col-12">
      <form
        id="systemNameForm"
        onChange={() => setChanged(true)}
        onSubmit={(e) => {
          e.preventDefault();
          handleSubmit();
        }}
      >
        <div className="row col-12 px-0">
          {/* System Name ID (visible only in edit mode) */}
          {props.edit && (
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  System Name ID
                  <input
                    type="text"
                    readOnly
                    className="inputForm w-100"
                    value={formData?.systemNameId || ""}
                  />
                </label>
              </div>
            </div>
          )}

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Domain Name<span className="red">*</span>
                <input
                  type="text"
                  onKeyUp={(e) => onChange("systemNameDescription", e)}
                  onChange={(e) => onChange("systemNameDescription", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.systemNameDescription || ""}
                  maxLength={100}
                />
              </label>
              {validation?.response === false &&
                validation.property?.includes("systemNameDescription") && (
                  <label className="validation">*System Name is required</label>
                )}
            </div>
          </div>

          {props.edit && (
            <div className="col-12">
              <div className="row">
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold w-100">
                      Last Modified
                      <input
                        readOnly
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={
                          formatDateWithTime(
                            formData?.lastModified
                          )?.toUpperCase() || ""
                        }
                      />
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold w-100">
                      Last Modified By
                      <input
                        readOnly
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={formData?.lastModifiedBy || ""}
                      />
                    </label>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      </form>

      <div className="col-12 justify-content-end d-flex mt-3">
        <button
          className="voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={handleSubmit}
          type="button"
          disabled={!changed}
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default SystemNamesForm;
