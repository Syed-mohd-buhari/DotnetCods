import {CREATE_BUNDLE_UPGRADE_INIZIATIVE, GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE, BundleUpgradeInitiativeCreate } from "../../../Model/BundleUpgradeIniziative"

const initState: BundleUpgradeInitiativeCreate = {
    ResultDtoCreate: null,
    BundleUpgradeInitiativeDtoCreate: null,
}
//const dispatch = useDispatch();


export const BundleUpgradeInitiativeCreateReducer = (state = initState, action: { type: string, payload: BundleUpgradeInitiativeCreate }) => {
    switch (action.type) {
        case CREATE_BUNDLE_UPGRADE_INIZIATIVE:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE:
            return { ...state, BundleUpgradeInitiativeDtoCreate: action.payload.BundleUpgradeInitiativeDtoCreate }
        default:
            return state;
    }
}
