import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProblemCategoryApi } from "../../../../Business/LookUp/ProblemCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetProblemCategoryCreateResource() {
  setLoader("ADD", "GetProblemCategoryCreateResource");

  let api = new ProblemCategoryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.problemCategoryGetCreateResourceProblemCategory());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "GetProblemCategoryCreateResource");
}

export async function CreatProblemCategory(data: TipologicaGridDto) {
  setLoader("ADD", "CreatProblemCategory");
  let api = new ProblemCategoryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.problemCategoryCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "CreatProblemCategory");
  return rtn;
}
