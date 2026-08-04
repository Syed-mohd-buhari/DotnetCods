import { LookUpEdit, LookUpEditRule } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const BuildConstructionEditReducer = (state = initState, action: { type: string, payload: LookUpEditRule }) => {
    switch (action.type) {
        case "EDIT_BUILD_CONSTRUCTION":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_BUILD_CONSTRUCTION":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
