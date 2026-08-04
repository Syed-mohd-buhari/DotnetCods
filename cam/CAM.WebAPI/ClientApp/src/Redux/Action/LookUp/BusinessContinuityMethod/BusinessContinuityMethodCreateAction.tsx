import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { BusinessContinuityMethodApi } from "../../../../Business/LookUp/BusinessContinuityMethodBusiness";
// import { useDispatch } from 'react-redux'

export async function GetBusinessContinuityMethodCreateResource() {
  setLoader("ADD", "GetBusinessContinuityMethodCreateResource");

  let api = new BusinessContinuityMethodApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.businessContinuityMethodGetCreateResourceBusinessContinuityMethod());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_GEO_RESILIENCE",
    payload: rtn,
  });
  setLoader("REMOVE", "GetBusinessContinuityMethodCreateResource");
}

export async function CreatBusinessContinuityMethod(data: TipologicaGridDto) {
  let api = new BusinessContinuityMethodApi();
  setLoader("ADD", "CreatBusinessContinuityMethod");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.businessContinuityMethodCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_GEO_RESILIENCE", payload: rtn });
  setLoader("REMOVE", "CreatBusinessContinuityMethod");
  return rtn;
}
