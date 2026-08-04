import { LookUpEditActivityDetails } from "../../../../Model/LookUp/ActivityDetails";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEditActivityDetails = {
	LookUpDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const ActivityDetailsEditReducer = (state = initState, action: { type: string; payload: LookUpEditActivityDetails }) => {
	switch (action.type) {
		case "EDIT_ACTIVITY_DETAILS": {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case "GET_EDIT_ACTIVITY_DETAILS":
			return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
		default:
			return state;
	}
};
