import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import {
  PlannedActivityForLinkDto,
  ResultDtoOfPlannedActivityForLinkDto,
  PlannedActivityMigrationsDto,
  UpdatePlannedActivityStatusDto,
  ResultDtoOfUpdatePlannedActivityStatusDto,
  PlannedActivityToConnectData,
  ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData,
  PlannedActivityConfirmationDto,
  DaMigrationStatusDtoGrid,
  PaWithDaMigrationApiResponse,
  PaWhenDaMigrationCompleteApiResponse,
} from "../../../Model/PlannedActivity";
import setLoader from "../LoaderAction";
import {
  PlannedActivityTypeForEnum,
  SettingsUpdatePlannedActivityDtoUpdate,
} from "../../../Model/SettingsUpdatePlannedActivity";

export async function GetLinkedDesignComponent(id: number, rule: number) {
  setLoader("ADD", "GetLinkedDesignComponent");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetLinkedDesignComponent(id, rule)
  );
  setLoader("REMOVE", "GetLinkedDesignComponent");

  return result?.data as Array<number>;
}
export async function GetCreateUnkownDCPALevel(id: number) {
  setLoader("ADD", "GetCreateUnkownDCPALevel");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityCreateUnkownDCPALevel(id)
  );
  setLoader("REMOVE", "GetCreateUnkownDCPALevel");

  return result?.data as Array<number>;
}
export async function GetActivityDetails(id: number, rule: number) {
  setLoader("ADD", "GetActivityDetails");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetActivityDetails(id, rule)
  );
  setLoader("REMOVE", "GetActivityDetails");

  return result?.data as string;
}

export async function ActivityStatusLogics(
  deliveryId?: number,
  budgetAvId?: number,
  responsibilityPhase?: number,
  localApproval?: string
) {
  setLoader("ADD", "ActivityStatusLogics");

  let api = new PlannedActivityApi();
  let localApprovalBool = localApproval
    ? localApproval?.toUpperCase() == "YES"
    : undefined;
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityActivityStatusLogics(
      deliveryId,
      budgetAvId,
      responsibilityPhase,
      localApprovalBool
    )
  );
  setLoader("REMOVE", "ActivityStatusLogics");

  return result?.data as number[];
}

export async function GetOpcoListForPlannedToConnect() {
  setLoader("ADD", "GetOpcoListForPlannedToConnect");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetOpCoList()
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetOpcoListForPlannedToConnect");

    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetOpcoListForPlannedToConnect");
}

export async function GetDesignComponentListForPlannedToConnect(
  opcoId: number
) {
  setLoader("ADD", "GetDesignComponentListForPlannedToConnect");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetDesignComponentList(opcoId)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetDesignComponentListForPlannedToConnect");

    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetDesignComponentListForPlannedToConnect");
}

export async function GetPlannedActivityListForPlannedToConnect(
  designComponentId: number | undefined,
  opcoId: number | undefined
) {
  setLoader("ADD", "GetPlannedActivityListForPlannedToConnect");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetPlannedActivityListForMigration(
      designComponentId,
      opcoId
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedActivityListForPlannedToConnect");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetPlannedActivityListForPlannedToConnect");
  }
}

export async function GetPlannedActivityTypeswithDcIdAndOpcoId(
  designComponentId: number | undefined,
  opcoId: number | undefined
) {
  setLoader("ADD", "GetPlannedActivityTypeswithDcIdAndOpcoId");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetPlannedActivityTypeswithDcIdAndOpcoId(
      designComponentId,
      opcoId
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedActivityTypeswithDcIdAndOpcoId");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetPlannedActivityListForPlannedToConnect");
  }
}

export async function GetplannedActivityCheckPlannedActivityTypefor(
  designComponentId?: number,
  opCoId?: number,
  plannedActivityTypeId?: number
) {
  setLoader("ADD", "GetplannedActivityCheckPlannedActivityTypefor");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityCheckPlannedActivityTypefor(
      designComponentId,
      opCoId,
      plannedActivityTypeId
    )
  );

  if (result && !result.warning) {
    setLoader("REMOVE", "GetplannedActivityCheckPlannedActivityTypefor");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetplannedActivityCheckPlannedActivityTypefor");
  }
}

export async function SubmitPlannedActivityMigration(
  data: PlannedActivityMigrationsDto
) {
  setLoader("ADD", "SubmitPlannedActivityMigration");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityPlannedActivityMigrations(data)
  );
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "SubmitPlannedActivityMigration");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "SubmitPlannedActivityMigration");
}

export async function GetPlannedActivityForLink(plannedId: number) {
  setLoader("ADD", "GetPlannedActivityForLink");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfPlannedActivityForLinkDto>
  >(() => api.plannedActivityGetPlannedActivityForLink(plannedId));
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedActivityForLink");
    return result.data as PlannedActivityForLinkDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetPlannedActivityForLink");
  }
}

export async function GetLcmEngineeringPlannedActivity(
  designComponentId?: number,
  opCoId?: number
) {
  setLoader("ADD", "GetLcmEngineeringPlannedActivity");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfDictionaryOfLongAndPlannedActivityToConnectData>
  >(() =>
    api.plannedActivityGetLcmEngineeringPlannedActivity(
      designComponentId,
      opCoId
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetLcmEngineeringPlannedActivity");

    return result.data as { [key: number]: PlannedActivityToConnectData };
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetLcmEngineeringPlannedActivity");
}

export async function GetCreateUpdatePlannedActivityStatus() {
  setLoader("ADD", "GetCreateUpdatePlannedActivityStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<
    Promise<UpdatePlannedActivityStatusDto>
  >(() => api.plannedActivityGetCreateUpdatePlannedActivityStatus());
  setLoader("REMOVE", "GetCreateUpdatePlannedActivityStatus");

  return result as UpdatePlannedActivityStatusDto;
}

export async function GetUpdateUpdatePlannedActivityStatus(
  plannedId: number,
  plannedActivityTYpeForId: number
) {
  setLoader("ADD", "GetUpdateUpdatePlannedActivityStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetUpdatePlannedActivityStatus(
      plannedId,
      plannedActivityTYpeForId
    )
  );
  setLoader("REMOVE", "GetUpdateUpdatePlannedActivityStatus");

  return result as ResultDto;
}

export async function CheckFiscalYear(data: number) {
  setLoader("ADD", "CheckFiscalYear");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
    api.plannedActivityCheckFiscalYear(data)
  );
  setLoader("REMOVE", "CheckFiscalYear");
  return result as boolean;
}

export async function SaveUpdatePlannedActivityStatus(
  data: UpdatePlannedActivityStatusDto
) {
  setLoader("ADD", "SaveUpdatePlannedActivityStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivitySaveUpdatePlannedActivityStatus(data)
  );
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );

  setLoader("REMOVE", "SaveUpdatePlannedActivityStatus");

  return result as ResultDto;
}

export async function GetConfrontoHardwareType(
  dcId: number,
  plannedDcId: number
) {
  setLoader("ADD", "SaveUpdatePlannedActivityStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
    api.plannedActivityGetConfrontoHardwareTypeFromDesignComponent(
      dcId,
      plannedDcId
    )
  );
  // rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
  setLoader("REMOVE", "SaveUpdatePlannedActivityStatus");

  return result as boolean;
}

export async function GetPlannedActivityListForUpdateStatus(
  designComponentId: number | undefined,
  opcoId: number | undefined,
  palnnedActivityTypeId: number | undefined,
  plannedActivityTypeForId: number
) {
  setLoader("ADD", "GetPlannedActivityListForUpdateStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetPlannedActivityListForUpdatePlannedActivityStatus(
      designComponentId,
      opcoId,
      palnnedActivityTypeId,
      plannedActivityTypeForId
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedActivityListForUpdateStatus");

    return result as ResultDto;
  } else {
    // rootStore.dispatch(
    //   setNotification({
    //     message: result?.info ?? "",
    //     notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    //   })
    // );
    setLoader("REMOVE", "GetPlannedActivityListForUpdateStatus");
  }
}

export async function GetPlannedActivityRelatedToDeliveryStatus(
  plannedActivityId: number,
  plannedActivityTypeFor?: PlannedActivityTypeForEnum
) {
  setLoader("ADD", "GetPlannedActivityRelatedToDeliveryStatus");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<
    Promise<{ [key: string]: string }>
  >(() =>
    api.getPlannedActivityRelatedDeliveryStatus(
      plannedActivityId,
      plannedActivityTypeFor
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedActivityRelatedToDeliveryStatus");

    return result as { [key: string]: string };
  } else {
    // rootStore.dispatch(
    //   setNotification({
    //     message: result?.info ?? "",
    //     notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    //   })
    // );
    setLoader("REMOVE", "GetPlannedActivityListForUpdateStatus");
  }
}

export async function GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
  plannedActivityResourceId?: number,
  deliveryStatusId?: number
) {
  setLoader("ADD", "GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId,
      deliveryStatusId
    )
  );
  if (result && !result.warning) {
    setLoader(
      "REMOVE",
      "GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource"
    );

    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader(
      "REMOVE",
      "GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource"
    );
  }
}

export async function SaveUpdatedAssetDetails(data: any) {
  setLoader("ADD", "SaveUpdateAssetDetails");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.saveUpdatedAssetDetails(data)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "SaveUpdateAssetDetails");

    return result as any;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "SaveUpdateAssetDetails");
  }
}

export async function GetPlannedRuleConfig(pagewiseId) {
  setLoader("ADD", "GetPlannedRuleConfig");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetPlannedRuleConfig(pagewiseId)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetPlannedRuleConfig");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetPlannedRuleConfig");
  }
}

export async function GetSettingUpdatePlannedActivityResource(
  plannedActivityTypeId: number,
  plannedActivityTypeFor: number
) {
  setLoader("ADD", "GetSettingUpdatePlannedActivityResource");

  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.plannedActivityGetSettingUpdatePlannedActivityResource(
      plannedActivityTypeId,
      plannedActivityTypeFor
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetSettingUpdatePlannedActivityResource");

    return result as any;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetSettingUpdatePlannedActivityResource");
  }
}

export async function GetplannedActivityConfirmation(
  dcfIds: number[],
  opCoId: number
) {
  setLoader("ADD", "GetCreateUnkownDCPALevel");

  const api = new PlannedActivityApi();
  const payload: PlannedActivityConfirmationDto = {
    designComponentName: "",
    dcfId: dcfIds,
    opcoId: opCoId,
    opcoName: "",
    lcmId: "",
    lcmPaId: "",
  };

  const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityConfirmation(payload)
  );

  setLoader("REMOVE", "GetCreateUnkownDCPALevel");

  return result as ResultDto;
}

export async function GetGetUpdateDaMigrationRecords(id: number) {
  let api = new PlannedActivityApi();
  setLoader("ADD", "GetUpdateDaMigrationRecords");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetUpdateDaMigrationRecords(id)
  );
  let rtn = {
    data: result,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsCustomerWheel");
  return rtn;
}

export async function SavePaWithDaMigration(
  daMigrationStatusList: PaWithDaMigrationApiResponse,
  paId: number,
  deliveryStatusId: number
) {
  setLoader("ADD", "SavePaWithDaMigration");

  const api = new PlannedActivityApi();

  // The API expects the full array of status objects, with updated statusId
  const payload = daMigrationStatusList;

  const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.updatePaWithDaMigration(payload, paId, deliveryStatusId)
  );

  setLoader("REMOVE", "SavePaWithDaMigration");

  return result as ResultDto;
}

export async function SavePaWhenDaMigrationCompleted(
  body: PaWhenDaMigrationCompleteApiResponse,
  paId: number,
  deliveryStatusId: number
) {
  setLoader("ADD", "SavePaWhenDaMigrationCompleted");
  const api = new PlannedActivityApi();

  const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.updatePaWhenDaMigrationCompleted(body, paId, deliveryStatusId)
  );

  setLoader("REMOVE", "SavePaWhenDaMigrationCompleted");

  return result as ResultDto;
}
