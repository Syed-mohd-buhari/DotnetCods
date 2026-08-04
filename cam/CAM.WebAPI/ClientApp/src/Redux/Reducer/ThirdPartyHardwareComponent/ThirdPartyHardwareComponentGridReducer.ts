import {
  GET_FILTER_THIRD_PARTY_HARDWARE_COMPONENT,
  GET_GRID_THIRD_PARTY_HARDWARE_COMPONENT,
  ThirdPartyHardwareComponentGrid,
} from "../../../Model/ThirdPartyHardwareComponent";

const initState: ThirdPartyHardwareComponentGrid = {
  ThirdPartyHardwareComponentGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ThirdPartyHardwareComponentGridReducer = (
  state = initState,
  action: { type: string; payload: ThirdPartyHardwareComponentGrid }
) => {
  switch (action.type) {
    case GET_GRID_THIRD_PARTY_HARDWARE_COMPONENT: {
      return {
        ...state,
        ThirdPartyHardwareComponentGridResult:
          action.payload.ThirdPartyHardwareComponentGridResult,
      };
    }
    case GET_FILTER_THIRD_PARTY_HARDWARE_COMPONENT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
