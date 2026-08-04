import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFPriorityApi } from "../../../../Business/LookUp/CNFPriorityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  CNFPriorityDto,
  CNFPriorityDtoGrid,
  CNFPriorityEdit,
} from "../../../../Model/LookUp/CNFPriority";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCNFPriorityEditResource(id: number) {
  setLoader("ADD", "GetCNFPriorityEditResource");

  let api = new CNFPriorityApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFPriorityDto>>(
    () => api.CNFPriorityGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as CNFPriorityEdit;
  rootStore.dispatch({ type: "GET_EDIT_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "GetCNFPriorityEditResource");

  return rtn;
}

export async function EditCNFPriority(data: CNFPriorityDto) {
  setLoader("ADD", "EditCNFPriority");
  let api = new CNFPriorityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFPriorityUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as CNFPriorityEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "EditCNFPriority");
  return rtn;
}
