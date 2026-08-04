import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { IntraVMTypeApi } from "../../../../Business/LookUp/IntraVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  IntraVMTypeDto,
  IntraVMTypeDtoGrid,
  IntraVMTypeEdit,
} from "../../../../Model/LookUp/IntraVMType";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetIntraVMTypeEditResource(id: number) {
  setLoader("ADD", "GetIntraVMTypeEditResource");

  let api = new IntraVMTypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<IntraVMTypeDto>>(
    () => api.IntraVMTypeGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as IntraVMTypeEdit;
  rootStore.dispatch({ type: "GET_EDIT_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "GetIntraVmTypeEditResource");

  return rtn;
}

export async function EditIntraVMType(data: IntraVMTypeDto) {
  setLoader("ADD", "EditIntraVMType");
  let api = new IntraVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IntraVMTypeUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as IntraVMTypeEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "EditIntraVMType");
  return rtn;
}
