import React, { useCallback, useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { TestInfoDtoGrid, TestInfoDtoUpdate } from "../../Model/TestInfo";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { EditTestInfo } from "../../Redux/Action/TestInfo/TestInfoEditAction";
import { CreatTestInfo } from "../../Redux/Action/TestInfo/TestInfoCreateAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { formatDateWithTime } from "../../Hook/Common";
import {
  DateInputComponent,
  DropdownInputComponent,
  TextAreaInputComponent,
  TextInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { dictionaryToArray } from "../../Hook/Dictionary";
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

const TestInfoModal: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<TestInfoDtoUpdate>(CreatTestInfo, EditTestInfo);

  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const dtoEditResourceState = (state: RootState) =>
    state.testInfoEditReducer.TestInfoDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.testInfoCreateReducer.TestInfoDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: TestInfoDtoGrid) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    // if (
    //   copy?.statusUrl === null ||
    //   copy?.statusUrl === undefined ||
    //   copy?.statusUrl.trim() === ""
    // ) {
    //   addInvalidProperty("statusUrl");
    // }

    // if (
    //   copy?.problemId === null ||
    //   copy?.problemId === undefined ||
    //   copy?.problemId.trim() === ""
    // ) {
    //   addInvalidProperty("problemId");
    // }

    // if (
    //   copy?.subNetwork === null ||
    //   copy?.subNetwork === undefined ||
    //   copy?.subNetwork.trim() === ""
    // ) {
    //   addInvalidProperty("subNetwork");
    // }

    // if (
    //   copy?.vendorCsr === null ||
    //   copy?.vendorCsr === undefined ||
    //   copy?.vendorCsr.trim() === ""
    // ) {
    //   addInvalidProperty("vendorCsr");
    // }

    // if (
    //   copy?.problemCategoryDescription === null ||
    //   copy?.problemCategoryDescription === undefined ||
    //   copy?.problemCategoryDescription.trim() === ""
    // ) {
    //   addInvalidProperty("problemCategoryDescription");
    // }

    // if (
    //   copy?.maintenanceReference === null ||
    //   copy?.maintenanceReference === undefined ||
    //   copy?.maintenanceReference.trim() === ""
    // ) {
    //   addInvalidProperty("maintenanceReference");
    // }

    // if (
    //   copy?.mitigation === null ||
    //   copy?.mitigation === undefined ||
    //   copy?.mitigation.trim() === ""
    // ) {
    //   addInvalidProperty("mitigation");
    // }

    // if (
    //   copy?.solutionDescription === null ||
    //   copy?.solutionDescription === undefined ||
    //   copy?.solutionDescription.trim() === ""
    // ) {
    //   addInvalidProperty("solutionDescription");
    // }
    // if (
    //   copy?.patchReference === null ||
    //   copy?.patchReference === undefined ||
    //   copy?.patchReference.trim() === ""
    // ) {
    //   addInvalidProperty("patchReference");
    // }
    // if (
    //   copy?.productUpgradeReference === null ||
    //   copy?.productUpgradeReference === undefined ||
    //   copy?.productUpgradeReference.trim() === ""
    // ) {
    //   addInvalidProperty("productUpgradeReference ");
    // }
    // if (
    //   copy?.suppleMental === null ||
    //   copy?.suppleMental === undefined ||
    //   copy?.suppleMental.trim() === ""
    // ) {
    //   addInvalidProperty("suppleMental");
    // }

    if (copy?.environmentId === null || copy?.environmentId === undefined) {
      addInvalidProperty("environmentId");
    }

    if (copy?.opCoId === null || copy?.opCoId === undefined) {
      addInvalidProperty("opCoId");
    }

    if (
      copy?.problemCategoryId === null ||
      copy?.problemCategoryId === undefined
    ) {
      addInvalidProperty("problemCategoryId");
    }

    if (copy?.systemTypeId === null || copy?.systemTypeId === undefined) {
      addInvalidProperty("systemTypeId");
    }

    if (copy?.severity === null || copy?.severity === undefined) {
      addInvalidProperty("severity");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as TestInfoDtoUpdate;
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
    const copy = { ...formData } as TestInfoDtoUpdate;
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
    if (formData) {
      setFormData({
        ...formData,
        problemCategoryTypes: obj as { [key: string]: string },
      });
    }
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
        <legend className="text-bb">System Details</legend>
        <div className="row mt-3">
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${LabelsDictionary["opCo"]?.Full ?? "opCo"}`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.opCos != undefined
                    ? dictionaryToArray(formData.opCos).find(
                        (x) => x.key === formData?.opCoId
                      )
                    : null
                }
                options={
                  formData && formData.opCos != undefined
                    ? dictionaryToArray(formData.opCos)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("opCoId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("opCoId")
                    ? true
                    : false
                }
                error="OpCo must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["systemTypes"]?.Full ?? "System Type"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.systemTypes != undefined
                    ? formData.systemTypes.find(
                        (x) => x.key === formData?.systemTypeId
                      )
                    : null
                }
                options={formData?.systemTypes ?? null}
                onChange={(e: any) => onChangeDropdown("systemTypeId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("systemTypeId")
                    ? true
                    : false
                }
                error="System Type must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["subNetwork"]?.Full ?? "Sub Network"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.subNetwork ?? ""}
                onChange={(e: any) => onChange("subNetwork", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("subNetwork")
                    ? true
                    : false
                }
                error="Sub Network must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["environmentTypes"]?.Full ?? "Environment"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.environmentTypes != undefined
                    ? dictionaryToArray(formData.environmentTypes).find(
                        (x) => x.key === formData?.environmentId
                      )
                    : null
                }
                options={
                  formData && formData.environmentTypes != undefined
                    ? dictionaryToArray(formData.environmentTypes)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("environmentId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("environmentId")
                    ? true
                    : false
                }
                error="Environment must have a value."
              />
            </div>
          </div>
        </div>
        <legend className="text-bb">Problem Classification</legend>
        <div className="row mt-3">
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["severityTypes"]?.Full ?? "Severity"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.severityTypes != undefined
                    ? dictionaryToArray(formData.severityTypes).find(
                        (x) => x.key === formData?.severity
                      )
                    : null
                }
                options={
                  formData && formData.severityTypes != undefined
                    ? dictionaryToArray(formData.severityTypes)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("severity", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("severity")
                    ? true
                    : false
                }
                error="Severity must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["problemCategoryTypes"]?.Full ??
                  "Problem Category"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.problemCategoryTypes != undefined
                    ? dictionaryToArray(formData.problemCategoryTypes).find(
                        (x) => x.key === formData?.problemCategoryId
                      )
                    : null
                }
                options={
                  formData && formData.problemCategoryTypes != undefined
                    ? dictionaryToArray(formData.problemCategoryTypes)
                    : null
                }
                onChange={(e: any) => onChangeDropdown("problemCategoryId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("problemCategoryId")
                    ? true
                    : false
                }
                error="Problem Category must have a value."
                isAdd={tipologicaPermesso ? true : false}
                onAddClicked={() => setIsVisibleModalLookup(1)}
              />
            </div>
          </div>
        </div>
        <legend className="text-bb">Problem Details</legend>
        <div className="row mt-3">
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${LabelsDictionary["problemId"]?.Full ?? "Problem Id"}`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.problemId ?? ""}
                onChange={(e: any) => onChange("problemId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("problemId")
                    ? true
                    : false
                }
                error="Problem Id must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextAreaInputComponent
                label={`${
                  LabelsDictionary["problemDescription"]?.Full ??
                  "Problem Description"
                }`}
                // required={true}
                areaRow={3}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.problemDescription ?? ""}
                onChange={(e: any) => onChange("problemDescription", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("problemDescription")
                    ? true
                    : false
                }
                error="Problem Description must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DateInputComponent
                label={`${LabelsDictionary["dateFound"]?.Full ?? "Date Found"}`}
                value={formData?.dateFound ?? null}
                onChange={(e: any, date: any) =>
                  changeDateTime("dateFound", date)
                }
                labelCSS="mb-0"
                showTimeSelect={true}
                dateFormat={"MMMM d, yyyy h:mm aa"}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("dateFound")
                    ? true
                    : false
                }
                error="Date Found must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${LabelsDictionary["vendorCsr"]?.Full ?? "Vendor CSR"}`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.vendorCsr ?? ""}
                onChange={(e: any) => onChange("vendorCsr", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("vendorCsr")
                    ? true
                    : false
                }
                error="Vendor CSR must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${LabelsDictionary["statusURL"]?.Full ?? "Status URL"}`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.statusUrl ?? ""}
                onChange={(e: any) => onChange("statusUrl", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("statusUrl")
                    ? true
                    : false
                }
                error="Status URL must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextAreaInputComponent
                label={`${
                  LabelsDictionary["mitigation"]?.Full ?? "Mitigation"
                }`}
                // required={true}
                areaRow={3}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.mitigation ?? ""}
                onChange={(e: any) => onChange("mitigation", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("mitigation")
                    ? true
                    : false
                }
                error="Mitigation must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextAreaInputComponent
                label={`${
                  LabelsDictionary["solutionDescription"]?.Full ??
                  "Solution Description"
                }`}
                // required={true}
                areaRow={3}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.solutionDescription ?? ""}
                onChange={(e: any) => onChange("solutionDescription", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("solutionDescription")
                    ? true
                    : false
                }
                error="Solution Description must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextAreaInputComponent
                label={`${
                  LabelsDictionary["suppleMental"]?.Full ?? "Supplemental"
                }`}
                // required={true}
                areaRow={3}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.suppleMental ?? ""}
                onChange={(e: any) => onChange("suppleMental", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("suppleMental")
                    ? true
                    : false
                }
                error="Supplemental must have a value."
              />
            </div>
          </div>
        </div>
        <legend className="text-bb">External Reference Links</legend>
        <div className="row mt-3">
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["patchReference"]?.Full ?? "Patch Reference"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.patchReference ?? ""}
                onChange={(e: any) => onChange("patchReference", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("patchReference")
                    ? true
                    : false
                }
                error="Patch Reference must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["productUpgradeReference"]?.Full ??
                  "Product Upgrade Reference"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.productUpgradeReference ?? ""}
                onChange={(e: any) => onChange("productUpgradeReference", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("productUpgradeReference")
                    ? true
                    : false
                }
                error="Product Upgrade Reference must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextAreaInputComponent
                label={`${
                  LabelsDictionary["maintenanceReference"]?.Full ??
                  "Maintenance Reference"
                }`}
                // required={true}
                areaRow={3}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.maintenanceReference ?? ""}
                onChange={(e: any) => onChange("maintenanceReference", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("maintenanceReference")
                    ? true
                    : false
                }
                error="Maintenance Reference must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["testReport"]?.Full ?? "Test Report"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.testReport ?? ""}
                onChange={(e: any) => onChange("testReport", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("testReport")
                    ? true
                    : false
                }
                error="Test Report must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["standardNir"]?.Full ?? "Standard NIR"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.standardNir ?? ""}
                onChange={(e: any) => onChange("standardNir", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("standardNir")
                    ? true
                    : false
                }
                error="Standard NIR must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["ericssonSecReport"]?.Full ??
                  "Ericsson Sec Report"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.ericssonSecReport ?? ""}
                onChange={(e: any) => onChange("ericssonSecReport", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("ericssonSecReport")
                    ? true
                    : false
                }
                error="Ericsson Sec Report must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["swAndStEntries"]?.Full ??
                  "SW and/or System Type Entries"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.swAndStEntries ?? ""}
                onChange={(e: any) => onChange("swAndStEntries", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("swAndStEntries")
                    ? true
                    : false
                }
                error="SW and/or System Type Entries must have a value."
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <TextInputComponent
                label={`${
                  LabelsDictionary["penTestingReport"]?.Full ??
                  "Pen Testing Report"
                }`}
                // required={true}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                value={formData?.penTestingReport ?? ""}
                onChange={(e: any) => onChange("penTestingReport", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("penTestingReport")
                    ? true
                    : false
                }
                error="Pen Testing Report must have a value."
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
export default TestInfoModal;
