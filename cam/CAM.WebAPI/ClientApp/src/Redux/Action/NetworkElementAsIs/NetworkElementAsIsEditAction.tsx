import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { EDIT_NETWORK_ELEMENT_AS_IS, GET_EDIT_NETWORK_ELEMENT_AS_IS, NetworkElementAsIsDtoUpdate, NetworkElementAsIsEdit } from "../../../Model/NetworkElementAsIs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNetworkElementAsIsEditResource(id: number) {
	setLoader("ADD", "GetNetworkElementAsIsEditResource");
	let api = new NetworkElementAsIsApi();
	let createResource = await ApiCallWithErrorHandling<Promise<NetworkElementAsIsDtoUpdate>>(() => api.networkElementAsIsGetUpdateResourceNetworkElementAsIs(id));
	let rtn = { NetworkElementAsIsDtoEdit: createResource } as NetworkElementAsIsEdit;
	rootStore.dispatch({ type: GET_EDIT_NETWORK_ELEMENT_AS_IS, payload: rtn });
	setLoader("REMOVE", "GetNetworkElementAsIsEditResource");
}

export async function EditNetworkElementAsIs(data: NetworkElementAsIsDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditNetworkElementAsIs");
	let api = new NetworkElementAsIsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkElementAsIsPut(data, forced));
	let rtn = { ResultDtoEdit: result } as NetworkElementAsIsEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_NETWORK_ELEMENT_AS_IS, payload: rtn });
	setLoader("REMOVE", "EditNetworkElementAsIs");
	return rtn;
}
