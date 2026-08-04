import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFFunctionStandardNameApi } from "../../../../Business/LookUp/CNFFunctionStandardNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  CNFFunctionStandardNameCreate,
  CNFFunctionStandardNameDto,
} from "../../../../Model/LookUp/CNFFunctionStandardName";

export async function GetCNFFunctionStandardNameCreateResource() {
  setLoader("ADD", "GetCNFFunctionStandardNameCreateResource");

  let api = new CNFFunctionStandardNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CNFFunctionStandardNameDto>
  >(() => api.CNFFunctionStandardNameGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as CNFFunctionStandardNameCreate;
  rootStore.dispatch({
    type: "GET_CREATE_CNFFUNCTIONSTANDARDNAME",
    payload: rtn,
  });
  setLoader("REMOVE", "GetCNFFunctionStandardNameCreateResource");
}

export async function CreatCNFFunctionStandardName(
  data: CNFFunctionStandardNameDto
) {
  setLoader("ADD", "CreatCNFFunctionStandardName");
  let api = new CNFFunctionStandardNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFFunctionStandardNameCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as CNFFunctionStandardNameCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CNFFUNCTIONSTANDARDNAME", payload: rtn });
  setLoader("REMOVE", "CreatCNFFunctionStandardName");
  return rtn;
}
