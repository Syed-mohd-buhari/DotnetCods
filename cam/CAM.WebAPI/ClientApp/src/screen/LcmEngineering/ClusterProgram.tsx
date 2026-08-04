import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import {
  DropdownInputComponent,
  TextInputComponent,
} from "../../Components/FormField";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { setNotification } from "../../Redux/Action/NotificationAction";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    onSaveClusterProgram(data?: any): any;
  };
  edit: boolean;
  existingAppClusterNames?: string[];
}

const ClusterProgram = (props: Props) => {
  const [formData, setFormData] = useState<any>({});
  const [applicationOptions, setApplicationOptions] = useState<any[]>([]);
  const [deploymentStatusOptions, setDeploymentStatusOptions] = useState<any[]>(
    []
  );
  const [validation, setValidation] = useState<any>(null);

  const programState = (state: RootState) =>
    state.infraProgramClusterResource?.InfraClusterDtoCreate;
  const programResource = useSelector(programState);

  const validate = (data: any) => {
    let errors: string[] = [];

    if (!data?.appClusterName) errors.push("appClusterName");
    if (
      data?.appClusterName &&
      props.existingAppClusterNames?.includes(data.appClusterName)
    ) {
      errors.push("duplicateAppClusterName");
    }
    if (!data?.applicationId) errors.push("applicationId");
    if (!data?.deploymentStatusId) errors.push("deploymentStatusId");

    const result = { response: errors.length === 0, property: errors };
    setValidation(result);
    return result;
  };

  useEffect(() => {
    if (programResource) {
      setApplicationOptions(
        programResource?.applicationNames?.map((item: any) => ({
          key: item.key,
          value: item.text,
        }))
      );
      const depOptions = programResource?.deploymentStatuesResources?.map(
        (item: any) => ({
          key: item.key,
          value: item.text,
        })
      );
      const trafficFreeOption = depOptions?.find(
        (res) => res.value?.toUpperCase() === "TRAFFIC FREE"
      );

      setDeploymentStatusOptions(depOptions);
      const plannedStatus = depOptions?.find(
        (res) => res.value?.toLowerCase() === "planned"
      );

      if (plannedStatus) {
        setFormData((prev: any) => ({
          ...prev,
          deploymentStatusId: plannedStatus.key,
          deploymentStatusValue: plannedStatus.value,
        }));
      }
    }
  }, [programResource]);

  const onChangeDropdown = (
    { property, value }: { property: string; value?: string[] },
    e: any
  ) => {
    let updated = { ...formData };
    updated[property] = e?.key;

    if (value) {
      value.forEach((v) => (updated[v] = e?.value));
    }

    setFormData(updated);

    if (validation?.property?.includes(property)) {
      setValidation({
        ...validation,
        property: validation.property.filter((p: string) => p !== property),
      });
    }
  };

  const handleChange = (property: string, e: any) => {
    let updated = { ...formData };
    updated[property] = e?.target?.value;
    setFormData(updated);

    if (validation?.property?.includes(property)) {
      setValidation({
        ...validation,
        property: validation.property.filter((p: string) => p !== property),
      });
    }
    if (
      property === "appClusterName" &&
      validation?.property?.includes("duplicateAppClusterName")
    ) {
      setValidation({
        ...validation,
        property: validation.property.filter(
          (p: string) => p !== "duplicateAppClusterName"
        ),
      });
    }
  };

  const onSave = () => {
    const validationResult = validate(formData);
    if (validationResult.response) {
      props.action.onSaveClusterProgram(formData);
      props.action.closeModal(true);
    } else {
      if (validationResult.property?.includes("duplicateAppClusterName")) {
        rootStore.dispatch(
          setNotification({
            message: `App Cluster Name "${formData?.appClusterName}" already exists. Please use a unique name.`,
            notifyType: NotifyType.warning,
          })
        );
      } else {
        rootStore.dispatch(
          setNotification({
            message: "Please fill all required fields",
            notifyType: NotifyType.warning,
          })
        );
      }
    }
  };

  return (
    <>
      <div className="row">
        {/* Cluster Name */}
        <div className="col-6">
          <TextInputComponent
            label="App Cluster Name"
            value={formData?.appClusterName}
            required={true}
            isError={
              validation?.property?.includes("appClusterName") ||
              validation?.property?.includes("duplicateAppClusterName")
            }
            error={
              validation?.property?.includes("duplicateAppClusterName")
                ? "App Cluster Name already exists. Please enter a unique name."
                : "Cluster Name is required"
            }
            onChange={(e: any) => handleChange("appClusterName", e)}
          />
        </div>

        {/* Application Name */}
        <div className="col-6">
          <DropdownInputComponent
            label="Application Name"
            required={true}
            options={applicationOptions}
            value={applicationOptions.filter(
              (x) => x.key === formData?.applicationId
            )}
            isError={validation?.property?.includes("applicationId")}
            error="Application is required"
            onChange={(e: any) =>
              onChangeDropdown(
                { property: "applicationId", value: ["applicationName"] },
                e
              )
            }
          />
        </div>

        {/* Deployment Status */}
        <div className="col-6">
          <DropdownInputComponent
            label="Deployment Status"
            required={true}
            options={deploymentStatusOptions}
            value={deploymentStatusOptions.filter(
              (x) => x.key === formData?.deploymentStatusId
            )}
            isError={validation?.property?.includes("deploymentStatusId")}
            error="Deployment Status is required"
            onChange={(e: any) =>
              onChangeDropdown(
                {
                  property: "deploymentStatusId",
                  value: ["deploymentStatusValue"],
                },
                e
              )
            }
            disabled={true}
          />
        </div>
      </div>

      {/* Buttons */}
      <div className="d-flex my-2" style={{ justifyContent: "flex-end" }}>
        <button
          className="btn btn-link px-4"
          onClick={() => props.action.closeModal()}
        >
          Cancel
        </button>

        <button className="btn btn-danger px-4" onClick={onSave}>
          Add
        </button>
      </div>
    </>
  );
};

export default ClusterProgram;
