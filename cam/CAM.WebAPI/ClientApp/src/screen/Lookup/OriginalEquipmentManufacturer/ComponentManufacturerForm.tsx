import React, { useState, useEffect, useCallback } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";

import { CreatComponentManufacturer } from "../../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerCreateAction";
import { EditComponentManufacturer } from "../../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerEditAction";

import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Select from "react-select";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import VodafoneName from "../../../Containers/Lookup/VodafoneNameContainer";
import { useAuth } from "../../../Hook/useAuth";
import { Modal } from "react-bootstrap";
import { GetVodafoneNameGridALL } from "../../../Redux/Action/LookUp/VodafoneName/VodafoneNameGridAction";
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
  lookUpFlag?: string;
}

const ComponentManufacturerForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<TipologicaGridDto>(
    CreatComponentManufacturer,
    EditComponentManufacturer
  );
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const dtoEditResourceState = (state: RootState) =>
    state.componentManufacturerEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.componentManufacturerCreateReducer.LookUpDtoCreate;
  const GridAll = (state: RootState) =>
    state.vodafoneNameGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);
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

  const OnChangeVFName = (property: string, e: any) => {
    let copy = { ...formData } as TipologicaGridDto;
    if (e !== null && e !== undefined) {
      copy[property] = e.key;
    }
    setFormData(copy);
  };

  const validazioneClient = (copy: TipologicaGridDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.componentManufacturer === null ||
      copy?.componentManufacturer === undefined ||
      copy?.componentManufacturer.trim() === ""
    ) {
      addInvalidProperty("componentManufacturer");
    }
    if (
      copy?.componentName === null ||
      copy?.componentName === undefined ||
      copy?.componentName.trim() === ""
    ) {
      addInvalidProperty("componentName");
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
        <div className="row px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
              Component Manufacturer
                <span className="red fz-20">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("componentManufacturer", e)}
                  onKeyUp={(e) => onChange("componentManufacturer", e)}
                  className="inputForm w-100 mt-0"
                  defaultValue={formData?.componentManufacturer}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("componentManufacturer") ? (
                  <label className="validation">*componentManufacturer is required</label>
                ) : null}
              </label>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                {props.lookUpFlag === "equipment"
                  ? "Component Name"
                  : "Software Name"}
                <span className="red fz-20">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("componentName", e)}
                  onKeyUp={(e) => onChange("componentName", e)}
                  className="inputForm w-100 mt-0"
                  defaultValue={formData?.componentName}
                />
                {validation &&
                validation.response === false &&
                validation.property?.includes("componentName") ? (
                  <label className="validation">*componentName is required</label>
                ) : null}
              </label>
            </div>
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
                <div className="col-6">
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
      <div className="col-12 justify-content-end d-flex mt-4">
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

export default ComponentManufacturerForm;
