import { ExportDownload } from "../../../Model/Report/Export";
import { DOWNLOAD_VOLTE_KPI_REPORT } from "../../../Model/Report/ReportVolteKPIModel";

const initState: ExportDownload = {
	file: null,
};

export const ExportVolteKPIReducer = (state = initState, action: { type: string; payload: ExportDownload }) => {
	switch (action.type) {
		case DOWNLOAD_VOLTE_KPI_REPORT: {
			return { ...state, file: action.payload.file };
		}
		default:
			return state;
	}
};
