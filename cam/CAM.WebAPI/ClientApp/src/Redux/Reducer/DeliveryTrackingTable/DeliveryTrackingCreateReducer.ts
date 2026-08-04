import {
  CREATE_DELIVERY_TRACKING,
  GET_CREATE_DELIVERY_TRACKING,
  DeliveryTrackingCreate,
} from "../../../Model/DeliveryTracking";

const initState: DeliveryTrackingCreate = {
  ResultDtoCreate: null,
  DeliveryTrackingDtoCreate: null,
};
//const dispatch = useDispatch();

export const DeliveryTrackingCreateReducer = (
  state = initState,
  action: { type: string; payload: DeliveryTrackingCreate }
) => {
  switch (action.type) {
    case CREATE_DELIVERY_TRACKING: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_DELIVERY_TRACKING:
      return {
        ...state,
        DeliveryTrackingDtoCreate: action.payload.DeliveryTrackingDtoCreate,
      };
    default:
      return state;
  }
};
