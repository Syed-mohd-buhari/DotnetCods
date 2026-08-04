import {
  GET_GRID_DELIVERY_TRACKING,
  GET_FILTER_DELIVERY_TRACKING,
  DeliveryTrackingGrid,
} from "../../../Model/DeliveryTracking";

const initState: DeliveryTrackingGrid = {
  DeliveryTrackingGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const DeliveryTrackingGridReducer = (
  state = initState,
  action: { type: string; payload: DeliveryTrackingGrid }
) => {
  switch (action.type) {
    case GET_GRID_DELIVERY_TRACKING: {
      return {
        ...state,
        DeliveryTrackingGridResult: action.payload.DeliveryTrackingGridResult,
      };
    }
    case GET_FILTER_DELIVERY_TRACKING:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
