import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreate = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const PlannedActivityNetworkElementCreateReducer = (state = initState, action: { type: string; payload: LookUpCreate }) => {
	switch (action.type) {
		case "CREATE_PLANNED_ACTIVITY_NETWORK_ELEMENT": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_PLANNED_ACTIVITY_NETWORK_ELEMENT":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
