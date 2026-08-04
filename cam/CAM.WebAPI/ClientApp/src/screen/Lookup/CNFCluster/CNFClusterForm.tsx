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
  GetCNFClusterCreateResource,
  CreatCNFCluster,
} from "../../../Redux/Action/LookUp/CNFCluster/CNFClusterCreateAction";
import { deleteLocation } from "../../../Redux/Action/LookUp/Location/LocationDeleteAction";
import {
  GetCNFClusterEditResource,
  EditCNFCluster,
} from "../../../Redux/Action/LookUp/CNFCluster/CNFClusterEditAction";
import {
  GetInterVMTypeGrid,
  GetFilterColumInterVMType,
} from "../../../Redux/Action/LookUp/InterVMType/InterVMTypeGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Select from "react-select";
import { CNFClusterDto } from "../../../Model/LookUp/CNFCluster";
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

const CNFClusterForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<CNFClusterDto>(CreatCNFCluster, EditCNFCluster);

  const dtoEditResourceState = (state: RootState) =>
    state.cNFClusterEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.cNFClusterCreateReducer.LookUpDtoCreate;

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

  const validazioneClient = (copy: CNFClusterDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.cnfClusterName === null ||
      copy?.cnfClusterName === undefined ||
      copy?.cnfClusterName.trim() === ""
    ) {
      addInvalidProperty("cnfClusterName");
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
    const copy = { ...formData } as CNFClusterDto;
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
    let copy = { ...formData } as CNFClusterDto;
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
        aria-describedby="alert-dialog-cnfClusterName"
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
                Description <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("cnfClusterName", e)}
                  onKeyUp={(e) => onChange("cnfClusterName", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.cnfClusterName}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("cnfClusterName") ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Node Pool <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("nodePool", e)}
                  onKeyUp={(e) => onChange("nodePool", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.nodePool}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("nodePool") ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Alias <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("alaisName", e)}
                  onKeyUp={(e) => onChange("alaisName", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.alaisName}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("alaisName") ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          {
            <div className="col-6">
              <label className="labelForm voda-bold   mb-0 w-100">
                CNF Name
                <div className="d-flex">
                  <Select
                    className="w-100"
                    menuPosition={"fixed"}
                    options={
                      formData?.cnfNameResource &&
                      dictionaryToArray(formData.cnfNameResource)
                    }
                    value={
                      formData?.cnfNameResource &&
                      dictionaryToArray(formData.cnfNameResource).find(
                        (x) => x.key == formData.cnfNameId
                      )
                    }
                    onChange={(e) => onChangeSelect("cnfNameId", e)}
                    // onKeyUp={(e) => onChangeSelect("cnfNameId", e)}
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
              validation.property?.includes("cnfNameId") ? (
                <label className="validation">*CNF Name cannot be empty!</label>
              ) : null}
            </div>
          }
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

export default CNFClusterForm;
