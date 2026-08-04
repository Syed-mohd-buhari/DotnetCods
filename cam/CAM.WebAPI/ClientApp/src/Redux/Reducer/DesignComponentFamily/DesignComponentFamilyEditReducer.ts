import {EDIT_DESIGN_COMPONENT_FAMILY, GET_EDIT_DESIGN_COMPONENT_FAMILY, DesignComponentFamilyEdit } from "../../../Model/DesignComponentFamily"

const initState: DesignComponentFamilyEdit = {
    DesignComponentFamilyDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const DesignComponentFamilyEditReducer = (state = initState, action: { type: string, payload: DesignComponentFamilyEdit }) => {
    switch (action.type) {
        case EDIT_DESIGN_COMPONENT_FAMILY:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_DESIGN_COMPONENT_FAMILY:
            return { ...state, DesignComponentFamilyDtoEdit: action.payload.DesignComponentFamilyDtoEdit }
        default:
            return state;
    }
}
