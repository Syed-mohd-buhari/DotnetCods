


import { GET_GRID_VIA_EXPORT_SOFTWARE, GET_FILTER_VIA_EXPORT_SOFTWARE, ViaExportSoftwareGrid } from "../../../Model/ViaExport/ViaExport"


const initState: ViaExportSoftwareGrid = {
    ViaExportSoftwareGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const ViaExportSoftwareGridReducer = (state = initState, action: { type: string, payload: ViaExportSoftwareGrid }) => {
    switch (action.type) {
        case GET_GRID_VIA_EXPORT_SOFTWARE:
            {
                return { ...state, ViaExportSoftwareGridResult: action.payload.ViaExportSoftwareGridResult }
            }
        case GET_FILTER_VIA_EXPORT_SOFTWARE:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
