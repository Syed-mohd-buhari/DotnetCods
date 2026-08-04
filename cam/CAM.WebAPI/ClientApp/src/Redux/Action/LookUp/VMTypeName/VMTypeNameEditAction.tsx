import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMTypeNameApi } from "../../../../Business/LookUp/VMTypeNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  VMTypeNameDto,
  VMTypeNameDtoGrid,
  VMTypeNameEdit,
} from "../../../../Model/LookUp/VMTypeName";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVMTypeNameEditResource(id: number) {
  setLoader("ADD", "GetVMTypeNameEditResource");

  let api = new VMTypeNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<VMTypeNameDto>>(
    () => api.VMTypeNameGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as VMTypeNameEdit;
  rootStore.dispatch({ type: "GET_EDIT_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "GetVmTypeNameEditResource");

  return rtn;
}

export async function EditVMTypeName(data: VMTypeNameDto) {
  setLoader("ADD", "EditVMTypeName");
  let api = new VMTypeNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMTypeNameUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as VMTypeNameEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "EditVMTypeName");
  return rtn;
}
