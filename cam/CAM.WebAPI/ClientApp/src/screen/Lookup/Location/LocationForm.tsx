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
  GetLocationCreateResource,
  CreatLocation,
} from "../../../Redux/Action/LookUp/Location/LocationCreateAction";
import { deleteLocation } from "../../../Redux/Action/LookUp/Location/LocationDeleteAction";
import {
  GetLocationEditResource,
  EditLocation,
} from "../../../Redux/Action/LookUp/Location/LocationEditAction";
import {
  GetLocationGrid,
  GetFilterColumLocation,
} from "../../../Redux/Action/LookUp/Location/LocationGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Select from "react-select";
import { LocationDto } from "../../../Model/LookUp/Location";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import { Form, Modal } from "react-bootstrap";
import { useAuth } from "../../../Hook/useAuth";
import DeploymentType from "../../../Containers/Lookup/DeploymentTypeContainer";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const LocationForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");
  const { tipologicaPermesso } = useAuth();

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
  } = useFormTableCrud<LocationDto>(CreatLocation, EditLocation);

  const dtoEditResourceState = (state: RootState) =>
    state.locationEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.locationCreateReducer.LookUpDtoCreate;

  const [isVisibleModalLookup, setIsVisibleModalLookup] =
    useState<boolean>(false);
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

  const validazioneClient = (copy: LocationDto) => {
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

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeDefault = (checked: boolean) => {
    const copy = { ...formData } as LocationDto;
    copy.defaultValue = checked;
    setFormData(copy);
  };

  const DeploymentTypeRefillData = (value: Array<any>) => {
    // var obj = value.reduce(
    //   (acc, item) => ({ ...acc, [item.id]: item.description }),
    //   {}
    // );
    // if (formData && formData?.deploymentTypeReosurce)
    //   formData.deploymentTypeReosurce = obj as { [key: string]: string };
    // setFormData(formData);
  };

  const removeValidation = (property: string) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as LocationDto;
    if (e != null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    removeValidation(property);
  };

  return (
    <div className="col-12">
      <Dialog
        open={isVisibleModalLookup}
        onClose={() => setIsVisibleModalLookup(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                setIsVisibleModalLookup(false);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {<DeploymentType returnObject={DeploymentTypeRefillData} />}
        </DialogContent>
      </Dialog>
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row col-12 px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Location <span className="red">*</span>
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
            <label className="labelForm voda-bold   mb-0 w-100">
              OpCo
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={
                    formData?.opcoResource &&
                    dictionaryToArray(formData.opcoResource)
                  }
                  value={
                    formData?.opcoResource &&
                    dictionaryToArray(formData.opcoResource).find(
                      (x) => x.key == formData.opcoId
                    )
                  }
                  onChange={(e) => onChangeSelect("opcoId", e)}
                  // onKeyUp={(e) => onChangeSelect("opcoId", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value.toString()}
                  getOptionValue={(option) => option["key"].toString()}
                />
              </div>
            </label>
            {validation &&
            validation.response == false &&
            validation.property?.includes("opcoId") ? (
              <label className="validation">*opco cannot be empty!</label>
            ) : null}
          </div>
          <div className="col-6">
            <label className="labelForm voda-bold mb-0 w-100">
              Location Type
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={
                    formData?.locationTypeResource &&
                    dictionaryToArray(formData.locationTypeResource)
                  }
                  value={
                    formData?.locationTypeResource &&
                    dictionaryToArray(formData?.locationTypeResource).filter(
                      (el) => formData?.locationTypeIdsList?.includes(el.key)
                    )
                  }
                  onChange={(e) =>
                    OnChangeMultiSelect("locationTypeIdsList", e)
                  }
                  // onKeyUp={(e) => OnChangeMultiSelect("locationTypeIdsList", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  isMulti
                  getOptionLabel={(option) => option.value.toString()}
                  getOptionValue={(option) => option["key"].toString()}
                />
                {tipologicaPermesso && (
                  <button
                    className="btn btn-link"
                    onClick={() => setIsVisibleModalLookup(true)}
                    type="button"
                  >
                    <img
                      style={{ height: 15 }}
                      src={require("../../../img/plus_icon.png")}
                      alt="+"
                    />
                  </button>
                )}
              </div>
            </label>
            {validation &&
            validation.response == false &&
            validation.property?.includes("locationTypeIdsList") ? (
              <label className="validation">
                *location Type cannot be empty!
              </label>
            ) : null}
          </div>
          <div className="col-6">
            <label className="voda-bold mt-2">
              Is this a default Location?
            </label>
            <div
              className="radio-content flex-mode"
              style={{ justifyContent: "start" }}
            >
              <Form.Check
                type="radio"
                className="radio"
                name="default"
                label="Yes"
                checked={formData?.defaultValue ? true : false}
                value="true"
                onChange={() => onChangeDefault(true)}
              />
              <Form.Check
                type="radio"
                className="radio"
                name="default"
                label="No"
                value="false"
                checked={!formData?.defaultValue ? true : false}
                onChange={() => onChangeDefault(false)}
              />
            </div>
            {/* <label className="labelForm voda-bold   w-100 h-100 d-flex align-items-center">
              <input
                // disabled={!formData?.forLcm}
                type="checkbox"
                onChange={(e) => onChange("defaultValue", e)}
                style={{ height: "15px" }}
                className="inputForm mb-1 mr-1"
                checked={formData?.defaultValue}
              />
              Is this a default Location?
            </label> */}
          </div>
          {props.edit === true ? (
            <div className="col-6 mt-3">
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
            <div className="col-6 mt-3">
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

export default LocationForm;
