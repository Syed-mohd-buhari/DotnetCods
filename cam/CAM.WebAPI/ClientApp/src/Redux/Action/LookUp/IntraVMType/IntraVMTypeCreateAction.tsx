import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { IntraVMTypeApi } from "../../../../Business/LookUp/IntraVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  IntraVMTypeCreate,
  IntraVMTypeDto,
} from "../../../../Model/LookUp/IntraVMType";

export async function GetIntraVMTypeCreateResource() {
  setLoader("ADD", "GetIntraVMTypeCreateResource");

  let api = new IntraVMTypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<IntraVMTypeDto>>(
    () => api.IntraVMTypeGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as IntraVMTypeCreate;
  rootStore.dispatch({ type: "GET_CREATE_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "GetIntraVMTypeCreateResource");
}

export async function CreatIntraVMType(data: IntraVMTypeDto) {
  setLoader("ADD", "CreatIntraVMType");
  let api = new IntraVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IntraVMTypeCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as IntraVMTypeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "CreatIntraVMType");
  return rtn;
}
