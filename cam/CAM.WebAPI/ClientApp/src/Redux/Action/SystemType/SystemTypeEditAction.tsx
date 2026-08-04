import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { EDIT_SYSTEM_TYPE, GET_EDIT_SYSTEM_TYPE, SystemTypeDtoUpdate, SystemTypeEdit } from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSystemTypeEditResource(id: number) {
	setLoader("ADD", "GetSystemTypeEditResource");

	let api = new SystemTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<SystemTypeDtoUpdate>>(() => api.systemTypeGetUpdateResourceSystemType(id));
	let rtn = { SystemTypeDtoEdit: createResource } as SystemTypeEdit;
	rootStore.dispatch({ type: GET_EDIT_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "GetSystemTypeEditResource");

	return rtn;
}

export async function EditSystemType(data: SystemTypeDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditSystemType");
	let api = new SystemTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypePut(data, forced));
	let rtn = { ResultDtoEdit: result } as SystemTypeEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "EditSystemType");
	return rtn;
}
