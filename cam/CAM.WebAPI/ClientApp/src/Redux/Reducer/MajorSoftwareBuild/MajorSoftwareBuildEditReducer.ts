import {EDIT_MAJOR_SOFTWARE_BUILD, GET_EDIT_MAJOR_SOFTWARE_BUILD, MajorSoftwareBuildEdit } from "../../../Model/MajorSoftwareBuild"

const initState: MajorSoftwareBuildEdit = {
    MajorSoftwareBuildDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const MajorSoftwareBuildEditReducer = (state = initState, action: { type: string, payload: MajorSoftwareBuildEdit }) => {
    switch (action.type) {
        case EDIT_MAJOR_SOFTWARE_BUILD:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_MAJOR_SOFTWARE_BUILD:
            return { ...state, MajorSoftwareBuildDtoEdit: action.payload.MajorSoftwareBuildDtoEdit }
        default:
            return state;
    }
}
