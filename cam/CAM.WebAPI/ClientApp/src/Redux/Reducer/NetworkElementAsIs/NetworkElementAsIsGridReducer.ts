import { GET_GRID_NETWORK_ELEMENT_AS_IS, GET_FILTER_NETWORK_ELEMENT_AS_IS, NetworkElementAsIsGrid,NewNetworkElementAsIsGrid } from "../../../Model/NetworkElementAsIs";

const initState: NetworkElementAsIsGrid = {
	NetworkElementAsIsGridResult: null,
	filter: null,
};

const initState2: NewNetworkElementAsIsGrid = {
	NewNetworkElementAsIsGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsIsGridReducer = (state = initState, action: { type: string; payload: NetworkElementAsIsGrid }) => {
	//console.log("Tems Reducer 1");
	switch (action.type) {
		case GET_GRID_NETWORK_ELEMENT_AS_IS: {
			return { ...state, NetworkElementAsIsGridResult: action.payload.NetworkElementAsIsGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};

//Added newly for New Network Element

export const NewNetworkElementAsIsGridReducer = (state = initState2, action: { type: string; payload: NewNetworkElementAsIsGrid }) => {
	//console.log("Tems Reducer 2");
	switch (action.type) {
		case GET_GRID_NETWORK_ELEMENT_AS_IS: {
			return { ...state, NewNetworkElementAsIsGridResult: action.payload.NewNetworkElementAsIsGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};