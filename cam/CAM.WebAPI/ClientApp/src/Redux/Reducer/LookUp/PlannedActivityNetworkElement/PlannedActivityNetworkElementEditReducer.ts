import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEdit = {
	LookUpDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const PlannedActivityNetworkElementEditReducer = (state = initState, action: { type: string; payload: LookUpEdit }) => {
	switch (action.type) {
		case "EDIT_PLANNED_ACTIVITY_NETWORK_ELEMENT": {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case "GET_EDIT_PLANNED_ACTIVITY_NETWORK_ELEMENT":
			return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
		default:
			return state;
	}
};
