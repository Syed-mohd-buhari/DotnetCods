import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const EndOfSupportContractEditReducer = (state = initState, action: { type: string, payload: LookUpEdit }) => {
    switch (action.type) {
        case "EDIT_END_OF_SUPPORT_CONTRACT":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_END_OF_SUPPORT_CONTRACT":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
