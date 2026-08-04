import {CREATE_DESIGN_COMPONENT_FAMILY, GET_CREATE_DESIGN_COMPONENT_FAMILY, DesignComponentFamilyCreate } from "../../../Model/DesignComponentFamily"

const initState: DesignComponentFamilyCreate = {
    ResultDtoCreate: null,
    DesignComponentFamilyDtoCreate: null,
}
//const dispatch = useDispatch();


export const DesignComponentFamilyCreateReducer = (state = initState, action: { type: string, payload: DesignComponentFamilyCreate }) => {
    switch (action.type) {
        case CREATE_DESIGN_COMPONENT_FAMILY:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_DESIGN_COMPONENT_FAMILY:
            return { ...state, DesignComponentFamilyDtoCreate: action.payload.DesignComponentFamilyDtoCreate }
        default:
            return state;
    }
}
