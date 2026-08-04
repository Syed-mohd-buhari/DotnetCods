import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMWorkloadTypeApi } from "../../../../Business/LookUp/VMWorkloadTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  VMWorkloadTypeDto,
  VMWorkloadTypeDtoGrid,
  VMWorkloadTypeEdit,
} from "../../../../Model/LookUp/VMWorkloadType";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVMWorkloadTypeEditResource(id: number) {
  setLoader("ADD", "GetVMWorkloadTypeEditResource");

  let api = new VMWorkloadTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VMWorkloadTypeDto>
  >(() => api.VMWorkloadTypeGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as VMWorkloadTypeEdit;
  rootStore.dispatch({ type: "GET_EDIT_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "GetVmWorkloadTypeEditResource");

  return rtn;
}

export async function EditVMWorkloadType(data: VMWorkloadTypeDto) {
  setLoader("ADD", "EditVMWorkloadType");
  let api = new VMWorkloadTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMWorkloadTypeUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as VMWorkloadTypeEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "EditVMWorkloadType");
  return rtn;
}
