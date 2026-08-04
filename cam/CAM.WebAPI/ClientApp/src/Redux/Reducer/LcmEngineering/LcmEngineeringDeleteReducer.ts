import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_LCM_ENGINEERING, RESTORE_LCM_ENGINEERING } from "../../../Model/LcmEngineering"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const LcmEngineeringDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_LCM_ENGINEERING:
        case RESTORE_LCM_ENGINEERING:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
