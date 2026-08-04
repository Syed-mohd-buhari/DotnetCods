import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_NETWORK_ELEMENT_AS_PLANNED,
  GET_EDIT_NETWORK_ELEMENT_AS_PLANNED,
  NetworkElementAsPlannedDtoUpdate,
  NetworkElementAsPlannedEdit,
} from "../../../Model/NetworkElementAsPlanned";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNetworkElementAsPlannedEditResource(id: number) {
  setLoader("ADD", "GetNetworkElementAsPlannedEditResource");

  let api = new NetworkElementAsPlannedApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<NetworkElementAsPlannedDtoUpdate>
  >(() => api.networkElementAsPlannedGetUpdateResourceLcmengineering(id));
  let rtn = {
    NetworkElementAsPlannedDtoEdit: createResource,
  } as NetworkElementAsPlannedEdit;
  rootStore.dispatch({
    type: GET_EDIT_NETWORK_ELEMENT_AS_PLANNED,
    payload: rtn,
  });
  setLoader("REMOVE", "GetNetworkElementAsPlannedEditResource");
}

export async function GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(
  plannedActivityResourceId?: number,
  deliveryStatusId?: number,
  isAddAsset?: boolean
) {
  setLoader(
    "ADD",
    "GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource"
  );

  let api = new NetworkElementAsPlannedApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(
      plannedActivityResourceId,
      deliveryStatusId,
      isAddAsset
    )
  );
  if (result && !result.warning) {
    setLoader(
      "REMOVE",
      "GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource"
    );

    return result;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader(
      "REMOVE",
      "GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource"
    );
  }
}

export async function EditNetworkElementAsPlanned(
  data: NetworkElementAsPlannedDtoUpdate,
  forced?: boolean
) {
  let api = new NetworkElementAsPlannedApi();
  setLoader("ADD", "EditNetworkElementAsPlanned");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsPlannedPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as NetworkElementAsPlannedEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_NETWORK_ELEMENT_AS_PLANNED, payload: rtn });
  setLoader("REMOVE", "EditNetworkElementAsPlanned");
  return rtn;
}
