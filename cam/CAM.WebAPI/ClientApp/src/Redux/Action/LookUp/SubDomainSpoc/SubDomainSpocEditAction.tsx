import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainSpocApi } from "../../../../Business/LookUp/SubDomainSpocBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSubDomainSpocEditResource(id: number) {
  setLoader("ADD", "GetSubDomainSpocEditResource");

  let api = new SubDomainSpocApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.subDomainSpocGetUpdateResourceSubDomainSpoc(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SUB_DOMAIN_SPOC", payload: rtn });
  setLoader("REMOVE", "GetSubDomainSpocEditResource");

  return rtn;
}

export async function EditSubDomainSpoc(data: TipologicaGridDto) {
  setLoader("ADD", "EditSubDomainSpoc");
  let api = new SubDomainSpocApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainSpocPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SUB_DOMAIN_SPOC", payload: rtn });
  setLoader("REMOVE", "EditSubDomainSpoc");
  return rtn;
}
