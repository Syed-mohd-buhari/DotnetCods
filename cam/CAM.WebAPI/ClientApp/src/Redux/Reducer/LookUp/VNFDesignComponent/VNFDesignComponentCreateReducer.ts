import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpCreate = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const VNFDesignComponentCreateReducer = (state = initState, action: { type: string, payload: LookUpCreate }) => {
    switch (action.type) {
        case "CREATE_VNF_DESIGN_COMPONENT":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_VNF_DESIGN_COMPONENT":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
