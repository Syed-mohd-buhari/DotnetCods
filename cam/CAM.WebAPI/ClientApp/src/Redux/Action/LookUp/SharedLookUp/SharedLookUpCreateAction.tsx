import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharedLookUpApi } from "../../../../Business/LookUp/SharedLookUpBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSharedLookUpCreateResource() {
  setLoader("ADD", "GetSharedLookUpCreateResource");

  let api = new SharedLookUpApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() =>
    api.sharedLookUpGetCreateResourceSharedLookUp(sessionStorage.apiType)
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_SHARED_LOOKUP",
    payload: rtn,
  });
  setLoader("REMOVE", "GetSharedLookUpCreateResource");
}

export async function CreateSaveFunction(data: TipologicaGridDto) {
  let api = new SharedLookUpApi();
  setLoader("ADD", "CreatSharedLookUp");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.sharedLookUpCreate(sessionStorage.apiType, data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SHARED_LOOKUP", payload: rtn });
  setLoader("REMOVE", "CreatSharedLookUp");
  return rtn;
}
