import { ResultDto } from "../../../../Model/CommonModels";

const initState: ResultDto = {
	data: undefined,
	info: undefined,
	warning: undefined,
};
//const dispatch = useDispatch();

export const SystemFunctionDeleteReducer = (state = initState, action: { type: string; payload: ResultDto }) => {
	switch (action.type) {
		case "DELETE_SYSTEM_FUNCTION": {
			return { ...state, ResultDto: action.payload };
		}
		default:
			return state;
	}
};
