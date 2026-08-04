import { GET_GRID_HARDWARE_CONFIGURATION, GET_FILTER_NETWORK_ELEMENT_AS_IS,HardwareConfigurationGrid } from "../../../Model/HardwareConfiguration";



const initState: HardwareConfigurationGrid = {
	HardwareConfigurationGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();


//Added newly for New Network Element

export const HardwareConfigurationGridReducer = (state = initState, action: { type: string; payload: HardwareConfigurationGrid }) => {
	//console.log("Tems Reducer 2");
	switch (action.type) {
		case GET_GRID_HARDWARE_CONFIGURATION: {
			return { ...state, HardwareConfigurationGridResult: action.payload.HardwareConfigurationGridResult };
		}
		case GET_FILTER_NETWORK_ELEMENT_AS_IS:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};