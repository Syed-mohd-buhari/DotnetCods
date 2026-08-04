import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { InterVMTypeApi } from "../../../../Business/LookUp/InterVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  InterVMTypeDto,
  InterVMTypeDtoGrid,
  InterVMTypeEdit,
} from "../../../../Model/LookUp/InterVMType";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetInterVMTypeEditResource(id: number) {
  setLoader("ADD", "GetInterVMTypeEditResource");

  let api = new InterVMTypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<InterVMTypeDto>>(
    () => api.InterVMTypeGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as InterVMTypeEdit;
  rootStore.dispatch({ type: "GET_EDIT_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "GetInterVmTypeEditResource");

  return rtn;
}

export async function EditInterVMType(data: InterVMTypeDto) {
  setLoader("ADD", "EditInterVMType");
  let api = new InterVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InterVMTypeUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as InterVMTypeEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "EditInterVMType");
  return rtn;
}
