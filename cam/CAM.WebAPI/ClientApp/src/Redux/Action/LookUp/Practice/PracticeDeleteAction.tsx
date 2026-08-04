import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PracticeApi } from "../../../../Business/LookUp/PracticeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePractice(id: number) {
  setLoader("ADD", "deletePractice");
  let api = new PracticeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.practiceDelete(id)
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
  rootStore.dispatch({ type: "DELETE_PRACTICE", payload: rtn });
  setLoader("REMOVE", "deletePractice");
  return rtn;
}

export async function DeleteDeepPractice(id: number) {
  let api = new PracticeApi();
  setLoader("ADD", "DeleteDeepPractice");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.practiceDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_PRACTICE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepPractice");
  return rtn;
}
