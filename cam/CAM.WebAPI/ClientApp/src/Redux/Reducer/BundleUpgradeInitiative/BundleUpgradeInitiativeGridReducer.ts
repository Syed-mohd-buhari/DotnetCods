import {  GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE, GET_GRID_BUNDLE_UPGRADE_INIZIATIVE, BundleUpgradeInitiativeGrid } from "../../../Model/BundleUpgradeIniziative"

const initState: BundleUpgradeInitiativeGrid = {
    BundleUpgradeInitiativeGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const BundleUpgradeInitiativeGridReducer = (state = initState, action: { type: string, payload: BundleUpgradeInitiativeGrid }) => {
    switch (action.type) {
        case GET_GRID_BUNDLE_UPGRADE_INIZIATIVE:
            {
                return { ...state, BundleUpgradeInitiativeGridResult: action.payload.BundleUpgradeInitiativeGridResult }
            }
        case GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
