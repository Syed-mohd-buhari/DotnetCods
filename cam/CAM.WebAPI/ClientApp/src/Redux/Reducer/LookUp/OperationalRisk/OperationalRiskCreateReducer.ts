import { LookUpCreateRisk } from "../../../../Model/LookUp/OperationalRisk"

const initState: LookUpCreateRisk = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const OperationalRiskCreateReducer = (state = initState, action: { type: string, payload: LookUpCreateRisk }) => {
    switch (action.type) {
        case "CREATE_OPERATIONAL_RISK":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_OPERATIONAL_RISK":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
