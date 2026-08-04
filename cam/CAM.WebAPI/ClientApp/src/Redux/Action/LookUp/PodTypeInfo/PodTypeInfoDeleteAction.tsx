import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PodTypeInfoApi } from "../../../../Business/LookUp/PodTypeInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeletePodTypeInfo(id: number) {
  setLoader("ADD", "DeletePodTypeInfo");
  let api = new PodTypeInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PodTypeInfoDelete(id)
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
  rootStore.dispatch({ type: "DELETE_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "DeletePodTypeInfo");
  return rtn;
}

export async function DeleteDeepPodTypeInfo(id: number) {
  let api = new PodTypeInfoApi();
  setLoader("ADD", "DeleteDeepPodTypeInfo");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PodTypeInfoDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "DeleteDeepPodTypeInfo");
  return rtn;
}
