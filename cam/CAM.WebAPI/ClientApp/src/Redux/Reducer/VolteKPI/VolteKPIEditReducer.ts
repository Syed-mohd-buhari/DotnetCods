import { EDIT_VOLTE_KPI, GET_EDIT_VOLTE_KPI, VolteKPIEdit } from "../../../Model/VolteKpi/VolteKPI";

const initState: VolteKPIEdit = {
	VolteKPIDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VolteKPIEditReducer = (state = initState, action: { type: string; payload: VolteKPIEdit }) => {
	switch (action.type) {
		case EDIT_VOLTE_KPI: {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case GET_EDIT_VOLTE_KPI:
			return { ...state, VolteKPIDtoEdit: action.payload.VolteKPIDtoEdit };
		default:
			return state;
	}
};
