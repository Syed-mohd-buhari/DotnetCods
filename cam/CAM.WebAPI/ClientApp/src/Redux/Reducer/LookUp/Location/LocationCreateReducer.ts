import { LocationCreate } from "../../../../Model/LookUp/Location";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LocationCreate = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const LocationCreateReducer = (state = initState, action: { type: string; payload: LocationCreate }) => {
	switch (action.type) {
		case "CREATE_LOCATION": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_LOCATION":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
