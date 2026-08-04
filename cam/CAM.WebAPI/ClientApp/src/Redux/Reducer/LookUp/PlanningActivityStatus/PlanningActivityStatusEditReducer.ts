import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const PlanningActivityStatusEditReducer = (state = initState, action: { type: string, payload: LookUpEdit }) => {
    switch (action.type) {
        case "EDIT_PLANNING_ACTIVITY_STATUS":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_PLANNING_ACTIVITY_STATUS":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
