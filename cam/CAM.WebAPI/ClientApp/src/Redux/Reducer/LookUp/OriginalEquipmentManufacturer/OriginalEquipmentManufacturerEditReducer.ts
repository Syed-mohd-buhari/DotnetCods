import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"

const initState: LookUpEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const OriginalEquipmentManufacturerEditReducer = (state = initState, action: { type: string, payload: LookUpEdit }) => {
    switch (action.type) {
        case "EDIT_ORIGINAL_EQUIPMENT_MANUFACTURER":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_ORIGINAL_EQUIPMENT_MANUFACTURER":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
