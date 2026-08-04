import { EDIT_NETWORK_ELEMENT_AS_IS, GET_EDIT_NETWORK_ELEMENT_AS_IS, NetworkElementAsIsEdit } from "../../../Model/NetworkElementAsIs";

const initState: NetworkElementAsIsEdit = {
	NetworkElementAsIsDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsIsEditReducer = (state = initState, action: { type: string; payload: NetworkElementAsIsEdit }) => {
	switch (action.type) {
		case EDIT_NETWORK_ELEMENT_AS_IS: {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case GET_EDIT_NETWORK_ELEMENT_AS_IS:
			return { ...state, NetworkElementAsIsDtoEdit: action.payload.NetworkElementAsIsDtoEdit };
		default:
			return state;
	}
};
