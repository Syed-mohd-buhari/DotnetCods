import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatSubDomainSpoc } from "../../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocCreateAction";
import { EditSubDomainSpoc } from "../../../Redux/Action/LookUp/SubDomainSpoc/SubDomainSpocEditAction";

import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { Form } from "react-bootstrap";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  subDomain: boolean;
}

const SubDomainSpocForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<TipologicaGridDto>(
    CreatSubDomainSpoc,
    EditSubDomainSpoc
  );

  const dtoEditResourceState = (state: RootState) =>
    state.subDomainSpocEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.subDomainSpocCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData({ ...editResource, isEdu: true, isSubDomain: true });
    } else {
      setFormData({ ...createResource, isEdu: true, isSubDomain: true });
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
      (copy.isEdu === undefined ||
        copy.isEdu === null ||
        copy.isEdu === false) &&
      (copy.isSubDomain === undefined ||
        copy.isSubDomain === null ||
        copy.isSubDomain === false)
    ) {
      addInvalidProperty("checkboxs");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeCheckBox = (type: string, checked: boolean) => {
    console.log("e => ", checked);
    let copy = { ...formData } as TipologicaGridDto;
    copy[type] = checked;
    setFormData(copy);
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
              <label className="labelForm voda-bold mb-0 w-100">
                Email Address
                <span className="red fz-20">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.description}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("description") ? (
                  <label className="validation">*Description is required</label>
                ) : null}
              </label>
            </div>
          </div>

          <div className="col-12">
            <label className="labelForm voda-bold mb-0 w-100">
              <div className="form-group">
                <Form.Check
                  type="checkbox"
                  label="Delivery"
                  id="supportedServiceBoundary"
                  value="supportedServiceBoundary"
                  onChange={(e) => onChangeCheckBox("isEdu", e.target.checked)}
                  checked={formData?.isEdu ? true : false}
                />
              </div>
              <div>
                <Form.Check
                  type="checkbox"
                  label="Technology Area Management"
                  id="supportedServiceBoundary"
                  value="supportedServiceBoundary"
                  onChange={(e) =>
                    onChangeCheckBox("isSubDomain", e.target.checked)
                  }
                  checked={formData?.isSubDomain ? true : false}
                />
              </div>

              {validation &&
              validation.response === false &&
              validation.property?.includes("checkboxs") ? (
                <label className="validation">*select at least one</label>
              ) : null}
            </label>
          </div>
          <div className="col-12">
            <div className="row">
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
                <div className="col-6 pr-0">
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
          className="voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => Save(formData, props.edit, validazioneClient, refresh)}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default SubDomainSpocForm;
