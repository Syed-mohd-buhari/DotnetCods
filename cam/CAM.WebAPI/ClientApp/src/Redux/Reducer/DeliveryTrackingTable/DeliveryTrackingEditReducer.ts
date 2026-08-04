import {
  EDIT_DELIVERY_TRACKING,
  GET_EDIT_DELIVERY_TRACKING,
  DeliveryTrackingEdit,
} from "../../../Model/DeliveryTracking";

const initState: DeliveryTrackingEdit = {
  DeliveryTrackingDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const DeliveryTrackingEditReducer = (
  state = initState,
  action: { type: string; payload: DeliveryTrackingEdit }
) => {
  switch (action.type) {
    case EDIT_DELIVERY_TRACKING: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_DELIVERY_TRACKING:
      return {
        ...state,
        DeliveryTrackingDtoEdit: action.payload.DeliveryTrackingDtoEdit,
      };
    default:
      return state;
  }
};
