import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { initState } from "../ActivityStatus/ActivityStatusGridReducer";
// const initState: LookUpGrid = {
//     LookUpGridResult: null,
//     LookUpGridResultAll: null,
//     filter: null,
// }
//const dispatch = useDispatch();
//const dispatch = useDispatch();

export const SecurityTireZoneGridReducer = (state = initState, action: { type: string; payload: LookUpGrid }) => {
	switch (action.type) {
		case "GET_GRID_SECURITY_TIRE_ZONE": {
			return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
		}
		case "GET_GRID_SECURITY_TIRE_ZONE_ALL": {
			return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
		}
		case "GET_FILTER_SECURITY_TIRE_ZONE":
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
