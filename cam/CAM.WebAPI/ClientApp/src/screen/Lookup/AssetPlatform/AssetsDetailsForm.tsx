import React, { useEffect, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import {
  DaAssetMigrationDto,
  DaAssetMigrationDtoCreate,
  TargetDesignComponentResource,
} from "../../../Model/LookUp/AssetMigrationModels";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { createAssetMigration } from "../../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import Select from "react-select";

interface Props {
  existingData?: DaAssetMigrationDto;
  action: {
    closeModal(changed?: boolean): any;
    refresh?(): any;
    setChanged?(changed: boolean): void;
    onSubmit?(asset: DaAssetMigrationDto): void;
  };
  edit: boolean;
  paId: number;
  opcoId: number;
  existingAssetResource: { key: number; value: string }[];
  locationResource: Record<string, string>;
  environmentResource: Record<string, string>;
  targetDesignComponentResource: TargetDesignComponentResource[];
  deploymentStatusResource: Record<
    string,
    { deploymentStatusId: number; deploymentStatusDescription: string }
  >;
  formDisabled?: boolean;

  existingNewElementNames?: string[];
  localAssets?: DaAssetMigrationDto[];
  defaultDecommissioned?: boolean;
}

const AssetsDetailsForm: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    changed,
    setChanged,
    validation,
    setValidation,
  } = useFormTableCrud<DaAssetMigrationDtoCreate>(
    (data) => createAssetMigration(props.paId, data),
    (data) => createAssetMigration(props.paId, data)
  );

  const dto = useSelector(
    (state: RootState) => state.assetsDetailsReducer?.daAssetMigrationDtoCreate
  );

  const [duplicateError, setDuplicateError] = useState<string>("");
  // const [isCurrentAssetOptional, setIsCurrentAssetOptional] =
  //   useState<boolean>(false);
  const [isRemoveAsset, setIsRemoveAsset] = useState<boolean>(false);
  const defaultGridItem: DaAssetMigrationDto = {
    daAssetMigrationId: 0,
    plannedActivityId: props.paId ?? 0,
    networkElementAsPlannedId: 0,
    oldAssetName: "",
    newelEmentName: "",
    locationId: 0,
    newEnvironmentId: 0,
    targetDesignComponenetId: 0,
    deploymentStatusId: 0,
    rfoDate: "",
    rfsDate: "",
    rfaDate: "",
    hwPoRaisedDate: "",
    hwPoArrivedDate: "",
    bomSubmittedDate: "",
    migrationCompletionDate: "",
    trafficNodePercentage: "",
    vecDate: "",
    startOfAppIntegration: "",
    migrationStart: "",
    deleted: false,
    orphan: false,
    lastModified: "",
    lastModifiedBy: "",
    targetDesignComponenet: "",
    newEnvironment: "",
    deploymentStatus: "",
    opcoId: 0,
    opcoDesc: "",
    location: "",
    oldEnvironment: "",
    oldDeploymentType: "",
    oldDeploymentStatus: "",
  };

  useEffect(() => {
    if (props.existingData) {
      const existing: any = props.existingData;
      if (existing.newelEmentName === "" || existing.newelEmentName == null) {
        //existing.oldDeploymentStatus = "";
        existing.deploymentStatusId = 0;
      }
      const mappedData: DaAssetMigrationDto = {
        ...defaultGridItem,
        ...props.existingData,
        newEnvironmentId: existing.newEnvironmentId ?? 0,
        deploymentStatusId: existing.newDeploymentStatusId ?? 0,
        targetDesignComponenetId: existing.targetDesignComponenetId ?? 0,
        newelEmentName: existing.newelEmentName ?? "",
        locationId: existing.locationId ?? 0,
        oldAssetName: existing.oldAssetName ?? "",
        deploymentStatus: existing.newDeploymentStatus ?? "",
      };
      setFormData({
        daAssetMigrationDtoGrid: [mappedData],
      } as DaAssetMigrationDtoCreate);
      // if (existing.oldAssetName && existing.oldAssetName.trim() !== "") {
      //   setIsCurrentAssetOptional(true);
      // } else {
      //   setIsCurrentAssetOptional(false);
      // }
      if (existing.isDecommissioned === true || props.defaultDecommissioned) {
        setIsRemoveAsset(true);
      } else {
        setIsRemoveAsset(false);
      }
    } else if (props.edit && dto) {
      setFormData(dto as DaAssetMigrationDtoCreate);
      setIsRemoveAsset(!!props.defaultDecommissioned);
    } else {
      setFormData({
        daAssetMigrationDtoGrid: [defaultGridItem],
      } as DaAssetMigrationDtoCreate);
      setIsRemoveAsset(!!props.defaultDecommissioned);
      // setIsCurrentAssetOptional(false);
    }
  }, [
    props.existingData,
    dto,
    props.edit,
    props.paId,
    props.defaultDecommissioned,
    setFormData,
  ]);
  // const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   const isChecked = e.target.checked;
  //   setIsCurrentAssetOptional(isChecked);
  //   if (isChecked) {
  //     updateGrid({
  //       oldAssetName: "",
  //       networkElementAsPlannedId: 0,
  //       locationId: 0,
  //       location: "",
  //       newEnvironment: "",
  //       newEnvironmentId: 0,
  //     });
  //   }
  // };

  const locationOptions = Object.entries(props.locationResource).map(
    ([key, value]) => ({ value: Number(key), label: value })
  );

  const environmentOptions = Object.entries(props.environmentResource).map(
    ([key, value]) => ({ value: Number(key), label: value })
  );

  const targetDesignComponentOptions = props.targetDesignComponentResource.map(
    (item) => ({
      value: item.key,
      label: item.value,
    })
  );

  const deploymentStatusOptions = Object.entries(
    props.deploymentStatusResource
  ).map(([_, res]) => ({
    value: res.deploymentStatusId,
    label: res.deploymentStatusDescription,
  }));

  const firstGrid: DaAssetMigrationDto =
    formData?.daAssetMigrationDtoGrid?.[0] || defaultGridItem;
  const selectedOldAsset =
    props.existingAssetResource.find(
      (x) => x.key === firstGrid.networkElementAsPlannedId
    ) || null;

  const selectedLocation =
    locationOptions.find(
      (opt) => Number(opt.value) === Number(firstGrid.locationId)
    ) || null;

  const selectedEnvironment =
    environmentOptions.find(
      (opt) => Number(opt.value) === Number(firstGrid.newEnvironmentId)
    ) || null;

  const selectedTargetDesignComponent =
    targetDesignComponentOptions.find(
      (opt) => opt.value == firstGrid.targetDesignComponenetId
    ) || null;

  const selectedDeploymentStatus =
    (deploymentStatusOptions.find(
      (opt) => opt.value === firstGrid.deploymentStatusId
    ) ||
      deploymentStatusOptions.find(
        (opt) => opt.label === firstGrid.deploymentStatus
      )) ??
    null;

  const checkDuplicateName = (
    name: string
  ): { isDuplicate: boolean; source?: string } => {
    if (!name || name.trim() === "") {
      return { isDuplicate: false };
    }

    const normalizedInput = name.trim().toLowerCase();

    if (props.existingData && props.existingData.newelEmentName) {
      const existingName = props.existingData.newelEmentName
        .trim()
        .toLowerCase();
      if (existingName === normalizedInput) {
        return { isDuplicate: false };
      }
    }

    if (props.existingNewElementNames) {
      const isDuplicateInApi = props.existingNewElementNames.some(
        (existingName) => {
          if (!existingName) return false;
          return existingName.trim().toLowerCase() === normalizedInput;
        }
      );

      if (isDuplicateInApi) {
        return { isDuplicate: true, source: "saved assets" };
      }
    }

    if (props.localAssets && props.localAssets.length > 0) {
      const isDuplicateInLocal = props.localAssets.some((localAsset) => {
        if (
          props.existingData &&
          localAsset.daAssetMigrationId ===
            props.existingData.daAssetMigrationId
        ) {
          return false;
        }

        if (
          !localAsset.newelEmentName ||
          localAsset.newelEmentName.trim() === ""
        ) {
          return false;
        }

        return (
          localAsset.newelEmentName.trim().toLowerCase() === normalizedInput
        );
      });

      if (isDuplicateInLocal) {
        return { isDuplicate: true, source: "unsaved changes" };
      }
    }

    return { isDuplicate: false };
  };

  const updateGrid = (updates: Partial<DaAssetMigrationDto>) => {
    setFormData((prev) => {
      const currentGrid = prev?.daAssetMigrationDtoGrid || [defaultGridItem];
      const newGrid = [...currentGrid];
      newGrid[0] = { ...newGrid[0], ...updates };
      return {
        ...(prev as DaAssetMigrationDtoCreate),
        daAssetMigrationDtoGrid: newGrid,
      };
    });
    setChanged(true);
  };

  const handleOldAssetNameChange = (option: any) => {
    const locationValue = locationOptions?.filter(
      (res) => res.value === option?.locationId
    )[0]?.label;
    const envValue = environmentOptions?.filter(
      (res) => res.value === option?.environmentId
    )[0]?.label;
    updateGrid({
      oldAssetName: option?.value || "",
      networkElementAsPlannedId: option?.key ?? null,
      locationId: option?.locationId || 0,
      location: locationValue ?? "",
      newEnvironment: envValue ?? "",
      newEnvironmentId: option?.environmentId || 0,
    });
  };
  const getNewDateFormat = (value: any): Date | null => {
    if (!value) return null;

    if (value instanceof Date) {
      return value;
    }

    if (typeof value === "string" && value.match(/^\d{4}-\d{2}-\d{2}$/)) {
      return new Date(`${value}T00:00:00`);
    }

    const parsedDate = new Date(value);

    if (isNaN(parsedDate.getTime())) {
      console.warn("Could not parse date:", value);
      return null;
    }

    return parsedDate;
  };
  const handleLocationChange = (option: any) => {
    updateGrid({
      locationId: option?.value || 0,
      location: option?.label || "",
    });
  };

  const handleEnvironmentChange = (option: any) => {
    updateGrid({
      newEnvironmentId: option?.value || 0,
      newEnvironment: option?.label || "",
    });
  };

  const handleTargetDesignComponentChange = (option: any) => {
    updateGrid({
      targetDesignComponenetId: option?.value || 0,
      targetDesignComponenet: option?.label || "",
    });
  };

  const handleDeploymentStatusChange = (option: any) => {
    updateGrid({
      deploymentStatusId: option?.value || 0,
      deploymentStatus: option?.label || "",
    });
  };

  const handleDateChange = (field: string, date: Date | null) => {
    updateGrid({
      [field]: date
        ? `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
            2,
            "0"
          )}-${String(date.getDate()).padStart(2, "0")}`
        : "",
    });
  };

  const handleInputChange = (field: string, value: string) => {
    if (field === "newelEmentName") {
      updateGrid({ [field]: value });

      if (value === "") {
        updateGrid({
          deploymentStatus: "",
          deploymentStatusId: 0,
          targetDesignComponenet: "",
          targetDesignComponenetId: 0,
        });
        setDuplicateError("");
      } else {
        const { isDuplicate, source } = checkDuplicateName(value);
        if (isDuplicate) {
          const errorMessage =
            source === "unsaved changes"
              ? `"${value}" already exists in your unsaved changes. Please use a different name.`
              : `"${value}" already exists in saved assets. Please use a different name.`;
          setDuplicateError(errorMessage);
        } else {
          setDuplicateError("");
        }
      }
    } else {
      updateGrid({ [field]: value });
    }
  };

  const validate = (copy: DaAssetMigrationDtoCreate) => {
    const result: CommonValidation = { response: true, property: [] };
    const addInvalid = (field: string) => {
      result.response = false;
      result.property.push(field);
    };

    const addObj = copy.daAssetMigrationDtoGrid?.[0];
    if (!addObj) addInvalid("daAssetMigrationDtoGrid");
    else {
      if (
        !isRemoveAsset &&
        // !isCurrentAssetOptional &&
        (addObj.networkElementAsPlannedId === null ||
          addObj.networkElementAsPlannedId === 0)
      )
        addInvalid("oldAssetName");

      if (duplicateError) {
        addInvalid("newelEmentName");
      }

      if (
        addObj.newelEmentName !== "" &&
        (addObj.deploymentStatusId === null || addObj.deploymentStatusId === 0)
      )
        addInvalid("deploymentStatus");
      if (
        addObj.newelEmentName !== "" &&
        addObj.deploymentStatus?.toLowerCase() !== "planned" &&
        addObj.deploymentStatusId !== null &&
        addObj.deploymentStatusId !== 0 &&
        (addObj.targetDesignComponenetId === null ||
          addObj.targetDesignComponenetId === 0)
      )
        addInvalid("targetDesignComponenet");
      if (
        addObj.newelEmentName !== "" &&
        addObj.deploymentStatus?.toLowerCase() !== "planned" &&
        addObj.targetDesignComponenetId !== null &&
        addObj.targetDesignComponenetId !== 0 &&
        (addObj.newEnvironmentId === null || addObj.newEnvironmentId === 0)
      )
        addInvalid("newEnvironment");

      if (
        addObj.targetDesignComponenetId !== null &&
        addObj.targetDesignComponenetId !== 0 &&
        (addObj.locationId === null || addObj.locationId === 0)
      ) {
        addInvalid("location");
      }

      if (
        addObj.targetDesignComponenetId !== null &&
        addObj.targetDesignComponenetId !== 0 &&
        (addObj.newEnvironmentId === null || addObj.newEnvironmentId === 0)
      ) {
        addInvalid("newEnvironment");
      }

      if (
        isRemoveAsset &&
        (!addObj.migrationCompletionDate ||
          addObj.migrationCompletionDate.trim() === "")
      ) {
        addInvalid("migrationCompletionDate");
      }
    }

    setValidation(result);
    return result;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData) return;

    const gridItem = formData.daAssetMigrationDtoGrid?.[0] || {};

    if (gridItem.newelEmentName && gridItem.newelEmentName.trim() !== "") {
      const { isDuplicate, source } = checkDuplicateName(
        gridItem.newelEmentName
      );
      if (isDuplicate) {
        const errorMessage =
          source === "unsaved changes"
            ? `"${gridItem.newelEmentName}" already exists in your unsaved changes. Please use a different name.`
            : `"${gridItem.newelEmentName}" already exists in saved assets. Please use a different name.`;
        setDuplicateError(errorMessage);

        const input = document.querySelector(
          'input[name="newelEmentName"]'
        ) as HTMLInputElement;
        if (input) {
          input.focus();
          input.select();
        }
        return;
      }
    }

    const assetData: DaAssetMigrationDto = {
      daAssetMigrationId:
        gridItem.daAssetMigrationId ||
        (props.existingData?.daAssetMigrationId ?? 0),
      plannedActivityId: props.paId ?? 0,
      networkElementAsPlannedId:
        gridItem.networkElementAsPlannedId === 0
          ? selectedOldAsset?.key ?? 0
          : gridItem.networkElementAsPlannedId,
      oldAssetName: gridItem.oldAssetName ?? "",
      newelEmentName: gridItem.newelEmentName ?? "",
      locationId: gridItem.locationId ?? 0,
      newEnvironmentId: gridItem.newEnvironmentId ?? 0,
      targetDesignComponenetId: gridItem.targetDesignComponenetId ?? 0,
      rfoDate: gridItem.rfoDate ?? "",
      rfsDate: gridItem.rfsDate ?? "",
      rfaDate: gridItem.rfaDate ?? "",
      hwPoRaisedDate: gridItem.hwPoRaisedDate ?? "",
      hwPoArrivedDate: gridItem.hwPoArrivedDate ?? "",
      bomSubmittedDate: gridItem.bomSubmittedDate ?? "",
      migrationCompletionDate: gridItem.migrationCompletionDate ?? "",
      trafficNodePercentage: gridItem.trafficNodePercentage ?? "",
      vecDate: gridItem.vecDate ?? "",
      startOfAppIntegration: gridItem.startOfAppIntegration ?? "",
      migrationStart: gridItem.migrationStart ?? "",
      deleted: false,
      orphan: false,
      lastModified: new Date().toISOString(),
      lastModifiedBy: "",
      targetDesignComponenet: gridItem.targetDesignComponenet ?? "",
      newEnvironment: gridItem.newEnvironment ?? "",
      deploymentStatusId: gridItem.deploymentStatusId ?? 0,
      deploymentStatus: gridItem.deploymentStatus ?? "",
      newDeploymentStatusId: gridItem.deploymentStatusId ?? 0,
      newDeploymentStatus: gridItem.deploymentStatus ?? "",
      opcoId: props.opcoId ?? 0,
      opcoDesc: "",
      location: gridItem.location ?? "",
      oldEnvironment: props.existingData?.oldEnvironment ?? "",
      oldDeploymentType: props.existingData?.oldDeploymentType ?? "",
      oldDeploymentStatus: props.existingData?.oldDeploymentStatus ?? "",
      isDecommissioned: isRemoveAsset,
    };

    const validationResult = validate(formData);
    if (!validationResult.response) {
      console.error("Validation failed:", validationResult.property);
      return;
    }

    if (props.action.onSubmit) {
      props.action.onSubmit(assetData);
    } else {
      props.action.closeModal(true);
    }
  };

  const stripHtml = (html) => {
    const temp = document.createElement("div");
    temp.innerHTML = html;
    return temp.textContent || temp.innerText || "";
  };

  return (
    <div className="col-12">
      <form
        id="formAssets"
        onChange={() => setChanged(true)}
        onSubmit={handleSubmit}
      >
        <div className="row col-12 pr-0">
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Current Asset Name
                {/* <span>
                  {!isCurrentAssetOptional && <span className="red">*</span>}
                </span> */}
              </span>
              <div
                style={{
                  display: "flex",
                  alignItems: "flex-start",
                  gap: "8px",
                }}
              >
                <div style={{ flex: 1 }}>
                  <Select
                    menuPosition="fixed"
                    isDisabled={props.formDisabled || isRemoveAsset}
                    options={props.existingAssetResource}
                    value={selectedOldAsset}
                    onChange={handleOldAssetNameChange}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>

                <div
                  style={{
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "flex-start",
                    gap: "6px",
                    minWidth: "170px",
                  }}
                >
                  {props.edit &&
                    (!firstGrid.newelEmentName ||
                      firstGrid.newelEmentName.trim() === "") && (
                      <label
                        style={{
                          display: "flex",
                          alignItems: "center",
                          whiteSpace: "nowrap",
                          fontWeight: "normal",
                          fontSize: "13px",
                          cursor: "pointer",
                          marginBottom: 0,
                        }}
                        title="Mark this as to be decommissioned"
                      >
                        <input
                          type="checkbox"
                          checked={isRemoveAsset}
                          onChange={(e) => {
                            setIsRemoveAsset(e.target.checked);
                          }}
                          disabled={props.formDisabled}
                          style={{ marginRight: "5px", cursor: "pointer" }}
                        />
                        To Be Decommissioned
                      </label>
                    )}
                </div>
              </div>
              {validation?.response === false &&
                validation.property?.includes("oldAssetName") && (
                  <label className="validation">
                    *Current Asset Name must have a value
                  </label>
                )}
            </label>
          </div>

          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">New Asset Name</span>
              <input
                className={`form-control ${duplicateError ? "is-invalid" : ""}`}
                type="text"
                name="newelEmentName"
                disabled={isRemoveAsset}
                value={firstGrid.newelEmentName ?? ""}
                onChange={(e) =>
                  handleInputChange("newelEmentName", e.target.value)
                }
                placeholder="Enter unique asset name"
              />
              {duplicateError && (
                <div className="validation text-danger mt-1">
                  {duplicateError}
                </div>
              )}
              {validation?.response === false &&
                validation.property?.includes("newelEmentName") &&
                !duplicateError && (
                  <label className="validation">
                    *New Asset Name is required.
                  </label>
                )}
            </label>
          </div>

          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Deployment Status{" "}
                {firstGrid.newelEmentName !== "" && (
                  <span className="red">*</span>
                )}
              </span>
              <Select
                menuPosition="fixed"
                isDisabled={
                  isRemoveAsset ||
                  firstGrid.newelEmentName === "" ||
                  firstGrid.newelEmentName === undefined ||
                  firstGrid.newelEmentName === null
                    ? true
                    : false
                }
                options={deploymentStatusOptions}
                value={selectedDeploymentStatus}
                onChange={handleDeploymentStatusChange}
                isSearchable
                isClearable
                getOptionLabel={(option) => option.label}
                getOptionValue={(option) => option.value.toString()}
              />
              {validation?.response === false &&
                validation.property?.includes("deploymentStatus") && (
                  <label className="validation">
                    *Deployment Status is required.
                  </label>
                )}
            </label>
          </div>

          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Target Design Component{" "}
                {firstGrid.newelEmentName !== "" &&
                  selectedDeploymentStatus &&
                  selectedDeploymentStatus?.label?.toLowerCase() !==
                    "planned" && <span className="red">*</span>}
              </span>
              <Select
                menuPosition="fixed"
                isDisabled={
                  isRemoveAsset ||
                  firstGrid.newelEmentName === "" ||
                  firstGrid.newelEmentName === undefined ||
                  firstGrid.newelEmentName === null
                }
                options={targetDesignComponentOptions}
                value={selectedTargetDesignComponent}
                onChange={handleTargetDesignComponentChange}
                isSearchable
                isClearable
                getOptionLabel={(option) => stripHtml(option.label)}
                getOptionValue={(option) => option.value.toString()}
              />
              {validation?.response === false &&
                validation.property?.includes("targetDesignComponenet") && (
                  <label className="validation">
                    *Target Design Component is required.
                  </label>
                )}
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">BOM Submitted</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.bomSubmittedDate)}
                onChange={(date) => handleDateChange("bomSubmittedDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">HW PO Raised</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.hwPoRaisedDate)}
                onChange={(date) => handleDateChange("hwPoRaisedDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">HW PO Arrived</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.hwPoArrivedDate)}
                onChange={(date) => handleDateChange("hwPoArrivedDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                VEC infra ready /CNIS -Workload cluster config
              </span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.vecDate)}
                onChange={(date) => handleDateChange("vecDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">RFO Date</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.rfoDate)}
                onChange={(date) => handleDateChange("rfoDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">Start of app Integration</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.startOfAppIntegration)}
                onChange={(date) =>
                  handleDateChange("startOfAppIntegration", date)
                }
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">RFA Date</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.rfaDate)}
                onChange={(date) => handleDateChange("rfaDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">RFS Date</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.rfsDate)}
                onChange={(date) => handleDateChange("rfsDate", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">Migration Start</span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.migrationStart)}
                onChange={(date) => handleDateChange("migrationStart", date)}
                disabled={isRemoveAsset}
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
            </label>
          </div>

          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Migration Completion Date
                {isRemoveAsset && <span className="red">*</span>}
              </span>
              <DatePicker
                selected={getNewDateFormat(firstGrid.migrationCompletionDate)}
                onChange={(date) =>
                  handleDateChange("migrationCompletionDate", date)
                }
                className="form-control"
                dateFormat="dd-MM-yyyy"
                placeholderText="Select Date"
                portalId="datepicker-portal"
                shouldCloseOnSelect={true}
                showYearDropdown
                showMonthDropdown
                openToDate={new Date()}
              />
              {validation?.response === false &&
                validation.property?.includes("migrationCompletionDate") && (
                  <label className="validation">
                    *Migration Completion Date is required.
                  </label>
                )}
            </label>
          </div>

          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">Traffic Node Percentage</span>
              <input
                type="number"
                value={firstGrid.trafficNodePercentage ?? ""}
                className="form-control"
                onChange={(e) =>
                  handleInputChange("trafficNodePercentage", e.target.value)
                }
                disabled={isRemoveAsset}
              />
            </label>
          </div>

          <div className="col-6 pr-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Location
                {selectedTargetDesignComponent && (
                  <span className="red">*</span>
                )}
              </span>
              <Select
                menuPosition="fixed"
                isDisabled={isRemoveAsset}
                options={locationOptions}
                value={selectedLocation}
                onChange={handleLocationChange}
                isSearchable
                isClearable
                getOptionLabel={(option) => option.label}
                getOptionValue={(option) => option.value.toString()}
              />
              {validation?.response === false &&
                validation.property?.includes("location") && (
                  <label className="validation">*Location is required</label>
                )}
            </label>
          </div>
          <div className="col-6 pl-0">
            <label className="labelForm voda-bold w-100 field-block">
              <span className="field-label-text">
                Environment{" "}
                {selectedTargetDesignComponent && (
                  <span className="red">*</span>
                )}
              </span>
              <Select
                menuPosition="fixed"
                isDisabled={isRemoveAsset || !selectedTargetDesignComponent}
                options={environmentOptions}
                value={selectedEnvironment}
                onChange={handleEnvironmentChange}
                isSearchable
                isClearable
                getOptionLabel={(option) => option.label}
                getOptionValue={(option) => option.value.toString()}
              />
              {validation?.response === false &&
                validation.property?.includes("newEnvironment") && (
                  <label className="validation">
                    *Environment Status is required.
                  </label>
                )}
            </label>
          </div>
        </div>

        <div className="col-12 justify-content-end d-flex footerModal">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => props.action.closeModal(false)}
            type="button"
          >
            {" "}
            Cancel{" "}
          </button>

          <button
            className="voda-bold btn btn-danger px-4 btnHeader"
            type="submit"
            disabled={!!duplicateError}
          >
            {props.edit ? "Update Asset" : "Create Asset"}
          </button>
        </div>
        <div id="datepicker-portal"></div>
      </form>
      <style>{`
        .field-block {
          display: flex;
          flex-direction: column;
        }
        .field-label-text {
          display: flex;
          align-items: flex-start;
          min-height: 40px;
          line-height: 1.2;
        }
      `}</style>
    </div>
  );
};

export default AssetsDetailsForm;
