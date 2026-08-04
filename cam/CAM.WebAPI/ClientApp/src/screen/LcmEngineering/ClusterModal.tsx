import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import {
  DropdownInputComponent,
  TextInputComponent,
} from "../../Components/FormField";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    onSaveCluster(data?: any): any;
  };
  edit: boolean;
}

const ClusterModal = (props: Props) => {
  const [formData, setFormData] = useState<any>(null);
  const [opcoOptions, setOpcoOptions] = useState<any>();
  const [platformOptions, setPlatformOptions] = useState<any>();
  const [locationOptions, setLocationOptions] = useState<any>();
  const [clusterTypeOptions, setClusterTypeOptions] = useState<any>();
  const [verticalResOptions, setVerticalResOptions] = useState<any>();
  const [hardwareTypeOptions, setHardwareTypeOptions] = useState<any>();
  const [deploymentStatusOptions, setDeploymentStatusOptions] = useState<any>();
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);

  const dtoNewResourceState = (state: RootState) =>
    state.infraClusterCreateReducer.InfraClusterDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  const dtoEditResourceState = (state: RootState) =>
    state.infraClusterEditReducer.InfraClusterDtoEdit;
  let editResource: any = useSelector(dtoEditResourceState);

  //Validation on Save
  const validationClient = (copy: any) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;
    console.log("Co", copy);
    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opCoId == null ||
      copy?.opCoId === undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }

    if (
      copy?.locationId == null ||
      copy?.locationId === undefined ||
      copy?.locationId === 0
    ) {
      addInvalidProperty("locationId");
    }

    if (copy?.site == "" || copy?.site === undefined) {
      addInvalidProperty("site");
    }

    if (copy?.clusterName == "" || copy?.clusterName === undefined) {
      addInvalidProperty("clusterName");
    }

    if (
      copy?.hardwaretypeId == null ||
      copy?.hardwaretypeId === undefined ||
      copy?.hardwaretypeId === 0
    ) {
      addInvalidProperty("hardwaretypeId");
    }

    if (
      copy?.deploymentStatusId == null ||
      copy?.deploymentStatusId === undefined ||
      copy?.deploymentStatusId === 0
    ) {
      addInvalidProperty("deploymentStatusId");
    }

    if (
      copy?.clustertypeId == null ||
      copy?.clustertypeId === undefined ||
      copy?.clustertypeId === 0
    ) {
      addInvalidProperty("clustertypeId");
    }

    if (
      copy?.platformId == null ||
      copy?.platformId === undefined ||
      copy?.platformId === 0
    ) {
      addInvalidProperty("platformId");
    }

    if (
      copy?.verticalResponsibleId == null ||
      copy?.verticalResponsibleId === undefined ||
      copy?.verticalResponsibleId === 0
    ) {
      addInvalidProperty("verticalResponsibleId");
    }
    console.log("copyValidation", copyValidation);
    setValidation(copyValidation);
    return copyValidation;
  };
  useEffect(() => {
    if (!props.edit) {
      const opCoList = createResource?.opcoReosurce?.map((item) => ({
        key: item?.key,
        value: item?.text,
      }));
      setOpcoOptions(opCoList);
      setLocationOptions(createResource?.locationReosurce);
      setPlatformOptions(
        createResource?.platfromResource?.map((item) => ({
          key: item?.key,
          value: item?.text,
        }))
      );
      setClusterTypeOptions(
        createResource?.clusterTypeMswResource?.map((item) => ({
          key: item?.key,
          value: item?.text,
        }))
      );
      setVerticalResOptions(
        createResource?.verticalResponsibleResource
          ? dictionaryToArray(createResource?.verticalResponsibleResource)
          : []
      );
      setHardwareTypeOptions(
        createResource?.hardwareMhwResource?.map((item) => ({
          key: item?.key,
          value: item?.text,
        }))
      );
      setDeploymentStatusOptions(createResource?.deploymentStatusReosurce);
      setFormData({
        ...formData,
        opCoId: opCoList?.[0]?.key,
        opCoValue: opCoList?.[0]?.value,
        deploymentStatusId: createResource?.deploymentStatusReosurce?.filter(
          (res) => res.value?.toLowerCase() === "planned"
        )[0]?.key,
        deploymentStatusValue: createResource?.deploymentStatusReosurce?.filter(
          (res) => res.value?.toLowerCase() === "planned"
        )[0]?.value,
      });
    }
  }, [createResource, editResource, props.edit]);

  const onChangeDropdown = (
    { property, value }: { property: string; value?: Array<string> },
    e: any
  ) => {
    let clusterUpdatedData = { ...formData } as any;
    if (property !== null && property !== undefined)
      clusterUpdatedData[property] = e && e["key"];
    if (value !== undefined && value?.length > 0) {
      value?.map((valueType) => {
        clusterUpdatedData[valueType] = e && e["value"];
      });
    }
    setFormData(clusterUpdatedData);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const handleChange = (property: string, e: any) => {
    let clusterUpdatedData = { ...formData } as any;
    clusterUpdatedData[property] = e && e?.target?.value;
    setFormData(clusterUpdatedData);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onSaveCluster = () => {
    if (validationClient(formData).response == true) {
      props?.action?.onSaveCluster({ ...formData, infraClusterAsPlannedId: 0 });
      setFormData(null);
      props?.action?.closeModal();
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Operational",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  return (
    <>
      <div className="row">
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Opco"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                opcoOptions &&
                opcoOptions.filter((x) => x.key == formData?.opCoId)
              }
              options={opcoOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("opCoId")) ??
                false
              }
              disabled={true}
              error="Opco must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  { property: "opCoId", value: ["opCoValue"] },
                  e
                )
              }
            />
          </div>
        </div>

        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Location"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                locationOptions &&
                locationOptions
                  ?.filter((x) => x.opcoId == formData?.opCoId)[0]
                  ?.locationDetails?.map((res) => ({
                    key: res?.value,
                    value: res?.text,
                  }))
                  ?.filter((x) => x?.key === formData?.locationId)
              }
              options={locationOptions
                ?.filter((val) => formData?.opCoId === val.opcoId)?.[0]
                ?.locationDetails?.map((res) => ({
                  key: res.value,
                  value: res.text,
                }))}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("locationId")) ??
                false
              }
              disabled={
                !formData?.opCoId || formData?.opCoId === 0 ? true : false
              }
              error="Location must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  { property: "locationId", value: ["locationValue"] },
                  e
                )
              }
            />
          </div>
        </div>
        <div className="col-6 ">
          <div className="col-12 pl-0">
            <TextInputComponent
              label="Site"
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              value={formData?.site}
              required={true}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("site")) ??
                false
              }
              error={"*Site must have a value."}
              onChange={(e: any) => handleChange("site", e)}
            />
          </div>
        </div>
        <div className="col-6 ">
          <div className="col-12 pl-0">
            <TextInputComponent
              label="Cluster Name"
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              value={formData?.clusterName}
              required={true}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("clusterName")) ??
                false
              }
              error={"*Cluster Name must have a value."}
              onChange={(e: any) => handleChange("clusterName", e)}
            />
          </div>
        </div>
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Hardware Type"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                hardwareTypeOptions &&
                hardwareTypeOptions.filter(
                  (x) => x.key == formData?.hardwaretypeId
                )
              }
              options={hardwareTypeOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("hardwaretypeId")) ??
                false
              }
              error="Hardware Type must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  { property: "hardwaretypeId", value: ["hardwaretypeValue"] },
                  e
                )
              }
            />
          </div>
        </div>
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Deployment Status"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                deploymentStatusOptions &&
                deploymentStatusOptions.filter(
                  (x) => x.key == formData?.deploymentStatusId
                )
              }
              options={deploymentStatusOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("deploymentStatusId")) ??
                false
              }
              disabled={true}
              error="Deployment Status must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  {
                    property: "deploymentStatusId",
                    value: ["deploymentStatusValue"],
                  },
                  e
                )
              }
            />
          </div>
        </div>
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Cluster Type"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                clusterTypeOptions &&
                clusterTypeOptions.filter(
                  (x) => x.key == formData?.clustertypeId
                )
              }
              options={clusterTypeOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("clustertypeId")) ??
                false
              }
              error="Cluster Type must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  { property: "clustertypeId", value: ["clustertypeValue"] },
                  e
                )
              }
            />
          </div>
        </div>
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Platform"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                platformOptions &&
                platformOptions.filter((x) => x.key == formData?.platformId)
              }
              options={platformOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("platformId")) ??
                false
              }
              error="Platform must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  { property: "platformId", value: ["platformValue"] },
                  e
                )
              }
            />
          </div>
        </div>
        <div className="col-6">
          <div className="col-12 pl-0">
            <DropdownInputComponent
              label={"Vertical Responsible"}
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={true}
              required={true}
              value={
                verticalResOptions &&
                verticalResOptions.filter(
                  (x) => x.key == formData?.verticalResponsibleId
                )
              }
              options={verticalResOptions}
              isError={
                (validation &&
                  validation.response === false &&
                  validation.property?.includes("verticalResponsibleId")) ??
                false
              }
              error="Platform must have a value."
              onChange={(e: any) =>
                onChangeDropdown(
                  {
                    property: "verticalResponsibleId",
                    value: ["verticalResponsibleValue"],
                  },
                  e
                )
              }
            />
          </div>
        </div>
      </div>
      <div className="d-flex my-2" style={{ justifySelf: "self-end" }}>
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => {
            setFormData(null);
            props?.action?.closeModal();
          }}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => onSaveCluster()}
          type="button"
        >
          Add
        </button>
      </div>
    </>
  );
};

export default ClusterModal;
