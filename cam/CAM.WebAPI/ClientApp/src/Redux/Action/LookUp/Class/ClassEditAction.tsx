import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ClassApi } from "../../../../Business/LookUp/ClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { ClassDto } from "../../../../Model/LookUp/Class";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetClassEditResource(id: number) {
  setLoader("ADD", "GetClassEditResource");

  let api = new ClassApi();
  let createResource = await ApiCallWithErrorHandling<Promise<ClassDto>>(() =>
    api.ClassGetUpdateResourceClass(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_CLASS", payload: rtn });
  setLoader("REMOVE", "GetClassEditResource");

  return rtn;
}

export async function EditClass(data: ClassDto) {
  setLoader("ADD", "EditClass");
  let api = new ClassApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ClassPut(data)
  );
  let rtn = { ResultDtoEdit: result } as ClassDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CLASS", payload: rtn });
  setLoader("REMOVE", "EditClass");
  return rtn;
}
