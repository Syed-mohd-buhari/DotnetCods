import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { GeneralSettingsApi } from "../../../Business/GeneralSettingsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_GENERAL_SETTINGS,
  GET_EDIT_GENERAL_SETTINGS,
  GeneralSettingsDtoUpdate,
  GeneralSettingsEdit,
} from "../../../Model/GeneralSettingsModal";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetGeneralSettingsEditResource(id: number) {
  setLoader("ADD", "GetGeneralSettingsEditResource");

  const api = new GeneralSettingsApi();
  const editResource = await ApiCallWithErrorHandling<Promise<GeneralSettingsDtoUpdate>>(
    () => api.generalSettingsGetSingle(id)
  );

  const rtn = { GeneralSettingsDtoEdit: editResource } as GeneralSettingsEdit;
  rootStore.dispatch({ type: GET_EDIT_GENERAL_SETTINGS, payload: rtn });

  setLoader("REMOVE", "GetGeneralSettingsEditResource");

  return rtn;
}

export async function EditGeneralSettings(
  data: GeneralSettingsDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditGeneralSettings");

  const api = new GeneralSettingsApi();
  const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.generalSettingsUpdate(data)
  );

  const rtn = { ResultDtoEdit: result } as GeneralSettingsEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "General setting updated.",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_GENERAL_SETTINGS, payload: rtn });

  setLoader("REMOVE", "EditGeneralSettings");

  return rtn;
}
