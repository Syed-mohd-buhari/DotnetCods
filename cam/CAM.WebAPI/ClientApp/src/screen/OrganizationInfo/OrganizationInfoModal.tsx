import React, { useCallback, useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import {
  OrganizationInfoDtoGrid,
  OrganizationInfoDtoUpdate,
} from "../../Model/OrganizationInfo";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import {
  EditOrganizationInfo,
  EditResourceOrganizationInfoRefillData,
} from "../../Redux/Action/OrganizationInfo/OrganizationInfoEditAction";
import {
  CreatOrganizationInfo,
  CreateResourceOrganizationInfoRefillData,
} from "../../Redux/Action/OrganizationInfo/OrganizationInfoCreateAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { formatDateWithTime } from "../../Hook/Common";
import {
  DateInputComponent,
  DropdownInputComponent,
  TextAreaInputComponent,
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import MainOrganisationContainer from "../../Containers/Lookup/MainOrganisationContainer";
import { GetAllOpCosAndVerticalResponsibles } from "../../Redux/Action/OrganizationInfo/OrganizationInfoGridAction";
import PracticeContainer from "../../Containers/Lookup/PracticeContainer";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import ModalRelated from "../../Components/ModalRelated";
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

const OrganizationInfoModal: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<OrganizationInfoDtoUpdate>(
    CreatOrganizationInfo,
    EditOrganizationInfo
  );

  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const dtoEditResourceState = (state: RootState) =>
    state.organizationInfoEditReducer.OrganizationInfoDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.organizationInfoCreateReducer.OrganizationInfoDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: OrganizationInfoDtoGrid) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.mainOrganisationId === null ||
      copy?.mainOrganisationId === undefined ||
      copy?.mainOrganisationId === 0
    ) {
      addInvalidProperty("mainOrganisationId");
    }

    if (
      copy?.contactId === null ||
      copy?.contactId === undefined ||
      copy?.contactId === 0
    ) {
      addInvalidProperty("contactId");
    }

    if (
      copy?.practiceContactId === null ||
      copy?.practiceContactId === undefined ||
      copy?.practiceContactId === 0
    ) {
      addInvalidProperty("practiceContactId");
    }
    if (
      copy?.practiceId === null ||
      copy?.practiceId === undefined ||
      copy?.practiceId === 0
    ) {
      addInvalidProperty("practiceId");
    }
    if (
      copy?.subdomainResponsibleId === null ||
      copy?.subdomainResponsibleId === undefined ||
      copy?.subdomainResponsibleId === 0
    ) {
      addInvalidProperty("subdomainResponsibleId");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as OrganizationInfoDtoUpdate;
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
    const copy = { ...formData } as OrganizationInfoDtoUpdate;
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
  const MainOrganisationRefillData = async () => {
    const copy = { ...formData } as OrganizationInfoDtoUpdate;

    if (props.edit) {
      const result = await EditResourceOrganizationInfoRefillData(
        formData?.organisationId ?? 0
      );
      copy.mainOrganisations =
        result?.mainOrganisations ?? copy.mainOrganisations;
      setFormData(copy);
    } else {
      const result = await CreateResourceOrganizationInfoRefillData();
      copy.mainOrganisations =
        result?.mainOrganisations ?? copy.mainOrganisations;
      setFormData(copy);
    }
  };

  const PracticeRefillData = async () => {
    const copy = { ...formData } as OrganizationInfoDtoUpdate;

    if (props.edit) {
      const result = await EditResourceOrganizationInfoRefillData(
        formData?.organisationId ?? 0
      );
      copy.practices = result?.practices ?? copy.practices;
      copy.practiceContacts = result?.practiceContacts ?? copy.practiceContacts;
      copy.practiceMethods = result?.practiceMethods ?? copy.practiceMethods;
      setFormData(copy);
    } else {
      const result = await CreateResourceOrganizationInfoRefillData();
      copy.practices = result?.practices ?? copy.practices;
      setFormData(copy);
    }
  };

  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 1:
          return (
            <MainOrganisationContainer
              returnObject={MainOrganisationRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></MainOrganisationContainer>
          );
        case 2:
          return (
            <PracticeContainer
              returnObject={PracticeRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></PracticeContainer>
          );
        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );

  const fetchResponse = async (id: number) => {
    try {
      const data = await GetAllOpCosAndVerticalResponsibles(id);
      if (data !== null && formData) {
        setFormData({
          ...formData,
          opCo: data?.["OpCos"] ?? "",
          verticalResponsible: data?.["VerticalResponsibles"] ?? "",
        });
      }
    } catch (err) {
      console.error("Error fetching data:", err);
    }
  };

  useEffect(() => {
    if (
      formData &&
      formData.contactId !== null &&
      formData.contactId !== undefined &&
      formData.contactId !== 0
    ) {
      fetchResponse(formData?.contactId);
    }
  }, [formData?.contactId]);

  useEffect(() => {
    if (
      formData &&
      formData.practiceId !== null &&
      formData.practiceId !== undefined &&
      formData.practiceId !== 0 &&
      formData.practiceMethods !== null &&
      formData.practiceMethods !== undefined
    ) {
      const practiceHeadId: any = dictionaryToArray(formData?.practiceMethods)
        .filter((value) => value.key === formData.practiceId)
        .map((value) => value.value)[0];
      setFormData({ ...formData, practiceContactId: practiceHeadId ?? 0 });
    }
  }, [formData?.practiceId]);

  const onSave = async () => {
    try {
      const result = await Save(
        formData,
        props.edit,
        validazioneClient,
        refresh
      );
      if (
        result?.data !== null &&
        result?.data?.dataRelatedList?.length !== 0 &&
        result?.warning === true
      ) {
        setIsVisibleModalRelated(true);
        setRelatedRecord(result.data);
      } else {
        setIsVisibleModalRelated(false);
        props.action.closeModal(false);
      }
    } catch (err) {
      setIsVisibleModalRelated(false);
      props.action.closeModal(false);
      console.log(err);
    }
  };

  return (
    <div className="col-12">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        headerTitle="Edit Organisation Entry"
        action={{
          closeModal: () => {
            setIsVisibleModalRelated(false);
          },
        }}
      />
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
        <div className="row">
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["mainOrganisation"]?.Full ??
                  "Main Organisation"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.mainOrganisations != undefined
                    ? dictionaryToArray(formData.mainOrganisations).find(
                        (x) => x.key === formData?.mainOrganisationId
                      )
                    : null
                }
                options={
                  formData && formData.mainOrganisations != undefined
                    ? dictionaryToArray(formData.mainOrganisations)
                    : []
                }
                onChange={(e: any) => onChangeDropdown("mainOrganisationId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("mainOrganisationId")
                    ? true
                    : false
                }
                disabled={false}
                error="Main Organisation must have a value."
                isAdd={tipologicaPermesso}
                onAddClicked={() => setIsVisibleModalLookup(1)}
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${LabelsDictionary["practice"]?.Full ?? "Practice"}`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.practices != undefined
                    ? dictionaryToArray(formData.practices).find(
                        (x) => x.key === formData?.practiceId
                      )
                    : null
                }
                options={
                  formData && formData.practices != undefined
                    ? dictionaryToArray(formData.practices)
                    : []
                }
                onChange={(e: any) => onChangeDropdown("practiceId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("practiceId")
                    ? true
                    : false
                }
                disabled={false}
                isAdd={tipologicaPermesso}
                error="Practice must have a value."
                onAddClicked={() => setIsVisibleModalLookup(2)}
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["practiceContact"]?.Full ?? "Practice Head"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.practiceContacts != undefined
                    ? dictionaryToArray(formData.practiceContacts).find(
                        (x) => x.key === formData?.practiceContactId
                      )
                    : null
                }
                options={
                  formData && formData.practiceContacts != undefined
                    ? dictionaryToArray(formData.practiceContacts).filter(
                        (val) => val.key === formData?.practiceContactId
                      )
                    : []
                }
                onChange={(e: any) => onChangeDropdown("practiceContactId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("practiceContactId")
                    ? true
                    : false
                }
                disabled={false}
                error="Practice Head must have a value."
                onAddClicked={() => setIsVisibleModalLookup(1)}
              />
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${LabelsDictionary["Contacts"]?.Full ?? "Contacts"}`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.contacts != undefined
                    ? dictionaryToArray(formData.contacts).find(
                        (x) => x.key === formData?.contactId
                      )
                    : null
                }
                options={
                  formData && formData.contacts != undefined
                    ? dictionaryToArray(formData.contacts)
                    : []
                }
                onChange={(e: any) => onChangeDropdown("contactId", e)}
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("contactId")
                    ? true
                    : false
                }
                error="Contact must have a value."
                onAddClicked={() => setIsVisibleModalLookup(1)}
              />
            </div>
          </div>
          {formData?.contactId !== 0 &&
            formData?.contactId !== null &&
            formData?.contactId !== undefined && (
              <>
                <div className="col-6">
                  <div className="form-group">
                    <TextInputComponent
                      label={`${LabelsDictionary["opCo"]?.Full ?? "OpCo"}`}
                      disabled={true}
                      isList={true}
                      labelCSS="mb-0"
                      inputCSS="labelForm mb-2"
                      value={formData?.opCo ?? ""}
                      onChange={(e: any) => onChange("opCo", e)}
                    />
                  </div>
                </div>
                <div className="col-6">
                  <div className="form-group">
                    <TextInputComponent
                      label={`${
                        LabelsDictionary["verticalResponsible"]?.Full ??
                        "Vertical Responsible"
                      }`}
                      isList={true}
                      disabled={true}
                      labelCSS="mb-0"
                      inputCSS="labelForm mb-2"
                      value={formData?.verticalResponsible ?? ""}
                      onChange={(e: any) => onChange("verticalResponsible", e)}
                    />
                  </div>
                </div>
              </>
            )}
          <div className="col-6">
            <div className="form-group">
              <DropdownInputComponent
                label={`${
                  LabelsDictionary["subDomainResponsibles"]?.Full ??
                  "Sub Domain Responsible"
                }`}
                labelCSS="mb-0"
                inputCSS="labelForm voda-bold mb-2"
                isSearchable={true}
                isClearable={true}
                required={true}
                value={
                  formData && formData?.subDomainResponsibles != undefined
                    ? dictionaryToArray(formData.subDomainResponsibles).find(
                        (x) => x.key === formData?.subdomainResponsibleId
                      )
                    : null
                }
                options={
                  formData && formData.subDomainResponsibles != undefined
                    ? dictionaryToArray(formData.subDomainResponsibles)
                    : []
                }
                onChange={(e: any) =>
                  onChangeDropdown("subdomainResponsibleId", e)
                }
                isError={
                  validation &&
                  validation.response === false &&
                  validation.property?.includes("subdomainResponsibleId")
                    ? true
                    : false
                }
                error="Sub Domain Responsible must have a value."
                onAddClicked={() => setIsVisibleModalLookup(1)}
              />
            </div>
          </div>
          <div className="col-6">
            <div className="row">
              <div className="col-6">
                <div className="form-group">
                  <ToggleInputComponent
                    label={"Is Edu Spoc?"}
                    value={formData?.isEduSpoc ?? false}
                    required={false}
                    onChange={(e: any) => onChange("isEduSpoc", e)}
                  />
                </div>
              </div>
              <div className="col-6">
                <div className="form-group">
                  <ToggleInputComponent
                    label={"Is Sub Domain Spoc?"}
                    value={formData?.isSubDomainSpoc ?? false}
                    required={false}
                    onChange={(e: any) => onChange("isSubDomainSpoc", e)}
                  />
                </div>
              </div>
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
          onClick={() => onSave()}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};
export default OrganizationInfoModal;
