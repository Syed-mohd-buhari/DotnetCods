import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_NETWORK_ELEMENT_AS_IS, RESTORE_NETWORK_ELEMENT_AS_IS } from "../../../Model/NetworkElementAsIs";

const initState: ResultDto = {
	data: undefined,
	info: undefined,
	warning: undefined,
};
//const dispatch = useDispatch();

export const NetworkElementAsIsDeleteReducer = (state = initState, action: { type: string; payload: ResultDto }) => {
	switch (action.type) {
		case DELETE_NETWORK_ELEMENT_AS_IS:
		case RESTORE_NETWORK_ELEMENT_AS_IS: {
			return { ...state, ResultDto: action.payload };
		}
		default:
			return state;
	}
};
