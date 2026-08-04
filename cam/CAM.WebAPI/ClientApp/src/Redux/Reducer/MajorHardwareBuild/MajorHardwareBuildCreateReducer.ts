import {CREATE_MAJOR_HARDWARE_BUILD, GET_CREATE_MAJOR_HARDWARE_BUILD, MajorHardwareBuildCreate } from "../../../Model/MajorHardwareBuild"

const initState: MajorHardwareBuildCreate = {
    ResultDtoCreate: null,
    MajorHardwareBuildDtoCreate: null,
}
//const dispatch = useDispatch();


export const MajorHardwareBuildCreateReducer = (state = initState, action: { type: string, payload: MajorHardwareBuildCreate }) => {
    switch (action.type) {
        case CREATE_MAJOR_HARDWARE_BUILD:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_MAJOR_HARDWARE_BUILD:
            return { ...state, MajorHardwareBuildDtoCreate: action.payload.MajorHardwareBuildDtoCreate }
        default:
            return state;
    }
}
