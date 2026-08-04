import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_BUILD_BAG, RESTORE_BUILD_BAG } from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteBuildBag(id: number) {
  setLoader("ADD", "deleteBuildBag");
  let api = new BuildBagApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagDelete(id)
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
  rootStore.dispatch({ type: DELETE_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "deleteBuildBag");
  return rtn;
}
export async function DeleteDeepBuildBag(id: number) {
  let api = new BuildBagApi();
  setLoader("ADD", "DeleteDeepBuildBag");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "DeleteDeepBuildBag");
  return rtn;
}

export async function RestoreBuildBag(id: number) {
  setLoader("ADD", "RestoreBuildBag");
  let api = new BuildBagApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagRestore(id)
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
  rootStore.dispatch({ type: RESTORE_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "RestoreBuildBag");
  return rtn;
}

export async function GetRelatedRecordsBuildBag(id: number) {
  let api = new BuildBagApi();
  setLoader("ADD", "GetRelatedRecordsBuildBag");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsBuildBag");
  return rtn;
}
