import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ClassApi } from "../../../../Business/LookUp/ClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { ClassDto } from "../../../../Model/LookUp/Class";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetClassCreateResource() {
  setLoader("ADD", "GetClassCreateResource");

  let api = new ClassApi();
  let createResource = await ApiCallWithErrorHandling<Promise<ClassDto>>(() =>
    api.ClassGetCreateResourceClass()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_CLASS", payload: rtn });
  setLoader("REMOVE", "GetClassCreateResource");
}

export async function CreatClass(data: ClassDto) {
  let api = new ClassApi();
  setLoader("ADD", "CreatClass");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ClassCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CLASS", payload: rtn });
  setLoader("REMOVE", "CreatClass");
  return rtn;
}
export async function GetClasssesByCategoryId(categoryId: number) {
  setLoader("ADD", "GetClasssesByCategoryId");  

  let api = new ClassApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetClasssesByCategoryId(categoryId)
  );
  setLoader("REMOVE", "GetAssetsByOpcoIdAndProductNameId");

  return data
}