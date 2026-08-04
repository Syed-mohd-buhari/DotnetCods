import React, { useState, useEffect, useCallback } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatOriginalEquipmentManufacturer } from "../../../Redux/Action/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerCreateAction";
import { EditOriginalEquipmentManufacturer } from "../../../Redux/Action/LookUp/OriginalEquipmentManufacturer/OriginalEquipmentManufacturerEditAction";

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

const OriginalEquipmentManufacturerForm: React.FC<Props> = (props) => {
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
    CreatOriginalEquipmentManufacturer,
    EditOriginalEquipmentManufacturer
  );
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const dtoEditResourceState = (state: RootState) =>
    state.originalEquipmentManufacturerEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.originalEquipmentManufacturerCreateReducer.LookUpDtoCreate;
  const GridAll = (state: RootState) =>
    state.vodafoneNameGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  // helpers/normalizeResource.ts
  const normalizeResource = (resource: any) => ({
    ...resource,
    isPlatformSoftware: resource?.isPlatformSoftware === "Yes",
  });

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(normalizeResource(editResource));
    } else {
      setFormData(normalizeResource(createResource));
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
      copy?.description === null ||
      copy?.description === undefined ||
      copy?.description.trim() === ""
    ) {
      addInvalidProperty("description");
    }

    if (
      props.edit === true &&
      props.lookUpFlag === "software" &&
      (copy?.vodafoneNameId === null || copy?.vodafoneNameId === undefined)
    ) {
      addInvalidProperty("vodafoneNameId");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 1:
          return (
            <VodafoneName
              returnObject={vodafoneNameRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
              showButtons={false}
              isPopup={true}
            />
          );
        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );

  const vodafoneNameRefillData = async () => {
    const res = await GetVodafoneNameGridALL();
    if (res && res.items) {
      let values: Array<any> = res.items;
      var obj = values.reduce(
        (acc, item) => ({ ...acc, [item.id]: item.description }),
        {}
      );
      setFormData({
        ...formData,
        vodafoneNameResource: obj,
      });
    }
  };

  return (
    <div className="col-12">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          setIsVisibleModalLookup(0);
          vodafoneNameRefillData();
        }}
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
                setIsVisibleModalLookup(0);
                vodafoneNameRefillData();
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {ReturnLookupContainer(isVisibleModalLookup)}
        </DialogContent>
      </Dialog>
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                {props.lookUpFlag === "equipment"
                  ? " Equipment Manufacturer"
                  : "Software Name"}
                <span className="red fz-20">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100 mt-0"
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
          {props.lookUpFlag === "software" && props.edit === true && (
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  VF Name
                  <span className="red fz-20">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={
                          formData?.vodafoneNameResource &&
                          dictionaryToArray(formData?.vodafoneNameResource)
                        }
                        value={
                          formData?.vodafoneNameResource &&
                          dictionaryToArray(
                            formData?.vodafoneNameResource
                          ).filter((el) => formData.vodafoneNameId === el.key)
                        }
                        onChange={(e) => OnChangeVFName("vodafoneNameId", e)}
                        //onKeyUp={(e) => OnChangeVFName("vodafoneNameId", e)}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        // isMulti
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                      ></Select>
                    </div>
                    {tipologicaPermesso && (
                      <button
                        className="btn btn-link"
                        onClick={() => setIsVisibleModalLookup(1)}
                        type="button"
                      >
                        <img
                          style={{ height: 15 }}
                          src={require("../../../img/plus_icon.png")}
                          alt="plus"
                        />
                      </button>
                    )}
                  </div>
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("vodafoneNameId") ? (
                    <label className="validation h-6">
                      *Please select a Vodafone Name
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
          )}
          <div className="col-12">
            <div className="col-12 mb-3 mt-4">
              <label className="labelForm voda-bold text-uppercase mb-0 w-100 widthAuto">
                <div className="switchContainer d-flex flex-row align-items-center">
                  <label className="switch mr-2">
                    <input
                      type="checkbox"
                      checked={formData?.isPlatformSoftware ?? false}
                      onChange={(e) => onChange("isPlatformSoftware", e)}
                    />

                    <span className="slider round"></span>
                  </label>
                  Is this a Platform Software (Eg: Broadcom)?
                </div>
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

export default OriginalEquipmentManufacturerForm;
