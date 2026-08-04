import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DeliveryTrackingApi } from "../../../Business/DeliveryTrackingBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_DELIVERY_TRACKING,
  RESTORE_DELIVERY_TRACKING,
} from "../../../Model/DeliveryTracking";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteDeliveryTrackingd(id: number) {
  let api = new DeliveryTrackingApi();
  setLoader("ADD", "deleteDeliveryTrackingd");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingDelete(id)
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
  rootStore.dispatch({ type: DELETE_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "deleteDeliveryTrackingd");
  return rtn;
}

export async function RestoreDeliveryTracking(id: number) {
  setLoader("ADD", "RestoreDeliveryTracking");
  let api = new DeliveryTrackingApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingRestore(id)
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
  rootStore.dispatch({ type: RESTORE_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "RestoreDeliveryTracking");
  return rtn;
}

export async function DeleteDeepDeliveryTracking(id: number) {
  let api = new DeliveryTrackingApi();
  setLoader("ADD", "DeleteDeepDeliveryTracking");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "DeleteDeepDeliveryTracking");
  return rtn;
}

export async function GetRelatedRecordsDeliveryTracking(id: number) {
  let api = new DeliveryTrackingApi();
  setLoader("ADD", "GetRelatedRecordsDeliveryTracking");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsDeliveryTracking");
  return rtn;
}
