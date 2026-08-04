import { LocationGrid } from "../../../../Model/LookUp/Location";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LocationGrid = {
    LookUpGridResult: null,
    LookUpGridResultAll: null,
    filter: null,
}


export const LocationGridReducer = (state = initState, action: { type: string; payload: LocationGrid }) => {
	switch (action.type) {
		case "GET_GRID_LOCATION": {
			return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
		}
		case "GET_GRID_LOCATION_ALL": {
			return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
		}
		case "GET_FILTER_LOCATION":
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
