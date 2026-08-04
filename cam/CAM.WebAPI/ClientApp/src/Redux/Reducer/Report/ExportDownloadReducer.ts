import { ExportDownload,DOWNLOAD_REPORT } from "../../../Model/Report/Export";

const initState: ExportDownload = {  
    file:null
}
//const dispatch = useDispatch();


export const ExportDownloadReducer = (state = initState, action: { type: string, payload: ExportDownload }) => {
    switch (action.type) {
        case DOWNLOAD_REPORT:
            {
                return { ...state, file: action.payload.file }
            }
            default:
                return state;
    }
}