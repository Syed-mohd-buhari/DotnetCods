import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpCreate = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const EndOfSupportContractCreateReducer = (state = initState, action: { type: string, payload: LookUpCreate }) => {
    switch (action.type) {
        case "CREATE_END_OF_SUPPORT_CONTRACT":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_END_OF_SUPPORT_CONTRACT":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
