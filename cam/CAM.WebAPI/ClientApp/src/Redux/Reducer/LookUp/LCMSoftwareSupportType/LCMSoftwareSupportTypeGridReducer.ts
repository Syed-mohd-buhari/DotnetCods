import { LCMSoftwareSupportTypGrid } from "../../../../Model/LookUp/LCMSoftwareSupportType"
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel"
const initState: LCMSoftwareSupportTypGrid = {
    GridResult: null,
    GridResultAll: null,
    filter: null,
}
//const dispatch = useDispatch();
//const dispatch = useDispatch();


export const LCMSoftwareSupportTypeGridReducer = (state = initState, action: { type: string, payload: LCMSoftwareSupportTypGrid }) => {
    switch (action.type) {
        case "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE":
            {
                return { ...state,GridResult : action.payload.GridResult }
            }
        case "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE_ALL":
            {
                return { ...state, GridResultAll: action.payload.GridResultAll }
            }
        case "GET_FILTER_LCM_SOFTWARE_SUPPORT_TYPE":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
