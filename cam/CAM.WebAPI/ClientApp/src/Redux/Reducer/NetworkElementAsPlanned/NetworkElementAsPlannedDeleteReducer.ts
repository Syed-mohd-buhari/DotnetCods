import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_NETWORK_ELEMENT_AS_PLANNED, RESTORE_NETWORK_ELEMENT_AS_PLANNED } from "../../../Model/NetworkElementAsPlanned";

const initState: ResultDto = {
	data: undefined,
	info: undefined,
	warning: undefined,
};
//const dispatch = useDispatch();

export const NetworkElementAsPlannedDeleteReducer = (state = initState, action: { type: string; payload: ResultDto }) => {
	switch (action.type) {
		case DELETE_NETWORK_ELEMENT_AS_PLANNED:
		case RESTORE_NETWORK_ELEMENT_AS_PLANNED: {
			return { ...state, ResultDto: action.payload };
		}
		default:
			return state;
	}
};
