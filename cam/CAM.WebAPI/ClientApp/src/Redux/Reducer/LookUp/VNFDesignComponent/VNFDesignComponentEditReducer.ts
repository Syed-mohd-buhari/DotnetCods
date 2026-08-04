import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const VNFDesignComponentEditReducer = (state = initState, action: { type: string, payload: LookUpEdit }) => {
    switch (action.type) {
        case "EDIT_VNF_DESIGN_COMPONENT":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_VNF_DESIGN_COMPONENT":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
