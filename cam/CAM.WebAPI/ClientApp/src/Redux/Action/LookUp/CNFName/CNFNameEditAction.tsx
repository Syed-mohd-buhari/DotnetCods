import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFNameApi } from "../../../../Business/LookUp/CNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  CNFNameDto,
  CNFNameDtoGrid,
  CNFNameEdit,
} from "../../../../Model/LookUp/CNFName";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCNFNameEditResource(id: number) {
  setLoader("ADD", "GetCNFNameEditResource");

  let api = new CNFNameApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CNFNameDto>>(() =>
    api.CNFNameGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as CNFNameEdit;
  rootStore.dispatch({ type: "GET_EDIT_CNFNAME", payload: rtn });
  setLoader("REMOVE", "GetCNFNameEditResource");

  return rtn;
}

export async function EditCNFName(data: CNFNameDto) {
  setLoader("ADD", "EditCNFName");
  let api = new CNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFNameUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as CNFNameEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CNFNAME", payload: rtn });
  setLoader("REMOVE", "EditCNFName");
  return rtn;
}
