import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentFamilyApi } from "../../../Business/DesignComponentFamilyBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_DESIGN_COMPONENT_FAMILY,
  RESTORE_DESIGN_COMPONENT_FAMILY,
} from "../../../Model/DesignComponentFamily";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDesignComponentFamily(id: number) {
  setLoader("ADD", "deleteDesignComponentFamily");
  let api = new DesignComponentFamilyApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designComponentFamilyDelete(id)
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
  rootStore.dispatch({ type: DELETE_DESIGN_COMPONENT_FAMILY, payload: rtn });
  setLoader("REMOVE", "deleteDesignComponentFamily");
  return rtn;
}

export async function DeleteDeepDesignComponentFamily(id: number) {
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "DeleteDeepDesignComponentFamily");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designComponentFamilyDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_DESIGN_COMPONENT_FAMILY, payload: rtn });
  setLoader("REMOVE", "DeleteDeepDesignComponentFamily");
  return rtn;
}

export async function GetRelatedRecordsDesignComponentFamily(id: number) {
  let api = new DesignComponentFamilyApi();
  setLoader("ADD", "GetRelatedRecordsDesignComponentFamily");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designComponentFamilyGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsDesignComponentFamily");
  return rtn;
}

// export async function RestoreDesignComponentFamily(id: number) {
// 	setLoader("ADD", "RestoreDesignComponentFamily");
// 	let api = new DesignComponentFamilyApi();
// 	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentFamilyRestore(id));
// 	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
// 	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
// 	rootStore.dispatch({ type: RESTORE_DESIGN_COMPONENT_FAMILY, payload: rtn });
// 	setLoader("REMOVE", "RestoreDesignComponentFamily");
// 	return rtn;
// }
