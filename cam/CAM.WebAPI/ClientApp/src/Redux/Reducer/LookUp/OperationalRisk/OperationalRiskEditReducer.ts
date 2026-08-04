import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"
import { LookUpEditRisk } from "../../../../Model/LookUp/OperationalRisk"

const initState: LookUpEditRisk = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const OperationalRiskEditReducer = (state = initState, action: { type: string, payload: LookUpEditRisk }) => {
    switch (action.type) {
        case "EDIT_OPERATIONAL_RISK":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_OPERATIONAL_RISK":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
