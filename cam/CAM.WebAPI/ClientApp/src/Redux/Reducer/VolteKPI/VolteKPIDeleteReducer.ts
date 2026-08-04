import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_VOLTE_KPI, RESTORE_VOLTE_KPI } from "../../../Model/VolteKpi/VolteKPI";

const initState: ResultDto = {
	data: undefined,
	info: undefined,
	warning: undefined,
};
//const dispatch = useDispatch();

export const VolteKPIDeleteReducer = (state = initState, action: { type: string; payload: ResultDto }) => {
	switch (action.type) {
		case DELETE_VOLTE_KPI:
		case RESTORE_VOLTE_KPI: {
			return { ...state, ResultDto: action.payload };
		}
		default:
			return state;
	}
};
