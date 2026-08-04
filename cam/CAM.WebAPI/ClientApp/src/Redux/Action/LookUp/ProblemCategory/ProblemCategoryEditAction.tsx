import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProblemCategoryApi } from "../../../../Business/LookUp/ProblemCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetProblemCategoryEditResource(id: number) {
  setLoader("ADD", "GetProblemCategoryEditResource");

  let api = new ProblemCategoryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.problemCategoryGetUpdateResourceProblemCategory(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "GetProblemCategoryEditResource");

  return rtn;
}

export async function EditProblemCategory(data: TipologicaGridDto) {
  setLoader("ADD", "EditProblemCategory");
  let api = new ProblemCategoryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.problemCategoryPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "EditProblemCategory");
  return rtn;
}
