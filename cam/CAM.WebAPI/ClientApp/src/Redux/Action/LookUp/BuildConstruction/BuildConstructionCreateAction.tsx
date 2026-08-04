import React from "react";

import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BuildConstructionApi } from "../../../../Business/LookUp/BuildConstructionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreateRule,
  TipologicaGridDto,
  TipologicaGridDtoRule,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

// import { useDispatch } from 'react-redux'

export async function GetBuildConstructionCreateResource() {
  setLoader("ADD", "GetBuildConstructionCreateResource");

  let api = new BuildConstructionApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDtoRule>
  >(() => api.buildConstructionGetCreateResourceBuildConstruction());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreateRule;
  rootStore.dispatch({ type: "GET_CREATE_BUILD_CONSTRUCTION", payload: rtn });
  setLoader("REMOVE", "GetBuildConstructionCreateResource");
}

export async function CreatBuildConstruction(data: TipologicaGridDto) {
  setLoader("ADD", "CreatBuildConstruction");
  let api = new BuildConstructionApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildConstructionCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as LookUpCreateRule;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_BUILD_CONSTRUCTION", payload: rtn });
  setLoader("REMOVE", "CreatBuildConstruction");
  return rtn;
}
