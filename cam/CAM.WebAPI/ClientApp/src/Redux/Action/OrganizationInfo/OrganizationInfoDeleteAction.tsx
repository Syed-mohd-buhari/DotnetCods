import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { OrganizationInfoApi } from "../../../Business/OrganizationInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_ORGANIZATION_INFO,
  RESTORE_ORGANIZATION_INFO,
} from "../../../Model/OrganizationInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteOrganizationInfo(id: number) {
  setLoader("ADD", "deleteOrganizationInfo");
  let api = new OrganizationInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoDelete(id)
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
  rootStore.dispatch({ type: DELETE_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "deleteOrganizationInfo");
  return rtn;
}
export async function DeleteDeepOrganizationInfo(id: number) {
  let api = new OrganizationInfoApi();
  setLoader("ADD", "DeleteDeepOrganizationInfo");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "DeleteDeepOrganizationInfo");
  return rtn;
}

export async function RestoreOrganizationInfo(id: number) {
  setLoader("ADD", "RestoreOrganizationInfo");
  let api = new OrganizationInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoRestore(id)
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
  rootStore.dispatch({ type: RESTORE_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "RestoreOrganizationInfo");
  return rtn;
}

export async function GetRelatedRecordsOrganizationInfo(id: number) {
  let api = new OrganizationInfoApi();
  setLoader("ADD", "GetRelatedRecordsOrganizationInfo");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsOrganizationInfo");
  return rtn;
}
