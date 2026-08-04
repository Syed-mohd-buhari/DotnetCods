import { GET_GRID_LCM_ENG_AUDIT, GET_FILTER_LCM_ENG_AUDIT, LcmEngAuditGrid } from "../../../Model/LcmEngAudit"

const initState: LcmEngAuditGrid = {
    LcmEngAuditGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const LcmEngAuditGridReducer = (state = initState, action: { type: string, payload: LcmEngAuditGrid }) => {
    switch (action.type) {
        case GET_GRID_LCM_ENG_AUDIT:
            {
                return { ...state, LcmEngAuditGridResult: action.payload.LcmEngAuditGridResult }
            }
        case GET_FILTER_LCM_ENG_AUDIT:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
