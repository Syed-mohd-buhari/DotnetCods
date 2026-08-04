import { LCMSoftwareSupportTypOperational } from "../../../../Model/LookUp/LCMSoftwareSupportType"
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LCMSoftwareSupportTypOperational = {
    ResultDtoCreate: null,
    Create: null,
}
//const dispatch = useDispatch();


export const LCMSoftwareSupportTypeCreateReducer = (state = initState, action: { type: string, payload: LCMSoftwareSupportTypOperational }) => {
    switch (action.type) {
        case "CREATE_LCM_SOFTWARE_SUPPORT_TYPE":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_LCM_SOFTWARE_SUPPORT_TYPE":
            return { ...state, Create: action.payload.Create }
        default:
            return state;
    }
}
