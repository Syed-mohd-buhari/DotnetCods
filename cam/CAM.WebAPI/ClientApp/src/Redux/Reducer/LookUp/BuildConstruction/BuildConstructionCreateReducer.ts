import { LookUpCreate, LookUpCreateRule } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpCreateRule = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const BuildConstructionCreateReducer = (state = initState, action: { type: string, payload: LookUpCreateRule }) => {
    switch (action.type) {
        case "CREATE_BUILD_CONSTRUCTION":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_BUILD_CONSTRUCTION":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
