import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProblemCategoryApi } from "../../../../Business/LookUp/ProblemCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteProblemCategory(id: number) {
  setLoader("ADD", "deleteProblemCategory");
  let api = new ProblemCategoryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.problemCategoryDelete(id)
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
  rootStore.dispatch({ type: "DELETE_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "deleteProblemCategory");
  return rtn;
}

export async function DeleteDeepProblemCategory(id: number) {
  let api = new ProblemCategoryApi();
  setLoader("ADD", "DeleteDeepProblemCategory");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.problemCategoryDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "DeleteDeepProblemCategory");
  return rtn;
}
