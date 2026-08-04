import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";
import { LookUpEditPlannedActivityResource } from "../../../../Model/LookUp/PlannedActivityResource";

const initState: LookUpEditPlannedActivityResource = {
	LookUpDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const PlannedActivityResourceEditReducer = (state = initState, action: { type: string; payload: LookUpEditPlannedActivityResource }) => {
	switch (action.type) {
		case "EDIT_PLANNED_ACTIVITY_RESOURCE": {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case "GET_EDIT_PLANNED_ACTIVITY_RESOURCE":
			return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
		default:
			return state;
	}
};
