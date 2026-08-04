import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFFunctionStandardNameApi } from "../../../../Business/LookUp/CNFFunctionStandardNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCNFFunctionStandardName(id: number) {
  setLoader("ADD", "DeleteCNFFunctionStandardName");
  let api = new CNFFunctionStandardNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFFunctionStandardNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CNFFUNCTIONSTANDARDNAME", payload: rtn });
  setLoader("REMOVE", "DeleteCNFFunctionStandardName");
  return rtn;
}

export async function DeleteDeepCNFFunctionStandardName(id: number) {
  let api = new CNFFunctionStandardNameApi();
  setLoader("ADD", "DeleteDeepCNFFunctionStandardName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFFunctionStandardNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CNFFUNCTIONSTANDARDNAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCNFFunctionStandardName");
  return rtn;
}
