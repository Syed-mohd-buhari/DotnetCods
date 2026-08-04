import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_NETWORK_ELEMENT_AS_IS, RESTORE_NETWORK_ELEMENT_AS_IS } from "../../../Model/NetworkElementAsIs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNetworkElementAsIs(id: number) {
	setLoader("ADD", "deleteNetworkElementAsIs");
	let api = new NetworkElementAsIsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsIsDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NETWORK_ELEMENT_AS_IS, payload: rtn });
	setLoader("REMOVE", "deleteNetworkElementAsIs");
	return rtn;
}

export async function DeleteDeepNetworkElementAsIs(id: number) {
	let api = new NetworkElementAsIsApi();
	setLoader("ADD", "DeleteDeepNetworkElementAsIs");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsIsDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NETWORK_ELEMENT_AS_IS, payload: rtn });
	setLoader("REMOVE", "DeleteDeepNetworkElementAsIs");
	return rtn;
}

export async function GetRelatedRecordsNetworkElementAsIs(id: number) {
	let api = new NetworkElementAsIsApi();
	setLoader("ADD", "GetRelatedRecordsNetworkElementAsIs");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsIsGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNetworkElementAsIs");
	return rtn;
}

export async function RestoreNetworkElementAsIs(id: number) {
	setLoader("ADD", "RestoreNetworkElementAsIs");
	let api = new NetworkElementAsIsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsIsRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_NETWORK_ELEMENT_AS_IS, payload: rtn });

	setLoader("REMOVE", "RestoreNetworkElementAsIs");
	return rtn;
}
