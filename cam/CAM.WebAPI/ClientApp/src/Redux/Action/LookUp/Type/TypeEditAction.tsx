import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { TypeApi } from "../../../../Business/LookUp/TypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { TypeDto } from "../../../../Model/LookUp/Type";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetTypeEditResource(id: number) {
  setLoader("ADD", "GetTypeEditResource");

  let api = new TypeApi();
  let createResource = await ApiCallWithErrorHandling<Promise<TypeDto>>(() =>
    api.TypeGetUpdateResourceType(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_TYPE", payload: rtn });
  setLoader("REMOVE", "GetTypeEditResource");

  return rtn;
}

export async function EditType(data: TypeDto) {
  setLoader("ADD", "EditType");
  let api = new TypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TypePut(data)
  );
  let rtn = { ResultDtoEdit: result } as TypeDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_TYPE", payload: rtn });
  setLoader("REMOVE", "EditType");
  return rtn;
}
