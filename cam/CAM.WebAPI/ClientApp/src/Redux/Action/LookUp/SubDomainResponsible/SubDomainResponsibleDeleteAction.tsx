import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainResponsibleApi } from "../../../../Business/LookUp/SubDomainResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteSubDomainResponsible(id: number) {
  setLoader("ADD", "deleteSubDomainResponsible");
  let api = new SubDomainResponsibleApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainResponsibleDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_SUB_DOMAIN_RESPONSIBLE", payload: rtn });
  setLoader("REMOVE", "deleteSubDomainResponsible");
  return rtn;
}

export async function DeleteDeepSubDomainResponsible(id: number) {
  let api = new SubDomainResponsibleApi();
  setLoader("ADD", "DeleteDeepSubDomainResponsible");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainResponsibleDeleteDeep(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_SUB_DOMAIN_RESPONSIBLE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepSubDomainResponsible");
  return rtn;
}

export async function GetRelatedRecordsSubDomainResponsible(id: number) {
  let api = new SubDomainResponsibleApi();
  setLoader("ADD", "GetRelatedRecordsSubDomainResponsible");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subDomainResponsibleGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsSubDomainResponsible");
  return rtn;
}
