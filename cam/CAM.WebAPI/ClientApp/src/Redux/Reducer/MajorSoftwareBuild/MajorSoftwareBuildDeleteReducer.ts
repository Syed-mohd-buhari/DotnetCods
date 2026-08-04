import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_MAJOR_SOFTWARE_BUILD, RESTORE_MAJOR_SOFTWARE_BUILD } from "../../../Model/MajorSoftwareBuild"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const MajorSoftwareBuildDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_MAJOR_SOFTWARE_BUILD:
            {
                return { ...state, ResultDto: action.payload }
            }
        case RESTORE_MAJOR_SOFTWARE_BUILD:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
