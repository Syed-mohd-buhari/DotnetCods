import {CREATE_SYSTEM_TYPE, GET_CREATE_SYSTEM_TYPE, SystemTypeCreate } from "../../../Model/SystemTypeModel"

const initState: SystemTypeCreate = {
    ResultDtoCreate: null,
    SystemTypeDtoCreate: null,
}
//const dispatch = useDispatch();


export const SystemTypeUpdateReducer = (state = initState, action: { type: string, payload: SystemTypeCreate }) => {
    switch (action.type) {
        case CREATE_SYSTEM_TYPE:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_SYSTEM_TYPE:
            return { ...state, SystemTypeDtoCreate: action.payload.SystemTypeDtoCreate }
        default:
            return state;
    }
}
