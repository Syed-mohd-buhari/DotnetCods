import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PodTypeInfoApi } from "../../../../Business/LookUp/PodTypeInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  PodTypeInfoCreate,
  PodTypeInfoDto,
} from "../../../../Model/LookUp/PodTypeInfo";

export async function GetPodTypeInfoCreateResource() {
  setLoader("ADD", "GetPodTypeInfoCreateResource");

  let api = new PodTypeInfoApi();
  let createResource = await ApiCallWithErrorHandling<Promise<PodTypeInfoDto>>(
    () => api.PodTypeInfoGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as PodTypeInfoCreate;
  rootStore.dispatch({ type: "GET_CREATE_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "GetPodTypeInfoCreateResource");
}

export async function CreatPodTypeInfo(data: PodTypeInfoDto) {
  setLoader("ADD", "CreatPodTypeInfo");
  let api = new PodTypeInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.PodTypeInfoCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as PodTypeInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_PODTYPEINFO", payload: rtn });
  setLoader("REMOVE", "CreatPodTypeInfo");
  return rtn;
}
