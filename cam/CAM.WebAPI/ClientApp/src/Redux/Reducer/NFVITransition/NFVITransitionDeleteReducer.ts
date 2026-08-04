import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_NFVI_TRAMSITION, RESTORE_NFVI_TRAMSITION } from "../../../Model/NFVITransition"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const NFVITransitionDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_NFVI_TRAMSITION:
            {
                return { ...state, ResultDto: action.payload }
            }
        case RESTORE_NFVI_TRAMSITION:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
