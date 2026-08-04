import { LCMSoftwareSupportTypOperational, LCMSoftwareSupportTypOperationalEdit } from "../../../../Model/LookUp/LCMSoftwareSupportType"
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LCMSoftwareSupportTypOperationalEdit = {
    Create: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const LCMSoftwareSupportTypeEditReducer = (state = initState, action: { type: string, payload: LCMSoftwareSupportTypOperationalEdit }) => {
    switch (action.type) {
        case "EDIT_LCM_SOFTWARE_SUPPORT_TYPE":
            {
                return { ...state, ResultDto: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_LCM_SOFTWARE_SUPPORT_TYPE":
            return { ...state, Create: action.payload.Create }
        default:
            return state;
    }
}
