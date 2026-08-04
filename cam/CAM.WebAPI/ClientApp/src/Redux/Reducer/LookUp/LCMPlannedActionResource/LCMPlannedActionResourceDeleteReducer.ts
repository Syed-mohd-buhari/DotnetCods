import { ResultDto } from "../../../../Model/CommonModels"


const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const LCMPlannedActionResourceDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case "DELETE_LCM_PLANNED_ACTION_RESOURCE":
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
