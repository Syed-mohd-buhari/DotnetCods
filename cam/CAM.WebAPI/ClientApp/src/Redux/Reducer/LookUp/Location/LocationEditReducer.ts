import { LocationEdit } from "../../../../Model/LookUp/Location";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LocationEdit = {
	LookUpDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const LocationEditReducer = (state = initState, action: { type: string; payload: LocationEdit }) => {
	switch (action.type) {
		case "EDIT_LOCATION": {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case "GET_EDIT_LOCATION":
			return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
		default:
			return state;
	}
};
