import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatNFVIBundleID } from "../../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDCreateAction";
import { EditNFVIBundleID } from "../../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDEditAction";
import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import { NFVIBundleIDDtoGrid } from "../../../Model/LookUp/NFVIBundleId";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

interface desc {
  id: string;
  value: string;
}

const NFVIBundleIDForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");
  const [description, setDescription] = useState<desc[]>([
    { id: "desc1", value: "" },
    { id: "desc2", value: "" },
    { id: "desc3", value: "" },
  ]);

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
  } = useFormTableCrud<NFVIBundleIDDtoGrid>(
    CreatNFVIBundleID,
    EditNFVIBundleID
  );

  const dtoEditResourceState = (state: RootState) =>
    state.nFVIBundleIDEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.nFVIBundleIDCreateReducer.LookUpDtoCreate;

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

  const updateDescription = (e) => {
    const id: string = e.target.id;
    let copyOfDescription = [...description] as desc[];
    if (e.target.value.length <= 2 && !isNaN(+e.target.value)) {
      copyOfDescription.map((el) => {
        if (el.id === id) {
          el.value = e.target.value.trim();
        }
      });
      setDescription(copyOfDescription);
      const finalDescription = `${description[0].value}.${description[1].value}.${description[2].value}`;
      let copy = { ...formData } as NFVIBundleIDDtoGrid;
      copy.description = finalDescription;
      setFormData(copy);
    }
  };

  useEffect(() => {
    if (editResource && props.edit) {
      let fullDescription = editResource.description;
      let splittedDescription = fullDescription?.split(".");
      let copy = [...description];

      copy.map((el, idx) => {
        if (splittedDescription) {
          el.value = splittedDescription[idx] ? splittedDescription[idx] : "";
        }
      });

      setDescription(copy);
    }
  }, [editResource]);

  const validazioneClient = (copy: NFVIBundleIDDtoGrid) => {
    let errorInDescription = false;
    description.map((el) => {
      if (!el.value) {
        errorInDescription = true;
      }
    });
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.description === null ||
      copy?.description === undefined ||
      copy?.description.trim() === "" ||
      errorInDescription
    ) {
      addInvalidProperty("description");
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
    <div className="col-md-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row">
          <div className="col-md-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-75">
                NFVI Bundle ID<span className="red">*</span>
                <div className="d-flex ">
                  <input
                    type="text"
                    id="desc1"
                    onChange={(e) => updateDescription(e)}
                    onKeyUp={(e) => updateDescription(e)}
                    className="inputForm w-50 mr-1"
                    value={description[0].value}
                  />
                  <span className="h1">.</span>
                  <input
                    type="text"
                    id="desc2"
                    onChange={(e) => updateDescription(e)}
                    onKeyUp={(e) => updateDescription(e)}
                    className="inputForm inputForm w-50 mx-1"
                    value={description[1].value}
                  />
                  <span className="h1">.</span>
                  <input
                    type="text"
                    id="desc3"
                    onChange={(e) => updateDescription(e)}
                    onKeyUp={(e) => updateDescription(e)}
                    className="inputForm inputForm w-50 mx-1"
                    value={description[2].value}
                  />
                </div>
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("description") ? (
                <label className="validation">
                  *All fields must contain at least one number{" "}
                </label>
              ) : null}
            </div>
          </div>
          {props.edit === true ? (
            <div className="col-md-12 row">
              <div className="col-md-6">
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

              <div className="col-md-6 pr-0">
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
      <div className="col-md-12 justify-content-end d-flex ">
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

export default NFVIBundleIDForm;
