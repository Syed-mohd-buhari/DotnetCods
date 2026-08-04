import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VerticalResponsibleApi } from "../../../../Business/LookUp/VerticalResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVerticalResponsibleCreateResource() {
	setLoader("ADD", "GetVerticalResponsibleCreateResource");

	let api = new VerticalResponsibleApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.verticalResponsibleGetCreateResourceVerticalResponsible());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "GetVerticalResponsibleCreateResource");
}

export async function CreatVerticalResponsible(data: TipologicaGridDto) {
	setLoader("ADD", "CreatVerticalResponsible");
	let api = new VerticalResponsibleApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.verticalResponsibleCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "CreatVerticalResponsible");
	return rtn;
}
