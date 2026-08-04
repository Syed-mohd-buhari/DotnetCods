import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMWorkloadTypeApi } from "../../../../Business/LookUp/VMWorkloadTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  VMWorkloadTypeCreate,
  VMWorkloadTypeDto,
} from "../../../../Model/LookUp/VMWorkloadType";

export async function GetVMWorkloadTypeCreateResource() {
  setLoader("ADD", "GetVMWorkloadTypeCreateResource");

  let api = new VMWorkloadTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VMWorkloadTypeDto>
  >(() => api.VMWorkloadTypeGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as VMWorkloadTypeCreate;
  rootStore.dispatch({ type: "GET_CREATE_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "GetVMWorkloadTypeCreateResource");
}

export async function CreatVMWorkloadType(data: VMWorkloadTypeDto) {
  setLoader("ADD", "CreatVMWorkloadType");
  let api = new VMWorkloadTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMWorkloadTypeCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as VMWorkloadTypeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "CreatVMWorkloadType");
  return rtn;
}
