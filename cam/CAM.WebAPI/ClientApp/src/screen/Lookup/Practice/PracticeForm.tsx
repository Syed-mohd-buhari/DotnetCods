import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatPractice } from "../../../Redux/Action/LookUp/Practice/PracticeCreateAction";
import { EditPractice } from "../../../Redux/Action/LookUp/Practice/PracticeEditAction";

import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { DropdownInputComponent } from "../../../Components/FormField";
import { dictionaryToArray } from "../../../Hook/Dictionary";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const PracticeForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<TipologicaGridDto>(CreatPractice, EditPractice);

  const dtoEditResourceState = (state: RootState) =>
    state.practiceEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.practiceCreateReducer.LookUpDtoCreate;

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
      copy?.practiceDescription === null ||
      copy?.practiceDescription === undefined ||
      copy?.practiceDescription.trim() === ""
    ) {
      addInvalidProperty("practiceDescription");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
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
              <label className="labelForm voda-bold   mb-0 w-100">
                Practice Description<span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("practiceDescription", e)}
                  onKeyUp={(e) => onChange("practiceDescription", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.practiceDescription}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("practiceDescription") ? (
                <label className="validation">
                  *Practice Description is required
                </label>
              ) : null}
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`Practice Head`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.practiceResource != undefined
                    ? dictionaryToArray(formData.practiceResource).find(
                        (x) => x.key === formData?.practiceEmailId
                      )
                    : null
                }
                options={
                  formData && formData.practiceResource != undefined
                    ? dictionaryToArray(formData.practiceResource)
                    : []
                }
                onChange={(e: any) => onChangeSelect("practiceEmailId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("practiceEmailId")
                    ? true
                    : false
                }
                error="Practice Head must have a value."
              />
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

export default PracticeForm;
