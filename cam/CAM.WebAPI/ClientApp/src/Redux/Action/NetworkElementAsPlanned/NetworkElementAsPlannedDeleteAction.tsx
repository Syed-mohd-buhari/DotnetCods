import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_NETWORK_ELEMENT_AS_PLANNED, RESTORE_NETWORK_ELEMENT_AS_PLANNED } from "../../../Model/NetworkElementAsPlanned";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNetworkElementAsPlanned(id: number) {
	setLoader("ADD", "deleteNetworkElementAsPlanned");
	let api = new NetworkElementAsPlannedApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsPlannedDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NETWORK_ELEMENT_AS_PLANNED, payload: rtn });
	setLoader("REMOVE", "deleteNetworkElementAsPlanned");
	return rtn;
}

export async function DeleteDeepNetworkElementAsPlanned(id: number, onlyPlannedActivities: boolean) {
	let api = new NetworkElementAsPlannedApi();
	setLoader("ADD", "DeleteDeepNetworkElementAsPlanned");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsPlannedDeleteDeep(id, onlyPlannedActivities));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NETWORK_ELEMENT_AS_PLANNED, payload: rtn });
	setLoader("REMOVE", "DeleteDeepNetworkElementAsPlanned");
	return rtn;
}

export async function GetRelatedRecordsNetworkElementAsPlanned(id: number) {
	let api = new NetworkElementAsPlannedApi();
	setLoader("ADD", "GetRelatedRecordsNetworkElementAsPlanned");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsPlannedGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNetworkElementAsPlanned");
	return rtn;
}

export async function RestoreNetworkElementAsPlanned(id: number) {
	setLoader("ADD", "RestoreNetworkElementAsPlanned");
	let api = new NetworkElementAsPlannedApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsPlannedRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_NETWORK_ELEMENT_AS_PLANNED, payload: rtn });

	setLoader("REMOVE", "RestoreNetworkElementAsPlanned");
	return rtn;
}
