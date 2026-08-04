import React, { useCallback, useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import {
  NFVISwCompatibleDtoGrid,
  NFVISwCompatibleDtoUpdate,
} from "../../Model/NFVISoftwareCompatible";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { EditNFVISwCompatible } from "../../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleEditAction";
import { CreatNFVISwCompatible } from "../../Redux/Action/NFVISoftwareCompatible/NFVISoftwareCompatibleCreateAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { formatDateWithTime } from "../../Hook/Common";
import {
  DateInputComponent,
  DropdownInputComponent,
  TextAreaInputComponent,
  TextInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import {
  dictionaryToArray,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import ProblemCategoryContainer from "../../Containers/Lookup/ProblemCategoryContainer";
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

const NFVISoftwareCompatibleModal: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    setChanged,
    setInputValue,
    confirmForm,
    checkIsExist,
  } = useFormTableCrud<NFVISwCompatibleDtoUpdate>(
    CreatNFVISwCompatible,
    EditNFVISwCompatible
  );

  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const dtoEditResourceState = (state: RootState) =>
    state.NFVISwCompatibleEditReducer.NFVISwCompatibleDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.NFVISwCompatibleCreateReducer.NFVISwCompatibleDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: NFVISwCompatibleDtoGrid) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.plaftFormId === null ||
      copy?.plaftFormId === undefined ||
      copy?.plaftFormId === 0
    ) {
      addInvalidProperty("plaftFormId");
    }

    if (
      copy?.productId === null ||
      copy?.productId === undefined ||
      copy?.productId === 0
    ) {
      addInvalidProperty("productId");
    }

    if (
      copy?.vendorId === null ||
      copy?.vendorId === undefined ||
      copy?.vendorId === 0
    ) {
      addInvalidProperty("vendorId");
    }

    if (
      copy?.minimumSupportedVersion === null ||
      copy?.minimumSupportedVersion === undefined ||
      copy?.minimumSupportedVersion === ""
    ) {
      addInvalidProperty("minimumSupportedVersion");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as NFVISwCompatibleDtoUpdate;
    if (validation?.property?.includes(fieldSet)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
    if (e && e["key"]) {
      copy[fieldSet] = e["key"];
      setFormData(copy);
    }
  };

  const changeDateTime = (fieldSet: string, date: any) => {
    const copy = { ...formData } as NFVISwCompatibleDtoUpdate;
    if (validation?.property?.includes(fieldSet)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(fieldSet);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
    if (date === null) {
      copy[fieldSet] = null;
      setFormData(copy);
    } else {
      const DateString = `${date.getFullYear()}/${
        date.getMonth() + 1
      }/${date.getDate()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
      copy[fieldSet] = DateString;
      setFormData(copy);
    }
  };
  const ProblemRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.problemCategoryId]: item.problemCategoryDescription,
      }),
      {}
    );
    // if (formData) {
    //   setFormData({
    //     ...formData,
    //     problemCategoryTypes: obj as { [key: string]: string },
    //   });
    // }
  };

  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 1:
          return (
            <ProblemCategoryContainer
              returnObject={ProblemRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></ProblemCategoryContainer>
          );
        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );

  return (
    <div className="col-12">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => setIsVisibleModalLookup(0)}
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
        {/* <legend className="text-bb"> Details</legend> */}
        <div className="row mt-3">
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={"Vendor"}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.vendorResource != undefined
                    ? resourceArrayRefactor(formData.vendorResource).find(
                        (x) => x.key === formData?.vendorId
                      )
                    : null
                }
                options={
                  formData && formData.vendorResource !== undefined
                    ? resourceArrayRefactor(formData?.vendorResource)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("vendorId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("vendorId")
                    ? true
                    : false
                }
                error="Vendor must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["plaftForm"]?.Full ?? "Select VMWARE Version"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.vmwareMswPlatform != undefined
                    ? dictionaryToArray(formData.vmwareMswPlatform).find(
                        (x) => x.key === formData?.plaftFormId
                      )
                    : null
                }
                options={
                  formData && formData.vmwareMswPlatform !== undefined
                    ? dictionaryToArray(formData?.vmwareMswPlatform)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("plaftFormId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("plaftFormId")
                    ? true
                    : false
                }
                error="VMWARE Version must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["productName"]?.Full ?? "productName"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.productName != undefined
                    ? resourceArrayRefactor(formData.productName).find(
                        (x) => x.key === formData?.productId
                      )
                    : null
                }
                options={
                  formData && formData.productName != undefined
                    ? resourceArrayRefactor(formData.productName)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("productId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("productId")
                    ? true
                    : false
                }
                error="Product Name must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["minimumSupportedVersion"]?.Full ??
                  "Minimum Supported Version"
                }`}
                required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.minimumSupportedVersion ?? ""}
                onChange={(e: any) => onChange("minimumSupportedVersion", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("minimumSupportedVersion")
                    ? true
                    : false
                }
                error="Minimum Supported Version must have a value."
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
export default NFVISoftwareCompatibleModal;
