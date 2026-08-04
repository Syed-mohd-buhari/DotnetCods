import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  GetUpdateUpdatePlannedActivityStatus,
  SaveUpdatePlannedActivityStatus,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import {
  dictionaryToArrayCrossSettingsDto,
  dictionaryToArraySettingsUpdatePlannedActivityDto,
} from "../../Hook/Dictionary";
import {
  DaPlannedActivityDcfDto,
  UpdatePlannedActivityStatusDto,
} from "../../Model/PlannedActivity";

interface Props {
  PAID: number;
  plannedActivitypeForId: number;
  action: {
    setIsVisibleModalStatus(val: boolean): any;
    refresh(): any;
  };
}

const UpdateServiceLevelPAStatusModal = (props: Props) => {
  const [isLatestSelected, setIsLatestSelected] = useState(false);
  const [dcfTableData, setDcfTableData] = useState<DaPlannedActivityDcfDto[]>(
    []
  );
  const [formData, setFormData] = useState<UpdatePlannedActivityStatusDto>();
  const [settingId, setSettingId] = useState<number>();
  const [showLatestProgress, setShowLatestProgress] = useState<boolean>(false);
  const [settingDescription, setSettingDescription] = useState<string>();

  useEffect(() => {
    if (props?.PAID !== undefined && props?.PAID !== null) {
      GetEditResource(props.PAID, props.plannedActivitypeForId, true);
    }
  }, [props?.PAID]);

  const GetEditResource = async (
    PAID: number,
    plannedActivityypeForId: number,
    loader
  ) => {
    await GetUpdateUpdatePlannedActivityStatus(
      PAID,
      plannedActivityypeForId!
    ).then((x) => {
      if (x && x.data !== undefined) {
        //check if Dc Id to updated for the unknown DC
        const resource = x.data.settingsUpdatePlannedActivityResource ?? {};
        const selectedId = x.data.settingsUpdatePlannedActivityId;
        const allItems =
          dictionaryToArraySettingsUpdatePlannedActivityDto(resource);
        const selected = allItems.find((item) => item.key === selectedId);
        const maxOrder = Math.max(
          ...allItems.map((item) => item.value.order ?? 0)
        );
        const isLatest = selected?.value?.order === maxOrder;
        setIsLatestSelected(isLatest);

        setDcfTableData(x.data.daPlannedActivtyDcfDto || []);

        let checkDC =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.specifyDC;
        let checkReleaseFlag =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.isReleaseDetailsUnknown;
        let checkPAReleaseFlag =
          x?.data?.settingsUpdatePlannedActivityResource?.[
            x?.data?.settingsUpdatePlannedActivityId
          ]?.isPAReleaseDetailsUnknown;

        if (props?.PAID) {
          setFormData({ ...x?.data });
          // setIsReleaseDcResource(x?.data?.designComponentResource);
        } else {
          const copy = { ...formData } as UpdatePlannedActivityStatusDto;
          copy.settingsUpdatePlannedActivityResource =
            x?.data?.settingsUpdatePlannedActivityResource;
          copy.settingsUpdatePlannedActivityId =
            x?.data?.settingsUpdatePlannedActivityId;
          copy.crossSettingscResource = x?.data?.crossSettingscResource;
          copy.isCrossSettings = x?.data?.isCrossSettings;
          copy.numberOfNodesInLabInput = x?.data?.numberOfNodesInLabInput;
          copy.numberOfNodesInput = x?.data?.numberOfNodesInput;
          copy.elementCount = x?.data?.elementCount;
          copy.ruleElementCount = x?.data?.ruleElementCount;
          copy.startNodesInLab = x?.data?.startNodesInLab;
          copy.startNodesInProd = x?.data?.startNodesInProd;
          // setIsReleaseDcResource(x?.data?.designComponentResource);
          setFormData(copy);
        }
        setSettingId(x.data.settingsUpdatePlannedActivityId);
        setShowLatestProgress(true);
      } else {
        setSettingId(undefined);
      }
    });
  };
  const onChangeSettings = (e: any, ruleElementCount: number) => {
    let copy = { ...formData } as UpdatePlannedActivityStatusDto;
    let val = e;

    copy.settingsUpdatePlannedActivityId = val;
    copy.ruleElementCount = ruleElementCount;

    if (ruleElementCount === 1) {
      copy.endNodesInProd = [];
    }

    let crossIds =
      formData &&
      formData.crossSettingscResource &&
      dictionaryToArrayCrossSettingsDto(formData.crossSettingscResource);

    let target = crossIds?.find(
      (x) => x.value.input == settingId && x.value.output == val
    );

    if (target !== null && target !== undefined) {
      copy.isCrossSettings = true;
    } else {
      copy.isCrossSettings = false;
    }

    setFormData(copy);
    //Rimuovi Validazione
    // if (validation?.property?.includes("settingsUpdatePlannedActivityId")) {
    //   let copy = { ...validation, property: [...validation.property] };
    //   let idxOfProperty = copy.property.indexOf(
    //     "settingsUpdatePlannedActivityId"
    //   );
    //   copy.property.splice(idxOfProperty, 1);
    //   setValidation(copy);
    // }
  };

  const handleDcfStatusChange = (index: number, selectedStatusId: string) => {
    const copy = { ...formData } as UpdatePlannedActivityStatusDto;

    if (copy.servicePlanDcfDto) {
      copy.servicePlanDcfDto[index] = {
        ...copy.servicePlanDcfDto[index],
        status: selectedStatusId,
      };
    }

    setFormData(copy);
  };

  const stripHtmlTags = (htmlString: string) => {
    if (!htmlString) return "";
    return htmlString.replace(/<[^>]*>/g, "");
  };
  const handleConfirmDialog = async () => {
    if (!formData) return;

    try {
      const payload: UpdatePlannedActivityStatusDto = {
        ...formData,
        plannedActivityTypeFor: formData.ruleLinkedDc,
        servicePlanDcfDto:
          formData.servicePlanDcfDto?.map((item) => ({
            servicePlanId: item.servicePlanId,
            dcfId: item.dcfId,
            status: item.status,
            servicePlanDcfMappingId: item.servicePlanDcfMappingId,
            plannedActivityId: item.plannedActivityId,
          })) ?? [],
      };

      const response = await SaveUpdatePlannedActivityStatus(payload);

      if (response && !response.warning) {
        props.action.refresh();
        props.action.setIsVisibleModalStatus(false);
        console.log("Saved successfully");
      }
    } catch (error) {
      console.error("Save failed", error);
    }
  };
  return (
    <div className="col-12 d-flex justify-content-center row mx-0">
      <div className="form-group col-12 mt-2 ">
        <label className="text-bb">
          Please select the latest progress
          <span className="red">*</span>
        </label>
        <label className="labelForm voda-bold w-100">
          <div className="d-flex pt-1 row mx-0 ">
            {formData?.settingsUpdatePlannedActivityResource &&
              dictionaryToArraySettingsUpdatePlannedActivityDto(
                formData?.settingsUpdatePlannedActivityResource
              )
                .sort((a, b) => a.value.order! - b.value.order!)
                .map((x) => (
                  <div
                    className={`border col-md d-flex row mx-0 align-content-start justify-content-center text-center py-3 statusRadio`}
                    key={x.key}
                  >
                    <input
                      type="radio"
                      name={"delivery" + x?.key}
                      // disabled={rtnStatusClass(x.key)}
                      value={x.key}
                      onChange={(e) => {
                        const selectedKey = e.target.value;
                        const selectedOrder = x.value.order;

                        setSettingDescription(
                          x?.value?.settingsUpdatePlannedActivityDescription
                        );
                        const allItems =
                          dictionaryToArraySettingsUpdatePlannedActivityDto(
                            formData?.settingsUpdatePlannedActivityResource ??
                              {}
                          );

                        const maxOrder = Math.max(
                          ...allItems.map((item) => item.value.order ?? 0)
                        );

                        const isLatest = selectedOrder === maxOrder;
                        setIsLatestSelected(isLatest);

                        onChangeSettings(
                          selectedKey,
                          x.value.ruleElementCount ?? 0
                        );
                      }}
                      checked={
                        formData.settingsUpdatePlannedActivityId == x.key
                      }
                    />
                    <label className="mt-2 mb-0 w-100">
                      {x.value?.settingsUpdatePlannedActivityDescription}
                    </label>
                  </div>
                ))}
          </div>
        </label>
      </div>
      {formData?.servicePlanDcfDto && formData.servicePlanDcfDto.length > 0 && (
        <div className="form-group col-12 mt-3">
          <label className="text-bb">
            Please Update DCF Status
            <span className="red">*</span>
          </label>
          <table
            className="w-100 table table-responsive"
            style={{
              minHeight: "15rem",
              maxWidth: "50rem",
            }}
          >
            <thead>
              <tr className="intestazione">
                <th>
                  <div
                    className="h-100 d-flex align-items-center justify-content-center divFilter"
                    style={{ width: "18rem" }}
                  >
                    <label>DCF</label>
                  </div>
                </th>

                <th>
                  <div
                    className="h-100 d-flex align-items-center justify-content-center divFilter"
                    style={{ width: "18rem" }}
                  >
                    <label>Status</label>
                  </div>
                </th>
              </tr>
            </thead>

            <tbody>
              {formData.servicePlanDcfDto.map((item, index) => (
                <tr
                  className="dati"
                  key={item.servicePlanId}
                  style={{ background: "white" }}
                >
                  <td style={{ padding: "8px 10px" }}>
                    <input
                      className="form-control"
                      type="text"
                      value={stripHtmlTags(item.dcfName) || ""}
                      disabled
                    />
                  </td>

                  <td style={{ padding: "8px 10px" }}>
                    <select
                      className="form-control"
                      value={item.status || ""}
                      onChange={(e) =>
                        handleDcfStatusChange(index, e.target.value)
                      }
                    >
                      {item.dcfStatus &&
                        Object.entries(item.dcfStatus).map(([key, value]) => (
                          <option key={key} value={key}>
                            {String(value)}
                          </option>
                        ))}
                    </select>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
      <div className="col-12 justify-content-end d-flex mb-3">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => {
            props.action.setIsVisibleModalStatus(false);
          }}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          type="button"
          onClick={() => handleConfirmDialog()}
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default UpdateServiceLevelPAStatusModal;
