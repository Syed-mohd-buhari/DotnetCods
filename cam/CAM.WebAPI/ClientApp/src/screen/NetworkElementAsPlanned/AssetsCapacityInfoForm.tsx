import React, { useEffect, useState } from "react";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import {
  DateInputComponent,
  DropdownInputComponent,
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";
import { AssetCapacityInfoGridDto } from "../../Model/NetworkElementAsPlanned";

interface AssetsCapacityInfoFormProps {
  isVisible: boolean;
  closeModal: () => void;
  onAdd: (data: AssetCapacityInfoGridDto) => void;
  existingData?: AssetCapacityInfoGridDto | null;
}

const AssetsCapacityInfoForm: React.FC<AssetsCapacityInfoFormProps> = ({
  isVisible,
  closeModal,
  onAdd,
  existingData,
}) => {
  const [inputError, setInputError] = useState<{
    field: string;
    type: string;
  } | null>(null);

  const dummFunction = (data: AssetCapacityInfoGridDto) => {
    console.log("Form data:", data);
    return Promise.resolve(data);
  };

  const { formData, setFormData, onChange } =
    useFormTableCrud<AssetCapacityInfoGridDto>(dummFunction, dummFunction);

  useEffect(() => {
    if (existingData) setFormData(existingData);
  }, [existingData, setFormData]);

  const handleAdd = () => {
    if (!formData) return;
    onAdd(formData);
    closeModal();
  };

  return (
    <div className="row">
      <div className="col-6 mb-3">
        <TextInputComponent
          label="Physical Server Host Name"
          value={formData?.physicalServerHostName ?? ""}
          onChange={(e) => onChange("physicalServerHostName", e)}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="IP Address"
          value={formData?.physicalServerIpAddress ?? ""}
          onChange={(e) => onChange("physicalServerIpAddress", e)}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="Serial Number"
          value={formData?.physicalServerSerialNumber ?? ""}
          onChange={(e) => onChange("physicalServerSerialNumber", e)}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="No of Instances"
          value={formData?.noOfInstances ?? ""}
          validationError={inputError?.field === "noOfInstances" ? true : false}
          error={
            inputError?.field === "noOfInstances"
              ? "*Only numbers are allowed."
              : undefined
          }
          onChange={(e) => {
            const value = e.target.value;

            if (/^\d*$/.test(value)) {
              onChange("noOfInstances", e);

              setInputError(null);
            } else {
              setInputError({
                field: "noOfInstances",

                type: "number",
              });
            }
          }}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="vCPU"
          value={formData?.vcpu ?? ""}
          validationError={inputError?.field === "vcpu" ? true : false}
          error={
            inputError?.field === "vcpu"
              ? "*Only numbers are allowed."
              : undefined
          }
          onChange={(e) => {
            const value = e.target.value;

            if (/^\d*$/.test(value)) {
              onChange("vcpu", e);

              setInputError(null);
            } else {
              setInputError({
                field: "vcpu",

                type: "number",
              });
            }
          }}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="Memory"
          value={formData?.memory ?? ""}
          validationError={inputError?.field === "memory" ? true : false}
          error={
            inputError?.field === "memory"
              ? "*Only Decimal are allowed."
              : undefined
          }
          onChange={(e) => {
            const value = e.target.value;

            if (/^\d*\.?\d*$/.test(value)) {
              onChange("memory", e);

              setInputError(null);
            } else {
              setInputError({
                field: "memory",

                type: "number",
              });
            }
          }}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      <div className="col-6 mb-3">
        <TextInputComponent
          label="Storage"
          value={formData?.storage ?? ""}
          validationError={inputError?.field === "storage" ? true : false}
          error={
            inputError?.field === "storage"
              ? "*Only Decimal are allowed."
              : undefined
          }
          onChange={(e) => {
            const value = e.target.value;

            if (/^\d*\.?\d*$/.test(value)) {
              onChange("storage", e);

              setInputError(null);
            } else {
              setInputError({
                field: "storage",

                type: "number",
              });
            }
          }}
          labelCSS="mb-0"
          inputCSS="labelForm voda-bold mb-2"
        />
      </div>
      {/* <div className="d-flex justify-content-end mt-3">
        <button
          type="button"
          className="btn btn-secondary me-2"
          onClick={closeModal}
        >
          Cancel
        </button>
        <button type="button" className="btn btn-primary" onClick={handleAdd}>
          Add
        </button>
      </div> */}
      <div
        className="col-12 mx-0"
        style={{
          padding: "1rem",
          paddingLeft: "1rem !important",
          display: "flex",
          justifyContent: "right",
        }}
      >
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={closeModal}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={handleAdd}
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default AssetsCapacityInfoForm;
