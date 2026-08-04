import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PracticeApi } from "../../../../Business/LookUp/PracticeBusiness";
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

export async function GetPracticeCreateResource() {
  setLoader("ADD", "GetPracticeCreateResource");

  let api = new PracticeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.practiceGetCreateResourcePractice());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_PRACTICE", payload: rtn });
  setLoader("REMOVE", "GetPracticeCreateResource");
}

export async function CreatPractice(data: TipologicaGridDto) {
  setLoader("ADD", "CreatPractice");
  let api = new PracticeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.practiceCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_PRACTICE", payload: rtn });
  setLoader("REMOVE", "CreatPractice");
  return rtn;
}
