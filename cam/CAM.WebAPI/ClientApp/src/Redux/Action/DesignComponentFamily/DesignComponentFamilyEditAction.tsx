import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentFamilyApi } from "../../../Business/DesignComponentFamilyBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DesignComponentFamilyDtoUpdate, DesignComponentFamilyEdit, EDIT_DESIGN_COMPONENT_FAMILY, GET_EDIT_DESIGN_COMPONENT_FAMILY } from "../../../Model/DesignComponentFamily";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignComponentFamilyEditResource(id: number) {
	setLoader("ADD", "GetDesignComponentFamilyEditResource");

	// const dispach = useDispatch();
	let api = new DesignComponentFamilyApi();
	let createResource = await ApiCallWithErrorHandling<Promise<DesignComponentFamilyDtoUpdate>>(() => api.designComponentFamilyGetUpdateResourceDesignComponentFamily(id));
	let rtn = { DesignComponentFamilyDtoEdit: createResource } as DesignComponentFamilyEdit;
	rootStore.dispatch({ type: GET_EDIT_DESIGN_COMPONENT_FAMILY, payload: rtn });
	setLoader("REMOVE", "GetDesignComponentFamilyEditResource");

	return rtn;
}

export async function EditDesignComponentFamily(data: DesignComponentFamilyDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditDesignComponentFamily");
	let api = new DesignComponentFamilyApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentFamilyPut(data, forced));
	let rtn = { ResultDtoEdit: result } as DesignComponentFamilyEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_DESIGN_COMPONENT_FAMILY, payload: rtn });
	setLoader("REMOVE", "EditDesignComponentFamily");
	return rtn;
}
