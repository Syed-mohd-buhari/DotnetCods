import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";
import { LookUpCreatePlannedActivityResource } from "../../../../Model/LookUp/PlannedActivityResource";

const initState: LookUpCreatePlannedActivityResource = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const PlannedActivityResourceCreateReducer = (state = initState, action: { type: string; payload: LookUpCreatePlannedActivityResource }) => {
	switch (action.type) {
		case "CREATE_PLANNED_ACTIVITY_RESOURCE": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_PLANNED_ACTIVITY_RESOURCE":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
