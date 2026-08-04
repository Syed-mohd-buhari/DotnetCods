import React, {
  useEffect,
  useState,
  forwardRef,
  useImperativeHandle,
} from "react";
import {
  DropdownInputComponent,
  MultiSelectComponent,
  MultiSelectWithDescription,
  ToggleInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";

interface Props {
  initialData?: any;
  readOnly?: boolean;
  userId?: number;
  userEmail?: string;
  opcoRes?: any;
  verticalRes?: any;
  roleRes?: any;
  subdomRes?: any;
  onChangeMainDropdown(e: any, type: string): any;
  opcoValue?: any;
  roleValue?: any;
  subDomainValue?: any;
  verticalValue?: any;
  restrictedOpcoValue?: any;
  isDesignContact?: boolean;
  isEduSpoc?: boolean;
  isSubDomainSpoc?: boolean;
  roleDescriptions?: Record<string, string>;
  verticalResponsibleRes?: any;
  verticalResponsibleValue?: any;
}

const EmbeddedOrganizationInfo = forwardRef((props: Props, ref) => {
  const { initialData, readOnly = false } = props;

  const [formData, setFormData] = useState<any>({
    mainOrganisationId: null,
    practiceId: null,
    practiceContactId: null,
    contactId: null,
    subdomainResponsibleId: null,
    opCo: "",
    verticalResponsible: "",
    restrictedOpco: [],
    isEduSpoc: props?.isEduSpoc ?? false,
    isSubDomainSpoc: props?.isSubDomainSpoc ?? false,
    isDesignContact: props?.isDesignContact ?? false,
  });
  const [initialFormData, setInitialFormData] = useState<any>(null);
  const [errors, setErrors] = useState<any>({});

  const { admin } = useAuth();
  const showRestrictedOpco = admin;

  useImperativeHandle(ref, () => ({
    getFormData: () => ({
      ...formData,
      contactId: props.userId || formData.contactId,
    }),
    validate: validateForm,
    hasChanges: () => {
      return JSON.stringify(formData) !== JSON.stringify(initialFormData);
    },
  }));

  useEffect(() => {
    setFormData((prev: any) => ({
      ...prev,
      isEduSpoc: props?.isEduSpoc ?? prev.isEduSpoc,
      isSubDomainSpoc: props?.isSubDomainSpoc ?? prev.isSubDomainSpoc,
      isDesignContact: props?.isDesignContact ?? prev.isDesignContact,
    }));
  }, [props.isEduSpoc, props.isSubDomainSpoc, props.isDesignContact]);
  useEffect(() => {
    if (initialData) {
      const data = {
        mainOrganisationId: initialData.mainOrganisationId || null,
        practiceId: initialData.practiceId || null,
        practiceContactId: initialData.practiceContactId || null,
        contactId: initialData.contactId || null,
        subdomainResponsibleId: initialData.subdomainResponsibleId || null,
        opCo: initialData.opCo || "",
        verticalResponsible: initialData.verticalResponsible || "",
        restrictedOpco: initialData.restrictedOpco || [],
        isEduSpoc:
          formData?.isEduSpoc != null
            ? formData?.isEduSpoc
            : props?.isEduSpoc || false,
        isSubDomainSpoc:
          formData?.isSubDomainSpoc != null
            ? formData?.isSubDomainSpoc
            : props?.isSubDomainSpoc || false,
        isDesignContact:
          formData?.isDesignContact != null
            ? formData?.isDesignContact
            : props?.isDesignContact || false,
      };

      setFormData(data);
      setInitialFormData(data);
    }
  }, [initialData]);

  const validateForm = () => {
    const newErrors: any = {};
    const org = initialData?.[0] || {};

    // if (!org.mainOrganisationId && !org.organisationId) {
    //   newErrors.mainOrganisationId = "Main Organisation is required";
    // }
    // if (!org.practiceId) {
    //   newErrors.practiceId = "Practice is required";
    // }
    // if (!org.practiceContactId) {
    //   newErrors.practiceContactId = "Practice Head is required";
    // }
    if (!props.opcoValue?.length) {
      newErrors.opco = "OpCo is required";
    }
    if (!props.roleValue?.length) {
      newErrors.role = "Role is required";
    }
    if (!props.verticalValue?.length) {
      newErrors.vertical = "Vertical is required";
    }
    if (!props.subDomainValue?.length) {
      newErrors.subdomainResponsibleId = "Sub Domain Responsible is required";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleDropdownChange = (field: string, e: any) => {
    setFormData((prev: any) => ({
      ...prev,
      [field]: e?.key ?? null,
    }));

    setErrors((prev: any) => ({
      ...prev,
      [field]: "",
    }));
  };

  const handleToggleChange = (field: string, e: any) => {
    const value = e?.target?.checked;

    setFormData((prev: any) => ({
      ...prev,
      [field]: value,
    }));
  };

  return (
    <div className="row col-12 mt-4">
      <div className="col-4 mb-4">
        <MultiSelectComponent
          label={"Opco"}
          labelCSS={"mb-0"}
          inputCSS="labelForm voda-bold mb-2"
          isSearchable
          isClearable={false}
          required={true}
          value={
            (props?.opcoValue &&
              props?.opcoRes &&
              dictionaryToArray(props?.opcoRes)
                .filter((item) => props?.opcoValue.includes(item.key))
                .map((item) => ({
                  label: item.value,
                  value: item.key,
                }))) ??
            []
          }
          options={
            props?.opcoRes
              ? dictionaryToArray(props?.opcoRes).map((item) => ({
                  label: item.value,
                  value: item.key,
                }))
              : []
          }
          onChange={(e: any) => props?.onChangeMainDropdown(e, "opco")}
        />
        {errors.opco && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.opco}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <MultiSelectComponent
          label={"Role"}
          labelCSS={"mb-0"}
          inputCSS="labelForm voda-bold mb-2"
          isSearchable
          isClearable={false}
          required={true}
          value={
            (props?.roleValue &&
              props?.roleRes &&
              dictionaryToArray(props?.roleRes)
                .filter((item) => props?.roleValue.includes(item.key))
                .map((item) => ({
                  label: item.value,
                  value: item.key,
                }))) ??
            []
          }
          options={
            props?.roleRes
              ? dictionaryToArray(props?.roleRes).map((item) => ({
                  label: item.value,
                  value: item.key,
                }))
              : []
          }
          onChange={(e: any) => props?.onChangeMainDropdown(e, "role")}
          descriptions={props?.roleDescriptions || {}}
          showTooltip={true}
        />
        {errors.role && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.role}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <MultiSelectComponent
          label={"Vertical"}
          labelCSS={"mb-0"}
          inputCSS="labelForm voda-bold mb-2"
          isSearchable
          isClearable={false}
          required={true}
          value={
            (props?.verticalValue &&
              props?.verticalRes &&
              dictionaryToArray(props?.verticalRes)
                .filter((item) => props?.verticalValue.includes(item.key))
                .map((item) => ({
                  label: item.value,
                  value: item.key,
                }))) ??
            []
          }
          options={
            props?.verticalRes
              ? dictionaryToArray(props?.verticalRes).map((item) => ({
                  label: item.value,
                  value: item.key,
                }))
              : []
          }
          onChange={(e: any) => props?.onChangeMainDropdown(e, "vertical")}
        />
        {errors.vertical && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.vertical}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <ToggleInputComponent
          label="Is Edu Spoc?"
          value={formData.isEduSpoc ?? false}
          onChange={(e: any) => handleToggleChange("isEduSpoc", e)}
          disabled={readOnly}
        />
      </div>
      <div className="col-4 mb-4">
        <ToggleInputComponent
          label="Is Sub Domain Spoc?"
          value={formData.isSubDomainSpoc ?? false}
          onChange={(e: any) => handleToggleChange("isSubDomainSpoc", e)}
          disabled={readOnly}
        />
      </div>
      <div className="col-4 mb-4">
        <ToggleInputComponent
          label="Is Design Contact?"
          value={formData.isDesignContact ?? false}
          onChange={(e: any) => handleToggleChange("isDesignContact", e)}
          disabled={readOnly}
        />
      </div>
      <div className="col-4 mb-4">
        <DropdownInputComponent
          label={
            LabelsDictionary["mainOrganisation"]?.Full ?? "Main Organisation"
          }
          isSearchable
          isClearable={false}
          required={true}
          value={
            initialData?.length > 0 && initialData[0]?.mainOrganisation
              ? {
                  key:
                    initialData[0]?.mainOrganisationId ||
                    initialData[0]?.organisationId,
                  value: initialData[0]?.mainOrganisation,
                }
              : null
          }
          options={
            initialData?.length > 0 && initialData[0]?.mainOrganisation
              ? {
                  [initialData[0]?.mainOrganisationId ||
                  initialData[0]?.organisationId]:
                    initialData[0]?.mainOrganisation,
                }
              : {}
          }
          labelCSS={"mb-0"}
          onChange={(e: any) => handleDropdownChange("mainOrganisationId", e)}
          disabled={true}
        />
        {errors.mainOrganisationId && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.mainOrganisationId}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <DropdownInputComponent
          label={LabelsDictionary["practice"]?.Full ?? "Practice"}
          isSearchable
          isClearable={false}
          required={true}
          value={
            initialData?.length > 0 && initialData[0]?.practice
              ? {
                  key: initialData[0]?.practiceId,
                  value: initialData[0]?.practice,
                }
              : null
          }
          options={
            initialData?.length > 0 && initialData[0]?.practice
              ? {
                  [initialData[0]?.practiceId]: initialData[0]?.practice,
                }
              : {}
          }
          labelCSS={"mb-0"}
          onChange={(e: any) => handleDropdownChange("practiceId", e)}
          disabled={true}
        />
        {errors.practiceId && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.practiceId}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <DropdownInputComponent
          label={"Sub Domain Responsible"}
          labelCSS={"mb-0"}
          inputCSS="labelForm voda-bold mb-2"
          isSearchable
          isClearable={false}
          required={true}
          value={
            props?.subDomainValue?.length > 0 && props?.subdomRes
              ? dictionaryToArray(props.subdomRes).find(
                  (item: any) => item.key === props.subDomainValue[0]
                ) || null
              : null
          }
          options={props?.subdomRes ? dictionaryToArray(props.subdomRes) : []}
          onChange={(e: any) => props?.onChangeMainDropdown(e, "subDomain")}
        />

        {errors.subdomainResponsibleId && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.subdomainResponsibleId}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <DropdownInputComponent
          label={LabelsDictionary["practiceContact"]?.Full ?? "Practice Head"}
          isSearchable
          isClearable={false}
          required={true}
          labelCSS={"mb-0"}
          value={
            initialData?.length > 0 && initialData[0]?.practiceContact
              ? {
                  key: initialData[0]?.practiceContactId,
                  value: initialData[0]?.practiceContact,
                }
              : null
          }
          options={
            initialData?.length > 0 && initialData[0]?.practiceContact
              ? {
                  [initialData[0]?.practiceContactId]:
                    initialData[0]?.practiceContact,
                }
              : {}
          }
          onChange={(e: any) => handleDropdownChange("practiceContactId", e)}
          disabled={true}
        />
        {errors.practiceContactId && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.practiceContactId}
          </div>
        )}
      </div>
      <div className="col-4 mb-4">
        <DropdownInputComponent
          label={LabelsDictionary["Contacts"]?.Full ?? "Contacts"}
          isSearchable
          isClearable
          labelCSS={"mb-0"}
          required={true}
          value={
            props.userId
              ? {
                  key: props.userId,
                  value: `${props.userEmail}`,
                }
              : null
          }
          options={
            props.userId
              ? [
                  {
                    key: props.userId,
                    value: `${props.userEmail}`,
                  },
                ]
              : []
          }
          onChange={(e: any) => handleDropdownChange("contactId", e)}
          disabled={true}
        />
      </div>
      <div className="col-4 mb-4">
        <MultiSelectComponent
          label={"Vertical Responsible"}
          labelCSS={"mb-0"}
          inputCSS="labelForm voda-bold mb-2"
          isSearchable
          isClearable={false}
          required={false}
          value={
            (props?.verticalResponsibleValue &&
              props?.verticalResponsibleRes &&
              dictionaryToArray(props?.verticalResponsibleRes)
                .filter((item) =>
                  props?.verticalResponsibleValue.includes(item.key)
                )
                .map((item) => ({
                  label: item.value,
                  value: item.key,
                }))) ??
            []
          }
          options={
            props?.verticalResponsibleRes
              ? dictionaryToArray(props?.verticalResponsibleRes).map(
                  (item) => ({
                    label: item.value,
                    value: item.key,
                  })
                )
              : []
          }
          onChange={(e: any) =>
            props?.onChangeMainDropdown(e, "verticalResponsible")
          }
        />
        {errors.verticalResponsible && (
          <div
            className="text-danger small"
            style={{ fontWeight: "800", marginTop: "-15px" }}
          >
            {errors.verticalResponsible}
          </div>
        )}
      </div>
      {showRestrictedOpco && (
        <div className="col-4 mb-4">
          <MultiSelectComponent
            label={"Restricted Opco"}
            labelCSS={"mb-0"}
            inputCSS="labelForm voda-bold mb-2"
            isSearchable
            isClearable={true}
            required={false}
            value={
              (props?.restrictedOpcoValue &&
                props?.opcoRes &&
                dictionaryToArray(props?.opcoRes)
                  .filter((item) =>
                    props?.restrictedOpcoValue.includes(item.key)
                  )
                  .map((item) => ({
                    label: item.value,
                    value: item.key,
                  }))) ??
              []
            }
            options={
              props?.opcoRes
                ? dictionaryToArray(props?.opcoRes).map((item) => ({
                    label: item.value,
                    value: item.key,
                  }))
                : []
            }
            onChange={(e: any) =>
              props?.onChangeMainDropdown(e, "restrictedOpco")
            }
          />
        </div>
      )}
    </div>
  );
});

export default EmbeddedOrganizationInfo;
