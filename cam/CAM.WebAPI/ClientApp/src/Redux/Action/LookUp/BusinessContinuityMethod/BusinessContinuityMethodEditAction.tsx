import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { BusinessContinuityMethodApi } from "../../../../Business/LookUp/BusinessContinuityMethodBusiness";

export async function GetBusinessContinuityMethodEditResource(id: number) {
  setLoader("ADD", "GetBusinessContinuityMethodEditResource");

  let api = new BusinessContinuityMethodApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.businessContinuityMethodGetUpdateResourceBusinessContinuityMethod(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_GEO_RESILIENCE", payload: rtn });
  setLoader("REMOVE", "GetBusinessContinuityMethodEditResource");

  return rtn;
}

export async function EditBusinessContinuityMethod(data: TipologicaGridDto) {
  setLoader("ADD", "EditBusinessContinuityMethod");
  let api = new BusinessContinuityMethodApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.businessContinuityMethodPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_GEO_RESILIENCE", payload: rtn });
  setLoader("REMOVE", "EditBusinessContinuityMethod");
  return rtn;
}
