import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  VolteKPIApiFetchParamCreator,
  VolteKPIApi,
} from "../../../Business/VolteKPIBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_VOLTE_KPI,
  VolteKPIDtoCreate,
  VolteKPICreate,
  GET_CREATE_VOLTE_KPI,
  VolteKPIQueryObjectGrid,
} from "../../../Model/VolteKpi/VolteKPI";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVolteKPICreateResource(
  parameters?: VolteKPIQueryObjectGrid
) {
  //  ;
  setLoader("ADD", "GetVolteKPICreateResource");

  let api = new VolteKPIApi();
  let createResource: VolteKPIDtoCreate | undefined = undefined;

  createResource = await ApiCallWithErrorHandling<Promise<VolteKPIDtoCreate>>(
    () => api.volteKPIGetKPI(parameters ?? {})
  );

  let rtn = {
    ResultDtoCreate: null,
    VolteKPIDtoCreate: createResource,
  } as VolteKPICreate;
  rootStore.dispatch({ type: GET_CREATE_VOLTE_KPI, payload: rtn });
  //  ;
  setLoader("REMOVE", "GetVolteKPICreateResource");
}

export async function CreatVolteKPI(data: VolteKPIDtoCreate, forced?: boolean) {
  //  ;
  setLoader("ADD", "CreatVolteKPI");

  let api = new VolteKPIApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.volteKPISaveOrEdit(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    VolteKPIDtoCreate: null,
  } as VolteKPICreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_VOLTE_KPI, payload: rtn });
  //  ;
  setLoader("REMOVE", "CreatVolteKPI");

  return rtn;
}
