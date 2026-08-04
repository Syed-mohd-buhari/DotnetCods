import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PracticeApi } from "../../../../Business/LookUp/PracticeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPracticeEditResource(id: number) {
  setLoader("ADD", "GetPracticeEditResource");

  let api = new PracticeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.practiceGetUpdateResourcePractice(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_PRACTICE", payload: rtn });
  setLoader("REMOVE", "GetPracticeEditResource");

  return rtn;
}

export async function EditPractice(data: TipologicaGridDto) {
  setLoader("ADD", "EditPractice");
  let api = new PracticeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.practicePut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_PRACTICE", payload: rtn });
  setLoader("REMOVE", "EditPractice");
  return rtn;
}
