import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { InterVMTypeApi } from "../../../../Business/LookUp/InterVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  InterVMTypeCreate,
  InterVMTypeDto,
} from "../../../../Model/LookUp/InterVMType";

export async function GetInterVMTypeCreateResource() {
  setLoader("ADD", "GetInterVMTypeCreateResource");

  let api = new InterVMTypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<InterVMTypeDto>>(
    () => api.InterVMTypeGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as InterVMTypeCreate;
  rootStore.dispatch({ type: "GET_CREATE_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "GetInterVMTypeCreateResource");
}

export async function CreatInterVMType(data: InterVMTypeDto) {
  setLoader("ADD", "CreatInterVMType");
  let api = new InterVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InterVMTypeCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as InterVMTypeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "CreatInterVMType");
  return rtn;
}
