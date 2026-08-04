import { fail } from "assert";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DeliveryTrackingApi } from "../../../Business/DeliveryTrackingBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_DELIVERY_TRACKING,
  GET_EDIT_DELIVERY_TRACKING,
  DeliveryTrackingDtoUpdate,
  DeliveryTrackingEdit,
} from "../../../Model/DeliveryTracking";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeliveryTrackingEditResource(id: number) {
  setLoader("ADD", "GetDeliveryTrackingEditResource");

  let api = new DeliveryTrackingApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<DeliveryTrackingDtoUpdate>
  >(() => api.DeliveryTrackingGetUpdateResourceDeliveryTracking(id));
  let rtn = {
    DeliveryTrackingDtoEdit: createResource,
  } as DeliveryTrackingEdit;
  rootStore.dispatch({ type: GET_EDIT_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "GetDeliveryTrackingEditResource");

  return rtn.DeliveryTrackingDtoEdit;
}

export async function EditDeliveryTracking(
  data: DeliveryTrackingDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditDeliveryTracking");
  let api = new DeliveryTrackingApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as DeliveryTrackingEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "EditDeliveryTracking");
  return rtn;
}
