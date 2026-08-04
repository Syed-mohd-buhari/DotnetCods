import { GET_GRID_VIA_EXPORT_HARDWARE, GET_FILTER_VIA_EXPORT_HARDWARE, ViaExportHardwareGrid } from "../../../Model/ViaExport/ViaExport"

const initState: ViaExportHardwareGrid = {
    ViaExportHardwareGridResult: null,
    filter: null,
}

export const ViaExportHardwareGridReducer = (state = initState, action: { type: string, payload: ViaExportHardwareGrid }) => {
    switch (action.type) {
        case GET_GRID_VIA_EXPORT_HARDWARE:
            {
                return { ...state, ViaExportHardwareGridResult: action.payload.ViaExportHardwareGridResult }
            }
        case GET_FILTER_VIA_EXPORT_HARDWARE:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
