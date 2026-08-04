import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainResponsibleApi } from "../../../../Business/LookUp/SubDomainResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSubDomainResponsibleCreateResource() {
  setLoader("ADD", "GetSubDomainResponsibleCreateResource");

  let api = new SubDomainResponsibleApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.subDomainResponsibleGetCreateResourceSubDomainResponsible());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_SUB_DOMAIN_RESPONSIBLE",
    payload: rtn,
  });
  setLoader("REMOVE", "GetSubDomainResponsibleCreateResource");
}

export async function CreatSubDomainResponsible(data: TipologicaGridDto) {
  let api = new SubDomainResponsibleApi();
  setLoader("ADD", "CreatSubDomainResponsible");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainResponsibleCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SUB_DOMAIN_RESPONSIBLE", payload: rtn });
  setLoader("REMOVE", "CreatSubDomainResponsible");
  return rtn;
}
