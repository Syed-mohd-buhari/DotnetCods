import {EDIT_SYSTEM_TYPE, GET_EDIT_SYSTEM_TYPE, SystemTypeEdit } from "../../../Model/SystemTypeModel"

const initState: SystemTypeEdit = {
    SystemTypeDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const SystemTypeEditReducer = (state = initState, action: { type: string, payload: SystemTypeEdit }) => {
    switch (action.type) {
        case EDIT_SYSTEM_TYPE:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_SYSTEM_TYPE:
            return { ...state, SystemTypeDtoEdit: action.payload.SystemTypeDtoEdit }
        default:
            return state;
    }
}
