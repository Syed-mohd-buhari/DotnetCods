import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFFunctionStandardNameApi } from "../../../../Business/LookUp/CNFFunctionStandardNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  CNFFunctionStandardNameDto,
  CNFFunctionStandardNameDtoGrid,
  CNFFunctionStandardNameEdit,
} from "../../../../Model/LookUp/CNFFunctionStandardName";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCNFFunctionStandardNameEditResource(id: number) {
  setLoader("ADD", "GetCNFFunctionStandardNameEditResource");

  let api = new CNFFunctionStandardNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CNFFunctionStandardNameDto>
  >(() => api.CNFFunctionStandardNameGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as CNFFunctionStandardNameEdit;
  rootStore.dispatch({
    type: "GET_EDIT_CNFFUNCTIONSTANDARDNAME",
    payload: rtn,
  });
  setLoader("REMOVE", "GetCNFFunctionStandardNameEditResource");

  return rtn;
}

export async function EditCNFFunctionStandardName(
  data: CNFFunctionStandardNameDto
) {
  setLoader("ADD", "EditCNFFunctionStandardName");
  let api = new CNFFunctionStandardNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFFunctionStandardNameUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as CNFFunctionStandardNameEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CNFFUNCTIONSTANDARDNAME", payload: rtn });
  setLoader("REMOVE", "EditCNFFunctionStandardName");
  return rtn;
}
