import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSystemTypeList(data: number) {
  setLoader("ADD", "GetSystemTypeList");

  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetSystemTypeList(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetSystemTypeList");

    return result?.data;
  }
  setLoader("REMOVE", "GetSystemTypeList");
}

export async function GetSystemTypeInfo(data: number) {
  setLoader("ADD", "GetSystemTypeInfo");

  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetSystemTypeInfo(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetSystemTypeInfo");

    return result?.data;
  }
  setLoader("REMOVE", "GetSystemTypeInfo");
}
export async function GetSystemTypeListFromAsPlanned(data: number) {
  setLoader("ADD", "GetSystemTypeListFromAsPlanned");

  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetSystemTypeListFromAsPlanned(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetSystemTypeListFromAsPlanned");

    return result?.data;
  }
  setLoader("REMOVE", "GetSystemTypeListFromAsPlanned");
}
export async function GetAsPlannedLocation(data: number) {
  setLoader("ADD", "GetAsPlannedLocation");

  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetAsPlannedLocation(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetAsPlannedLocation");

    return result?.data;
  }
  setLoader("REMOVE", "GetAsPlannedLocation");
}
export async function GetNetworkElementAsPlannedResource(data: {
  sysId: number;
  opcoId: number;
}) {
  setLoader("ADD", "GetNetworkElementAsPlannedResource");

  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsGetNetworkElementAsPlannedResource(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetNetworkElementAsPlannedResource");

    return result?.data;
  }
  setLoader("REMOVE", "GetNetworkElementAsPlannedResource");
}
