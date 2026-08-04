import {CREATE_THIRD_PARTY_HARDWARE_COMPONENT, GET_CREATE_THIRD_PARTY_HARDWARE_COMPONENT, ThirdPartyHardwareComponentCreate } from "../../../Model/ThirdPartyHardwareComponent"

const initState: ThirdPartyHardwareComponentCreate = {
    ResultDtoCreate: null,
    ThirdPartyHardwareComponentDtoCreate: null,
}
//const dispatch = useDispatch();


export const ThirdPartyHardwareComponentCreateReducer = (state = initState, action: { type: string, payload: ThirdPartyHardwareComponentCreate }) => {
    switch (action.type) {
        case CREATE_THIRD_PARTY_HARDWARE_COMPONENT:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_THIRD_PARTY_HARDWARE_COMPONENT:
            return { ...state, ThirdPartyHardwareComponentDtoCreate: action.payload.ThirdPartyHardwareComponentDtoCreate }
        default:
            return state;
    }
}
