import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_THIRD_PARTY_HARDWARE_COMPONENT, RESTORE_THIRD_PARTY_HARDWARE_COMPONENT } from "../../../Model/ThirdPartyHardwareComponent"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const ThirdPartyHardwareComponentDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_THIRD_PARTY_HARDWARE_COMPONENT:
        case RESTORE_THIRD_PARTY_HARDWARE_COMPONENT:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
