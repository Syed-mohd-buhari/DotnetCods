import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFPriorityApi } from "../../../../Business/LookUp/CNFPriorityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  CNFPriorityCreate,
  CNFPriorityDto,
} from "../../../../Model/LookUp/CNFPriority";

export async function GetCNFPriorityCreateResource() {
  setLoader("ADD", "GetCNFPriorityCreateResource");

  let api = new CNFPriorityApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFPriorityDto>>(
    () => api.CNFPriorityGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as CNFPriorityCreate;
  rootStore.dispatch({ type: "GET_CREATE_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "GetCNFPriorityCreateResource");
}

export async function CreatCNFPriority(data: CNFPriorityDto) {
  setLoader("ADD", "CreatCNFPriority");
  let api = new CNFPriorityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFPriorityCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as CNFPriorityCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "CreatCNFPriority");
  return rtn;
}
