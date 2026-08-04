import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MainOrganisationApi } from "../../../../Business/LookUp/MainOrganisationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteMainOrganisation(id: number) {
  setLoader("ADD", "deleteMainOrganisation");
  let api = new MainOrganisationApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.mainOrganisationDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "deleteMainOrganisation");
  return rtn;
}

export async function DeleteDeepMainOrganisation(id: number) {
  let api = new MainOrganisationApi();
  setLoader("ADD", "DeleteDeepMainOrganisation");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.mainOrganisationDeleteDeep(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "DeleteDeepMainOrganisation");
  return rtn;
}
