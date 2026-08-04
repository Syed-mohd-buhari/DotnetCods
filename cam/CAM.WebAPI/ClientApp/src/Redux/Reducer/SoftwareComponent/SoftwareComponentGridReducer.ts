import { GET_GRID_SOFTWARE_COMPONENT, GET_FILTER_NETWORK_ELEMENT_AS_IS,SoftwareComponentGrid } from "../../../Model/SoftwareComponent";



const initState: SoftwareComponentGrid = {
	SoftwareComponentGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();


//Added newly for New Network Element

export const SoftwareComponentGridReducer = (state = initState, action: { type: string; payload: SoftwareComponentGrid }) => {
	//console.log("Tems Reducer 2");
	switch (action.type) {
		case GET_GRID_SOFTWARE_COMPONENT: {
			return { ...state, SoftwareComponentGridResult: action.payload.SoftwareComponentGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};