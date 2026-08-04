import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemFunctionApi } from "../../../../Business/LookUp/SystemFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSystemFunctionEditResource(id: number) {
  setLoader("ADD", "GetSystemFunctionEditResource");

  // const dispach = useDispatch();
  let api = new SystemFunctionApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.systemFunctionGetUpdateResourceSystemFunction(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SYSTEM_FUNCTION", payload: rtn });
  setLoader("REMOVE", "GetSystemFunctionEditResource");

  return rtn;
}

export async function EditSystemFunction(data: TipologicaGridDto) {
  let api = new SystemFunctionApi();
  setLoader("ADD", "EditSystemFunction");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemFunctionPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SYSTEM_FUNCTION", payload: rtn });
  setLoader("REMOVE", "EditSystemFunction");
  return rtn;
}
