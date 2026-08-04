import {GET_SYSTEM_TYPE_CONSTRAINT_INFO,GET_SYSTEM_TYPE_NAME, SystemTypeCommonInfo } from "../../../Model/SystemTypeModel"

const initState: SystemTypeCommonInfo = {
   ConstraintInfo: null,
   SystemSolution:""
}
//const dispatch = useDispatch();


export const SystemTypeCreateReducer = (state = initState, action: { type: string, payload: SystemTypeCommonInfo }) => {
    switch (action.type) {
        case GET_SYSTEM_TYPE_NAME:
            {
                return { ...state, SystemSolution: action.payload }
            }
        case GET_SYSTEM_TYPE_CONSTRAINT_INFO:
            return { ...state, ConstraintInfo: action.payload }
        default:
            return state;
    }
}
