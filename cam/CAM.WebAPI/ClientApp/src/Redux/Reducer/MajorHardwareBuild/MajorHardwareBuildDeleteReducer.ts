import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_MAJOR_HARDWARE_BUILD, RESTORE_MAJOR_HARDWARE_BUILD } from "../../../Model/MajorHardwareBuild";

const initState: ResultDto = {
	data: undefined,
	info: undefined,
	warning: undefined,
};
//const dispatch = useDispatch();

export const MajorHardwareBuildDeleteReducer = (state = initState, action: { type: string; payload: ResultDto }) => {
	switch (action.type) {
		case DELETE_MAJOR_HARDWARE_BUILD: {
			return { ...state, ResultDto: action.payload };
		}
		case RESTORE_MAJOR_HARDWARE_BUILD: {
			return { ...state, ResultDto: action.payload };
		}
		default:
			return state;
	}
};
