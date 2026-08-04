import {CREATE_MAJOR_SOFTWARE_BUILD, GET_CREATE_MAJOR_SOFTWARE_BUILD, MajorSoftwareBuildCreate } from "../../../Model/MajorSoftwareBuild"

const initState: MajorSoftwareBuildCreate = {
    ResultDtoCreate: null,
    MajorSoftwareBuildDtoCreate: null,
}
//const dispatch = useDispatch();


export const MajorSoftwareBuildCreateReducer = (state = initState, action: { type: string, payload: MajorSoftwareBuildCreate }) => {
    switch (action.type) {
        case CREATE_MAJOR_SOFTWARE_BUILD:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_MAJOR_SOFTWARE_BUILD:
            return { ...state, MajorSoftwareBuildDtoCreate: action.payload.MajorSoftwareBuildDtoCreate }
        default:
            return state;
    }
}
