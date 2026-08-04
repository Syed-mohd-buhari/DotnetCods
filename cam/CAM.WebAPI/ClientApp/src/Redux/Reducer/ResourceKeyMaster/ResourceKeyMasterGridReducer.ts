import { GET_GRID_RESOURCE_KEY_MASTER, GET_FILTER_NETWORK_ELEMENT_AS_IS,ResourceKeyMasterGrid } from "../../../Model/ResourceKeyMaster";



const initState: ResourceKeyMasterGrid = {
	ResourceKeyMasterGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();


//Added newly for New Network Element

export const ResourceKeyMasterGridReducer = (state = initState, action: { type: string; payload: ResourceKeyMasterGrid }) => {
	//console.log("Tems Reducer 2");
	switch (action.type) {
		case GET_GRID_RESOURCE_KEY_MASTER: {
			return { ...state, ResourceKeyMasterGridResult: action.payload.ResourceKeyMasterGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};