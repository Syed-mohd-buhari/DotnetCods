import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreate = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const SystemFunctionCreateReducer = (state = initState, action: { type: string; payload: LookUpCreate }) => {
	switch (action.type) {
		case "CREATE_SYSTEM_FUNCTION": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_SYSTEM_FUNCTION":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
