import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_SYSTEM_TYPE, RESTORE_SYSTEM_TYPE } from "../../../Model/SystemTypeModel"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const SystemTypeDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_SYSTEM_TYPE:
        case RESTORE_SYSTEM_TYPE:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
