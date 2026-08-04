import { EDIT_NETWORK_ELEMENT_AS_PLANNED, GET_EDIT_NETWORK_ELEMENT_AS_PLANNED, NetworkElementAsPlannedEdit } from "../../../Model/NetworkElementAsPlanned";

const initState: NetworkElementAsPlannedEdit = {
	NetworkElementAsPlannedDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsPlannedEditReducer = (state = initState, action: { type: string; payload: NetworkElementAsPlannedEdit }) => {
	switch (action.type) {
		case EDIT_NETWORK_ELEMENT_AS_PLANNED: {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case GET_EDIT_NETWORK_ELEMENT_AS_PLANNED:
			return { ...state, NetworkElementAsPlannedDtoEdit: action.payload.NetworkElementAsPlannedDtoEdit };
		default:
			return state;
	}
};
