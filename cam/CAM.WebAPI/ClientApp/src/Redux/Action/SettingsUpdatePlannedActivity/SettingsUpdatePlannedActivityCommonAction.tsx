import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SettingsUpdatePlannedActivityApi } from "../../../Business/SettingsUpdatePlannedActivityBusiness";
import { ChangeGridOrderDto, ResultDto } from "../../../Model/CommonModels";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function ChangeGridSettingsUpdatePlannedActivity(
  data: Array<ChangeGridOrderDto>
) {
  setLoader("ADD", "ChangeGridSettingsUpdatePlannedActivity");

  let api = new SettingsUpdatePlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.settingsUpdatePlannedActivityChangeGridOrderSettingsUpdatePlannedActivity(
      data
    )
  );
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  setLoader("REMOVE", "ChangeGridSettingsUpdatePlannedActivity");

  return result;
}

export async function GetPATypeAndDeploymentStatus(
  plannedActivityTypeFor?: number
) {
  setLoader("ADD", "ChangeGridSettingsUpdatePlannedActivity");

  let api = new SettingsUpdatePlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.settingsUpdatePlannedActivityGetPATypeAndDeploymentStatus(
      plannedActivityTypeFor
    )
  );

  setLoader("REMOVE", "ChangeGridSettingsUpdatePlannedActivity");

  return result;
}

export async function GetCrossSettings(
  plannedActivityFor: number,
  plannedActivityTypeId: number,
  currentDeliveryStatus: number
) {
  setLoader("ADD", "GetCrossSettings");

  let api = new SettingsUpdatePlannedActivityApi();
  let result = await ApiCallWithErrorHandling<
    Promise<{ [key: string]: string }>
  >(() =>
    api.settingsUpdatePlannedActivityGetCrossSettings(
      plannedActivityFor,
      plannedActivityTypeId,
      currentDeliveryStatus
    )
  );

  setLoader("REMOVE", "GetCrossSettings");

  return result;
}
