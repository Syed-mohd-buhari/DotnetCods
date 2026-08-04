import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SecurityTireZoneApi } from "../../../../Business/LookUp/SecurityTireZoneBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSecurityTireZoneCreateResource() {
	setLoader("ADD", "GetSecurityTireZoneCreateResource");

	let api = new SecurityTireZoneApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.securityTireZoneGetCreateResourceSecurityTireZone());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "GetSecurityTireZoneCreateResource");
}

export async function CreatSecurityTireZone(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSecurityTireZone");
	let api = new SecurityTireZoneApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.securityTireZoneCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "CreatSecurityTireZone");
	return rtn;
}
