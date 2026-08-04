import {EDIT_BUNDLE_UPGRADE_INIZIATIVE, GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE, BundleUpgradeInitiativeEdit } from "../../../Model/BundleUpgradeIniziative"

const initState: BundleUpgradeInitiativeEdit = {
    BundleUpgradeInitiativeDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const BundleUpgradeInitiativeEditReducer = (state = initState, action: { type: string, payload: BundleUpgradeInitiativeEdit }) => {
    switch (action.type) {
        case EDIT_BUNDLE_UPGRADE_INIZIATIVE:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE:
            return { ...state, BundleUpgradeInitiativeDtoEdit: action.payload.BundleUpgradeInitiativeDtoEdit }
        default:
            return state;
    }
}
