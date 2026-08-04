import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFNameApi } from "../../../../Business/LookUp/VNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  VNFNameDto,
  VNFNameDtoGrid,
  VNFNameEdit,
} from "../../../../Model/LookUp/VNFName";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVNFNameEditResource(id: number) {
  setLoader("ADD", "GetVNFNameEditResource");

  let api = new VNFNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<VNFNameDto>>(() =>
    api.VNFNameGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as VNFNameEdit;
  rootStore.dispatch({ type: "GET_EDIT_VNFNAME", payload: rtn });
  setLoader("REMOVE", "GetVNFNameEditResource");

  return rtn;
}

export async function EditVNFName(data: VNFNameDto) {
  setLoader("ADD", "EditVNFName");
  let api = new VNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFNameUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as VNFNameEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VNFNAME", payload: rtn });
  setLoader("REMOVE", "EditVNFName");
  return rtn;
}
