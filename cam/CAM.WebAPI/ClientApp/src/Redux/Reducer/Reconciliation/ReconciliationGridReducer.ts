import { GET_GRID_RECONCILIATION, GET_FILTER_RECONCILIATION, ReconciliationGrid } from "../../../Model/Reconciliation";

const initState: ReconciliationGrid = {
	ReconciliationGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();

export const ReconciliationGridReducer = (state = initState, action: { type: string; payload: ReconciliationGrid }) => {
	switch (action.type) {
		case GET_GRID_RECONCILIATION: {
			return { ...state, ReconciliationGridResult: action.payload.ReconciliationGridResult };
		}
		case GET_FILTER_RECONCILIATION:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
