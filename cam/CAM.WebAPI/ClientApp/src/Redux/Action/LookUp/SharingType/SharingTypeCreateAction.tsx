import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharingTypeApi } from "../../../../Business/LookUp/SharingTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSharingTypeCreateResource() {
	setLoader("ADD", "GetSharingTypeCreateResource");

	let api = new SharingTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.sharingTypeGetCreateResourceSharingType());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "GetSharingTypeCreateResource");
}

export async function CreatSharingType(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSharingType");
	let api = new SharingTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.sharingTypeCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatSharingType");
	return rtn;
}
