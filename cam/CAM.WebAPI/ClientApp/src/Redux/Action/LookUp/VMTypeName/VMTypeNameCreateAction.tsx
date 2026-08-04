import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMTypeNameApi } from "../../../../Business/LookUp/VMTypeNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  VMTypeNameCreate,
  VMTypeNameDto,
} from "../../../../Model/LookUp/VMTypeName";

export async function GetVMTypeNameCreateResource() {
  setLoader("ADD", "GetVMTypeNameCreateResource");

  let api = new VMTypeNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<VMTypeNameDto>>(
    () => api.VMTypeNameGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as VMTypeNameCreate;
  rootStore.dispatch({ type: "GET_CREATE_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "GetVMTypeNameCreateResource");
}

export async function CreatVMTypeName(data: VMTypeNameDto) {
  setLoader("ADD", "CreatVMTypeName");
  let api = new VMTypeNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMTypeNameCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as VMTypeNameCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "CreatVMTypeName");
  return rtn;
}
