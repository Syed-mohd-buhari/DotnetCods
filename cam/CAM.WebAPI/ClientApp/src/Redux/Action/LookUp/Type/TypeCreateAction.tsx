import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { TypeApi } from "../../../../Business/LookUp/TypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { TypeDto } from "../../../../Model/LookUp/Type";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetTypeCreateResource() {
  setLoader("ADD", "GetTypeCreateResource");

  let api = new TypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<TypeDto>>(() =>
    api.TypeGetCreateResourceType()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_TYPE", payload: rtn });
  setLoader("REMOVE", "GetTypeCreateResource");
}

export async function CreatType(data: TypeDto) {
  let api = new TypeApi();
  setLoader("ADD", "CreatType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TypeCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_TYPE", payload: rtn });
  setLoader("REMOVE", "CreatType");
  return rtn;
}
export async function GetTypesByClassId(classId: number) {
  setLoader("ADD", "GetTypesByClassId");  

  let api = new TypeApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetTypesByClassId(classId)
  );
  setLoader("REMOVE", "GetTypesByClassId");

  return data
}