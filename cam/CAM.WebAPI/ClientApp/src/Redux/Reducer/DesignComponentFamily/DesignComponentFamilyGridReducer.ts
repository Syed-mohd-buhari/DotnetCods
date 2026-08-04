import {  GET_FILTER_DESIGN_COMPONENT_FAMILY, GET_GRID_DESIGN_COMPONENT_FAMILY, DesignComponentFamilyGrid } from "../../../Model/DesignComponentFamily"

const initState: DesignComponentFamilyGrid = {
    DesignComponentFamilyGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const DesignComponentFamilyGridReducer = (state = initState, action: { type: string, payload: DesignComponentFamilyGrid }) => {
    switch (action.type) {
        case GET_GRID_DESIGN_COMPONENT_FAMILY:
            {
                return { ...state, DesignComponentFamilyGridResult: action.payload.DesignComponentFamilyGridResult }
            }
        case GET_FILTER_DESIGN_COMPONENT_FAMILY:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
