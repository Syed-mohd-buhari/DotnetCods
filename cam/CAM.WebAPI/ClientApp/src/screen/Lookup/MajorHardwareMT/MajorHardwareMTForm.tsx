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
  GetMajorHardwareMTCreateResource,
  CreatMajorHardwareMT,
} from "../../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTCreateAction";
import { deleteLocation } from "../../../Redux/Action/LookUp/Location/LocationDeleteAction";
import {
  GetMajorHardwareMTEditResource,
  EditMajorHardwareMT,
} from "../../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTEditAction";
import {
  GetIntraVMTypeGrid,
  GetFilterColumIntraVMType,
} from "../../../Redux/Action/LookUp/IntraVMType/IntraVMTypeGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Select from "react-select";
import { MajorHardwareMTDto } from "../../../Model/LookUp/MajorHardwareMT";
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

const MajorHardwareForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<MajorHardwareMTDto>(
    CreatMajorHardwareMT,
    EditMajorHardwareMT
  );

  const dtoEditResourceState = (state: RootState) =>
    state.majorHardwareMTEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.majorHardwareMTCreateReducer.LookUpDtoCreate;

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

  const validazioneClient = (copy: MajorHardwareMTDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.hardwareSolution === null ||
      copy?.hardwareSolution === undefined ||
      copy?.hardwareSolution.trim() === ""
    ) {
      addInvalidProperty("hardwareSolution");
    }

    if (
      copy?.hardwareType === null ||
      copy?.hardwareType === undefined ||
      copy?.hardwareType.trim() === ""
    ) {
      addInvalidProperty("hardwareType");
    }

    // Validation for the three new dropdowns
    if (
      copy?.buildConstructionId === null ||
      copy?.buildConstructionId === undefined ||
      copy?.buildConstructionId === 0
    ) {
      addInvalidProperty("buildConstructionId");
    }

    if (
      copy?.orgEqpManuFacturerId === null ||
      copy?.orgEqpManuFacturerId === undefined ||
      copy?.orgEqpManuFacturerId === 0
    ) {
      addInvalidProperty("orgEqpManuFacturerId");
    }

    if (
      copy?.platformId === null ||
      copy?.platformId === undefined ||
      copy?.platformId === 0
    ) {
      addInvalidProperty("platformId");
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
    const copy = { ...formData } as MajorHardwareMTDto;
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
    let copy = { ...formData } as MajorHardwareMTDto;
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
        aria-describedby="alert-dialog-intraDescription"
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
        <div className="row col-12 px-0 gy-3">
          <div className="col-6 mb-3">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Hardware Solution <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("hardwareSolution", e)}
                  onKeyUp={(e) => onChange("hardwareSolution", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.hardwareSolution}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("hardwareSolution") ? (
                <label className="validation">
                  *Hardware Solution is required
                </label>
              ) : null}
            </div>
          </div>

          <div className="col-6 mb-3">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Hardware type <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("hardwareType", e)}
                  onKeyUp={(e) => onChange("hardwareType", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.hardwareType}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("hardwareType") ? (
                <label className="validation">*Hardware Type is required</label>
              ) : null}
            </div>
          </div>

          {/* Build Construction Dropdown */}
          <div className="col-6 mb-3">
            <label className="labelForm voda-bold mb-0 w-100">
              Build Construction <span className="red">*</span>
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={formData?.buildConstructionResources}
                  value={
                    formData?.buildConstructionResources &&
                    formData.buildConstructionResources.find(
                      (x) => x.key === formData.buildConstructionId
                    )
                  }
                  onChange={(e) => onChangeSelect("buildConstructionId", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.text.toString()}
                  getOptionValue={(option) => option.key.toString()}
                />
              </div>
            </label>
            {validation &&
            validation.response === false &&
            validation.property?.includes("buildConstructionId") ? (
              <label className="validation">
                *Build Construction is required
              </label>
            ) : null}
          </div>

          {/* OEM/Equipment Manufacturer Dropdown */}
          <div className="col-6 mb-3">
            <label className="labelForm voda-bold mb-0 w-100">
              OEM/Equipment Manufacturer <span className="red">*</span>
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={formData?.orgEqpmanuFacturerResources}
                  value={
                    formData?.orgEqpmanuFacturerResources &&
                    formData.orgEqpmanuFacturerResources.find(
                      (x) => x.key === formData.orgEqpManuFacturerId
                    )
                  }
                  onChange={(e) => onChangeSelect("orgEqpManuFacturerId", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.text.toString()}
                  getOptionValue={(option) => option.key.toString()}
                />
              </div>
            </label>
            {validation &&
            validation.response === false &&
            validation.property?.includes("orgEqpManuFacturerId") ? (
              <label className="validation">
                *OEM/Equipment Manufacturer is required
              </label>
            ) : null}
          </div>

          {/* Platform Dropdown */}
          <div className="col-6 mb-3">
            <label className="labelForm voda-bold mb-0 w-100">
              Platform <span className="red">*</span>
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={formData?.platformResources}
                  value={
                    formData?.platformResources &&
                    formData.platformResources.find(
                      (x) => x.key === formData.platformId
                    )
                  }
                  onChange={(e) => onChangeSelect("platformId", e)}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.text.toString()}
                  getOptionValue={(option) => option.key.toString()}
                />
              </div>
            </label>
            {validation &&
            validation.response === false &&
            validation.property?.includes("platformId") ? (
              <label className="validation">*Platform is required</label>
            ) : null}
          </div>

          {/* <div className="col-6">
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
          </div> */}
          {/* <div className="col-6">
            <label className="labelForm voda-bold mb-0 w-100">
              Location Type
              <div className="d-flex">
                <Select
                  className="w-100"
                  menuPosition={"fixed"}
                  options={
                    formData?.interVMTypeResource &&
                    dictionaryToArray(formData.interVMTypeResource)
                  }
                  value={
                    formData?.interVMTypeResource &&
                    dictionaryToArray(formData?.interVMTypeResource).filter(
                      (el) => formData?.intervmTypeIdsList?.includes(el.key)
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
          </div> */}

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

export default MajorHardwareForm;
