import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_VNF_TRANSITION, RESTORE_VNF_TRANSITION } from "../../../Model/VNFTransition"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const VNFTransitionDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_VNF_TRANSITION:
        case RESTORE_VNF_TRANSITION:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
