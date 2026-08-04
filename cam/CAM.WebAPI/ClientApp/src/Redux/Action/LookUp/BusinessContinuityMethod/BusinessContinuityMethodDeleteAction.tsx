import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { BusinessContinuityMethodApi } from "../../../../Business/LookUp/BusinessContinuityMethodBusiness";

export async function deleteBusinessContinuityMethod(id: number) {
  setLoader("ADD", "deleteBusinessContinuityMethod");
  let api = new BusinessContinuityMethodApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.businessContinuityMethodDelete(id)
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
  rootStore.dispatch({ type: "DELETE_GEO_RESILIENCE", payload: rtn });
  setLoader("REMOVE", "deleteBusinessContinuityMethod");
  return rtn;
}

export async function DeleteDeepBusinessContinuityMethod(id: number) {
  let api = new BusinessContinuityMethodApi();
  setLoader("ADD", "DeleteDeepBusinessContinuityMethod");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.businessContinuityMethodDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_GEO_RESILIENCE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepBusinessContinuityMethod");
  return rtn;
}

export async function GetRelatedRecordsBusinessContinuityMethod(id: number) {
  let api = new BusinessContinuityMethodApi();
  setLoader("ADD", "GetRelatedRecordsBusinessContinuityMethod");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.businessContinuityMethodGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
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
  setLoader("REMOVE", "GetRelatedRecordsBusinessContinuityMethod");
  return rtn;
}
