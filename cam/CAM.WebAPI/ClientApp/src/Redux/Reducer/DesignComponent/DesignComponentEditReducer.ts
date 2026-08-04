import {EDIT_DESIGN_COMPONENT, GET_EDIT_DESIGN_COMPONENT, DesignComponentEdit } from "../../../Model/DesignComponent"

const initState: DesignComponentEdit = {
    DesignComponentDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const DesignComponentEditReducer = (state = initState, action: { type: string, payload: DesignComponentEdit }) => {
    switch (action.type) {
        case EDIT_DESIGN_COMPONENT:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_DESIGN_COMPONENT:
            return { ...state, DesignComponentDtoEdit: action.payload.DesignComponentDtoEdit }
        default:
            return state;
    }
}
