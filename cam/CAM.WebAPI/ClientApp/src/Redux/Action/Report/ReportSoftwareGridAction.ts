import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { ReportSoftwareApi } from "../../../Business/Report/ReportSoftwareBusiness";
import { ReportSoftwareDtoGrid, ReportSoftwareGrid, ReportSoftwareQueryObjectGrid, QueryResultDtoOfReportSoftwareDtoGrid, GET_GRID_REPORT_SOFTWARE, GET_FILTER_REPORT_SOFTWARE } from "../../../Model/Report/ReportSoftwareModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetReportSoftwareGrid(queryFilter?: ReportSoftwareQueryObjectGrid) {
	setLoader("ADD", "GetReportSoftwareGrid");

	let result: QueryResultDtoOfReportSoftwareDtoGrid | null | undefined;
	let api = new ReportSoftwareApi();

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfReportSoftwareDtoGrid>>(() =>
			api.reportSoftwareGetReport(
				queryFilter ?? {}
			)
		);

		let rtn = { ReportSoftwareGridResult: result, filter: null } as ReportSoftwareGrid;
		rootStore.dispatch({ type: GET_GRID_REPORT_SOFTWARE, payload: rtn });
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
		var rtn = { ReportSoftwareGridResult: null, filter: null } as ReportSoftwareGrid;
		rootStore.dispatch({ type: GET_GRID_REPORT_SOFTWARE, payload: rtn });
	}
	setLoader("REMOVE", "GetReportSoftwareGrid");
}

export async function GetFilterColumReportSoftware(columName: string, columValue: string, queryFilter?: ReportSoftwareQueryObjectGrid) {

	let api = new ReportSoftwareApi();
	let result: FilterValueDto[];
		result = await api.reportSoftwareGetFilterResult(
			queryFilter ?? {}, columName,
			columValue,

		);
	let rtn = { filter: result, ReportSoftwareGridResult: null } as ReportSoftwareGrid;
	rootStore.dispatch({ type: GET_FILTER_REPORT_SOFTWARE, payload: rtn });

	return rtn;
}
