import { CREATE_VOLTE_KPI, GET_CREATE_VOLTE_KPI, VolteKPICreate } from "../../../Model/VolteKpi/VolteKPI";

const initState: VolteKPICreate = {
	ResultDtoCreate: null,
	VolteKPIDtoCreate: null,
};
//const dispatch = useDispatch();

export const VolteKPICreateReducer = (state = initState, action: { type: string; payload: VolteKPICreate }) => {
	switch (action.type) {
		case CREATE_VOLTE_KPI: {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case GET_CREATE_VOLTE_KPI:
			return { ...state, VolteKPIDtoCreate: action.payload.VolteKPIDtoCreate };
		default:
			return state;
	}
};
