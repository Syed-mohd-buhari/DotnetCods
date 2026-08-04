import {  LookUpCreateRule } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpCreateRule = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const BudgetAvailabilityCreateReducer = (state = initState, action: { type: string, payload: LookUpCreateRule }) => {
    switch (action.type) {
        case "CREATE_BUDGET_AVAILABILITY":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_BUDGET_AVAILABILITY":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
