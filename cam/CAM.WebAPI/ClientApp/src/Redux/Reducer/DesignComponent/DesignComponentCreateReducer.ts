import {CREATE_DESIGN_COMPONENT, GET_CREATE_DESIGN_COMPONENT, DesignComponentCreate } from "../../../Model/DesignComponent"

const initState: DesignComponentCreate = {
    ResultDtoCreate: null,
    DesignComponentDtoCreate: null,
}
//const dispatch = useDispatch();


export const DesignComponentCreateReducer = (state = initState, action: { type: string, payload: DesignComponentCreate }) => {
    switch (action.type) {
        case CREATE_DESIGN_COMPONENT:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_DESIGN_COMPONENT:
            return { ...state, DesignComponentDtoCreate: action.payload.DesignComponentDtoCreate }
        default:
            return state;
    }
}
