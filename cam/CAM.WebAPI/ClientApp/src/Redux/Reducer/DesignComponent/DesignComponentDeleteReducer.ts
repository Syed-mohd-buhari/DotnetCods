import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_DESIGN_COMPONENT, RESTORE_DESIGN_COMPONENT } from "../../../Model/DesignComponent"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const DesignComponentDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_DESIGN_COMPONENT:
        case RESTORE_DESIGN_COMPONENT:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
