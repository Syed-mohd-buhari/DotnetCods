import { LookUpCreateActivityDetails } from "../../../../Model/LookUp/ActivityDetails";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreateActivityDetails = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const ActivityDetailsCreateReducer = (state = initState, action: { type: string; payload: LookUpCreateActivityDetails }) => {
	switch (action.type) {
		case "CREATE_ACTIVITY_DETAILS": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_ACTIVITY_DETAILS":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
