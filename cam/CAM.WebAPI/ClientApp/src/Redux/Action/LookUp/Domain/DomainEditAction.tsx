import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemNamesApi } from "../../../../Business/LookUp/DomainBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { SystemNamesDto } from "../../../../Model/LookUp/Domain";
import {
  LookUpEdit,
  LookUpForSystemNamesEdit,
  TipologicaGridDtoRule,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSystemNameEditResource(id: number) {
  setLoader("ADD", "GetSystemNameEditResource");
  let api = new SystemNamesApi();
  let editResource = await ApiCallWithErrorHandling<Promise<SystemNamesDto>>(() =>
    api.systemNamesGetUpdateResourceSystemNames(id)
  );
  let rtn = { LookUpDtoEdit: editResource } as LookUpForSystemNamesEdit;
  rootStore.dispatch({ type: "GET_EDIT_SYSTEM_NAME", payload: rtn });
  setLoader("REMOVE", "GetSystemNameEditResource");

  return rtn;
}

export async function EditSystemName(data: SystemNamesDto) {
  setLoader("ADD", "EditSystemName");
  let api = new SystemNamesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemNamesPut(data)
  );
  let rtn = { ResultDtoEdit: result } as LookUpForSystemNamesEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SYSTEM_NAME", payload: rtn });
  setLoader("REMOVE", "EditSystemName");
  return rtn;
}
