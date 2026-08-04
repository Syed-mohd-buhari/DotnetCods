import {EDIT_MAJOR_HARDWARE_BUILD, GET_EDIT_MAJOR_HARDWARE_BUILD, MajorHardwareBuildEdit } from "../../../Model/MajorHardwareBuild"

const initState: MajorHardwareBuildEdit = {
    MajorHardwareBuildDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const MajorHardwareBuildEditReducer = (state = initState, action: { type: string, payload: MajorHardwareBuildEdit }) => {
    switch (action.type) {
        case EDIT_MAJOR_HARDWARE_BUILD:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_MAJOR_HARDWARE_BUILD:
            return { ...state, MajorHardwareBuildDtoEdit: action.payload.MajorHardwareBuildDtoEdit }
        default:
            return state;
    }
}
