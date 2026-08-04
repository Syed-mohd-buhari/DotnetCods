import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_DESIGN_COMPONENT_FAMILY, RESTORE_DESIGN_COMPONENT_FAMILY } from "../../../Model/DesignComponentFamily"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const DesignComponentFamilyDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_DESIGN_COMPONENT_FAMILY:
        case RESTORE_DESIGN_COMPONENT_FAMILY:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
