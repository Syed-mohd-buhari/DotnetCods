import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainResponsibleApi } from "../../../../Business/LookUp/SubDomainResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSubDomainResponsibleEditResource(id: number) {
  setLoader("ADD", "GetSubDomainResponsibleEditResource");

  let api = new SubDomainResponsibleApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.subDomainResponsibleGetUpdateResourceSubDomainResponsible(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SUB_DOMAIN_RESPONSIBLE", payload: rtn });
  setLoader("REMOVE", "GetSubDomainResponsibleEditResource");

  return rtn;
}

export async function EditSubDomainResponsible(data: TipologicaGridDto) {
  setLoader("ADD", "EditSubDomainResponsible");
  let api = new SubDomainResponsibleApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainResponsiblePut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SUB_DOMAIN_RESPONSIBLE", payload: rtn });
  setLoader("REMOVE", "EditSubDomainResponsible");
  return rtn;
}
