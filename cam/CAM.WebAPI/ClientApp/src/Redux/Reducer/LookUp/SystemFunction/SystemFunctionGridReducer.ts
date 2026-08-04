import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

export const initState: LookUpGrid = {
	LookUpGridResult: null,
	LookUpGridResultAll: null,
	filter: null,
};
//const dispatch = useDispatch();

export const SystemFunctionGridReducer = (state = initState, action: { type: string; payload: LookUpGrid }) => {
	switch (action.type) {
		case "GET_GRID_SYSTEM_FUNCTION": {
			return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
		}
		case "GET_GRID_SYSTEM_FUNCTION_ALL": {
			return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
		}
		case "GET_FILTER_SYSTEM_FUNCTION":
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
