import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_BUNDLE_UPGRADE_INIZIATIVE, RESTORE_BUNDLE_UPGRADE_INIZIATIVE } from "../../../Model/BundleUpgradeIniziative"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const BundleUpgradeInitiativeDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_BUNDLE_UPGRADE_INIZIATIVE:
        case RESTORE_BUNDLE_UPGRADE_INIZIATIVE:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
