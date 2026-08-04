import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpCreate = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const FullOrPartialResourceCreateReducer = (state = initState, action: { type: string, payload: LookUpCreate }) => {
    switch (action.type) {
        case "CREATE_F_O_P_R":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_F_O_P_R":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
