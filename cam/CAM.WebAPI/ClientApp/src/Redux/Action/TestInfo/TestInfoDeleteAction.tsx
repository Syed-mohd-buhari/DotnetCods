import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TestInfoApi } from "../../../Business/TestInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_TEST_INFO, RESTORE_TEST_INFO } from "../../../Model/TestInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteTestInfo(id: number) {
  setLoader("ADD", "deleteTestInfo");
  let api = new TestInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.testInfoDelete(id)
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
  rootStore.dispatch({ type: DELETE_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "deleteTestInfo");
  return rtn;
}
export async function DeleteDeepTestInfo(id: number) {
  let api = new TestInfoApi();
  setLoader("ADD", "DeleteDeepTestInfo");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.testInfoDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "DeleteDeepTestInfo");
  return rtn;
}

export async function RestoreTestInfo(id: number) {
  setLoader("ADD", "RestoreTestInfo");
  let api = new TestInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.testInfoRestore(id)
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
  rootStore.dispatch({ type: RESTORE_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "RestoreTestInfo");
  return rtn;
}
