import {  LookUpEditRule } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEditRule = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const BudgetAvailabilityEditReducer = (state = initState, action: { type: string, payload: LookUpEditRule }) => {
    switch (action.type) {
        case "EDIT_BUDGET_AVAILABILITY":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_BUDGET_AVAILABILITY":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
