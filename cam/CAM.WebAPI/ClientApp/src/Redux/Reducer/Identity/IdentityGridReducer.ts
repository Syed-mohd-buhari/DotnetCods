import { GET_GRID_IDENTITY, GET_FILTER_NETWORK_ELEMENT_AS_IS,IdentityGrid } from "../../../Model/Identity";



const initState: IdentityGrid = {
	IdentityGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();


//Added newly for New Network Element

export const IdentityGridReducer = (state = initState, action: { type: string; payload: IdentityGrid }) => {
	//console.log("Tems Reducer 2");
	switch (action.type) {
		case GET_GRID_IDENTITY: {
			return { ...state, IdentityGridResult: action.payload.IdentityGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};