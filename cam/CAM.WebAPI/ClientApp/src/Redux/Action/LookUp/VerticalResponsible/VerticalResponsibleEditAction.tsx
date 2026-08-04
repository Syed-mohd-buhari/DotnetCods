import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VerticalResponsibleApi } from "../../../../Business/LookUp/VerticalResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVerticalResponsibleEditResource(id: number) {
	setLoader("ADD", "GetVerticalResponsibleEditResource");

	let api = new VerticalResponsibleApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.verticalResponsibleGetUpdateResourceVerticalResponsible(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "GetVerticalResponsibleEditResource");

	return rtn;
}

export async function EditVerticalResponsible(data: TipologicaGridDto) {
	setLoader("ADD", "EditVerticalResponsible");
	let api = new VerticalResponsibleApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.verticalResponsiblePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "EditVerticalResponsible");
	return rtn;
}
