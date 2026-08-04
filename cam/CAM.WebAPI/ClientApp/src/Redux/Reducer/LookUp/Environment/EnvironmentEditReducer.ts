import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const EnvironmentEditReducer = (state = initState, action: { type: string, payload: LookUpEdit }) => {
    switch (action.type) {
        case "EDIT_ENVIRONMENT":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_ENVIRONMENT":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
