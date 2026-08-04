import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DeliveryTrackingApi } from "../../../Business/DeliveryTrackingBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_DELIVERY_TRACKING,
  GET_CREATE_DELIVERY_TRACKING,
  DeliveryTrackingCreate,
  DeliveryTrackingDtoCreate,
} from "../../../Model/DeliveryTracking";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDeliveryTrackingCreateResource() {
  setLoader("ADD", "GetDeliveryTrackingCreateResource");

  let api = new DeliveryTrackingApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<DeliveryTrackingDtoCreate>
  >(() => api.DeliveryTrackingGetCreateResourceDeliveryTracking());
  let rtn = {
    ResultDtoCreate: null,
    DeliveryTrackingDtoCreate: { ...createResource, eomStatus: 1 },
  } as DeliveryTrackingCreate;
  rootStore.dispatch({ type: GET_CREATE_DELIVERY_TRACKING, payload: rtn });
  setLoader("REMOVE", "GetDeliveryTrackingCreateResource");

  return rtn.DeliveryTrackingDtoCreate;
}

export async function CreatDeliveryTracking(
  data: DeliveryTrackingDtoCreate,
  forced?: boolean
) {
  let api = new DeliveryTrackingApi();
  setLoader("ADD", "CreatDeliveryTracking");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.DeliveryTrackingCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    DeliveryTrackingDtoCreate: null,
  } as DeliveryTrackingCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_DELIVERY_TRACKING, payload: rtn });

  setLoader("REMOVE", "CreatDeliveryTracking");
  return rtn;
}

export async function GetDeliveryTrackingImportStatus(file: File) {
  let api = new DeliveryTrackingApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetDeliveryTrackingImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.deliveryTrackingImportStatus(file)
    );
    console.log("Api Response:", res);
    setLoader("REMOVE", "GetDeliveryTrackingImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDeliveryTrackingImportStatus");
}