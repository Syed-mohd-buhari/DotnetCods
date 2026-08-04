import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { DeliveryTrackingApi } from "../../../Business/DeliveryTrackingBusiness";
import {
  GET_FILTER_DELIVERY_TRACKING,
  GET_GRID_DELIVERY_TRACKING,
  DeliveryTrackingDtoGrid,
  DeliveryTrackingGrid,
  DeliveryTrackingQueryObjectGrid,
  QueryResultDtoOfDeliveryTrackingDtoGrid,
} from "../../../Model/DeliveryTracking";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDeliveryTrackingGrid(
  queryFilter?: DeliveryTrackingQueryObjectGrid,
  returnValues?: boolean,
  hideLoader?: boolean
) {
  if (hideLoader !== true) {
    setLoader("ADD", "GetDeliveryTrackingGrid");
  }
  let api = new DeliveryTrackingApi();
  let result: QueryResultDtoOfDeliveryTrackingDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDeliveryTrackingDtoGrid>
    >(() => api.DeliveryTrackingGetDeliveryTracking(queryFilter ?? {}));

    let rtn = {
      DeliveryTrackingGridResult: result,
      filter: null,
    } as DeliveryTrackingGrid;
    if (returnValues !== true) {
      rootStore.dispatch({ type: GET_GRID_DELIVERY_TRACKING, payload: rtn });
    } else {
      setLoader("REMOVE", "GetDeliveryTrackingGrid");
      return rtn.DeliveryTrackingGridResult?.items as DeliveryTrackingDtoGrid[];
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_DELIVERY_TRACKING,
      payload: {
        DeliveryTrackingGridResult: null,
        filter: null,
      } as DeliveryTrackingGrid,
    });
  }
  setLoader("REMOVE", "GetDeliveryTrackingGrid");
}

export async function GetFilterColumDeliveryTracking(
  columName: string,
  columValue: string,
  queryFilter?: DeliveryTrackingQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new DeliveryTrackingApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.DeliveryTrackingGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    DeliveryTrackingGridResult: null,
  } as DeliveryTrackingGrid;
  rootStore.dispatch({ type: GET_FILTER_DELIVERY_TRACKING, payload: rtn });
}
