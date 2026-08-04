import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { ViaExportSoftwareApi } from "../../../Business/Report/ViaExportSoftwareBusiness";
import { ViaExportQuery, QueryResultDtoOfViaExport, GET_GRID_VIA_EXPORT_SOFTWARE, GET_FILTER_VIA_EXPORT_SOFTWARE, ViaExportSoftwareGrid } from "../../../Model/ViaExport/ViaExport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetViaExportSoftwareGrid(queryFilter?: ViaExportQuery) {
	setLoader("ADD", "GetViaExportSoftwareGrid");

	let result: QueryResultDtoOfViaExport | null | undefined;
	let api = new ViaExportSoftwareApi();

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfViaExport>>(() =>
			api.viaExportSoftwareGetViaExport(
				queryFilter ?? {}
			)
		);

		let rtn = { ViaExportSoftwareGridResult: result, filter: null } as ViaExportSoftwareGrid;
		rootStore.dispatch({ type: GET_GRID_VIA_EXPORT_SOFTWARE, payload: rtn });
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
		var rtn = { ViaExportSoftwareGridResult: null, filter: null } as ViaExportSoftwareGrid;
		rootStore.dispatch({ type: GET_GRID_VIA_EXPORT_SOFTWARE, payload: rtn });
	}
	setLoader("REMOVE", "GetViaExportSoftwareGrid");
}

export async function GetFilterColumViaExportSoftware(columName: string, columValue: string, queryFilter?: ViaExportQuery) {
	// setLoader("ADD", "GetFilterColumViaExportSoftware");

	let api = new ViaExportSoftwareApi();
	let result: FilterValueDto[];
	result = await api.viaExportSoftwareGetFilterResult(
		queryFilter ?? {}, columName,
		columValue,

	);
	let rtn = { filter: result, ViaExportSoftwareGridResult: null } as ViaExportSoftwareGrid;
	rootStore.dispatch({ type: GET_FILTER_VIA_EXPORT_SOFTWARE, payload: rtn });
	// setLoader("REMOVE", "GetFilterColumViaExportSoftware");

	return rtn;
}
