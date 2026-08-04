import {  GET_FILTER_MAJOR_SOFTWARE_BUILD, GET_GRID_MAJOR_SOFTWARE_BUILD, MajorSoftwareBuildGrid } from "../../../Model/MajorSoftwareBuild"

const initState: MajorSoftwareBuildGrid = {
    MajorSoftwareBuildGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const MajorSoftwareBuildGridReducer = (state = initState, action: { type: string, payload: MajorSoftwareBuildGrid }) => {
    switch (action.type) {
        case GET_GRID_MAJOR_SOFTWARE_BUILD:
            {
                return { ...state, MajorSoftwareBuildGridResult: action.payload.MajorSoftwareBuildGridResult }
            }
        case GET_FILTER_MAJOR_SOFTWARE_BUILD:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
