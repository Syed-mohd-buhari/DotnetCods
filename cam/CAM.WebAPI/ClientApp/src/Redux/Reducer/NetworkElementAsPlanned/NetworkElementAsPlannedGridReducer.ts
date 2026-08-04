import { GET_GRID_NETWORK_ELEMENT_AS_PLANNED, GET_FILTER_NETWORK_ELEMENT_AS_PLANNED, NetworkElementAsPlannedGrid } from "../../../Model/NetworkElementAsPlanned";

const initState: NetworkElementAsPlannedGrid = {
	NetworkElementAsPlannedGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsPlannedGridReducer = (state = initState, action: { type: string; payload: NetworkElementAsPlannedGrid }) => {
	switch (action.type) {
		case GET_GRID_NETWORK_ELEMENT_AS_PLANNED: {
			return { ...state, NetworkElementAsPlannedGridResult: action.payload.NetworkElementAsPlannedGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_PLANNED:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
